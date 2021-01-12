using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using XxSlitFrame.Tools.ConfigData;
using XxSlitFrame.Tools.ConfigData.Editor;
using XxSlitFrame.Tools.Svc;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel
{
    public class CustomBuildScene : EditorWindow
    {
        private static BuildSceneData _buildSceneData;
        private BuildSceneDataInfo _currentBuildSceneDataInfo;

        [MenuItem("XFrame/场景工具", false, 0)]
        private static void ShowWindow()
        {
            EditorWindow window = EditorWindow.GetWindow<CustomBuildScene>();
            window.minSize = window.minSize;
            window.maxSize = new Vector2(1600, 900);
            window.titleContent = new GUIContent() {image = null, text = "场景工具"};
            window.Show();
        }

        private void OnEnable()
        {
            if (_buildSceneData == null)
            {
                _buildSceneData = (BuildSceneData) AssetDatabase.LoadAssetAtPath("Assets/XxSlitFrame/Config/BuildSceneData.asset", typeof(BuildSceneData));
            }
        }

        private void OnGUI()
        {
            #region 配置文件

            EditorGUILayout.BeginHorizontal();
            //自定义枚举下拉框
            EditorGUILayout.LabelField("场景配置数据:", GUILayout.MaxWidth(80));
#pragma warning disable 618
            _buildSceneData = (BuildSceneData) EditorGUILayout.ObjectField(_buildSceneData, typeof(BuildSceneData));
#pragma warning restore 618
            if (_buildSceneData != null)
            {
                if (GUILayout.Button("选择打包路径", GUILayout.MaxWidth(120)))
                {
                    string projectPath = Application.dataPath;
                    string openFolderPath = EditorUtility.OpenFolderPanel("选择打包路径", "", "");
                    if (openFolderPath.Contains(projectPath))
                    {
                        //转换为相对路径
                        openFolderPath = openFolderPath.Remove(0, projectPath.Length - 6);
                        _buildSceneData.assetBundlePath = openFolderPath;
                    }
                }

                _buildSceneData.assetBundlePath = EditorGUILayout.TextField(_buildSceneData.assetBundlePath, GUILayout.MaxWidth(580));

                if (GUILayout.Button("保存数据", GUILayout.MaxWidth(60)))
                {
                    SaveData();
                }
            }


            EditorGUILayout.EndHorizontal();

            #endregion

            #region 打包场景AB资源

            EditorGUILayout.BeginVertical();

            //选择打包路径
            if (GUILayout.Button("增加场景打包", GUILayout.MaxHeight(20)))
            {
                _buildSceneData.buildSceneDataInfo.Add(new BuildSceneDataInfo());
            }

            foreach (BuildSceneDataInfo buildSceneData in _buildSceneData.buildSceneDataInfo)
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField("场景文件:", GUILayout.MaxWidth(60));
#pragma warning disable 618
                buildSceneData.sceneObj = (SceneAsset) EditorGUILayout.ObjectField(buildSceneData.sceneObj, typeof(SceneAsset));
                EditorGUILayout.LabelField("场景环境:", GUILayout.MaxWidth(60));
#pragma warning disable 618
                buildSceneData.envObj = (GameObject) EditorGUILayout.ObjectField(buildSceneData.envObj, typeof(GameObject));
                EditorGUILayout.LabelField("场景物体:", GUILayout.MaxWidth(60));
#pragma warning disable 618
                buildSceneData.propObj = (GameObject) EditorGUILayout.ObjectField(buildSceneData.propObj, typeof(GameObject));
                EditorGUILayout.LabelField("场景人物:", GUILayout.MaxWidth(60));
#pragma warning disable 618
                buildSceneData.character = (GameObject) EditorGUILayout.ObjectField(buildSceneData.character, typeof(GameObject));
                EditorGUILayout.LabelField("场景UI:", GUILayout.MaxWidth(60));
#pragma warning disable 618
                buildSceneData.ui = (GameObject) EditorGUILayout.ObjectField(buildSceneData.ui, typeof(GameObject));
                EditorGUILayout.LabelField("场景功能:", GUILayout.MaxWidth(60));
#pragma warning disable 618
                buildSceneData.function = (GameObject) EditorGUILayout.ObjectField(buildSceneData.function, typeof(GameObject));


                //选择打包路径
                if (GUILayout.Button("删除场景打包", GUILayout.MaxWidth(80)))
                {
                    _buildSceneData.buildSceneDataInfo.Remove(buildSceneData);
                    break;
                }

                //清空打包数据
                if (GUILayout.Button("清空场景打包", GUILayout.MaxWidth(80)))

                {
                    buildSceneData.sceneObjMd5 = "";
                    buildSceneData.sceneBuildPath = new List<string>();
                    buildSceneData.sceneBuildPathMd5 = new List<string>();

                    buildSceneData.characterMd5 = "";
                    buildSceneData.characterBuildPath = new List<string>();
                    buildSceneData.characterBuildPathMd5 = new List<string>();

                    buildSceneData.functionMd5 = "";
                    buildSceneData.functionBuildPath = new List<string>();
                    buildSceneData.functionBuildPathMd5 = new List<string>();

                    buildSceneData.uiMd5 = "";
                    buildSceneData.uiBuildPath = new List<string>();
                    buildSceneData.uiBuildPathMd5 = new List<string>();


                    buildSceneData.propObjMd5 = "";
                    buildSceneData.propObjBuildPath = new List<string>();
                    buildSceneData.propObjBuildPathMd5 = new List<string>();

                    buildSceneData.envObjMd5 = "";
                    buildSceneData.envObjBuildPath = new List<string>();
                    buildSceneData.envObjBuildPathMd5 = new List<string>();


                    break;
                }

                EditorGUILayout.EndHorizontal();

#pragma warning restore 618
            }

            EditorGUILayout.EndVertical();

            #endregion

            #region 开始打包AB

            if (GUILayout.Button("场景打包", GUILayout.MaxHeight(40)))
            {
                foreach (BuildSceneDataInfo buildSceneData in _buildSceneData.buildSceneDataInfo)
                {
                    BuildSceneData(buildSceneData);
                }

                Debug.Log("场景打包完毕");
                AssetDatabase.Refresh();
            }

            #endregion
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
            EditorUtility.SetDirty(_buildSceneData);
            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// 打包场景数据
        /// </summary>
        /// <param name="buildSceneData"></param>
        private void BuildSceneData(BuildSceneDataInfo buildSceneData)
        {
            //当前要打包的场景文件数据
            _currentBuildSceneDataInfo = RedefiningBuildFileMD5(buildSceneData);

            //比较与旧版文件是否一致
            Dictionary<string, List<string>> buildSceneDataDic = CompareSceneData(_currentBuildSceneDataInfo, buildSceneData);
            if (buildSceneDataDic.Count > 0)
            {
                foreach (KeyValuePair<string, List<string>> pair in buildSceneDataDic)
                {
                    Debug.Log("当前场景需要重新打包的文件" + pair.Key);
                }

                //打包数据
                BuildSceneData(buildSceneDataDic);
                SaveBuildInfo(_currentBuildSceneDataInfo, buildSceneData);
            }
            else
            {
                Debug.Log("当前场景不需要打包" + buildSceneData.sceneObj.name);
            }

            //重新定义打包的Md5
        }

        /// <summary>
        /// 比较场景数据
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, List<string>> CompareSceneData(BuildSceneDataInfo currentBuildSceneData, BuildSceneDataInfo localBuildSceneData)
        {
            Dictionary<string, List<string>> buildData = new Dictionary<string, List<string>>();

            #region 场景Md5

            //环境Md5
            if (currentBuildSceneData.sceneObjMd5 != localBuildSceneData.sceneObjMd5)
            {
                buildData.Add(localBuildSceneData.sceneObj.name + "scene", currentBuildSceneData.sceneBuildPath);
            }
            else
            {
                if (currentBuildSceneData.sceneBuildPathMd5.Count != localBuildSceneData.sceneBuildPathMd5.Count)
                {
                    buildData.Add(localBuildSceneData.sceneObj.name + "scene", currentBuildSceneData.sceneBuildPath);
                }
                else
                {
                    for (int i = 0; i < currentBuildSceneData.sceneBuildPathMd5.Count; i++)
                    {
                        if (currentBuildSceneData.sceneBuildPathMd5[i] != localBuildSceneData.sceneBuildPathMd5[i])
                        {
                            buildData.Add(localBuildSceneData.sceneObj.name + "scene", currentBuildSceneData.sceneBuildPath);
                            break;
                        }
                    }
                }
            }

            #endregion

            #region 环境Md5

            //环境Md5
            if (currentBuildSceneData.envObjMd5 != localBuildSceneData.envObjMd5)
            {
                buildData.Add(localBuildSceneData.envObj.name, currentBuildSceneData.envObjBuildPath);
            }
            else
            {
                if (currentBuildSceneData.envObjBuildPathMd5.Count != localBuildSceneData.envObjBuildPathMd5.Count)
                {
                    buildData.Add(localBuildSceneData.envObj.name, currentBuildSceneData.envObjBuildPath);
                }
                else
                {
                    for (int i = 0; i < currentBuildSceneData.envObjBuildPathMd5.Count; i++)
                    {
                        if (currentBuildSceneData.envObjBuildPathMd5[i] != localBuildSceneData.envObjBuildPathMd5[i])
                        {
                            buildData.Add(localBuildSceneData.envObj.name, currentBuildSceneData.envObjBuildPath);
                            break;
                        }
                    }
                }
            }

            #endregion

            #region 道具Md5

            //道具Md5
            if (currentBuildSceneData.propObjMd5 != localBuildSceneData.propObjMd5)
            {
                buildData.Add(localBuildSceneData.propObj.name, currentBuildSceneData.propObjBuildPath);
            }
            else
            {
                if (currentBuildSceneData.propObjBuildPathMd5.Count != localBuildSceneData.propObjBuildPathMd5.Count)
                {
                    buildData.Add(localBuildSceneData.propObj.name, currentBuildSceneData.propObjBuildPath);
                }
                else
                {
                    for (int i = 0; i < currentBuildSceneData.propObjBuildPathMd5.Count; i++)
                    {
                        if (currentBuildSceneData.propObjBuildPathMd5[i] != localBuildSceneData.propObjBuildPathMd5[i])
                        {
                            buildData.Add(localBuildSceneData.propObj.name, currentBuildSceneData.propObjBuildPath);
                            break;
                        }
                    }
                }
            }

            #endregion

            #region 人物Md5

            if (currentBuildSceneData.characterMd5 != localBuildSceneData.characterMd5)
            {
                buildData.Add(localBuildSceneData.character.name, currentBuildSceneData.characterBuildPath);
            }
            else
            {
                if (currentBuildSceneData.characterBuildPathMd5.Count != localBuildSceneData.characterBuildPathMd5.Count)
                {
                    buildData.Add(localBuildSceneData.character.name, currentBuildSceneData.characterBuildPath);
                }
                else
                {
                    for (int i = 0; i < currentBuildSceneData.characterBuildPathMd5.Count; i++)
                    {
                        if (currentBuildSceneData.characterBuildPathMd5[i] != localBuildSceneData.characterBuildPathMd5[i])
                        {
                            buildData.Add(localBuildSceneData.character.name, currentBuildSceneData.characterBuildPath);
                            break;
                        }
                    }
                }
            }

            #endregion

            #region UIMd5

            //环境Md5
            if (currentBuildSceneData.uiMd5 != localBuildSceneData.uiMd5)
            {
                buildData.Add(localBuildSceneData.ui.name, currentBuildSceneData.uiBuildPath);
            }
            else
            {
                if (currentBuildSceneData.uiBuildPathMd5.Count != localBuildSceneData.uiBuildPathMd5.Count)
                {
                    buildData.Add(localBuildSceneData.ui.name, currentBuildSceneData.uiBuildPath);
                }
                else
                {
                    for (int i = 0; i < currentBuildSceneData.uiBuildPathMd5.Count; i++)
                    {
                        if (currentBuildSceneData.uiBuildPathMd5[i] != localBuildSceneData.uiBuildPathMd5[i])
                        {
                            buildData.Add(localBuildSceneData.ui.name, currentBuildSceneData.uiBuildPath);
                            break;
                        }
                    }
                }
            }

            #endregion

            #region 方法Md5

            //环境Md5
            if (currentBuildSceneData.functionMd5 != localBuildSceneData.functionMd5)
            {
                buildData.Add(localBuildSceneData.function.name, currentBuildSceneData.functionBuildPath);
            }
            else
            {
                if (currentBuildSceneData.functionBuildPathMd5.Count != localBuildSceneData.functionBuildPathMd5.Count)
                {
                    buildData.Add(localBuildSceneData.function.name, currentBuildSceneData.functionBuildPath);
                }
                else
                {
                    for (int i = 0; i < currentBuildSceneData.functionBuildPathMd5.Count; i++)
                    {
                        if (currentBuildSceneData.functionBuildPathMd5[i] != localBuildSceneData.functionBuildPathMd5[i])
                        {
                            buildData.Add(localBuildSceneData.function.name, currentBuildSceneData.functionBuildPath);
                            break;
                        }
                    }
                }
            }

            #endregion



            return buildData;
        }

        /// <summary>
        /// 重新打包文件MD5
        /// </summary>
        private BuildSceneDataInfo RedefiningBuildFileMD5(BuildSceneDataInfo buildSceneDataInfo)
        {
            BuildSceneDataInfo newBuildSceneDataInfo = new BuildSceneDataInfo();


            #region 第一次筛选全部路径

            //环境资源路径
            newBuildSceneDataInfo.envObjBuildPath = new List<string>(AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(buildSceneDataInfo.envObj)));
            //道具资源路径
            newBuildSceneDataInfo.propObjBuildPath = new List<string>(AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(buildSceneDataInfo.propObj)));
            //人物资源路径
            newBuildSceneDataInfo.characterBuildPath = new List<string>(AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(buildSceneDataInfo.character)));
            //UI资源路径
            newBuildSceneDataInfo.uiBuildPath = new List<string>(AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(buildSceneDataInfo.ui)));
            //方法资源路径
            newBuildSceneDataInfo.functionBuildPath = new List<string>(AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(buildSceneDataInfo.function)));
            //场景资源路径
            newBuildSceneDataInfo.sceneBuildPath = new List<string>(AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(buildSceneDataInfo.sceneObj)));

            #endregion

            #region 去除不需要打包的文件 /CS文件

            newBuildSceneDataInfo.envObjBuildPath = RemoveUnnecessaryFiles(newBuildSceneDataInfo.envObjBuildPath);
            newBuildSceneDataInfo.propObjBuildPath = RemoveUnnecessaryFiles(newBuildSceneDataInfo.propObjBuildPath);
            newBuildSceneDataInfo.characterBuildPath = RemoveUnnecessaryFiles(newBuildSceneDataInfo.characterBuildPath);
            newBuildSceneDataInfo.uiBuildPath = RemoveUnnecessaryFiles(newBuildSceneDataInfo.uiBuildPath);
            newBuildSceneDataInfo.functionBuildPath = RemoveUnnecessaryFiles(newBuildSceneDataInfo.functionBuildPath);
            newBuildSceneDataInfo.sceneBuildPath = RemoveUnnecessaryFiles(newBuildSceneDataInfo.sceneBuildPath);

            #endregion

            #region 剔除资源重复路径

            //筛选后道具路径
            newBuildSceneDataInfo.propObjBuildPath = DataSvc.RemoveRepeat(newBuildSceneDataInfo.propObjBuildPath, newBuildSceneDataInfo.envObjBuildPath);
            //筛选后人物路径
            newBuildSceneDataInfo.characterBuildPath =
                DataSvc.RemoveRepeat(newBuildSceneDataInfo.characterBuildPath, DataSvc.MergeList(newBuildSceneDataInfo.envObjBuildPath, newBuildSceneDataInfo.propObjBuildPath));

            //筛选后ui路径
            newBuildSceneDataInfo.uiBuildPath = DataSvc.RemoveRepeat(newBuildSceneDataInfo.uiBuildPath,
                DataSvc.MergeList(newBuildSceneDataInfo.envObjBuildPath, newBuildSceneDataInfo.propObjBuildPath, newBuildSceneDataInfo.characterBuildPath));

            //筛选后方法路径
            newBuildSceneDataInfo.functionBuildPath = DataSvc.RemoveRepeat(newBuildSceneDataInfo.functionBuildPath, DataSvc.MergeList(newBuildSceneDataInfo.envObjBuildPath,
                newBuildSceneDataInfo.propObjBuildPath,
                newBuildSceneDataInfo.characterBuildPath, newBuildSceneDataInfo.uiBuildPath));

            //筛选后场景路径
            newBuildSceneDataInfo.sceneBuildPath = DataSvc.RemoveRepeat(newBuildSceneDataInfo.sceneBuildPath, DataSvc.MergeList(newBuildSceneDataInfo.envObjBuildPath,
                newBuildSceneDataInfo.propObjBuildPath,
                newBuildSceneDataInfo.characterBuildPath, newBuildSceneDataInfo.uiBuildPath, newBuildSceneDataInfo.functionBuildPath));

            #endregion

            #region Md5赋值

            newBuildSceneDataInfo.envObjMd5 = ResSvc.FileOperation.GetMD5HashFromFile(AssetDatabase.GetAssetPath(buildSceneDataInfo.envObj));
            newBuildSceneDataInfo.propObjMd5 = ResSvc.FileOperation.GetMD5HashFromFile(AssetDatabase.GetAssetPath(buildSceneDataInfo.propObj));
            newBuildSceneDataInfo.characterMd5 = ResSvc.FileOperation.GetMD5HashFromFile(AssetDatabase.GetAssetPath(buildSceneDataInfo.character));
            newBuildSceneDataInfo.uiMd5 = ResSvc.FileOperation.GetMD5HashFromFile(AssetDatabase.GetAssetPath(buildSceneDataInfo.ui));
            newBuildSceneDataInfo.functionMd5 = ResSvc.FileOperation.GetMD5HashFromFile(AssetDatabase.GetAssetPath(buildSceneDataInfo.function));
            newBuildSceneDataInfo.sceneObjMd5 = ResSvc.FileOperation.GetMD5HashFromFile(AssetDatabase.GetAssetPath(buildSceneDataInfo.sceneObj));

            newBuildSceneDataInfo.envObjBuildPathMd5 = GetFilesMd5(newBuildSceneDataInfo.envObjBuildPath);
            newBuildSceneDataInfo.propObjBuildPathMd5 = GetFilesMd5(newBuildSceneDataInfo.propObjBuildPath);
            newBuildSceneDataInfo.characterBuildPathMd5 = GetFilesMd5(newBuildSceneDataInfo.characterBuildPath);
            newBuildSceneDataInfo.uiBuildPathMd5 = GetFilesMd5(newBuildSceneDataInfo.uiBuildPath);
            newBuildSceneDataInfo.functionBuildPathMd5 = GetFilesMd5(newBuildSceneDataInfo.functionBuildPath);
            newBuildSceneDataInfo.sceneBuildPathMd5 = GetFilesMd5(newBuildSceneDataInfo.sceneBuildPath);

            #endregion

            return newBuildSceneDataInfo;
        }

        /// <summary>
        /// 移除不需要的打包的文件
        /// </summary>
        /// <param name="buildPath"></param>
        /// <returns></returns>
        private List<string> RemoveUnnecessaryFiles(List<string> buildPath)
        {
            List<string> removeFiles = new List<string>();

            foreach (string filePath in buildPath)
            {
                if (filePath.Contains(".cs") || filePath.Contains(".asset"))
                {
                    removeFiles.Add(filePath);
                }
            }

            foreach (string removeFile in removeFiles)
            {
                buildPath.Remove(removeFile);
            }

            return buildPath;
        }

        /// <summary>
        /// 返回文件列表的Md5
        /// </summary>
        /// <param name="filePaths"></param>
        /// <returns></returns>
        private List<string> GetFilesMd5(List<string> filePaths)
        {
            List<string> fileMd5 = new List<string>();
            for (int i = 0; i < filePaths.Count; i++)
            {
                fileMd5.Add(ResSvc.FileOperation.GetMD5HashFromFile(filePaths[i]));
            }

            return fileMd5;
        }

        /// <summary>
        /// 保存打包信息
        /// </summary>
        private void SaveBuildInfo(BuildSceneDataInfo currentBuildSceneData, BuildSceneDataInfo localBuildSceneData)
        {
            localBuildSceneData.sceneObjMd5 = currentBuildSceneData.sceneObjMd5;
            localBuildSceneData.sceneBuildPath = currentBuildSceneData.sceneBuildPath;
            localBuildSceneData.sceneBuildPathMd5 = currentBuildSceneData.sceneBuildPathMd5;

            localBuildSceneData.envObjMd5 = currentBuildSceneData.envObjMd5;
            localBuildSceneData.envObjBuildPath = currentBuildSceneData.envObjBuildPath;
            localBuildSceneData.envObjBuildPathMd5 = currentBuildSceneData.envObjBuildPathMd5;

            localBuildSceneData.propObjMd5 = currentBuildSceneData.propObjMd5;
            localBuildSceneData.propObjBuildPath = currentBuildSceneData.propObjBuildPath;
            localBuildSceneData.propObjBuildPathMd5 = currentBuildSceneData.propObjBuildPathMd5;

            localBuildSceneData.characterMd5 = currentBuildSceneData.characterMd5;
            localBuildSceneData.characterBuildPath = currentBuildSceneData.characterBuildPath;
            localBuildSceneData.characterBuildPathMd5 = currentBuildSceneData.characterBuildPathMd5;

            localBuildSceneData.uiMd5 = currentBuildSceneData.uiMd5;
            localBuildSceneData.uiBuildPath = currentBuildSceneData.uiBuildPath;
            localBuildSceneData.uiBuildPathMd5 = currentBuildSceneData.uiBuildPathMd5;

            localBuildSceneData.functionMd5 = currentBuildSceneData.functionMd5;
            localBuildSceneData.functionBuildPath = currentBuildSceneData.functionBuildPath;
            localBuildSceneData.functionBuildPathMd5 = currentBuildSceneData.functionBuildPathMd5;
        }

        /// <summary>
        /// 打包场景数据
        /// </summary>
        private static void BuildSceneData(Dictionary<string, List<string>> sceneObjectPathDic)
        {
            AssetBundleBuild[] buildMap = new AssetBundleBuild[sceneObjectPathDic.Count];
            for (int i = 0; i < buildMap.Length; i++)
            {
                string assetBundleName = sceneObjectPathDic.Keys.ToArray()[i];
                buildMap[i].assetBundleName = assetBundleName;
                buildMap[i].assetNames = sceneObjectPathDic[assetBundleName].ToArray();
            }

            Debug.Log("打包文件个数:" + sceneObjectPathDic.Count);

            if (buildMap.Length > 0)
            {
                BuildPipeline.BuildAssetBundles(_buildSceneData.assetBundlePath, buildMap, BuildAssetBundleOptions.None, BuildTarget.WebGL);
            }
        }
    }
}