using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour
{
    //this class needs to show ads and track when to show ads, maybe do a check to see IF you can show one, "Try to show one"


    public int AdCounter;
    public int AdCounterReset;

    void Start()
    {
        Advertisement.Initialize("3494431", true);

        //if the ad counter hits the ad number, then show an ad, then resets
        AdCounter = 0;
        AdCounterReset = 5;
    }

    /// <summary>
    /// A public function to call when you wanna set the value outside of this class, to prevent to many ads showing
    /// </summary>
    /// <param name="value"></param>
    public void ResetAdCounter(int value)
    {
        AdCounter = value;
    }


    /// <summary>
    /// Attempts to show an advert if not too many have been shown already
    /// </summary>
    public void TryToShowAd()
    {
        if (AdCounter >= AdCounterReset)
        {
            if (Advertisement.IsReady("video"))
            {
                Advertisement.Show("video");
                AdCounter = 0;
            }
        }
    }

    /// <summary>
    /// Directly opens an ad if its ready
    /// </summary>
    public void ShowAd()
    {
        if (Advertisement.IsReady("video"))
        {
            Advertisement.Show("video");
        }
    }

}

