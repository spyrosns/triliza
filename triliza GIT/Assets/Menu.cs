using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Text thscore;
    public Text[] txtlevs;

    void Start()
    {
        thscore.text = "" + PlayerPrefs.GetInt("ScoreSaved");   //Deixnei to apothikeumeno level
        for (int i = 0; i < txtlevs.Length; i++)
            txtlevs[i].color = Color.black;
        txtlevs[PlayerPrefs.GetInt("LevelSaved")].color = Color.yellow;  //kitrino xrwma sto apothikeumeno level
    }

    public void SelLevel(int levelid)   //Epilogh level
    {
        for (int i = 0; i < txtlevs.Length; i++)
            txtlevs[i].color = Color.black;
        txtlevs[levelid].color = Color.yellow;  //kitrino xrwma sto epilegmeno level

        PlayerPrefs.SetInt("LevelSaved", levelid);  //Apothikefsi epilegmenou level
    }

    public void GoToPlay()
    {
        SceneManager.LoadScene(1); // Load Play Scene
    }
}
