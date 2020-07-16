using System.IO;
using UnityEditor;

/// <summary>
/// Tool that creates the different AssetBundles defined in the editor. 
/// 
/// Creates a new MenuOption that drops down two different options for
/// creating the AssetBundles specific for each platform. 
/// </summary>
public class CreateAssetBundles
{
    /// <summary>
    /// This option is used to create the AssetBundles for the windows edition, 
    /// in this case is to be used in the Editor. Checks if the directory 
    /// exists and if not creates it and then creates the AssetBundles. 
    /// </summary>
    [MenuItem("Create/AssetBundle/Windows")]
    static void BuildWindowsAssetBundle()
    {
        // Path to the AssetBundles
        string assetBundleDirectory = "Assets/StreamingAssets/AssetBundles/Windows/";

        // Check if Directory exists and creates it in case it's necessary
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }

        // The Build the AssetBundles
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.None,
                                        BuildTarget.StandaloneWindows64);
    }

    /// <summary>
    /// This option is used to create the AssetBundles for the Android edition. They
    /// are located in the Streaming Assets folder for later access by the app when
    /// executing. 
    /// 
    /// The folder chosen is the StreamingAssets one because is the more accesible
    /// when executing the game in a mobile phone. 
    /// </summary>
    [MenuItem("Create/AssetBundle/Android")]
    static void BuildAndroidAssetBundle()
    {
        // Path to create the AssetBundles in
        string assetBundleDirectory = "Assets/StreamingAssets/AssetBundles/Android/";

        // Check if the directory exists and creates it if necessary
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }

        // Build the different AssetBundles
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        BuildAssetBundleOptions.None,
                                        BuildTarget.Android);
    }
}
