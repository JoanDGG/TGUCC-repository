using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FadeWalls : MonoBehaviour
{
    [System.NonSerialized] public int charactersInside = 0;
    public float lerpDuration = 0.15f;
    private Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public void changeAlpha(float alpha)
    {
        foreach (Vector3Int tilePosition in tilemap.cellBounds.allPositionsWithin)
        {
            tilemap.RemoveTileFlags(tilePosition, TileFlags.LockColor);
            StartCoroutine(LerpColour(tilePosition, alpha));
        }
    }

    IEnumerator LerpColour(Vector3Int tilePosition, float alpha)
    {
        Color lerpedColour = tilemap.GetColor(tilePosition);
        float timeElapsed = 0;

        while (timeElapsed < lerpDuration)
        {
            lerpedColour = new Color(lerpedColour.r, lerpedColour.g, lerpedColour.b, Mathf.Lerp(1, alpha, timeElapsed / lerpDuration));
            tilemap.SetColor(tilePosition, lerpedColour);
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        lerpedColour = new Color(lerpedColour.r, lerpedColour.g, lerpedColour.b, alpha);
        tilemap.SetColor(tilePosition, lerpedColour);
        tilemap.SetTileFlags(tilePosition, TileFlags.LockColor);
    }
}
