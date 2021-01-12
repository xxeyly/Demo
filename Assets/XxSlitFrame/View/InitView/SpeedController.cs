using UnityEngine.UI;

namespace XxSlitFrame.View.InitView
{
    public class SpeedController : BaseWindow
    {
        private Slider _cameraSpeed;

        public override void Init()
        {
            _cameraSpeed.value = persistentDataSvc.cameraSpeed;
        }

        protected override void InitView()
        {
            BindUi(ref _cameraSpeed, "CameraSpeed");
        }

        protected override void InitListener()
        {
            _cameraSpeed.onValueChanged.AddListener(OnChangeView);
        }

        private void OnChangeView(float value)
        {
            persistentDataSvc.cameraSpeed = value;
        }
    }
}