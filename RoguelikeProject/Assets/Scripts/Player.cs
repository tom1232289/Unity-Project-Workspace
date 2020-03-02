using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    // 公有引用
    public AudioClip[] m_acMove;    // 移动的音效

    // 公有变量
    public Vector2 m_posTarget = new Vector2(1, 1);    // 玩家要去到的下一个位置
    public float m_fSpeed = 20;  // 移动速度

    // 私有引用
    private Rigidbody2D m_rd;
    private BoxCollider2D m_collider;
    private Animator m_animator;

    // 私有变量
    private float m_fMoveCD = 0.4f;    // 移动的冷却时间
    private float m_fCurMoveCD; // 当前移动的冷却时间

    private void Awake() {
        m_rd = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<BoxCollider2D>();
        m_animator = GetComponent<Animator>();
    }

    private void Update() {
        m_rd.MovePosition(Vector2.Lerp(transform.position, m_posTarget, m_fSpeed * Time.deltaTime));

        if (!GameManager.Instance.m_bCanMove) {
            if (Input.GetKeyDown(KeyCode.Return)) {
                SceneManager.LoadScene("Start");
            }
            return;
        }

        m_fCurMoveCD += Time.deltaTime;
        if (m_fCurMoveCD < m_fMoveCD) {
            return;
        }
        float fX = Input.GetAxisRaw("Horizontal");
        float fY = Input.GetAxisRaw("Vertical");
        // 优先水平移动
        if (fX > 0) {
            fY = 0;
        }
        // 开始移动
        if (fX != 0 || fY != 0) {
            m_fCurMoveCD = 0;
            GameManager.Instance.ReduceFood(1);
            m_collider.enabled = false;
            RaycastHit2D hit = Physics2D.Linecast(m_posTarget, m_posTarget + new Vector2(fX, fY));
            m_collider.enabled = true;
            // 没有碰到物体 => 可以移动
            if (hit.transform == null) {
                // 播放音效
                int iRandom = Random.Range(0, m_acMove.Length);
                AudioSource.PlayClipAtPoint(m_acMove[iRandom], Camera.main.transform.position);
                m_posTarget += new Vector2(fX, fY);
            }
            else if (hit.collider.tag == "HardWall") {
                return;
            }
            else if (hit.collider.tag == "Wall") {
                // 播放攻击动画
                m_animator.SetTrigger("Attack");
                // 墙体受到攻击
                hit.collider.SendMessage("UnderAttack");
            }
            else if (hit.collider.tag == "Food") {
                // 吃食物
                hit.collider.SendMessage("EatFood");
                // 播放移动音效
                int iRandom = Random.Range(0, m_acMove.Length);
                AudioSource.PlayClipAtPoint(m_acMove[iRandom], Camera.main.transform.position);
                // 移动过去
                m_posTarget += new Vector2(fX, fY);
            }
            // 发送玩家移动了的消息
            GameManager.Instance.m_posPlayerTarget = m_posTarget;
            GameManager.Instance.OnPlayerMoved();
        }
    }

    private void UnderAttack(int damage) {
        // 播放受伤动画
        m_animator.SetTrigger("Hurt");
        // 减少食物
        GameManager.Instance.ReduceFood(damage);
    }
}