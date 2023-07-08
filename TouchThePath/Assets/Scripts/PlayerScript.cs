using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerScript : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rigidbody2;
    public GameObject Prefab_HandPrint;
    public float SpeedMultifier;

    [Space]
    internal float HandPrintLoadtime = 0f;
    public int HandPrintLimit;
    public float HandPrintInterval;
    public float HandPrintAlphaDifference;
    

    void ShootHandPrint()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 1);
        
        if(hit.collider != null)
        {
            //Initiate a handprint on wall
            Instantiate(Prefab_HandPrint, hit.point, transform.rotation);//TO BE ALTERED
        }
    }

    void Awake()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();

        //Restore HandPrint Alpha Color
        Color OriginalCol = Prefab_HandPrint.GetComponent<SpriteRenderer>().color;
        Prefab_HandPrint.GetComponent<SpriteRenderer>().color = new Color(OriginalCol.r, OriginalCol.g, OriginalCol.b, 1f);
    }


    void Update()
    {
        //Behavior: move
        rigidbody2.velocity = new Vector2(Input.GetAxis("Horizontal")*SpeedMultifier, Input.GetAxis("Vertical")*SpeedMultifier);

        //Behavior: handPrint
        if(Input.GetAxis("Fire1") > 0 && HandPrintLoadtime <= 0f)
        {
            //alter alpha value
            Color OriginalCol = Prefab_HandPrint.GetComponent<SpriteRenderer>().color;
            Prefab_HandPrint.GetComponent<SpriteRenderer>().color = new Color(OriginalCol.r, OriginalCol.g, OriginalCol.b, OriginalCol.a - HandPrintAlphaDifference);
            ShootHandPrint();
            HandPrintLoadtime = HandPrintInterval;
        }
        else if(HandPrintLoadtime > 0f)
        {
            HandPrintLoadtime -= Time.deltaTime;
        }
    }
}
