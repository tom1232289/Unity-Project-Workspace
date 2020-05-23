using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerStart : MonoBehaviour
{
    public void OnBtnStartClicked() {
        SceneManager.LoadScene("Game");
    }

    public void OnBtnExitClicked() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
