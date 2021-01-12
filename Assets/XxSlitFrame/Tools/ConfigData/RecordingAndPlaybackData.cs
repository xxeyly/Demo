using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecordingAndPlaybackData", menuName = "配置文件/录制回放", order = 1)]
public class RecordingAndPlaybackData : ScriptableObject
{
    public List<GameObject> operationObj;

    public void GetAllClient()
    {
        foreach (GameObject gameObject in operationObj)
        {
            
        }
    }
}