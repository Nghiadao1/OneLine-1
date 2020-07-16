using System.IO;
using UnityEngine;

/// <summary>
/// This class is the one that will load the different assetBundles from the 
/// StreaminsAssets folder and store them here to be used later in the game 
/// for loading different prefabs and Assets. There is a specific loading 
/// different for every platform due to the differences between the folder 
/// architecture and organization of the Operating System.
/// </summary>
public class LoadAssetBundle
{
    // The different AssetBundles loaded
    private AssetBundle _tileSet;    // AssetBundle for Tile prefabs
    private AssetBundle _pathSet;    // AssetBundle for Path prefabs
    private AssetBundle _touchSet;   // AssetBundle for Touch prefabs

    /// <summary>
    /// This function loads the different AssetBundles when the game is running 
    /// in an Android device. 
    /// 
    /// Gets the .jar path and loads the AssetBundles from it.
    /// </summary>
    /// <param name="filesPath"></param>
    public void LoadBundlesAndroid(string filesPath)
    {
        // Tile AssetBundle loading
        WWW loadingAssets = WWW.LoadFromCacheOrDownload(Path.Combine(filesPath, "Android/tile_set"), 0);
        while (!loadingAssets.isDone) { }
        _tileSet = loadingAssets.assetBundle;

        // Paths AssetBundle loading 
        loadingAssets = WWW.LoadFromCacheOrDownload(Path.Combine(filesPath, "Android/paths_set"), 0);
        while (!loadingAssets.isDone) { }
        _pathSet = loadingAssets.assetBundle;

        // Touch AssetBundle loading
        loadingAssets = WWW.LoadFromCacheOrDownload(Path.Combine(filesPath, "Android/touch_skins"), 0);
        while (!loadingAssets.isDone) { }
        _touchSet = loadingAssets.assetBundle;

        // Check if any of the AssetBundles failed to Load
        if (_tileSet == null || _pathSet == null || _touchSet == null)
        {
            Debug.LogError("Loading Asset Bundles error, AssetBundles not loaded");
        }
    }

    /// <summary>
    /// This function loads the different AssetBundles when the game is running 
    /// in a Windows device or the Editor. 
    /// </summary>
    /// <param name="filesPath"></param>
    public void LoadBundlesWindows(string filesPath)
    {
        // Load all AssetBundles and store them
        _tileSet = AssetBundle.LoadFromFile(Path.Combine(filesPath, "Windows/tile_set"));
        _pathSet = AssetBundle.LoadFromFile(Path.Combine(filesPath, "Windows/paths_set"));
        _touchSet = AssetBundle.LoadFromFile(Path.Combine(filesPath, "Windows/touch_skins"));

        // Check if any of the AssetBundles is not loaded correctly
        if (_tileSet == null || _pathSet == null || _touchSet == null)
        {
            Debug.LogError("Loading Asset Bundles error, AssetBundles not loaded");
        }
    }

    /// <summary>
    /// This function is used to get the TileAssetBundle for getting 
    /// the different skins of the tile and use them to set the Tile information.
    /// </summary>
    /// <returns>The Tile set Button</returns>
    public AssetBundle getTileSkins()
    {
        return _tileSet;
    }

    /// <summary>
    /// Gives access to the Path Skins Asset bundle, to load the different
    /// prefabs and establishes them in the Tile object. 
    /// </summary>
    /// <returns>Access to the Path Set Bundle</returns>
    public AssetBundle getPathSkins()
    {
        return _pathSet;
    }

    /// <summary>
    /// Givess access to the Touch Skins Asset Bundle, to load the different 
    /// touching feedback prefabs. 
    /// </summary>
    /// <returns></returns>
    public AssetBundle getTouchSkins()
    {
        return _touchSet;
    }
}

