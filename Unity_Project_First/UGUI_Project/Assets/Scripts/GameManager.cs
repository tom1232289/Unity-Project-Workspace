using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void OnClickedBtnStartGame(string sSceneName) {
        SceneManager.LoadScene(sSceneName);
    }
}
