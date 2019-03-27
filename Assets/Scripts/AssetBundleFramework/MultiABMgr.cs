/*
*Title:"Assetbundle框架"项目开发
*
*Description:
*   多个AB包加载类
*           1、获取AB包之间的依赖和引用关系
*           2、管理Assetbundle之间的自动连锁（递归）加载机制
*
*Date:2019
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
    public class MultiABMgr
    {
        // 单个AB包加载对象(Loader)
        private SingleABLoader _CurrentSingleABLoader;
        // AB包Loader缓存集合
        private Dictionary<string, SingleABLoader> _DicSingleABLoaderCache;
        // 当前AB包名称
        private string _CurrentABName;
        // AB包对应的依赖-引用关系集合
        private Dictionary<string, ABRelation> _DicABRelation;
        // 所有AB包是否加载完成
        private DelLoadComplete _LoadAllAssetBundleComplete;

        public MultiABMgr(string abName, DelLoadComplete loadComplete)
        {
            _CurrentABName = abName;
            _LoadAllAssetBundleComplete = loadComplete;
            _DicSingleABLoaderCache = new Dictionary<string, SingleABLoader>();
            _DicABRelation = new Dictionary<string, ABRelation>();
        }

        public void CompleteLoadAB(string abName)
        {
            if (_CurrentABName.Equals(abName))
            {
                if(_LoadAllAssetBundleComplete != null)
                {
                    _LoadAllAssetBundleComplete(abName);
                }
            }
        }

        public IEnumerator LoadAssetBundle(string abName)
        {
            // AB包关系的建立
            if (!_DicABRelation.ContainsKey(abName))
            {
                //建立关系
                ABRelation abRelation = new ABRelation(abName);
                _DicABRelation.Add(abName ,abRelation);
            }
            ABRelation tempABRelation = _DicABRelation[abName];

            // 得到指定AB包所有的依赖关系引用关系（查询Manifest清单）
            string[] strDependeceArray = ABManifestLoader.Instance.GetAssetBundleDependce(abName);
            foreach (var depend in strDependeceArray)
            {
                // 添加依赖项
                tempABRelation.AddDependence(depend);
                // 添加引用项

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
            
            yield return null;
        }
    }
}
