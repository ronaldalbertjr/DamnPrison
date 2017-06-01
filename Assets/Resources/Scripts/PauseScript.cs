using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
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

    public void OnBackToMenuPressed()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuScene");
    }
}
