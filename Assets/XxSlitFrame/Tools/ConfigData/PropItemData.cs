using System;
using System.Collections.Generic;
using UnityEngine;
using XxSlitFrame.Tools.General;

namespace XxSlitFrame.Tools.ConfigData
{
    [Serializable]
    [CreateAssetMenu(fileName = "PropItemData", menuName = "配置文件/物品数据", order = 1)]
    public class PropItemData : ScriptableObject
    {
        public List<PropItemGroupInfo> groupInfos;


        [Serializable]
        public class PropItemGroupInfo
        {

            public List<PropItemInfo> propItemGroupInfo = new List<PropItemInfo>();

            public int bigIndex;

            public int smallIndex;

            /// <summary>
            /// 增加物品
            /// </summary>
            public PropType addPropType;
        }

        [Serializable]
        public class PropItemInfo
        {
            /// <summary>
            /// 道具类型
            /// </summary>
            public PropType propTypes;

            /// <summary>
            /// 显示隐藏
            /// </summary>
            public bool display;
        }
    }
}