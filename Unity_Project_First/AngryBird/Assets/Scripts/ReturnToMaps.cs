using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToMaps : MonoBehaviour
{
    // 私有引用
    private GameObject m_Maps;

    private void Awake() {
        m_Maps = GameObject.Find("Canvas").transform.Find("Maps").gameObject;
    }
    // panel的回退按钮点击事件
    public void OnBtnReturnClicked() {
        transform.parent.gameObject.SetActive(false);
        m_Maps.SetActive(true);
    }
}
