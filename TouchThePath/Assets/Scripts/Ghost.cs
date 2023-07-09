using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Ghost : MonoBehaviour
{
	public SpriteRenderer eye;
	public SpriteRenderer body;

	public void HideInstant()
	{
		gameObject.SetActive(false);
	}

	public void Show()
	{
		gameObject.SetActive(true);
		var c = new Color(1, 1, 1, 0);
		eye.color = c;
		body.color = c;
		eye.DOFade(1f, 0.5f);
		body.DOFade(1f, 1f);
	}
}
