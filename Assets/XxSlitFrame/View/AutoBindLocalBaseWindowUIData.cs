using UnityEngine;


namespace XxSlitFrame.View
{
    // ReSharper disable once InconsistentNaming
    public class AutoBindLocalBaseWindowUIData : AutoBindBaseWindowUIData
    {
        protected override Transform GetWindow()
        {
            return transform;
        }
    }
}