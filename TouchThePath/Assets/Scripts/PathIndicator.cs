using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathIndicator : MonoBehaviour
{
    [HideInInspector]
    public Vector2 Point1;
    public Vector2 Point2;
    private bool Heading1to2 = true;
    public float SpeedMultifier;

    // Start is called before the first frame update
    void Start()
    {
        Point1 = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Heading1to2)
        {
            transform.position = Vector2.MoveTowards(transform.position, Point2, Time.deltaTime* SpeedMultifier);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, Point1, Time.deltaTime* SpeedMultifier);
        }

        if(Vector2.Distance(transform.position, Point2) < 0.1f && Heading1to2)
        {
            Heading1to2 = false;
        }

        if (Vector2.Distance(transform.position, Point1) < 0.1f && !Heading1to2)
        {
            Heading1to2 = true;
        }
    }
}
