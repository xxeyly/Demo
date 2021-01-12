using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBuildType : MonoBehaviour
{
    /// <summary>
    /// 打包场景类型
    /// </summary>
    public BuildType buildType;

    public enum BuildType
    {
        Env,
        Prop,
        Character,
        UI,
        Function,
    }
}