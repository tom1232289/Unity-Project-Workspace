using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour {
    public float m_fMinLaunchForce = 15f;
    public Slider m_AimSlider;
    public int m_iPlayerNumber = 1;
    public float m_fMaxChargeTime = 0.75f;
    public AudioSource m_ShootingAudio;
    public AudioClip m_ChargingClip;
    public Rigidbody m_Shell;
    public Transform m_FireTransform;
    public AudioClip m_FireClip;

    private float m_fCurrentLaunchForce;
    private string m_sFireButton;
    private float m_fChargeSpeed;
    private bool m_bFired;

    private void OnEnable() {
        m_fCurrentLaunchForce = m_fMinLaunchForce;
        m_AimSlider.value = m_fMinLaunchForce;
    }

    // Start is called before the first frame update
    void Start() {
        m_sFireButton = "Fire" + m_iPlayerNumber;

        m_fChargeSpeed = (m_fMinLaunchForce - m_fMinLaunchForce) / m_fMaxChargeTime;
    }

    // Update is called once per frame
    void Update() {
        m_AimSlider.value = m_fMinLaunchForce;

        if (m_fCurrentLaunchForce >= m_fMaxChargeTime && !m_bFired) {
            Fire();
        }
        else if (Input.GetButtonDown(m_sFireButton)) {
            m_bFired = false;
            m_fCurrentLaunchForce = m_fMinLaunchForce;
            m_ShootingAudio.clip = m_ChargingClip;
            m_ShootingAudio.Play();
        }
        else if (Input.GetButton(m_sFireButton) && !m_bFired) {
            m_fCurrentLaunchForce += m_fChargeSpeed * Time.deltaTime;
            m_AimSlider.value = m_fCurrentLaunchForce;
        }
        else if (Input.GetButtonUp(m_sFireButton) && !m_bFired) {
            Fire();
        }
    }

    private void Fire() {
        m_bFired = true;
        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation);
        shellInstance.velocity = m_fCurrentLaunchForce * m_FireTransform.forward;

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        m_fCurrentLaunchForce = m_fMinLaunchForce;
    }
}
