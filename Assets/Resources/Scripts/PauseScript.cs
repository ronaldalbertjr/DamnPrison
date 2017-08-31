using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    #region Variables
        [SerializeField]
        GameObject menuPanel;
        [SerializeField]
        GameObject exitPanel;
        [SerializeField]
        GameObject optionsPanel;
        [SerializeField]
        Toggle toggle;
        [SerializeField]
        Dropdown resolutionDropdown;
        [SerializeField]
        Dropdown qualityDropdown;
        [SerializeField]
        Toggle vSyncToggle;
        [SerializeField]
        Toggle antiAlisingToggle;
        [SerializeField]
        Slider volumeSlider;
    #endregion

    void Start()
    {
        GetComponent<Canvas>().enabled = false;
    }

	void Update ()
    {
        if (Input.GetKey(KeyCode.Escape) && Time.timeScale != 0)
        {
            GetComponent<Canvas>().enabled = true;
            Time.timeScale = 0;

            volumeSlider.value = AudioListener.volume;
            qualityDropdown.value = QualitySettings.GetQualityLevel();
            vSyncToggle.isOn = Convert.ToBoolean(QualitySettings.vSyncCount);
            antiAlisingToggle.isOn = Convert.ToBoolean(QualitySettings.antiAliasing);
            toggle.isOn = !Screen.fullScreen;
            if (Screen.width == 1920 && Screen.height == 1080)
            {
                resolutionDropdown.value = 0;
            }
            else if (Screen.width == 1680 && Screen.height == 1050)
            {
                resolutionDropdown.value = 1;
            }
            else if (Screen.width == 1600 && Screen.height == 900)
            {
                resolutionDropdown.value = 2;
            }
            else if (Screen.width == 1440 && Screen.height == 900)
            {
                resolutionDropdown.value = 3;
            }
            else if (Screen.width == 1366 && Screen.height == 768)
            {
                resolutionDropdown.value = 4;
            }
            else if (Screen.width == 1280 && Screen.height == 1024)
            {
                resolutionDropdown.value = 5;
            }
            else if (Screen.width == 1024 && Screen.height == 768)
            {
                resolutionDropdown.value = 6;
            }
        }
	}
    
    public void OnResumePressed()
    {
        Time.timeScale = 1;
        GetComponent<Canvas>().enabled = false;
    }

    public void OnBackToTitlePressed()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void OnOptionsPressed()
    {
        menuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void OnBackToPausedPressed()
    {
        menuPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void OnExitPressed()
    {
        menuPanel.SetActive(false);
        exitPanel.SetActive(true);
    }
    
    public void OnYesPressed()
    {
        Application.Quit();
    }

    public void OnNoPressed()
    {
        menuPanel.SetActive(true);
        exitPanel.SetActive(false);
    }

    public void OnToggleValueChanged(Toggle toggle)
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    public void OnVSyncToggleValueChanged(Toggle toggle)
    {
        QualitySettings.vSyncCount = Convert.ToInt32(toggle.isOn);
    }

    public void OnAntiAlisingToggleChanged(Toggle toggle)
    {
        QualitySettings.antiAliasing = Convert.ToInt32(toggle.isOn);
    }
    
    public void OnDropdownValueChanged(Dropdown dropdown)
    {
        switch(dropdown.value)
        {
            case 0:
                Screen.SetResolution(1920, 1080, Screen.fullScreen);
                break;
            case 1:
                Screen.SetResolution(1680, 1050, Screen.fullScreen);
                break;
            case 2:
                Screen.SetResolution(1600, 900, Screen.fullScreen);
                break;
            case 3:
                Screen.SetResolution(1440, 900, Screen.fullScreen);
                break;
            case 4:
                Screen.SetResolution(1366, 768, Screen.fullScreen);
                break;
            case 5:
                Screen.SetResolution(1280, 1024, Screen.fullScreen);
                break;
            case 6:
                Screen.SetResolution(1024, 768, Screen.fullScreen);
                break;
        }
    }

    public void OnQualityDropdownValueChanged(Dropdown dropdown)
    {
        QualitySettings.SetQualityLevel(dropdown.value);
    }

    public void OnVolumeSliderChanged(Slider slider)
    {
        AudioListener.volume = slider.value;
    }
    
}
