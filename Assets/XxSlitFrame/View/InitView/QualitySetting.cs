using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XxSlitFrame.Tools.ConfigData;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.View.InitView
{
    public class QualitySetting : BaseWindow
    {
        private Button _low;
        private GameObject _lowSelect;
        private Button _center;
        private GameObject _centerSelect;
        private Button _high;
        private GameObject _highSelect;

        public override void Init()
        {
            switch (persistentDataSvc.qualitySettingType)
            {
                case QualitySettingType.Low:
                    ShowQuality(0);
                    break;
                case QualitySettingType.Center:
                    ShowQuality(1);

                    break;
                case QualitySettingType.High:
                    ShowQuality(2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ShowQuality(int qualityIndex)
        {
            if (qualityIndex == 0)
            {
                ShowObj(_lowSelect);
                HideObj(_centerSelect, _highSelect);
                QualitySettings.SetQualityLevel(0, true);
                persistentDataSvc.qualitySettingType = QualitySettingType.Low;
            }
            else if (qualityIndex == 1)
            {
                ShowObj(_centerSelect);
                HideObj(_lowSelect, _highSelect);
                QualitySettings.SetQualityLevel(2, true);
                persistentDataSvc.qualitySettingType = QualitySettingType.Center;
            }
            else if (qualityIndex == 2)
            {
                ShowObj(_highSelect);
                HideObj(_lowSelect, _centerSelect);
                QualitySettings.SetQualityLevel(4, true);
                persistentDataSvc.qualitySettingType = QualitySettingType.High;
            }
        }

        protected override void InitView()
        {
            BindUi(ref _low, "Low");
            BindUi(ref _lowSelect, "Low/LowSelect");
            BindUi(ref _center, "Center");
            BindUi(ref _centerSelect, "Center/CenterSelect");
            BindUi(ref _high, "High");
            BindUi(ref _highSelect, "High/HighSelect");
        }

        protected override void InitListener()
        {
            BindListener(_low, EventTriggerType.PointerClick, OnLow);
            BindListener(_center, EventTriggerType.PointerClick, OnCenter);
            BindListener(_high, EventTriggerType.PointerClick, OnHigh);
        }

        private void OnLow(BaseEventData targetObj)
        {
            audioSvc.PlayEffectAudio(AudioData.AudioType.EClick);

            ShowQuality(0);
        }

        private void OnCenter(BaseEventData targetObj)
        {
            audioSvc.PlayEffectAudio(AudioData.AudioType.EClick);

            ShowQuality(1);
        }

        private void OnHigh(BaseEventData targetObj)
        {
            audioSvc.PlayEffectAudio(AudioData.AudioType.EClick);

            ShowQuality(2);
        }
    }
}