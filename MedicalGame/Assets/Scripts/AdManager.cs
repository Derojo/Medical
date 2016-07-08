using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

[Prefab("AdManager", false, "")]
public class AdManager : Singleton<AdManager> 
{
	public bool enableAds;
	public bool playAddAfterQuestionWrong = false;
	public bool playAddAfterQuestionRight = false;
	
    public void ShowAd(string zone = "")
    {
		if(enableAds) {
			#if UNITY_EDITOR
				//StartCoroutine(WaitForAd ());
			#endif

			if (string.Equals (zone, ""))
				zone = null;

			ShowOptions options = new ShowOptions {resultCallback = AdCallbackhandler };

			if (Advertisement.IsReady (zone)) {
				Advertisement.Show (zone, options);
			}
		}
    }

    private void AdCallbackhandler (ShowResult result)
    {
		switch (result)
		{
		  case ShowResult.Finished:
			Debug.Log("The ad was successfully shown.");
			//
			// YOUR CODE TO REWARD THE GAMER
			// Give coins etc.
			break;
		  case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			break;
		  case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");
			break;
		}
    }

	
    IEnumerator WaitForAd()
    {
        float currentTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        yield return null;

        while (Advertisement.isShowing)
            yield return null;

        Time.timeScale = currentTimeScale;
    }

}