using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAudio : MonoBehaviour
{
    // 公有引用
    public AudioClip m_HitClip;

    private void Die() {
        // 摧毁墙的音效
        AudioSource.PlayClipAtPoint(m_HitClip,transform.position);
        // 销毁自身
        Destroy(gameObject);
    }
}
