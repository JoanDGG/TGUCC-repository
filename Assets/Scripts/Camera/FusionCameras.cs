using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionCameras : MonoBehaviour
{
    public float smooth = 2.0f;

    private Camera camera1;

    private bool faded = false;
    private float x = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        camera1 = gameObject.transform.parent.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(faded)
        {
            print("No overlaps");
            //while (x > -0.5f)
            //{
            //    x -= 0.005f;
            //}
            x = Mathf.Lerp(-0.5f, 0.0f, smooth * Time.deltaTime);
        }
        else
        {
            print("Overlaps");
            //while (x < 0.0f)
            //{
            //    x += 0.005f;
            //}
            x = Mathf.Lerp(0.0f, -0.5f, smooth * Time.deltaTime);
        }
        camera1.rect = new Rect(x, 0.0f, 1.0f, 1.0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Kid"))
        {
            //Change camera1 rect
            faded = false;
            print("Enter");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "CameraView2")
        {
            //Change camera1 rect
            faded = true;
            print("Exit");
        }
    }

    private IEnumerator FadeCameras(float x)
    {
        if(x == 0.0f)
        {
            while(x > -0.5f)
            {
                x -= 0.05f;
                yield return new WaitForSeconds(0.1f);
            }
        }
        else if (x == -0.5f)
        {
            while (x < 0.0f)
            {
                x += 0.05f;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
