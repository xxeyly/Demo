using UnityEditor;
using UnityEngine;
using XxSlitFrame.View;

namespace XxSlitFrame.Tools.Editor.ConfigBaseWindowEditor
{
    [InitializeOnLoad]
    public class CustomBaseWindowHierarchy
    {
        static CustomBaseWindowHierarchy()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HierarchyItemCb;
        }

        private static void HierarchyItemCb(int instanceid, Rect selectionrect)
        {
            GameObject obj = EditorUtility.InstanceIDToObject(instanceid) as GameObject;
            if (obj != null)
            {
                if (obj.GetComponent<BaseWindow>() != null)
                {
                    if (Application.isPlaying && obj.GetComponent<BaseWindow>().GetViewShowType() == ViewShowType.Frozen)
                    {
                        return;
                    }

                    // CheckBox 
                    Rect rectCheck = new Rect(selectionrect);
                    rectCheck.x += rectCheck.width - 20;
                    rectCheck.width = 18;
                    GameObject window = obj.transform.Find("Window").gameObject;
                    window.SetActive(GUI.Toggle(rectCheck, window.activeSelf, string.Empty));
                    if (window.GetComponent<CanvasGroup>())
                    {
                        window.GetComponent<CanvasGroup>().alpha = window.activeSelf ? 1 : 0;
                    }
                    else
                    {
                        Debug.Log(window.transform.parent.name);
                    }

                 
                }
            }
        }
    }
}