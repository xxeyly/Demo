using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData;
using XxSlitFrame.Tools.General;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel
{
    public class CustomTip : EditorWindow
    {
        private TipsData _tipsData;
        Vector2 _scrollPos = Vector2.zero;
        private static bool _display;

        [MenuItem("XFrame/提示工具 #T", false)]
        private static void ShowWindow()
        {
            EditorWindow window = EditorWindow.GetWindow<CustomTip>();
            window.minSize = new Vector2(900, 300);
            window.maxSize = new Vector2(1600, 900);
            window.titleContent = new GUIContent() {image = null, text = "提示工具"};
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

        private void OnEnable()
        {
            _display = false;
            if (_tipsData == null)
            {
                _tipsData = (TipsData) AssetDatabase.LoadAssetAtPath("Assets/XxSlitFrame/Config/TipsData.asset", typeof(TipsData));
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        private void SaveData()
        {
            if (_tipsData != null)
            {
                //标记脏区
                EditorUtility.SetDirty(_tipsData);
                // 保存所有修改
                AssetDatabase.SaveAssets();
            }
        }

        private void OnDestroy()
        {
            SaveData();
        }

        private void OnGUI()
        {
            #region 配置文件

            EditorGUILayout.BeginHorizontal();
            //自定义枚举下拉框
            EditorGUILayout.LabelField("提示配置数据:", GUILayout.MaxWidth(80));
#pragma warning disable 618
            _tipsData = (TipsData) EditorGUILayout.ObjectField(_tipsData, typeof(TipsData));
#pragma warning restore 618

            if (GUILayout.Button("保存数据", GUILayout.MaxWidth(60)))
            {
                SaveData();
            }

            EditorGUILayout.EndHorizontal();

            #endregion

            if (_tipsData != null)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("增加"))
                {
                    _tipsData.tipsDataInfos.Add(new TipsData.TipsDataInfo() {tipIndex = _tipsData.tipsDataInfos.Count});
                }

                EditorGUILayout.EndHorizontal();
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

                for (int i = 0; i < _tipsData.tipsDataInfos.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("提示索引", GUILayout.MaxWidth(50));
                    _tipsData.tipsDataInfos[i].tipIndex = EditorGUILayout.IntField(_tipsData.tipsDataInfos[i].tipIndex, GUILayout.MaxWidth(30));
                    EditorGUILayout.LabelField("确认操作", GUILayout.MaxWidth(50));
                    _tipsData.tipsDataInfos[i].sureOperation = EditorGUILayout.Toggle(_tipsData.tipsDataInfos[i].sureOperation, GUILayout.MaxWidth(10));
                    EditorGUILayout.LabelField("结束事件", GUILayout.MaxWidth(50));
                    _tipsData.tipsDataInfos[i].endEvent = (ListenerEventType) EditorGUILayout.EnumPopup(_tipsData.tipsDataInfos[i].endEvent, GUILayout.MaxWidth(100));
                    EditorGUILayout.LabelField("对话音频", GUILayout.MaxWidth(50));
#pragma warning disable 618
                    _tipsData.tipsDataInfos[i].tipsAudioClip = (AudioClip) EditorGUILayout.ObjectField(_tipsData.tipsDataInfos[i].tipsAudioClip, typeof(AudioClip));
#pragma warning restore 618
                    EditorGUILayout.LabelField("提示内容", GUILayout.MaxWidth(50));
                    _tipsData.tipsDataInfos[i].tipsContent = EditorGUILayout.TextField(_tipsData.tipsDataInfos[i].tipsContent);

                    if (GUILayout.Button("删除", GUILayout.MaxWidth(60)))
                    {
                        _tipsData.tipsDataInfos.RemoveAt(i);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndScrollView();
            }
        }
    }
}