using System.Collections;
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

		player.transform.position = map.playerSpawnPoint.position;
		//player.transform.rotation = map.

		treasure.transform.position = map.treasurePoint.position;

		nightSprite.enabled = false;

		AudioManager.Instance.PlayBgm(Sound.GameDayBGM);
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
		});
		AudioManager.Instance.PlayBgm(Sound.GameNightBGM);
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
		SceneHelper.Instance.GotoStart();
	}



}
