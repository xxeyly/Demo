using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XxSlitFrame.Tools.ConfigData.Editor
{
    [Serializable]
    [CreateAssetMenu(fileName = "BuildSceneData", menuName = "配置文件/打包场景数据", order = 1)]
    public class BuildSceneData : ScriptableObject
    {
        /// <summary>
        /// 打包地址
        /// </summary>
        public string assetBundlePath;

        /// <summary>
        /// 场景打包数据
        /// </summary>
        [Header("场景打包数据")] public List<BuildSceneDataInfo> buildSceneDataInfo = new List<BuildSceneDataInfo>();
    }

    /// <summary>
    /// 打包场景数据
    /// </summary>
    [Serializable]
    public class BuildSceneDataInfo
    {
        /// <summary>
        /// 场景
        /// </summary>
        public SceneAsset sceneObj;

        public string sceneObjMd5;

        public List<string> sceneBuildPath;
        public List<string> sceneBuildPathMd5;

        /// <summary>
        /// 环境
        /// </summary>
        public GameObject envObj;

        public string envObjMd5;

        public List<string> envObjBuildPath;
        public List<string> envObjBuildPathMd5;

        /// <summary>
        /// 道具
        /// </summary>
        public GameObject propObj;

        public string propObjMd5;
        public List<string> propObjBuildPath;
        public List<string> propObjBuildPathMd5;

        /// <summary>
        /// 角色
        /// </summary>
        public GameObject character;

        public string characterMd5;
        public List<string> characterBuildPath;
        public List<string> characterBuildPathMd5;

        /// <summary>
        /// UI
        /// </summary>
        public GameObject ui;

        public string uiMd5;
        public List<string> uiBuildPath;
        public List<string> uiBuildPathMd5;

        /// <summary>
        /// 功能
        /// </summary>
        public GameObject function;

        public string functionMd5;
        public List<string> functionBuildPath;
        public List<string> functionBuildPathMd5;
    }
}