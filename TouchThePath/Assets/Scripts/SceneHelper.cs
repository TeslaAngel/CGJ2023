using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneHelper : MonoBehaviour
{
	static SceneHelper _instance;
	public static SceneHelper Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = CreateInstance();
			}
			return _instance;
		}
	}

	public static SceneHelper CreateInstance()
	{
		//var obj = new GameObject("Scene Helper");
		var canvasPrefab = Resources.Load<GameObject>("Prefabs/UI/CoverCanvas");
		var canvasObj = Instantiate(canvasPrefab);
		var helper = canvasObj.GetComponent<SceneHelper>();
		helper.fadeMask.enabled = false;
		GameObject.DontDestroyOnLoad(canvasObj);
		return helper;
	}



	public Image fadeMask;
	public float fadeTime = 0.7f;
	//public float minStayTime = 0.5f;


	public void FadeLoadScene(string sceneName)
	{
		fadeMask.enabled = true;
		fadeMask.color = new Color(0f, 0f, 0f, 0f);
		fadeMask.DOFade(1, fadeTime).SetUpdate(true).OnComplete(() =>
		{
			SceneManager.LoadScene(sceneName);
			fadeMask.DOFade(0, fadeTime).SetUpdate(true).OnComplete(() =>
			{
				fadeMask.enabled = false;
			});
		});
	}


	// Start is called before the first frame update
	void Start()
	{

	}

}
