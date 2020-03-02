using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour {

    // 公有变量
    public float m_fSpeed = 1;  // 敌人移动的速度
    public int m_iDamage = 10;  // 敌人的伤害
    public AudioClip[] m_acAttacks;   // 敌人攻击的音效

    // 私有引用
    private Transform m_Player;
    private Rigidbody2D m_rd;
    private BoxCollider2D m_collider;
    private Animator m_animator;

    // 私有变量
    private Vector2 m_posTarget = new Vector2();

    private void Awake() {
        m_Player = GameObject.FindGameObjectWithTag("Player").transform;
        m_rd = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<BoxCollider2D>();
        m_posTarget = transform.position;
        m_animator = GetComponent<Animator>();
        GameManager.Instance.m_Enemys.Add(this);
    }

    private void Update() {
        m_rd.MovePosition(Vector2.Lerp(transform.position, m_posTarget, m_fSpeed * Time.deltaTime));
    }

    public void Action() {
        Vector2 offset = m_Player.position - transform.position;
        if (offset.magnitude < 1.1f) {
            /*攻击*/
            // 播放攻击动画
            m_animator.SetTrigger("Attack");
            // 播放攻击音效
            int iRandom = Random.Range(0, m_acAttacks.Length);
            AudioSource.PlayClipAtPoint(m_acAttacks[iRandom], Camera.main.transform.position);
            // 玩家受到伤害
            m_Player.SendMessage("UnderAttack", m_iDamage);
        }
        else {
            /*移动*/
            float x = 0, y = 0;
            if (Mathf.Abs(offset.x) > Math.Abs(offset.y)) {
                // 按照x轴移动
                if (offset.x > 0) {
                    x += 1;
                }
                else {
                    x -= 1;
                }
            }
            else {
                // 按照y轴移动
                if (offset.y > 0) {
                    y += 1;
                }
                else {
                    y -= 1;
                }
            }
            // 检测是否是玩家要移动的下一个位置
            if (m_posTarget + new Vector2(x, y) == GameManager.Instance.m_posPlayerTarget) {
                return;
            }
            // 检测是否碰撞到墙
            m_collider.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(m_posTarget, m_posTarget + new Vector2(x, y));
            m_collider.enabled = true;
            if (hit.transform == null || hit.collider.tag == "Food") {
                m_posTarget += new Vector2(x, y);
            }
        }
    }
}