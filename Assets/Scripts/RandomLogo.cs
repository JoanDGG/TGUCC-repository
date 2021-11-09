using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLogo : MonoBehaviour
{
    public float force = 25.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y < GameObject.Find("Main Camera").
            GetComponent<SmoothFollow>().yLimits.x)
        {
            Destroy(gameObject);
        }
    }

    public void Aparecer()
    {
        //transform.position = Random.insideUnitCircle * 10;

        Vector3 direction = new Vector3(gameObject.transform.position.x 
            - GameObject.Find("Main Camera").
            transform.position.x, 1f, 0f);
        print(direction);

        GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
    }
}
