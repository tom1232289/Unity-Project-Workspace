using UnityEngine;

public class Destory : MonoBehaviour {
    public float time = 1.5f;

    // Start is called before the first frame update
    private void Start() {
        GameObject.Destroy(this.gameObject, time);
    }

    // Update is called once per frame
    private void Update() {
    }
}