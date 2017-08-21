using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneScript : MonoBehaviour
{
    #region Variables
        [SerializeField]
        MovieTexture movie;
    #endregion

    void Start ()
    {
        GetComponent<RawImage>().texture = movie as MovieTexture;
        GetComponent<AudioSource>().clip = movie.audioClip;

        movie.Play();
        Invoke("RunGameScene", 36f);		
	}

    private void Update()
    {
        if(Input.anyKey)
        {
            RunGameScene();
        }
    }

    void RunGameScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
	
}
