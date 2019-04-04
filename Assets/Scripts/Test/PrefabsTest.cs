using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabsTest : MonoBehaviour
{
    AssetBundleFramework.DelAssetLoadComplete LoadCallBackDel;
    // Start is called before the first frame update
    void Start()
    {
        LoadCallBackDel += LoadCallBack;
        AssetBundleFramework.AssetBundleMgr.GetInstance().LoadBundleAsset("ui/prefabs.u3dassetbundle", "Canvas", LoadCallBackDel);
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
