using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    // 公有变量
    public float m_fHealth = 100;

    // 私有变量
    private Animator m_anim;
    private List<EnemySight> m_EnemySights = new List<EnemySight>();

    private void Awake() {
        m_anim = GetComponent<Animator>();
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");
        for(int i = 0; i < gos.Length; ++i) { 
            m_EnemySights.Add(gos[i].GetComponent<EnemySight>());
        }
    }

    public void UnderAttack(float fDamage) {
        m_fHealth -= fDamage;
        if (m_fHealth <= 0) {
            Die();
        }
    }

    private void Die() {
        m_anim.SetBool("Dead", true);
        foreach (var enemySight in m_EnemySights) {
            enemySight.m_bPlayerInSight = false;
        }

        StartCoroutine("ReStart");
    }

    private IEnumerator ReStart() {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Game");
    }
}
