using System;
using XxSlitFrame.Tools.General;
using XxSlitFrame.View;

//StartUsing
using UnityEngine.UI;

//EndUsing
namespace Custom
{
    public class ShowTips : BaseWindow
    {
//StartVariable
        private Text _tipContent;
//EndVariable

        public override void Init()
        {
        }

        protected override void InitView()
        {
//StartVariableBindPath
            BindUi(ref _tipContent, "TipContent");
//EndVariableBindPath
        }

        protected override void InitListener()
        {
            listenerSvc.AddListenerEvent<Action>(ListenerEventType.WatchVideoEnd, WatchVideoEnd);
            listenerSvc.AddListenerEvent<string>(ListenerEventType.ShowTipContent, ShowTipContent);
        }

        private void ShowTipContent(string tipContent)
        {
            _tipContent.text = tipContent;
        }

        private void WatchVideoEnd(Action action)
        {
            AddTimeTask(() =>
            {
                HideView();
                action();
            }, "视频观看成功", General.ViewSwitchTime);
        }
    }
}