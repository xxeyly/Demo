using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using XxSlitFrame.Tools.ConfigData.Editor;
using Object = UnityEngine.Object;

namespace XxSlitFrame.Tools.Editor.CustomEditorPanel
{
    //自定义动画编辑
    public class CustomAnimator : EditorWindow
    {
        private static bool _display;

        [MenuItem("XFrame/动画工具 #A", false, 1)]
        private static void ShowWindow()
        {
            EditorWindow window = EditorWindow.GetWindow<CustomAnimator>();
            window.minSize = new Vector2(1600, 900);
            window.maxSize = new Vector2(1600, 900);
            window.titleContent = new GUIContent() {image = null, text = "动画工具"};
            if (!_display)
            {
                window.Show();
            }
            else
            {
                window.Close();
            }

            _display = !_display;
        }

        private void OnEnable()
        {
            _display = false;
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
            if (customAnimatorClipConfig != null)
            {
                EditorUtility.SetDirty(customAnimatorClipConfig);

                for (int i = 0; i < customAnimatorClipConfig.animFbxAndAnimClipDatas.Count; i++)
                {
                    if (customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animatorClipData != null)
                    {
                        EditorUtility.SetDirty(customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animatorClipData);
                    }
                }

                // 保存所有修改
                AssetDatabase.SaveAssets();
            }
        }

        private ModelImporter _modelImporter;
        public Vector2 scrollPos = Vector2.zero;

        public CustomAnimatorClipConfig customAnimatorClipConfig;

        public Animator game;

        private void OnGUI()
        {
            #region 动画Fbx源文件

            EditorGUILayout.BeginHorizontal();

            #endregion

            #region 动画配置文件

            EditorGUILayout.LabelField("动画配置数据:", GUILayout.MaxWidth(80));

#pragma warning disable 618
            customAnimatorClipConfig = (CustomAnimatorClipConfig) EditorGUILayout.ObjectField(customAnimatorClipConfig, typeof(CustomAnimatorClipConfig));
#pragma warning restore 618
            if (customAnimatorClipConfig != null)
            {
                EditorGUILayout.LabelField("当前控制器文件", GUILayout.MaxWidth(80));

#pragma warning disable 618
                customAnimatorClipConfig.animatorController = (Animator) EditorGUILayout.ObjectField(customAnimatorClipConfig.animatorController, typeof(Animator), GUILayout.MaxWidth(300));
#pragma warning restore 618
            }

            #endregion

            #region 项目名称

            if (customAnimatorClipConfig != null)
            {
                EditorGUILayout.LabelField("Animator名称:", GUILayout.MaxWidth(80));
                customAnimatorClipConfig.animatorName = EditorGUILayout.TextField(customAnimatorClipConfig.animatorName, GUILayout.MaxWidth(470));
            }

            #endregion

            #region 动画存放路径

            if (customAnimatorClipConfig != null)
            {
                if (GUILayout.Button("选择动画存放路径", GUILayout.MaxWidth(120)))
                {
                    string projectPath = Application.dataPath;
                    string openFolderPath = EditorUtility.OpenFolderPanel("选择打包路径", "", "");
                    if (openFolderPath.Contains(projectPath))
                    {
                        //转换为相对路径
                        openFolderPath = openFolderPath.Remove(0, projectPath.Length - 6);
                        customAnimatorClipConfig.exportPath = openFolderPath;
                    }
                }

                customAnimatorClipConfig.exportPath = EditorGUILayout.TextField(customAnimatorClipConfig.exportPath, GUILayout.MaxWidth(580));
            }

            if (customAnimatorClipConfig != null)
            {
                if (GUILayout.Button("保存当前动画配置文件"))
                {
                    SaveData();
                }
            }


            EditorGUILayout.EndHorizontal();

            #endregion

            #region 配置文件

            EditorGUILayout.BeginHorizontal();

            if (customAnimatorClipConfig != null)
            {
                if (GUILayout.Button("增加动画配置文件"))
                {
                    if (customAnimatorClipConfig != null)
                    {
                        customAnimatorClipConfig.animFbxAndAnimClipDatas.Add(new AnimFbxAndAnimClipData());
                    }

                    return;
                }
            }


            if (customAnimatorClipConfig != null)
            {
                if (GUILayout.Button("分割动画并配置Animator数据"))
                {
                    BuildAnim();
                }
            }

            #endregion

            EditorGUILayout.EndHorizontal();

            #region 动画帧

            #region 动画配置文件

            EditorGUILayout.BeginVertical();

            #region 单个动画配置文件

            EditorGUILayout.BeginVertical();

            #region 头文件

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            if (customAnimatorClipConfig != null)
            {
                for (int i = 0; i < customAnimatorClipConfig.animFbxAndAnimClipDatas.Count; i++)
                {
                    EditorGUILayout.BeginVertical();

                    EditorGUILayout.BeginHorizontal(); //1
                    EditorGUILayout.LabelField("动画Fbx源文件:", GUILayout.MaxWidth(100));
#pragma warning disable 618
                    customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animFbx =
                        (GameObject) EditorGUILayout.ObjectField(customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animFbx, typeof(GameObject), GUILayout.MaxWidth(400));
#pragma warning restore 618

                    EditorGUILayout.LabelField("动画配置数据:", GUILayout.MaxWidth(80));

#pragma warning disable 618
                    customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animatorClipData =
                        (AnimatorClipData) EditorGUILayout.ObjectField(customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animatorClipData, typeof(AnimatorClipData), GUILayout.MaxWidth(400));
#pragma warning restore 618

                    if (GUILayout.Button("增加关键帧", GUILayout.MaxWidth(100)))
                    {
                        if (customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animatorClipData != null)
                        {
                            customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animatorClipData.animatorClipDataInfos.Add(new AnimatorClipData.AnimatorClipDataInfo()
                                {animatorControllerParameterType = AnimatorControllerParameterType.Trigger});
                        }

                        return;
                    }

                    if (GUILayout.Button("删除动画配置", GUILayout.MaxWidth(110)))
                    {
                        customAnimatorClipConfig.animFbxAndAnimClipDatas.RemoveAt(i);
                        return;
                    }

                    EditorGUILayout.EndHorizontal(); //1

                    EditorGUILayout.BeginHorizontal(); //2

                    EditorGUILayout.EndHorizontal(); //2
                    if (customAnimatorClipConfig != null && customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animatorClipData != null)
                    {
                        for (int j = 0; j < customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animatorClipData.animatorClipDataInfos.Count; j++)
                        {
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField("动画属性名称", GUILayout.MaxWidth(70));
                            customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animatorClipData.animatorClipDataInfos[j].animatorClipName =
                                EditorGUILayout.TextField(customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animatorClipData.animatorClipDataInfos[j].animatorClipName);
                            EditorGUILayout.LabelField("属性类型", GUILayout.MaxWidth(50));
                            customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animatorClipData.animatorClipDataInfos[j].animatorControllerParameterType =
                                (AnimatorControllerParameterType) EditorGUILayout.EnumPopup(
                                    customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animatorClipData.animatorClipDataInfos[j].animatorControllerParameterType,
                                    GUILayout.MaxWidth(100));
                            EditorGUILayout.LabelField("固定过渡持续时间", GUILayout.MaxWidth(100));
                            customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animatorClipData.animatorClipDataInfos[j].fixedDuration =
                                EditorGUILayout.Toggle(customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animatorClipData.animatorClipDataInfos[j].fixedDuration, GUILayout.MaxWidth(10));
                            EditorGUILayout.LabelField("持续过度时间", GUILayout.MaxWidth(70));
                            customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animatorClipData.animatorClipDataInfos[j].transitionDuration =
                                EditorGUILayout.FloatField(customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animatorClipData.animatorClipDataInfos[j].transitionDuration, GUILayout.MaxWidth(30));
                            EditorGUILayout.LabelField("循环", GUILayout.MaxWidth(30));
                            customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animatorClipData.animatorClipDataInfos[j].animatorClipIsLoop =
                                EditorGUILayout.Toggle(customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animatorClipData.animatorClipDataInfos[j].animatorClipIsLoop, GUILayout.MaxWidth(10));
                            EditorGUILayout.LabelField("倒放", GUILayout.MaxWidth(30));
                            customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animatorClipData.animatorClipDataInfos[j].animatorClipIsRewind =
                                EditorGUILayout.Toggle(customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animatorClipData.animatorClipDataInfos[j].animatorClipIsRewind, GUILayout.MaxWidth(10));
                            EditorGUILayout.LabelField("开始帧", GUILayout.MaxWidth(40));
                            customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animatorClipData.animatorClipDataInfos[j].animatorClipFirstFrame =
                                EditorGUILayout.IntField(customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animatorClipData.animatorClipDataInfos[j].animatorClipFirstFrame, GUILayout.MaxWidth(40));
                            EditorGUILayout.LabelField("结束帧", GUILayout.MaxWidth(40));
                            customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animatorClipData.animatorClipDataInfos[j].animatorClipLastFrame =
                                EditorGUILayout.IntField(customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animatorClipData.animatorClipDataInfos[j].animatorClipLastFrame, GUILayout.MaxWidth(40));
                            if (GUILayout.Button("增加关键帧"))
                            {
                                customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animatorClipData.animatorClipDataInfos.Insert(j + 1,
                                    new AnimatorClipData.AnimatorClipDataInfo() {animatorControllerParameterType = AnimatorControllerParameterType.Trigger});
                            }

                            if (GUILayout.Button("删除关键帧"))
                            {
                                customAnimatorClipConfig.animFbxAndAnimClipDatas[i].animatorClipData.animatorClipDataInfos.RemoveAt(j);
                            }

                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }
            }


            EditorGUILayout.BeginVertical();

            EditorGUILayout.EndScrollView();

            #endregion


            EditorGUILayout.BeginVertical();

            #endregion

            EditorGUILayout.EndVertical();

            #endregion

            #endregion

            #region 配置表更新

            if (customAnimatorClipConfig != null)
            {
                EditorUtility.SetDirty(customAnimatorClipConfig);
            }

            #endregion
        }

        /// <summary>
        /// 分割并配置动画数据
        /// </summary>
        private void BuildAnim()
        {
            if (Directory.Exists(customAnimatorClipConfig.exportPath) && customAnimatorClipConfig.animatorName != "")
            {
                AnimatorController animatorController =
                    AnimatorController.CreateAnimatorControllerAtPath(customAnimatorClipConfig.exportPath + "/" + customAnimatorClipConfig.animatorName + ".controller");
                AnimatorStateMachine rootStateMachine = animatorController.layers[0].stateMachine;


                foreach (AnimFbxAndAnimClipData animFbxAndAnimClipData in customAnimatorClipConfig.animFbxAndAnimClipDatas)
                {
                    if (animFbxAndAnimClipData.animatorClipData != null)
                    {
                        _modelImporter = (ModelImporter) AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(animFbxAndAnimClipData.animFbx));
                        if (_modelImporter != null)
                        {
                            _modelImporter.animationType = ModelImporterAnimationType.Generic;
                            _modelImporter.generateAnimations = ModelImporterGenerateAnimations.GenerateAnimations;
                            ModelImporterClipAnimation[] animations = new ModelImporterClipAnimation[animFbxAndAnimClipData.animatorClipData.animatorClipDataInfos.Count];


                            for (int i = 0; i < animFbxAndAnimClipData.animatorClipData.animatorClipDataInfos.Count; i++)
                            {
                                animations[i] = SetClipAnimation(animFbxAndAnimClipData.animatorClipData.animatorClipDataInfos[i].animatorClipName,
                                    animFbxAndAnimClipData.animatorClipData.animatorClipDataInfos[i].animatorClipFirstFrame,
                                    animFbxAndAnimClipData.animatorClipData.animatorClipDataInfos[i].animatorClipLastFrame,
                                    animFbxAndAnimClipData.animatorClipData.animatorClipDataInfos[i].animatorClipIsLoop);
                            }

                            _modelImporter.clipAnimations = animations;
                            _modelImporter.SaveAndReimport();
                            //该动画文件下的所有文件
                            List<Object> allAnimObject = new List<Object>(AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(animFbxAndAnimClipData.animFbx)));
                            List<string> allAnimClipName = GetAllAnimClipName();
                            Dictionary<string, AnimationClip> animationClipDic = BuildingAnimationClips(allAnimObject, allAnimClipName);

                            for (int i = 0; i < animFbxAndAnimClipData.animatorClipData.animatorClipDataInfos.Count; i++)
                            {
                                //添加 参数
                                animatorController.AddParameter(animFbxAndAnimClipData.animatorClipData.animatorClipDataInfos[i].animatorClipName,
                                    animFbxAndAnimClipData.animatorClipData.animatorClipDataInfos[i].animatorControllerParameterType);
                                //添加 片段
                                AnimatorState state = rootStateMachine.AddState(animFbxAndAnimClipData.animatorClipData.animatorClipDataInfos[i].animatorClipName);
                                //动画是否倒放
                                state.speed = animFbxAndAnimClipData.animatorClipData.animatorClipDataInfos[i].animatorClipIsRewind ? -1 : 1;
                                //设置动画
                                state.motion = animationClipDic[animFbxAndAnimClipData.animatorClipData.animatorClipDataInfos[i].animatorClipName];
                                // 关联片段 
                                AnimatorStateTransition animatorStateTransition = rootStateMachine.AddAnyStateTransition(state);
                                //设置关联参数
                                animatorStateTransition.AddCondition(AnimatorConditionMode.If, 0, animFbxAndAnimClipData.animatorClipData.animatorClipDataInfos[i].animatorClipName);
                                //设置持续时间
                                animatorStateTransition.duration = animFbxAndAnimClipData.animatorClipData.animatorClipDataInfos[i].transitionDuration;
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("当前Fbx文件没有选择");
                    }
                }

                if (customAnimatorClipConfig.animatorController != null)
                {
                    customAnimatorClipConfig.animatorController.runtimeAnimatorController = animatorController;
                }
            }
        }


        #region 设置动画片段

        private ModelImporterClipAnimation SetClipAnimation(string clipName, int firstFrame, int lastFrame, bool isLoop)
        {
            ModelImporterClipAnimation clip = new ModelImporterClipAnimation {name = clipName, firstFrame = firstFrame, lastFrame = lastFrame, loopTime = isLoop};

            if (isLoop)
            {
                clip.wrapMode = WrapMode.Loop;
            }
            else
            {
                clip.wrapMode = WrapMode.Default;
            }

            return clip;
        }

        #endregion

        #region 构建动画片段键值对

        private Dictionary<string, AnimationClip> BuildingAnimationClips(List<Object> allFbxObject, List<string> animClipNames)
        {
            Dictionary<string, AnimationClip> animationClipDic = new Dictionary<string, AnimationClip>();
            AnimationClip animationClip;
            foreach (string animClipName in animClipNames)
            {
                foreach (Object objectClip in allFbxObject)
                {
                    if (objectClip.name == animClipName)
                    {
                        animationClip = objectClip as AnimationClip;
                        animationClipDic.Add(animClipName, animationClip);
                        break;
                    }
                }
            }

            return animationClipDic;
        }

        /// <summary>
        /// 获得所有动画片段的名字
        /// </summary>
        /// <returns></returns>
        private List<string> GetAllAnimClipName()
        {
            List<string> animNames = new List<string>();
            foreach (AnimFbxAndAnimClipData animFbxAndAnimClipData in customAnimatorClipConfig.animFbxAndAnimClipDatas)
            {
                foreach (AnimatorClipData.AnimatorClipDataInfo animatorClipDataInfo in animFbxAndAnimClipData.animatorClipData.animatorClipDataInfos)
                {
                    animNames.Add(animatorClipDataInfo.animatorClipName);
                }
            }

            return animNames;
        }

        #endregion
    }
}