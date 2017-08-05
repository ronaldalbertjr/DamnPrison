using System.Collections;
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
        Toggle toggle;
        [SerializeField]
        Dropdown dropdown;
    #endregion

    void Start()
    {
        if(Screen.fullScreen)
        {
            toggle.isOn = false;
        }
        else
        {
            toggle.isOn = true;
        }
        if(Screen.width == 1920 && Screen.height == 1080)
        {
            dropdown.value = 0;
        }
        else if(Screen.width == 1680 && Screen.height == 1050)
        {
            dropdown.value = 1;
        }
        else if (Screen.width == 1600 && Screen.height == 900)
        {
            dropdown.value = 2;
        }
        else if (Screen.width == 1440 && Screen.height == 900)
        {
            dropdown.value = 3;
        }
        else if (Screen.width == 1366 && Screen.height == 768)
        {
            dropdown.value = 4;
        }
        else if (Screen.width == 1280 && Screen.height == 1024)
        {
            dropdown.value = 5;
        }
        else if (Screen.width == 1024 && Screen.height == 768)
        {
            dropdown.value = 6;
        }
        GetComponent<Canvas>().enabled = false;
    }

	void Update ()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            GetComponent<Canvas>().enabled = true;
            Time.timeScale = 0;
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
        SceneManager.LoadScene("MenuScene");
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
    
}
