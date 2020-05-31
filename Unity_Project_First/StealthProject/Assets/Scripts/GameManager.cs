using System.Collections;
using System.Collections.Generic;
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
    public AudioSource m_asNormol;  // 播放正常状态下bgm的as
    public AudioSource m_asAlerm;   // 播放警报状态下bgm的as

    // 公有变量
    [HideInInspector]
    public Vector3 m_MonitoredPlayerPos = Vector3.zero;      // 监控到的主角的位置

    public float m_fChangeBgmSpeed = 1;

    // 私有引用
    private GameObject m_TextWin;

    // 私有变量
    private List<AudioSource> m_asSirens = new List<AudioSource>();     // 警报灯的AudioSource

    private void Awake() {
        m_instance = this;
        GameObject[] goSirens = GameObject.FindGameObjectsWithTag("Siren");
        foreach (var siren in goSirens) {
            m_asSirens.Add(siren.GetComponent<AudioSource>());
        }

        m_TextWin = GameObject.Find("Canvas").transform.Find("textWin").gameObject;
    }

    private void Update() {
        // 警报打开
        if (AlermLight.Instance.m_bIsOn) {
            // 播放警报声
            PlaySiren();
            // 切换bgm
            m_asNormol.volume = Mathf.Lerp(m_asNormol.volume, 0, m_fChangeBgmSpeed * Time.deltaTime);
            m_asAlerm.volume = Mathf.Lerp(m_asAlerm.volume, 0.5f, m_fChangeBgmSpeed * Time.deltaTime);
        }
        // 警报关闭
        else {
            // 关闭警报声
            StopSiren();
            // 切换bgm
            m_asAlerm.volume = Mathf.Lerp(m_asAlerm.volume, 0, m_fChangeBgmSpeed * Time.deltaTime);
            m_asNormol.volume = Mathf.Lerp(m_asNormol.volume, 1, m_fChangeBgmSpeed * Time.deltaTime);
        }
    }

    private void PlaySiren() {
        foreach (var audioSource in m_asSirens) {
            // 不播放警报时 才 播放警报
            if (!audioSource.isPlaying) {
                audioSource.Play();
            }
        }
    }

    private void StopSiren() {
        foreach (var audioSource in m_asSirens) {
            // 优化：播放警报时 才 停止警报声
            if (audioSource.isPlaying) {
                audioSource.Stop();
            }
        }
    }

    // 警报
    public void Alerm(Vector3 position) {
        // 打开警报
        AlermLight.Instance.m_bIsOn = true;
        // 记录主角当前位置
        GameManager.Instance.m_MonitoredPlayerPos = position;
    }

    // 胜利
    public void Win() {
        m_TextWin.SetActive(true);
        StartCoroutine("ReStart");
    }

    private IEnumerator ReStart() {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Game");
    }
}