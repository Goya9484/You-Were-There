using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement sceneInstance = null;
    AudioSource Sound;
    bool isOpenScene;
    string tempName;

    public void Awake()
    {
        if (sceneInstance == null)
        {
            sceneInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (sceneInstance != this)
                Destroy(this.gameObject);
        }
    }

    public void Start()
    {
        isOpenScene = true;
        if (SceneManager.GetActiveScene().name == "00Title")
        {
            tempName = SceneManager.GetActiveScene().name;
            Sound = AudioManagement.audioInstance.GetComponent<AudioSource>();
            Sound.clip = AudioManagement.audioInstance.audios_BGM[0];
            Sound.Play();
            DontDestroyOnLoad(Sound);
        }
    }
    public void Update()
    {
        if (isOpenScene)
        {
            if (tempName == SceneManager.GetActiveScene().name)
                return;

            if (SceneManager.GetActiveScene().name == "00Title")
            {
                MusicPlay("00Title", 0);
                DontDestroyOnLoad(Sound);
            }
            if (SceneManager.GetActiveScene().name == "02Tutorial")
                MusicPlay("02Tutorial", 2);
            if (SceneManager.GetActiveScene().name == "03Stage01")
                MusicPlay("03Stage01", 3);
            if (SceneManager.GetActiveScene().name == "04Stage02")
                MusicPlay("04Stage02", 4);
            if (SceneManager.GetActiveScene().name == "05Stage03")
                MusicPlay("05Stage03", 5);
            if (SceneManager.GetActiveScene().name == "06Ending")
                MusicPlay("06Ending", 1);
            if (SceneManager.GetActiveScene().name == "Death")
                MusicPlay("Death", 6);
        }
    }

    public void MusicPlay(string SceneName, int AudioNum)
    {
        if (SceneManager.GetActiveScene().name == SceneName)
        {
            tempName = SceneManager.GetActiveScene().name;
            Sound = AudioManagement.audioInstance.GetComponent<AudioSource>();
            Sound.clip = AudioManagement.audioInstance.audios_BGM[AudioNum];
            Sound.Play();
            isOpenScene = false;
        }
    }
    public void Go_Title()
    {
        Sound.Stop();
        isOpenScene = true;
        SceneManager.LoadScene("00Title");
    }
    public void Go_Opening()
    {
        DialogTrigger.isDialog = true;
        SceneManager.LoadScene("01Opening");
    }
    public void Go_Tutorial()
    {
        Sound.Stop();
        isOpenScene = true;
        SceneManager.LoadScene("02Tutorial");
    }
    public void Go_Stage01()
    {
        Sound.Stop();
        isOpenScene = true;
        SceneManager.LoadScene("03Stage01");
    }
    public void Go_Stage02()
    {
        Sound.Stop();
        isOpenScene = true;
        SceneManager.LoadScene("04Stage02");
    }
    public void Go_Stage03()
    {
        Sound.Stop();
        isOpenScene = true;
        SceneManager.LoadScene("05Stage03");
    }
    public void Go_Ending()
    {
        Sound.Stop();
        isOpenScene = true;
        DialogTrigger.isDialog = true;
        SceneManager.LoadScene("06Ending");
    }
    public void Go_Death()
    {
        Sound.Stop();
        isOpenScene = true;
        SceneManager.LoadScene("Death");
    }
    public void Go_Quit()
    {
        Application.Quit();
    }
}
