using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour {
    public int m_iPlayerNumber = 1;                 // 坦克编号
    public float m_fSpeed = 12f;                    // 坦克的移速
    public float m_fTurnSpeed = 180f;               // 坦克的转速
    public AudioSource m_MovementAudio;
    public AudioClip m_EngineIdling;
    public AudioClip m_EngineDriving;               
    public float m_fPitchRange = 0.2f;              // 随机音高时加减值

    private string m_sMovementAxisName;             // 移动轴的名字
    private string m_TurnAxisName;                  // 转向轴的名字
    private Rigidbody m_rigidBody;                  // 刚体组件
    private float m_fMovementInputValue;            // 输入的移动的大小
    private float m_fTurnInputValue;                // 输入的转向的大小
    private float m_OriginalPitch;                  // 初始的音高
    private ParticleSystem[] m_particleSystems;     // 粒子系统的数组  

    private void Awake() {
        m_rigidBody = GetComponent<Rigidbody>();
    }

    private void OnEnable() {
        m_rigidBody.isKinematic = false;

        m_fMovementInputValue = 0f;
        m_fTurnInputValue = 0f;

        m_particleSystems = GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < m_particleSystems.Length; ++i) {
            m_particleSystems[i].Play();
        }
    }

    private void OnDisable() {
        m_rigidBody.isKinematic = true;

        for (int i = 0; i < m_particleSystems.Length; ++i) {
            m_particleSystems[i].Stop();
        }
    }

    // Start is called before the first frame update
    void Start() {
        m_sMovementAxisName = "Vertical" + m_iPlayerNumber;
        m_TurnAxisName = "Horizontal" + m_iPlayerNumber;

        m_OriginalPitch = m_MovementAudio.pitch;
    }

    // Update is called once per frame
    void Update() {
        m_fMovementInputValue = Input.GetAxis(m_sMovementAxisName);
        m_fTurnInputValue = Input.GetAxis(m_TurnAxisName);

        EngineAudio();
    }

    private void EngineAudio() {
        if (Mathf.Abs(m_fMovementInputValue) < 0.1f && Mathf.Abs(m_fTurnInputValue) < 0.1f) {
            if (m_MovementAudio.clip == m_EngineDriving) {
                m_MovementAudio.clip = m_EngineIdling;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_fPitchRange, m_OriginalPitch + m_fPitchRange);
                m_MovementAudio.Play();
            }
        }
        else {
            if (m_MovementAudio.clip == m_EngineIdling) {
                m_MovementAudio.clip = m_EngineDriving;
                m_MovementAudio.pitch = Random.Range(m_OriginalPitch - m_fPitchRange, m_OriginalPitch + m_fPitchRange);
                m_MovementAudio.Play();
            }
        }
    }

    private void FixedUpdate() {
        Move();
        Turn();
    }

    private void Move() {
        Vector3 movement = transform.forward * m_fMovementInputValue * m_fSpeed * Time.deltaTime;
        m_rigidBody.MovePosition(m_rigidBody.position + movement);
    }

    private void Turn() {
        float turn = m_fTurnInputValue * m_fTurnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        m_rigidBody.MoveRotation(m_rigidBody.rotation *turnRotation);
    }
}
