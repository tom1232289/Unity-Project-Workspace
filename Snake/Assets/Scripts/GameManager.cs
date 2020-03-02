using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    // 单例
    private static GameManager m_instance;

    public static GameManager Instance {
        get { return m_instance; }
    }

    // 公有引用
    public GameObject m_prefabFood;     // 食物的预制体
    public Sprite[] m_FoodSprites;      // 食物的图片精灵数组
    public GameObject m_Canvas;
    public Text m_textScore;            // 分数
    public Text m_textLength;           // 长度
    public Text m_textStage;            // 阶段
    public Image m_imgPause;
    public Sprite[] m_spritePauses = new Sprite[2];
    public GameObject m_prefabReward;
    public GameObject[] m_Walls = new GameObject[4];
    public Image m_imgBg;

    // 公有变量
    public int m_iScore;                // 分数
    public int m_iLength;               // 长度
    public int m_iSnakeSkin;            // 蛇的皮肤
    public int m_iBorderMode;           // 边界模式（0：有边界，1：无边界）
    public bool m_bIsPause;

    // 私有变量
    private int m_iLeftLimit = 11;      // 最左边能走的次数
    private int m_iRightLimit = 20;
    private int m_iTopLimit = 11;
    private int m_iBottomLimit = 11;
    private GameObject m_foods;

    private void Awake() {
        m_instance = this;
        m_Canvas = GameObject.Find("Canvas");

        LoadUserParam();

        // 没有边界需要隐藏墙
        if (m_iBorderMode == 1) {
            foreach (var wall in m_Walls) {
                wall.GetComponent<Image>().color = Color.clear;
            }
        }
    }

    private void Start() {
        m_foods = new GameObject("Foods");
        m_foods.transform.SetParent(m_Canvas.transform, false);
    }

    private void Update() {
        if (m_iScore >= 100 && m_iScore < 500) {
            m_textStage.text = "阶段2";
            m_imgBg.color = new Color(14 / 255f, 75 / 255f, 41 / 255f);
        }
        else if (m_iScore >= 500 && m_iScore < 1000) {
            m_textStage.text = "阶段3";
            m_imgBg.color = new Color(35 / 255f, 243 / 255f, 40 / 255f);
        }
        else if (m_iScore >= 1000 && m_iScore < 5000) {
            m_textStage.text = "阶段4";
            m_imgBg.color = new Color(211 / 255f, 154 / 255f, 145 / 255f);
        }
        else if (m_iScore >= 5000) {
            m_textStage.text = "无尽模式";
            m_imgBg.color = new Color(145 / 255f, 45 / 255f, 200 / 255f);
        }
    }

    public void FoodMaker() {
        // 创建一个物体的实例
        GameObject food = Instantiate(m_prefabFood);
        // 将实例作为Foods的子物体
        food.transform.SetParent(m_foods.transform, false);

        // 修改食物的图片精灵
        int iRamdom = Random.Range(0, m_FoodSprites.Length);
        food.GetComponent<Image>().sprite = m_FoodSprites[iRamdom];
        // 将实例移动到随机方格位置
        int x = Random.Range(-m_iLeftLimit, m_iRightLimit);
        int y = Random.Range(-m_iBottomLimit, m_iTopLimit);
        food.transform.localPosition = new Vector3(x * 30, y * 30, 0);

        // 随机是否生成奖励
        int iRandom = Random.Range(0, 5);
        if (iRandom == 0) {
            GameObject reward = Instantiate(m_prefabReward);
            reward.transform.SetParent(m_foods.transform,false);
            x = Random.Range(-m_iLeftLimit, m_iRightLimit);
            y = Random.Range(-m_iBottomLimit, m_iTopLimit);
            food.transform.localPosition = new Vector3(x * 30, y * 30, 0);
        }
    }

    private void LoadUserParam() {
        m_iBorderMode = PlayerPrefs.GetInt("mode", 0);
        m_iSnakeSkin = PlayerPrefs.GetInt("skin", 0);
    }

    public void OnBtnHomeClicked() {
        SceneManager.LoadScene("Start");
    }

    public void OnBtnPauseClicked() {
        m_bIsPause = !m_bIsPause;
        if (m_bIsPause) {
            Time.timeScale = 0;
            m_imgPause.sprite = m_spritePauses[1];
        }
        else {
            Time.timeScale = 1;
            m_imgPause.sprite = m_spritePauses[0];
        }
    }
}