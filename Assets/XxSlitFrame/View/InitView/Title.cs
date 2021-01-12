using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XxSlitFrame.Tools.ConfigData;

namespace XxSlitFrame.View.InitView
{
    public class Title : BaseWindow
    {
        private Button _audioOpen;
        private Button _audioClose;
        private Button _close;
        private GameObject _closePanel;
        private Button _exit;
        private Button _no;
        private GameObject _mask;
        public override void Init()
        {
            HideObj(_mask, _closePanel);
            if (persistentDataSvc.audioState)
            {
                ShowObj(_audioOpen);
                HideObj(_audioClose);
            }
            else
            {
                ShowObj(_audioClose);
                HideObj(_audioOpen);
            }
        }

        protected override void InitView()
        {
            BindUi(ref _audioOpen, "AudioOpen");
            BindUi(ref _audioClose, "AudioClose");
            BindUi(ref _close, "Close");
            BindUi(ref _closePanel, "ClosePanel");
            BindUi(ref _exit, "ClosePanel/Exit");
            BindUi(ref _no, "ClosePanel/No");
            BindUi(ref _mask, "Mask");
        }

        protected override void InitListener()
        {
            BindListener(_audioOpen, EventTriggerType.PointerClick, OnAudioOpen);
            BindListener(_audioClose, EventTriggerType.PointerClick, OnAudioClose);
            BindListener(_close, EventTriggerType.PointerClick, OnClose);
            BindListener(_exit, EventTriggerType.PointerClick, OnExit);
            BindListener(_no, EventTriggerType.PointerClick, OnNo);
        }

        private void OnAudioOpen(BaseEventData targetObj)
        {
            ShowObj(_audioClose);
            HideObj(_audioOpen);
            audioSvc.PauseBackgroundAudio();
        }

        private void OnAudioClose(BaseEventData targetObj)
        {
            ShowObj(_audioOpen);
            HideObj(_audioClose);
            audioSvc.PlayBackgroundAudio();
        }

        private void OnClose(BaseEventData targetObj)
        {
            ShowObj(_closePanel, _mask);
            audioSvc.PlayEffectAudio(AudioData.AudioType.DisplayObjects);
        }

        private void OnExit(BaseEventData targetObj)
        {
            audioSvc.PlayEffectAudio(AudioData.AudioType.DisplayObjects);

            sceneSvc.SceneEsc();
        }

        private void OnNo(BaseEventData targetObj)
        {
            audioSvc.PlayEffectAudio(AudioData.AudioType.DisplayObjects);

            HideObj(_closePanel, _mask);
        }
    }
}