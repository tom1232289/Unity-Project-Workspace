using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    public float m_fDampTime = 0.2f;
    /*[HideInInspector]*/ public Transform[] m_Targets;
    public float m_fScreenEdgeBuffer = 4f;
    public float m_fMinSize = 6.5f;

    private Camera m_camera;
    private Vector3 m_DesiredPosition;
    private Vector3 m_MoveVelocity;
    private float m_fZoomSpeed;

    private void Awake() {
        m_camera = GetComponentInChildren<Camera>();
    }

    private void FixedUpdate() {
        Move();
        Zoom();
    }

    private void Move() {
        FindAveragePosition();

        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_fDampTime);
    }

    private void FindAveragePosition() {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        for (int i = 0; i < m_Targets.Length; ++i) {
            if (!m_Targets[i].gameObject.activeSelf)
                continue;

            averagePos += m_Targets[i].position;
            ++numTargets;
        }

        if (numTargets > 0)
            averagePos /= numTargets;

        averagePos.y = transform.position.y;

        m_DesiredPosition = averagePos;
    }

    private void Zoom() {
        float requiredSize = FindRequiredSize();
        m_camera.orthographicSize = Mathf.SmoothDamp(m_camera.orthographicSize, requiredSize, ref m_fZoomSpeed, m_fDampTime);
    }

    private float FindRequiredSize() {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);

        float size = 0f;

        for (int i = 0; i < m_Targets.Length; ++i) {
            if (!m_Targets[i].gameObject.activeSelf)
                continue;

            Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].position);
            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / m_camera.aspect);
        }

        size += m_fScreenEdgeBuffer;
        size = Mathf.Max(size, m_fMinSize);

        return size;
    }

    public void SetStartPositionAndSize() {
        FindAveragePosition();

        transform.position = m_DesiredPosition;

        m_camera.orthographicSize = FindRequiredSize();
    }
}
