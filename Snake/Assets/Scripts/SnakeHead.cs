using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SnakeHead : MonoBehaviour {

    // 公有引用
    public Sprite[] m_SnakeBodySprites;         // 蛇身图像的预制体
    public Transform[] m_transWalls;            // 墙的位置（上下左右）
    public GameObject m_prefabDie;              // 死亡特效
    public GameObject m_prefabSnakeBody;        // 蛇身的预制体
    public AudioClip m_clipDie;
    public AudioClip m_clipEat;

    // 公有变量
    public float m_fSpeed = 0.35f;              // 蛇头移动的速度
    public float m_fKeyDownCD = 0.35f;          // 按键的CD
    public int m_iTimesSpeed = 2;               // 蛇头移动的倍速

    // 私有变量
    private float m_fX;                         // x轴偏移量
    private float m_fY;                         // y轴偏移量
    private float m_fStep = 30f;                // 每次移动多少个像素
    private bool m_bIsCD;                       // 按键是否处于CD状态
    private float m_fCurrKeyDownCD;             // 当前的按键CD了多少秒
    private List<Transform> m_listSnakeBodys = new List<Transform>();   // 蛇身的List
    private Vector3 m_oldPosition;              // 蛇头之前的位置
    private GameObject m_SnakeBodys;            // 存放蛇身的空物体
    private bool m_bMoved;                      // 蛇头是否移动过
    private bool m_bIsDie;                      // 是否死亡

    private enum MoveDirection { UP, DOWN, LEFT, RIGHT };
    private MoveDirection m_moveDirection;

    private void Awake() {
        m_SnakeBodys = new GameObject("SnakeBodys");
        m_SnakeBodys.transform.SetParent(GameManager.Instance.m_Canvas.transform, false);

        string sSkinHead;
        string[] sSkinBody = new string[2];
        if (GameManager.Instance.m_iSnakeSkin == 0) {
            sSkinHead = "sh01";
            sSkinBody[0] = "sb0101";
            sSkinBody[1] = "sb0102";
        }
        else {
            sSkinHead = "sh02";
            sSkinBody[0] = "sb0201";
            sSkinBody[1] = "sb0202";
        }

        this.GetComponent<Image>().sprite = Resources.Load<Sprite>(sSkinHead);
        // 加载皮肤：蛇身
        m_SnakeBodySprites[0] = Resources.Load<Sprite>(sSkinBody[0]);
        m_SnakeBodySprites[1] = Resources.Load<Sprite>(sSkinBody[1]);
    }

    private void Start() {
        // 一开始蛇头向上移动
        m_fX = 0;
        m_fY = m_fStep;
        InvokeRepeating("Move", 0, m_fSpeed);
        // 创建食物
        GameManager.Instance.FoodMaker();
    }

    private void Update() {
        if (GameManager.Instance.m_bIsPause) {
            return;
        }

        if (m_bIsDie) {
            CancelInvoke();
            return;
        }

        // 按下空格加速
        if (Input.GetKeyDown(KeyCode.Space)) {
            CancelInvoke();
            InvokeRepeating("Move", 0, m_fSpeed / m_iTimesSpeed);
        }
        else if (Input.GetKeyUp(KeyCode.Space)) {
            CancelInvoke();
            InvokeRepeating("Move", 0, m_fSpeed);
        }

        if (!m_bIsCD) {
            if (Input.GetKeyDown(KeyCode.W)) {
                // 不允许掉头
                if (m_moveDirection == MoveDirection.DOWN) {
                    return;
                }

                // 移动蛇头
                m_fX = 0;
                m_fY = m_fStep;
                // 旋转蛇头
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                // 更新前进方向
                m_moveDirection = MoveDirection.UP;
                // 按键进入CD状态
                m_bIsCD = true;
            }
            else if (Input.GetKeyDown(KeyCode.S)) {
                if (m_moveDirection == MoveDirection.UP) {
                    return;
                }

                m_fX = 0;
                m_fY = -m_fStep;

                transform.localRotation = Quaternion.Euler(0, 0, 180);

                m_moveDirection = MoveDirection.DOWN;

                m_bIsCD = true;
            }
            else if (Input.GetKeyDown(KeyCode.A)) {
                if (m_moveDirection == MoveDirection.RIGHT) {
                    return;
                }

                m_fX = -m_fStep;
                m_fY = 0;

                transform.localRotation = Quaternion.Euler(0, 0, 90);

                m_moveDirection = MoveDirection.LEFT;

                m_bIsCD = true;
            }
            else if (Input.GetKeyDown(KeyCode.D)) {
                if (m_moveDirection == MoveDirection.LEFT) {
                    return;
                }

                m_fX = m_fStep;
                m_fY = 0;

                transform.localRotation = Quaternion.Euler(0, 0, -90);

                m_moveDirection = MoveDirection.RIGHT;

                m_bIsCD = true;
            }
        }
        else {
            if (m_fCurrKeyDownCD > m_fKeyDownCD) {
                m_bIsCD = false;
                m_fCurrKeyDownCD = 0;
            }
            else {
                m_fCurrKeyDownCD += Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Food") {
            // 加分
            GameManager.Instance.m_iScore += 5;
            GameManager.Instance.m_textScore.text = GameManager.Instance.m_iScore.ToString();

            Destroy(other.gameObject);
            Grow();
            GameManager.Instance.FoodMaker();
        }
        else if (other.tag == "Reward") {
            int iRandom = Random.Range(5, 10);
            GameManager.Instance.m_iScore += (iRandom * 10);
            GameManager.Instance.m_textScore.text = GameManager.Instance.m_iScore.ToString();
            Destroy(other.gameObject);
            Grow();
        }
        else if (other.tag == "Wall") {
            if (GameManager.Instance.m_iBorderMode == 1) {
                if (m_bMoved) {
                    CrossWall(other);
                    m_bMoved = false;
                }
            }
            else {
                Die();
            }
        }
        else if (other.tag == "SnakeBody") {
            Die();
        }
    }

    private void Move() {
        // 刷新蛇头的位置
        m_oldPosition = transform.localPosition;
        transform.localPosition = new Vector3(m_oldPosition.x + m_fX, m_oldPosition.y + m_fY, m_oldPosition.z);

        // 刷新蛇身的位置
        for (int i = m_listSnakeBodys.Count - 1; i > 0; --i) {
            m_listSnakeBodys[i].localPosition = m_listSnakeBodys[i - 1].localPosition;
        }
        if (m_listSnakeBodys.Count > 0) {
            m_listSnakeBodys[0].localPosition = m_oldPosition;
        }

        m_bMoved = true;
    }

    private void Grow() {
        // 播放吃东西音效
        AudioSource.PlayClipAtPoint(m_clipEat, Vector3.zero);

        GameObject snakeBody = Instantiate(m_prefabSnakeBody, m_oldPosition, Quaternion.identity);
        snakeBody.transform.SetParent(m_SnakeBodys.transform, false);
        // 改变蛇身的图像
        int index = m_listSnakeBodys.Count % 2;
        snakeBody.GetComponent<Image>().sprite = m_SnakeBodySprites[index];

        m_listSnakeBodys.Add(snakeBody.transform);

        // 长度加1
        ++GameManager.Instance.m_iLength;
        GameManager.Instance.m_textLength.text = GameManager.Instance.m_iLength.ToString();
    }

    private void CrossWall(Collider2D other) {
        if (other.name == "TopWall") {
            transform.localPosition = new Vector3(transform.localPosition.x, m_transWalls[1].localPosition.y, transform.localPosition.z);
        }
        else if (other.name == "BottomWall") {
            transform.localPosition = new Vector3(transform.localPosition.x, m_transWalls[0].localPosition.y, transform.localPosition.z);
        }
        else if (other.name == "LeftWall") {
            transform.localPosition = new Vector3(m_transWalls[3].localPosition.x, transform.localPosition.y, transform.localPosition.z);
        }
        else if (other.name == "RightWall") {
            transform.localPosition = new Vector3(m_transWalls[2].localPosition.x, transform.localPosition.y, transform.localPosition.z);
        }
    }

    private void Die() {
        m_bIsDie = true;
        // 播放死亡音效
        AudioSource.PlayClipAtPoint(m_clipDie, new Vector3(0, 0, -10));
        // 播放死亡特效
        Instantiate(m_prefabDie);
        // 存储本局信息
        StoreInfo();
        // 等待几秒
        StartCoroutine(GameOver(1.5f));
    }

    private void StoreInfo() {
        PlayerPrefs.SetInt("lastScore", GameManager.Instance.m_iScore);
        PlayerPrefs.SetInt("lastLength", GameManager.Instance.m_iLength);
        if (PlayerPrefs.GetInt("bestScore", 0) < GameManager.Instance.m_iScore) {
            PlayerPrefs.SetInt("bestScore", GameManager.Instance.m_iScore);
            PlayerPrefs.SetInt("bestLength", GameManager.Instance.m_iLength);
        }
    }

    private IEnumerator GameOver(float t) {
        yield return new WaitForSeconds(t);
        SceneManager.LoadScene("Main");
    }
}