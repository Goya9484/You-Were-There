using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapZoomController : MonoBehaviour
{
    public Transform lControllerPos;
    public Transform rControllerPos;
    public GameObject map;
    private Vector3 leftOriginPos;
    private Vector3 rightOriginPos;

    void Update()
    {
        DoubbleTriggerButtonOn();
        MapZoomInOut();
    }

    public void DoubbleTriggerButtonOn()
    {
        //각 컨트롤러 트리거 버튼 누를시 현재 컨트롤러 위치를 저장
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
            leftOriginPos = lControllerPos.position;
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
            rightOriginPos = rControllerPos.position;
    }

    public void MapZoomInOut()
    {
        //각 컨트롤러 트리거 버튼을 누른채 컨트롤러 위치 변화를 감지
        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger) &&
            OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
        {
                if (lControllerPos.position.x > leftOriginPos.x && rControllerPos.position.x > rightOriginPos.x)
                    map.transform.Translate(3 * Time.deltaTime, 0, 0);
                else
                    map.transform.Translate(-3 * Time.deltaTime, 0, 0);
                if (lControllerPos.position.y > leftOriginPos.y && rControllerPos.position.y > rightOriginPos.y)
                    map.transform.Translate(0, 3 * Time.deltaTime, 0);
                else
                    map.transform.Translate(0, -3 * Time.deltaTime, 0);
                if (lControllerPos.position.z < leftOriginPos.z && rControllerPos.position.z < rightOriginPos.z)
                    map.transform.Translate(0, 0, -3 * Time.deltaTime);
                else
                    map.transform.Translate(0, 0, 3 * Time.deltaTime);
        }
    }
}
