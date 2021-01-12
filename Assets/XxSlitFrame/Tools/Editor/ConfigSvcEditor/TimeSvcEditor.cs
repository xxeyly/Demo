using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.Tools.Editor.ConfigSvcEditor
{
    [CustomEditor(typeof(TimeSvc))]
    public class TimeSvcEditor : UnityEditor.Editor
    {
        private TimeSvc _timeSvc;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _timeSvc = (TimeSvc) target;
            if (_timeSvc.timeTaskList != null)
            {
                for (int i = 0; i < _timeSvc.timeTaskList.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("任务ID:" + _timeSvc.timeTaskList[i].tid, GUILayout.MaxWidth(150));
                    EditorGUILayout.LabelField("任务循环方式:" + _timeSvc.timeTaskList[i].loopType, GUILayout.MaxWidth(150));
                    EditorGUILayout.LabelField("任务名字:" + _timeSvc.timeTaskList[i].tidName, GUILayout.MaxWidth(150));
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
    }
}