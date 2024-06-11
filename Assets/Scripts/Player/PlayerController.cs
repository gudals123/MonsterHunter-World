using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerController : Controller
{
    public enum PlayerState
    {
        Idle,
        GetHit,
        Dead,
        Attack,
        Roll,
        Fall,
        Walk,
        Run,
        WeaponSheath
    }

    private Player player;
    private float walkSpeed = 4f;
    private float runSpeed = 7f;

    [SerializeField] private Transform cameraArm;

    //Pet pet
    public bool isRightAttack { get; private set; }
    public bool isRoll {  get; private set; }


    PlayerState playerState;

    public void Start()
    {
        player = GetComponent<Player>();
        playerState = PlayerState.Idle;
        moveSpeed = walkSpeed;
        isRoll = false;
    }


    public void Update()
    {
        InputSend();
        player.GroundCheck();
        Debug.Log(moveSpeed);
        LookAround();
    }

    public void InputSend()
    {

        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));


        //구르기
        if (Input.GetKeyDown(KeyCode.Space) && !isRoll)
        {
            isRoll = true;
            player.Roll();
            playerState = PlayerState.Roll;
            player.AnimatorControll(playerState);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isRoll = false;
        }
        //무기 스위치
        else if ((player.isArmed && Input.GetKeyDown(KeyCode.LeftShift)) ||
            (!player.isArmed && Input.GetMouseButtonDown(0)))
        {
            player.WeaponSwitch();
            playerState = PlayerState.WeaponSheath;
        }
        //달리기
        else if (moveInput != Vector2.zero && Input.GetKey(KeyCode.LeftShift) && !player.isArmed
            && player.animator.GetBool(PlayerAnimatorParamiter.IsSwitchDone))
        {
            moveSpeed = runSpeed;
            player.Move(moveSpeed, moveInput);
            playerState = PlayerState.Run;
            player.AnimatorControll(playerState);
        }
        //걷기
        else if (moveInput != Vector2.zero )
        {
            moveSpeed = walkSpeed;
            player.Move(moveSpeed, moveInput);
            playerState = PlayerState.Walk;
            player.AnimatorControll(playerState);
        }
        //정지
        else
        {
            playerState = PlayerState.Idle;
            player.AnimatorControll(playerState);
        }


    }

    private void LookAround()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 camAngle = cameraArm.rotation.eulerAngles;

        float x = camAngle.x - mouseDelta.y;

        if (x < 180f)
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335, 361);
        }

        cameraArm.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }
}
