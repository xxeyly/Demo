using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using XAnimator.Base;
using XxSlitFrame.Tools.ConfigData;
using XxSlitFrame.Tools.General;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel
{
    public class CustomStepInitData : EditorWindow
    {
        StepInitData _stepInitData;
        Vector2 _scrollPos = Vector2.zero;
        private static bool _display;

        [MenuItem("XFrame/步骤数据 #S")]
        private static void ShowWindow()
        {
            EditorWindow window = EditorWindow.GetWindow<CustomStepInitData>();
            window.minSize = new Vector2(1000, 300);
            window.maxSize = new Vector2(1600, 900);
            window.titleContent = new GUIContent() {image = null, text = "步骤数据"};
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
            if (_stepInitData == null)
            {
                _stepInitData = (StepInitData) AssetDatabase.LoadAssetAtPath("Assets/XxSlitFrame/Config/" + SceneManager.GetActiveScene().name + "StepData.asset", typeof(StepInitData));
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
            if (_stepInitData != null)
            {
                //标记脏区
                EditorUtility.SetDirty(_stepInitData);
                // 保存所有修改
                AssetDatabase.SaveAssets();
            }
        }

        public void OnGUI()
        {
            #region 配置文件

            EditorGUILayout.BeginHorizontal();
            //自定义枚举下拉框
            EditorGUILayout.LabelField("步骤配置数据:", GUILayout.MaxWidth(80));
#pragma warning disable 618
            _stepInitData = (StepInitData) EditorGUILayout.ObjectField(_stepInitData, typeof(StepInitData));
#pragma warning restore 618

            if (_stepInitData != null)
            {
                if (GUILayout.Button("保存数据", GUILayout.MaxWidth(60)))
                {
                    SaveData();
                }
            }

            EditorGUILayout.EndHorizontal();

            #endregion

            if (_stepInitData != null)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("增加"))
                {
                    _stepInitData.stepInitDataInfoGroups.Add(new StepInitDataInfo());
                }

                EditorGUILayout.EndHorizontal();
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

                for (int i = 0; i < _stepInitData.stepInitDataInfoGroups.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("步骤名称", GUILayout.MaxWidth(50));
                    _stepInitData.stepInitDataInfoGroups[i].stepName = EditorGUILayout.TextField(_stepInitData.stepInitDataInfoGroups[i].stepName);
                    EditorGUILayout.LabelField("大步骤", GUILayout.MaxWidth(50));
                    _stepInitData.stepInitDataInfoGroups[i].bigIndex = EditorGUILayout.IntField(_stepInitData.stepInitDataInfoGroups[i].bigIndex);
                    EditorGUILayout.LabelField("小步骤", GUILayout.MaxWidth(50));
                    _stepInitData.stepInitDataInfoGroups[i].smallIndex = EditorGUILayout.IntField(_stepInitData.stepInitDataInfoGroups[i].smallIndex);
                    _stepInitData.stepInitDataInfoGroups[i].isPlayTip = GUILayout.Toggle(_stepInitData.stepInitDataInfoGroups[i].isPlayTip, "播放提示", GUILayout.MaxWidth(70));
                    _stepInitData.stepInitDataInfoGroups[i].showTip = GUILayout.Toggle(_stepInitData.stepInitDataInfoGroups[i].showTip, "显示提示", GUILayout.MaxWidth(70));
                    EditorGUILayout.LabelField("提示索引", GUILayout.MaxWidth(50));
                    _stepInitData.stepInitDataInfoGroups[i].tipIndex = EditorGUILayout.IntField(_stepInitData.stepInitDataInfoGroups[i].tipIndex);
                    EditorGUILayout.LabelField("提示事件", GUILayout.MaxWidth(50));
                    _stepInitData.stepInitDataInfoGroups[i].tipPlayOverEvent = (ListenerEventType) EditorGUILayout.EnumPopup(_stepInitData.stepInitDataInfoGroups[i].tipPlayOverEvent);
                    _stepInitData.stepInitDataInfoGroups[i].propGroup = GUILayout.Toggle(_stepInitData.stepInitDataInfoGroups[i].propGroup, "物品", GUILayout.MaxWidth(70));
                    _stepInitData.stepInitDataInfoGroups[i].cameraMove = GUILayout.Toggle(_stepInitData.stepInitDataInfoGroups[i].cameraMove, "相机移动", GUILayout.MaxWidth(70));
                    _stepInitData.stepInitDataInfoGroups[i].cameraPosType =
                        (CameraPosType) EditorGUILayout.EnumPopup(_stepInitData.stepInitDataInfoGroups[i].cameraPosType);
                    _stepInitData.stepInitDataInfoGroups[i].isPlayAnim = GUILayout.Toggle(_stepInitData.stepInitDataInfoGroups[i].isPlayAnim, "播放动画", GUILayout.MaxWidth(70));
                    EditorGUILayout.LabelField("动画类型", GUILayout.MaxWidth(50));
                    _stepInitData.stepInitDataInfoGroups[i].animType = (AnimType) EditorGUILayout.EnumPopup(_stepInitData.stepInitDataInfoGroups[i].animType);
                    EditorGUILayout.LabelField("动画进度", GUILayout.MaxWidth(50));
                    _stepInitData.stepInitDataInfoGroups[i].animSpeedProgress =
                        (AnimSpeedProgress) EditorGUILayout.EnumPopup(_stepInitData.stepInitDataInfoGroups[i].animSpeedProgress);
                    EditorGUILayout.LabelField("动画事件", GUILayout.MaxWidth(50));
                    _stepInitData.stepInitDataInfoGroups[i].animPlayOverEvent = (ListenerEventType) EditorGUILayout.EnumPopup(_stepInitData.stepInitDataInfoGroups[i].animPlayOverEvent);

                    if (GUILayout.Button("上移", GUILayout.MaxWidth(80)))
                    {
                        if (i > 0)
                        {
                            StepInitDataInfo tempStepInitDataInfo = _stepInitData.stepInitDataInfoGroups[i];
                            _stepInitData.stepInitDataInfoGroups[i] = _stepInitData.stepInitDataInfoGroups[i - 1];
                            _stepInitData.stepInitDataInfoGroups[i - 1] = tempStepInitDataInfo;
                        }
                    }

                    if (GUILayout.Button("下移", GUILayout.MaxWidth(80)))
                    {
                        if (i < _stepInitData.stepInitDataInfoGroups.Count - 1)
                        {
                            StepInitDataInfo tempStepInitDataInfo = _stepInitData.stepInitDataInfoGroups[i];
                            _stepInitData.stepInitDataInfoGroups[i] = _stepInitData.stepInitDataInfoGroups[i + 1];
                            _stepInitData.stepInitDataInfoGroups[i + 1] = tempStepInitDataInfo;
                        }
                    }

                    if (GUILayout.Button("删除步骤", GUILayout.MaxWidth(80)))
                    {
                        _stepInitData.stepInitDataInfoGroups.RemoveAt(i);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndScrollView();

                EditorUtility.SetDirty(_stepInitData);
            }
        }
    }
}