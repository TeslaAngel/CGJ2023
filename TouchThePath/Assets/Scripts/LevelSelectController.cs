using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnlockedLevels
{
    public static int LevelUnlocked = 1;
}

public class LevelSelectController : MonoBehaviour
{

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
	}


	public void toScene(int i, string levelMapName)
	{
        Debug.Log("Click Level " + i);
        
        if (!string.IsNullOrEmpty(levelMapName))
        {
            SceneHelper.Instance.GotoGameLevel(levelMapName);
        }
    }
}
