using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public static class UnlockedLevels
{
    public static int LevelUnlocked = 1;
}

public class LevelSelectController : MonoBehaviour
{
    public Canvas canvas;
	public GameObject helpText;

	//public DoorObject[] doors;
	int selectingDoorIndex = -1;

	Camera cam;
	int layerMask;

    //public Action<int> OnSelectLevel;


    // Start is called before the first frame update
    void Start()
	{
		cam = Camera.main;
		layerMask = LayerMask.GetMask("Interact");

		if (GD.haveTouchTitle)
		{
			canvas.gameObject.SetActive(false);
		}
		helpText.SetActive(GD.haveTouchTitle);
	}


	public void toScene(int i, string levelMapName)
	{
        Debug.Log("Click Level " + i);
        
        if (!string.IsNullOrEmpty(levelMapName))
        {
            SceneHelper.Instance.GotoGameLevel(levelMapName);
        }
    }


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			GD.haveTouchTitle = true;
			canvas.GetComponent<CanvasGroup>().DOFade(0f, 1f).OnComplete(() =>
			{
				canvas.gameObject.SetActive(false);
				helpText.SetActive(true);
			});
		}
	}
}
