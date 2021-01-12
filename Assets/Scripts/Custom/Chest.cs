using System;
using XxSlitFrame.View;

//StartUsing
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XxSlitFrame.Tools.General;
using Random = UnityEngine.Random;

/// <summary>
/// 抽奖概率
/// </summary>
[Serializable]
public class LuckDrawProbability
{
    public GameObject prize;

    public float probability;

    /*[HideInInspector] */
    public float minNumber;

    /*[HideInInspector] */
    public float maxNumber;
}

//EndUsing
namespace Custom
{
    public class Chest : BaseWindow
    {
//StartVariable
        private List<ChestItem> _chestGroup;
        private GameObject _key;
        private Image _keyIcon;
        private Text _keyNumber;
        private GameObject _freeKey;
        private Toggle _agreement;
        private Button _receiveKey;
        private GameObject _continueGame;

        private Button _continueGameBtn;


//EndVariable
        public List<LuckDrawProbability> luckDrawProbabilities;
        [SerializeField] private List<int> randomShake;
        [SerializeField] private List<int> randomVideo;
        public float shakeSpeed;
        public float shakeAngle;
        public int defaultKeyNumber;
        public int currentKeyNumber;
        private int _freeBtnTimeTask;
        public bool isGetFreeKey;

        public override void Init()
        {
            //随机数
            randomShake = new List<int>();
            randomVideo = new List<int>();
            //兑奖概率计算
            CalculationCashPrizeProbability();
            GetRandomNumber(3, 0, 8, randomShake);
            GetRandomNumber(3, 0, 8, randomVideo);
            for (int i = 0; i < _chestGroup.Count; i++)
            {
                _chestGroup[i].Init();
                if (randomShake.Contains(i))
                {
                    _chestGroup[i].isShake = true;
                }

                if (randomVideo.Contains(i))
                {
                    _chestGroup[i].isVideo = true;
                }

                _chestGroup[i].InitData(this);
            }

            //默认钥匙数量
            _keyNumber.text = defaultKeyNumber.ToString();
            currentKeyNumber = defaultKeyNumber;
        }

        /// <summary>
        /// 计算兑奖概率
        /// </summary>
        private void CalculationCashPrizeProbability()
        {
            for (int i = 0; i < luckDrawProbabilities.Count; i++)
            {
                if (i <= 0)
                {
                    luckDrawProbabilities[i].minNumber = 0;
                }
                else
                {
                    luckDrawProbabilities[i].minNumber = luckDrawProbabilities[i - 1].maxNumber + 1;
                }

                luckDrawProbabilities[i].maxNumber =
                    luckDrawProbabilities[i].minNumber + luckDrawProbabilities[i].probability - 1;
            }
        }

        /// <summary>
        /// 获得随机数
        /// </summary>
        /// <param name="randomCount"></param>
        /// <param name="randomMinNumber"></param>
        /// <param name="randomNumberMax"></param>
        /// <param name="deposit"></param>
        private void GetRandomNumber(int randomCount, int randomMinNumber, int randomNumberMax, List<int> deposit)
        {
            if (deposit.Count >= randomCount)
            {
                return;
            }

            int randomNumber = Random.Range(randomMinNumber, randomNumberMax + 1);
            if (!deposit.Contains(randomNumber))
            {
                deposit.Add(randomNumber);
            }

            GetRandomNumber(randomCount, randomMinNumber, randomNumberMax, deposit);
        }

        protected override void InitView()
        {
//StartVariableBindPath
            BindUi(ref _chestGroup, "ChestGroup");
            BindUi(ref _key, "Key");
            BindUi(ref _keyIcon, "Key/KeyIcon");
            BindUi(ref _keyNumber, "Key/KeyNumber");
            BindUi(ref _freeKey, "FreeKey");
            BindUi(ref _agreement, "FreeKey/Agreement");
            BindUi(ref _receiveKey, "FreeKey/ReceiveKey");
            BindUi(ref _continueGame, "ContinueGame");
            BindUi(ref _continueGameBtn, "ContinueGame/ContinueGameBtn");
//EndVariableBindPath
        }

        protected override void InitListener()
        {
//StartVariableBindListener
            BindListener(_receiveKey, EventTriggerType.PointerUp, OnReceiveKeyUp);
            BindListener(_receiveKey, EventTriggerType.PointerDown, OnReceiveKeyDown);
            BindListener(_continueGameBtn, EventTriggerType.PointerUp, OnContinueGameBtnUp);
            BindListener(_continueGameBtn, EventTriggerType.PointerDown, OnContinueGameBtnDown);
//EndVariableBindListener
        }

        private void OnContinueGameBtnDown(BaseEventData arg0)
        {
            _continueGameBtn.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        }

        private void OnReceiveKeyDown(BaseEventData arg0)
        {
            _receiveKey.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        }

        //StartVariableBindEvent
        private void OnReceiveKeyUp(BaseEventData targetObj)
        {
            _receiveKey.transform.localScale = Vector3.one;

            if (_agreement.isOn)
            {
                DeleteSwitchTask(_freeBtnTimeTask);
                _freeKey.transform.localScale = Vector3.one;
                viewSvc.ShowView(typeof(ShowTips));
                listenerSvc.ExecuteEvent<Action>(ListenerEventType.WatchVideoEnd, GetFreeKey);
                listenerSvc.ExecuteEvent<string>(ListenerEventType.ShowTipContent, "视频观看成功！");
            }
        }

        /// <summary>
        /// 获得免费钥匙
        /// </summary>
        private void GetFreeKey()
        {
            //默认钥匙数量
            _keyNumber.text = defaultKeyNumber.ToString();
            currentKeyNumber = defaultKeyNumber;
            HideObj(_freeKey);
            ShowObj(_key);
            isGetFreeKey = true;
        }

        private void OnContinueGameBtnUp(BaseEventData targetObj)
        {
            _continueGameBtn.transform.localScale = Vector3.one;

            HideView();
        }

        /// <summary>
        /// 随机获得奖品
        /// </summary>
        /// <returns></returns>
        public GameObject GetPrize()
        {
            int randomNumber = Random.Range(0, 100);
            foreach (LuckDrawProbability luckDrawProbability in luckDrawProbabilities)
            {
                if (luckDrawProbability.minNumber <= randomNumber && randomNumber <= luckDrawProbability.maxNumber)
                {
                    return luckDrawProbability.prize;
                }
            }

            return null;
        }

        /// <summary>
        /// 显示免费钥匙
        /// </summary>
        public void ShowFreeKey()
        {
            ShowObj(_freeKey);
            HideObj(_key);
            _freeBtnTimeTask = AddSwitchTask(new List<UnityAction>()
            {
                () => { _receiveKey.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f - 0.01f); },
                () => { _receiveKey.transform.DOScale(new Vector3(1, 1, 1), 0.5f - 0.01f); },
            }, "免费领取按钮动态大小", 0.5f, 0);
        }

        /// <summary>
        /// 显示继续游戏
        /// </summary>
        public void ShowContinueGame()
        {
            ShowObj(_continueGame);
            HideObj(_key);
            HideObj(_freeKey);
        }

        /// <summary>
        /// 显示当前钥匙数量
        /// </summary>
        public void ShowCurrentNumber()
        {
            _keyNumber.text = currentKeyNumber.ToString();
        }

//EndVariableBindEvent
    }
}