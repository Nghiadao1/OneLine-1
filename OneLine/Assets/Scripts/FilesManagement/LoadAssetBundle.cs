using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class LoadAssetBundle
{
    private AssetBundle _tileSet;
    private AssetBundle _pathSet;
    private AssetBundle _touchSet;

    public void LoadBundlesAndroid(string filesPath)
    {
        WWW loadingAssets = WWW.LoadFromCacheOrDownload(Path.Combine(filesPath, "Android/tile_set"), 0);
        while (!loadingAssets.isDone) { }
        _tileSet = loadingAssets.assetBundle;

        loadingAssets = WWW.LoadFromCacheOrDownload(Path.Combine(filesPath, "Android/paths_set"), 0);
        while (!loadingAssets.isDone) { }
        _pathSet = loadingAssets.assetBundle;

        loadingAssets = WWW.LoadFromCacheOrDownload(Path.Combine(filesPath, "Android/touch_skins"), 0);
        while (!loadingAssets.isDone) { }
        _touchSet = loadingAssets.assetBundle;

        if(_tileSet == null || _pathSet == null || _touchSet == null)
        {
            Debug.LogError("Loading Asset Bundles error, AssetBundles not loaded");
        }
    }

    public void LoadBundlesWindows(string filesPath)
    {
        _tileSet = AssetBundle.LoadFromFile(Path.Combine(filesPath, "Windows/tile_set"));
        _pathSet = AssetBundle.LoadFromFile(Path.Combine(filesPath, "Windows/paths_set"));
        _touchSet = AssetBundle.LoadFromFile(Path.Combine(filesPath, "Windows/touch_skins"));

        if (_tileSet == null || _pathSet == null || _touchSet == null)
        {
            Debug.LogError("Loading Asset Bundles error, AssetBundles not loaded");
        }
    }

    public AssetBundle getTileSkins()
    {
        return _tileSet;
    }

    public AssetBundle getPathSkins()
    {
        return _pathSet;
    }

    public AssetBundle getTouchSkins()
    {
        return _touchSet;
    }
}
