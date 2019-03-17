/*
*Title:"AssetBundle框架"项目开发
*
*Description:
*	利用WWW加载AssetBundle包
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

namespace AssetBundleFramework
{
    public class SingleAssetBundle : System.IDisposable
    {

        //引用类：资源加载类
        private AssetLoader _AssetLoader;
        //委托：

        //AssetBundle 名称
        private string _ABName;

        //AssetBundle 下载地址
        private string _ABDownLoadPath;

        //构造函数
        public SingleAssetBundle(string abName)
        {
            _ABName = abName;
            //委托定义
            
            //AB包下载路径
            _ABDownLoadPath = PathTool.GetWWWPath() + "/" + _ABName;
        }

        //加载AssetBundle资源包
        public IEnumerator LoadAssetBundle()
        {
            yield return null;
        }

        /// <summary>
        /// 加载AB包内的资源
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="isCache"></param>
        /// <returns></returns>
        public UnityEngine.Object LoadAsset(string assetName, bool isCache)
        {
            return null;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {

        }
    }

}
