using UnityEngine;

public class FollowTarget : MonoBehaviour {
    private Vector3 offset;
    private Transform player1;
    private Transform player2;
    private Camera camera;

    // Start is called before the first frame update
    private void Start() {
        player1 = GameObject.Find("Tank1").transform;
        player2 = GameObject.Find("Tank2").transform;
        offset = transform.position - (player1.position + player2.position) / 2;
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    private void Update() {
        if (player1 == null || player2 == null)
            return;
             
        transform.position = (player1.position + player2.position) / 2 + offset;
        float distance = Vector3.Distance(player1.position,player2.position);
        float size = distance * 0.98f;
        camera.orthographicSize = size;
    }
}