using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : MonoBehaviour
{
    // 显示星星
    public void Show() {
        GameManager.Instance.ShowStars();
    }
}
