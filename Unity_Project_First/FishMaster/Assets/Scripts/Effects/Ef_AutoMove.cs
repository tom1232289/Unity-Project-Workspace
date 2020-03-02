using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ef_AutoMove : MonoBehaviour
{
    // 公有变量
    public float m_fSpeed = 1f;
    public Vector3 m_dir = Vector3.right;

    private void Update() {
        transform.Translate(m_fSpeed * m_dir * Time.deltaTime);
    }
}
