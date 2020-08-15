using System.Collections;
using UnityEngine;

public class Platform : MonoBehaviour
{
    /// <summary>
    /// 公有引用
    /// </summary>
    // 要改变Sprite的平台
    public SpriteRenderer[] m_spriteRenderers;

    // 旁边的障碍物
    public GameObject m_goObstacle;

    /// <summary>
    /// 私有引用
    /// </summary>
    private Rigidbody2D m_rb;

    /// <summary>
    /// 私有变量 
    /// </summary>
    // 多少秒后掉落
    private float m_fFallTime = 2;

    private void Awake()
    {
        EventCenter.AddListener<float>(EventDefine.UpdatePaltformFallTime, UpdatePaltformFallTime);

        m_rb = GetComponent<Rigidbody2D>();
    }

    private void Destroy()
    {
        EventCenter.RemoveListener<float>(EventDefine.UpdatePaltformFallTime, UpdatePaltformFallTime);
    }

    private void Update()
    {
        // 优化：摄像机看不到的平台，让它回到池子里
        if (Camera.main.transform.position.y - transform.position.y > 7)
        {
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        /// <summary>
        /// 平台到时间后掉落
        /// </summary>
        if (gameObject.activeInHierarchy && GameManager.Instance.m_bIsGameStarted && GameManager.Instance.m_bPlayerIsMoving)
        {
            m_fFallTime -= Time.deltaTime;
            if (m_fFallTime <= 0)
            {
                Fall();
            }
        }
    }

    public void Init(Sprite sprite, int iDir)
    {
        /// <summary>
        /// 还原属性
        /// </summary>
        m_rb.bodyType = RigidbodyType2D.Static;
        m_fFallTime = GameManager.Instance.m_fInitPlatformFallTime;

        /// <summary>
        /// 改变平台的Sprite
        /// </summary>
        foreach (var spriteRenderer in m_spriteRenderers)
        {
            spriteRenderer.sprite = sprite;
        }

        /// <summary>
        /// 障碍物生成在左边还是右边
        /// </summary>
        // 障碍物生成在右边（默认为左边，所以生成在左边不需要处理）
        if (iDir == 1)
        {
            if (m_goObstacle != null)
            {
                // 把障碍物扔到右边去
                Vector3 pos = m_goObstacle.transform.localPosition;
                m_goObstacle.transform.localPosition = new Vector3(pos.x * -1, pos.y, pos.z);
            }
        }
    }

    /// <summary>
    /// 平台往下掉落
    /// </summary>
    private void Fall()
    {
        if (GameManager.Instance.m_bIsGameStarted && GameManager.Instance.m_bIsGamePause == false)
        {
            if (m_rb.bodyType != RigidbodyType2D.Dynamic)
            {
                m_rb.bodyType = RigidbodyType2D.Dynamic;
                StartCoroutine(DealyHide());
            }
        }
    }

    private IEnumerator DealyHide()
    {
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);
    }

    private void UpdatePaltformFallTime(float fFallTime)
    {
        m_fFallTime = fFallTime;
    }
}