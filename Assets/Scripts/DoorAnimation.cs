using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    private Animator animator;
    private SmoothFollow smoothFollow;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        smoothFollow = GameObject.Find("Main Camera").GetComponent<SmoothFollow>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        animator.SetBool("Open", true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        animator.SetBool("Open", false);
    }

    public void Open()
    {
        StartCoroutine(WaitForOpen());
    }

    IEnumerator WaitForOpen()
    {
        yield return new WaitForSeconds(
            GameObject.Find("Main Camera").GetComponent<ShakeScreen>().shakeTime / 2);
        animator.SetBool("Open", true);
        smoothFollow.xLimits.x -= 7f;
        smoothFollow.xLimits.y += 7f;
        smoothFollow.yLimits.x -= 7f;
        smoothFollow.yLimits.y += 7f;
        Destroy(GetComponent<Collider2D>());
    }
}
