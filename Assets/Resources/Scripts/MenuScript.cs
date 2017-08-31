using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    #region Variables
        [SerializeField]
        Text damnPrisonText;
        [SerializeField]
        Text jogarText;
        [SerializeField]
        AudioSource waterDrop;
        [SerializeField]
        AudioSource menuOST;
        [HideInInspector]
        public bool startPressed;
    #endregion

    private void Start()
    {
        Time.timeScale = 0;
        startPressed = false;
    }

    private void Update()
    {
        if(startPressed)
        {
            menuOST.Stop();
            waterDrop.Play();
            damnPrisonText.color = Color.Lerp(damnPrisonText.color, new Color(damnPrisonText.color.r, damnPrisonText.color.g, damnPrisonText.color.b, 0), 0.05f);
            jogarText.color = Color.Lerp(jogarText.color, new Color(jogarText.color.r, jogarText.color.g, jogarText.color.b, 0), 0.05f);

            if(damnPrisonText.color.a < 0.05f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void OnStartPressed()
    {
        Time.timeScale = 1;
        startPressed = true;
    }
}
