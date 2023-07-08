using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GD
{
    public static bool toNight = false;
}


public class TreasureScript : MonoBehaviour
{
    public SpriteRenderer nightSprite;
    [Space]
    public List<GameObject> Ghosts;

    private void Start()
    {
        nightSprite.color = new Color(nightSprite.color.r, nightSprite.color.g, nightSprite.color.b, 0f);
    }

    void SetNSalpha(float f)
    {
        nightSprite.color = new Color(nightSprite.color.r, nightSprite.color.g, nightSprite.color.b, f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //set daytime to night
            GD.toNight = true;
        }
    }

    private void Update()
    {
        if (GD.toNight)
        {
            if(nightSprite.color.a < 1f)
            {
                SetNSalpha(Mathf.Lerp(nightSprite.color.a, 1f, Time.deltaTime));
            }

            foreach(GameObject ghost in Ghosts)
            {
                Color OGC = ghost.GetComponent<SpriteRenderer>().color;
                if (ghost.GetComponent<SpriteRenderer>().color.a < 1f)
                {
                    ghost.GetComponent<SpriteRenderer>().color = new Color(OGC.r, OGC.g, OGC.b, Mathf.Lerp(OGC.a, 1f, Time.deltaTime));
                }
            }
        }
    }
}
