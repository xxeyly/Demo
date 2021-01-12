using System;
using System.Collections.Generic;
#if HighlightEffect
using HighlightPlus;
#endif
using UnityEngine;
using UnityEngine.Events;
using XxSlitFrame.Tools.General;
using XxSlitFrame.Tools.Svc;

namespace Prop
{
    public class PropItem : MonoBehaviour
    {
        private Collider _collider;

        [Header("物品类型")] public PropType propType;
#if HighlightEffect
        [Header("高亮物体")] public HighlightEffect highlightEffect;
#endif

        [SerializeField] private Dictionary<PropType, UnityAction> actionDic;

        /// <summary>
        /// 初始触发
        /// </summary>
        public bool initCollider;

        public void Init()
        {
            _collider = GetComponent<Collider>();
            if (_collider != null)
            {
                _collider.enabled = initCollider;
            }
#if HighlightEffect
            if (highlightEffect != null)
            {
                highlightEffect.highlighted = false;
            }
#endif
            actionDic = new Dictionary<PropType, UnityAction>
            {
                {
                    PropType.Normal, () => { Debug.Log("Normal"); }
                }
            };
        }

        /// <summary>
        /// 显示高亮
        /// </summary>
        public void ShowHigh()
        {
#if HighlightEffect
            if (highlightEffect != null)
            {
                highlightEffect.highlighted = true;
            }
#endif
        }

        /// <summary>
        /// 隐藏高亮
        /// </summary>
        public void HideHigh()
        {
#if HighlightEffect
            if (highlightEffect != null)
            {
                highlightEffect.highlighted = false;
            }
#endif
        }

        /// <summary>
        /// 显示物体
        /// </summary>
        public void ShowObj()
        {
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 隐藏物体
        /// </summary>
        public void HideObj()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 打开触发
        /// </summary>
        public void OpenTrigger()
        {
            if (_collider != null)
            {
                _collider.enabled = true;
            }
        }

        /// <summary>
        /// 关闭触发
        /// </summary>
        public void CloseTrigger()
        {
            if (_collider != null)
            {
                _collider.enabled = false;
            }
        }

        private void OnMouseDown()
        {
            actionDic[propType]?.Invoke();
        }

        private void OnMouseEnter()
        {
        }

        private void OnMouseExit()
        {
        }
    }
}