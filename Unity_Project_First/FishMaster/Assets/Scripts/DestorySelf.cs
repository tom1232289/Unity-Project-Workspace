﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestorySelf : MonoBehaviour {
    // 公有变量
    public float m_fDuration = 1f;

    private void Start() {
        Destroy(gameObject, m_fDuration);
    }
}
