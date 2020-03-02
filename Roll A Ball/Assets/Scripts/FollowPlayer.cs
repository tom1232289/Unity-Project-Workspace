using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
    public Transform m_playerTransform;

    private Vector3 m_vectorOffset;
    // Start is called before the first frame update
    void Start() {
        m_vectorOffset = transform.localPosition - m_playerTransform.localPosition;
    }

    // Update is called once per frame
    void Update() {
        transform.localPosition = m_playerTransform.localPosition + m_vectorOffset;
    }
}
