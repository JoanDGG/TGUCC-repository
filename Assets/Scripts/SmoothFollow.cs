using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
Descripcion:
Este script maneja el movimiento de la camara de forma suave, recibe los parametros como los
limites hasta donde puede llegar y los margenes que le dan al objetivo para moverse sin que la
camara se mueva

Autor: Joan Daniel Guerrero Garcia
*/

public class SmoothFollow : MonoBehaviour
{
    public float xMargin = 2.0f;    // Cantidad en x para que no se mueva mientras que el objetivo lo haga
    public float yMargin = 0.0f;    // Cantidad en y para que no se mueva mientras que el objetivo lo haga

    public float xSmooth = 2.0f;   // Valor de la suavidad con la que la camara se mueve en el eje x
    public float ySmooth = 2.0f;   // Valor de la suavidad con la que la camara se mueve en el eje y

    public float yOffset = 1.0f;    // Offset value on the y axis

    [VectorLabels("Min", "Max")]
    public Vector2 xLimits;        // Limites en x para que la camara se mueva
    [VectorLabels("Min", "Max")]
    public Vector2 yLimits;        // Limites en y para que la camara se mueva

    public GameObject target;       // GameObject que seguira

    private Transform CameraTarget; // Transform del GameObject objetivo

    void Awake()
    {
        CameraTarget = target.transform;
    }

    bool CheckXMargin()     // Revisa si la posicion en x del personaje supera al margen en el eje x
    {
        return Mathf.Abs(transform.position.x - CameraTarget.position.x) > xMargin;
    }

    bool CheckYMargin()     // Revisa si la posicion en x del personaje supera al margen en el eje y
    {
        return Mathf.Abs(transform.position.y - CameraTarget.position.y) > yMargin;
    }

    void FixedUpdate()
    {
        TrackPlayer();
    }

    void TrackPlayer()
    {
        CameraTarget = target.transform;
        Vector2 offset = new Vector2(0, yOffset);
        float targetX = transform.position.x;
        float targetY = transform.position.y;

        if(CheckXMargin())
        {
            // Se utiliza la funcion Lerp para hacer una transicion suave con la camara hacia el target
            targetX = Mathf.Lerp(transform.position.x, CameraTarget.position.x, xSmooth * Time.deltaTime);
        }

        if (CheckYMargin())
        {
            targetY = Mathf.Lerp(transform.position.y, CameraTarget.position.y + offset.y, ySmooth * Time.deltaTime);
        }

        // Se utiliza la funcion Clamp para mover la camara hacia el target dentro de los limites en x & y
        targetX = Mathf.Clamp(targetX, xLimits.x, xLimits.y);
        targetY = Mathf.Clamp(targetY, yLimits.x, yLimits.y);

        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }
}
