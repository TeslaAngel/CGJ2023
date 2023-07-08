using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorDetecter : MonoBehaviour
{

	public DoorObject[] doors;
	int selectingDoorIndex = -1;

	Camera cam;
	int layerMask;


	public Action<int> OnSelectLevel;


	// Start is called before the first frame update
	void Start()
	{
		cam = Camera.main;
		layerMask = LayerMask.GetMask("Interact");
	}

	// Update is called once per frame
	void Update()
	{
		//鼠标选中检测
		var pos = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
		var col = Physics2D.OverlapPoint(pos, layerMask);
		//Debug.Log(col);
		if (col != null && col.gameObject.name.StartsWith("DoorDectector_"))
		{
			//越界？不检查哟
			var strIndex = col.gameObject.name.Substring(14);
			int index = int.Parse(strIndex);

			if (index != selectingDoorIndex)
			{
				Debug.LogWarning("selected: " + index);
				selectingDoorIndex = index;
				doors[index].PlayAnimation(true);

				for (int i = 0; i < doors.Length; i++)
				{
					if (i != index)
						doors[i].PlayAnimation(false);
				}
			}
		}
		else if (selectingDoorIndex != -1)
		{
			doors[selectingDoorIndex].PlayAnimation(false);
			selectingDoorIndex = -1;
		}


		//选择关卡
		if (selectingDoorIndex != -1)
		{
			if (Input.GetMouseButtonDown(0))
			{
				Debug.Log("Click Level " + selectingDoorIndex);
				OnSelectLevel?.Invoke(selectingDoorIndex);
			}
		}

	}
}
