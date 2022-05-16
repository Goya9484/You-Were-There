using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;
using OVRTouchSample;

public class Freeze : MonoBehaviour
{
    public Transform originTransform;
    public GameObject sheild;
    public Vector3 originLocalEulerAngles;

    void Start()
    {
        originLocalEulerAngles = transform.localEulerAngles;
    }

    void Update()
    {
        if (!CharacterMove.isViewFlag)//캐릭터 시점이 될 경우
        {
            if (GetComponent<Hand>() != null)
            {
                GetComponent<Hand>().enabled = false;
            }
            sheild.SetActive(true);//발판 오브젝트를 활성화
            transform.parent = null;//이 때 플레이어 오브젝트의 자식으로 계속 되어있으면 끊임없이 움직일 수 있으니
                                    //부모 오브젝트를 null로 해주어 손 오브젝트의 마지막 Position과 Rotation을 고정화
        }
        else//플레이어 시점이 될 경우
        {
            if (GetComponent<Hand>() != null)
            {
                GetComponent<Hand>().enabled = true;
            }
            sheild.SetActive(false);//발판 오브젝트를 비활성화
            //손 오브젝트의 위치를 다시 플레이어 오브젝트의 자식으로 되돌림
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = originLocalEulerAngles;
            transform.parent = originTransform;
        }
    }
}
