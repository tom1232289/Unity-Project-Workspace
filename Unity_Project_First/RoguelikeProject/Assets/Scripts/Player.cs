using UnityEngine;

public class Player : MonoBehaviour {
    // 公有引用
    public AudioClip[] m_acMoves; // 移动的音效

    // 公有变量
    public float m_fSpeed = 5;

    // 私有引用
    private Rigidbody2D m_rd;
    private BoxCollider2D m_collider2d;
    private Animator m_animator;

    // 私有变量
    private Vector2 m_posTarget = new Vector2(1, 1);    // 目标位置
    private float m_fCDTime = 0.5f;     // 移动的冷却时间
    private float m_fCurCDTime = 1f;    // 当前移动的冷却时间

    private void Awake() {
        m_rd = GetComponent<Rigidbody2D>();
        m_collider2d = GetComponent<BoxCollider2D>();
        m_animator = GetComponent<Animator>();
    }

    private void Update() {
        // 失败
        if (!GameManager.Instance.m_bCanMove) {
            return;
        }

        // 主角移动
        m_rd.MovePosition(Vector2.Lerp(transform.position, m_posTarget, m_fSpeed * Time.deltaTime));
        // 增加当前移动的冷却时间
        m_fCurCDTime += Time.deltaTime;
        // 移动冷却中
        if (m_fCurCDTime < m_fCDTime) {
            return;
        }
        // 获取要移动的位置
        float fHorizontal = Input.GetAxisRaw("Horizontal");
        float fVertical = Input.GetAxisRaw("Vertical");
        if (fHorizontal > 0) {
            fVertical = 0;
        }

        if (fHorizontal != 0 || fVertical != 0) {
            // 重置当前移动和攻击的冷却时间
            m_fCurCDTime = 0f;
            // 食物减1
            GameManager.Instance.ReduceFood(1);
            // 检测碰撞到的物体
            m_collider2d.enabled = false;   // 防止射线检测到自己的collider
            RaycastHit2D hit = Physics2D.Linecast(m_posTarget, m_posTarget + new Vector2(fHorizontal, fVertical));
            m_collider2d.enabled = true;
            // 没有碰到物体 => 移动
            if (hit.transform == null) {
                // 播放移动音效
                int iRandom = Random.Range(0, m_acMoves.Length);
                AudioSource.PlayClipAtPoint(m_acMoves[iRandom], Camera.main.transform.position);
                m_posTarget += new Vector2(fHorizontal, fVertical);
            }
            // 碰到了物体
            else if (hit.collider.tag == "HardWall") {
                return;
            }
            else if (hit.collider.tag == "Wall") {
                // 播放攻击动画
                m_animator.SetTrigger("Attack");

                // 墙体受到攻击
                hit.collider.SendMessage("UnderAttack");
                return;
            }
            else if (hit.collider.tag == "Food") {
                hit.collider.SendMessage("AddFood");
                hit.collider.SendMessage("Die");
                // 播放移动音效
                int iRandom = Random.Range(0, m_acMoves.Length);
                AudioSource.PlayClipAtPoint(m_acMoves[iRandom], Camera.main.transform.position);
                // 移动过去
                m_posTarget += new Vector2(fHorizontal, fVertical);
            }
            else if (hit.collider.tag == "Enemy") {
                return;
            }
            // 通知所有人主角移动了
            GameManager.Instance.m_posPlayerTarget = m_posTarget;
            GameManager.Instance.OnPlayerMove();
        }
    }

    // 受到攻击
    private void UnderAttack(int iDamage) {
        GameManager.Instance.ReduceFood(iDamage);
        // 播放受到攻击的动画
        m_animator.SetTrigger("Hurt");
    }
}