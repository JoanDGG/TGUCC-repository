using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;

    private Animator animator;
    private SmoothFollow smoothFollow;
    private SmoothFollow smoothFollow2;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        smoothFollow = camera1.GetComponent<SmoothFollow>();
        smoothFollow2 = camera2.GetComponent<SmoothFollow>();
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
        StartCoroutine(WaitForOpen(smoothFollow));
        StartCoroutine(WaitForOpen(smoothFollow2));
    }

    IEnumerator WaitForOpen(SmoothFollow smoothFollow)
    {
        yield return new WaitForSeconds(
            camera1.GetComponent<ShakeScreen>().shakeTime / 2);
        animator.SetBool("Open", true);
        smoothFollow.xLimits.x -= 7f;
        smoothFollow.xLimits.y += 7f;
        smoothFollow.yLimits.x -= 7f;
        smoothFollow.yLimits.y += 7f;

        Destroy(GetComponent<Collider2D>());
    }
}
