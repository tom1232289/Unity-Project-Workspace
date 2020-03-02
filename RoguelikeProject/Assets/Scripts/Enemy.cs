using UnityEngine;

public class Enemy : MonoBehaviour {

    // 公有引用
    public AudioClip[] m_acAttack;  // 僵尸攻击的音效

    // 公有变量
    public float m_fSpeed = 20;  // 僵尸移动的速度
    public int m_iAttack = 10;   // 僵尸的攻击力

    // 私有引用
    private Rigidbody2D m_rb;
    private Transform m_player;
    private BoxCollider2D m_collider;
    private Animator m_animator;

    // 私有变量
    private Vector2 m_posTarget = new Vector2();    // 敌人要去的方格位置

    private void Awake() {
        m_rb = GetComponent<Rigidbody2D>();
        m_player = GameObject.FindGameObjectWithTag("Player").transform;
        m_collider = GetComponent<BoxCollider2D>();
        m_animator = GetComponent<Animator>();
        m_posTarget = transform.position;
        GameManager.Instance.m_listEnmeys.Add(this);
    }

    private void Update() {
        m_rb.MovePosition(Vector2.Lerp(transform.position, m_posTarget, m_fSpeed * Time.deltaTime));
    }

    public void Action() {
        Vector2 offset = m_player.position - transform.position;
        if (offset.magnitude < 1.1f) {
            /*攻击*/
            // 播放音效
            int iRandom = Random.Range(0, m_acAttack.Length);
            AudioSource.PlayClipAtPoint(m_acAttack[iRandom], Camera.main.transform.position);
            // 播放攻击动画
            m_animator.SetTrigger("Attack");
            // 玩家受到攻击
            m_player.SendMessage("UnderAttack", m_iAttack);
        }
        else {
            /*移动*/
            float fX = 0, fY = 0;
            if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y)) {
                // 按照X轴移动
                if (offset.x > 0) {
                    fX += 1;
                }
                else {
                    fX -= 1;
                }
            }
            else {
                // 按照Y轴移动
                if (offset.y > 0) {
                    fY += 1;
                }
                else {
                    fY -= 1;
                }
            }
            // 判断 敌人要走的位置 是否 是玩家要走的位置 => 是则不走
            if (m_posTarget + new Vector2(fX, fY) == GameManager.Instance.m_posPlayerTarget) {
                return;
            }
            // 判断前方是否有障碍
            m_collider.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(m_posTarget, m_posTarget + new Vector2(fX, fY));
            m_collider.enabled = true;
            if (hit.transform == null || hit.transform.tag == "Food") {
                m_posTarget += new Vector2(fX, fY);
            }
        }
    }
}