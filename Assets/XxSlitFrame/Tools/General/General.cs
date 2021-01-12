using UnityEngine;
using UnityEngine.Rendering;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.Tools.General
{
    /// <summary>
    /// 事件类型
    /// </summary>
    public enum ListenerEventType
    {
        Normal,
        CameraMoveToTargetPos,
        PropInit,
        PropShowGroup,
        PlayDialogueParagraph,
        PlayTips,
        StopTips,
        PlayTipsAndAction,
        WatchVideoEnd,
        ShowTipContent
    }

    public enum AnimType
    {
        Normal,

        /// <summary>
        /// 自检
        /// </summary>
        SelfInspectionPage
    }

    public enum PropType
    {
        Normal,
    }

    /// <summary>
    /// 相机位置类型
    /// </summary>
    public enum CameraPosType
    {
        默认位置,
    }

    public static class General
    {
        #region 点击声音

        #endregion

        #region 视图时间

        /// <summary>
        /// 视图切换时间
        /// </summary>
        public const float ViewSwitchTime = 2f;

        /// <summary>
        /// 视图错误显示时间
        /// </summary>
        public const float ViewErrorTime = 1f;

        /// <summary>
        /// 版本信息位置
        /// </summary>
        public const string VersionDataInfoLocalPath = "/XxSlitFrame/Resources/VersionData/VersionInfo.Json";

        public const string VersionDataInfoServerPath = "/VersionData/VersionInfo.Json";

        /// <summary>
        /// 文件下载路径
        /// </summary>
        public const string DownFilePath = "/XxSlitFrame/Resources/DownFile/DownFileInfo.Json";

        #endregion

        #region 对话声音

        #endregion

        #region 网络地址

        public const string ServerAssetBundle = "http://127.0.0.1/AssetBundle/";

        #endregion

        /// <summary>
        /// 获得网页跟目录地址
        /// </summary>
        /// <returns></returns>
        public static string GetUrlRootPath()
        {
            Debug.Log("获取文件地址2");

            string url = Application.absoluteURL;
            //当前网页的url
            int index = url.LastIndexOf('/');
            if (index > 0)
            {
                var path = url.Substring(0, index);
                path = path + '/';
                return path;
            }
            else
            {
                Debug.LogError("未找到文件");
                return "";
            }
        }

        /// <summary>
        /// 获得版本信息地址
        /// </summary>
        /// <returns></returns>
        public static string GetFileConfigPath()
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                return GetUrlRootPath() + VersionDataInfoServerPath;
            }
            else if (Application.isEditor)
            {
                return "file://" + Application.dataPath + VersionDataInfoLocalPath;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获得文件数据地址
        /// </summary>
        /// <returns></returns>
        public static string GetFileDataPath(string relativePath)
        {
            if (Application.platform == RuntimePlatform.WebGLPlayer)
            {
                return PersistentDataSvc.Instance.serverPath + relativePath;
            }
            else if (Application.isEditor)
            {
                return "file://" + Application.dataPath + "/" + relativePath;
            }
            else
            {
                return "";
            }
        }
    }
}