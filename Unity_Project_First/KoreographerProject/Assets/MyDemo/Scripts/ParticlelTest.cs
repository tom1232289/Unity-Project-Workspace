using System.Collections;
using System.Collections.Generic;
using SonicBloom.Koreo;
using UnityEngine;

public class ParticleTest : MonoBehaviour
{
    // 公有变量
    public string m_sEventID;
    public float m_fParticlePerBeat = 100;

    // 私有引用
    private ParticleSystem m_ps;

    private void Awake() {
        m_ps = GetComponent<ParticleSystem>();
    }

    private void Start() {
        Koreographer.Instance.RegisterForEvents(m_sEventID, CreateParticle);
    }

    private void CreateParticle(KoreographyEvent koreographyEvent) {
        int iParticleCount = (int)(Koreographer.GetBeatTimeDelta() * m_fParticlePerBeat);
        m_ps.Emit(iParticleCount);
    }
}
