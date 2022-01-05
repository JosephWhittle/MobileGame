using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private GameObject _inGameUI;
    [SerializeField] private GameObject _UnlockGoButton;
    [SerializeField] private GameRunner _gameRunner;
    [SerializeField] private AdManager _adManager;

    public void LoadScene(string scene)
    {
        SceneManager.LoadSceneAsync(scene);
    }

    /// <summary>
    /// Shows the in game UI
    /// </summary>
    public void ShowUI()
    {
        _gameRunner.CanTap = false;
        _inGameUI.SetActive(true);
        _UnlockGoButton.SetActive(false);
    }

    /// <summary>
    /// Runs the ad, this is a button command
    /// </summary>
    public void RunAd()
    {
        _adManager.ShowAd();
        _gameRunner.CanTap = false;
        _UnlockGoButton.SetActive(true);

    }

    /// <summary>
    /// This is a continue button, that shows the go button thats unlocked when the ad is played
    /// </summary>
    public void Continue()
    {
        _inGameUI.SetActive(false);
        _adManager.ResetAdCounter(2);
        _gameRunner.CanTap = true;
    }



}