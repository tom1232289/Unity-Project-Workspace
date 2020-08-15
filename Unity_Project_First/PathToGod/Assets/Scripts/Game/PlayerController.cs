using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// 公有引用
    /// </summary>
    // 平台层
    public LayerMask m_PlatformLayer;

    // 障碍物层
    public LayerMask m_ObstacleLayer;

    /// <summary>
    /// 私有引用
    /// </summary>
    private ManagerVars m_managerVars;

    private Rigidbody2D m_rb;

    // 向下发射射线的位置，用于检测平台层
    private Transform m_posRayDown;

    // 向左、右发射射线的位置，用于检测障碍物层
    private Transform m_posRayLeft, m_posRayRight;

    private SpriteRenderer m_sr;

    private AudioSource m_as;

    /// <summary>
    /// 私有变量
    /// </summary>
    // 往左跳的位置
    private Vector3 m_posNextPlatformLeft = Vector3.zero;

    // 往右跳的位置
    private Vector3 m_posNextPlatformRight = Vector3.zero;

    // 是否在跳跃状态中 => 在跳跃状态中则不能跳跃
    private bool m_bIsJumping;

    // 射线上一次检测到的平台
    private GameObject m_LastRayPlatform;

    private void Awake()
    {
        EventCenter.AddListener(EventDefine.ChangeSkin, ChangeSkin);

        m_managerVars = ManagerVars.GetManagerVars();
        m_rb = GetComponent<Rigidbody2D>();
        m_posRayDown = transform.Find("posRayDown");
        m_posRayLeft = transform.Find("posRayLeft");
        m_posRayRight = transform.Find("posRayRight");
        m_sr = GetComponent<SpriteRenderer>();
        m_as = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventDefine.ChangeSkin, ChangeSkin);
    }

    private void Start()
    {
        ChangeSkin();
    }

    private void Update()
    {
        //Debug.DrawRay(m_posRayDown.position, Vector2.down * 1f, Color.red);
        //Debug.DrawRay(m_posRayLeft.position, Vector2.left * 0.15f, Color.red);
        //Debug.DrawRay(m_posRayRight.position, Vector2.right * 0.15f, Color.red);

        if (UpdateReturnDecide() == false)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && !m_bIsJumping && m_posNextPlatformLeft != Vector3.zero)
        {
            GameManager.Instance.m_bPlayerIsMoving = true;

            /// <summary>
            /// 生成新的平台
            /// </summary>
            EventCenter.Broadcast(EventDefine.DecidePath);

            /// <summary>
            /// 人物跳跃
            /// </summary>
            Vector3 posMouse = Input.mousePosition;

            // 点击的在屏幕的左边 => 往左边跳
            if (posMouse.x <= Screen.width / 2)
            {
                Jump(true);
            }
            // 点击的在屏幕的右边 => 往右边跳
            else
            {
                Jump(false);
            }
        }

        /// <summary>
        /// 游戏结束1:跳下去了
        /// </summary>
        // (m_rb.velocity.y一直是小于0的，可能和DoTween有关)
        if (m_rb.velocity.y < 0 && GameManager.Instance.m_bIsGameOver == false)
        {
            if (RayHitPlatform() == false)
            {
                JumpDead();
            }
        }

        /// <summary>
        /// 游戏结束2:撞死了
        /// </summary>
        if (m_bIsJumping && GameManager.Instance.m_bIsGameOver == false)
        {
            if (RayHitObstacle() == true)
            {
                ExplosionDeath();
            }
        }

        /// <summary>
        /// 游戏结束3:随平台掉下去了
        /// </summary>
        if (Camera.main.transform.position.y - transform.position.y > 6 && GameManager.Instance.m_bIsGameOver == false)
        {
            SlowDeath();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Platform")
        {
            m_bIsJumping = false;
            /// <summary>
            /// 计算人物要跳的位置
            /// </summary>
            // 当前平台的位置
            Vector3 currentPlatformPos = other.transform.position;
            // 往左跳的位置
            m_posNextPlatformLeft = new Vector3(currentPlatformPos.x - m_managerVars.fNextXPos, currentPlatformPos.y + m_managerVars.fNextYPos, 0);
            // 往右跳的位置
            m_posNextPlatformRight = new Vector3(currentPlatformPos.x + m_managerVars.fNextXPos, currentPlatformPos.y + m_managerVars.fNextYPos, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Pickup")
        {
            // 播放音效
            if (GameManager.Instance.GetMusicOn())
            {
                m_as.PlayOneShot(m_managerVars.m_acDiamond);
            }

            GameManager.Instance.AddDiamond();
            other.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            other.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 是否点击到了UI，双端使用 
    /// </summary>
    private bool IsPointerOverGameObject()
    {
        // 创建一个点击事件
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        // 向点击位置发射一条射线，检测是否点击的是否是UI
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults.Count > 0;
    }

    // Update的return判断
    private bool UpdateReturnDecide()
    {
        // 点击的是UI => 直接返回
        //// 移动端
        //if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        //{
        //    int iFingerId = Input.GetTouch(0).fingerId;
        //    if (EventSystem.current.IsPointerOverGameObject(iFingerId))
        //    {
        //        return false;
        //    }
        //}
        //// 电脑端
        //else
        //{
        //    if (EventSystem.current.IsPointerOverGameObject())
        //    {
        //        return false;
        //    }
        //}
        if (IsPointerOverGameObject())
            return false;

        // 游戏没开始 或者 游戏已经结束了 => 直接返回
        if (GameManager.Instance.m_bIsGameStarted == false || GameManager.Instance.m_bIsGameOver == true)
        {
            return false;
        }

        // 游戏是暂停的 => 直接返回
        if (GameManager.Instance.m_bIsGamePause)
        {
            return false;
        }

        return true;
    }

    private void Jump(bool bJumpLeft)
    {
        m_bIsJumping = true;

        // 播放音效
        if (GameManager.Instance.GetMusicOn())
        {
            m_as.PlayOneShot(m_managerVars.m_acJump);
        }

        if (bJumpLeft)
        {
            // 人物图像反转
            transform.localScale = new Vector3(-1, 1, 1);
            // 人物Y轴移动（Y轴设置的高一点，有一个往下落的效果）
            transform.DOMoveY(m_posNextPlatformLeft.y + 0.8f, 0.2f);
            // 人物X轴移动
            transform.DOMoveX(m_posNextPlatformLeft.x, 0.2f);
        }
        else
        {
            // 人物图像反转
            transform.localScale = Vector3.one;
            // 人物Y轴移动（Y轴设置的高一点，有一个往下落的效果）
            transform.DOMoveY(m_posNextPlatformRight.y + 0.8f, 0.2f);
            // 人物X轴移动
            transform.DOMoveX(m_posNextPlatformRight.x, 0.2f);
        }
    }

    /// <summary>
    /// 射线是否检测到了平台
    /// </summary>
    /// <returns></returns>
    private bool RayHitPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(m_posRayDown.position, Vector2.down, 1f, m_PlatformLayer);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Platform")
            {
                // 射线检测的不是上一次的平台 => 加分
                if (m_LastRayPlatform != hit.collider.gameObject)
                {
                    // 处理第一次的情况
                    if (m_LastRayPlatform == null)
                    {
                        m_LastRayPlatform = hit.collider.gameObject;
                        return true;
                    }

                    // 防止射线检测 检测到的平台 一会是跳起的平台，一会是要跳到的平台，在两个平台之间快速切换 导致的 一次加好多分
                    if (m_LastRayPlatform.transform.position.y > hit.collider.transform.position.y)
                    {
                        return true;
                    }

                    GameManager.Instance.AddScore();
                    m_LastRayPlatform = hit.collider.gameObject;
                }

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 射线是否检测到了障碍物
    /// </summary>
    /// <returns></returns>
    private bool RayHitObstacle()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(m_posRayLeft.position, Vector2.left, 0.15f, m_ObstacleLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(m_posRayRight.position, Vector2.right, 0.15f, m_ObstacleLayer);

        if (hitLeft.collider != null)
        {
            if (hitLeft.collider.tag == "Obstacle")
            {
                return true;
            }
        }

        if (hitRight.collider != null)
        {
            if (hitRight.collider.tag == "Obstacle")
            {
                return true;
            }
        }

        return false;
    }

    private void JumpDead()
    {
        GameManager.Instance.m_bIsGameOver = true;

        // 播放音效
        if (GameManager.Instance.GetMusicOn())
        {
            m_as.PlayOneShot(m_managerVars.m_acFall);
        }

        // 更改人物的sortingLayer，使平台、障碍物挡在人物前面
        m_sr.sortingLayerName = "Default";
        // 禁用人物的BoxCollider2D，使人物可以掉下去
        GetComponent<BoxCollider2D>().enabled = false;

        // 调用结束面板
        StartCoroutine(DelayShowGameOverPanel());
    }

    private void ExplosionDeath()
    {
        GameManager.Instance.m_bIsGameOver = true;

        // 播放音效
        if (GameManager.Instance.GetMusicOn())
        {
            m_as.PlayOneShot(m_managerVars.m_acHit);
        }

        // 播放死亡特效
        GameObject go = ObjectPool.Instance.GetExplosionEffect();
        go.transform.position = transform.position;
        go.SetActive(true);

        m_sr.enabled = false;

        // 调用结束面板
        StartCoroutine(DelayShowGameOverPanel());
    }

    private void SlowDeath()
    {
        GameManager.Instance.m_bIsGameOver = true;

        // 播放音效
        if (GameManager.Instance.GetMusicOn())
        {
            m_as.PlayOneShot(m_managerVars.m_acFall);
        }

        // 调用结束面板
        StartCoroutine(DelayShowGameOverPanel());
    }

    private IEnumerator DelayShowGameOverPanel()
    {
        yield return new WaitForSeconds(1);
        EventCenter.Broadcast(EventDefine.ShowGameOverPanel);
    }

    /// <summary>
    /// 更换人物皮肤
    /// </summary>
    private void ChangeSkin()
    {
        m_sr.sprite = m_managerVars.m_listBackSkinSprite[GameManager.Instance.GetSelectedSkin()];
    }
}