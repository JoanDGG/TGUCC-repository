using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class NpcAI : MonoBehaviour
{
    public Transform[] targets = new Transform[4];
    public float speed = 200f;
    public float nextWayPointDistance = 1.2f;

    public GameObject floatingText;
    public Transform npcGFX;

    private GameObject newText;
    private Path path;
    private int currentWayPoint = 0;
    private int currentTarget = 0;
    private bool reachedEndOfPath = false;
    private bool stuck = true;
    private bool scared = false;

    private Seeker seeker;
    private Rigidbody2D ridigbody2D;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        ridigbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        InvokeRepeating("UpdatePath", 1f, 0.5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            //print(currentTarget);
            seeker.StartPath(ridigbody2D.position, 
                targets[currentTarget].position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
        else
        {
            currentTarget = 0;
            print("Error");
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
            return;


        if(currentWayPoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] 
                            - ridigbody2D.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        if(!scared)
            ridigbody2D.AddForce(force);

        float distance = Vector2.Distance(ridigbody2D.position,
            path.vectorPath[currentWayPoint]);

        if(distance < nextWayPointDistance)
        {
            currentWayPoint++;
        }

        transform.GetChild(0).GetComponent<Animator>()
            .SetFloat("velocity", Mathf.Abs(ridigbody2D.velocity.x 
                                              + ridigbody2D.velocity.y));

        if (ridigbody2D.velocity.x >= 0.01f)
        {
            npcGFX.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (ridigbody2D.velocity.x <= -0.01f)
        {
            npcGFX.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.NPCNames.Contains(other.gameObject) 
            && (other.gameObject != gameObject))
        {
            StartCoroutine(WaitRoutine());
        }
        else if(other.CompareTag("PathPoint"))
        {
            if (currentTarget >= targets.Length - 1)
            {
                currentTarget = 0;
            }
            else
            {
                currentTarget++;
                //print("Nuevo target = " + currentTarget);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (GameManager.NPCNames.Contains(other.gameObject)
            && (other.gameObject != gameObject))
        {
            stuck = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (GameManager.NPCNames.Contains(other.gameObject)
            && (other.gameObject != gameObject))
        {
            stuck = false;
        }
    }

    public void GetScared()
    {
        //print("AAAAHHH");
        scared = true;
        StartCoroutine(ScaredRoutine());
    }

    private IEnumerator ScaredRoutine()
    {
        gameObject.GetComponent<Collider2D>().enabled = false;
        SpriteRenderer image = gameObject.transform.GetChild(0).
            gameObject.GetComponent<SpriteRenderer>();
        image.color = new Color(image.color.r,
                                image.color.g,
                                image.color.b, 0.3f);
        yield return new WaitForSeconds(3);
        scared = false;
        yield return new WaitForSeconds(7);
        gameObject.GetComponent<Collider2D>().enabled = true;
        image.color = new Color(image.color.r,
                                image.color.g,
                                image.color.b, 1f);
    }

    IEnumerator NewPath()
    {
        yield return new WaitForSeconds(Random.Range(2, 5));
    }

    IEnumerator WaitRoutine()
    {
        print("Empieza...");
        yield return new WaitForSeconds(3);
        print("Termina");
        if (stuck)
        {
            print("Si");
            Vector3 position = new Vector3(
                                   gameObject.transform.position.x,
                                   gameObject.transform.position.y + 0.75f,
                                   gameObject.transform.position.z);
            newText = Instantiate(floatingText, position, Quaternion.identity);
            Renderer textRenderer = newText.GetComponent<Renderer>();
            textRenderer.sortingOrder = 6;

            newText.GetComponent<FadeOut>().WriteText();
            if (newText.GetComponent<FadeOut>().finishedWriting)
            {
                if (seeker.IsDone())
                {
                    seeker.StartPath(ridigbody2D.position,
                        targets[currentTarget].position, OnPathComplete);
                }
            }
            newText.GetComponent<FadeOut>().FadeOutText();
        }
    }
}
