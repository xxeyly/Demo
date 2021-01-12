using System;
using System.Collections.Generic;
using UnityEngine;
using XxSlitFrame.Tools.General;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.Tools.ConfigData
{
    /// <summary>
    /// 提示数据
    /// </summary>
    [CreateAssetMenu(fileName = "TipsData", menuName = "配置文件/提示数据", order = 1)]
    public class TipsData : ScriptableObject
    {
        [HideInInspector] public List<TipsDataInfo> tipsDataInfos;

        [Serializable]
        public class TipsDataInfo
        {
            [Header("提示索引")] public int tipIndex;
            [Header("提示内容")] public string tipsContent;
            [Header("执行事件")] public ListenerEventType endEvent;
            [Header("对话音频")] public AudioClip tipsAudioClip;
            [Header("确认操作")] public bool sureOperation;
        }
    }
}