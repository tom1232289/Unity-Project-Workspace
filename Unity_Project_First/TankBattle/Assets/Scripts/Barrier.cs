using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour {
    public AudioClip m_AudioClipHit;

    public void PlayAudio() {
        AudioSource.PlayClipAtPoint(m_AudioClipHit, transform.position);
    }
}
