using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimation : MonoBehaviour {

    // 公有变量
    public float m_fSpeedDampTime = 0.5f;           // 经过此时间后达到相应速度
    public float m_fAngularSpeedDampTime = 0.2f;    // 经过此时间后达到相应角速度

    // 私有引用
    private NavMeshAgent m_navAgent;
    private Animator m_anim;
    private EnemySight m_EnemySight;

    private void Awake() {
        m_navAgent = GetComponent<NavMeshAgent>();
        m_anim = GetComponent<Animator>();
        m_EnemySight = GetComponent<EnemySight>();
        m_navAgent.updateRotation = false;
    }

    private void OnAnimatorMove() {
        // 使用navMeshAgent控制移动
        m_navAgent.velocity = m_anim.deltaPosition / Time.deltaTime;

        transform.rotation = m_anim.rootRotation;
    }

    private void Update() {
        // 闲置状态
        if (m_navAgent.desiredVelocity == Vector3.zero) {
            m_anim.SetFloat("Speed", 0, m_fSpeedDampTime, Time.deltaTime);
            m_anim.SetFloat("AngularSpeed", 0, m_fAngularSpeedDampTime, Time.deltaTime);
        }
        // 移动状态
        else {
            // 角度
            float fAngle = Vector3.SignedAngle(transform.forward, m_navAgent.desiredVelocity, Vector3.up);
            fAngle *= Mathf.Deg2Rad;
            if (Mathf.Abs(fAngle) < 0.2f) {
                transform.LookAt(transform.position + m_navAgent.desiredVelocity);
                fAngle = 0;
            }
            m_anim.SetFloat("AngularSpeed", fAngle, m_fAngularSpeedDampTime, Time.deltaTime);

            // 速度
            // 速度慢慢达到desiredVelocity
            Vector3 projection = Vector3.Project(m_navAgent.desiredVelocity, transform.forward);
            m_anim.SetFloat("Speed", projection.magnitude, m_fSpeedDampTime, Time.deltaTime);
        }

        // 控制射击
        m_anim.SetBool("PlayerInSight", m_EnemySight.m_bPlayerInSight);
    }
}