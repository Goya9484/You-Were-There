using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    //������Ʈ, ������Ʈ ���� ����
    public GameObject gameCharacter;
    public Animator animator;
    Rigidbody myRigid;
    OVRCameraRig headRig;
    AudioSource sound;

    //ĳ���� ���� ����
    public float moveSpeed = 5.0f;
    public float jumpPower = 5.0f;
    public float rotateSpeed = 5.0f;

    //ĳ���� ���� ����
    Vector3 movement_Third;//3��Ī ���� ���¿����� ĳ���� �̵� ������ ���� vector3��
    Vector3 movement_First;//1��Ī ���� ���¿����� ĳ���� �̵� ������ ���� vector3��
    Vector3 originPosition = new Vector3(0, 2, 0);//ĳ������ �ʱ� ��ġ��
    Quaternion originQuaternion = Quaternion.Euler(0, 0, 0);//ĳ������ �ʱ� rotation��
    float horizontalMove;//3��Ī �������� ĳ���� �̵��� ���� horizontal �Է°�
    float verticalMove;//3��Ī �������� ĳ���� �̵��� ���� vertical �Է°�
    float tempCam;//1��Ī �������� ĳ���� �� ȸ���� ���� ��

    //���� �÷���
    private bool isHeadCameraFlag;//1��Ī ���������� ��� on�� ���� �÷���
    private bool isJumpFlag;//ĳ������ �������� ������ ���� �÷���
    private bool isGround;//ĳ������ ������ ���� ground Ȯ�ο� �÷���
    public static bool isViewFlag;//1��Ī/3��Ī ���� ����� �÷���
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
        {//ĳ����-�÷��̾� ���� ��ȯ
            isViewFlag = !headRig.disableEyeAnchorCameras;
            headRig.disableEyeAnchorCameras = !headRig.disableEyeAnchorCameras;
        }

        if (isViewFlag)
        {//3��Ī ���� ĳ���� �̵�
            Vector2 coord = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);//���� ��Ʈ�ѷ� �Է°� ������ ����

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
        {//1��Ī ���� ĳ���� �̵�
            Move_First();//3��Ī �����϶��� �ٸ� �̵� ����(1��Ī ���� �ٸ� ����)
            Turn_First();//1��Ī �������� �ڿ������� �̵��� ���� ĳ���� ȸ��
        }

        
    }

    void FixedUpdate()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {//ĳ���� ����
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
    {//3��Ī ���� �̵� ���
        movement_Third.Set(horizontalMove, 0, verticalMove);
        movement_Third = movement_Third.normalized * moveSpeed * Time.deltaTime;
        myRigid.MovePosition(transform.position + movement_Third);
    }

    void Turn_Third()
    {//3��Ī ���� ȸ�� ���
        if (horizontalMove == 0 && verticalMove == 0)
            return;

        Quaternion playerRotate = Quaternion.LookRotation(movement_Third);
        myRigid.rotation = Quaternion.Slerp(myRigid.rotation, playerRotate, rotateSpeed * Time.deltaTime);
    }

    void Move_First()
    {//1��Ī ���� �̵� ���
        Vector2 coord = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
        movement_First = new Vector3(coord.x, 0, coord.y);

        transform.Translate(movement_First * moveSpeed * Time.deltaTime);
    }

    void Turn_First()
    {//1��Ī ���� ī�޶� ȸ�� ���
        Vector2 coord = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        myRigid.rotation = Quaternion.Euler(myRigid.rotation.eulerAngles.x,
            myRigid.rotation.eulerAngles.y + coord.x * rotateSpeed, myRigid.rotation.eulerAngles.z);
    }

    void Jump()
    {//���� ���
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
        //�ߺ� ���� ������ ���� Ȯ��
        if (col.gameObject.tag == "Ground")
            isGround = true;
            
        //������ �״� ������Ʈ Ȯ��
        if(col.gameObject.tag == "DeathTrigger")
            isDeath = true;

        //���μ� ���� �ִϸ��̼�
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
