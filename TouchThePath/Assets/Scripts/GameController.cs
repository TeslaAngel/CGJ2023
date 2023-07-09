using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameController : MonoBehaviour
{
	public static GameController Instance { get; private set; }
	public bool IsAtNight { get; private set; }

	public GameObject player;
	public TreasureScript treasure;

	public SpriteRenderer nightSprite;
	public float nightFadeTime = 0.5f;

	[Space]
	List<GameObject> Ghosts;
    List<GameObject> FakeMountains;


	public GameObject handPrintPrefab;

	List<HandPrint> handPrints;

	//定义了手掌[0(第一个), 1(最后一个)]对应的alpha 
	public AnimationCurve handPrintAlphaCurve;

	public LevelMap currentMap;


	// Use this for initialization
	void Start()
	{
		Instance = this;

		if (currentMap == null)
		{
			var mapName = SceneHelper.Instance.toLoadLevelMapName;
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
		}

		if (nightSprite != null)
		{
			nightSprite.enabled = false;
		}
		
		handPrints = new List<HandPrint>();

		AudioManager.Instance.PlayBgm(Sound.GameDayBGM);

		Ghosts = currentMap.Ghosts;
        FakeMountains = currentMap.FakeMountains;


        //Turn Ghosts inActive
        foreach (GameObject ghost in Ghosts)
        {
            ghost.SetActive(false);
        }
		//Making Fakemountains Static
		foreach (GameObject mount in FakeMountains)
		{
			//mount.GetComponent<PathIndicator>().enabled = false;
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

		float t = handPrints.Count / (float)currentMap.maxHandPrintCount;
		float alpha = handPrintAlphaCurve.Evaluate(t);
		handprint.SetDayStatus(alpha);
		handPrints.Add(handprint);
		//TODO: sfx

		return true;
	}

	private void Update()
	{
		if (IsAtNight)
		{
			
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
		});

		AudioManager.Instance.PlayBgm(Sound.GameNightBGM);

		//Turn Ghosts Active
		foreach (GameObject ghost in Ghosts)
		{
			//Invoke()
			ghost.SetActive(true);
		}
        //Making Fakemountains Dynamic
        foreach (GameObject mount in FakeMountains)
        {
            //mount.GetComponent<PathIndicator>().enabled = true;
        }
    }

	public void OnPlayerWin()
	{
		//TODO 显示小关成功界面 宝石发光之类的？
		Debug.Log("Game Win!");
		SceneHelper.Instance.GotoLevelSelect();
		UnlockedLevels.LevelUnlocked++;
	}


	public void OnPlayerDied()
	{
		//TODO 显示失败界面
		//失败音效
		Debug.Log("Game Lose!");


		nightSprite.enabled = true;
		nightSprite.DOFade(0.5f, nightFadeTime);

		Invoke("PlayerDiedGoStart", 2);
		
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
