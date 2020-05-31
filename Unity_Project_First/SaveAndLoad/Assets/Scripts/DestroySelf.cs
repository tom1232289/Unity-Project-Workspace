using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{  
    // 公有变量
    public float m_fTime = 5;   // 过了_秒销毁自身

    private void Start() {
        Destroy(gameObject, m_fTime);
    }
}
