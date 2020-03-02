using UnityEngine;

public class TankAttack : MonoBehaviour {
    private Transform firePosition;
    public GameObject shellPrefab;
    public KeyCode fireKey = KeyCode.Space;
    public float shellSpeed = 15;
    public AudioClip shotClip;
    public AudioSource shotAudio;

    // Start is called before the first frame update
    private void Start() {
        firePosition = transform.Find("firePosition");
    }

    // Update is called once per frame
    private void Update() {
        if (Input.GetKeyDown(fireKey)) {
            //AudioSource.PlayClipAtPoint(shotClip,transform.position);
            shotAudio.clip = shotClip;
            shotAudio.Play();
            GameObject go = GameObject.Instantiate(shellPrefab, firePosition.position, firePosition.rotation);
            go.GetComponent<Rigidbody>().velocity = transform.forward * shellSpeed;
        }
    }
}