using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XAnimator.Base;
using XxSlitFrame.Tools.General;
using XxSlitFrame.Tools.Svc;

/// <summary>
/// 对话角色
/// </summary>
public enum Role
{
    Doctor = 0,
    Patient = 1
}

/// <summary>
/// 对话长短
/// </summary>
public enum Length
{
    EShort = 0,
    EIn = 1,
    ELong = 2
}

namespace XxSlitFrame.Tools.ConfigData
{
    [Serializable]
    [CreateAssetMenu(fileName = "DialogueData", menuName = "配置文件/对话数据", order = 1)]
    public class DialogueData : ScriptableObject
    {
        [HideInInspector] [Header("对话片段")] public List<DialogDataInfoContainer> dataInfos;
    }

    [Serializable]
    public class DialogDataInfoContainer
    {
        [Header("对话内容组")] public List<DialogDataInfo> dialogDataInfos = new List<DialogDataInfo>();
        [Header("执行事件")] public ListenerEventType endEvent;
    }

    [Serializable]
    public class DialogDataInfo
    {
        [Header("对话角色")] public Role role;
        [Header("对话长度")] public Length length;
        [TextArea] [Header("对话内容")] public string dialogContent;
        [TextArea] [Header("对话动画")] public AnimType animType;
        [Header("执行事件")] public ListenerEventType dialogClipEvent;
        [Header("对话音频")] public AudioClip dialogueAudioClip;
    }
}