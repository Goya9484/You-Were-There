using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRUIRay : MonoBehaviour
{
    public static VRUIRay instance;
    public Transform rightHand;
    public Transform dot;
    public AudioSource selectSound;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        Ray ray = new Ray(rightHand.position, rightHand.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10, 1<<LayerMask.NameToLayer("UI")))
        {
            dot.gameObject.SetActive(true);
            dot.position = hit.point;
        }
        else
            dot.gameObject.SetActive(false);

        if (dot.gameObject.activeSelf)
        {
            if (OVRInput.GetDown(OVRInput.Button.Any))
            {
                Button btn = hit.transform.GetComponent<Button>();
                if (btn != null)
                {
                    GameObject temp = Instantiate(Resources.Load("AudioSource") as GameObject);
                    selectSound = temp.GetComponent<AudioSource>();
                    selectSound.clip = AudioManagement.audioInstance.audios_SFX[0];
                    selectSound.Play();
                    StartCoroutine(Delay(btn, selectSound.clip.length));
                }
            }
        }
    }

    IEnumerator Delay(Button btn, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        btn.onClick.Invoke();
        Destroy(selectSound);
    }
}
