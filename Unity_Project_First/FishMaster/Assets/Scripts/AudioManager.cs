using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    // 单例
    private static AudioManager m_instance;

    public static AudioManager Instance {
        get { return m_instance; }
    }

    // 公有引用
    public AudioClip m_acSeaWave;
    public AudioClip m_acGold;
    public AudioClip m_acReward;
    public AudioClip m_acFire;
    public AudioClip m_acChangeGun;
    public AudioClip m_acLvUp;

    // 公有变量
    public bool m_bIsMute; // 是否静音

    // 私有引用
    private AudioSource m_AudioSource;

    private void Awake() {
        m_instance = this;
        m_AudioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        m_bIsMute = Convert.ToBoolean(PlayerPrefs.GetInt("mute", 0));
        SetMute();
    }

    public void PlayAudio(AudioClip ac) {
        if (m_bIsMute) {
            return;
        }
        AudioSource.PlayClipAtPoint(ac, Camera.main.transform.position, 0.05f);
    }

    public void SwitchMuteParam(bool isOn) {
        m_bIsMute = !isOn;
        SetMute();
    }

    private void SetMute() {
        if (m_bIsMute) {
            m_AudioSource.Pause();
        }
        else {
            m_AudioSource.Play();
        }
    }
}