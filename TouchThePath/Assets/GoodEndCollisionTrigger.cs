using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodEndCollisionTrigger : MonoBehaviour
{

    private bool Activated = false;
    Camera cameraM;
    public float speed;
    float t = 0;
    bool playingEnd = false;
    public float waitTime = 4.5f;
    public float fadeInTime = 3f;
    public float endingDelayTime = 5f;
    public Canvas canvas;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(collision.gameObject.GetComponent<PlayerScript>());
            Activated = true;
            collision.gameObject.GetComponent<Animator>().Play("Player_Touch_Left", 0, 0f);
			collision.gameObject.GetComponent<Animator>().speed = 0f;

            GetComponent<Animator>().SetBool("ToEnd", true);
            t = Time.time;
        }
    }

    private void Start()
    {
        cameraM = Camera.main;
        canvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Activated)
        {
            cameraM.transform.position = Vector3.Lerp(cameraM.transform.position, new Vector3(transform.position.x, transform.position.y, -10f), Time.deltaTime*speed);
            cameraM.fieldOfView = Mathf.Lerp(cameraM.fieldOfView, 20f, Time.deltaTime * speed);
			if (Time.time - t > waitTime && !playingEnd)
            {
                OnEnd();
            }
		}

        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
		//	SceneHelper.Instance.Restart();
		//}
    }

    public void OnEnd()
    {
        playingEnd = true;

        canvas.gameObject.SetActive(true);
        var cg = canvas.GetComponent<CanvasGroup>();
        cg.alpha = 0f;
		cg.DOFade(1f, fadeInTime).OnComplete(() =>
		{
            StartCoroutine(DelayJump());
		});
	}

    IEnumerator DelayJump()
    {
        yield return new WaitForSecondsRealtime(endingDelayTime);
		SceneHelper.Instance.Restart();
	}
}
