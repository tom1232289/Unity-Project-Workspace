using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinHead : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider) {
        // 如果碰撞到的是针，则调用GameManager组件的GameOver方法结束游戏
        if (collider.tag == "PinHead") {
            GameObject.Find("GameManager").GetComponent<GameManager>().GameOver();
        }
    }
}
