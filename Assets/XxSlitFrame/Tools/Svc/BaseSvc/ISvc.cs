namespace XxSlitFrame.Tools.Svc.BaseSvc
{
    public interface ISvc
    {
        /// <summary>
        /// 开启服务
        /// </summary>
        void StartSvc();

        /// <summary>
        /// 服务初始化
        /// </summary>
        void InitSvc();
    }
}