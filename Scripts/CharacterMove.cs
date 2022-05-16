using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    //오브젝트, 컴포넌트 선언 관련
    public GameObject gameCharacter;
    public Animator animator;
    Rigidbody myRigid;
    OVRCameraRig headRig;
    AudioSource sound;

    //캐릭터 스탯 관련
    public float moveSpeed = 5.0f;
    public float jumpPower = 5.0f;
    public float rotateSpeed = 5.0f;

    //캐릭터 조작 관련
    Vector3 movement_Third;//3인칭 시점 상태에서의 캐릭터 이동 조작을 위한 vector3값
    Vector3 movement_First;//1인칭 시점 상태에서의 캐릭터 이동 조작을 위한 vector3값
    Vector3 originPosition = new Vector3(0, 2, 0);//캐릭터의 초기 위치값
    Quaternion originQuaternion = Quaternion.Euler(0, 0, 0);//캐릭터의 초기 rotation값
    float horizontalMove;//3인칭 시점에서 캐릭터 이동을 위한 horizontal 입력값
    float verticalMove;//3인칭 시점에서 캐릭터 이동을 위한 vertical 입력값
    float tempCam;//1인칭 시점에서 캐릭터 몸 회전을 위한 값

    //각종 플래그
    private bool isHeadCameraFlag;//1인칭 시점에서의 기능 on을 위한 플래그
    private bool isJumpFlag;//캐릭터의 다중점프 방지를 위한 플래그
    private bool isGround;//캐릭터의 점프를 위한 ground 확인용 플래그
    public static bool isViewFlag;//1인칭/3인칭 시점 변경용 플래그
    private bool isDeath;

    void Start()
    {
        animator = GetComponent<Animator>();
        myRigid = GetComponent<Rigidbody>();
        isGround = true;
        isViewFlag = false;
        isDeath = false;
        headRig = GameObject.FindWithTag("Head").GetComponent<OVRCameraRig>();
    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {//캐릭터-플레이어 시점 전환
            isViewFlag = !headRig.disableEyeAnchorCameras;
            headRig.disableEyeAnchorCameras = !headRig.disableEyeAnchorCameras;
        }

        if (isViewFlag)
        {//3인칭 시점 캐릭터 이동
            Vector2 coord = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);//왼쪽 컨트롤러 입력값 저장할 변수

            if (coord.x > 0.3f)
            {
                animator.SetBool("right", true);
                horizontalMove = 1;
            }
            if (coord.x < -0.3f)
            {
                animator.SetBool("left", true);
                horizontalMove = -1;
            }
            if (coord.x > -0.3f && coord.x < 0.3f)
            {
                animator.SetBool("right", false);
                animator.SetBool("left", false);
                horizontalMove = 0;
            }
            if (coord.y > 0.3f)
            {
                animator.SetBool("forward", true);
                verticalMove = 1;
            }
            if (coord.y < -0.3f)
            {
                animator.SetBool("back", true);
                verticalMove = -1;
            }
            if (coord.y > -0.3f && coord.y < 0.3f)
            {
                animator.SetBool("forward", false);
                animator.SetBool("back", false);
                verticalMove = 0;
            }
            Move_Third();
            Turn_Third();
        }
        if (!isViewFlag)
        {//1인칭 시점 캐릭터 이동
            Move_First();//3인칭 시점일때와 다른 이동 조작(1인칭 때는 다른 조작)
            Turn_First();//1인칭 시점에서 자연스러운 이동을 위한 캐릭터 회전
        }

        
    }

    void FixedUpdate()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {//캐릭터 점프
            if (isGround)
            {
                Jump();
                animator.SetBool("idle jump", true);
                isJumpFlag = true;
            }
        }
        else if (OVRInput.GetUp(OVRInput.Button.One))
            animator.SetBool("idle jump", false);
    }

    void Move_Third()
    {//3인칭 시점 이동 기능
        movement_Third.Set(horizontalMove, 0, verticalMove);
        movement_Third = movement_Third.normalized * moveSpeed * Time.deltaTime;
        myRigid.MovePosition(transform.position + movement_Third);
    }

    void Turn_Third()
    {//3인칭 시점 회전 기능
        if (horizontalMove == 0 && verticalMove == 0)
            return;

        Quaternion playerRotate = Quaternion.LookRotation(movement_Third);
        myRigid.rotation = Quaternion.Slerp(myRigid.rotation, playerRotate, rotateSpeed * Time.deltaTime);
    }

    void Move_First()
    {//1인칭 시점 이동 기능
        Vector2 coord = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        movement_First = new Vector3(coord.x, 0, coord.y);

        transform.Translate(movement_First * moveSpeed * Time.deltaTime);
    }

    void Turn_First()
    {//1인칭 시점 카메라 회전 기능
        Vector2 coord = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        myRigid.rotation = Quaternion.Euler(myRigid.rotation.eulerAngles.x,
            myRigid.rotation.eulerAngles.y + coord.x * rotateSpeed, myRigid.rotation.eulerAngles.z);
    }

    void Jump()
    {//점프 기능
        if (!isJumpFlag)
            return;

        GameObject temp = Instantiate(Resources.Load("AudioSource") as GameObject);
        sound = temp.GetComponent<AudioSource>();
        sound.clip = AudioManagement.audioInstance.audios_SFX[1];
        sound.Play();
        myRigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        isGround = false;
        isJumpFlag = false;
    }

    void OnCollisionEnter(Collision col)
    {
        //중복 점프 방지용 지면 확인
        if (col.gameObject.tag == "Ground")
            isGround = true;
            
        //닿으면 죽는 오브젝트 확인
        if(col.gameObject.tag == "DeathTrigger")
            isDeath = true;

        //봉인석 복구 애니메이션
        if (col.gameObject.tag == "SealStone")
        {
            col.gameObject.GetComponent<Animation>().Play();
            SealStone.isSealRestoration = true;
        }
    }

    void CharacterDeath()
    {
        if(gameObject.transform.position.y == -6 || isDeath)
            SceneManagement.sceneInstance.Go_Death();
    }
}
