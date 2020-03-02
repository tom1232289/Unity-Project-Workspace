using DG.Tweening;
using UnityEngine;

public class GetStart : MonoBehaviour {

    // 公有变量
    public Vector3 m_pos = new Vector3(0, 0, 0);

    private void Start() {
        DOTween.To(() => m_pos, x => m_pos = x, new Vector3(10, 10, 10), 2);
    }
}