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
            }

            foreach (var movedAsset in movedAssets)
            {
                MarkAssetBundlePath(movedAsset);
            }
        }

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
            //得到资源文件的相对路径
            int tmpIndex = fileInfo.FullName.IndexOf("Assets");
            string relativePath = fileInfo.FullName.Substring(tmpIndex);
            //设置AB包名和扩展名
            AssetImporter assetImporter = AssetImporter.GetAtPath(relativePath);
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
            //判断该文件是否在二级目录文件夹下
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

        private void OnPreprocessTexture()
        {
            Debug.LogError("32423");
            TextureImporter textureImporter = (TextureImporter)assetImporter;
            if (HideFlags.NotEditable == textureImporter.hideFlags)
            {
                return;
            }

            ProcessTextureType(textureImporter);
            ProcessPackingTag(textureImporter);
            //ProcessMipmap(textureImporter);
            //ProcessReadable(textureImporter, asset);
            //ProcessFilterMode(textureImporter);
            //ProcessScenePlatformSetting(textureImporter);
            //ProcessCompression(textureImporter);
            //ProcessRawImage(textureImporter, asset);
        }

        private void OnPostprocessTexture(Texture2D texture)
        {
            TextureImporter textureImporter = (TextureImporter)assetImporter;
            Debug.LogError("3345345");
            //if (tempIgnoreAssets.Contains(assetImporter.assetPath))
            //{
            //    return;
            //}

            //ProcessResizeTextureToMutiple4(textureImporter, texture);
        }

        private void ProcessTextureType(TextureImporter textureImporter)
        {
            if (textureImporter.assetPath.StartsWith(PathTool.ImagesDir))
            {
                textureImporter.textureType = TextureImporterType.Sprite;
            }
        }

        private void ProcessPackingTag(TextureImporter textureImporter)
        {
            if (TextureImporterType.Sprite != textureImporter.textureType
                || textureImporter.assetPath.Contains("/nopack")
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
