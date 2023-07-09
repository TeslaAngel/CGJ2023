using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayerForPlayer : MonoBehaviour
{
    public void PlayInhaleAudio()
    {
        AudioManager.Instance.PlaySfx(Sound.PlayerInhale);
    }

    public void PlayPunchAudio()
    {
        AudioManager.Instance.PlaySfx(Sound.PlayerBeingHit);
    }

    /*
    public void PlayWalkAudio1()
    {
        AudioManager.Instance.PlaySfx(Sound.PlayerWalk1);
    }
    */

    public void PlayWalkAudio2()
    {
        AudioManager.Instance.PlaySfx(Sound.PlayerWalk2);
    }
}
