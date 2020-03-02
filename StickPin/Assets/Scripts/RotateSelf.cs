using UnityEngine;

public class RotateSelf : MonoBehaviour {
    public float m_fSpeed = 90;

    public enum RotateDirection {
        Clockwise = -1,
        Counterclockwise = 1
    }

    public RotateDirection m_RotateDirection = RotateDirection.Clockwise;

    // Start is called before the first frame update
    private void Start() {
    }

    // Update is called once per frame
    private void Update() {
        transform.Rotate(new Vector3(0, 0, m_fSpeed * Time.deltaTime * (int)m_RotateDirection));
    }
}