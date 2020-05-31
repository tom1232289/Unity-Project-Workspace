using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCard : MonoBehaviour {
    public AudioClip m_acKeyCardPickUp;     // 捡起钥匙的音效

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            // 玩家获取钥匙
            other.GetComponent<Player>().m_bHasKey = true;
            // 播放音效
            AudioSource.PlayClipAtPoint(m_acKeyCardPickUp, Camera.main.transform.position);
            // 销毁钥匙 
            Destroy(gameObject);
        }
    }
}
