using UnityEngine;
using UnityEngine.UI;
using XxSlitFrame.Tools;

namespace XxSlitFrame.View.InitView
{
    public class ExcessiveScene : SingletonBaseWindow<ExcessiveScene>
    {
        private Scrollbar _sceneLoadProgress;

        public override void Init()
        {
        }

        protected override void InitView()
        {
            BindUi(ref _sceneLoadProgress, "SceneLoadProgress");
        }

        protected override void InitListener()
        {
        }

        /// <summary>
        /// 更新场景进度
        /// </summary>
        /// <param name="sceneProgress"></param>
        public void UpdateAsyncLoadProgress(float sceneProgress)
        {
            _sceneLoadProgress.size = sceneProgress;
        }
    }
}