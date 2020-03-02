using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    // 公有变量
    public float m_fSpeed = 10f;

    // 私有变量
    public Vector3 m_posGoldPanel;

    private void Awake() {
        m_posGoldPanel = GameObject.Find("textGold").transform.position;
    }

    private void Update() {
        transform.position = Vector3.MoveTowards(transform.position, m_posGoldPanel, m_fSpeed * Time.deltaTime);
        if (transform.position == m_posGoldPanel) {
            AudioManager.Instance.PlayAudio(AudioManager.Instance.m_acGold);
            Destroy(gameObject);
        }
    }
}
