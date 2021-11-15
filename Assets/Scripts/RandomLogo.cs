using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLogo : MonoBehaviour
{
    public float force = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(Vector3.up * force, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y < GameObject.Find("Camera1").
            GetComponent<SmoothFollow>().yLimits.x)
        {
            Destroy(gameObject);
        }
    }
}
