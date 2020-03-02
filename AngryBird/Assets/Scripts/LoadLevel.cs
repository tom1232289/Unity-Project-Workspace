using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class LoadLevel : MonoBehaviour
{
    private void Awake() {
        string sNowLevel = "Game" + PlayerPrefs.GetString("nowLevel");
        Object go = Resources.Load(sNowLevel);
        if (go == null) {
            Debug.Log("已是最后一关");
            // 重置记录的当前关
            int iLevel = Int32.Parse(sNowLevel[sNowLevel.Length - 1].ToString());
            --iLevel;
            sNowLevel = sNowLevel.Remove(sNowLevel.Length - 1);
            sNowLevel = sNowLevel.Insert(sNowLevel.Length, iLevel.ToString());
            PlayerPrefs.SetString("nowLevel", sNowLevel);
            SceneManager.LoadScene("Level");
            return;
        }
        Instantiate(go);
    }
}
