using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour {

    private void Start() {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Load");
    }
}