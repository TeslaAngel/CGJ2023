using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorObject : MonoBehaviour
{
	//public float selectedScale = 1.5f;
	public int Index;
	public string levelMapName;
	public LevelSelectController selectController;
	//const float speed = 8f;

	//bool selected = false;
	//float scale = 1f;
	//Vector3 initialScale;
	//Coroutine coroutine;

    /*
	public void PlayAnimation(bool selected)
	{
		if (this.selected == selected)
			return;
		this.selected = selected;

		if (coroutine != null)
		{
			StopCoroutine(coroutine);
		}
		coroutine = StartCoroutine(IE_Animation(selected ? selectedScale : 1f));
	}

	IEnumerator IE_Animation(float targetScale)
	{
		while (Mathf.Abs(targetScale - scale) > 0.02f)
		{
			scale += (targetScale - scale) * Time.deltaTime * speed;
			transform.localScale = initialScale * scale;
			yield return null;
		}
		scale = targetScale;
		transform.localScale = initialScale * scale;
	}


	// Start is called before the first frame update
	void Start()
	{
		initialScale = transform.localScale;
	}
	*/


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
			//Door: Call desinated scene switch
			selectController.toScene(Index, levelMapName);
			print("to");
        }
    }
}
