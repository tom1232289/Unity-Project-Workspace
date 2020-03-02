﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    private void Start() {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Game");
    }
}
