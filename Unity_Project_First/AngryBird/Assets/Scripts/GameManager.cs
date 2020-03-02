using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    // 单例
    private static GameManager m_instance;

    public static GameManager Instance {
        get { return m_instance; }
    }

    // 公有引用
    public List<Bird> m_listBirds;
    public List<Pig> m_listPigs;
    public GameObject m_win;
    public GameObject m_lose;
    public GameObject[] m_stars;

    // 公有变量
    public const int I_TOTAL_LEVEL = 10;

    // 私有变量
    private Vector3 m_BirdOriginPos = new Vector3();
    private bool m_bIsWin;

    private void Awake() {
        m_instance = this;
        if (m_listBirds.Count > 0) {
            m_BirdOriginPos = m_listBirds[0].transform.position;
        }
    }

    private void Start() {
        InitBirdStatus();
    }

    private void InitBirdStatus() {
        for (int i = 0; i < m_listBirds.Count; ++i) {
            if (i == 0) {
                m_listBirds[i].transform.position = m_BirdOriginPos;
                m_listBirds[i].enabled = true;
                m_listBirds[i].m_sp.enabled = true;
                m_listBirds[i].GetComponent<CircleCollider2D>().enabled = true;
                m_listBirds[i].GetComponent<Rigidbody2D>().gravityScale = 1;
            }
            else {
                m_listBirds[i].enabled = false;
                m_listBirds[i].m_sp.enabled = false;
                m_listBirds[i].GetComponent<CircleCollider2D>().enabled = false;
                m_listBirds[i].GetComponent<Rigidbody2D>().gravityScale = 0;
            }
        }
    }

    public void NextBird() {
        // 场景上还有猪
        if (m_listPigs.Count > 0) {
            // 场景上还有小鸟
            if (m_listBirds.Count > 0) {
                // 下一只小鸟 上!
                InitBirdStatus();
            }
            // 场景上没有小鸟了
            else {
                // 失败
                m_lose.SetActive(true);
            }
        }
        // 胜利
        else {
            m_bIsWin = true;
            m_win.SetActive(true);
        }
    }

    public void ShowStars() {
        StartCoroutine("ShowStar");
    }

    private IEnumerator ShowStar() {
        for (int i = 0; i < m_listBirds.Count + 1; ++i) {
            if (i >= m_stars.Length)
                break;

            yield return new WaitForSeconds(0.2f);
            m_stars[i].SetActive(true);
        }
    }

    public void OnBtnRetryClicked() {
        Time.timeScale = 1;     // 暂停界面时点击此按钮
        SaveData();
        SceneManager.LoadScene("Game");
    }

    public void OnBtnHomeClicked() {
        Time.timeScale = 1;
        SaveData();
        SceneManager.LoadScene("Level");
    }

    // 黑色小鸟专用
    public void WaitNextBird(float fSecond) {
        Invoke("NextBird", fSecond);
    }

    private void SaveData() {
        if (m_bIsWin) {
            // 存储第几关第几个星星
            string sNowLevel = PlayerPrefs.GetString("nowLevel");
            if (m_listBirds.Count + 1 > PlayerPrefs.GetInt(sNowLevel, 0)) {
                PlayerPrefs.SetInt(sNowLevel, m_listBirds.Count + 1);
            }
            // 存储获得的总星星个数
            int iTotalStarNum = 0;
            for (int i = 0; i < I_TOTAL_LEVEL; ++i) {
                iTotalStarNum += PlayerPrefs.GetInt("level_" + i, 0);
            }
            PlayerPrefs.SetInt("totalStarNum",iTotalStarNum);
        }
    }
}