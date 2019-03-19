﻿/*
*Title:"Assetbundle框架"项目开发
*
*Description:
*   名称：AB资源加载类
*	    1、加载与管理AB资源
*	    2、加载具有“缓存功能”的资源，带选用参数
*       3、卸载、释放AB资源
*       4、查看当前AB资源
*Date:2019
*
*Version:0.1
*
*Modify Recoder:
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetLoader : System.IDisposable {
    private AssetBundle _CurrentAssetBundle;    //当前AB包
    private Hashtable _Ht;                      //缓存容器集合
    
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="ab">给定用WWW加载的AssetBundle的实例</param>
    public AssetLoader(AssetBundle ab)
    {
        if (ab != null)
        {
            _CurrentAssetBundle = ab;
            _Ht = new Hashtable();
        }
        else
        {
            Debug.LogError(GetType() + "AssetLoader构造函数参数为null");
        }
    }

    /// <summary>
    /// 加载当前AB包中指定的资源
    /// </summary>
    /// <param name="assetName">资源名</param>
    /// <param name="isCache">是否开启缓存</param>
    /// <returns></returns>
    public UnityEngine.Object LoadAsset(string assetName, bool isCache = false)
    {
        return LoadResource<UnityEngine.Object>(assetName, isCache);
    }

    /// <summary>
    /// 加载当前AB包的资源
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    /// <param name="assetName">资源名称</param>
    /// <param name="isCache">是否需要缓存</param>
    /// <returns>加载的资源</returns>
    private T LoadResource<T>(string assetName, bool isCache = false) where T : UnityEngine.Object
    {
        //缓存集合是否已经存在
        if (_Ht.Contains(assetName))
        {
            return _Ht[assetName] as T;
        }

        //加载资源
        T tmpTResource = _CurrentAssetBundle.LoadAsset<T>(assetName);
        //判断是否加入缓存集合
        if(tmpTResource != null && isCache)
        {
            _Ht.Add(assetName, tmpTResource);
        }
        else if(tmpTResource == null)
        {
            Debug.LogError(GetType() + "资源：" + assetName + "加载失败请检查");
        }
        return tmpTResource;
    }

    //检查某个资源是否已经卸载了
    public bool AssetIsUnLoad(UnityEngine.Object asset)
    {
        return false;
    }

    //卸载当前Assetbundle内存镜像资源
    public void Dispose()
    {
        throw new NotImplementedException();
    }

    //释放当前Assetbundle内存镜像资源和内存资源
    public void DisposeAll()
    {

    }

    //查询当前AB包中的所有资源
    public void CheckAssetbundleAllAsset()
    {

    }
}