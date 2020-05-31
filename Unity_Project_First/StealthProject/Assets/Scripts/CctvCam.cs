using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CctvCam : MonoBehaviour
{
    private void OnTriggerStay(Collider other) {
        // 主角进入摄像机监控范围
        if (other.tag == "Player") {
            // 警报
            GameManager.Instance.Alerm(other.transform.position);
        }
    }
}
