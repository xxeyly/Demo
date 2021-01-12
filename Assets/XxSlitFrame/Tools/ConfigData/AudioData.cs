using System;
using System.Collections.Generic;
using UnityEngine;

namespace XxSlitFrame.Tools.ConfigData
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "配置文件/音频数据", order = 1)]
    public class AudioData : ScriptableObject
    {
        [HideInInspector] [Header("音效")] public List<AudioDataInfo> audioDataInfos;

        /// <summary>
        /// 音乐类型
        /// </summary>
        public enum AudioType
        {
            /// <summary>
            /// 默认音乐类型
            /// </summary>
            [Header("默认音乐类型")] ENormal,

            /// <summary>
            /// 背景音乐
            /// </summary>
            [Header("背景音乐")] EBackground,

            /// <summary>
            /// 进入场景
            /// </summary>
            [Header("进入场景")] EEnterExperiment,

            /// <summary>
            /// 错误弹窗
            /// </summary>
            [Header("错误弹窗")] EErrorPopup,

            /// <summary>
            /// 提示弹窗
            /// </summary>
            [Header("提示弹窗")] ETipsPopup,

            /// <summary>
            /// 关闭
            /// </summary>
            [Header("关闭")] EClose,

            /// <summary>
            /// 关闭背景音乐
            /// </summary>
            [Header("关闭背景音乐")] ECloseBackground,

            /// <summary>
            /// 确定
            /// </summary>
            [Header("确定")] ESure,

            /// <summary>
            /// 跳步骤
            /// </summary>
            [Header("跳步骤")] ENextStep,

            /// <summary>
            /// 重做,下一步
            /// </summary>
            [Header("重做")] ERedo,

            /// <summary>
            /// 鼠标点击
            /// </summary>
            [Header("鼠标点击")] EClick,

            /// <summary>
            /// 显示物品
            /// </summary>
            DisplayObjects,

            /// <summary>
            /// 提示音
            /// </summary>
            Tip,

            /// <summary>
            /// 警告
            /// </summary>
            Warning,

            /// <summary>
            /// 消防广播
            /// </summary>
            FireBroadcast,

            /// <summary>
            /// 自检
            /// </summary>
            SelfInspection,

            /// <summary>
            /// 按钮点击
            /// </summary>
            BtnClick,

            /// <summary>
            /// 宝箱打开
            /// </summary>
            OpenChest
        }

        [Serializable]
        public class AudioDataInfo
        {
            [Header("对应时长")] public AudioType audioType;
            [Header("对话音频")] public AudioClip dialogueAudioClip;
        }
    }
}