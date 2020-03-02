using UnityEngine;

public class PickUp : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(new Vector3(1, 1, 1));
    }
}