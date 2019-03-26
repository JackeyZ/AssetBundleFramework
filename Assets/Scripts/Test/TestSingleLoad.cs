using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSingleLoad : MonoBehaviour {

    private AssetBundleFramework.SingleABLoader _loadObj = null;
    private AssetBundleFramework.SingleABLoader _loadObj2 = null;
    private string _assetbundleDependName = "commonscene/materials.u3dassetbundle";
    private string _assetbundleName = "commonscene/prefabs.u3dassetbundle";
    private string _assetName = "Cube2";

#region 加载无依赖AB包
    //void Start()
    //{
    //    _loadObj = new AssetBundleFramework.SingleABLoader(_assetbundleName, LoadComplete);
    //    StartCoroutine(_loadObj.LoadAssetBundle());

    //}

    //void LoadComplete(string abName)
    //{
    //    GameObject prefab = _loadObj.LoadAsset(_assetName) as GameObject;
    //    GameObject.Instantiate(prefab);
    //    prefab.transform.position = new Vector3(0, 0, 0);
    //}
#endregion

    void Start()
    {
        _loadObj = new AssetBundleFramework.SingleABLoader(_assetbundleDependName, LoadComplete1);
        StartCoroutine(_loadObj.LoadAssetBundle());
    }

    void LoadComplete1(string abName)
    {
        _loadObj2 = new AssetBundleFramework.SingleABLoader(_assetbundleName, LoadComplete2);
        StartCoroutine(_loadObj2.LoadAssetBundle());
    }
    void LoadComplete2(string abName)
    {
        GameObject prefab = _loadObj2.LoadAsset(_assetName) as GameObject;
        GameObject.Instantiate(prefab);
        prefab.transform.position = new Vector3(0, 0, 0);
        string[] resArray = _loadObj2.RetriveAllAssetName();
        foreach (var item in resArray)
        {
            Debug.LogError(item);
        }
        StartCoroutine("UnloadAB");
    }

    IEnumerator UnloadAB()
    {
        yield return new WaitForSeconds(5f);
        _loadObj2.DisposeAll();
        _loadObj.DisposeAll();
    }

}
