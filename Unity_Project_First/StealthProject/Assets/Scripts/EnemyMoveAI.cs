using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveAI : MonoBehaviour {

    // 公有引用
    public List<Transform> m_WayPoints;   // 巡逻的路径点

    // 公有变量
    public float m_fIdleTime = 1;   // 在路径点休息的时间
    public float m_fChaseSpeed = 3; // 追逐的速度
    public float m_fSearchSpeed = 2; // 搜索的速度
    public float m_fChaseTime = 2;

    // 私有引用
    private NavMeshAgent m_navAgent;
    private EnemySight m_EnemySight;
    private PlayerHealth m_PlayerHealth;

    // 私有变量
    private float m_fCurIdleTime = 0;
    private int m_iIndex = 0;   // 路径点的下标
    private float m_fCurChaseTime;

    private void Awake() {
        m_navAgent = GetComponent<NavMeshAgent>();
        m_navAgent.SetDestination(m_WayPoints[0].position);
        m_EnemySight = GetComponent<EnemySight>();
        m_PlayerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }

    private void Update() {
        if (m_PlayerHealth.m_fHealth <= 0) {
            m_navAgent.isStopped = false;
            Patrol();
            return;
        }

        if (m_EnemySight.m_bPlayerInSight) {
            Shoot();
        }
        else if (GameManager.Instance.m_MonitoredPlayerPos != Vector3.zero || m_EnemySight.m_posHear != Vector3.zero) {
            m_navAgent.isStopped = false;
            Chase();
        }
        else {
            m_navAgent.isStopped = false;
            Patrol();
        }
    }

    // 巡逻
    private void Patrol() {
        m_navAgent.speed = 2.5f;
        // 到达了下一个路径点
        if (m_navAgent.remainingDistance <= 0.5f) {
            // 在当前路径点站一会
            m_fCurIdleTime += Time.deltaTime;
            // 移动至下一个路径点
            if (m_fCurIdleTime > m_fIdleTime) {
                m_fCurIdleTime = 0; // 重置时间
                ++m_iIndex;
                m_iIndex %= m_WayPoints.Count;
                m_navAgent.SetDestination(m_WayPoints[m_iIndex].position);
            }
        }
    }

    // 追逐
    private void Chase() {
        if (GameManager.Instance.m_MonitoredPlayerPos != Vector3.zero) {
            m_navAgent.destination = GameManager.Instance.m_MonitoredPlayerPos;
            m_navAgent.speed = m_fChaseSpeed;
        }
        else {
            m_navAgent.destination = m_EnemySight.m_posHear;
            m_navAgent.speed = m_fSearchSpeed;
        }

        // 放弃追逐
        if (m_navAgent.remainingDistance < 1.5f) {
            m_fCurChaseTime += Time.deltaTime;
            if (m_fCurChaseTime > m_fChaseTime && !m_EnemySight.m_bPlayerInSight) {
                m_fCurChaseTime = 0;
                GameManager.Instance.m_MonitoredPlayerPos = Vector3.zero;
                m_EnemySight.m_posHear = Vector3.zero;
                AlermLight.Instance.m_bIsOn = false;
            }
        }
    }

    // 射击
    private void Shoot() {
        m_navAgent.isStopped = true;
    }
}