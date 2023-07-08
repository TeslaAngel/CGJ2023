using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitScript : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Cancel") >0)
        {
            //Application.Quit();
            SceneManager.LoadScene("level_select");
        }
    }
}
