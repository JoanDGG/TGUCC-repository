using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{
    [System.NonSerialized] public bool forceOut = false;
    [System.NonSerialized] public bool finishedWriting = false;
    public float fadeTime = 3.0f;
    public float fadeMovement = 0.5f;
    public float lifeTime = 5.0f;

    private TextMesh texto;

    // Start is called before the first frame update
    void Start()
    {
        texto = GetComponent<TextMesh>();
        StartCoroutine(lifeTimeRoutine());
    }

    public void WriteText()
    {
        texto = GetComponent<TextMesh>();
        texto.text = "";
        StartCoroutine(WriteTextRoutine());
    }

    public void FadeInText()
    {
        texto = GetComponent<TextMesh>();
        texto.color = new Color(texto.color.r, texto.color.g, texto.color.b, 0f);
        StartCoroutine(FadeInRoutine());
    }

    public void FadeOutText()
    {
        texto.color = new Color(texto.color.r, texto.color.g, texto.color.b, 1f);
        StartCoroutine(FadeOutRoutine());
    }

    private IEnumerator lifeTimeRoutine()
    {
        yield return new WaitForSeconds(lifeTime);
        FadeOutText();
    }

    private IEnumerator WriteTextRoutine()
    {
        while(texto.text != "...")
        {
            texto.text += ".";
            yield return new WaitForSeconds(0.8f);
        }
        if(texto.text == "...")
        {
            finishedWriting = true;
        }
    }

    private IEnumerator FadeInRoutine()
    {
        while (texto.color.a < 1.0f)
        {
            if(!forceOut)
            {
                gameObject.transform.Translate(new Vector3(0f,
                       (Time.deltaTime / fadeMovement), 0f));

                texto.color = new Color(texto.color.r,
                                        texto.color.g,
                                        texto.color.b,
                                        texto.color.a + (Time.deltaTime / fadeTime));
                yield return null;
            }
            else
            {
                texto.color = new Color(texto.color.r, 
                                        texto.color.g, 
                                        texto.color.b, 1f);
                FadeOutText();
                break;
            }
        }
        if (texto.color.a >= 0.9f)
        {
            texto.color = new Color(texto.color.r, 
                                    texto.color.g, 
                                    texto.color.b, 1f);
        }
    }

    private IEnumerator FadeOutRoutine()
    {
        while (texto.color.a > 0.0f)
        {
            gameObject.transform.Translate(new Vector3(0f,
                       -(Time.deltaTime / fadeMovement), 0f));

            texto.color = new Color(texto.color.r,
                                    texto.color.g, 
                                    texto.color.b,
                                    texto.color.a - (Time.deltaTime / fadeTime));
            yield return null;
        }
        if (texto.color.a <= 0.1f)
        {
            texto.color = new Color(texto.color.r, 
                                    texto.color.g, 
                                    texto.color.b, 0f);
            Destroy(gameObject);
        }
    }
}
