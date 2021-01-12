using UnityEditor;
using UnityEngine;
using XAnimator.Base;
using XxSlitFrame.Tools.ConfigData;
using XxSlitFrame.Tools.General;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel
{
    public class CustomDialog : EditorWindow
    {
        private DialogueData _dialogueData;
        Vector2 _scrollPos = Vector2.zero;


        [MenuItem("XFrame/对话工具")]
        private static void ShowWindow()
        {
            EditorWindow window = EditorWindow.GetWindow<CustomDialog>();
            window.minSize = new Vector2(900, 300);
            window.maxSize = new Vector2(1600, 900);
            window.titleContent = new GUIContent() {image = null, text = "对话工具"};
            window.Show();
        }

        private void OnEnable()
        {
            if (_dialogueData == null)
            {
                _dialogueData = (DialogueData) AssetDatabase.LoadAssetAtPath("Assets/XxSlitFrame/Config/DialogueData.asset", typeof(DialogueData));
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
            EditorUtility.SetDirty(_dialogueData);
            // 保存所有修改
            AssetDatabase.SaveAssets();
        }

        public void OnGUI()
        {
            #region 配置文件

            EditorGUILayout.BeginHorizontal();
            //自定义枚举下拉框
            EditorGUILayout.LabelField("对话配置数据:", GUILayout.MaxWidth(80));
#pragma warning disable 618
            _dialogueData = (DialogueData) EditorGUILayout.ObjectField(_dialogueData, typeof(DialogueData));
#pragma warning restore 618

            if (_dialogueData != null)
            {
                if (GUILayout.Button("保存数据", GUILayout.MaxWidth(60)))
                {
                    SaveData();
                }
            }


            EditorGUILayout.EndHorizontal();

            #endregion

            EditorGUILayout.BeginHorizontal();


            if (_dialogueData != null)
            {
                if (GUILayout.Button("增加对话组"))
                {
                    _dialogueData.dataInfos.Add(new DialogDataInfoContainer());
                }

                EditorGUILayout.EndHorizontal();
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

                for (int i = 0; i < _dialogueData.dataInfos.Count; i++)
                {
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField("对话组:" + i, GUILayout.MaxWidth(50));

                    EditorGUILayout.LabelField("对话结束事件", GUILayout.MaxWidth(70));
                    _dialogueData.dataInfos[i].endEvent =
                        (ListenerEventType) EditorGUILayout.EnumPopup(_dialogueData.dataInfos[i].endEvent, GUILayout.MaxWidth(100));
                    if (GUILayout.Button("增加对话片段", GUILayout.MaxWidth(120)))
                    {
                        _dialogueData.dataInfos[i].dialogDataInfos.Add(new DialogDataInfo());
                    }


                    if (GUILayout.Button("删除对话片段", GUILayout.MaxWidth(120)))
                    {
                        if (_dialogueData.dataInfos[i].dialogDataInfos.Count > 0)
                        {
                            _dialogueData.dataInfos[i].dialogDataInfos.RemoveAt(_dialogueData.dataInfos[i].dialogDataInfos.Count - 1);
                        }
                    }

                    if (GUILayout.Button("删除对话组", GUILayout.MaxWidth(120)))
                    {
                        _dialogueData.dataInfos.RemoveAt(i);
                        break;
                    }

                    EditorGUILayout.EndHorizontal();

                    for (int j = 0; j < _dialogueData.dataInfos[i].dialogDataInfos.Count; j++)
                    {
                        EditorGUILayout.BeginVertical();
                        EditorGUILayout.BeginHorizontal();

                        EditorGUILayout.LabelField("对话角色", GUILayout.MaxWidth(50));
                        _dialogueData.dataInfos[i].dialogDataInfos[j].role =
                            (Role) EditorGUILayout.EnumPopup(_dialogueData.dataInfos[i].dialogDataInfos[j].role, GUILayout.MaxWidth(80));
                        EditorGUILayout.LabelField("对话长度", GUILayout.MaxWidth(50));
                        _dialogueData.dataInfos[i].dialogDataInfos[j].length =
                            (Length) EditorGUILayout.EnumPopup(_dialogueData.dataInfos[i].dialogDataInfos[j].length, GUILayout.MaxWidth(80));
                        EditorGUILayout.LabelField("对话内容:", GUILayout.MaxWidth(60));

                        _dialogueData.dataInfos[i].dialogDataInfos[j].dialogContent = EditorGUILayout.TextField(_dialogueData.dataInfos[i].dialogDataInfos[j].dialogContent);

                        EditorGUILayout.LabelField("对话动画", GUILayout.MaxWidth(50));
                        _dialogueData.dataInfos[i].dialogDataInfos[j].animType =
                            (AnimType) EditorGUILayout.EnumPopup(_dialogueData.dataInfos[i].dialogDataInfos[j].animType, GUILayout.MaxWidth(100));
                        EditorGUILayout.LabelField("对话事件", GUILayout.MaxWidth(50));
                        _dialogueData.dataInfos[i].dialogDataInfos[j].dialogClipEvent =
                            (ListenerEventType) EditorGUILayout.EnumPopup(_dialogueData.dataInfos[i].dialogDataInfos[j].dialogClipEvent, GUILayout.MaxWidth(100));
                        EditorGUILayout.LabelField("对话音频", GUILayout.MaxWidth(50));
                        _dialogueData.dataInfos[i].dialogDataInfos[j].dialogueAudioClip =
#pragma warning disable 618
                            (AudioClip) EditorGUILayout.ObjectField(_dialogueData.dataInfos[i].dialogDataInfos[j].dialogueAudioClip, typeof(AudioClip));
#pragma warning restore 618
                        if (GUILayout.Button("增加", GUILayout.MaxWidth(80)))
                        {
                            _dialogueData.dataInfos[i].dialogDataInfos.Insert(j + 1, new DialogDataInfo());
                        }

                        if (GUILayout.Button("删除", GUILayout.MaxWidth(80)))
                        {
                            _dialogueData.dataInfos[i].dialogDataInfos.RemoveAt(j);
                        }

                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.EndVertical();
                    }

                    EditorGUILayout.EndVertical();
                }

                EditorGUILayout.EndScrollView();

                EditorUtility.SetDirty(_dialogueData);
            }
        }
    }
}