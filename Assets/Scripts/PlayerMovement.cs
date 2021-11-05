using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [System.NonSerialized] public bool letControls = true;
    public float movementSpeed = 5;

    private Vector2 move;
    private float horizontal;
    private float vertical;

    private SpriteRenderer sprRenderer;
    private Rigidbody2D rigidbody2d;
    private Animator animator;

    void Start()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(gameObject.name == "Spooky")
        {
            horizontal = ((Input.GetKey("d")) ? 1 : (Input.GetKey("a")) ? -1 : 0);
            vertical   = ((Input.GetKey("w")) ? 1 : (Input.GetKey("s")) ? -1 : 0);
        }
        else if (gameObject.name == "Kid")
        {
            horizontal = ((Input.GetKey("right")) ? 1 : (Input.GetKey("left")) ? -1 : 0);
            vertical   = ((Input.GetKey("up"))    ? 1 : (Input.GetKey("down")) ? -1 : 0);
        }

        move = new Vector2(horizontal * movementSpeed, vertical * movementSpeed);

        animator.SetFloat("velocity", Mathf.Abs(move.x + move.y));

        if (move.x > 0.1)
        {
            sprRenderer.flipX = false;
        }
        else if (move.x < -0.1)
        {
            sprRenderer.flipX = true;
        }
    }

    void FixedUpdate()
    {
        if (letControls)
        {
            rigidbody2d.MovePosition(rigidbody2d.position + move * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "SecretWalls")
        {
            FadeWalls walls = other.GetComponent<FadeWalls>();
            walls.charactersInside++;
            walls.changeAlpha(0.5f);
        }
        else if (other.gameObject.name == "Key")
        {
            print("Llave encontrada!!");
            GameObject.Find("Main Camera").GetComponent<ShakeScreen>().TriggerShake();
            other.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "SecretWalls")
        {
            FadeWalls walls = other.GetComponent<FadeWalls>();
            walls.charactersInside--;
            if (walls.charactersInside == 0)
            {
                walls.changeAlpha(1.0f);
            }
        }
    }
}
