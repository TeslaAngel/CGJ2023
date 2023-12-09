using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class GameController : MonoBehaviour
{
	public static GameController Instance { get; private set; }
	public bool IsAtNight { get; private set; }

	public GameObject player;
	public TreasureScript treasure;

	public SpriteRenderer nightSprite;
	public float nightFadeTime = 0.5f;

	[Space]
	//public bool ChaseByCamera;
	public float CameraDrag;
	public Transform followTarget;
	Camera mainCamera;

	[Space]
	List<Ghost> ghosts;
    List<GameObject> fakeMountains;


	public GameObject handPrintPrefab;

	List<HandPrint> handPrints;

	//定义了手掌[0(第一个), 1(最后一个)]对应的alpha 
	public AnimationCurve handPrintAlphaCurve;

	public LevelMap currentMap;

	public TMP_Text handPrintText;
	string handPrintTextContent;


	// Use this for initialization
	void Start()
	{
		Instance = this;
		mainCamera = Camera.main;

		if (currentMap == null)
		{
			var mapName = SceneHelper.Instance.toLoadLevelMapName;
			mapName = string.IsNullOrEmpty(mapName) ? "1" : mapName;
			var mapPrefab = Resources.Load<GameObject>("Prefabs/Map/Map_" + mapName);
			var mapObj = Instantiate(mapPrefab);
			var map = mapObj.GetComponent<LevelMap>();
			this.currentMap = map;
		}

		if (player != null)
		{
			player.transform.position = currentMap.playerSpawnPoint.position;
			//player.transform.rotation = map.
		}

		if (treasure != null)
		{
			treasure.transform.position = currentMap.treasurePoint.position;
			mainCamera.transform.position = treasure.transform.position - new Vector3(0, 0, 10f);
		}

		if (nightSprite != null)
		{
			nightSprite.enabled = false;
		}
		
		handPrints = new List<HandPrint>();

		AudioManager.Instance.PlayBgm(Sound.GameDayBGM);


		ghosts = new List<Ghost>();
		//Turn Ghosts inActive
		foreach (GameObject ghost in currentMap.Ghosts)
		{
			var g = ghost.GetComponent<Ghost>();
			g.HideInstant();
			ghosts.Add(g);
		}

		fakeMountains = currentMap.FakeMountains;
		//Making Fakemountains Static
		//foreach (GameObject mount in fakeMountains)
		//{
		//	//mount.GetComponent<PathIndicator>().enabled = false;
		//}

		if (handPrintText != null)
		{
			handPrintTextContent = handPrintText.text;
			UpdateHandPrintText();
		}
    }


	public void ActivateGhost()
	{

	}

	public bool CanAddHandPrint()
	{
		return !IsAtNight && handPrints.Count < currentMap.maxHandPrintCount;
	}

	public bool AddHandPrint(Vector3 position, Vector2 hitPointNormal, Transform intendedParent)
	{
		//Initiate a handprint on wall
		//var rotation = Quaternion.Euler(0f, 0f, angle - 90f);
		var rotation = Quaternion.LookRotation(Vector3.back, -hitPointNormal);

		var obj = Instantiate(handPrintPrefab, position, rotation, intendedParent);
		var handprint = obj.GetComponent<HandPrint>();

		float t = handPrints.Count / (float)(currentMap.maxHandPrintCount - 1);
		float alpha = handPrintAlphaCurve.Evaluate(t);
		handprint.SetDayStatus(alpha);
		handPrints.Add(handprint);
		AudioManager.Instance.PlaySfx(Sound.CreateHandPrint);

		UpdateHandPrintText();

		return true;
	}

	void UpdateHandPrintText()
	{
		int remainCount = currentMap.maxHandPrintCount - handPrints.Count;
		int totalCount = currentMap.maxHandPrintCount;
		string text = string.Format(handPrintTextContent, remainCount, totalCount);
		handPrintText.text = text;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			SceneHelper.Instance.Restart();
		}


		//if (Input.GetKeyDown(KeyCode.Alpha1))
		//	GetPunch();

		//Camera Chase
		if (followTarget != null)
		{
			Vector3 targetPos = new Vector3(followTarget.position.x, followTarget.position.y, -10f);
			mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, Time.deltaTime * CameraDrag);
		}
	
	}


	public void OnTurnNight()
	{
		Debug.Log("TurnNight");

		//set daytime to night
		IsAtNight = true;
		treasure.PlayInvokeAnimation(() =>
		{
			nightSprite.enabled = true;
			nightSprite.DOFade(1f, nightFadeTime).OnComplete(() => treasure.gameObject.SetActive(false));
			foreach (var handprint in handPrints)
			{
				handprint.SetNightStatus();
			}

			//Turn Ghosts Active
			foreach (var ghost in ghosts)
			{
				ghost.Show();
			}
			//Making Fakemountains Dynamic
			//foreach (GameObject mount in FakeMountains)
			//{
			//    //mount.GetComponent<PathIndicator>().enabled = true;
			//}
		});

		AudioManager.Instance.PlaySfx(Sound.InvokeGem);
		AudioManager.Instance.PlayBgm(Sound.GameNightBGM);
    }

	public void OnPlayerWin()
	{
		nightSprite.enabled = true;
		nightSprite.DOFade(0.5f, nightFadeTime / 2);

		//TODO 显示小关成功界面 宝石发光之类的？
		Debug.Log("Game Win!");
		UnlockedLevels.LevelUnlocked++;
		if (UnlockedLevels.LevelUnlocked > 3)
		{
			SceneHelper.Instance.GotoGoodEnd();
		}
		else
		{
			SceneHelper.Instance.GotoLevelSelect();
		}

		//Invoke(nameof(PlayerWinGotoScene), nightFadeTime);
	}

	//private void PlayerWinGotoScene()
	//{
	//	if (UnlockedLevels.LevelUnlocked > 3)
	//	{
	//		SceneHelper.Instance.GotoGoodEnd();
	//	}
	//	else
	//	{
	//		SceneHelper.Instance.GotoLevelSelect();
	//	}
	//}


	public void OnPlayerDied(float delay)
	{
		//TODO 显示失败界面
		//失败音效
		Debug.Log("Game Lose!");


		nightSprite.enabled = true;
		nightSprite.DOFade(0.5f, nightFadeTime);

		Invoke(nameof(PlayerDiedGoStart), delay);
		
        //AudioManager.Instance.PlayBgm(Sound.PlayerDied);
    }

	private void PlayerDiedGoStart()
	{
        SceneHelper.Instance.GotoLevelSelect();
    }


	public void ShowMonsterEyes(Vector2 position)
	{
		var prefab = Resources.Load<GameObject>("Prefabs/Objects/MonsterEye");

		int count = Random.Range(8, 12);
		for (int i = 0; i < count; i++)
		{
			var size = Random.Range(0.5f, 2f);
			if (Random.value < 0.1f)
				size *= 3;

			Vector2 offset = Random.insideUnitCircle;
			offset.x *= 2;
			offset = (5f * Mathf.Pow(offset.magnitude, 3) + 1f) * offset.normalized;

			var go = Instantiate(prefab);
			go.transform.position = position + offset;
			go.transform.localScale = size * Vector3.one;

			var sp = go.GetComponent<SpriteRenderer>();
			sp.color = new Color(1, 1, 1, 0);
			sp.DOFade(1f, 0.5f).SetDelay(Random.Range(0, 0.2f));
		}
	}

}
