using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StartController : MonoBehaviour
{
	public Button startButton;


	// Start is called before the first frame update
	void Start()
	{
		CursorHelper.SetImage(CursorImageType.PalmNormal);
		startButton.onClick.AddListener(OnClickStart);
	}


	void OnClickStart()
	{
		SceneHelper.Instance.FadeLoadScene("level_select");
	}

}
