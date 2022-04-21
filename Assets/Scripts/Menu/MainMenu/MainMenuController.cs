using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuController : MonoBehaviour
{
    [Header("Volume Settings")]
    [SerializeField] TMP_Text volumeTextValue = null;
    [SerializeField] Slider volumeSlider = null;
    [SerializeField] float defaultVolume = 1.0f;

    [Header("Confirmation")]
    [SerializeField] GameObject comfirmationPrompt = null;

    [Header("Gameplay Settings")]
    [SerializeField] TMP_Text ControllerXSenTextValue = null;
    [SerializeField] TMP_Text ControllerYSenTextValue = null;
    [Space]
    [SerializeField] Slider contrllerXSenSlider = null;
    [SerializeField] Slider contrllerYSenSlider = null;
    [Space]
    [SerializeField] int defaultXSen = 2;
    [SerializeField] int defaultYSen = 2;
    [Space]
    public int mainControllerXSen = 2;
    public int mainControllerYSen = 2;

    [Header("Toggle Settings")]
    [SerializeField] Toggle invertXToggle = null;
    [SerializeField] Toggle invertYToggle = null;

    [Header("Graphic Settings")]
    [SerializeField] Slider brightnessSlider = null;
    [SerializeField] TMP_Text brightnessTextValue = null;
    [SerializeField] float defaultBrightness = 50;

    [Space(10)]
    [SerializeField] TMP_Dropdown qualityDropdown;
    [SerializeField] Toggle fullScreenToggle;

    int qualityLevel;
    bool _isFullScreen;
    float brightnessLevel;


    [Header("Levels To Load")]
    public string newGameLevel;
    string levelToLoad;
    [SerializeField] GameObject noSavedGameDialog = null;

    [Header("Resolution Dropdowns")]
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(newGameLevel);
    }

    public void LoadGameDialogYes()
    {
        if(PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            noSavedGameDialog.SetActive(true);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    // Sound
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    // Gameplay
    public void SetControllerXSen(float sensitivityX)
    {
        mainControllerXSen = Mathf.RoundToInt(sensitivityX);
        ControllerXSenTextValue.text = sensitivityX.ToString("0");
    }

    public void SetControllerYSen(float sensitivityY)
    {
        mainControllerYSen = Mathf.RoundToInt(sensitivityY);
        ControllerYSenTextValue.text = sensitivityY.ToString("0");
    }

    public void GameplayApply()
    {
        // X
        if(invertXToggle.isOn)
        {
            PlayerPrefs.SetInt("masterInvertX", 1);
            // Invert X
        }
        else
        {
            PlayerPrefs.SetInt("masterInvertX", 0);
            // Not Invert X
        }

        // Y
        if(invertYToggle.isOn)
        {
            PlayerPrefs.SetInt("masterInvertY", 1);
            // Invert Y
        }
        else
        {
            PlayerPrefs.SetInt("masterInvertY", 0);
            // Not Invert Y
        }

        PlayerPrefs.SetFloat("masterXSen", mainControllerXSen);
        PlayerPrefs.SetFloat("masterYSen", mainControllerYSen);
        StartCoroutine(ConfirmationBox());
    }

    public void SetBrightness(float brightness)
    {
        brightnessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");
    }

    public void SetFullScreen(bool isFullScreen)
    {
        _isFullScreen = isFullScreen;
    }

    public void SetQuality(int qualityIndex)
    {
        qualityLevel = qualityIndex;
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", brightnessLevel);
        // Change your brightness with post processing or whatever it is

        PlayerPrefs.SetInt("masterQuality", qualityLevel);
        QualitySettings.SetQualityLevel(qualityLevel);

        PlayerPrefs.SetInt("masterFullScreen", (_isFullScreen ? 1 : 0));
        Screen.fullScreen = _isFullScreen;

        StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(string MenuType)
    {
        if(MenuType == "Graphics")
        {
            // Reset brightness value
            brightnessSlider.value = defaultBrightness;
            brightnessTextValue.text = defaultBrightness.ToString("0.0");

            qualityDropdown.value = 1;
            QualitySettings.SetQualityLevel(1);

            fullScreenToggle.isOn = false;
            Screen.fullScreen = false;

            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;
            GraphicsApply();
        }

        if(MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }

        if(MenuType == "Gameplay")
        {
            ControllerXSenTextValue.text = defaultXSen.ToString("0");
            ControllerYSenTextValue.text = defaultYSen.ToString("0");
            contrllerXSenSlider.value = defaultXSen;
            contrllerYSenSlider.value = defaultYSen;
            mainControllerXSen = defaultXSen;
            mainControllerYSen = defaultYSen;
            invertYToggle.isOn = false;
            invertXToggle.isOn = false;
            GameplayApply();
        }
    }

    public IEnumerator ConfirmationBox()
    {
        comfirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(1);
        comfirmationPrompt.SetActive(false);
    }
}   
