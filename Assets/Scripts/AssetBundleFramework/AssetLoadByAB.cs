using System.Collections;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR_WIN
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

public class AssetLoadByAB {

    public T LoadObject<T>(string bundleName, string assetName) where T : UnityEngine.Object
    {
#if UNITY_EDITOR_WIN
        string assetPath = this.GetAssetPath(bundleName, assetName);
        if (!string.IsNullOrEmpty(assetPath))
            return AssetDatabase.LoadAssetAtPath<T>(assetPath);
#endif
        return (T)null;
    }

#if UNITY_EDITOR_WIN
    /// <summary>
    /// 根据ab路径和资源名称返回资源路径（Assets/...）
    /// </summary>
    /// <param name="bundleName">assetbundle名</param>
    /// <param name="assetName">资源文件名，可带扩展名</param>
    /// <returns></returns>
    public string GetAssetPath(string bundleName, string assetName)
    {
        string[] paths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(bundleName, Path.GetFileNameWithoutExtension(assetName));
        if(paths.Length == 0)
        {
            Debug.LogError("AB包路径有误：" + bundleName + "/" + assetName);
            return null;
        }
        string extension = Path.GetExtension(assetName);
        foreach (string path in paths)
        {
            if (string.IsNullOrEmpty(extension))
            {
                return path;
            }
            else if (Path.GetExtension(path) == extension)
            {
                return path;
            }
        }
        return null;
    }
#endif
}
