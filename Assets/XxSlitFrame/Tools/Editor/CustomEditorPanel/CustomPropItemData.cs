using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData;
using XxSlitFrame.Tools.General;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel
{
    public class CustomPropItemData : EditorWindow
    {
        private static EditorWindow window;
        private static bool _display;

        [MenuItem("XFrame/道具工具 #P")]
        private static void ShowWindow()
        {
            window = EditorWindow.GetWindow<CustomPropItemData>();
            window.minSize = new Vector2(900, 300);
            window.maxSize = new Vector2(1600, 900);
            window.titleContent = new GUIContent() {image = null, text = "道具工具"};
            if (!_display)
            {
                window.Show();
            }
            else
            {
                window.Close();
            }

            _display = !_display;
        }

        PropItemData _propItemData;
        Vector2 _scrollPos = Vector2.zero;

        private void OnEnable()
        {
            _display = false;
            if (_propItemData == null)
            {
                _propItemData = (PropItemData) AssetDatabase.LoadAssetAtPath("Assets/XxSlitFrame/Config/PropItemData.asset", typeof(PropItemData));
            }
        }

        private void OnDestroy()
        {
            SaveData();
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        private void SaveData()
        {
            //标记脏区
            EditorUtility.SetDirty(_propItemData);
            // 保存所有修改
            AssetDatabase.SaveAssets();
        }

        public void OnGUI()
        {
            #region 配置文件

            EditorGUILayout.BeginHorizontal();
            //自定义枚举下拉框
            EditorGUILayout.LabelField("道具配置数据:", GUILayout.MaxWidth(80));
#pragma warning disable 618
            _propItemData = (PropItemData) EditorGUILayout.ObjectField(_propItemData, typeof(PropItemData));
#pragma warning restore 618


            if (_propItemData != null)
            {
                if (GUILayout.Button("保存数据", GUILayout.MaxWidth(60)))
                {
                    SaveData();
                }
            }

            EditorGUILayout.EndHorizontal();

            #endregion

            #region 道具编辑

            if (_propItemData != null)
            {
                int propItemCount = System.Enum.GetNames(new PropType().GetType()).Length;

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("增加"))
                {
                    PropItemData.PropItemGroupInfo propItemGroupInfo = new PropItemData.PropItemGroupInfo
                    {
                        propItemGroupInfo = new List<PropItemData.PropItemInfo>()
                    };
                    _propItemData.groupInfos.Add(propItemGroupInfo);

                    for (int i = 0; i < propItemCount; i++)
                    {
                        propItemGroupInfo.propItemGroupInfo.Add(new PropItemData.PropItemInfo() {propTypes = (PropType) i});
                    }
                }

                if (GUILayout.Button("刷新"))
                {
                    foreach (PropItemData.PropItemGroupInfo groupInfo in _propItemData.groupInfos)
                    {
                        int currentPropItemCount = groupInfo.propItemGroupInfo.Count;
                        if (currentPropItemCount < propItemCount)
                        {
                            for (int i = 0; i < propItemCount - currentPropItemCount; i++)
                            {
                                groupInfo.propItemGroupInfo.Add(new PropItemData.PropItemInfo() {propTypes = (PropType) currentPropItemCount + i});
                            }
                        }
                        else
                        {
                            groupInfo.propItemGroupInfo.RemoveRange(propItemCount, groupInfo.propItemGroupInfo.Count - propItemCount);
                            for (int i = 0; i < propItemCount; i++)
                            {
                                groupInfo.propItemGroupInfo[i].propTypes = (PropType) i;
                            }
                        }
                    }
                }

                EditorGUILayout.EndHorizontal();

                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.BeginVertical();


                for (int i = 0; i < _propItemData.groupInfos.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("大步骤:", GUILayout.MaxWidth(40));
                    _propItemData.groupInfos[i].bigIndex = EditorGUILayout.IntField(_propItemData.groupInfos[i].bigIndex, GUILayout.MaxWidth(20));
                    EditorGUILayout.LabelField("小步骤:", GUILayout.MaxWidth(40));
                    _propItemData.groupInfos[i].smallIndex = EditorGUILayout.IntField(_propItemData.groupInfos[i].smallIndex, GUILayout.MaxWidth(20));

                    if (GUILayout.Button("增加", GUILayout.MaxWidth(40)))
                    {
                        foreach (PropItemData.PropItemInfo propItemInfo in _propItemData.groupInfos[i].propItemGroupInfo)
                        {
                            if (propItemInfo.propTypes == _propItemData.groupInfos[i].addPropType)
                            {
                                propItemInfo.display = true;
                                break;
                            }
                        }

                        break;
                    }

                    _propItemData.groupInfos[i].addPropType = (PropType) EditorGUILayout.EnumPopup(_propItemData.groupInfos[i].addPropType, GUILayout.MaxWidth(100));

                    if (GUILayout.Button("删除", GUILayout.MaxWidth(40)))
                    {
                        _propItemData.groupInfos.RemoveAt(i);
                        break;
                    }

                    EditorGUILayout.BeginHorizontal();
                    foreach (PropItemData.PropItemInfo propItemInfo in _propItemData.groupInfos[i].propItemGroupInfo)
                    {
                        if (propItemInfo.display)
                        {
                            {
                                propItemInfo.display = GUILayout.Toggle(propItemInfo.display, propItemInfo.propTypes.ToString(), GUILayout.MinWidth(20), GUILayout.MaxWidth(100));
                            }
                        }
                    }

                    EditorGUILayout.EndHorizontal();


                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndScrollView();
            }

            #endregion
        }
    }
}