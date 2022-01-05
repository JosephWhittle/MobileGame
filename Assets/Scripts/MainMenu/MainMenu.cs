using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _optionsCanvas;
    [SerializeField] private Slider _myVolumeSlider;
    [SerializeField] private Slider _myVibrationSlider;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private AudioSource _pop;

    void Start()
    {
        _myVolumeSlider.onValueChanged.AddListener(VolumeSlider);
        _myVibrationSlider.onValueChanged.AddListener(VibrationSlider);
        _myVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume");
        _myVibrationSlider.value = PlayerPrefs.GetFloat("Vibrate");

        _coinText.text = PlayerPrefs.GetInt("MoonCoins").ToString();
    }

    /// <summary>
    /// updates the vibration slider
    /// </summary>
    /// <param name="value"></param>
    public void VibrationSlider(float value)
    {
        PlayerPrefs.SetFloat("Vibrate",value);
    }

 
    /// <summary>
    /// updates th volume slider
    /// </summary>
    /// <param name="value"></param>
    public void VolumeSlider(float value)
    {
        PlayerPrefs.SetFloat("MasterVolume", value);
        _audioMixer.SetFloat("Volume", value);
    }

    /// <summary>
    /// A multi use function to change scene with input
    /// </summary>
    /// <param name="scene"></param>
    public void ChangeScene(string scene)
    {
        _pop.Play();
        SceneManager.LoadSceneAsync(scene);
    }

    /// <summary>
    /// Shows the options menu
    /// </summary>
    public void ShowOptions()
    {
        _pop.Play();
        _optionsCanvas.SetActive(true);
    }

    /// <summary>
    /// Hides the options menu
    /// </summary>
    public void HideOptions()
    {
        _pop.Play();
        _optionsCanvas.SetActive(false);
    }

    /// <summary>
    /// Resets the moon coins
    /// </summary>
    public void ResetMoonCoins()
    {
        _pop.Play();
        PlayerPrefs.SetInt("MoonCoins", 0);
    }



}
