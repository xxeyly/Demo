using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XxSlitFrame.Tools.ConfigData;
using XxSlitFrame.Tools.General;
using XxSlitFrame.Tools.Svc;
using XxSlitFrame.View;

namespace Custom
{
    public class ChestItem : LocalBaseWindow
    {
        public bool isVideo;
        public bool isShake;
        public bool isOpen;
        private Image _chestImg;
        private Image _videoIcon;
        private GameObject _reward;
        private Button _event;
        private int _shakeTimeTask;
        private Chest _chest;
        private bool _isEnter;

        /// <summary>
        /// 奖励物品
        /// </summary>
        public GameObject rewardItem;

        protected override void InitView()
        {
            BindUi(ref _chestImg, "ChestImg");
            BindUi(ref _videoIcon, "ChestImg/VideoIcon");
            BindUi(ref _reward, "Reward");
            BindUi(ref _event, "Event");
        }


        protected override void InitListener()
        {
            BindListener(_event, EventTriggerType.PointerDown, OnEventDown);
            BindListener(_event, EventTriggerType.PointerUp, OnEventUp);
            BindListener(_event, EventTriggerType.PointerEnter, OnEnter);
            BindListener(_event, EventTriggerType.PointerExit, OnExit);
        }

        private void OnExit(BaseEventData arg0)
        {
            _isEnter = false;
        }

        private void OnEnter(BaseEventData arg0)
        {
            _isEnter = true;
        }

        private void OnEventUp(BaseEventData arg0)
        {
            _chestImg.transform.localScale = Vector3.one;
            if (isOpen || !_isEnter)
            {
                return;
            }

            _chestImg.transform.localEulerAngles = Vector3.zero;
            //是播放视频
            if (isVideo)
            {
                ViewSvc.Instance.ShowView(typeof(ShowTips));
                ListenerSvc.Instance.ExecuteEvent<Action>(ListenerEventType.WatchVideoEnd, ShowRewardItem);
                ListenerSvc.Instance.ExecuteEvent<string>(ListenerEventType.ShowTipContent, "视频观看成功！");
            }
            else
            {
                if (_chest.currentKeyNumber > 0)
                {
                    ShowRewardItem();
                }
                else
                {
                }
            }
        }

        private void OnEventDown(BaseEventData arg0)
        {
            _chestImg.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        }

        /// <summary>
        /// 显示奖品
        /// </summary>
        private void ShowRewardItem()
        {
            //停掉摇晃动画
            TimeSvc.Instance.DeleteSwitchTask(_shakeTimeTask);
            AudioSvc.Instance.PlayEffectAudio(AudioData.AudioType.BtnClick);
            AudioSvc.Instance.PlayEffectAudio(AudioData.AudioType.OpenChest);
            isOpen = true;
            HideObj(_chestImg.gameObject);
            //显示奖品
            rewardItem = _chest.GetPrize();
            ShowObj(_reward);
            GameObject tempRewardItem = Instantiate(rewardItem, Vector3.zero, Quaternion.identity, _reward.transform);
            tempRewardItem.transform.localPosition = Vector3.zero;
            //根据是不是视频减少钥匙数量
            RemoveKeyNumber();
        }

        /// <summary>
        /// 减少钥匙数量
        /// </summary>
        private void RemoveKeyNumber()
        {
            if (!isVideo)
            {
                _chest.currentKeyNumber -= 1;
                _chest.ShowCurrentNumber();
                CheckNumber();
            }
        }

        private void CheckNumber()
        {
            if (_chest.currentKeyNumber <= 0)
            {
                //还没有领取免费钥匙
                if (!_chest.isGetFreeKey)
                {
                    _chest.ShowFreeKey();
                }
                else
                {
                    _chest.ShowContinueGame();
                }
            }
        }

        protected override void InitData()
        {
        }

        public void InitData(Chest chest)
        {
            _chest = chest;
            if (isVideo)
            {
                ShowObj(_videoIcon.gameObject);
            }

            if (isShake)
            {
                _shakeTimeTask =
                    TimeSvc.Instance.AddSwitchTask(new List<UnityAction>()
                    {
                        () =>
                        {
                            _chestImg.transform.DOLocalRotate(new Vector3(0, 0, -_chest.shakeAngle),
                                _chest.shakeSpeed - 0.01f);
                        },
                        () => { _chestImg.transform.DOLocalRotate(new Vector3(0, 0, 0), _chest.shakeSpeed - 0.01f); },
                        () =>
                        {
                            _chestImg.transform.DOLocalRotate(new Vector3(0, 0, _chest.shakeAngle),
                                _chest.shakeSpeed - 0.01f);
                        },
                        () => { _chestImg.transform.DOLocalRotate(new Vector3(0, 0, 0), _chest.shakeSpeed - 0.01f); },
                    }, "抖动任务", _chest.shakeSpeed, 0);
            }
        }
    }
}