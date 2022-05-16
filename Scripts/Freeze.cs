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
        if (!CharacterMove.isViewFlag)//ĳ���� ������ �� ���
        {
            if (GetComponent<Hand>() != null)
            {
                GetComponent<Hand>().enabled = false;
            }
            sheild.SetActive(true);//���� ������Ʈ�� Ȱ��ȭ
            transform.parent = null;//�� �� �÷��̾� ������Ʈ�� �ڽ����� ��� �Ǿ������� ���Ӿ��� ������ �� ������
                                    //�θ� ������Ʈ�� null�� ���־� �� ������Ʈ�� ������ Position�� Rotation�� ����ȭ
        }
        else//�÷��̾� ������ �� ���
        {
            if (GetComponent<Hand>() != null)
            {
                GetComponent<Hand>().enabled = true;
            }
            sheild.SetActive(false);//���� ������Ʈ�� ��Ȱ��ȭ
            //�� ������Ʈ�� ��ġ�� �ٽ� �÷��̾� ������Ʈ�� �ڽ����� �ǵ���
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = originLocalEulerAngles;
            transform.parent = originTransform;
        }
    }
}
