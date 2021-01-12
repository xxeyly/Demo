using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XxSlitFrame.Tools;
using XxSlitFrame.Tools.ConfigData;
using XxSlitFrame.Tools.General;
using XxSlitFrame.Tools.Svc;

namespace Prop
{
    public class PropManager : StartSingleton
    {
        public static PropManager Instance;

        [Header("物品")] private Dictionary<PropType, PropItem> _propDic;
        [Header("物品列表")] [SerializeField] private List<PropItem> _propItems;

        public PropItemData propItemData;

        /// <summary>
        /// 道具初始化 隐藏物体 关闭触发 关闭高亮
        /// </summary>
        private void PropInit()
        {
            foreach (PropItem propItem in _propItems)
            {
                propItem.Init();
            }
        }

        public void HideAll()
        {
            foreach (PropItem propItem in _propItems)
            {
                HideObject(propItem.propType);
            }
        }

        /// <summary>
        /// 操作物体
        /// </summary>
        /// <param name="display">显示隐藏物体</param>
        /// <param name="trigger">是否开启触发</param>
        /// <param name="high">是否高亮</param>
        /// <param name="propType">操作的物体</param>
        public void OperationObj(bool display, bool trigger, bool high, params PropType[] propType)
        {
            foreach (PropType type in propType)
            {
                if (_propDic.Keys.Contains(type))
                {
                    PropItem operationObj = _propDic[type];
                    if (display)
                    {
                        operationObj.ShowObj();
                    }
                    else
                    {
                        operationObj.HideObj();
                    }

                    if (trigger)
                    {
                        operationObj.OpenTrigger();
                    }
                    else
                    {
                        operationObj.CloseTrigger();
                    }

                    if (high)
                    {
                        operationObj.ShowHigh();
                    }
                    else
                    {
                        operationObj.HideHigh();
                    }
                }
            }
        }

        /// <summary>
        /// 隐藏指定物体
        /// </summary>
        /// <param name="propType"></param>
        private void HideObject(params PropType[] propType)
        {
            for (int i = 0; i < propType.Length; i++)
            {
                if (_propDic.ContainsKey(propType[i]))
                {
                    _propDic[propType[i]].HideObj();
                }
            }
        }

        /// <summary>
        /// 显示物体
        /// </summary>
        /// <param name="propType"></param>
        private void ShowObject(params PropType[] propType)
        {
            for (int i = 0; i < propType.Length; i++)
            {
                if (_propDic.ContainsKey(propType[i]))
                {
                    _propDic[propType[i]].ShowObj();
                }
            }
        }

        public override void StartSvc()
        {
            Instance = GetComponent<PropManager>();
        }

        public override void Init()
        {
            _propDic = new Dictionary<PropType, PropItem>();
            _propItems = new List<PropItem>(FindObjectsOfType<PropItem>());
            foreach (PropItem propItem in _propItems)
            {
                propItem.Init();
                if (!_propDic.ContainsKey(propItem.propType))
                {
                    _propDic.Add(propItem.propType, propItem);
                }
                else
                {
                    Debug.Log(propItem.gameObject);
                }
            }

            ListenerSvc.Instance.AddListenerEvent(ListenerEventType.PropInit, PropInit);
            ListenerSvc.Instance.AddListenerEvent<int, int>(ListenerEventType.PropShowGroup, PropShowGroup);
        }

        private void PropShowGroup(int bigStep, int smallStep)
        {
            //获得当前组
            PropItemData.PropItemGroupInfo propItemGroupInfo = null;
            foreach (PropItemData.PropItemGroupInfo itemGroupInfo in propItemData.groupInfos)
            {
                if (itemGroupInfo.bigIndex == PersistentDataSvc.Instance.currentStepBigIndex &&
                    itemGroupInfo.smallIndex == PersistentDataSvc.Instance.currentStepSmallIndex)
                {
                    propItemGroupInfo = itemGroupInfo;
                    break;
                }
            }

            if (propItemGroupInfo != null)
            {
                for (int i = 0; i < propItemGroupInfo.propItemGroupInfo.Count; i++)
                {
                    if (propItemGroupInfo.propItemGroupInfo[i].display)
                    {
                        ShowObject(propItemGroupInfo.propItemGroupInfo[i].propTypes);
                    }
                    else
                    {
                        HideObject(propItemGroupInfo.propItemGroupInfo[i].propTypes);
                    }
                }
            }
        }

        /// <summary>
        /// 根据物品类型,获得物品
        /// </summary>
        /// <param name="propType"></param>
        public PropItem GetObjByPropType(PropType propType)
        {
            if (_propDic.ContainsKey(propType))
            {
                return _propDic[propType];
            }

            return null;
        }
    }
}