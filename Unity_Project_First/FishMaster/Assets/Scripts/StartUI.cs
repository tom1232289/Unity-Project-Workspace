using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUI : MonoBehaviour
{
    public void OnBtnNewGameDown() {
        PlayerPrefs.DeleteKey("gold");
        PlayerPrefs.DeleteKey("level");
        PlayerPrefs.DeleteKey("exp");
        PlayerPrefs.DeleteKey("bigTimer");
        PlayerPrefs.DeleteKey("smallTimer");
        SceneManager.LoadScene("Game");
    }

    public void OnBtnContinueGameDown() {
        SceneManager.LoadScene("Game");
    }

    public void OnBtnQuitDown() {
        Application.Quit();
    }
}
