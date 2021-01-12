using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.View
{
    /// <summary>
    /// 生成脚本
    /// </summary>
    public class GenerationBaseWindow : MonoBehaviour
    {
        [Header("生成脚本相对路径")] public string generationRelativePath;

        [Header("脚本名字")] [SerializeField] public string scriptsName;

        [Header("脚本命名空间")] public string generationScriptsNamespace;
        [Header("脚本替换")] public bool replaceSameScript;

        //生成脚本的绝对路径
        private string _generationAbsolutelyPath;
        private Type _generationBaseWindowType;

        public void Init()
        {
            InitData();
        }


        /// <summary>
        /// 生成脚本
        /// </summary>
        public void Generation()
        {
            if (!string.IsNullOrEmpty(this.scriptsName) && !string.IsNullOrEmpty(generationRelativePath))
            {
                //脚本引入
                string generationScriptsUsing =
                    "using UnityEngine;\n" +
                    "using UnityEngine.EventSystems;\n" +
                    "using UnityEngine.UI;\n" +
                    "using XxSlitFrame.View;\n\n";
                //脚本空间
                string generationScriptsSpace = "namespace " + generationScriptsNamespace + "\n{\n ";
                string generationScriptsName = "   public class " + scriptsName + " : BaseWindow\n    {\n";
                string generationScriptsDeclarationUi = OneClickDeclarationUi() + "\n";
                string generationScriptsInit = "        public override void Init()\n        {\n        }\n\n";
                string generationScriptsInitView = "        protected override void InitView()\n        {\n" + OneClickBindUi() + "        }\n\n";
                string generationScriptsInitListener = "        protected override void InitListener()\n        {\n" + OneClickBindListener() + "        }\n\n";
                string generationScriptsListenerMethod = OneClickStatementListener();
                string generationScriptsEnding = "    }\n}";

                string generationScriptsContent = generationScriptsUsing + generationScriptsSpace + generationScriptsName + generationScriptsDeclarationUi + generationScriptsInit
                                                  + generationScriptsInitView + generationScriptsInitListener + generationScriptsListenerMethod + generationScriptsEnding;
                if (replaceSameScript)
                {
                    if (File.Exists(_generationAbsolutelyPath + "/" + scriptsName + ".cs"))
                    {
                        File.Delete(_generationAbsolutelyPath + "/" + scriptsName + ".cs");
                    }

                    ResSvc.FileOperation.SaveTextToLoad(_generationAbsolutelyPath, scriptsName + ".cs", generationScriptsContent);
                }
                else
                {
                    if (!File.Exists(_generationAbsolutelyPath + "/" + scriptsName + ".cs"))
                    {
                        ResSvc.FileOperation.SaveTextToLoad(_generationAbsolutelyPath, scriptsName + ".cs", generationScriptsContent);
                    }
                    else
                    {
                        Debug.LogError("相同文件夹下存在相同的脚本");
                    }
                }
            }
        }

        /// <summary>
        /// 添加脚本
        /// </summary>
        public void AddComponent()
        {
            GetGenerationBaseWindowType(generationScriptsNamespace + "." + scriptsName);
            if (_generationBaseWindowType != null && !gameObject.GetComponent(_generationBaseWindowType))
            {
                gameObject.AddComponent(_generationBaseWindowType);
            }

            _generationBaseWindowType = null;
        }

        /// <summary>
        /// 获得生成的类型
        /// </summary>
        /// <param name="scriptsName"></param>
        /// <returns></returns>
        private void GetGenerationBaseWindowType(string scriptsName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var defaultAssemblies = assemblies.First(assembly => assembly.GetName().Name == "Assembly-CSharp");
            var type = defaultAssemblies.GetType(scriptsName);
            _generationBaseWindowType = type;
        }

        /// <summary>
        /// 生成路径
        /// </summary>
        public void InitData()
        {
            generationRelativePath = "Assets" + "/Scripts/" + SceneManager.GetActiveScene().name;
            _generationAbsolutelyPath = Application.dataPath + generationRelativePath.Remove(0, 6);
            scriptsName = gameObject.name;
            if (generationRelativePath.Contains("Assets/Scripts"))
            {
                if (generationRelativePath.Length > 14)
                {
                    generationScriptsNamespace = generationRelativePath.Remove(0, 15).Replace("/", ".");
                }
            }
        }

        #region UI绑定

        /// <summary>
        /// 一键获得绑定UI
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private string OneClickDeclarationUi()
        {
            Transform window = transform.Find("Window").transform;
            string allUiName = "";
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                if (child.GetComponent<BindUiType>())
                {
                    switch (child.GetComponent<BindUiType>().type)
                    {
                        case BindUiType.UiType.GameObject:
                            allUiName += "private GameObject _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.Button:
                            allUiName += "private Button _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.Image:
                            allUiName += "private Image _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.Text:
                            allUiName += "private Text _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.Toggle:
                            allUiName += "private Toggle _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.RawImage:
                            allUiName += "private RawImage _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.Scrollbar:
                            allUiName += "private Scrollbar _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.ScrollRect:
                            allUiName += "private ScrollRect _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.InputField:
                            allUiName += "private InputField _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                        case BindUiType.UiType.Dropdown:
                            allUiName += "private Dropdown _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            break;
                    }
                }
            }

            TextEditor textEditor = new TextEditor
            {
                text = allUiName
            };
            textEditor.OnFocus();
            textEditor.Copy();
            return allUiName;
        }

        /// <summary>
        /// 一键绑定UI
        /// </summary>
        /// <returns></returns>
        private string OneClickBindUi()
        {
            Transform window = transform.Find("Window").transform;
            string allBindName = "";
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                if (child.GetComponent<BindUiType>())
                {
                    allBindName += "            BindUi(ref _" + DataSvc.FirstCharToLower(child.name) + ", \"" +
                                   GetUiComponentPath(child, "") + "\");" + "\n";
                }
            }

            TextEditor textEditor = new TextEditor
            {
                text = allBindName
            };
            textEditor.OnFocus();
            textEditor.Copy();
            return allBindName;
        }

        /// <summary>
        /// 获得UI路径
        /// </summary>
        /// <returns></returns>
        private static string GetUiComponentPath(Transform uiTr, string uiPath)
        {
            Transform defaultUiTr = uiTr;
            int hierarchy = 0;
            while (uiTr.parent.name != "Window")
            {
                hierarchy++;
                uiTr = uiTr.parent;
            }

            for (int i = 1; i <= hierarchy; i++)
            {
                uiTr = GetParentByHierarchy(defaultUiTr, i);
                uiPath = uiTr.name + "/" + uiPath;
            }

            return uiPath + defaultUiTr.name;
        }

        /// <summary>
        /// 根据UI层级获得父物体
        /// </summary>
        /// <param name="uiTr"></param>
        /// <param name="hierarchy"></param>
        /// <returns></returns>
        private static Transform GetParentByHierarchy(Transform uiTr, int hierarchy)
        {
            for (int i = 0; i < hierarchy; i++)
            {
                uiTr = uiTr.parent;
            }

            return uiTr;
        }

        /// <summary>
        /// 一键绑定UI事件
        /// </summary>
        /// <returns></returns>
        private string OneClickBindListener()
        {
            Transform window = transform.Find("Window").transform;
            string allBindName = "";
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                if (child.GetComponent<BindUiType>())
                {
                    if (child.GetComponent<BindUiType>().type == BindUiType.UiType.Button)
                    {
                        allBindName += "            BindListener(_" + DataSvc.FirstCharToLower(child.name) + ", " + "EventTriggerType.PointerClick" + ", " + "On" + child.name +
                                       ");" + "\n";
                    }
                  
                }
            }

            TextEditor textEditor = new TextEditor
            {
                text = allBindName
            };
            textEditor.OnFocus();
            textEditor.Copy();
            return allBindName;
        }

        /// <summary>
        /// 一键声明UI事件
        /// </summary>
        /// <returns></returns>
        private string OneClickStatementListener()
        {
            Transform window = transform.Find("Window").transform;
            string allBindName = "";
            List<BindUiType> listener = new List<BindUiType>(window.GetComponentsInChildren<BindUiType>(true));
            for (int i = 0; i < GetTriggerTypes(listener).Count; i++)
            {
                if (i != GetTriggerTypes(listener).Count - 1)
                {
                    if (listener[i].GetComponent<BindUiType>().type == BindUiType.UiType.Button)
                    {
                        allBindName += "        private void On" + listener[i].name + "(BaseEventData targetObj)" + "\n" + "        {" + "\n" + "        }\n\n";
                    }
                 
                }
                else
                {
                    if (listener[i].GetComponent<BindUiType>().type == BindUiType.UiType.Button)
                    {
                        allBindName += "        private void On" + listener[i].name + "(BaseEventData targetObj)" + "\n" + "        {" + "\n" + "        }\n";
                    }
                }
            }

            TextEditor textEditor = new TextEditor
            {
                text = allBindName
            };
            textEditor.OnFocus();
            textEditor.Copy();
            return allBindName;
        }

        /// <summary>
        /// 获得触发事件
        /// </summary>
        /// <returns></returns>
        private List<BindUiType> GetTriggerTypes(List<BindUiType> listener)
        {
            for (int i = 0; i < listener.Count; i++)
            {
                if (listener[i].GetComponent<BindUiType>())
                {
                    if (listener[i].GetComponent<BindUiType>().type != BindUiType.UiType.Button)
                    {
                        listener.Remove(listener[i]);
                    }
                }
            }

            return listener;
        }

        #endregion
    }
}