using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Born : MonoBehaviour
{
    // 公有变量
    public bool m_bIsPlayer;
    public float m_fDuringTime = 1.5f;

    // 公有引用
    public GameObject m_Player;
    public GameObject[] m_Enemys;

    private void Start() {
        Invoke("BornTank", m_fDuringTime);
    }

    private void BornTank() {
        if (m_bIsPlayer) {
            Instantiate(m_Player, transform.position, Quaternion.identity);
        }
        else {
            int iRandom = Random.Range(0, m_Enemys.Length);
            Instantiate(m_Enemys[iRandom], transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
