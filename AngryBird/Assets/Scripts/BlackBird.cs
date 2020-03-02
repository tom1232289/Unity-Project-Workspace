using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlackBird : Bird
{
    // 私有变量
    private List<Enemy> m_Enemys = new List<Enemy>();

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy") {
            m_Enemys.Add(other.transform.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Enemy") {
            m_Enemys.Remove(other.transform.GetComponent<Enemy>());
        }
    }

    public override void ReleaseSkill() {
        base.ReleaseSkill();
        if (m_Enemys.Count > 0) {
            for (int i = 0; i < m_Enemys.Count; ++i) {
                m_Enemys[i].Die();
            }
        }

        base.Die();

        GameManager_Game.Instance.WaitNextBird();
    }
}
