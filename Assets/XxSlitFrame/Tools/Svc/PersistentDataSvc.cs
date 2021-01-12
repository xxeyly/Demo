using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using XxSlitFrame.Tools.Svc.BaseSvc;
using Random = UnityEngine.Random;

namespace XxSlitFrame.Tools.Svc
{
    /// <summary>
    /// 加载场景方式
    /// </summary>
    public enum SceneLoadType
    {
        Normal,
        SceneName,
        SceneIndex
    }

    public enum QualitySettingType
    {
        Low,
        Center,
        High
    }

    /// <summary>
    /// 动态加载数据
    /// </summary>
    public class PersistentDataSvc : SvcBase
    {
        public static PersistentDataSvc Instance;

        /// <summary>
        /// 场景索引
        /// </summary>
        [Header("场景索引")] public int sceneIndex;

        /// <summary>
        /// 场景名称
        /// </summary>
        [Header("场景名称")] public string sceneName;

        /// <summary>
        /// 跳转场景
        /// </summary>
        [Header("跳转场景")] public bool jump;

        /// <summary>
        /// 跳转场景索引
        /// </summary>
        [Header("跳转场景索引")] public string jumpSceneName;

        /// <summary>
        /// 音乐开关
        /// </summary>
        [Header("音乐开关")] public bool audioState;

        [Header("下载项目配置数据")] public ResSvc.VersionInfo versionInfo;
        [Header("下载文件配置数据")] public ResSvc.DownFile downFileInfo;
        [Header("下载文件配置数据完毕")] public bool downFileInfoOver;
        [Header("下载项目配置数据完毕")] public bool downVersionOver;
        [Header("下载数据完毕")] public bool downFileOver;
        [Header("下载文件数据")] [SerializeField] public Dictionary<string, byte[]> downFileData;
        [Header("相机速度值")] public float cameraSpeed = 1;
        [Header("自动播放模式")] public bool autoPlay = false;

        /// <summary>
        /// 当前大步骤
        /// </summary>
        [Header("当前大步骤")] public int currentStepBigIndex;

        /// <summary>
        /// 当前小步骤
        /// </summary>
        [Header("当前小步骤")] public int currentStepSmallIndex;

        /// <summary>
        /// 当前小步骤
        /// </summary>
        [Header("当前小小步骤")] public int currentStepSmallSmallIndex;

        [Header("当前质量")] public QualitySettingType qualitySettingType = QualitySettingType.High;
        [Header("鼠标状态")] public bool mouseState;

        public override void InitSvc()
        {
            audioState = true;
            // gameObject.name = "ServerURL";
            // Debug.Log("下载配置文件");
            ResSvc.Instance.StartDownProjectConfig();
        }

        /// <summary>
        /// 加载场景方式
        /// </summary>
        [Header("加载场景方式")] public SceneLoadType sceneLoadType;

        /// <summary>
        /// 服务器地址
        /// </summary>
        [Header("服务器地址")] public string serverPath;

        /// <summary>
        /// 平台加载
        /// </summary>
        [Header("平台加载")] public bool platformLoad;

        [Header("场景资源加载完毕")] public bool sceneResLoad;

        public override void StartSvc()
        {
            Instance = GetComponent<PersistentDataSvc>();
            // Debug.Log("获取文件地址");

            if (Application.platform == RuntimePlatform.WindowsPlayer)
            {
                serverPath = General.General.GetUrlRootPath();
            }
        }
    }
}