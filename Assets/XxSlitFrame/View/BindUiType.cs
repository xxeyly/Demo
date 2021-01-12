using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace XxSlitFrame.View
{
    public class BindUiType : MonoBehaviour
    {
        public enum UiType
        {
            Button,
            Image,
            Text,
            Toggle,
            RawImage,
            Scrollbar,
            Dropdown,
            InputField,
            ScrollRect,
            GameObject,
            Slider,
            ChildList,
            VideoPlayer
        }

        [FormerlySerializedAs("Type")] public UiType type;
        public LocalBaseWindow childType;
    }
}