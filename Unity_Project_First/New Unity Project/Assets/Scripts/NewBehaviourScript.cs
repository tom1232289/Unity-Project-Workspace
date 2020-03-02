using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log(transform.Find("GameObject (1)/GameObject2"));
        //         print(transform.Find("GameObject (1)/GameObject2"));

        //         Transform[] children = transform.GetComponentsInChildren<Transform>();
        //         for (int i = 0; i < children.Length; ++i)
        //         {
        //             if (children[i] != transform)
        //             {
        //                 Destroy(children[i].gameObject);
        //             }
        //         }
        //         foreach (Transform t in children) {
        //             if (t != transform) {
        //                 Destroy(t.gameObject);
        //             }
        //         }
    }

    // Update is called once per frame
    private void Update()
    {
        //print(2);
    }
}