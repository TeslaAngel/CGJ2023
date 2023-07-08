using System;
using System.Collections;
using UnityEngine;


[CreateAssetMenu(fileName = "SoundSheet", menuName = "Sound Sheet")]
public class SoundSheet : ScriptableObject
{
	[Serializable]
	public struct SoundDefine
	{
		public Sound sound;
		public AudioClip clip;
	}

	public SoundDefine[] soundDefines;
}
