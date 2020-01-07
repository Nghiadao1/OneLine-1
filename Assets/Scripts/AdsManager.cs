using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Monetization;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
    public static AdsManager instance;

    private string gameId = "123456";
    private bool testMode = true;
    private int rewardCOUT = 0;
    public int reward = 25;

    // Ad type
    string placementVideo = "video";
    string placementIdRewardedVideo = "rewardedVideo";

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

    // Start is called before the first frame update
    void Start()
    {
        Monetization.Initialize(gameId, true);
    }

    public void ShowAd(string zone = "")
    {
#if UNITY_EDITOR
        StartCoroutine(WaitForAd(true));
#endif

        if (string.Equals(zone, ""))
            zone = null;

        ShowOptions options = new ShowOptions();
        options.resultCallback = AdCallbackhandler;

        if (Advertisement.IsReady(zone))
            Advertisement.Show(zone, options);

    }

    void AdCallbackhandler(UnityEngine.Advertisements.ShowResult result)
    {
        switch (result)
        {
            case UnityEngine.Advertisements.ShowResult.Finished:
                GameManager.instance.AddCoins(reward);
                break;
            case UnityEngine.Advertisements.ShowResult.Skipped:
                break;
            case UnityEngine.Advertisements.ShowResult.Failed:
                break;
        }
    }

    public void ShowNormalAd(int coins)
    {
        reward = coins;
        StartCoroutine(WaitForAd(true));
    }
    
    IEnumerator WaitForAd(bool rewarded = false)
    {
        string placementId = rewarded ? placementIdRewardedVideo : placementVideo;

        while (!Monetization.IsReady(placementId))
        {
            yield return null;
        }

        ShowAdPlacementContent ad = null;
        ad = Monetization.GetPlacementContent(placementId) as ShowAdPlacementContent;

        if(ad != null)
        {
            if (rewarded)
            {
                ad.Show(AdFinished);
            }
            else
            {
                ad.Show();
            }
        }
    }

    void AdFinished(UnityEngine.Monetization.ShowResult result)
    {
        if(result == UnityEngine.Monetization.ShowResult.Finished)
        {
            GameManager.instance.AddCoins(reward);
        }
    }
}
