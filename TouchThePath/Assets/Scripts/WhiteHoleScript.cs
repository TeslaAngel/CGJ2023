using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhiteHoleScript : MonoBehaviour
{
    public float AttractRadius;
    public Rigidbody2D Player;
    public float Power;

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, Player.position) <= AttractRadius)
        {
            Vector2 pp = Player.position;
            Vector2 tp = transform.position;
            Player.AddForce((pp - tp) * Power);
        }
    }
}
