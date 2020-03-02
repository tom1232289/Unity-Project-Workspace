using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    private void Start() {
        int iDay = PlayerPrefs.GetInt("Level", 1);
        GameObject.Find("textLoad").GetComponent<Text>().text = "Day " + iDay;
        Invoke("LoadGame", 1f);
    }

    private void LoadGame() {
        SceneManager.LoadScene("Game");
    }
}
