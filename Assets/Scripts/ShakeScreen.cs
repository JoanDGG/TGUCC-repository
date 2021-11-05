using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeScreen : MonoBehaviour
{

    // A measure of how quickly the shake effect should evaporate
    public float dampingSpeed = 1.0f;
    public float shakeMagnitude = 0.6f;
    
    private float shakeDuration = 0f;

    new private Camera camera;

    new private Transform transform;
    private Vector3 initialPosition;

    void Start()
    {
        camera = gameObject.GetComponent<Camera>();
        transform = gameObject.GetComponent<Transform>();
        initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeDuration > 0)
        {
            GameObject.Find("Spooky").GetComponent<PlayerMovement>().letControls = false;
            GameObject.Find("Kid").GetComponent<PlayerMovement>().letControls = false;

            camera.fieldOfView = 30;

            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            GameObject.Find("Spooky").GetComponent<PlayerMovement>().letControls = true;
            GameObject.Find("Kid").GetComponent<PlayerMovement>().letControls = true;

            shakeDuration = 0f;
        }
    }

    public void TriggerShake()
    {
        shakeDuration = 2.0f;
    }
}
