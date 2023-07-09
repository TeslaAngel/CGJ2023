using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMap : MonoBehaviour
{
	public Transform playerSpawnPoint;
	public Transform treasurePoint;
    [Space]
    public List<GameObject> Ghosts;
    public List<GameObject> FakeMountains;

    public int maxHandPrintCount = 8;
}
