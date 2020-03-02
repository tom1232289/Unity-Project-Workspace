using UnityEngine;

public class Player : MonoBehaviour {
    public float m_fMoveSpeed = 3f;
    public Sprite[] m_TankSprites; // 上、右、下、左
    public GameObject m_BulletPrefab;
    public float m_fTimeInterval;
    public GameObject m_ExplosionPrefab;
    public GameObject m_ShieldPrefab;
    public AudioSource m_MoveAudio;
    public AudioClip[] m_AudioClipTank;

    private float m_fHorizontal;
    private float m_fVertical;
    private SpriteRenderer m_SpriteRenderer;
    private Vector3 m_BulletEulerAngles;
    private bool m_bIsDefended = true;
    private float m_fDefendTime = 3f;

    private void Awake() {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate() {
        if (PlayerManager.Instanse.m_bIsGameover)
            return;

        Move();
    }

    private void Update() {
        if (PlayerManager.Instanse.m_bIsGameover)
            return;

        // 是否处于护盾状态
        if (m_bIsDefended) {
            m_ShieldPrefab.SetActive(true);
            m_fDefendTime -= Time.deltaTime;
            if (m_fDefendTime <= 0) {
                m_bIsDefended = false;
                m_ShieldPrefab.SetActive(false);
            }
        }

        // 攻击CD
        if (m_fTimeInterval > 0.4f) {
            Attack();
        }
        else {
            m_fTimeInterval += Time.deltaTime;
        }
    }



    private void Move() {
        m_fVertical = Input.GetAxis("Vertical");
        if (m_fVertical > 0) {
            m_SpriteRenderer.sprite = m_TankSprites[0];
            m_BulletEulerAngles = new Vector3(0, 0, 0);
        }
        else if (m_fVertical < 0) {
            m_SpriteRenderer.sprite = m_TankSprites[2];
            m_BulletEulerAngles = new Vector3(0, 0, 180);
        }

        transform.Translate(Vector3.up * m_fVertical * m_fMoveSpeed * Time.deltaTime);

        if (Mathf.Abs(m_fVertical) > 0.05f) {
            m_MoveAudio.clip = m_AudioClipTank[1];
            if (!m_MoveAudio.isPlaying) {
                m_MoveAudio.Play();
            }
        }

        if (m_fVertical != 0)
            return;

        m_fVertical = 0;
        m_fHorizontal = Input.GetAxis("Horizontal");
        if (m_fHorizontal > 0) {
            m_SpriteRenderer.sprite = m_TankSprites[1];
            m_BulletEulerAngles = new Vector3(0, 0, -90);
        }
        else if (m_fHorizontal < 0) {
            m_SpriteRenderer.sprite = m_TankSprites[3];
            m_BulletEulerAngles = new Vector3(0, 0, 90);
        }

        transform.Translate(Vector3.right * m_fHorizontal * m_fMoveSpeed * Time.deltaTime);

        if (Mathf.Abs(m_fHorizontal) > 0.05f) {
            m_MoveAudio.clip = m_AudioClipTank[1];
            if (!m_MoveAudio.isPlaying) {
                m_MoveAudio.Play();
            }
        }
        else {
            m_MoveAudio.clip = m_AudioClipTank[0];
            if (!m_MoveAudio.isPlaying) {
                m_MoveAudio.Play();
            }
        }
    }

    private void Attack() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            // 子弹产生的角度：当前坦克的角度+子弹应该旋转的角度
            Instantiate(m_BulletPrefab, transform.position, Quaternion.Euler( /*transform.eulerAngles + */m_BulletEulerAngles));
            m_fTimeInterval = 0;
        }
    }

    private void Die() {
        if (m_bIsDefended)
            return;

        // 设置玩家坦克为死亡状态
        PlayerManager.Instanse.m_bIsDead = true;

        // 产生爆炸特效
        Instantiate(m_ExplosionPrefab, transform.position, transform.rotation);

        // 死亡
        Destroy(gameObject);
    }
}