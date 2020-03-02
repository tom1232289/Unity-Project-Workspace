using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager_Loading : MonoBehaviour {

    private void Start() {
        SceneManager.LoadSceneAsync("Level");
    }
}