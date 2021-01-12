using UnityEngine;

namespace XxSlitFrame.Tools.Svc.BaseSvc
{
    public abstract class SvcBase : MonoBehaviour, ISvc
    {
        public bool init;
        public abstract void StartSvc();


        public abstract void InitSvc();
    }
}