using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundles
{
    [MenuItem("Create/AssetBundle/Windows")]
    static void BuildWindowsAssetBundle()
    {
        string assetBundleDirectory = "Assets/StreamingAssets/AssetBundles/Windows/";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.None,
                                        BuildTarget.StandaloneWindows64);
    }

    [MenuItem("Create/AssetBundle/Android")]
    static void BuildAndroidAssetBundle()
    {
        string assetBundleDirectory = "Assets/StreamingAssets/AssetBundles/Android/";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.None,
                                        BuildTarget.Android);
    }
}
