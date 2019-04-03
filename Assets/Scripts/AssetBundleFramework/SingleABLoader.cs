/*
*Title:"AssetBundle框架"项目开发
*
*Description:
*	名称：单个AB包加载类
*   作用：利用WebRequest下载单个AssetBundle包
*
*Date:2017
*
*Version:0.1
*
*Modify Recoder:
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;

namespace AssetBundleFramework
{
    public class SingleABLoader : System.IDisposable
    {
        //引用类：包内资源加载类
        private AssetLoader _AssetLoader;
        //委托：AB包加载完成之后调用
        DelLoadComplete _LoadCallback;
        
        //AssetBundle 名称
        private string _ABName;

        //AssetBundle 下载地址
        private string _ABDownLoadPath;

        private bool _Loaded = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="abName">AB包名称</param>
        /// <param name="loadCallback">下载完成回调</param>
        public SingleABLoader(string abName, DelLoadComplete loadCallback)
        {
            _ABName = abName;
            _LoadCallback = loadCallback;
            //AB包下载路径
            _ABDownLoadPath = PathTool.GetWWWPath() + "/" + _ABName;
        }
        
        /// <summary>
        /// 外部调用，加载AssetBundle资源包
        /// </summary>
        /// <returns></returns>
        public IEnumerator LoadAssetBundle()
        {
            if(_Loaded == false)
            {
                _Loaded = true;
                UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(_ABDownLoadPath);
                yield return request.SendWebRequest();
                //取得ab的方式1
                //AssetBundle ab_prefab = DownloadHandlerAssetBundle.GetContent(request);
                //取得ab的方式2
                AssetBundle ab_prefab = (request.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
                if (ab_prefab == null)
                {
                    _Loaded = false;
                    Debug.LogError(GetType() + "LoadAssetBundle失败：" + _ABDownLoadPath);
                    yield return null;
                }
                _AssetLoader = new AssetLoader(ab_prefab);
                if (_LoadCallback != null)
                {
                    _LoadCallback(_ABName);
                }
            }
        }

        /// <summary>
        /// 外部调用加载AB包内的资源
        /// </summary>
        /// <param name="assetName">包内资源名称</param>
        /// <param name="isCache">是否缓存包内资源</param>
        /// <returns></returns>
        public UnityEngine.Object LoadAsset(string assetName, bool isCache = true)
        {
            if (_AssetLoader != null) { 
                return _AssetLoader.LoadAsset(assetName, isCache);
            }
            Debug.LogError(GetType() + "/LoadAsset()/ 参数_AssetLoader == null  ,请检查！");
            return null;
        }

        /// <summary>
        /// 卸载ab资源
        /// </summary>
        /// <param name="asset"></param>
        public void UnLoadAsset(UnityEngine.Object asset)
        {
            if(_AssetLoader != null)
            {
                _AssetLoader.UnLoadAsset(asset);
            }
            else
            {
                Debug.LogError(GetType() + "：_AssetLoader is null");
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (_AssetLoader != null)
            {
                _AssetLoader.Dispose();
                _AssetLoader = null;
            }
            else
            {
                Debug.LogError(GetType() + "：_AssetLoader is null");
            }
        }

        /// <summary>
        /// 释放所有资源
        /// </summary>
        public void DisposeAll()
        {
            if (_AssetLoader != null)
            {
                Debug.LogError("DisposeAll");
                _AssetLoader.DisposeAll();
                _AssetLoader = null;
            }
            else
            {
                Debug.LogError(GetType() + "：_AssetLoader is null");
            }
        }

        /// <summary>
        /// 查询AB包中的所有资源
        /// </summary>
        /// <returns></returns>
        public string[] RetriveAllAssetName()
        {
            if (_AssetLoader != null)
            {
                return _AssetLoader.RetriveAllAssetName();
            }
            Debug.LogError(GetType() + "：_AssetLoader is null");
            return null;
        }
    }

}
