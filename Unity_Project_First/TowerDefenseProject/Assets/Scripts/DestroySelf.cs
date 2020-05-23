using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    // 公有变量
    public float m_fDurationTime = 1f;

    private void Start() {
        Destroy(gameObject, m_fDurationTime);
    }
}
