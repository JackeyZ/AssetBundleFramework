using UnityEngine;
using UnityEditor;
using System.IO;

namespace AssetBundleFramework
{ 
    class GenericAssetImporter : AssetPostprocessor
    {

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var importedAsset in importedAssets)
            {
                MarkAssetBundlePath(importedAsset);
                ProcessTextureType(importedAsset);
                ProcessPackingTag(importedAsset);
            }

            foreach (var movedAsset in movedAssets)
            {
                MarkAssetBundlePath(movedAsset);
                ProcessTextureType(movedAsset);
                ProcessPackingTag(movedAsset);
            }
        }

        /// <summary>
        /// 标记ab包路径
        /// </summary>
        /// <param name="assetPath"></param>
        private static void MarkAssetBundlePath(string assetPath)
        {
            FileInfo fileInfo = new FileInfo(assetPath);
            //参数检查
            if (string.IsNullOrEmpty(fileInfo.Extension) || fileInfo.Extension == ".meta")
                return;

            string fullPath = fileInfo.FullName.Replace('\\', '/');
            if (!fullPath.StartsWith(PathTool.assetBundelResourcesRoot))
            {
                return;
            }
            //得到AB包名称
            string assetBundleName = CalculationAssetBundleName(fileInfo);
           
            //设置AB包名和扩展名
            AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
            assetImporter.assetBundleName = assetBundleName;
            if (fileInfo.Extension == ".unity")
            {
                assetImporter.assetBundleVariant = "u3dScene";
            }
            else
            {
                assetImporter.assetBundleVariant = "u3dAssetBundle";
            }
        }
        /// <summary>
        /// 计算AB包路径名
        /// </summary>
        /// <param name="fileinfo"></param>
        /// <returns></returns>
        private static string CalculationAssetBundleName(FileInfo fileinfo)
        {
            string assetBundleName = string.Empty;
            //window下的路径
            string winPath = fileinfo.FullName;
            //unity下的操作的路径
            string unityPath = winPath.Replace('\\', '/'); //反斜杠替换成斜杠
                                                           //得到资源类型文件夹后的路径（二级目录相对路径 如：Textures/1.png）
            int subIndex = unityPath.IndexOf(PathTool.assetBundelResourcesRoot) + PathTool.assetBundelResourcesRoot.Length + 1;
            string typePath = unityPath.Substring(subIndex);
            //判断该文件是否不在一级目录
            if (typePath.Contains("/"))
            {
                assetBundleName = typePath.Substring(0, typePath.LastIndexOf("/"));
            }
            //如果类型相对路径不包含‘/’则表示该资源文件放在了场景资源文件夹下，与二级目录同级
            else
            {
                assetBundleName = fileinfo.Name;
            }

            return assetBundleName;
        }
        
        /// <summary>
        /// 设置资源类型为sprite
        /// </summary>
        /// <param name="assetPath"></param>
        static private void ProcessTextureType(string assetPath)
        {
            AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
            // 判断资源类型
            if (!(assetImporter is TextureImporter))
            {
                return;
            }
            TextureImporter textureImporter = assetImporter as TextureImporter;
            if (textureImporter.assetPath.StartsWith(PathTool.ImagesDir))
            {
                textureImporter.textureType = TextureImporterType.Sprite;
            }
        }

        /// <summary>
        /// 设置图集打包tag
        /// </summary>
        /// <param name="assetPath"></param>
        static  private void ProcessPackingTag(string assetPath)
        {
            AssetImporter assetImporter = AssetImporter.GetAtPath(assetPath);
            // 判断资源类型
            if (!(assetImporter is TextureImporter))
            {
                return;
            }
            TextureImporter textureImporter = assetImporter as TextureImporter;
            if (textureImporter.assetPath.Contains("/nopack")
                || !textureImporter.assetPath.StartsWith(PathTool.ImagesDir))
            {
                textureImporter.spritePackingTag = string.Empty;
                return;
            }

            int subIndex = textureImporter.assetPath.IndexOf(PathTool.ImagesDir);
            string pack_tag = textureImporter.assetPath.Substring(subIndex + PathTool.ImagesDir.Length);
            pack_tag = "image" + pack_tag.Substring(0, pack_tag.LastIndexOf("/")).ToLower();
            textureImporter.spritePackingTag = pack_tag;
        }
    }
}
