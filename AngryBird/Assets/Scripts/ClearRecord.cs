using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearRecord : MonoBehaviour
{
    public void OnBtnClearRecordClicked() {
        // 清除记录
        PlayerPrefs.DeleteAll();
        // 重新加载游戏
        SceneManager.LoadScene("Loading");
    }
}
