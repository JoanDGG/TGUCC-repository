using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeScreen : MonoBehaviour
{

    // A measure of how quickly the shake effect should evaporate
    public float dampingSpeed = 1.0f;
    public float shakeMagnitude = 0.6f;
    public float shakeTime = 2.0f;

    public GameObject spooky;
    public GameObject kid;

    private float shakeDuration = 0f;

    new private Camera camera;

    new private Transform transform;
    private Vector3 initialPosition;

    public GameObject floatingText;
    public Transform BeginTextPosition;
    private GameObject newText;

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
            if(spooky != null)
                spooky.GetComponent<PlayerMovement>().letControls = false;
            else if (kid != null)
                kid.GetComponent<PlayerMovement>().letControls = false;

            camera.fieldOfView = 30;

            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            if (spooky != null)
                spooky.GetComponent<PlayerMovement>().letControls = true;
            else if (kid != null)
                kid.GetComponent<PlayerMovement>().letControls = true;

            shakeDuration = 0f;
        }
    }

    public void TriggerShake()
    {
        shakeDuration = shakeTime;

        newText = Instantiate(floatingText, BeginTextPosition.position,
                                       Quaternion.identity, BeginTextPosition);
        Renderer textRenderer = newText.GetComponent<Renderer>();
        textRenderer.sortingOrder = 6;
        newText.layer = 8;
        newText.GetComponent<TextMesh>().characterSize = 1.1f;
        newText.GetComponent<TextMesh>().alignment = TextAlignment.Center;
        newText.GetComponent<FadeOut>().lifeTime = shakeTime;

        newText.GetComponent<TextMesh>().text = "Master key Found!!!";
        newText.GetComponent<FadeOut>().FadeInText();
    }
}
