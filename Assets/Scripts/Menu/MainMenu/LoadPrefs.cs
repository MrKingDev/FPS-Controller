using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadPrefs : MonoBehaviour
{
    [Header("General Setting")]
    [SerializeField] bool canUse = false;
    [SerializeField] MainMenuController menuController;
    
    [Header("Volume Setting")]
    [SerializeField] TMP_Text volumeTextValue = null;
    [SerializeField] Slider volumeSlider = null;
    
    [Header("Brightness Setting")]
    [SerializeField] Slider brightnessSlider = null;
    [SerializeField] TMP_Text brightnessTextValue = null;

    [Header("Quality Level Setting")]
    [SerializeField] TMP_Dropdown qualityDropdown;

    [Header("Fullscreen Setting")]
    [SerializeField] Toggle fullScreenToggle;

    [Header("SensitivityX Setting")]
    [SerializeField] TMP_Text ControllerXSenTextValue = null;
    [SerializeField] Slider contrllerXSenSlider = null;

    [Header("SensitivityY Setting")]
    [SerializeField] TMP_Text ControllerYSenTextValue = null;
    [SerializeField] Slider contrllerYSenSlider = null;

    [Header("Invert X Setting")]
    [SerializeField] Toggle invertXToggle = null;

    [Header("Invert Y Setting")]
    [SerializeField] Toggle invertYToggle = null;

    private void Awake()
    {
        if(canUse)
        {
            if(PlayerPrefs.HasKey("masterVolume"))
            {
                float localVolume = PlayerPrefs.GetFloat("masterVolume");

                volumeTextValue.text = localVolume.ToString("0.0");
                volumeSlider.value = localVolume;
                AudioListener.volume = localVolume;
            }
            else
            {
                menuController.ResetButton("Audio");
            }

            if(PlayerPrefs.HasKey("masterQuality"))
            {
                int localQuality = PlayerPrefs.GetInt("masterQuality");
                qualityDropdown.value = localQuality;
                QualitySettings.SetQualityLevel(localQuality);
            }

            if(PlayerPrefs.HasKey("masterFullscreen"))
            {
                int localFullscreen = PlayerPrefs.GetInt("masterFullscreen");

                if(localFullscreen == 1)
                {
                    Screen.fullScreen = true;
                    fullScreenToggle.isOn = true;
                }
                else
                {
                    Screen.fullScreen = false;
                    fullScreenToggle.isOn = false;
                }
            }

            if(PlayerPrefs.HasKey("masterBrightness"))
            {
                float localBrightness = PlayerPrefs.GetFloat("masterBrightness");

                brightnessTextValue.text = localBrightness.ToString("0.0");
                brightnessSlider.value = localBrightness;
                // Change the brightness
            }

            if(PlayerPrefs.HasKey("masterXSen"))
            {
                float localXSensitivity = PlayerPrefs.GetFloat("masterXSen");

                ControllerXSenTextValue.text = localXSensitivity.ToString("0");
                contrllerXSenSlider.value = localXSensitivity;
                menuController.mainControllerXSen = Mathf.RoundToInt(localXSensitivity);
            }

            if(PlayerPrefs.HasKey("masterYSen"))
            {
                float localYSensitivity = PlayerPrefs.GetFloat("masterYSen");

                ControllerYSenTextValue.text = localYSensitivity.ToString("0");
                contrllerYSenSlider.value = localYSensitivity;
                menuController.mainControllerYSen = Mathf.RoundToInt(localYSensitivity);
            }

            if(PlayerPrefs.HasKey("masterInvertX"))
            {
                if(PlayerPrefs.GetInt("masterInvertX") == 1)
                {
                    invertXToggle.isOn = true;
                }
                else
                {
                    invertXToggle.isOn = false;
                }
            }

            if(PlayerPrefs.HasKey("masterInvertY"))
            {
                if(PlayerPrefs.GetInt("masterInvertY") == 1)
                {
                    invertYToggle.isOn = true;
                }
                else
                {
                    invertYToggle.isOn = false;
                }
            }
        }
    }
}
