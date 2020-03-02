using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBird : Bird {
    public List<Pig> m_listBlocks = new List<Pig>();

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy") {
            m_listBlocks.Add(other.transform.GetComponent<Pig>());
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Enemy") {
            m_listBlocks.Remove(other.transform.GetComponent<Pig>());
        }
    }

    public override void ReleaseSkill() {
        base.ReleaseSkill();
        if (m_listBlocks.Count > 0 && m_listBlocks != null) {
            for (int i = 0; i < m_listBlocks.Count; ++i) {
                m_listBlocks[i].Die();
            }
        }

        base.Die();
        GameManager.Instance.WaitNextBird(3);
    }
}