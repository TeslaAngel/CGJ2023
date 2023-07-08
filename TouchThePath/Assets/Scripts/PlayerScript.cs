using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [Space]
    public bool ChaseByCamera;
    public float CameraDrag;

    void Awake()
    {
        rigidbody2 = GetComponent<Rigidbody2D>();

        //Restore HandPrint Alpha Color
        Color OriginalCol = Prefab_HandPrint.GetComponent<SpriteRenderer>().color;
        Prefab_HandPrint.transform.GetComponent<SpriteRenderer>().color = new Color(OriginalCol.r, OriginalCol.g, OriginalCol.b, 1f);
    }


    //Death when touch wall at night
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GD.toNight)
        {
            //Bad ending
            Destroy(GetComponent<PlayerScript>());
        }
    }


    void Update()
    {
        //Behavior: move
        rigidbody2.velocity = new Vector2(Input.GetAxis("Horizontal")*SpeedMultifier, Input.GetAxis("Vertical")*SpeedMultifier);

        //Behavior: handPrint
        if(Input.GetAxis("Fire1") > 0 && HandPrintLoadtime <= 0f && Prefab_HandPrint.GetComponent<SpriteRenderer>().color.a > 0)
        {
            //raycast
            Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (hit.collider != null)
            {
                //Initiate a handprint on wall
                Instantiate(Prefab_HandPrint, hit.point, Quaternion.Euler(0f, 0f, angle));//TO BE ALTERED
            }
            else
            {
                return;
            }

            //alter alpha value
            Color OriginalCol = Prefab_HandPrint.GetComponent<SpriteRenderer>().color;
            Prefab_HandPrint.GetComponent<SpriteRenderer>().color = new Color(OriginalCol.r, OriginalCol.g, OriginalCol.b, OriginalCol.a - (1f/HandPrintLimit));
            HandPrintLoadtime = HandPrintInterval;
        }

        //timer for handprint
        if(HandPrintLoadtime > 0f)
        {
            HandPrintLoadtime -= Time.deltaTime;
        }

        //Camera Chase
        if (ChaseByCamera)
        {
            Vector3 targetPos = new Vector3(transform.position.x, transform.position.y, -10f);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, targetPos, Time.deltaTime * CameraDrag);
        }
        
    }
}
