using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XxSlitFrame.Tools.ConfigData;
using XxSlitFrame.Tools.General;
using XxSlitFrame.Tools.Svc;
using XxSlitFrame.View;

namespace Tips
{
    [Serializable]
    public class Tips : SingletonBaseWindow<Tips>
    {
        private Text _shellContent;
        private Image _contentBackground;
        private Text _content;
        private Button _sure;
        public TipsData tipsData;

        /// <summary>
        /// 换行字数
        /// </summary>
        [SerializeField] private int lineFeedCount = 30;

        [Header("提示内容")] public Dictionary<int, TipsData.TipsDataInfo> stepTipsDic;
        private int _tipPlayTimeTask;

        protected override void OnlyOnceInit()
        {
            base.OnlyOnceInit();
            stepTipsDic = new Dictionary<int, TipsData.TipsDataInfo>();
            foreach (TipsData.TipsDataInfo tipsDataInfo in tipsData.tipsDataInfos)
            {
                stepTipsDic.Add(tipsDataInfo.tipIndex, tipsDataInfo);
            }
        }

        private int _tipsIndex;

        protected override void InitViewShowType()
        {
            base.InitViewShowType();
            ViewShowType = ViewShowType.Frozen;
        }

        public override void Init()
        {
            // lineFeedCount = 40;
        }

        protected override void InitView()
        {
            BindUi(ref _shellContent, "ShellContent");
            BindUi(ref _contentBackground, "ShellContent/ContentBackground");
            BindUi(ref _content, "ShellContent/ContentBackground/Content");
            BindUi(ref _sure, "ShellContent/ContentBackground/Content/Sure");
        }

        protected override void InitListener()
        {
            BindListener(_sure, EventTriggerType.PointerClick, OnSure);
            listenerSvc.AddListenerEvent<int>(ListenerEventType.PlayTips, PlayTips);
            listenerSvc.AddListenerEvent(ListenerEventType.StopTips, StopTips);
            listenerSvc.AddListenerEvent<int, UnityAction>(ListenerEventType.PlayTipsAndAction, PlayTips);
        }

        /// <summary>
        /// 停止音频播放
        /// </summary>
        private void StopTips()
        {
            audioSvc.StopTipAndDialogAudio();
            DeleteTimeTask(_tipPlayTimeTask);
        }

        /// <summary>
        /// 显示提示
        /// </summary>
        public void PlayTips(int tipsIndex)
        {
            string content = stepTipsDic[tipsIndex].tipsContent.Trim();
            if (content.Length > lineFeedCount)
            {
                //字数大于40字,换行
                if (content.Length / lineFeedCount >= 1)
                {
                    for (int i = 1; i <= content.Length / lineFeedCount; i++)
                    {
                        content = content.Insert(lineFeedCount * i, "\n");
                    }
                }
            }


            _content.text = content;
            _shellContent.text = content;
            audioSvc.PlayTipAndDialogAudio(stepTipsDic[tipsIndex].tipsAudioClip);
            _tipsIndex = tipsIndex;
            // Debug.Log("当前操作索引" + tipsIndex + stepTipsDic[tipsIndex].sureOperation);
            if (stepTipsDic[tipsIndex].sureOperation)
            {
                ShowObj(_sure);
            }
            else
            {
                HideObj(_sure);
            }
        }

        /// <summary>
        /// 显示提示,并在结束后执行事件
        /// </summary>
        public void PlayTips(int tipsIndex, UnityAction action)
        {
            PlayTips(tipsIndex);
            if (stepTipsDic.ContainsKey(tipsIndex) && stepTipsDic[tipsIndex].tipsAudioClip != null)
            {
                _tipPlayTimeTask = AddTimeTask(action, "提示事件", stepTipsDic[tipsIndex].tipsAudioClip.length);
            }
        }

        private void OnSure(BaseEventData targetObj)
        {
            switch (_tipsIndex)
            {
                case 0:
                    break;
            }
        }

        protected override void ViewDestroy()
        {
            base.ViewDestroy();
            audioSvc.StopTipAndDialogAudio();
        }
    }
}