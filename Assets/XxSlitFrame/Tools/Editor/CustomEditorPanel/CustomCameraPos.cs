using System;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData;
using XxSlitFrame.Tools.General;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel
{
    public class CustomCameraPos : EditorWindow
    {
        CameraPosData _cameraPosData;
        Vector2 _scrollPos = Vector2.zero;

        [MenuItem("XFrame/相机位置")]
        private static void ShowWindow()
        {
            EditorWindow window = EditorWindow.GetWindow<CustomCameraPos>();
            window.minSize = new Vector2(1000, 300);
            window.maxSize = new Vector2(1000, 100);
            window.titleContent = new GUIContent() {image = null, text = "相机位置"};
            window.Show();
        }
        private void OnEnable()
        {
            if (_cameraPosData == null)
            {
                _cameraPosData = (CameraPosData) AssetDatabase.LoadAssetAtPath("Assets/XxSlitFrame/Config/CameraPosData.asset", typeof(CameraPosData));
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
            EditorUtility.SetDirty(_cameraPosData);
            // 保存所有修改
            AssetDatabase.SaveAssets();
        }

        public void OnGUI()
        {
            #region 配置文件

            EditorGUILayout.BeginHorizontal();
            //自定义枚举下拉框
            EditorGUILayout.LabelField("相机配置数据:", GUILayout.MaxWidth(80));
#pragma warning disable 618
            _cameraPosData = (CameraPosData) EditorGUILayout.ObjectField(_cameraPosData, typeof(CameraPosData));
#pragma warning restore 618
            if (_cameraPosData != null)
            {
                if (GUILayout.Button("保存数据", GUILayout.MaxWidth(60)))
                {
                   SaveData();
                }
            }


            EditorGUILayout.EndHorizontal();

            #endregion

            if (_cameraPosData != null)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("增加相机位置"))
                {
                    _cameraPosData.cameraPosInfosGroup.Add(new CameraPosInfo(){cameraFieldView = 60f});
                }

                EditorGUILayout.EndHorizontal();

                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

                for (int i = 0; i < _cameraPosData.cameraPosInfosGroup.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("位置信息", GUILayout.MaxWidth(50));
                    _cameraPosData.cameraPosInfosGroup[i].cameraPosType =
                        (CameraPosType) EditorGUILayout.EnumPopup(_cameraPosData.cameraPosInfosGroup[i].cameraPosType, GUILayout.MaxWidth(150));
                    EditorGUILayout.LabelField("相机深度", GUILayout.MaxWidth(50));
                    _cameraPosData.cameraPosInfosGroup[i].cameraFieldView =
                        EditorGUILayout.FloatField(_cameraPosData.cameraPosInfosGroup[i].cameraFieldView, GUILayout.MaxWidth(20));
                    EditorGUILayout.LabelField("寻路位置", GUILayout.MaxWidth(50));
                    _cameraPosData.cameraPosInfosGroup[i].navMeshAgentPos.x =
                        EditorGUILayout.FloatField(_cameraPosData.cameraPosInfosGroup[i].navMeshAgentPos.x, GUILayout.MaxWidth(60));
                    _cameraPosData.cameraPosInfosGroup[i].navMeshAgentPos.y =
                        EditorGUILayout.FloatField(_cameraPosData.cameraPosInfosGroup[i].navMeshAgentPos.y, GUILayout.MaxWidth(60));
                    _cameraPosData.cameraPosInfosGroup[i].navMeshAgentPos.z =
                        EditorGUILayout.FloatField(_cameraPosData.cameraPosInfosGroup[i].navMeshAgentPos.z, GUILayout.MaxWidth(60));

                    EditorGUILayout.LabelField("相机位置", GUILayout.MaxWidth(50));
                    _cameraPosData.cameraPosInfosGroup[i].cameraPos.x = EditorGUILayout.FloatField(_cameraPosData.cameraPosInfosGroup[i].cameraPos.x, GUILayout.MaxWidth(60));
                    _cameraPosData.cameraPosInfosGroup[i].cameraPos.y = EditorGUILayout.FloatField(_cameraPosData.cameraPosInfosGroup[i].cameraPos.y, GUILayout.MaxWidth(60));
                    _cameraPosData.cameraPosInfosGroup[i].cameraPos.z = EditorGUILayout.FloatField(_cameraPosData.cameraPosInfosGroup[i].cameraPos.z, GUILayout.MaxWidth(60));

                    EditorGUILayout.LabelField("相机旋转", GUILayout.MaxWidth(50));
                    _cameraPosData.cameraPosInfosGroup[i].cameraRot.x = EditorGUILayout.FloatField(_cameraPosData.cameraPosInfosGroup[i].cameraRot.x, GUILayout.MaxWidth(60));
                    _cameraPosData.cameraPosInfosGroup[i].cameraRot.y = EditorGUILayout.FloatField(_cameraPosData.cameraPosInfosGroup[i].cameraRot.y, GUILayout.MaxWidth(60));
                    _cameraPosData.cameraPosInfosGroup[i].cameraRot.z = EditorGUILayout.FloatField(_cameraPosData.cameraPosInfosGroup[i].cameraRot.z, GUILayout.MaxWidth(60));

                    if (GUILayout.Button("删除相机位置", GUILayout.MaxWidth(80)))
                    {
                        _cameraPosData.cameraPosInfosGroup.RemoveAt(i);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndScrollView();
            }
        }
    }
}