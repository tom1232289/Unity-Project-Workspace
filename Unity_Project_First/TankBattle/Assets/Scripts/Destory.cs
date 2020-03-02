using UnityEngine;

public class Destory : MonoBehaviour {
    public float m_fDuringTime = 0.167f;

    // Start is called before the first frame update
    private void Start() {
        Destroy(gameObject, m_fDuringTime);
    }

    // Update is called once per frame
    private void Update() {
    }
}