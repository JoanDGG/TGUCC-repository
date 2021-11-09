using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutCandies : MonoBehaviour
{
    public float fadeTime = 0.25f;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator FadeOutRoutine()
    {
        while (spriteRenderer.color.a > 0.0f)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r,
                                             spriteRenderer.color.g,
                                             spriteRenderer.color.b,
                                             spriteRenderer.color.a 
                                             - (Time.deltaTime / fadeTime));
            yield return null;
        }
        if (spriteRenderer.color.a <= 0.2f)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r,
                                             spriteRenderer.color.g,
                                             spriteRenderer.color.b, 0f);
            Destroy(gameObject);
        }
    }
}
