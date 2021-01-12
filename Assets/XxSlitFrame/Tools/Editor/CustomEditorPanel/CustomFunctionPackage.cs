using System;
using UnityEditor;
using UnityEngine;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel
{
    public class CustomFunctionPackage : EditorWindow
    {
        public string packageServerPath;
        public Vector2 packageServerScroll = Vector2.zero;
        public int packageCount;

        // [MenuItem("xxslit/工具包")]
        private static void ShowWindow()
        {
            EditorWindow window = EditorWindow.GetWindow<CustomFunctionPackage>();
            window.minSize = new Vector2(1366, 768);
            window.maxSize = new Vector2(1366, 768);
            window.titleContent = new GUIContent() {image = null, text = "动画工具"};
            window.Show();
        }


        private void OnGUI()
        {
            #region 项目名称

            packageCount = 50;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("包服务器地址:", GUILayout.MaxWidth(80));
            packageServerPath = EditorGUILayout.TextField(packageServerPath);

            if (GUILayout.Button("连接服务器", GUILayout.MaxWidth(100), GUILayout.MaxHeight(20)))
            {
                ConnectServer();
            }

            EditorGUILayout.EndHorizontal();

            packageServerScroll = EditorGUILayout.BeginScrollView(packageServerScroll, false, true);
            int a = 4;
            int b = 4;
            for (int i = 0; i < a; i++)
            {
                for (int j = 0; j < b; j++)
                {
                    var tex = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/UI/动脉穿刺-图片/跳步/mmm.png");
                    if (GUI.Button(new Rect((tex.width) * i + 100, (j * tex.height) + 20, 100, 20), "Level 2"))
                    {
                    }

                    EditorGUI.DrawTextureTransparent(new Rect((tex.width) * i - 20 + 100, (j * tex.height) + 20 + 20, tex.width - 20, tex.height - 20), tex);
                }
            }


            EditorGUILayout.EndScrollView();

            #endregion
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        private void ConnectServer()
        {
        }
    }
}