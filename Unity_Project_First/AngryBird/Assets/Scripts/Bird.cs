using UnityEngine;

public class Bird : MonoBehaviour {

    // 公有引用
    public Transform m_RightRubberPos;
    public Transform m_LeftRubberPos;
    public LineRenderer m_lrRight;
    public LineRenderer m_lrLeft;
    public GameObject m_eftBoom;
    public AudioClip m_clipSelectBird;
    public AudioClip m_clipBirdFly;
    public Sprite m_spriteBirdHurt;

    // 公有变量
    public float m_fMaxDis = 1;     // 皮筋最大长度
    public float m_fSmooth = 3;     // 镜头移动的速度

    // 私有引用
    [HideInInspector]
    public SpringJoint2D m_sp;

    [HideInInspector]
    public Rigidbody2D m_rd;
    private SpriteRenderer m_srBirdHurt;
    private Trail m_BirdTrail;

    // 私有变量
    private bool m_bIsClicked;
    private bool m_bCouldClick = true;
    private bool m_bCouldReleaseSkill;
    private bool m_bIsDie;

    private void Awake() {
        m_sp = GetComponent<SpringJoint2D>();
        m_rd = GetComponent<Rigidbody2D>();
        m_BirdTrail = GetComponent<Trail>();
        m_srBirdHurt = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if (m_bIsClicked) {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position -= new Vector3(0, 0, Camera.main.transform.position.z);
            if (Vector3.Distance(transform.position, m_RightRubberPos.position) > m_fMaxDis) {
                Vector3 direction = (transform.position - m_RightRubberPos.position).normalized;     // 方向单位化
                transform.position = m_RightRubberPos.position + m_fMaxDis * direction;
            }

            // 画出皮筋
            DrawRubber();
        }

        // 相机跟随
        float birdPosX = transform.position.x;
        Vector3 destPos = new Vector3(Mathf.Clamp(birdPosX, 0, 15), Camera.main.transform.position.y, Camera.main.transform.position.z);
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, destPos, m_fSmooth * Time.deltaTime);

        if (m_bCouldReleaseSkill) {
            if (Input.GetMouseButtonDown(0)) {
                ReleaseSkill();
            }
        }
    }

    private void OnMouseDown() {
        if (m_bCouldClick) {
            m_bIsClicked = true;
            m_rd.isKinematic = true;

            // 播放音效
            AudioSource.PlayClipAtPoint(m_clipSelectBird, transform.position);

            // 启用画线组件
            m_lrRight.enabled = true;
            m_lrLeft.enabled = true;
        }
    }

    private void OnMouseUp() {
        if (m_bCouldClick) {
            m_bIsClicked = false;
            m_rd.isKinematic = false;

            Invoke("Fly", 0.1f);

            // 禁用画线组件
            m_lrRight.enabled = false;
            m_lrLeft.enabled = false;

            m_bCouldClick = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        m_bCouldReleaseSkill = false;
        m_BirdTrail.heroIdle();
    }

    private void Fly() {
        m_sp.enabled = false;
        m_bCouldReleaseSkill = true;

        // 播放飞行音效
        AudioSource.PlayClipAtPoint(m_clipBirdFly, transform.position);

        // 启用拖尾
        m_BirdTrail.heroAttack();

        // 下一只小鸟
        Invoke("Next", 3);
    }

    private void DrawRubber() {
        m_lrRight.SetPosition(0, m_RightRubberPos.position);
        m_lrRight.SetPosition(1, transform.position);

        m_lrLeft.SetPosition(0, m_LeftRubberPos.position);
        m_lrLeft.SetPosition(1, transform.position);
    }

    protected virtual void Next() {
        Die();
        GameManager.Instance.NextBird();
    }

    protected void Die() {
        if (!m_bIsDie) {
            GameManager.Instance.m_listBirds.Remove(this);
            Destroy(gameObject);
            Instantiate(m_eftBoom, transform.position, Quaternion.identity);
            m_bIsDie = true;
        }
    }

    public virtual void ReleaseSkill() {
        m_bCouldReleaseSkill = false;
    }

    public void Hurt() {
        m_srBirdHurt.sprite = m_spriteBirdHurt;
    }
}