using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    private Transform m_startPoint; // 针发射的开始点
    private Transform m_spawnPoint; // 实例化针的位置
    private Pin m_currPin;  // 当前的针
    private bool m_bIsGameOver = false;
    private int m_iScore;
    private Text m_textScore;
    private Camera m_mainCamera;

    public GameObject m_prefabPin;  // 针的prefab
    public float m_fSpeed = 5;  // 失败时背景渐变和相机放大的速度

    // Start is called before the first frame update
    private void Start() {
        m_startPoint = GameObject.Find("StartPoint").transform;
        m_spawnPoint = GameObject.Find("SpawnPoint").transform;
        m_textScore = GameObject.Find("Score").GetComponent<Text>();
        m_mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        SpawnPin();
    }

    // Update is called once per frame
    private void Update() {
        if (m_bIsGameOver == true)
            return;

        if (Input.GetMouseButtonDown(0)) {
            m_currPin.StartFly();
            SpawnPin();
            ++m_iScore;
            m_textScore.text = m_iScore.ToString();
        }
    }

    // 实例化针 并 获取到实例化的针对象
    private void SpawnPin() {
        m_currPin = GameObject.Instantiate(m_prefabPin, m_spawnPoint.position, m_prefabPin.transform.rotation).GetComponent<Pin>();
    }

    public void GameOver() {
        // 如果游戏已经结束，则其他针调用GameOver函数时则直接返回
        if (m_bIsGameOver == true)
            return;
        // 让小球不再旋转
        GameObject.Find("Circle").GetComponent<RotateSelf>().enabled = false;
        // 开启结束动画的协程，
        StartCoroutine(GameOverAnimation());
        m_bIsGameOver = true;
    }

    IEnumerator GameOverAnimation() {
        while (true) {
            m_mainCamera.backgroundColor = Color.Lerp(m_mainCamera.backgroundColor, Color.red, m_fSpeed * Time.deltaTime);  // 渐变背景颜色
            m_mainCamera.orthographicSize = Mathf.Lerp(m_mainCamera.orthographicSize, 4f, m_fSpeed * Time.deltaTime);   // 相机放大
            if (Mathf.Abs(m_mainCamera.orthographicSize - 4f) < 0.01f) {
                break;
            }
            yield return 0;
        }
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}