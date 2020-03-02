using UnityEngine;

public class LoadLevel : MonoBehaviour {

    private void Awake() {
        // 初始化场景
        Instantiate(Resources.Load(PlayerPrefs.GetString("nowLevel", "level_1")));
    }
}