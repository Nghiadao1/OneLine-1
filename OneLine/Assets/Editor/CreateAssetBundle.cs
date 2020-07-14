using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundle
{
    [MenuItem("AssetBundle/Create Asset Bundles For Windows")]
    static void BuildWindowsAssetBundle()
    {
        string assetBundleDirectory = "Assets/AssetBundles/Windows/";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.None,
                                        BuildTarget.StandaloneWindows64);
    }

    [MenuItem("AssetBundle/Create Asset Bundles For Android")]
    static void BuildAndroidAssetBundle()
    {
        string assetBundleDirectory = "Assets/AssetBundles/Android/";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.None,
                                        BuildTarget.Android);
    }
}
