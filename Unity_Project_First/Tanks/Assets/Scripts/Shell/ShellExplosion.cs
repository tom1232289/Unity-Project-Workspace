using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellExplosion : MonoBehaviour {
    public float m_fMaxLiftTime = 2f;
    public float m_fExplosionRadius = 5f;
    public LayerMask m_TankMask;
    public float m_ExplosionForce = 1000f;
    public ParticleSystem m_ExplosionParticles;
    public AudioSource m_ExplosionAudio;
    public float m_fMaxDamage = 100f;

    private void Start() {
        Destroy(gameObject, m_fMaxLiftTime);
    }

    private void OnTriggerEnter(Collider other) {
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_fExplosionRadius, m_TankMask);

        for (int i = 0; i < colliders.Length; ++i) {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
            if(!targetRigidbody)
                continue;

            targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_fExplosionRadius);
            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();
            if(!targetHealth)
                continue;

            float damage = CalculateDamage(targetRigidbody.position);
            targetHealth.TakeDamage(damage);
        }

        m_ExplosionParticles.transform.parent = null;
        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();

        ParticleSystem.MainModule mainModule = m_ExplosionParticles.main;
        Destroy(m_ExplosionParticles.gameObject, mainModule.duration);

        Destroy(gameObject);
    }

    private float CalculateDamage(Vector3 targetPosition) {
        Vector3 explosionToTarget = targetPosition - transform.position;

        float explosionDistance = explosionToTarget.magnitude;

        float relativeDistance = (m_fExplosionRadius - explosionDistance) / m_fExplosionRadius;

        float damage = relativeDistance * m_fMaxDamage;

        damage = Mathf.Max(0f, damage);

        return damage;
    }
}
