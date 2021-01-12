using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using XxSlitFrame.View;
using Object = UnityEngine.Object;

namespace XxSlitFrame.Tools.Editor.ConfigBaseWindowEditor
{
    public static class WindowBaseEditor
    {
        [MenuItem("GameObject/Create Empty WindowView", false, 0)]
        public static void OnCreateEmptyWindowView()
        {
            if (Object.FindObjectOfType<Canvas>())
            {
                //View 窗口根目录
                GameObject windowView = new GameObject("Empty WindowView");
                Vector2 windowSize = Object.FindObjectOfType<CanvasScaler>().referenceResolution;
                windowView.AddComponent<RectTransform>().sizeDelta = windowSize;
                // windowView.AddComponent<GenerationBaseWindow>().Init();
                windowView.AddComponent<AutoBindBaseWindowUIData>();
                //Window目录
                GameObject window = new GameObject("Window");
                window.AddComponent<RectTransform>().sizeDelta = windowSize;
                window.AddComponent<CanvasGroup>();
                //背景
                GameObject background = new GameObject("Background");
                background.AddComponent<Image>().rectTransform.sizeDelta = windowSize;

                //调整层级
                GameObject canvas = Object.FindObjectOfType<Canvas>().gameObject;
                if (canvas != null)
                {
                    windowView.transform.SetParent(canvas.transform);
                    window.transform.SetParent(windowView.transform);
                    background.transform.SetParent(window.transform);
                    //Transform 调整
                    windowView.transform.localPosition = Vector3.zero;
                    windowView.transform.localScale = Vector3.one;
                    window.transform.localPosition = Vector3.zero;
                    window.transform.localScale = Vector3.one;
                    background.transform.localPosition = Vector3.zero;
                    background.transform.localScale = Vector3.one;
                }
                else
                {
                    Debug.LogError("场景中没有Canvas");
                }
            }
        }

        [MenuItem("GameObject/绑定UI /@(Alt+T) 绑定Button  &t", false, 0)]
        public static void BindComponent()
        {
            GameObject uiObj = Selection.objects.First() as GameObject;
            Debug.Log(uiObj.GetComponent<VideoPlayer>());
            if (uiObj != null)
            {
                if (!uiObj.GetComponent<BindUiType>())
                {
                    uiObj.AddComponent<BindUiType>();
                }

                if (uiObj.GetComponent<Button>())
                {
                    uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.Button;
                }
                else if (uiObj.GetComponent<Image>() &&
                         !uiObj.GetComponent<Button>() &&
                         !uiObj.GetComponent<Scrollbar>() &&
                         !uiObj.GetComponent<ScrollRect>() &&
                         !uiObj.GetComponent<RawImage>() &&
                         !uiObj.GetComponent<InputField>() &&
                         !uiObj.GetComponent<Dropdown>())
                {
                    uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.Image;
                }
                else if (uiObj.GetComponent<Text>())
                {
                    Debug.Log("VAR");
                    uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.Text;
                }
                else if (uiObj.GetComponent<Toggle>())
                {
                    uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.Toggle;
                }
                else if (uiObj.GetComponent<RawImage>())
                {
                    uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.RawImage;
                }
                else if (uiObj.GetComponent<Scrollbar>())
                {
                    uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.Scrollbar;
                }
                else if (uiObj.GetComponent<Dropdown>())
                {
                    uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.Dropdown;
                }
                else if (uiObj.GetComponent<InputField>())
                {
                    uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.InputField;
                }
                else if (uiObj.GetComponent<ScrollRect>())
                {
                    uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.ScrollRect;
                }
                else if (uiObj.GetComponent<VideoPlayer>())
                {
                    uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.VideoPlayer;
                }
                else if (uiObj.GetComponent<Slider>())
                {
                    uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.Slider;
                }
                else if (uiObj.GetComponentInChildren<LocalBaseWindow>())
                {
                    uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.ChildList;
                    uiObj.GetComponent<BindUiType>().childType = uiObj.GetComponentInChildren<LocalBaseWindow>();
                }
                else
                {
                    uiObj.GetComponent<BindUiType>().type = BindUiType.UiType.GameObject;
                }
            }
        }
    }
}