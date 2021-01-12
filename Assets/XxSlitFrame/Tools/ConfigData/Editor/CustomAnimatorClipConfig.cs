using System;
using System.Collections.Generic;
using UnityEngine;

namespace XxSlitFrame.Tools.ConfigData.Editor
{
    [CreateAssetMenu(fileName = "CustomAnimatorClipConfig", menuName = "配置文件/动画文件关联配置", order = 1)]
    public class CustomAnimatorClipConfig : ScriptableObject
    {
        /// <summary>
        /// 动画控制器名称
        /// </summary>
        [Header("场景控制器")] public Animator animatorController;

        /// <summary>
        /// 动画控制器名称
        /// </summary>
        [Header("项目名称")] public string animatorName;

        /// <summary>
        /// 输出文件夹
        /// </summary>
        [Header("输出文件夹")] public string exportPath;

        public List<AnimFbxAndAnimClipData> animFbxAndAnimClipDatas = new List<AnimFbxAndAnimClipData>();
    }

    [Serializable]
    public class AnimFbxAndAnimClipData
    {
        public GameObject animFbx;
        public AnimatorClipData animatorClipData;
    }
}