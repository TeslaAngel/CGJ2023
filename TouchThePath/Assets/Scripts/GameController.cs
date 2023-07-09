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
	public List<GameObject> Ghosts;
    public List<GameObject> FakeMountains;


	public GameObject handPrintPrefab;

	List<HandPrint> handPrints;

	//定义了手掌[0(第一个), 1(最后一个)]对应的alpha 
	public AnimationCurve handPrintAlphaCurve;

	LevelMap currentMap;


	// Use this for initialization
	void Start()
	{
		Instance = this;

		//var levelId = SceneHelper.Instance.toLoadLevel;
		var mapName = SceneHelper.Instance.toLoadLevelMapName;
		if (string.IsNullOrEmpty(mapName))
			mapName = "1";
		var mapPrefab = Resources.Load<GameObject>("Prefabs/Map/Map_" + mapName);

		var mapObj = Instantiate(mapPrefab);
		var map = mapObj.GetComponent<LevelMap>();
		this.currentMap = map;

		player.transform.position = map.playerSpawnPoint.position;
		//player.transform.rotation = map.

		treasure.transform.position = map.treasurePoint.position;

		nightSprite.enabled = false;
		handPrints = new List<HandPrint>();

		AudioManager.Instance.PlayBgm(Sound.GameDayBGM);


        //Turn Ghosts inActive
        foreach (GameObject ghost in Ghosts)
        {
            ghost.SetActive(false);
        }
		//Making Fakemountains Static
		foreach (GameObject mount in FakeMountains)
		{
			mount.GetComponent<PathIndicator>().enabled = false;
		}
    }


	public void ActivateGhost()
	{

	}

	public bool CanAddHandPrint()
	{
		return handPrints.Count < currentMap.maxHandPrintCount;
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
            mount.GetComponent<PathIndicator>().enabled = true;
        }
    }

	public void OnPlayerWin()
	{
		//TODO 显示小关成功界面 宝石发光之类的？
		Debug.Log("Game Win!");
		SceneHelper.Instance.GotoLevelSelect();
	}


	public void OnPlayerDied()
	{
		//TODO 显示失败界面
		//失败音效
		Debug.Log("Game Lose!");
		Invoke("PlayerDiedGoStart", 2);
		
        //AudioManager.Instance.PlayBgm(Sound.PlayerDied);
    }

	private void PlayerDiedGoStart()
	{
        SceneHelper.Instance.GotoStart();
    }

}
