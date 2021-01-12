using System;
using System.Collections.Generic;
using UnityEngine;
using XAnimator.Base;
using XxSlitFrame.Tools.General;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.Tools.ConfigData
{
    [Serializable]
    [CreateAssetMenu(fileName = "StepInitData", menuName = "配置文件/步骤初始化数据", order = 1)]
    public class StepInitData : ScriptableObject
    {
        [HideInInspector] [Header("当前场景的步骤组")] public List<StepInitDataInfo> stepInitDataInfoGroups;

        public StepInitDataInfo GetCurrentStepIndex()
        {
            foreach (StepInitDataInfo stepInitDataInfo in stepInitDataInfoGroups)
            {
                if (stepInitDataInfo.bigIndex == PersistentDataSvc.Instance.currentStepBigIndex && stepInitDataInfo.smallIndex == PersistentDataSvc.Instance.currentStepSmallIndex)
                {
                    return stepInitDataInfo;
                }
            }

            return new StepInitDataInfo();
        }
    }


    [Serializable]
    public class StepInitDataInfo
    {
        /// <summary>
        /// 大步骤索引
        /// </summary>
        public int bigIndex;

        /// <summary>
        /// 小步骤索引
        /// </summary>
        public int smallIndex;

        /// <summary>
        /// 播报的语音索引
        /// </summary>
        [Header("步骤名称")] public string stepName;

        /// <summary>
        /// 播报的语音索引
        /// </summary>
        [Header("播报的语音索引")] public int tipIndex;

        /// <summary>
        /// 显示提示视图
        /// </summary>
        [Header("播报的语音索引")] public bool showTip;

        /// <summary>
        /// 语音播放是否开启
        /// </summary>
        [Header("语音播放是否开启")] public bool isPlayTip;

        /// <summary>
        /// 物品索引
        /// </summary>
        [Header("物品索引")] public bool propGroup;

        /// <summary>
        /// 物品组索引
        /// </summary>
        [Header("相机移动")] public bool cameraMove;

        /// <summary>
        /// 要移动的位置
        /// </summary>
        [Header("要移动的位置")] public CameraPosType cameraPosType;

        /// <summary>
        /// 要播放的动画类型
        /// </summary>
        [Header("要播放的动画类型")] public AnimType animType;

        /// <summary>
        /// 是否要播放动画
        /// </summary>
        [Header("是否要播放动画")] public bool isPlayAnim;

        /// <summary>
        /// 要播放的动画进度
        /// </summary>
        [Header("要播放的动画进度")] public AnimSpeedProgress animSpeedProgress;

        /// <summary>
        /// 动画播放完的执行事件
        /// </summary>
        [Header("动画播放完的执行事件")] public ListenerEventType animPlayOverEvent;

        /// <summary>
        /// 提示播放完的执行事件
        /// </summary>
        [Header("提示播放完的执行事件")] public ListenerEventType tipPlayOverEvent;
    }
}