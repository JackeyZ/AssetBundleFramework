/*
*Title:""项目开发
*
*Description:
*	[描述]
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
using UnityEditor;

namespace AssetBundleFramework
{
    public class AssetBundleConst
    {
        private static string outPutPath = string.Empty;
        public static string assetBundelResourcesRoot = Application.dataPath + "/" + "AssetBundleResources";
        public static BuildTarget buildTarget = BuildTarget.StandaloneWindows64;
        public static string ImagesDir = "Assets/AssetBundleResources/UI/Images";

        public static string OutPutPath
        {
            get
            {
                return GetPlatformPath() + "/" + GetPlatformName();
            }
        }

        public static string GetPlatformPath()
        {
            string platformPath = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    platformPath = Application.streamingAssetsPath;
                    break;
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.Android:
                    platformPath = Application.persistentDataPath;
                    break;
            }
            return platformPath;
        }

        public static string GetPlatformName()
        {
            string platformName = string.Empty;
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsEditor:
                    platformName = "Windows";
                    break;
                case RuntimePlatform.IPhonePlayer:
                    platformName = "IPhone";
                    break;
                case RuntimePlatform.Android:
                    platformName = "Android";
                    break;
            }
            return platformName;
        }
    }
}
