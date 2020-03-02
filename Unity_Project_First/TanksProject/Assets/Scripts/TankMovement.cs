using UnityEngine;

public class TankMovement : MonoBehaviour {
    public float speed = 10;
    public float angularSpeed = 10;
    public int number = 1;
    public AudioClip idleAudio;
    public AudioClip drivingAudio;

    private Rigidbody rigidbody;
    private AudioSource audio; 

    // Start is called before the first frame update
    private void Start() {
        rigidbody = GetComponent<Rigidbody>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void FixedUpdate() {
        float v = Input.GetAxis("VerticalPlayer" + number);
        rigidbody.velocity = transform.forward * v * speed;
        float h = Input.GetAxis("HorizontalPlayer" + number);
        rigidbody.angularVelocity = transform.up * h * angularSpeed;

        if (Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f) {
            audio.clip = drivingAudio;
            if(audio.isPlaying == false)
                audio.Play();
        }
        else {
            audio.clip = idleAudio;
            if (audio.isPlaying == false)
                audio.Play();
        }
    }
}