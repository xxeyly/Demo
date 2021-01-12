using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel
{
    public class CustomAssessment : EditorWindow
    {
        public AssessmentData _assessmentData;

        [MenuItem("XFrame/考核工具")]
        private static void ShowWindow()
        {
            EditorWindow window = EditorWindow.GetWindow<CustomAssessment>();
            window.minSize = new Vector2(800, 450);
            window.maxSize = new Vector2(1600, 900);
            window.titleContent = new GUIContent() {image = null, text = "考核工具"};
            window.Show();
        }

        private void OnEnable()
        {
            if (_assessmentData == null)
            {
                _assessmentData = (AssessmentData) AssetDatabase.LoadAssetAtPath("Assets/XxSlitFrame/Config/AssessmentData.asset", typeof(AssessmentData));
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
            EditorUtility.SetDirty(_assessmentData);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 滚动条
        /// </summary>
        private Vector2 scrollBarPos = Vector2.zero;

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            //自定义枚举下拉框
            EditorGUILayout.LabelField("考核配置数据:", GUILayout.MaxWidth(80));
#pragma warning disable 618
            _assessmentData = (AssessmentData) EditorGUILayout.ObjectField(_assessmentData, typeof(AssessmentData));
#pragma warning restore 618

            if (_assessmentData != null)
            {
                EditorGUILayout.LabelField("总分数:", GUILayout.MaxWidth(40));

                _assessmentData.zfs = EditorGUILayout.TextField(_assessmentData.zfs, GUILayout.MaxWidth(40));
                EditorGUILayout.LabelField("及格分数:", GUILayout.MaxWidth(50));

                _assessmentData.jgfs = EditorGUILayout.TextField(_assessmentData.jgfs, GUILayout.MaxWidth(40));

                if (GUILayout.Button("增加步骤", GUILayout.Width(60)))
                {
                    _assessmentData.list.Add(new TopicInfoData());
                }

                if (GUILayout.Button("保存考核数据", GUILayout.Width(80)))
                {
                    SaveData();
                }
            }


            EditorGUILayout.EndHorizontal();

            if (_assessmentData == null)
            {
                return;
            }

            scrollBarPos = EditorGUILayout.BeginScrollView(scrollBarPos);
            for (int i = 0; i < _assessmentData.list.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();

                _assessmentData.list[i].number = i.ToString();
                EditorGUILayout.LabelField("题号:" + _assessmentData.list[i].number, GUILayout.MaxWidth(50));
                _assessmentData.list[i].title = EditorGUILayout.TextField(_assessmentData.list[i].title);
                EditorGUILayout.LabelField("类型:", GUILayout.MaxWidth(40));
                _assessmentData.list[i].type = EditorGUILayout.TextField(_assessmentData.list[i].type, GUILayout.MaxWidth(20));

                EditorGUILayout.LabelField("分数:", GUILayout.MaxWidth(40));
                _assessmentData.list[i].fs = EditorGUILayout.TextField(_assessmentData.list[i].fs,
                    GUILayout.MaxWidth(20));
                EditorGUILayout.LabelField("单位:", GUILayout.MaxWidth(40));
                _assessmentData.list[i].unit = EditorGUILayout.TextField(_assessmentData.list[i].unit, GUILayout.MaxWidth(20));
                if (GUILayout.Button("增加选项", GUILayout.Width(60)))
                {
                    _assessmentData.list[i].OpList().Add(String.Empty);
                    _assessmentData.list[i].OptionList().Add(false);
                }

                if (GUILayout.Button(_assessmentData.list[i].ShowOpList() ? "隐藏选项" : "显示选项", GUILayout.Width(60)))
                {
                    _assessmentData.list[i].SwitchShowOpList();
                }

                if (GUILayout.Button("增加选项", GUILayout.Width(60)))
                {
                    _assessmentData.list[i].OpList().Add(String.Empty);
                    _assessmentData.list[i].OptionList().Add(false);
                }

                if (GUILayout.Button("插入", GUILayout.Width(40)))
                {
                    _assessmentData.list.Insert(i + 1, new TopicInfoData());
                }

                if (GUILayout.Button("上移", GUILayout.Width(40)))
                {
                    if (i != 0)
                    {
                        TopicInfoData tempTopicInfoData = _assessmentData.list[i];
                        _assessmentData.list[i] = _assessmentData.list[i - 1];
                        _assessmentData.list[i - 1] = tempTopicInfoData;
                    }
                }

                if (GUILayout.Button("下移", GUILayout.Width(40)))
                {
                    if (i != _assessmentData.list.Count - 1)
                    {
                        TopicInfoData tempTopicInfoData = _assessmentData.list[i];
                        _assessmentData.list[i] = _assessmentData.list[i + 1];
                        _assessmentData.list[i + 1] = tempTopicInfoData;
                    }
                }

                if (GUILayout.Button("删除", GUILayout.Width(40)))
                {
                    _assessmentData.list.RemoveAt(i);
                    return;
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical();
                //选项
                _assessmentData.list[i].op = null;
                if (_assessmentData.list[i].OptionList().Count < _assessmentData.list[i].OpList().Count)
                {
                    for (int j = 0; j < _assessmentData.list[i].OpList().Count - _assessmentData.list[i].OptionList().Count; j++)
                    {
                        _assessmentData.list[i].OptionList().Add(false);
                    }
                }

                _assessmentData.list[i].zqda = "";
                if (_assessmentData.list[i].ShowOpList())
                {
                    //选项列表
                    for (int j = 0; j < _assessmentData.list[i].OpList().Count; j++)
                    {
                        EditorGUILayout.BeginVertical();
                        EditorGUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField("选项:" + j, GUILayout.MaxWidth(50));
                        _assessmentData.list[i].OpList()[j] = EditorGUILayout.TextField(_assessmentData.list[i].OpList()[j]);
                        EditorGUILayout.LabelField("正确答案:", GUILayout.MaxWidth(50));
                        _assessmentData.list[i].OptionList()[j] = EditorGUILayout.Toggle("", _assessmentData.list[i].OptionList()[j], GUILayout.MaxWidth(20));

                        if (_assessmentData.list[i].OptionList()[j])
                        {
                            _assessmentData.list[i].zqda += _assessmentData.list[i].OpList()[j] + ";";
                        }

                        if (GUILayout.Button("删除选项", GUILayout.Width(60)))
                        {
                            _assessmentData.list[i].OpList().RemoveAt(j);
                            _assessmentData.list[i].OptionList().RemoveAt(j);
                            return;
                        }


                        EditorGUILayout.EndHorizontal();

                        EditorGUILayout.EndVertical();
                    }
                }

                if (_assessmentData.list[i].OpList().Count == 0)
                {
                    _assessmentData.list[i].zqda = _assessmentData.list[i].title;
                }

                for (int j = 0; j < _assessmentData.list[i].OpList().Count; j++)
                {
                    _assessmentData.list[i].op += _assessmentData.list[i].OpList()[j] + ";";
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndVertical();


                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }
    }
}