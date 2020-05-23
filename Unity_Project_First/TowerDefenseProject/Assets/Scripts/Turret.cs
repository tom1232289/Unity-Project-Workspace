using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    // 公有引用
    public GameObject m_prefabBullet;   // 子弹的预制体
    public float m_fAttackCD = 1f;  // 攻击的间隔

    // 公有变量
    public bool m_bIsLaser = false;
    public float m_fLaserDamage = 70f;

    // 私有引用
    private Transform m_posFire;        // 炮口
    private Transform m_transHead;      // 炮身
    private LineRenderer m_lrLaser;
    private GameObject m_effectLaserHurt;   // 被激光到的特效

    // 私有变量
    private List<GameObject> m_listEnemys = new List<GameObject>();
    private float m_fCurTime;

    private void Awake() {
        m_transHead = transform.Find("Head");
        m_posFire = m_transHead.Find("Turret").Find("FirePos");
        m_fCurTime = m_fAttackCD;
        if (m_bIsLaser) {
            m_lrLaser = transform.GetComponentInChildren<LineRenderer>();
            m_effectLaserHurt = m_posFire.Find("Laser").Find("effectLaserHurt").gameObject;
        }
    }

    private void Update() {
        // 炮口转向
        if (m_listEnemys.Count > 0 && m_listEnemys[0] != null) {
            Vector3 targetPos = m_listEnemys[0].transform.position;
            targetPos.y = m_transHead.position.y;
            m_transHead.LookAt(targetPos);
        }

        // 子弹攻击
        if (m_bIsLaser == false) {
            m_fCurTime += Time.deltaTime;
            if (m_listEnemys.Count > 0 && m_fCurTime >= m_fAttackCD) {
                m_fCurTime = 0;
                Attack();
            }
        }
        // 激光攻击
        else {
            if (m_listEnemys.Count > 0) {
                if (m_listEnemys[0] == null) {
                    UpdateEnemys();
                }

                if (m_listEnemys.Count > 0) {
                    if (m_lrLaser.enabled == false)
                        m_lrLaser.enabled = true;
                    m_lrLaser.SetPositions(new Vector3[] { m_posFire.position, m_listEnemys[0].transform.position });
                    // 造成伤害
                    m_listEnemys[0].GetComponent<Enemy>().UnderAttack(m_fLaserDamage * Time.deltaTime);
                    // 播放特效
                    m_effectLaserHurt.SetActive(true);
                    m_effectLaserHurt.transform.position = m_listEnemys[0].transform.position;
                    Vector3 pos = transform.position;
                    pos.y = m_listEnemys[0].transform.position.y;
                    m_effectLaserHurt.transform.LookAt(pos);
                }
            }
            else {
                m_lrLaser.enabled = false;
                m_effectLaserHurt.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Enemy") {
            m_listEnemys.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Enemy") {
            m_listEnemys.Remove(other.gameObject);
        }
    }

    private void Attack() {
        if (m_listEnemys[0] == null) {
            UpdateEnemys();
        }
        if (m_listEnemys.Count > 0) {
            GameObject bullet = GameObject.Instantiate(m_prefabBullet, m_posFire.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().SetTarget(m_listEnemys[0].transform);
        }
        else {
            m_fCurTime = m_fAttackCD;
        }
    }

    private void UpdateEnemys() {
        m_listEnemys.RemoveAll(item => item == null);
    }
}