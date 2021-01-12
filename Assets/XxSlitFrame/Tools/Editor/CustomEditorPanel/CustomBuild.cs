using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using LitJson;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using XxSlitFrame.Tools.ConfigData.Editor;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel
{
    public class CustomBuild : EditorWindow
    {
        private static CustomBuildData _buildData;

        [MenuItem("XFrame/打包工具", false, 0)]
        private static void ShowWindow()
        {
            EditorWindow window = EditorWindow.GetWindow<CustomBuild>();
            window.minSize = window.minSize;
            window.maxSize = new Vector2(600, 650);
            window.titleContent = new GUIContent() {image = null, text = "打包工具"};
            window.Show();
        }

        Vector2 _copyFolderCountScroll = Vector2.zero;

        private static FtpOperation _ftpOperation;

        private void OnEnable()
        {
            if (_buildData == null)
            {
                _buildData =
                    (CustomBuildData) AssetDatabase.LoadAssetAtPath("Assets/XxSlitFrame/Config/CustomBuildData.asset",
                        typeof(CustomBuildData));
            }
        }

        private void OnDestroy()
        {
            SaveData();
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        private void SaveData()
        {
            EditorUtility.SetDirty(_buildData);
            AssetDatabase.SaveAssets();
        }

        public void OnGUI()
        {
            #region 打包方式

            EditorGUILayout.BeginHorizontal();
            //自定义枚举下拉框
            EditorGUILayout.LabelField("选择当前项目的打包方式:", GUILayout.MaxWidth(130));
            _buildData.buildTarget = (BuildTarget) EditorGUILayout.EnumPopup(_buildData.buildTarget);

            #endregion

            #region 数据存储

            if (GUILayout.Button("保存数据", GUILayout.MaxWidth(80), GUILayout.MaxHeight(20)))
            {
                //标记脏区
                EditorUtility.SetDirty(_buildData);
                // 保存所有修改
                AssetDatabase.SaveAssets();
            }

            EditorGUILayout.EndHorizontal();

            #endregion

            #region 打包路径

            EditorGUILayout.BeginHorizontal();
            //选择打包路径
            if (GUILayout.Button("选择打包路径", GUILayout.MaxWidth(80)))
            {
                _buildData.exportPath = EditorUtility.OpenFolderPanel("选择打包路径", "", "");
            }

            _buildData.exportPath =
                EditorGUILayout.TextField(_buildData.exportPath, GUILayout.MaxWidth(520), GUILayout.MaxHeight(20));
            EditorGUILayout.EndHorizontal();

            #endregion

            #region 项目名称

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("项目中文名称:", GUILayout.MaxWidth(70));
            _buildData.exportCnProjectName = EditorGUILayout.TextField(_buildData.exportCnProjectName);

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("项目英文名称:", GUILayout.MaxWidth(70));
            _buildData.exportEnProjectName = EditorGUILayout.TextField(_buildData.exportEnProjectName);


            EditorGUILayout.EndHorizontal();


            EditorGUILayout.BeginHorizontal();

            _buildData.chineseShell = GUILayout.Toggle(_buildData.chineseShell, "中文输出外壳", GUILayout.MaxWidth(90));

            _buildData.learningModel = GUILayout.Toggle(_buildData.learningModel, "学习模式", GUILayout.MaxWidth(70));

            if (_buildData.learningModel)
            {
                _buildData.assessmentMode = false;
            }

            _buildData.assessmentMode = GUILayout.Toggle(_buildData.assessmentMode, "考核模式", GUILayout.MaxWidth(70));

            if (_buildData.assessmentMode)
            {
                _buildData.learningModel = false;
            }

            _buildData.projectNameDate = GUILayout.Toggle(_buildData.projectNameDate, "日期", GUILayout.MaxWidth(50));
            _buildData.watermark = GUILayout.Toggle(_buildData.watermark, "水印", GUILayout.MaxWidth(50));
            _buildData.versionWatermark = _buildData.watermark;
            _buildData.updateToFtp =
                GUILayout.Toggle(_buildData.updateToFtp, "打包后是否更新到FTP服务器", GUILayout.MaxWidth(170));
            _buildData.versionSet = GUILayout.Toggle(_buildData.versionSet, "版本打包设置");
            EditorGUILayout.EndHorizontal();

            #endregion

            #region 服务器与打包信息

            EditorGUILayout.BeginHorizontal();
            if (_buildData.updateToFtp)
            {
                EditorGUILayout.LabelField("服务器地址:", GUILayout.MaxWidth(70));
                _buildData.ftpServerPath = EditorGUILayout.TextField(_buildData.ftpServerPath);
                EditorGUILayout.LabelField("用户名:", GUILayout.MaxWidth(40));
                _buildData.ftpUser = EditorGUILayout.TextField(_buildData.ftpUser);
                EditorGUILayout.LabelField("密码:", GUILayout.MaxWidth(30));
                _buildData.ftpPwd = EditorGUILayout.TextField(_buildData.ftpPwd);
                EditorGUILayout.LabelField("存放目录:", GUILayout.MaxWidth(50));
                _buildData.ftpRoot = EditorGUILayout.TextField(_buildData.ftpRoot);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            if (_buildData.versionSet)
            {
                _buildData.versionWatermark =
                    GUILayout.Toggle(_buildData.versionWatermark, "版本水印", GUILayout.MaxWidth(70));
                _buildData.watermark = _buildData.versionWatermark;
                _buildData.versionDownLoad =
                    GUILayout.Toggle(_buildData.versionDownLoad, "版本下载", GUILayout.MaxWidth(70));
                _buildData.versionLoadingProgress = GUILayout.Toggle(_buildData.versionLoadingProgress, "版本下载进度",
                    GUILayout.MaxWidth(90));
                _buildData.versionSceneProgress =
                    GUILayout.Toggle(_buildData.versionSceneProgress, "版本场景进度", GUILayout.MaxWidth(90));
                EditorGUILayout.LabelField("版本考核时间:", GUILayout.MaxWidth(70));
                _buildData.versionAssessmentTime =
                    EditorGUILayout.IntField(_buildData.versionAssessmentTime, GUILayout.MaxWidth(70));
            }

            EditorGUILayout.EndHorizontal();

            #endregion

            #region 动态增加拷贝文件夹

            EditorGUILayout.BeginHorizontal();
            //增加拷贝数量
            if (GUILayout.Button("增加拷贝数量", GUILayout.MaxWidth(80)))
            {
                _buildData.folderCopies.Add(new FolderCopy());
            }


            EditorGUILayout.EndHorizontal();


            _copyFolderCountScroll = EditorGUILayout.BeginScrollView(_copyFolderCountScroll);
            for (int i = 0; i < _buildData.folderCopies.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();

                //选择复制路径
                if (GUILayout.Button("选择复制路径", GUILayout.MaxWidth(80)))
                {
                    _buildData.folderCopies[i].copyFolderPath = EditorUtility.OpenFolderPanel("选择打包路径", "", "");
                }

                _buildData.folderCopies[i].copyFolderPath =
                    EditorGUILayout.TextField(_buildData.folderCopies[i].copyFolderPath, GUILayout.MaxHeight(20));


                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("粘贴文件路径", GUILayout.MaxWidth(80));
                _buildData.folderCopies[i].pasteFolderPath =
                    EditorGUILayout.TextField(_buildData.folderCopies[i].pasteFolderPath, GUILayout.MaxHeight(20));
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                //增加拷贝数量
                if (GUILayout.Button("删除", GUILayout.MaxWidth(60), GUILayout.MaxHeight(40)))
                {
                    _buildData.folderCopies.RemoveAt(i);
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();

            /*if (_buildData.folderCopies.Count > 0)
            {
                
                
                if (_buildData.copyFolderCount > _buildData.copyFolderPaths.Count)
                {
                    for (int i = 0; i < _buildData.copyFolderCount - _buildData.copyFolderPaths.Count; i++)
                    {
                        _buildData.copyFolderPaths.Add(string.Empty);
                    }
                }
                else
                {
                    for (int i = 0; i < _buildData.copyFolderPaths.Count - _buildData.copyFolderCount; i++)
                    {
                        _buildData.copyFolderPaths.RemoveAt(_buildData.copyFolderPaths.Count - 1);
                    }
                }

                if (_buildData.copyFolderCount > _buildData.pasteFolderPaths.Count)
                {
                    for (int i = 0; i < _buildData.copyFolderCount - _buildData.pasteFolderPaths.Count; i++)
                    {
                        _buildData.pasteFolderPaths.Add(string.Empty);
                    }
                }
                else
                {
                    for (int i = 0; i < _buildData.pasteFolderPaths.Count - _buildData.copyFolderCount; i++)
                    {
                        _buildData.pasteFolderPaths.RemoveAt(_buildData.pasteFolderPaths.Count - 1);
                    }
                }

                _copyFolderCountScroll = EditorGUILayout.BeginScrollView(_copyFolderCountScroll);
                for (int i = 0; i < _buildData.copyFolderPaths.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.BeginHorizontal();

                    //选择复制路径
                    if (GUILayout.Button("选择复制路径", GUILayout.MaxWidth(80)))
                    {
                        _buildData.copyFolderPaths[i] = EditorUtility.OpenFolderPanel("选择打包路径", "", "");
                    }

                    _buildData.copyFolderPaths[i] = EditorGUILayout.TextField(_buildData.copyFolderPaths[i], GUILayout.MaxWidth(450), GUILayout.MaxHeight(20));


                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("粘贴文件路径", GUILayout.MaxWidth(80));
                    _buildData.pasteFolderPaths[i] = EditorGUILayout.TextField(_buildData.pasteFolderPaths[i], GUILayout.MaxWidth(450), GUILayout.MaxHeight(20));
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                    //增加拷贝数量
                    if (GUILayout.Button("删除", GUILayout.MaxWidth(60), GUILayout.MaxHeight(40)))
                    {
                        _buildData.copyFolderPaths.Remove(_buildData.copyFolderPaths[i]);
                        _buildData.pasteFolderPaths.Remove(_buildData.pasteFolderPaths[i]);
                    }

                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndScrollView();
            }*/

            #endregion

            #region 打包

            // EditorGUILayout.BeginHorizontal();
            //选择打包路径
            if (GUILayout.Button("开始打包", GUILayout.MaxHeight(40)))
            {
                //保存打包数据
                SaveData();
                //保存打包版本数据
                SaveBuildData();
                //拷贝打包文件
                for (int i = 0; i < _buildData.folderCopies.Count; i++)
                {
                    if (_buildData.folderCopies[i].copyFolderPath != string.Empty &&
                        _buildData.folderCopies[i].pasteFolderPath != string.Empty)
                    {
                        Copy(_buildData.folderCopies[i].copyFolderPath,
                            ProjectPath() + "/" + _buildData.folderCopies[i].pasteFolderPath);
                    }
                }

                BuildPipeline.BuildPlayer(FindEnableEditorScenes(), ProjectPath(), _buildData.buildTarget,
                    BuildOptions.CompressWithLz4HC);
            }

            // EditorGUILayout.EndHorizontal();

            #endregion
        }


        /// <summary>
        /// 保存版本数据
        /// </summary>
        private void SaveBuildData()
        {
            ResSvc.VersionInfo version = new ResSvc.VersionInfo
            {
                watermark = _buildData.versionWatermark, downLoad = _buildData.versionDownLoad,
                loadingProgress = _buildData.versionLoadingProgress, sceneProgress = _buildData.versionSceneProgress,
                assessmentTime = _buildData.versionAssessmentTime
            };
            string versionData = Encoding.UTF8.GetString(Encoding.Default.GetBytes(JsonMapper.ToJson(version)));

            ResSvc.FileOperation.SaveTextToLoad(Application.dataPath + "/XxSlitFrame/Resources/VersionData",
                "VersionInfo.Json", versionData);
        }


        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        /// <param name="sourceDirName"></param>
        /// <param name="destDirName"></param>
        private static void Copy(string sourceDirName, string destDirName)
        {
            if (sourceDirName.Substring(sourceDirName.Length - 1) != "\\")
            {
                sourceDirName = sourceDirName + "\\";
            }

            if (destDirName.Substring(destDirName.Length - 1) != "\\")
            {
                destDirName = destDirName + "\\";
            }

            if (Directory.Exists(sourceDirName))
            {
                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                }

                foreach (string item in Directory.GetFiles(sourceDirName))
                {
                    if (item.Contains("meta"))
                    {
                        continue;
                    }

                    File.Copy(item, destDirName + Path.GetFileName(item), true);
                }

                foreach (string item in Directory.GetDirectories(sourceDirName))
                {
                    Copy(item, destDirName + item.Substring(item.LastIndexOf("\\", StringComparison.Ordinal) + 1));
                }
            }
        }

        /// <summary>
        /// 获得打包的场景
        /// </summary>
        /// <returns></returns>
        private string[] FindEnableEditorScenes()
        {
            List<string> editorScenes = new List<string>();
            foreach (EditorBuildSettingsScene editorBuildSettingsScene in EditorBuildSettings.scenes)
            {
                if (editorBuildSettingsScene.enabled)
                {
                    editorScenes.Add(editorBuildSettingsScene.path);
                }
            }

            return editorScenes.ToArray();
        }

        /// <summary>
        /// 项目路径
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private string ProjectPath()
        {
            //打包路径
            string path = "";
            path += _buildData.exportPath + "/";
            string chinsesPath = "";
            //如果启动中文外壳
            if (_buildData.chineseShell)
            {
                chinsesPath += _buildData.exportCnProjectName;
                if (_buildData.learningModel)
                {
                    chinsesPath += "-学习模式";
                }

                if (_buildData.assessmentMode)
                {
                    chinsesPath += "-考核模式";
                }

                if (_buildData.projectNameDate)
                {
                    chinsesPath += "-" + DateTime.Now.ToString("yyyy.MM.dd");
                }

                if (_buildData.watermark)
                {
                    chinsesPath += "-水印";
                }
                else
                {
                    chinsesPath += "-无水印";
                }

                chinsesPath += "/";
            }

            path += chinsesPath;
            path += _buildData.exportEnProjectName;

            /*if (_buildData.learningModel)
            {
                path += "-Study";
            }

            if (_buildData.assessmentMode)
            {
                path += "-Assessment";
            }

            if (_buildData.projectNameDate)
            {
                path += "-" + DateTime.Now.ToString("yyyy.MM.dd");
            }

            if (_buildData.watermark)
            {
                path += "-Watermark";
            }
            else
            {
                path += "-NoWatermark";
            }*/


            return path;
        }

        /// <summary>
        /// Build完成后的回调
        /// </summary>
        /// <param name="target">打包的目标平台</param>
        /// <param name="pathToBuiltProject">包体的完整路径</param>
        [PostProcessBuild(1)]
        public static void AfterBuild(BuildTarget target, string pathToBuiltProject)
        {
            Debug.Log("Build Success  输出平台: " + target + "  输出路径: " + pathToBuiltProject);

            //打开文件或文件夹
            System.Diagnostics.Process.Start(pathToBuiltProject);

            int index = pathToBuiltProject.LastIndexOf("/", StringComparison.Ordinal);

            Debug.Log("导出包体的目录 :" + pathToBuiltProject.Substring(0, index));
            _ftpOperation = new FtpOperation(_buildData);
#if !UNITY_EDITOR
            if (_buildData.updateToFtp)
            {
                _ftpOperation.UploadFile();
                Debug.Log("FTP文件已经上传成功...");
            }
#endif
            GetWindow<CustomBuild>().Close();
        }
    }

    /// <summary>
    /// FTP工具
    /// </summary>
    public class FtpOperation
    {
        private CustomBuildData _customBuildData;

        public FtpOperation(CustomBuildData customBuildData)
        {
            _customBuildData = customBuildData;
        }

        /// <summary>
        /// 项目路径
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private string ProjectPath()
        {
            //打包路径
            string path = "";
            path += _customBuildData.exportPath + "/";
            string chinsesPath = "";
            //如果启动中文外壳
            if (_customBuildData.chineseShell)
            {
                chinsesPath += _customBuildData.exportCnProjectName;
                if (_customBuildData.learningModel)
                {
                    chinsesPath += "-学习模式";
                }

                if (_customBuildData.assessmentMode)
                {
                    chinsesPath += "-考核模式";
                }

                if (_customBuildData.projectNameDate)
                {
                    chinsesPath += "-" + DateTime.Now.ToString("yyyy.MM.dd");
                }

                if (_customBuildData.watermark)
                {
                    chinsesPath += "-水印";
                }
                else
                {
                    chinsesPath += "-无水印";
                }

                chinsesPath += "/";
            }

            path += chinsesPath + ProjectName();

            return path;
        }

        /// <summary>
        /// 项目名称
        /// </summary>
        /// <returns></returns>
        private string ProjectName()
        {
            string projectName = _customBuildData.exportEnProjectName;
            /*
            if (_customBuildData.learningModel)
            {
                projectName += "-Study";
            }

            if (_customBuildData.assessmentMode)
            {
                projectName += "-Assessment";
            }

            if (_customBuildData.projectNameDate)
            {
                projectName += "-" + DateTime.Now.ToString("yyyy.MM.dd");
            }

            if (_customBuildData.watermark)
            {
                projectName += "-Watermark";
            }
            else
            {
                projectName += "-NoWatermark";
            }
            */

            return projectName;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        public void UploadFile()
        {
            Dictionary<string, List<string>> filePathInfos = new Dictionary<string, List<string>>();
            filePathInfos = GetFiles(ProjectPath(), ProjectPath(), filePathInfos);

            foreach (KeyValuePair<string, List<string>> pair in filePathInfos)
            {
                if (pair.Key != "")
                {
                    MakeDir(_customBuildData.ftpServerPath, _customBuildData.ftpUser, _customBuildData.ftpPwd,
                        _customBuildData.ftpRoot + "/" + ProjectName() + pair.Key);
                }
                else
                {
                    MakeDir(_customBuildData.ftpServerPath, _customBuildData.ftpUser, _customBuildData.ftpPwd,
                        _customBuildData.ftpRoot);
                    MakeDir(_customBuildData.ftpServerPath, _customBuildData.ftpUser, _customBuildData.ftpPwd,
                        _customBuildData.ftpRoot + "/" + ProjectName());
                }
            }

            foreach (KeyValuePair<string, List<string>> pair in GetFiles(ProjectPath(),
                ProjectPath(), filePathInfos))
            {
                foreach (string s in pair.Value)
                {
                    WebClient client = new WebClient();
                    Uri uri = new Uri("ftp://" + _customBuildData.ftpServerPath + "/" + _customBuildData.ftpRoot + "/" +
                                      ProjectName() + s);
                    client.Credentials = new NetworkCredential(_customBuildData.ftpUser, _customBuildData.ftpPwd);
                    client.UploadFileAsync(uri, "STOR", ProjectPath() + s);
                }
            }
        }

        /// <summary>
        ///在ftp服务器上创建文件目录
        /// </summary>
        /// <param name="dirName">文件目录</param>
        /// <returns></returns>
        public bool MakeDir(string path, string user, string pwd, string dirName)
        {
            try
            {
                string uri = ("ftp://" + path + "/" + dirName + "/");
                if (DirectoryIsExist(uri, user, pwd))
                {
                    // Debug.Log("已存在");
                    return true;
                }

                string url = "ftp://" + path + "/" + dirName;
                FtpWebRequest reqFtp = (FtpWebRequest) WebRequest.Create(new Uri(url));
                reqFtp.UseBinary = true;
                // reqFtp.KeepAlive = false;
                reqFtp.Method = WebRequestMethods.Ftp.MakeDirectory;
                reqFtp.Credentials = new NetworkCredential(user, pwd);
                FtpWebResponse response = (FtpWebResponse) reqFtp.GetResponse();
                response.Close();
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError("因{0},无法下载" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 判断ftp上的文件目录是否存在
        /// </summary>
        /// <returns></returns>        
        private static bool DirectoryIsExist(string uri, string user, string pwd)
        {
            string[] value = GetFileList(uri, user, pwd);
            if (value == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static string[] GetFileList(string uri, string user, string pwd)
        {
            StringBuilder result = new StringBuilder();
            try
            {
                FtpWebRequest reqFtp = (FtpWebRequest) WebRequest.Create(uri);
                reqFtp.UseBinary = true;
                reqFtp.Credentials = new NetworkCredential(user, pwd);
                reqFtp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                WebResponse response = reqFtp.GetResponse();
                StreamReader reader =
                    new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException(),
                        Encoding.UTF8);
                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    line = reader.ReadLine();
                }

                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch
            {
                return null;
            }
        }

        private Dictionary<string, List<string>> GetFiles(string rootPath, string path,
            Dictionary<string, List<string>> fileList)
        {
            string filename;
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] fil = dir.GetFiles();
            DirectoryInfo[] dii = dir.GetDirectories();
            List<string> newFileList = new List<string>();
            foreach (FileInfo f in fil)
            {
                filename = f.FullName.Remove(0, rootPath.Length);
                if (!fileList.ContainsKey(path.Remove(0, rootPath.Length)))
                {
                    newFileList.Add(filename);
                    fileList.Add(path.Remove(0, rootPath.Length), newFileList);
                }

                newFileList.Add(filename);
            }

            //获取子文件夹内的文件列表，递归遍历  
            foreach (DirectoryInfo d in dii)
            {
                GetFiles(rootPath, d.FullName, fileList);
            }

            return fileList;
        }
    }
}