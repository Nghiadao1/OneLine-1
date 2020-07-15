using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

/// <summary>
/// Class that controls the different ads shown to the player in-game. Is the one
/// that controls when the ad has ended or when it has been skipped. 
/// 
/// Communicates with the GameManager the different status of the different ads 
/// and how they ended. 
/// 
/// Singleton.
/// </summary>
public class AdManager : MonoBehaviour, IUnityAdsListener
{
    // Instance of the AdManager, singleton
    private static AdManager instance;

    // Different games ID for different platforms
#if UNITY_ANDROID
    private string gameID = "3714873";
#elif UNITY_IOS
    private string gameID = "3714872";
#endif

    // Ad type 
    string placementVideo = "video";
    string placementIdRewardedVideo = "rewardedVideo";
    string placementBannerID = "bannerID";

    /// <summary>
    /// The Awake function only creates the instance of the AdManager when it 
    /// does not exist. 
    /// </summary>
    private void Awake()
    {
        // Create Instance
        if (instance == null)
        {
            instance = this;
            Object.DontDestroyOnLoad(this);
        }
        // If instance already exists, destroy current GameObject
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// The Start is used to initialize all information needed for  showing ads 
    /// to the Player. 
    /// </summary>
    private void Start()
    {
        // Adds this GameObject as a Listener for the Ads
        Advertisement.AddListener(GetInstance());
        // Then initialize ads
        Advertisement.Initialize(gameID, true);
        // Show Different banners
        StartCoroutine(ShowBannerWhenInitialized());
    }

    /// <summary>
    /// Function that returns the instance of AdManager.
    /// </summary>
    /// <returns>AdManager instance of singleton</returns>
    public static AdManager GetInstance()
    {
        return instance;
    }

    /// <summary>
    /// Function that initializes all Banners that will be shown in the game. 
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowBannerWhenInitialized()
    {
        // Wait for Banners to be initialized
        while (!Advertisement.isInitialized)
        {
            yield return new WaitForSeconds(0.5f);
        }
        // Set the position of the Banner and show it 
        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
        Advertisement.Banner.Show(GetInstance().placementBannerID);
    }

    /// <summary>
    /// Shows a Rewarded video when called. 
    /// </summary>
    public void ShowRewardedVideo()
    {
        // Check if Rewarded video is ready to be shown
        if (Advertisement.IsReady(placementIdRewardedVideo))
        {
            Advertisement.Show(placementIdRewardedVideo);
        }
    }

    /// <summary>
    /// Function that manages when an ad has ended. 
    /// </summary>
    /// <param name="placementId">Ad that ended</param>
    /// <param name="res">Result of the ad</param>
    public void OnUnityAdsDidFinish(string placementId, ShowResult res)
    {
        // If the result is finished or skipped, tell GameManager that Ad ended
        if (res == ShowResult.Finished || res == ShowResult.Skipped)
        {
            GameManager.GetInstance().AdEnded();
        }
        // Is not, log a warning
        else if (res == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error");
        }
    }

    /// <summary>
    /// Not implemented, function from the Interface.
    /// </summary>
    /// <param name="placementId"></param>
    public void OnUnityAdsReady(string placementId)
    {
        
    }

    /// <summary>
    /// Manages when an ad ended with an error. Logs an Error to console.
    /// </summary>
    /// <param name="message">Error message</param>
    public void OnUnityAdsDidError(string message)
    {
        Debug.LogError("Ad errored: " + message);
    }

    /// <summary>
    /// Not implemented, function from the Interface.
    /// </summary>
    /// <param name="placementId"></param>
    public void OnUnityAdsDidStart(string placementId) { }
}
