using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
    private static AdManager instance;

    private string gameID = "1234567";

    // Ad type 
    string placementVideo = "video";
    string placementIdRewardedVideo = "rewardedVideo";
    string placementBannerID = "bannerPlacement";

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            Object.DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        Advertisement.AddListener(GetInstance());
        Advertisement.Initialize(gameID, true);
        StartCoroutine(ShowBannerWhenInitialized());
    }

    public static AdManager GetInstance()
    {
        return instance;
    }

    IEnumerator ShowBannerWhenInitialized()
    {
        while (!Advertisement.isInitialized)
        {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
        Advertisement.Banner.Show(GetInstance().placementBannerID);
    }

    public void ShowRewardedVideo()
    {
        if (Advertisement.IsReady(placementIdRewardedVideo))
        {
            Advertisement.Show(placementIdRewardedVideo);
        }
        else
        {
            Debug.Log("Rewarded video is not ready at the moment! Try again later!");
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult res)
    {
        if(res == ShowResult.Finished || res == ShowResult.Skipped)
        {
            GameManager.GetInstance().AdEnded();
        }
        else if(res == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        if(placementId == GetInstance().placementIdRewardedVideo)
        {

        }
    }

    public void OnUnityAdsDidError(string message)
    {
        Debug.LogError("Ad errored: " + message);
    }

    public void OnUnityAdsDidStart(string placementId) { }
}
