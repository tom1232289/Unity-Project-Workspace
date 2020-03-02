using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ef_AutoRotate : MonoBehaviour
{
    // 公有变量
    public float m_fSpeed = 10f;

    private void Update() {
        transform.Rotate(Vector3.forward, m_fSpeed * Time.deltaTime);
    }
}
