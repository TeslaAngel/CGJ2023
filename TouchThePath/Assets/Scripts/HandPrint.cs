using System.Collections;
using UnityEngine;
using DG.Tweening;

public class HandPrint : MonoBehaviour
{
	public Color color = Color.white;
	public SpriteRenderer spriteDay;
	public SpriteRenderer spriteNight;
	public SpriteRenderer spriteNightLight;


	public float handPrintFadeInTime = 1f;
	public float handPrintFlashCycle = 3.3f;

	bool atNight;
	float flashOffset;
	float flashTimer;

	float alpha = 1f;

	public void SetDayStatus(float alpha)
	{
		spriteDay.gameObject.SetActive(true);
		spriteNight.gameObject.SetActive(false);
		spriteNightLight.gameObject.SetActive(false);

		this.alpha = alpha;

		color.a = alpha;
		spriteDay.color = color;
		color.a = 0f;
		spriteNight.color = color;
		spriteNightLight.color = color;
		atNight = false;
	}

	public void SetNightStatus()
	{
		spriteNight.gameObject.SetActive(true);
		spriteNight.DOFade(alpha, handPrintFadeInTime).OnComplete(() =>
		{
			spriteDay.DOFade(0f, 0.3f).OnComplete(() =>
			{
				spriteDay.gameObject.SetActive(false);
				atNight = true;
				flashTimer = 0f;
			});
		});

		color.a = 0f;
		spriteNightLight.color = color;
		spriteNightLight.gameObject.SetActive(true);

		flashOffset = Random.Range(-1f, 1f) * 1f;
	}

	void Update()
	{
		if (atNight)
		{
			flashTimer += Time.deltaTime;
			float t = Mathf.Sin((flashTimer * Mathf.PI * 2 / handPrintFlashCycle) + flashOffset);
			color.a = t * alpha;
			spriteNightLight.color = color;
		}
	}
}
