using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData.Editor;
using XxSlitFrame.Tools.Svc;
using XxSlitFrame.View;

namespace XxSlitFrame.Tools.Editor.ConfigBaseWindowEditor
{
    [CustomEditor(typeof(AutoBindBaseWindowUIData))]
    public class ConfigBaseWindowEditor : UnityEditor.Editor
    {
        private AutoBindBaseWindowUIData _autoBindBaseWindowUiData;
        private bool _viewDeclarationUi;

        private string _usingContent;
        private string _variableContent;
        private string _variableBindPathContent;
        private string _variableBindListenerContent;
        private string _variableBindListenerEventContent;

        private string _usingContentStartMark = "//StartUsing";
        private string _usingContentEndMark = "//EndUsing";

        private string _variableContentStartMark = "//StartVariable";
        private string _variableContentEndMark = "//EndVariable";

        private string _variableBindPathContentStartMark = "//StartVariableBindPath";
        private string _variableBindPathContentEndMark = "//EndVariableBindPath";

        private string _variableBindListenerContentStartMark = "//StartVariableBindListener";
        private string _variableBindListenerContentEndMark = "//EndVariableBindListener";

        private string _variableBindEventStartMark = "//StartVariableBindEvent";
        private string _variableBindEventEndMark = "//EndVariableBindEvent";


        /// <summary>
        /// UI生成
        /// </summary>
        private void UIGenerate()
        {
            _viewDeclarationUi = true;
            _autoBindBaseWindowUiData.OneClickDeclarationUi();
            _autoBindBaseWindowUiData.OneClickBindUi();
            _autoBindBaseWindowUiData.OneClickBindListener();
            _autoBindBaseWindowUiData.OneClickStatementListener();
            _usingContent = "";
            for (int i = 0; i < _autoBindBaseWindowUiData.allUiVariableUsing.Count; i++)
            {
                _usingContent += _autoBindBaseWindowUiData.allUiVariableUsing[i];
            }

            _variableContent = "";
            for (int i = 0; i < _autoBindBaseWindowUiData.allUiVariableName.Count; i++)
            {
                _variableContent += _autoBindBaseWindowUiData.allUiVariableName[i];
            }

            _variableBindPathContent = String.Empty;
            for (int i = 0; i < _autoBindBaseWindowUiData.allUiVariableBind.Count; i++)
            {
                _variableBindPathContent += _autoBindBaseWindowUiData.allUiVariableBind[i];
            }

            _variableBindListenerContent = String.Empty;
            for (int i = 0; i < _autoBindBaseWindowUiData.allUiVariableBindListener.Count; i++)
            {
                if (_autoBindBaseWindowUiData.allUiVariableBindListener[i] != string.Empty)
                {
                    _variableBindListenerContent +=
                        _autoBindBaseWindowUiData.allUiVariableBindListener[i];
                }
            }
            _variableBindListenerEventContent = String.Empty;
            for (int i = 0; i < _autoBindBaseWindowUiData.allUiVariableBindListenerEvent.Count; i++)
            {
                if (_autoBindBaseWindowUiData.allUiVariableBindListenerEvent[i] != string.Empty)
                {
                    _variableBindListenerEventContent +=
                        _autoBindBaseWindowUiData.allUiVariableBindListenerEvent[i];
                }
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _autoBindBaseWindowUiData = (AutoBindBaseWindowUIData) target;
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("UI引用"))
            {
                UIGenerate();
                string usingContent = "";
                for (int i = 0; i < _autoBindBaseWindowUiData.allUiVariableUsing.Count; i++)
                {
                    usingContent += _autoBindBaseWindowUiData.allUiVariableUsing[i];
                }

                TextEditor textEditor = new TextEditor
                {
                    text = usingContent
                };
                textEditor.OnFocus();
                textEditor.Copy();
            }

            if (GUILayout.Button("UI变量"))
            {
                UIGenerate();
                string variableContent = "";
                for (int i = 0; i < _autoBindBaseWindowUiData.allUiVariableName.Count; i++)
                {
                    variableContent += _autoBindBaseWindowUiData.allUiVariableName[i];
                }

                TextEditor textEditor = new TextEditor
                {
                    text = variableContent
                };
                textEditor.OnFocus();
                textEditor.Copy();
            }

            if (GUILayout.Button("UI地址"))
            {
                UIGenerate();
                string currentContent = "";
                for (int i = 0; i < _autoBindBaseWindowUiData.allUiVariableBind.Count; i++)
                {
                    currentContent += _autoBindBaseWindowUiData.allUiVariableBind[i];
                }

                TextEditor textEditor = new TextEditor
                {
                    text = currentContent
                };
                textEditor.OnFocus();
                textEditor.Copy();
            }

            if (GUILayout.Button("UI事件"))
            {
                UIGenerate();

                string currentContent = "";
                for (int i = 0; i < _autoBindBaseWindowUiData.allUiVariableBindListener.Count; i++)
                {
                    currentContent += _autoBindBaseWindowUiData.allUiVariableBindListener[i];
                }

                TextEditor textEditor = new TextEditor
                {
                    text = currentContent
                };
                textEditor.OnFocus();
                textEditor.Copy();
            }

            if (GUILayout.Button("UI触发"))
            {
                UIGenerate();

                string currentContent = "";
                for (int i = 0; i < _autoBindBaseWindowUiData.allUiVariableBindListenerEvent.Count; i++)
                {
                    currentContent += _autoBindBaseWindowUiData.allUiVariableBindListenerEvent[i];
                }

                TextEditor textEditor = new TextEditor
                {
                    text = currentContent
                };
                textEditor.OnFocus();
                textEditor.Copy();
            }

            if (GUILayout.Button("UI生成"))
            {
                UIGenerate();

                //面板上有BaseWindow脚本
                if (_autoBindBaseWindowUiData.GetComponent<BaseWindow>())
                {
                    Type scriptType = _autoBindBaseWindowUiData.GetComponent<BaseWindow>().GetType();
                    string[] scriptNameSplit = scriptType.ToString().Split(new char[] {'.'});
                    string scriptName = scriptNameSplit[scriptNameSplit.Length - 1];
                    string scriptPath = GetPath(scriptName);
                    string oldScriptContent = ResSvc.FileOperation.GetTextToLoad(scriptPath);
                    string scriptContent = String.Empty;
                    scriptContent = ReplaceScriptContent(oldScriptContent, _usingContent, _usingContentStartMark,
                        _usingContentEndMark, "namespace", 0, false);
                    scriptContent = ReplaceScriptContent(scriptContent, _variableContent, _variableContentStartMark,
                        _variableContentEndMark, "BaseWindow", 17, true);

                    scriptContent = ReplaceScriptContent(scriptContent, _variableBindPathContent,
                        _variableBindPathContentStartMark,
                        _variableBindPathContentEndMark, "InitView()", 21, true);
                    if (_variableBindListenerContent != String.Empty)
                    {
                        scriptContent = ReplaceScriptContent(scriptContent, _variableBindListenerContent,
                            _variableBindListenerContentStartMark,
                            _variableBindListenerContentEndMark, "InitListener()",
                            25, true);
                    }


                    if (_variableBindListenerEventContent != String.Empty)
                    {
                        scriptContent = ReplaceScriptContent(scriptContent, _variableBindListenerEventContent,
                            _variableBindEventStartMark,
                            _variableBindEventEndMark, _variableBindListenerContentEndMark,
                            (_variableBindListenerContentEndMark).Length + 12, false);
                    }

                    scriptContent = Encoding.UTF8.GetString(Encoding.Default.GetBytes(scriptContent));

                    if (oldScriptContent != scriptContent)
                    {
                        ResSvc.FileOperation.SaveTextToLoad(scriptPath, scriptContent);
                    }
                }
            }


            EditorGUILayout.EndHorizontal();
            if (_viewDeclarationUi)
            {
                for (int i = 0; i < _autoBindBaseWindowUiData.allUiVariableName.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal(new GUIStyle("ObjectPickerBackground"), GUILayout.MaxHeight(25));
                    EditorGUILayout.LabelField(_autoBindBaseWindowUiData.allUiVariableViewName[i],
                        GUILayout.MaxHeight(25));
                    if (GUILayout.Button("属性", GUILayout.MinWidth(40), GUILayout.MaxWidth(40)))
                    {
                        TextEditor textEditor = new TextEditor
                        {
                            text = _autoBindBaseWindowUiData.allUiVariableName[i]
                        };
                        textEditor.OnFocus();
                        textEditor.Copy();
                    }

                    if (GUILayout.Button("地址", GUILayout.MinWidth(40), GUILayout.MaxWidth(40)))
                    {
                        TextEditor textEditor = new TextEditor
                        {
                            text = _autoBindBaseWindowUiData.allUiVariableBind[i]
                        };
                        textEditor.OnFocus();
                        textEditor.Copy();
                    }

                    if (_autoBindBaseWindowUiData.allUiVariableBindListener[i] != string.Empty)
                    {
                        if (GUILayout.Button("绑定", GUILayout.MinWidth(40), GUILayout.MaxWidth(40)))
                        {
                            TextEditor textEditor = new TextEditor
                            {
                                text = _autoBindBaseWindowUiData.allUiVariableBindListener[i]
                            };
                            textEditor.OnFocus();
                            textEditor.Copy();
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("空", GUILayout.MinWidth(40), GUILayout.MaxWidth(40)))
                        {
                        }
                    }

                    if (_autoBindBaseWindowUiData.allUiVariableBindListener[i] != string.Empty)
                    {
                        if (GUILayout.Button("事件", GUILayout.MinWidth(40), GUILayout.MaxWidth(40)))
                        {
                            TextEditor textEditor = new TextEditor
                            {
                                text = _autoBindBaseWindowUiData.allUiVariableBindListenerEvent[i]
                            };
                            textEditor.OnFocus();
                            textEditor.Copy();
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("空", GUILayout.MinWidth(40), GUILayout.MaxWidth(40)))
                        {
                        }
                    }


                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        /// <summary>
        /// 获得脚本路径
        /// </summary>
        /// <param name="_scriptName"></param>
        /// <returns></returns>
        static string GetPath(string _scriptName)
        {
            string[] path = UnityEditor.AssetDatabase.FindAssets(_scriptName);

            for (int i = 0; i < path.Length; i++)
            {
                if (AssetDatabase.GUIDToAssetPath(path[i]).Contains("Assets") &&
                    AssetDatabase.GUIDToAssetPath(path[i]).Contains(_scriptName + ".cs"))
                {
                    return AssetDatabase.GUIDToAssetPath(path[i]);
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptsContent">脚本内容</param>
        /// <param name="insertContent">插入内容</param>
        /// <param name="insertStartMark">插入开始标记</param>
        /// <param name="insertEndMark">插入结束标记</param>
        /// <param name="insertMarkContent">识别插入标记内容</param>
        /// <param name="insertOffset">插入偏移</param>
        /// <param name="replace">替换</param>
        /// <returns></returns>
        private string ReplaceScriptContent(string scriptsContent, string insertContent,
            string insertStartMark, string insertEndMark, string insertMarkContent, int insertOffset, bool replace)
        {
            //查找是否生成过
            if (scriptsContent.Contains(insertStartMark) && scriptsContent.Contains(insertEndMark))
            {
                if (replace)
                {
                    //开始位置
                    int usingStartIndex = scriptsContent.IndexOf(insertStartMark, StringComparison.Ordinal) +
                                          insertStartMark.Length;
                    //结束位置
                    int usingEndIndex = scriptsContent.IndexOf(insertEndMark, StringComparison.Ordinal);

                    //要替换的内容
                    string scriptUsingContent = String.Empty;
                    for (int i = 0; i < scriptsContent.Length; i++)
                    {
                        if (i > usingStartIndex && i < usingEndIndex)
                        {
                            scriptUsingContent += scriptsContent[i];
                        }
                    }

                    //替换新内容
                    scriptUsingContent = scriptsContent.Replace(scriptUsingContent, insertContent);
                    return scriptUsingContent;
                }

                {
                    return scriptsContent;
                }
            }
            else
            {
                int scriptsInsertIndex =
                    scriptsContent.IndexOf(insertMarkContent, StringComparison.Ordinal) + insertOffset;
                insertContent = "\n" + insertStartMark + "\n" + insertContent + insertEndMark + "\n";
                scriptsContent = scriptsContent.Insert(scriptsInsertIndex, insertContent);
                return scriptsContent;
            }
        }

        /// <summary>
        /// 返回命名空间
        /// </summary>
        /// <param name="scriptNameSplit"></param>
        /// <returns></returns>
        private string GetScriptNamespace(string[] scriptNameSplit)
        {
            string scriptsNameSpace = String.Empty;
            if (scriptNameSplit.Length > 1)
            {
                for (int i = 0; i < scriptNameSplit.Length - 1; i++)
                {
                    scriptsNameSpace += scriptNameSplit[i];
                }
            }

            return scriptsNameSpace;
        }
    }
}