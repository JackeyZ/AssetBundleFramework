﻿/*
*Title:"Assetbundle框架"项目开发
*
*Description:
*   多个AB包加载类--------加载所需包的时候自动加载其依赖的AB包
*           1、获取AB包之间的依赖和引用关系
*           2、管理Assetbundle之间的自动连锁（递归）加载机制
*
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

namespace AssetBundleFramework
{
    public class MultiABMgr
    {
        // 单个AB包加载对象(Loader)
        private SingleABLoader _CurrentSingleABLoader;
        // 单个AB包Loader缓存集合
        private Dictionary<string, SingleABLoader> _DicSingleABLoaderCache;
        // 当前AB包名称
        private string _CurrentABName;
        // AB包对应的依赖-引用关系集合
        private Dictionary<string, ABRelation> _DicABRelation;
        // 所有AB包是否加载完成
        private DelLoadComplete _LoadAllAssetBundleComplete;

        public MultiABMgr(string abName)
        {
            _CurrentABName = abName;
            _DicSingleABLoaderCache = new Dictionary<string, SingleABLoader>();
            _DicABRelation = new Dictionary<string, ABRelation>();
        }

        public void CompleteLoadAB(string abName)
        {
            // 判断加载完成的是否是当前所需要加载的AB包，如果是则表示AB包以及它依赖的包都加载完成了
            if (_CurrentABName.Equals(abName))
            {
                if (_LoadAllAssetBundleComplete != null)
                {
                    _LoadAllAssetBundleComplete(abName);
                    ClearLoadCallBack();
                }
            }
        }

        public IEnumerator LoadAssetBundle(string abName, DelLoadComplete loadCallback = null)
        {
            if(loadCallback != null)
            {
                _LoadAllAssetBundleComplete += loadCallback;
            }
            // AB包关系的建立
            if (!_DicABRelation.ContainsKey(abName))
            {
                //建立关系
                ABRelation abRelation = new ABRelation(abName);
                _DicABRelation.Add(abName ,abRelation);
            }
            ABRelation tempABRelation = _DicABRelation[abName];     // 获得当前AB包的依赖关系

            // 得到指定AB包所有的依赖关系（查询Manifest清单）
            string[] strDependeceArray = ABManifestLoader.Instance.GetAssetBundleDependce(abName);
            foreach (var depend in strDependeceArray)
            {
                // 添加依赖项
                tempABRelation.AddDependence(depend);
                // 先加载依赖的AB包并设置被依赖关系
                yield return LoadReference(depend, abName, loadCallback);
            }

            //真正加载AB包
            if (_DicSingleABLoaderCache.ContainsKey(abName))
            {
                yield return _DicSingleABLoaderCache[abName].LoadAssetBundle();
            }
            else
            {
                _CurrentSingleABLoader = new SingleABLoader(abName, CompleteLoadAB);
                _DicSingleABLoaderCache.Add(abName, _CurrentSingleABLoader);
                yield return _CurrentSingleABLoader.LoadAssetBundle();
            }
        }

        /// <summary>
        /// 加载依赖的AB包并设置被依赖关系
        /// </summary>
        /// <param name="abName">当前包依赖的AB包名称</param>
        /// <param name="refABName">当前的AB包名称</param>
        /// <returns></returns>
        private IEnumerator LoadReference(string abName, string refABName, DelLoadComplete loadCallback)
        {
            //如果AB包已经加载
            if (_DicABRelation.ContainsKey(abName))
            {
                ABRelation tmpABRelationObj = _DicABRelation[abName];
                //添加AB包引用关系（被依赖）
                tmpABRelationObj.AddReference(refABName);
            }
            else {
                ABRelation tmpABRelationObj = new ABRelation(abName);
                tmpABRelationObj.AddReference(refABName);
                _DicABRelation.Add(abName, tmpABRelationObj);

                //开始加载依赖的包(这是一个递归调用)
                yield return LoadAssetBundle(abName, loadCallback);
            }
        }

        /// <summary>
        /// 加载（AB包中）资源
        /// </summary>
        /// <param name="abName">AssetBunlde 名称</param>
        /// <param name="assetName">资源名称</param>
        /// <param name="isCache">是否使用（资源）缓存</param>
        /// <returns></returns>
        public UnityEngine.Object LoadAsset(string abName, string assetName, bool isCache = true)
        {
            foreach (string item_abName in _DicSingleABLoaderCache.Keys)
            {
                if (abName == item_abName)
                {
                    return _DicSingleABLoaderCache[item_abName].LoadAsset(assetName, isCache);
                }
            }
            Debug.LogError(GetType() + "/LoadAsset()/找不到AsetBunder包，无法加载资源，请检查！ abName=" + abName + " assetName=" + assetName);
            return null;
        }

        public bool ContainsAssetBundle(string abName)
        {
            return _DicSingleABLoaderCache.ContainsKey(abName);
        }

        /// <summary>
        /// 释放所有的资源
        /// </summary>
        public void DisposeAllAsset()
        {
            try
            {
                //逐一释放所有加载过的AssetBundel 包中的资源
                foreach (SingleABLoader item_sABLoader in _DicSingleABLoaderCache.Values)
                {
                    item_sABLoader.DisposeAll();
                }
            }
            finally
            {
                _DicSingleABLoaderCache.Clear();
                _DicSingleABLoaderCache = null;

                //释放其他对象占用资源
                _DicABRelation.Clear();
                _DicABRelation = null;
                _CurrentABName = null;
                _LoadAllAssetBundleComplete = null;

                //卸载没有使用到的资源
                Resources.UnloadUnusedAssets();
                //强制垃圾收集
                System.GC.Collect();
            }

        }

        private void ClearLoadCallBack()
        {
            Delegate[] delArray = _LoadAllAssetBundleComplete.GetInvocationList();
            for (int i = 0; i < delArray.Length; i++)
            {
                _LoadAllAssetBundleComplete -= delArray[i] as DelLoadComplete;
            }
        }
    }
}
