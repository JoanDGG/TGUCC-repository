using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour
{
    [System.NonSerialized] public bool letControls = true;
    public float movementSpeed = 5;

    private Vector2 move;
    private Vector2 trackVelocity;
    private Vector2 lastPos;
    private float horizontal;
    private float vertical;
    private float mouseInput;
    private bool interact;
    private bool trigger;
    private bool inspect;

    public KeyCode InteractKey;
    public KeyCode TriggerKey;
    public KeyCode InspectKey;

    private SpriteRenderer sprRenderer;
    private Rigidbody2D rigidbody2d;
    private Animator animator;
    private PhotonView view;

    private GameObject HideSpot;
    private GameObject Trap;

    new private GameObject camera;

    public int numCandies;

    
    void Start()
    {
        sprRenderer = GetComponent<SpriteRenderer>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        view = GetComponent<PhotonView>();
        lastPos = rigidbody2d.position;
        if (gameObject.CompareTag("Spooky"))
        {
            camera = GameObject.Find("Camera1");
        }
        else
        {
            camera = GameObject.Find("Camera2");
        }
        if(camera != null)
            camera.GetComponent<SmoothFollow>().target = gameObject;

        if (gameObject.CompareTag("Spooky"))
        {
            InteractKey = GameManager.ControlsP1[0];
            TriggerKey = GameManager.ControlsP1[1];
            InspectKey = GameManager.ControlsP1[2];

            print(gameObject.name);
            print(InteractKey.ToString());
            print(TriggerKey.ToString());
            print(InspectKey.ToString());
        }
        else if (gameObject.CompareTag("Kid"))
        {
            InteractKey = GameManager.ControlsP2[0];
            TriggerKey = GameManager.ControlsP2[1];
            InspectKey = GameManager.ControlsP2[2];
        }
    }

    void Update()
    {
        if (GameManager.room == 3 || GameManager.room == 5)
        {
            //print("Local controls");
            if (gameObject.CompareTag("Spooky"))
            {
                horizontal = ((Input.GetKey("d")) ? 1 : (Input.GetKey("a")) ? -1 : 0);
                vertical = ((Input.GetKey("w")) ? 1 : (Input.GetKey("s")) ? -1 : 0);
            }
            else if (gameObject.CompareTag("Kid"))
            {
                horizontal = ((Input.GetKey("right")) ? 1 : (Input.GetKey("left")) ? -1 : 0);
                vertical = ((Input.GetKey("up")) ? 1 : (Input.GetKey("down")) ? -1 : 0);
            }
            mouseInput = Input.GetAxis("Mouse ScrollWheel");
            interact = Input.GetKeyDown(InteractKey);
            trigger = Input.GetKeyDown(TriggerKey);
            inspect = Input.GetKeyDown(InspectKey);

            move = new Vector2(horizontal * movementSpeed, vertical * movementSpeed);
            animator.SetFloat("velocity", Mathf.Abs(move.x) + Mathf.Abs(move.y));
        }
        else if ((view.IsMine && GameManager.room != 5) ||
            GameManager.room == 1 || GameManager.room == 4 ||
            (view.IsMine && GameManager.room == 2))
        {
            //print("Online controls");
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            mouseInput = Input.GetAxis("Mouse ScrollWheel");
            interact = Input.GetKeyDown(InteractKey);
            trigger = Input.GetKeyDown(TriggerKey);
            inspect = Input.GetKeyDown(InspectKey);

            move = new Vector2(horizontal * movementSpeed, vertical * movementSpeed);
            animator.SetFloat("velocity", Mathf.Abs(move.x) + Mathf.Abs(move.y));
        }

        HandleInputs();
    }

    void FixedUpdate()
    {
        if (letControls)
        {
            rigidbody2d.MovePosition(rigidbody2d.position + move * Time.deltaTime);
            trackVelocity = (rigidbody2d.position - lastPos) * 50;
            lastPos = rigidbody2d.position;
            if (trackVelocity.x > 0.1)
            {
                sprRenderer.flipX = false;
            }
            else if (trackVelocity.x < -0.1)
            {
                sprRenderer.flipX = true;
            }
        }
    }

    private void HandleInputs()
    {
        if (interact)
        {
            //print("Z");
            if (HideSpot != null)
            {
                if(GameManager.room == 2)
                {
                    view.RPC("Search", RpcTarget.All);
                }
                else
                {
                    HideSpot.GetComponent<HideSpotController>().Search();
                }
            }
            else if (Trap != null)
            {
                if (GameManager.room == 2)
                {
                    view.RPC("AccessControl", RpcTarget.All);
                }
                else
                {
                    Trap.GetComponent<TrapController>().AccessControl();
                }
            }
        }
        else if (trigger)
        {
            //print("X");
            if (Trap != null)
            {
                if (GameManager.room == 2)
                {
                    view.RPC("Trigger", RpcTarget.All);
                }
                else
                {
                    Trap.GetComponent<TrapController>().Trigger();
                }
            }
        }
        else if (inspect)
        {
            //print("C");
            if (Trap != null)
            {
                Trap.GetComponent<TrapController>().ShowOwner();
            }
        }
        if(camera != null)
        {
            if (mouseInput > 0)
            {
                camera.GetComponent<ZoomCamera>().ReduceFOV();
            }
            if (mouseInput < 0)
            {
                camera.GetComponent<ZoomCamera>().IncreaseFOV();
            }
        }
    }

    [PunRPC]
    private void Search()
    {
        HideSpot.GetComponent<HideSpotController>().Search();
    }

    [PunRPC]
    private void AccessControl()
    {
        Trap.GetComponent<TrapController>().AccessControl();
    }

    [PunRPC]
    private void Trigger()
    {
        Trap.GetComponent<TrapController>().Trigger();
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
            GameObject.Find("Camera1").GetComponent<ShakeScreen>().TriggerShake();
            GameObject.Find("Camera2").GetComponent<ShakeScreen>().TriggerShake();
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("HideSpot"))
        {
            HideSpot = other.gameObject;
        }
        else if (other.CompareTag("Trap"))
        {
            Trap = other.gameObject;
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
        else if (other.CompareTag("HideSpot"))
        {
            HideSpot = null;
        }
        else if (other.CompareTag("Trap"))
        {
            Trap = null;
        }
    }

    public void Candies(Vector2 position, int candies = 0)
    {
        if(candies == 0)
            candies = Random.Range(1, 6);
        numCandies = candies;
        if (GameManager.room == 2)
        {
            view.RPC("GetCandies", RpcTarget.All, position, candies);
        }
        else
        {
            GetCandies(position, candies);
        }
    }

    [PunRPC]
    private void GetCandies(Vector2 position, int candies)
    {
        numCandies = candies;
        StartCoroutine(ShowCandies(position, candies));
    }

    private IEnumerator ShowCandies(Vector2 Originalposition, int candies)
    {
        if (gameObject.tag == "Spooky")
        {
            GameManager.spookyCandies += candies;
            //print(GameManager.spookyCandies);

            GameObject.Find(gameObject.tag + "Score").
                GetComponent<Text>().text = GameManager.spookyCandies.ToString();
        }
        else
        {
            GameManager.kidCandies += candies;
            //print(GameManager.kidCandies);

            GameObject.Find(gameObject.tag + "Score").
                GetComponent<Text>().text = GameManager.kidCandies.ToString();
        }

        candies = (candies > 3 ? 3 : candies);
        for (int i = 0; i < candies; i++)
        {
            if (GameManager.candies[i] != null)
            {
                Vector3 position = new Vector3(
                                   Originalposition.x,
                                   Originalposition.y + 0.75f, 0f);
                GameObject candyObject = Instantiate(GameManager.candies[i],
                    position, Quaternion.identity);
                Renderer textRenderer = candyObject.GetComponent<Renderer>();
                textRenderer.sortingOrder = 6;
                yield return new WaitForSeconds(0.12f);
            }
        }
    }
}
