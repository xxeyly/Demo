using System;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel
{
    public class CustomAudio : EditorWindow
    {
        [MenuItem("XFrame/音频工具")]
        private static void ShowWindow()
        {
            EditorWindow window = EditorWindow.GetWindow<CustomAudio>();
            window.minSize = new Vector2(900, 300);
            window.maxSize = new Vector2(1600, 900);
            window.titleContent = new GUIContent() {image = null, text = "音频工具"};
            window.Show();
        }


        AudioData _audioData;
        Vector2 _scrollPos = Vector2.zero;

        private void OnEnable()
        {
            if (_audioData == null)
            {
                _audioData = (AudioData) AssetDatabase.LoadAssetAtPath("Assets/XxSlitFrame/Config/AudioData.asset", typeof(AudioData));
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
            EditorUtility.SetDirty(_audioData);
            // 保存所有修改
            AssetDatabase.SaveAssets();
        }

        public void OnGUI()
        {
            #region 配置文件

            EditorGUILayout.BeginHorizontal();
            //自定义枚举下拉框
            EditorGUILayout.LabelField("音频配置数据:", GUILayout.MaxWidth(80));
#pragma warning disable 618
            _audioData = (AudioData) EditorGUILayout.ObjectField(_audioData, typeof(AudioData));
#pragma warning restore 618
            if (_audioData != null)
            {
                if (GUILayout.Button("保存数据", GUILayout.MaxWidth(60)))
                {
                    SaveData();
                }
            }


            EditorGUILayout.EndHorizontal();

            #endregion

            if (_audioData != null)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("增加"))
                {
                    _audioData.audioDataInfos.Add(new AudioData.AudioDataInfo());
                }

                EditorGUILayout.EndHorizontal();
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

                for (int i = 0; i < _audioData.audioDataInfos.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("音频类型", GUILayout.MaxWidth(50));
                    _audioData.audioDataInfos[i].audioType = (AudioData.AudioType) EditorGUILayout.EnumPopup(_audioData.audioDataInfos[i].audioType, GUILayout.MaxWidth(150));
                    EditorGUILayout.LabelField("对应音频", GUILayout.MaxWidth(50));
#pragma warning disable 618
                    _audioData.audioDataInfos[i].dialogueAudioClip = (AudioClip) EditorGUILayout.ObjectField(_audioData.audioDataInfos[i].dialogueAudioClip, typeof(AudioClip));
#pragma warning restore 618
                    if (GUILayout.Button("删除音频", GUILayout.MaxWidth(80)))
                    {
                        _audioData.audioDataInfos.RemoveAt(i);
                    }

                    EditorUtility.SetDirty(_audioData);
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndScrollView();
            }
        }
    }
}