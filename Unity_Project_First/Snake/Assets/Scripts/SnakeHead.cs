using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SnakeHead : MonoBehaviour {

    // 公有变量
    public int m_iStep = 30; // 每次移动多少个像素
    public float m_fVelocity = 0.35f; // 移动的速度
    public GameObject m_prefabSnakeBody;    // 蛇身的预制体
    public Sprite[] m_spriteSnakeBody;      // 蛇身的图像数组
    public List<Transform> m_listSnakeBody; // 蛇身位置的数组
    public Transform m_transCanvas;         // 画布的位置
    public Transform[] m_transBorders;      // 游戏边界的位置（上下左右）
    public GameObject m_prefabDie;          // 死亡特效
    public AudioClip m_clipDie;
    public AudioClip m_clipEat;

    // 私有变量
    private int m_iX = 0; // X轴的移动值
    private int m_iY = 0; // Y轴的移动值
    private Vector3 m_OldPos; // 蛇头当前的位置
    private bool m_bIsDie;

    private enum MoveDirection { UP, LEFT, DOWN, RIGHT };
    private MoveDirection m_iMoveDirection; // 移动方向

    private void Awake() {
        // 加载皮肤：蛇头
        int iSkin = PlayerPrefs.GetInt("skin", 0);
        string sSkinHead;
        string[] sSkinBody = new string[2];
        if (iSkin == 0) {
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
        m_spriteSnakeBody[0] = Resources.Load<Sprite>(sSkinBody[0]);
        m_spriteSnakeBody[1] = Resources.Load<Sprite>(sSkinBody[1]);
    }

    private void Start() {
        m_iY = m_iStep;
        InvokeRepeating("Move", 0, m_fVelocity);
    }

    private void Update() {
        // 暂停状态下直接返回
        if (GameManager.Instance.m_bIsPause)
            return;
        // 死亡状态下停止移动
        if (m_bIsDie) {
            CancelInvoke();
            return;
        }

        // 按下空格加速
        if (Input.GetKeyDown(KeyCode.Space)) {
            CancelInvoke();
            InvokeRepeating("Move", 0, m_fVelocity - 0.2f);
        }
        // 抬起空格恢复原来速度
        if (Input.GetKeyUp(KeyCode.Space)) {
            CancelInvoke();
            InvokeRepeating("Move", 0, m_fVelocity);
        }

        // 移动按键
        if (Input.GetKey(KeyCode.W)) {
            // 不允许掉头
            if (m_iMoveDirection == MoveDirection.DOWN)
                return;
            // 改变蛇头的方向
            transform.localRotation = Quaternion.Euler(0, 0, 0);
            // 赋值移动的距离
            m_iX = 0;
            m_iY = m_iStep;
            // 更新移动方向的变量
            m_iMoveDirection = MoveDirection.UP;
        }
        if (Input.GetKey(KeyCode.A)) {
            if (m_iMoveDirection == MoveDirection.RIGHT)
                return;
            transform.localRotation = Quaternion.Euler(0, 0, 90);
            m_iX = -m_iStep;
            m_iY = 0;
            m_iMoveDirection = MoveDirection.LEFT;
        }
        if (Input.GetKey(KeyCode.S)) {
            if (m_iMoveDirection == MoveDirection.UP)
                return;
            transform.localRotation = Quaternion.Euler(0, 0, 180);
            m_iX = 0;
            m_iY = -m_iStep;
            m_iMoveDirection = MoveDirection.DOWN;
        }
        if (Input.GetKey(KeyCode.D)) {
            if (m_iMoveDirection == MoveDirection.LEFT)
                return;
            transform.localRotation = Quaternion.Euler(0, 0, -90);
            m_iX = m_iStep;
            m_iY = 0;
            m_iMoveDirection = MoveDirection.RIGHT;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // 如果蛇头碰到了食物
        if (other.tag == "Food") {
            Destroy(other.gameObject);
            Grow();
            GameManager.Instance.UpdateUI();
            FoodMaker.Instance.MakeFood();
        }
        else if (other.tag == "Reward") {
            Destroy(other.gameObject);
            Grow();
            GameManager.Instance.UpdateUI(Random.Range(5, 10) * 10);
        }
        else if (other.tag == "Body") {
            Die();
        }
        else if (other.tag == "Wall") {
            if (GameManager.Instance.m_bHasBorder) {
                Die();
            }
            else {
                CrossWall(other);
            }
        }
    }

    private void Move() {
        // 移动蛇头
        m_OldPos = transform.localPosition;
        transform.localPosition = new Vector3(m_OldPos.x + m_iX, m_OldPos.y + m_iY, m_OldPos.z);
        // 移动蛇身（将蛇身移动到上一个蛇身的位置）
        if (m_listSnakeBody.Count > 0) {
            for (int i = m_listSnakeBody.Count - 1; i > 0; --i) {
                m_listSnakeBody[i].localPosition = m_listSnakeBody[i - 1].localPosition;
            }

            m_listSnakeBody[0].localPosition = m_OldPos;
        }
    }

    private void Grow() {
        // 播放吃东西音效
        AudioSource.PlayClipAtPoint(m_clipEat, Vector3.zero);
        // 生成蛇身
        GameObject snakeBody = Instantiate(m_prefabSnakeBody, new Vector3(2000, 2000, 0), Quaternion.identity);
        snakeBody.transform.SetParent(m_transCanvas, false);
        // 改变蛇身的图像
        int iIndex = m_listSnakeBody.Count % 2;
        snakeBody.GetComponent<Image>().sprite = m_spriteSnakeBody[iIndex];
        // 将新生成的蛇身的位置放到蛇身位置的数组里
        m_listSnakeBody.Add(snakeBody.transform);
    }

    private void CrossWall(Collider2D other) {
        if (other.name == "Top") {
            transform.localPosition = new Vector3(transform.localPosition.x, m_transBorders[1].localPosition.y + 30, transform.localPosition.z);
        }
        else if (other.name == "Bottom") {
            transform.localPosition = new Vector3(transform.localPosition.x, m_transBorders[0].localPosition.y - 30, transform.localPosition.z);
        }
        else if (other.name == "Left") {
            transform.localPosition = new Vector3(m_transBorders[3].localPosition.x - 30 - 3.5f, transform.localPosition.y, transform.localPosition.z);
        }
        else if (other.name == "Right") {
            transform.localPosition = new Vector3(m_transBorders[2].localPosition.x + 30, transform.localPosition.y, transform.localPosition.z);
        }
    }

    private void Die() {
        // 播放死亡音效
        AudioSource.PlayClipAtPoint(m_clipDie, new Vector3(0,0,-10));

        m_bIsDie = true;
        Instantiate(m_prefabDie);
        // 存储本局信息
        PlayerPrefs.SetInt("lastScore", GameManager.Instance.m_iScore);
        PlayerPrefs.SetInt("lastLength", GameManager.Instance.m_iLength);
        if (PlayerPrefs.GetInt("bestScore", 0) < GameManager.Instance.m_iScore) {
            PlayerPrefs.SetInt("bestScore", GameManager.Instance.m_iScore);
            PlayerPrefs.SetInt("bestLength", GameManager.Instance.m_iLength);
        }

        StartCoroutine(GameOver(1.5f));
    }

    private IEnumerator GameOver(float t) {
        yield return new WaitForSeconds(t);
        SceneManager.LoadScene("Main");
    }
}