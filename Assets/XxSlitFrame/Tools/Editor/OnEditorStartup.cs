using System;
using UnityEditor;
using UnityEditor.Callbacks;

namespace XxSlitFrame.Tools.Editor
{
    [InitializeOnLoad]
    public class OnEditorStartup
    {
        static OnEditorStartup()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.WebGL) return;
#if !UNITY_2019
            EditorApplication.delayCall =
 () => { EditorApplication.ExecuteMenuItem("Edit/Graphics Emulation/WebGL 2.0"); };
#endif
        }

        /// <summary>
        /// Build完成后的回调
        /// </summary>
        [PostProcessBuild(1)]
        public static void AfterBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.WebGL) return;
#if !UNITY_2019
            EditorApplication.delayCall =
 () => { EditorApplication.ExecuteMenuItem("Edit/Graphics Emulation/WebGL 2.0"); };
#endif
        }
    }
}