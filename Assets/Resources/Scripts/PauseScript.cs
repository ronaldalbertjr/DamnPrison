using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    #region Variables
        [SerializeField]
        GameObject menuPanel;
        [SerializeField]
        GameObject exitPanel;
    #endregion

    void Start()
    {
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
}
