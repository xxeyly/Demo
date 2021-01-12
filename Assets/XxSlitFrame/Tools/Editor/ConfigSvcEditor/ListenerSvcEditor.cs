using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.General;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.Tools.Editor.ConfigSvcEditor
{
    [CustomEditor(typeof(ListenerSvc))]
    public class ListenerSvcEditor : UnityEditor.Editor
    {
        private ListenerSvc _listenerSvc;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _listenerSvc = (ListenerSvc) target;
            if (_listenerSvc.listenerDic != null)
            {
                foreach (KeyValuePair<ListenerEventType, Delegate> pair in _listenerSvc.listenerDic)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("绑定事件:" + pair.Key);
                    EditorGUILayout.LabelField("事件方法:" + pair.Value.Method.Name);
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
    }
}