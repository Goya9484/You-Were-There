using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ChatManagement : MonoBehaviour
{
    [SerializeField]
    public static ChatManagement chantInstance = null;
    public Queue<string> textStorage;
    public GameObject txtBox;

    public void Awake()
    {
        if (chantInstance == null)
        {
            chantInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (chantInstance != this)
                Destroy(this.gameObject);
        }
    }

    void Start()
    {
        textStorage = new Queue<string>();
    }

    public void StartDialog(Dialog_Opening dialogOpening)
    {
        txtBox = GameObject.Find("txtBox");
        foreach (string sentence in dialogOpening.opSentence)
        {
            textStorage.Enqueue(sentence);
        }
        DisplayNextDig();
    }

    public void StartDialog(Dialog_Ending dialogEnding)
    {
        txtBox = GameObject.Find("txtBox");
        foreach (string sentence in dialogEnding.edSentence)
        {
            textStorage.Enqueue(sentence);
        }
        DisplayNextDig();
    }

    public void DisplayNextDig()
    {
        if (textStorage.Count == 0)
        {
            EndDig();
        }
        string sentence = textStorage.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypingEffect(sentence));
    }

    IEnumerator TypingEffect(string sentece)
    {// 대사 타이핑 효과
        txtBox.GetComponent<TextMeshProUGUI>().text = "";
        foreach (char txt in sentece.ToCharArray())
        {
            txtBox.GetComponent<TextMeshProUGUI>().text += txt;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void EndDig()
    {
        if (SceneManager.GetActiveScene().name == "01Opening")
            SceneManagement.sceneInstance.Go_Tutorial();
        if (SceneManager.GetActiveScene().name == "06Ending")
            SceneManagement.sceneInstance.Go_Quit();
    }
}
