using System.Collections;
using System.Collections.Generic;
using SonicBloom.Koreo;
using UnityEngine;

public class BallTest : MonoBehaviour
{
    // 公有变量
    public string m_sEventID;
    public float m_fJumpSpeed;

    // 私有引用 
    private Rigidbody m_rd;

    private void Awake() {
        m_rd = GetComponent<Rigidbody>();
    }

    private void Start() {
        Koreographer.Instance.RegisterForEvents(m_sEventID, BallJump);
    }

    private void BallJump(KoreographyEvent koreographyEvent) {
        Vector3 velocity = m_rd.velocity;
        velocity.y = m_fJumpSpeed;
        m_rd.velocity = velocity;
    }
}
