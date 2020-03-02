using UnityEngine;
using UnityEngine.EventSystems;

public class Bird : MonoBehaviour {

    // 公有变量
    public float m_fMaxDis = 1f;    // 橡皮筋最大长度
    public Sprite m_spriteHurt;     // 受伤图片
    public AudioClip m_acHurt;      // 受伤音效
    public AudioClip m_acFly;       // 飞行音效
    public float m_fCameraSpeed = 5;    // 相机跟随速度
    public GameObject m_effectDie;

    // 私有引用
    private LineRenderer m_lrLeft;
    private LineRenderer m_lrRight;
    private Vector3 m_posSlingShotLeft = new Vector3();
    private Vector3 m_posSlingShotRight = new Vector3();
    private SpringJoint2D m_sj;
    protected Rigidbody2D m_rb;
    private MyTrail m_trail;    // 拖尾效果

    // 私有变量
    private bool m_bIsClicked;
    private bool m_bIsClickable = true; // 解决小鸟飞出后仍然可以点击小鸟的BUG
    private bool m_bCouldReleaseSkill = true;   // 只有在飞的过程中才能释放技能
    private bool m_bIsFlying;

    private void Awake() {
        m_lrLeft = GameObject.Find("slingShot_left").GetComponent<LineRenderer>();
        m_posSlingShotLeft = GameObject.Find("slingShot_left").transform.Find("pos").position;
        m_lrRight = GameObject.Find("slingShot_right").GetComponent<LineRenderer>();
        m_posSlingShotRight = GameObject.Find("slingShot_right").transform.Find("pos").position;
        m_sj = GetComponent<SpringJoint2D>();
        m_rb = GetComponent<Rigidbody2D>();
        m_trail = GetComponent<MyTrail>();
    }

    private void Update() {
        // 点击的是UI界面 => 返回
        if (EventSystem.current.IsPointerOverGameObject()) {
            return;
        }


        if (m_bIsClicked) {
            // 移动小鸟的位置
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // 减去摄像机多出来的Z轴
            transform.position -= new Vector3(0, 0, Camera.main.transform.position.z);
            // 不能超过橡皮筋最大长度
            if (Vector3.Distance(transform.position, m_posSlingShotLeft) > m_fMaxDis) {
                Vector3 direction = (transform.position - m_posSlingShotLeft).normalized;
                transform.position = m_posSlingShotLeft + m_fMaxDis * direction;
            }

            // 画橡皮筋拉出来的线
            DrawLine();
        }

        // 相机跟随
        float fBirdPosX = transform.position.x;
        Vector3 destPos = new Vector3(Mathf.Clamp(fBirdPosX, 0, 15), Camera.main.transform.position.y, Camera.main.transform.position.z);
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, destPos, m_fCameraSpeed * Time.deltaTime);

        if (Input.GetMouseButtonDown(0) && m_bIsFlying && m_bCouldReleaseSkill) {
            ReleaseSkill();
        }
    }

    private void OnMouseDown() {
        if (m_bIsClickable) {
            m_bIsClicked = true;
            // 小鸟失去物理属性
            m_rb.isKinematic = true;
            // 显示橡皮筋
            m_lrLeft.enabled = true;
            m_lrRight.enabled = true;
        }
    }

    private void OnMouseUp() {
        if (m_bIsClickable) {
            m_bIsClicked = false;
            m_bIsClickable = false;
            // 擦掉橡皮筋
            m_lrLeft.enabled = false;
            m_lrRight.enabled = false;
            // 小鸟恢复物理属性
            m_rb.isKinematic = false;
            // 使小鸟飞出
            Invoke("Fly", 0.1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        m_bIsFlying = false;
        // 碰撞到物体则 取消拖尾效果
        m_trail.heroIdle();
        // 碰撞到物体后3s调用下一只小鸟
        Invoke("NextBird", 3f);
    }

    private void DrawLine() {
        m_lrLeft.SetPosition(0, m_posSlingShotLeft);
        m_lrLeft.SetPosition(1, transform.position);
        m_lrRight.SetPosition(0, m_posSlingShotRight);
        m_lrRight.SetPosition(1, transform.position);
    }

    private void Fly() {
        m_bIsFlying = true;
        // 使弹簧失去作用
        m_sj.enabled = false;
        // 小鸟恢复重力
        m_rb.gravityScale = 1;
        // 播放小鸟飞行声音
        AudioSource.PlayClipAtPoint(m_acFly, transform.position, 0.5f);
        // 播放拖尾效果
        m_trail.heroAttack();
    }

    public void Hurt() {
        // 更换小鸟受伤的图片
        transform.GetComponent<SpriteRenderer>().sprite = m_spriteHurt;
        // 播放小鸟受伤的音效
        AudioSource.PlayClipAtPoint(m_acHurt, transform.position);
    }

    private void NextBird() {
        // 销毁自身
        Die();

        GameManager_Game.Instance.NextBird();
    }

    protected void Die() {
        // 从小鸟数组中移除自身
        GameManager_Game.Instance.m_Birds.Remove(this);
        // 播放死亡特效
        Instantiate(m_effectDie, transform.position, Quaternion.identity);
        // 销毁
        Destroy(gameObject);
    }

    public virtual void ReleaseSkill() {
        // 释放过技能后就不能再释放了
        m_bCouldReleaseSkill = false;
    }
}