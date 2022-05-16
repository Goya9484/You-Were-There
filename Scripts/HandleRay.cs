using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleRay : MonoBehaviour
{
    public GameObject world;
    public Transform handR;
    public Transform handL;
    const float lineAngle = 180; //기준선
    Vector3 handRtrans; //오른손 위치값
    Vector3 handLtrans; //왼손 위치값
    Vector3 pos; // 현재 오브젝트 위치값

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
            {//그랩 버튼을 누를때마다 중심점 역할 오브젝트 위치를 항상 오른손 옆으로 위치하게 하기 위한 식
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
                if (transform.position.z > handRtrans.z)//우회전
                    world.transform.Rotate(Vector3.up * 28 * Time.deltaTime);
                if (transform.position.z < handRtrans.z)//좌회전
                    world.transform.Rotate(Vector3.up * -28 * Time.deltaTime);
            }
        }

        if (OVRInput.GetUp(OVRInput.Button.PrimaryHandTrigger) || OVRInput.GetUp(OVRInput.Button.SecondaryHandTrigger))
            isFirst = true;
    }
}
