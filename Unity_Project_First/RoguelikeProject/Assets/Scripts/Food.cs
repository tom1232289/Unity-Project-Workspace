using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    // 公有变量 
    public int m_iFoodScore = 10;

    private void AddFood() {
        GameManager.Instance.AddFood(m_iFoodScore);
    }

    private void Die() {
        Destroy(gameObject);
    }
}
