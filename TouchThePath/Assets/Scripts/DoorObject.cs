using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorObject : MonoBehaviour
{
	//public float selectedScale = 1.5f;
	public int Index;
	public string levelMapName;
	public LevelSelectController selectController;
	public GameObject PortalEffect;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
			//Door: Call desinated scene switch
			selectController.toScene(Index, levelMapName);
			print("to");
        }
    }

    private void Start()
    {
        if (Index != UnlockedLevels.LevelUnlocked)
        {

            Destroy(GetComponent<DoorObject>());
        }
        else
        {
            Instantiate(PortalEffect, transform.position, transform.rotation);
        }
    }
}
