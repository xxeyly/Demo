using UnityEngine;

namespace XxSlitFrame.Tools
{
    public abstract class StartSingleton : MonoBehaviour, IStartSingleton
    {
        public abstract void StartSvc();
        public abstract void Init();
        
    }
}