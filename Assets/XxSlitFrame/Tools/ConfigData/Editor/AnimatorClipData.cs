using System;
using System.Collections.Generic;
using UnityEngine;

namespace XxSlitFrame.Tools.ConfigData.Editor
{
    [CreateAssetMenu(fileName = "AnimatorClipData", menuName = "配置文件/动画片段配置", order = 1)]
    public class AnimatorClipData : ScriptableObject
    {
        [HideInInspector] public List<AnimatorClipDataInfo> animatorClipDataInfos = new List<AnimatorClipDataInfo>();

        [Serializable]
        public class AnimatorClipDataInfo
        {
            /// <summary>
            /// 动画名字 && 属性名称
            /// </summary>
            [Header("动画名字 && 属性名称")] public string animatorClipName;

            /// <summary>
            /// 属性类型
            /// </summary>
            [Header("属性类型")] public AnimatorControllerParameterType animatorControllerParameterType;

            /// <summary>
            /// 固定过渡持续时间
            /// </summary>
            [Header("固定过渡持续时间")] public bool fixedDuration;

            /// <summary>
            /// 过渡持续时间
            /// </summary>
            [Header("过渡持续时间")] public float transitionDuration;

            /// <summary>
            /// 开始帧
            /// </summary>
            [Header("开始帧")] public int animatorClipFirstFrame;

            /// <summary>
            /// 结束帧
            /// </summary>
            [Header("结束帧")] public int animatorClipLastFrame;

            /// <summary>
            /// 是否循环
            /// </summary>
            [Header("是否循环")] public bool animatorClipIsLoop;

            /// <summary>
            /// 是否倒放
            /// </summary>
            [Header("是否循环")] public bool animatorClipIsRewind;
        }
    }
}