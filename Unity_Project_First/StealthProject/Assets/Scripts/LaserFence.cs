using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFence : MonoBehaviour
{
    // 公有变量
    public bool m_bIsFlicker;           // 是否是闪烁激光栅栏
    public float m_fOnTime = 2f;        // 激光开着的时间
    public float m_fOffTime = 2f;       // 激光关闭的时间
    public float m_fCurTime = 0;        // 记录到达闪烁的时间

    // 私有引用
    private Renderer m_renderer;
    private AudioSource m_as;
    private BoxCollider m_collider;
    private Light m_light;

    // 私有变量
    private bool m_bIsOn;   // 激光是否是亮着的

    private void Awake() {
        m_renderer = GetComponent<Renderer>();
        m_as = GetComponent<AudioSource>();
        m_collider = GetComponent<BoxCollider>();
        m_light = GetComponent<Light>();
    }

    private void OnTriggerStay(Collider other) {
        // 主角进入激光射线范围
        if (other.tag == "Player") {
            // 警报
            GameManager.Instance.Alerm(other.transform.position);
        }
    }

    private void Update() {
        // 当前栅栏 是 闪烁激光栅栏
        if (m_bIsFlicker) {
            // 刷新闪烁冷却时间
            m_fCurTime += Time.deltaTime;
            // 当前激光是 亮 的
            if (m_bIsOn) {
                // CD结束
                if (m_fCurTime >= m_fOnTime) {
                    m_fCurTime = 0;
                    SetObjectActive(false);
                    m_bIsOn = false;
                }
            }
            // 当前激光是 灭 的
            else {
                // CD结束
                if (m_fCurTime >= m_fOffTime) {
                    m_fCurTime = 0;
                    SetObjectActive(true);
                    m_bIsOn = true;
                }
            }
        }
    }

    // 设置组件是否激活 
    private void SetObjectActive(bool bIsOn) {
        m_renderer.enabled = bIsOn;
        m_as.enabled = bIsOn;
        m_collider.enabled = bIsOn;
        m_light.enabled = bIsOn;
    }
}
