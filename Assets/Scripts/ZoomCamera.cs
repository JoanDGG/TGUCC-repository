using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomCamera : MonoBehaviour
{
    [System.NonSerialized] public Camera mycamera;

    [VectorLabels("Min", "Max")]
    public Vector2 yOffsetLimits = new Vector2(0.4f, 1.0f);
    // Ranges: Min(0.0, 0.9) & Max(0.1, 1.0)

    [VectorLabels("Min", "Max")]
    public Vector2 xMarginLimits = new Vector2(0.5f, 2.0f);
    // Ranges: Min(0.0, 1.9) & Max(0.1, 2.0)

    [VectorLabels("Min", "Max")]
    public Vector2 FOVLimits = new Vector2(10.0f, 35.0f);
    // Ranges (recommended): Min(1.0, 49.0) & Max(2.0, 40.0)

    private float yOffsetSlope;
    private float yOffsetYIntercept;

    private float xMarginSlope;
    private float xMarginYIntercept;

    // Start is called before the first frame update
    void Start()
    {
        mycamera = gameObject.GetComponent<Camera>();
        SetZoomLimitValues(yOffsetLimits.x,
                           yOffsetLimits.y, 
                           xMarginLimits.x, 
                           xMarginLimits.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (mycamera.fieldOfView > FOVLimits.x)
            {
                mycamera.fieldOfView--;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (mycamera.fieldOfView < FOVLimits.y)
            {
                mycamera.fieldOfView++;
            }
        }
    }

    void FixedUpdate()
    {
        SetZoomValues();
    }

    private void SetZoomValues()
    {
        // Needs SmoothFollow script
        gameObject.GetComponent<SmoothFollow>().yOffset =
                yOffsetSlope * mycamera.fieldOfView - yOffsetYIntercept;

        gameObject.GetComponent<SmoothFollow>().xMargin =
            xMarginSlope * mycamera.fieldOfView - xMarginYIntercept;
    }

    public void SetZoomLimitValues(float minYOffset,
                                   float maxYOffset, 
                                   float minXMargin,
                                   float maxXMargin)
    {
        yOffsetSlope = ((maxYOffset - minYOffset) / (FOVLimits.y - FOVLimits.x));
        yOffsetYIntercept = (minYOffset - (yOffsetSlope * FOVLimits.x));
        print(yOffsetSlope + ", " + yOffsetYIntercept);

        xMarginSlope = ((maxXMargin - minXMargin) / (FOVLimits.y - FOVLimits.x));
        xMarginYIntercept = (minXMargin - (xMarginSlope * FOVLimits.x));
    }
}