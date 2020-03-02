using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ef_PlayEffect : MonoBehaviour
{
    // 公有引用
    public GameObject[] m_Effects;  // 各种特效

    public void PlayEffect() {
        foreach (var effect in m_Effects) {
            Instantiate(effect);
        }
    }
}
