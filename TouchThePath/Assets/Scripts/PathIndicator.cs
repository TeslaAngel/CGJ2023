using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class PathIndicator : MonoBehaviour
{
    [HideInInspector]
    public Vector2 Des1;
    public Vector2 Des2;

    public float Speed;
    private bool Heading = true; //From des1 to des2

    private Animator animator;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (animator)
            {
                animator.SetBool("Kill", true);
                Destroy(GetComponent<PathIndicator>());
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Des1 = transform.position;
        if(GetComponent<Animator>())
        {
            animator = GetComponent<Animator>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Heading)
        {
            if (Vector2.Distance(transform.position, Des2) < 0.1f)
            {
                Heading = false;
                return;
            }
            transform.position = Vector2.MoveTowards(transform.position, Des2, Speed * Time.deltaTime);
            if (animator)
            {
                animator.SetBool("Down&Left", true);
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, Des1) < 0.1f)
            {
                Heading = true;
                return;
            }
            transform.position = Vector2.MoveTowards(transform.position, Des1, Speed * Time.deltaTime);
            if (animator)
            {
                animator.SetBool("Down&Left", false);
            }
        }
    }
}
