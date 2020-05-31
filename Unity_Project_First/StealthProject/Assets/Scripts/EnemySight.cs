using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySight : MonoBehaviour {

    // 公有变量
    public bool m_bPlayerInSight;   // 是否观测到玩家
    public float m_fFieldOfView = 110;  // 敌人的视野范围
    public Vector3 m_posHear = Vector3.zero;

    // 私有引用
    private Animator m_anim;
    private NavMeshAgent m_navAgent;
    private SphereCollider m_sphereCollider;

    private void Awake() {
        m_anim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        m_navAgent = GetComponent<NavMeshAgent>();
        m_sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "Player") {
            /// 视觉检测
            Vector3 forward = transform.forward;    // 敌人的正前方
            Vector3 playerDir = other.transform.position - transform.position;  // 敌人指向玩家的向量
            float fAngle = Vector3.Angle(forward, playerDir);   // 敌人和玩家的夹角
            // 在敌人视野正前方 且 视野前方没有障碍物
            RaycastHit hitInfo;
            bool bHit = Physics.Raycast(transform.position + Vector3.up, other.transform.position - transform.position, out hitInfo);
            if (fAngle < 0.5 * m_fFieldOfView && (!bHit || hitInfo.collider.tag == "Player")) {
                m_bPlayerInSight = true;
                GameManager.Instance.Alerm(other.transform.position);
            }
            else {
                m_bPlayerInSight = false;
            }

            /// 听觉检测
            // 玩家在走或者跑
            if (m_anim.GetCurrentAnimatorStateInfo(0).IsName("Locomotion")) {
                // 记录下敌人检测到的警报位置(超出距离不算听到)
                NavMeshPath navPath = new NavMeshPath();
                // 通过navigation计算到玩家的最短距离，距离 > Trigger的半径则不算听到
                if (m_navAgent.CalculatePath(other.transform.position, navPath)) {
                    // 计算最短路径 的 距离
                    List<Vector3> wayPoints = new List<Vector3>();
                    wayPoints.Add(transform.position);
                    foreach (var corner in navPath.corners) {
                        wayPoints.Add(corner);
                    }
                    wayPoints.Add(other.transform.position);
                    float fDistance = 0;
                    for (int i = 0; i < wayPoints.Count - 1; ++i) {
                        fDistance += (wayPoints[i] - wayPoints[i + 1]).magnitude;
                    }
                    // 距离 < Trigger的半径 时才算听到
                    if (fDistance < m_sphereCollider.radius) {
                        m_posHear = other.transform.position;
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Player") {
            m_bPlayerInSight = false;
        }
    }
}