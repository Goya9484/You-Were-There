using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManagement : MonoBehaviour
{
    public static AudioManagement audioInstance = null;

    [SerializeField]
    public AudioClip[] audios_BGM;
    public AudioClip[] audios_SFX;
    AudioSource source_BGM;
    AudioSource source_SFX;

    public void Awake()
    {
        if(audioInstance == null)
        {
            audioInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (audioInstance != this)
                Destroy(this.gameObject);
        }
    }
}
