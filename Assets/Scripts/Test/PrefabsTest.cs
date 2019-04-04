using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabsTest : MonoBehaviour
{
    Action<UnityEngine.Object> LoadCallBackDel;
    // Start is called before the first frame update
    void Start()
    {
        LoadCallBackDel += LoadCallBack;;
        PrefabLoader.LoadPrefab("ui/prefabs.u3dassetbundle", "Canvas", LoadCallBackDel);
    }

    void LoadCallBack(UnityEngine.Object obj)
    {
        GameObject go = Instantiate<GameObject>(obj as GameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
