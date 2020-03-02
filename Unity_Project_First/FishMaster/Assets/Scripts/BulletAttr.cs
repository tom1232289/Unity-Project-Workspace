using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BulletAttr : MonoBehaviour
{
    // 公有引用
    public GameObject m_prefabWeb;

    // 公有变量
    public int m_iSpeed = 9;
    public int m_iDamage = 1;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Border") {
            Destroy(gameObject);
        }
        else if (other.tag == "Fish") {
            GameObject goWeb = Instantiate(m_prefabWeb);
            goWeb.transform.SetParent(transform.parent,false);
            goWeb.transform.position = transform.position;
            goWeb.GetComponent<WebAttr>().m_iDamage = m_iDamage;
            Destroy(gameObject);
        }
    }
}
