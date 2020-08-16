using UnityEngine;

public class EnemyController : MonoBehaviour
{
    /// <summary>
    /// 公有引用
    /// </summary>
    // 机器人修好的音效
    public AudioClip m_acFixed;

    // 击中机器人的音效数组
    public AudioClip[] m_acHurts;

    // 击中机器人的特效
    public GameObject m_effectHit;

    /// <summary>
    /// 私有引用
    /// </summary>
    private Rigidbody2D m_rigidbody2d;

    private Animator m_anim;

    private AudioSource m_audioSource;

    // 冒烟的特效
    private ParticleSystem m_effectSmoke;

    /// <summary>
    /// 公有变量
    /// </summary>
    // 移动的方向（true：水平方向；false：垂直方向）
    public bool m_bIsHorizontal;

    // 移动的速度
    public float m_fMoveSpeed = 1;

    // 经过__时间改变方向
    public float m_fChangeDirTime = 3;

    /// <summary>
    /// 私有变量
    /// </summary>
    // 移动的方向（1：正方向；-1：反方向）
    private int m_iDirection = 1;

    // 改变方向计时器
    private float m_fChangeDirTimer;

    // 是否是故障状态
    private bool m_bIsBroken = true;

    private void Awake()
    {
        m_rigidbody2d = GetComponent<Rigidbody2D>();
        m_anim = GetComponent<Animator>();
        m_audioSource = GetComponent<AudioSource>();
        m_effectSmoke = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        //m_anim.SetFloat("fMove", m_iDirection);
        //m_anim.SetBool("bIsHorizontal", m_bIsHorizontal);
        PlayMoveAnimation();
    }

    private void Update()
    {
        if (!m_bIsBroken)
        {
            return;
        }

        Move();

        ChangeDirection();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "player")
        {
            RubyController rubyController = other.gameObject.GetComponent<RubyController>();
            rubyController.ChangeHp(-1);
        }
    }

    private void Move()
    {
        Vector2 pos = transform.position;
        if (m_bIsHorizontal)
        {
            pos.x += Time.deltaTime * m_fMoveSpeed * m_iDirection;
        }
        else
        {
            pos.y += Time.deltaTime * m_fMoveSpeed * m_iDirection;
        }
        m_rigidbody2d.MovePosition(pos);
    }

    private void ChangeDirection()
    {
        m_fChangeDirTimer += Time.deltaTime;
        if (m_fChangeDirTimer >= m_fChangeDirTime)
        {
            m_iDirection *= -1;
            //m_anim.SetFloat("fMove", m_iDirection);
            PlayMoveAnimation();
            m_fChangeDirTimer = 0;
        }
    }

    private void PlayMoveAnimation()
    {
        if (m_bIsHorizontal)
        {
            m_anim.SetFloat("fMoveX", m_iDirection);
            m_anim.SetFloat("fMoveY", 0);
        }
        else
        {
            m_anim.SetFloat("fMoveX", 0);
            m_anim.SetFloat("fMoveY", m_iDirection);
        }
    }

    public void Fix()
    {
        m_bIsBroken = false;
        m_rigidbody2d.simulated = false;
        m_anim.SetTrigger("Fixed");
        m_effectSmoke.Stop();

        // 播放击中机器人的特效
        Instantiate(m_effectHit, transform.position, Quaternion.identity);

        // 停止播放走路音效
        m_audioSource.Stop();
        // 播放机器人被击中音效
        if (m_acHurts.Length > 0)
        {
            int iRandom = Random.Range(0, m_acHurts.Length);
            m_audioSource.volume = 0.7f;
            m_audioSource.PlayOneShot(m_acHurts[iRandom]);
        }
        // 延时播放机器人修好的音效
        Invoke("PlayFixedSound", 0.5f);
    }

    /// <summary>
    /// 播放机器人修好的音效
    /// </summary>
    private void PlayFixedSound()
    {
        m_audioSource.PlayOneShot(m_acFixed);
    }
}