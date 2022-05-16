using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gate : MonoBehaviour
{
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (SceneManager.GetActiveScene().name == "02Tutorial")
                //SceneManagement.sceneInstance.Go_Ending();
                SceneManagement.sceneInstance.Go_Stage01();
            if (SceneManager.GetActiveScene().name == "03Stage01")
                SceneManagement.sceneInstance.Go_Stage02();
            if (SceneManager.GetActiveScene().name == "04Stage02")
                SceneManagement.sceneInstance.Go_Stage03();
            if (SceneManager.GetActiveScene().name == "05Stage03")
                SceneManagement.sceneInstance.Go_Ending(); 
        }
    }
}
