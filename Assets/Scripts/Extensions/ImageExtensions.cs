using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

/// <summary>
/// The extensions for <see cref="Image"/>.
/// </summary>
public static class ImageExtensions
{
    /// <summary>
    /// Load the sprite into this image.
    /// </summary>
    public static void LoadSprite(this Image image, string bundleName, string spriteName, Action complete = null)
    {
#if UNITY_EDITOR
        AssetBundleFramework.AssetBundleMgr.GetInstance().LoadBundleAsset(bundleName, spriteName, (obj) => {
            if (image != null && obj != null)
            {
                Texture2D tex = obj as Texture2D;
                if (tex != null)
                {
                    Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                    image.sprite = sprite as Sprite;
                    return;
                }
                AssetBundleFramework.AssetBundleMgr.GetInstance().LoadBundleAsset(bundleName, AssetBundleFramework.AssetBundleDefined.SPRITE_ATLAS_NAME, (atlasObj) =>
                {
                    SpriteAtlas atlas = atlasObj as SpriteAtlas;
                    if (atlas != null)
                    {
                        image.sprite = atlas.GetSprite(spriteName);
                        return;
                    }
                    Debug.LogError(bundleName + ":" + spriteName + "加载失败");
                });
            }
        });
        //image.sprite = AssetBundleFramework.AssetLoadInEditor.LoadObject<Sprite>(bundleName, spriteName);
#else
        AssetBundleFramework.AssetBundleMgr.GetInstance().LoadBundleAsset(bundleName, spriteName, (obj) => {
            if (image != null && obj != null)
            {
                Texture2D tex = obj as Texture2D;
                if (tex != null)
                {
                    Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                    image.sprite = sprite as Sprite;
                    return;
                }
                AssetBundleFramework.AssetBundleMgr.GetInstance().LoadBundleAsset(bundleName, AssetBundleFramework.AssetBundleDefined.SPRITE_ATLAS_NAME, (atlasObj) =>
                {
                    SpriteAtlas atlas = atlasObj as SpriteAtlas;
                    if (atlas != null)
                    {
                        image.sprite = atlas.GetSprite(spriteName);
                        return;
                    }
                    Debug.LogError(bundleName + "/" + spriteName + "加载失败");
                });
            }
        });
#endif
    }
}
