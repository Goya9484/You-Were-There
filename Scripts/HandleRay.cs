using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleRay : MonoBehaviour
{
    public GameObject world;
    public Transform handR;
    public Transform handL;
    const float lineAngle = 180; //���ؼ�
    Vector3 handRtrans; //������ ��ġ��
    Vector3 handLtrans; //�޼� ��ġ��
    Vector3 pos; // ���� ������Ʈ ��ġ��

    public bool isFirst = true;

    void Update()
    {
        MapRotate();
    }

    public void MapRotate()
    {
        if (OVRInput.Get(OVRInput.Button.PrimaryHandTrigger) && OVRInput.Get(OVRInput.Button.SecondaryHandTrigger))
        {
            if(isFirst)
            {//�׷� ��ư�� ���������� �߽��� ���� ������Ʈ ��ġ�� �׻� ������ ������ ��ġ�ϰ� �ϱ� ���� ��
                transform.position = handR.position;
                isFirst = false;
            }

            handRtrans = new Vector3(handR.position.x, 0, handR.position.z);
            handLtrans = new Vector3(handL.position.x, 0, handL.position.z);
            pos = new Vector3(transform.position.x, 0, transform.position.z);

            float dot = Vector3.Dot((handLtrans - pos).normalized, (handRtrans - handLtrans).normalized);
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

            if (lineAngle - angle < 30)
            {
                if (transform.position.z > handRtrans.z)//��ȸ��
                    world.transform.Rotate(Vector3.up * 28 * Time.deltaTime);
                if (transform.position.z < handRtrans.z)//��ȸ��
                    world.transform.Rotate(Vector3.up * -28 * Time.deltaTime);
            }
        }

        if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger) || OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger))
            isFirst = true;
    }
}
