using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System.IO;

/// <summary>
/// Build specific script. It is created because it's important to ensure
/// that the game_data file exists in order for the game to work. 
/// 
/// Checks that the file exists and if not, creates the file and then 
/// builds the game. 
/// </summary>
public class BuildPlayer : MonoBehaviour
{
    /// <summary>
    /// This option appears in Editor's menu, dropping down an 
    /// option to build the game. This option executes this 
    /// function that ensures that the game_data files exists 
    /// before building the app. 
    /// </summary>
    [MenuItem("Build/Build Android")]
    public static void BuildAndroid()
    {
        // Build player options, for building the game
        BuildPlayerOptions bpo = new BuildPlayerOptions();

        // Set everything to work fine when building 
        bpo.scenes = new[] {"Assets/Scenes/MainMenu.unity", "Assets/Scenes/LevelSelection.unity", "Assets/Scenes/Game.unity" };
        bpo.locationPathName = "../AndroidBuild/OneLine.apk";
        bpo.target = BuildTarget.Android;
        bpo.options = BuildOptions.None;

        // Check game_data file and create it if don't exists
        if (!File.Exists("Assets/StreamingAssets/game_data.json"))
        {
            CreateGameInfo.CreateInfo();
        }

        // Build game
        BuildReport rp = BuildPipeline.BuildPlayer(bpo);
        BuildSummary bs = rp.summary;

        // Build result management
        if(bs.result == BuildResult.Succeeded)
        {
            Debug.Log("Build suceeded: " + bs.totalSize + " bytes");
        }
        
        if(bs.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }
}
