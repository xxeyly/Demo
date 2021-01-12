using System;
using System.Collections.Generic;
using UnityEngine;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.View
{
    // ReSharper disable once InconsistentNaming
    public class AutoBindBaseWindowUIData : MonoBehaviour
    {
        /// <summary>
        /// 所有UI变量引用
        /// </summary>
        [HideInInspector] public List<string> allUiVariableUsing;

        /// <summary>
        /// 所有UI变量名称
        /// </summary>
        [HideInInspector] public List<string> allUiVariableName;

        /// <summary>
        /// 所有UI变量显示名称
        /// </summary>
        [HideInInspector] public List<string> allUiVariableViewName;

        /// <summary>
        /// 所有UI变量绑定
        /// </summary>
        [HideInInspector] public List<string> allUiVariableBind;

        /// <summary>
        /// 所有UI变量绑定事件
        /// </summary>
        [HideInInspector] public List<string> allUiVariableBindListener;

        /// <summary>
        /// 所有UI变量绑定事件声明
        /// </summary>
        [HideInInspector] public List<string> allUiVariableBindListenerEvent;

        #region UI自动生成代码

        /// <summary>
        /// 获得Window界面 LocalBaseWindow从跟目录开始索引
        /// </summary>
        /// <returns></returns>
        protected virtual Transform GetWindow()
        {
            return transform.Find("Window").transform;
        }

        /// <summary>
        /// 一键获得绑定UI
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string OneClickDeclarationUi()
        {
            Transform window = GetWindow();
            string allUiName = "";
            allUiVariableName = new List<string>();
            allUiVariableViewName = new List<string>();
            allUiVariableUsing = new List<string>();
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                if (child.GetComponent<BindUiType>() && !GetUiComponentContainLocalBaseWindow(child))
                {
                    switch (child.GetComponent<BindUiType>().type)
                    {
                        case BindUiType.UiType.GameObject:
                            allUiName += "        private GameObject _" + DataSvc.FirstCharToLower(child.name) + ";" +
                                         "\n";
                            allUiVariableName.Add("        private GameObject _" +
                                                  DataSvc.FirstCharToLower(child.name) + ";" +
                                                  "\n");
                            if (!allUiVariableUsing.Contains("using UnityEngine;\n"))
                            {
                                allUiVariableUsing.Add("using UnityEngine;" + "\n");
                            }

                            break;
                        case BindUiType.UiType.Button:
                            allUiName += "        private Button _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            allUiVariableName.Add(
                                "        private Button _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n");
                            if (!allUiVariableUsing.Contains("using UnityEngine.UI;\n"))
                            {
                                allUiVariableUsing.Add("using UnityEngine.UI;" + "\n");
                            }

                            break;
                        case BindUiType.UiType.Image:
                            allUiName += "        private Image _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            allUiVariableName.Add("        private Image _" + DataSvc.FirstCharToLower(child.name) +
                                                  ";" +
                                                  "\n");
                            if (!allUiVariableUsing.Contains("using UnityEngine.UI;\n"))
                            {
                                allUiVariableUsing.Add("using UnityEngine.UI;" + "\n");
                            }

                            break;
                        case BindUiType.UiType.Text:
                            allUiName += "        private Text _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            allUiVariableName.Add("        private Text _" + DataSvc.FirstCharToLower(child.name) +
                                                  ";" + "\n");
                            if (!allUiVariableUsing.Contains("using UnityEngine.UI;\n"))
                            {
                                allUiVariableUsing.Add("using UnityEngine.UI;" + "\n");
                            }

                            break;
                        case BindUiType.UiType.Toggle:
                            allUiName += "        private Toggle _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            allUiVariableName.Add(
                                "        private Toggle _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n");
                            if (!allUiVariableUsing.Contains("using UnityEngine.UI;\n"))
                            {
                                allUiVariableUsing.Add("using UnityEngine.UI;" + "\n");
                            }

                            break;
                        case BindUiType.UiType.RawImage:
                            allUiName += "        private RawImage _" + DataSvc.FirstCharToLower(child.name) + ";" +
                                         "\n";
                            allUiVariableName.Add("        private RawImage _" + DataSvc.FirstCharToLower(child.name) +
                                                  ";" +
                                                  "\n");
                            if (!allUiVariableUsing.Contains("using UnityEngine.UI;\n"))
                            {
                                allUiVariableUsing.Add("using UnityEngine.UI;" + "\n");
                            }

                            break;
                        case BindUiType.UiType.Scrollbar:
                            allUiName += "        private Scrollbar _" + DataSvc.FirstCharToLower(child.name) + ";" +
                                         "\n";
                            allUiVariableName.Add("        private Scrollbar _" + DataSvc.FirstCharToLower(child.name) +
                                                  ";" +
                                                  "\n");
                            if (!allUiVariableUsing.Contains("using UnityEngine.UI;\n"))
                            {
                                allUiVariableUsing.Add("using UnityEngine.UI;" + "\n");
                            }

                            break;
                        case BindUiType.UiType.ScrollRect:
                            allUiName += "        private ScrollRect _" + DataSvc.FirstCharToLower(child.name) + ";" +
                                         "\n";
                            allUiVariableName.Add("        private ScrollRect _" +
                                                  DataSvc.FirstCharToLower(child.name) + ";" +
                                                  "\n");
                            if (!allUiVariableUsing.Contains("using UnityEngine.UI;\n"))
                            {
                                allUiVariableUsing.Add("using UnityEngine.UI;" + "\n");
                            }

                            break;
                        case BindUiType.UiType.InputField:
                            allUiName += "        private InputField _" + DataSvc.FirstCharToLower(child.name) + ";" +
                                         "\n";
                            allUiVariableName.Add("        private InputField _" +
                                                  DataSvc.FirstCharToLower(child.name) + ";" +
                                                  "\n");
                            if (!allUiVariableUsing.Contains("using UnityEngine.UI;\n"))
                            {
                                allUiVariableUsing.Add("using UnityEngine.UI;" + "\n");
                            }

                            break;
                        case BindUiType.UiType.Dropdown:
                            allUiName += "        private Dropdown _" + DataSvc.FirstCharToLower(child.name) + ";" +
                                         "\n";
                            allUiVariableName.Add("        private Dropdown _" + DataSvc.FirstCharToLower(child.name) +
                                                  ";" +
                                                  "\n");
                            if (!allUiVariableUsing.Contains("using UnityEngine.UI;\n"))
                            {
                                allUiVariableUsing.Add("using UnityEngine.UI;" + "\n");
                            }

                            break;
                        case BindUiType.UiType.Slider:
                            allUiName += "        private Slider _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            allUiVariableName.Add("        private Slider _" + DataSvc.FirstCharToLower(child.name) +
                                                  ";" +
                                                  "\n");
                            if (!allUiVariableUsing.Contains("using UnityEngine.UI;\n"))
                            {
                                allUiVariableUsing.Add("using UnityEngine.UI;" + "\n");
                            }

                            break;
                        case BindUiType.UiType.VideoPlayer:
                            allUiName += "        private VideoPlayer _" + DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                            allUiVariableName.Add("        private VideoPlayer _" + DataSvc.FirstCharToLower(child.name) +
                                                  ";" +
                                                  "\n");
                            if (!allUiVariableUsing.Contains("using UnityEngine.UI;\n"))
                            {
                                allUiVariableUsing.Add("using UnityEngine.UI;" + "\n");
                            }

                            break;

                        case BindUiType.UiType.ChildList:

                            string childTypeName;
                            Type childType;
                            if (child.GetComponent<BindUiType>().childType != null)
                            {
                                childType = child.GetComponent<BindUiType>().childType.GetType();
                                if (childType == typeof(LocalUIBaseWindow))
                                {
                                    childTypeName = child.GetComponentInChildren<LocalUIBaseWindow>().uiType.ToString();
                                    if (!allUiVariableUsing.Contains("using System.Collections.Generic;\n"))
                                    {
                                        allUiVariableUsing.Add("using System.Collections.Generic;" + "\n");
                                    }
                                }
                                else
                                {
                                    childTypeName = child.GetComponent<BindUiType>().childType.GetType().ToString();
                                    if (!allUiVariableUsing.Contains("using System.Collections.Generic;\n"))
                                    {
                                        allUiVariableUsing.Add("using System.Collections.Generic;" + "\n");
                                    }
                                }

                                childTypeName = childTypeName.Split('.')[childTypeName.Split('.').Length - 1];
                                allUiName += "        private List<" + childTypeName + "> _" +
                                             DataSvc.FirstCharToLower(child.name) + ";" + "\n";
                                allUiVariableName.Add("        private List<" + childTypeName + "> _" +
                                                      DataSvc.FirstCharToLower(child.name) + ";" +
                                                      "\n");
                            }

                            break;
                    }

                    allUiVariableViewName.Add(child.name);
                }
            }

            return allUiName;
        }

        /// <summary>
        /// 一键绑定UI
        /// </summary>
        /// <returns></returns>
        public string OneClickBindUi()
        {
            Transform window = GetWindow();
            allUiVariableBind = new List<string>();
            string allBindName = "";
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                if (child.GetComponent<BindUiType>() && !GetUiComponentContainLocalBaseWindow(child))
                {
                    allBindName += "            BindUi(ref _" + DataSvc.FirstCharToLower(child.name) + ",\"" +
                                   GetUiComponentPath(child, "") + "\");" + "\n";
                    allUiVariableBind.Add("            BindUi(ref _" + DataSvc.FirstCharToLower(child.name) + ",\"" +
                                          GetUiComponentPath(child, "") + "\");" + "\n");
                }
            }


            return allBindName;
        }

        /// <summary>
        /// 获得UI路径
        /// </summary>
        /// <returns></returns>
        private string GetUiComponentPath(Transform uiTr, string uiPath)
        {
            Transform defaultUiTr = uiTr;
            int hierarchy = 0;
            while (uiTr.parent.name != GetWindow().name)
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
        /// 获得UI组件是否包含LocalBaseWindow路径
        /// </summary>
        /// <returns></returns>
        private bool GetUiComponentContainLocalBaseWindow(Transform uiTr)
        {
            Transform defaultUiTr = uiTr;
            int hierarchy = 0;
            while (uiTr.parent.name != GetWindow().name)
            {
                hierarchy++;
                uiTr = uiTr.parent;
            }

            if (hierarchy == 0)
            {
                return false;
            }
            else
            {
                for (int i = 0; i <= hierarchy; i++)
                {
                    if (GetParentByHierarchy(defaultUiTr, i).GetComponent<LocalBaseWindow>())
                    {
                        return true;
                    }
                }
            }


            return false;
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
        public string OneClickBindListener()
        {
            Transform window = GetWindow();
            string allBindName = "";
            allUiVariableBindListener = new List<string>();
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                if (child.GetComponent<BindUiType>() && !GetUiComponentContainLocalBaseWindow(child))
                {
                    if (child.GetComponent<BindUiType>().type == BindUiType.UiType.Button)
                    {
                        allBindName += "            BindListener(_" + DataSvc.FirstCharToLower(child.name) + "," +
                                       "EventTriggerType.PointerClick" + "," + "On" + child.name +
                                       ");" + "\n";
                        allUiVariableBindListener.Add("            BindListener(_" +
                                                      DataSvc.FirstCharToLower(child.name) + "," +
                                                      "EventTriggerType.PointerClick" + "," + "On" + child.name +
                                                      ");" + "\n");
                        if (!allUiVariableUsing.Contains("using UnityEngine.EventSystems;\n"))
                        {
                            allUiVariableUsing.Add("using UnityEngine.EventSystems;" + "\n");
                        }
                    }
                    else
                    {
                        allUiVariableBindListener.Add(string.Empty);
                    }
                }
            }


            return allBindName;
        }

        /// <summary>
        /// 一键声明UI事件
        /// </summary>
        /// <returns></returns>
        public string OneClickStatementListener()
        {
            Transform window = GetWindow();

            string allBindName = "";
            allUiVariableBindListenerEvent = new List<string>();
            foreach (Transform child in window.GetComponentsInChildren<Transform>(true))
            {
                if (child.GetComponent<BindUiType>() && !GetUiComponentContainLocalBaseWindow(child))
                {
                    if (child.GetComponent<BindUiType>().type == BindUiType.UiType.Button)
                    {
                        allBindName += "        private void On" + child.name + "(BaseEventData targetObj)" + "\n" +
                                       "{" +
                                       "\n" + "}" + "\n";
                        allUiVariableBindListenerEvent.Add("        private void On" + child.name +
                                                           "(BaseEventData targetObj)" + "\n" + "        {" +
                                                           "\n" + "        }" + "\n");
                    }
                    else
                    {
                        allUiVariableBindListenerEvent.Add(string.Empty);
                    }
                }
            }

            /*TextEditor textEditor = new TextEditor
            {
                text = allBindName
            };
            textEditor.OnFocus();
            textEditor.Copy();*/
            return allBindName;
        }

        /// <summary>
        /// 隐藏界面
        /// </summary>
        public virtual void HideView()
        {
            transform.Find("Window").gameObject.SetActive(false);
        }

        /// <summary>
        /// 隐藏界面
        /// </summary>
        public virtual void ShowView()
        {
            transform.Find("Window").gameObject.SetActive(true);
        }

        #endregion
    }
}