using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using XxSlitFrame.Tools.General;
using XxSlitFrame.Tools.Svc;

namespace XAnimator.Base
{
    /// <summary>
    /// 播放动画进化
    /// </summary>
    public enum AnimSpeedProgress
    {
        None,
        Start,
        End
    }

    /// <summary>
    /// 动画控制器基类
    /// </summary>
    public abstract class XAnimatorControllerBase : MonoBehaviour
    {
        protected UnityEngine.Animator animator;
        private List<string> _animationClips;

        public virtual void StartSvc()
        {
            animator = GetComponent<UnityEngine.Animator>();
            _animationClips = new List<string>();
            foreach (AnimationClip animationClip in animator.runtimeAnimatorController.animationClips)
            {
                _animationClips.Add(animationClip.name);
            }
        }

        public void PlayAnim(AnimType animationType, float animProgress)
        {
            if (_animationClips.Contains(animationType.ToString()))
            {
                animator.speed = 0;
                if (animProgress >= 1f)
                {
                    animProgress = 0.99f;
                }

                animator.Play(animationType.ToString(), 0, animProgress);
            }
        }

        public void PlayAnim(AnimType animationType, AnimSpeedProgress animSpeedProgress)
        {
            if (_animationClips.Contains(animationType.ToString()))
            {
                animator.speed = 0;
                if (animSpeedProgress == AnimSpeedProgress.End)
                {
                    animator.Play(animationType.ToString(), 0, 0.99f);
                }
                else if (animSpeedProgress == AnimSpeedProgress.Start)
                {
                    animator.Play(animationType.ToString(), 0, normalizedTime: 0.01f);
                }
            }
        }

        private int _playAnimTimeTask;

        /// <summary>
        /// 播放动画+事件
        /// </summary>
        /// <param name="animationType"></param>
        /// <param name="eventAction"></param>
        /// <param name="delay"></param>
        public void PlayAnim(AnimType animationType, UnityAction eventAction, float delay = 0)
        {
            if (_animationClips.Contains(animationType.ToString()))
            {
                PlayAnim(animationType);
                _playAnimTimeTask = TimeSvc.Instance.AddTimeTask(eventAction, "播放动画:" + animationType, GetPlayAnimLength(animationType) + delay);
            }
        }


        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="animationType"></param>
        public void PlayAnim(AnimType animationType)
        {
            if (_animationClips.Contains(animationType.ToString()))
            {
                animator.speed = 1;
                animator.SetTrigger(animationType.ToString());
            }
        }

        /// <summary>
        /// 停止播放动画
        /// </summary>
        public void StopAnim()
        {
            animator.speed = 0;
        }

        /// <summary>
        /// 获得动画时长
        /// </summary>
        /// <param name="animType"></param>
        /// <returns></returns>
        public float GetPlayAnimLength(AnimType animType)
        {
            AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip item in clips)
            {
                if (item.name == animType.ToString())
                {
                    return item.length;
                }
            }

            return -1;
        }

        public void StopAnimTaskTime()
        {
            StopAnim();
            TimeSvc.Instance.DeleteTimeTask(_playAnimTimeTask);
        }
    }
}