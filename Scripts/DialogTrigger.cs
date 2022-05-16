using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogTrigger : MonoBehaviour
{
    public ChatManagement chatManagement;
    public Dialog_Opening dialog_Opening;
    public Dialog_Ending dialog_Ending;
    public static bool isDialog;

    public void DigTrigger(Dialog_Opening digOp)
    {
        FindObjectOfType<ChatManagement>().StartDialog(digOp);
    }

    public void DigTrigger(Dialog_Ending digEd)
    {
        FindObjectOfType<ChatManagement>().StartDialog(digEd);
    }

    public void Start()
    {
        isDialog = true;
    }

    public void Update()
    {
        if(isDialog)
        {
            if (SceneManager.GetActiveScene().name == "01Opening")
            {
                DigTrigger(dialog_Opening);
                isDialog = false;
            }
            if (SceneManager.GetActiveScene().name == "06Ending")
            {
                DigTrigger(dialog_Ending);
                isDialog = false;
            }
        }
        

        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            if (SceneManager.GetActiveScene().name == "01Opening" || SceneManager.GetActiveScene().name == "06Ending")
                chatManagement.DisplayNextDig();
        }
    }
}
