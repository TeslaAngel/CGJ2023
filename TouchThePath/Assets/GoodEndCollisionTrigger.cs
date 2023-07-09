using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodEndCollisionTrigger : MonoBehaviour
{

    private bool Activated = false;
    Camera cameraM;
    public float speed;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(collision.gameObject.GetComponent<PlayerScript>());
            Activated = true;
            collision.gameObject.GetComponent<Animator>().Play("Player_Walk_Left");
            collision.gameObject.GetComponent<Animator>().speed = 0f;

            GetComponent<Animator>().SetBool("ToEnd", true);
        }
    }

    private void Start()
    {
        cameraM = Camera.main;
    }

    private void Update()
    {
        if (Activated)
        {
            cameraM.transform.position = Vector3.Lerp(cameraM.transform.position, new Vector3(transform.position.x, transform.position.y, -10f), Time.deltaTime*speed);
            cameraM.fieldOfView = Mathf.Lerp(cameraM.fieldOfView, 20f, Time.deltaTime * speed);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

}
