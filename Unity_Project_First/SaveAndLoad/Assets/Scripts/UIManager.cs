using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    // 单例
    private static UIManager m_instance;

    public static UIManager Instance {
        get { return m_instance; }
    }

    // 公有引用
    public Text m_textShoot;    // 射击数的text
    public Text m_textScore;    // 得分的text
    public Text m_textSaveMsg;  // 保存(加载)成功的text

    // 私有变量
    [HideInInspector]
    public int m_iShoot;       // 射击数

    [HideInInspector]
    public int m_iScore;       // 得分

    private void Awake() {
        m_instance = this;
    }

    private void Start() {
        m_textShoot.text = 0.ToString();
        m_textScore.text = 0.ToString();
    }

    private void Update() {
        m_textShoot.text = m_iShoot.ToString();
        m_textScore.text = m_iScore.ToString();
    }

    public void AddShootNum() {
        ++m_iShoot;
    }

    public void AddScoreNum() {
        ++m_iScore;
    }

    // 显示提示信息
    public void ShowMessage(string sMsg) {
        StartCoroutine("showMessage", sMsg);
    }

    private IEnumerator showMessage(string sMsg) {
        m_textSaveMsg.text = sMsg;
        m_textSaveMsg.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        m_textSaveMsg.gameObject.SetActive(false);
    }
}