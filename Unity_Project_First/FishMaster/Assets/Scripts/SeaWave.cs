using UnityEngine;

public class SeaWave : MonoBehaviour {

    // 公有变量
    public float m_fSpeed = 5f;

    // 私有变量
    private Vector3 m_posDest;  // 目标位置

    private void Start() {
        m_posDest = transform.position;
        m_posDest.x *= -1;
        AudioManager.Instance.PlayAudio(AudioManager.Instance.m_acSeaWave);
    }

    private void Update() {
        transform.position = Vector3.MoveTowards(transform.position, m_posDest, m_fSpeed * Time.deltaTime);
    }
}