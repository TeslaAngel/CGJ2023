using System;
using System.Collections.Generic;
using UnityEngine;

public enum SoundPosition
{
	Global,
	Player,
	//Object
}

public enum Sound
{
	None,
	GameDayBGM,
	GameNightBGM,

	PlayerDied = 100,
    PlayerInhale,
	PlayerDrop,
	CreateHandPrint,
    PlayerBeingHit,
	PlayerWalk1,
	PlayerWalk2,
	FakeMountain,

	//添加在下面
}


public class AudioManager : MonoBehaviour
{
	static AudioManager _instance;
	public static AudioManager Instance
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

	public static AudioManager CreateInstance()
	{
		//var obj = new GameObject("Scene Helper");
		var prefab = Resources.Load<GameObject>("Prefabs/AudioManager");
		var obj = Instantiate(prefab);
		var mgr = obj.GetComponent<AudioManager>();
		mgr.Init();
		GameObject.DontDestroyOnLoad(obj);
		return mgr;
	}


	public AudioSource globalSfxAudioSouce;
	public AudioSource bgmAudioSouce;
	public SoundSheet soundSheet;

	Dictionary<Sound, AudioClip> soundClips = new Dictionary<Sound, AudioClip>();

	private void Init()
	{
		foreach (var d in soundSheet.soundDefines)
		{
			soundClips[d.sound] = d.clip;
		}
	}


	public void PlaySfx(Sound sound, SoundPosition position = SoundPosition.Global)
	{
		if (soundClips.TryGetValue(sound, out AudioClip clip))
		{
			if (position == SoundPosition.Global)
			{
				bgmAudioSouce.PlayOneShot(clip);
			}
			else
			{
				//TDOO: 立体音: 玩家身上
			}
		}
		else
		{
			Debug.LogWarning("No such sound " + sound);
		}
	}


	public void PlayBgm(Sound sound)
	{
		if (soundClips.TryGetValue(sound, out AudioClip clip))
		{
			bgmAudioSouce.clip = clip;
			bgmAudioSouce.Play();
			//bgmAudioSouce.loop = true;  //prefab里设定
		}
		else
		{
			Debug.LogWarning("No such sound " + sound);
		}
	}

}

