using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XxSlitFrame.Tools;
using XxSlitFrame.Tools.General;
using XxSlitFrame.Tools.Svc;

namespace XAnimator.Base
{
    /// <summary>
    /// 动画控制管理器
    /// </summary>
    public class AnimatorControllerManager : StartSingleton
    {
        public static AnimatorControllerManager Instance;

        [Header("所有动画控制器")] [SerializeField] private List<XAnimatorControllerBase> allAnimController = new List<XAnimatorControllerBase>();

        /// <summary>
        /// 动画控制器任务
        /// </summary>
        private int _animatorTimeTask;

        public override void StartSvc()
        {
            Instance = GetComponent<AnimatorControllerManager>();
        }

        public override void Init()
        {
            allAnimController = new List<XAnimatorControllerBase>(FindObjectsOfType<XAnimatorControllerBase>());
            foreach (XAnimatorControllerBase animatorControllerBase in allAnimController)
            {
                animatorControllerBase.StartSvc();
            }
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="animType"></param>
        public void PlayAnim(AnimType animType)
        {
            foreach (XAnimatorControllerBase controllerBase in allAnimController)
            {
                controllerBase.PlayAnim(animType);
            }
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="animType"></param>
        /// <param name="playProgress">播放进度</param>
        public void PlayAnim(AnimType animType, float playProgress)
        {
            foreach (XAnimatorControllerBase controllerBase in allAnimController)
            {
                controllerBase.PlayAnim(animType, playProgress);
            }
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="animType"></param>
        /// <param name="animSpeedProgress"></param>
        public void PlayAnim(AnimType animType, AnimSpeedProgress animSpeedProgress)
        {
            foreach (XAnimatorControllerBase controllerBase in allAnimController)
            {
                if (controllerBase.gameObject.activeInHierarchy)
                {
                    controllerBase.PlayAnim(animType, animSpeedProgress);
                }
            }
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="animType"></param>
        /// <param name="animAction"></param>
        public void PlayAnim(AnimType animType, UnityAction animAction)
        {
            TimeSvc.Instance.DeleteTimeTask(_animatorTimeTask);
            _animatorTimeTask = TimeSvc.Instance.AddTimeTask(animAction, "动画播放时间", GetPlayAnimLength(animType));
            foreach (XAnimatorControllerBase controllerBase in allAnimController)
            {
                controllerBase.PlayAnim(animType);
            }
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="animType"></param>
        /// <param name="listenerEventType"></param>
        public void PlayAnim(AnimType animType, ListenerEventType listenerEventType)
        {
            TimeSvc.Instance.DeleteTimeTask(_animatorTimeTask);
            _animatorTimeTask = TimeSvc.Instance.AddTimeTask(() => { ListenerSvc.Instance.ExecuteEvent(listenerEventType); }, "动画播放时间", GetPlayAnimLength(animType));
            foreach (XAnimatorControllerBase controllerBase in allAnimController)
            {
                controllerBase.PlayAnim(animType);
            }
        }

        public void StopAnimAction()
        {
            TimeSvc.Instance.DeleteTimeTask(_animatorTimeTask);
            foreach (XAnimatorControllerBase controllerBase in allAnimController)
            {
                controllerBase.StopAnim();
            }
        }

        public void StopAllAnimAction()
        {
            foreach (XAnimatorControllerBase controllerBase in allAnimController)
            {
                controllerBase.StopAnimTaskTime();
            }
        }

        /// <summary>
        /// 获得动画时长
        /// </summary>
        /// <param name="animType"></param>
        /// <returns></returns>
        public float GetPlayAnimLength(AnimType animType)
        {
            float animLength = 0;
            foreach (XAnimatorControllerBase animatorControllerBase in allAnimController)
            {
                if (animatorControllerBase.GetPlayAnimLength(animType) != -1f)
                {
                    animLength = animatorControllerBase.GetPlayAnimLength(animType);
                    return animLength;
                }
            }

            return animLength;
        }
    }
}