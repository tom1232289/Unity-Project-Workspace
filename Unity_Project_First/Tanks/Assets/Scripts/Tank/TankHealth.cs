using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour {
    public GameObject m_ExplosionPrefab;
    public float m_fStartingHealth = 100f;
    public Slider m_Slider;
    public Image m_FillImage;
    public Color m_ZeroHealthColor = Color.red;
    public Color m_FullHealthColor = Color.green;

    private ParticleSystem m_ExplosionParticles;
    private AudioSource m_ExplosionAudio;
    private float m_fCurrentHealth;
    private bool m_bDead = false;

    private void Awake() {
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();
        m_ExplosionParticles.gameObject.SetActive(false);
    }

    private void OnEnable() {
        m_fCurrentHealth = m_fStartingHealth;
        m_bDead = false;

        SetHealthUI();
    }

    private void SetHealthUI() {
        m_Slider.value = m_fCurrentHealth;

        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_fCurrentHealth / m_fStartingHealth);
    }

    public void TakeDamage(float amount) {
        m_fCurrentHealth -= amount;

        SetHealthUI();

        if (m_fCurrentHealth <= 0f && !m_bDead) {
            OnDeath();
        }
    }

    private void OnDeath() {
        m_bDead = true;

        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);
        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();
        gameObject.SetActive(false);
    }
}
