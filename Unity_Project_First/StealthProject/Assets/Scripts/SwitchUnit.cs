using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchUnit : MonoBehaviour
{
    // 公有引用
    public GameObject m_goLaser;    // 要关闭的激光
    public Material m_materialLock; // 锁的材质
    public Renderer m_rendererLock; // 锁的渲染器

    // 私有引用
    private AudioSource m_as;

    // 私有变量
    private bool m_bIsClosed;   // 激光是否已经关闭了

    private void Awake() {
        m_as = GetComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "Player" && !m_bIsClosed) {
            if (Input.GetKeyDown(KeyCode.E)) {
                // 关闭激光
                m_goLaser.SetActive(false);
                m_bIsClosed = true;
                // 播放音效
                m_as.Play();
                // 更换解锁图案
                m_rendererLock.material = m_materialLock;
            }
        }
    }
}
