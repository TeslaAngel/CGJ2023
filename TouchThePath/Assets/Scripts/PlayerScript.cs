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
    public float HandPrintLimit;
    public float HandPrintInterval;
    

    void ShootHandPrint()
    {
        Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (hit.collider != null)
        {
            //Initiate a handprint on wall
            Instantiate(Prefab_HandPrint, hit.point, Quaternion.Euler(0f, 0f, angle));//TO BE ALTERED
        }
    }

    void Awake()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();

        //Restore HandPrint Alpha Color
        Color OriginalCol = Prefab_HandPrint.GetComponent<SpriteRenderer>().color;
        Prefab_HandPrint.transform.GetComponent<SpriteRenderer>().color = new Color(OriginalCol.r, OriginalCol.g, OriginalCol.b, 1f);
    }


    void Update()
    {
        //Behavior: move
        rigidbody2.velocity = new Vector2(Input.GetAxis("Horizontal")*SpeedMultifier, Input.GetAxis("Vertical")*SpeedMultifier);

        //Behavior: handPrint
        if(Input.GetAxis("Fire1") > 0 && HandPrintLoadtime <= 0f && Prefab_HandPrint.GetComponent<SpriteRenderer>().color.a > 0)
        {
            //alter alpha value
            Color OriginalCol = Prefab_HandPrint.GetComponent<SpriteRenderer>().color;
            ShootHandPrint();
            Prefab_HandPrint.GetComponent<SpriteRenderer>().color = new Color(OriginalCol.r, OriginalCol.g, OriginalCol.b, OriginalCol.a - (1f/HandPrintLimit));
            HandPrintLoadtime = HandPrintInterval;
        }
        else if(HandPrintLoadtime > 0f)
        {
            HandPrintLoadtime -= Time.deltaTime;
        }
    }
}
