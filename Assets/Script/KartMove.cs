using Fusion;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Windows;
using DG.Tweening;
using Cinemachine;


public class KartMove : NetworkBehaviour
{
    [SerializeField]
    private Rigidbody rigid;
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private TMP_Text nickNameText;
    [SerializeField]
    private CinemachineVirtualCamera VC;


    public GameUI hud;
    [SerializeField] NetworkMecanimAnimator nma;
    [SerializeField] Animator anim;
    [SerializeField] KartLapController kartLapController;

    [SerializeField] float maxMoveSpeed;
    [SerializeField] float normalRotationSpeed;
    [SerializeField] float maxReverseSpeed;
    [SerializeField] float driftRotationSpeed;
    [SerializeField] float curReverseSpeed;
    [SerializeField] float acceleration;
    [SerializeField] float runStartSpeed;

    [Networked] public float curMoveSpeed { get; set; }
    [Networked] public NetworkButtons PrevButtons { get; set; }

    [Networked(OnChanged = nameof(OnNickNameChanged))]
    public NetworkString<_16> NickName { get; set; }



    public float InterpolatedSpeed => speedInterpolator.Value;

    Interpolator<float> speedInterpolator;

    private NetworkButtons buttons;
    private NetworkButtons pressed;
    private NetworkButtons released;
    private Vector2 inputDir;
    private Vector3 moveDir;
    private Vector3 preMoveDir;

    public override void Spawned()
    {
        if (!Object.HasInputAuthority)
        {
            Destroy(VC.gameObject);
            Destroy(cam.gameObject);
            return;
        }

        cam.gameObject.SetActive(true);
        speedInterpolator = GetInterpolator<float>(nameof(curMoveSpeed));
        NickName = ClientInfo.Username;
        RPC_SendNickName(NickName);
    }
    public override void Render()
    {
        AnimatorController();
    }
    public override void FixedUpdateNetwork()
    {
        

        NetworkInputData input = Buttonssetting();

        transform.Rotate(0, input.yaw * normalRotationSpeed * Runner.DeltaTime, 0);

        /* + transform.right * inputDir.x*/
        ;

        if (kartLapController.isFinish == false)
        {
            moveDir = transform.forward * inputDir.y;
            if (buttons.IsSet(Buttons.drift))
            {
                Drift();
                return;
            }

            if (inputDir.y > 0)
            {

                if (curMoveSpeed < maxMoveSpeed)
                {
                    curMoveSpeed += acceleration;

                    if (curMoveSpeed >= maxMoveSpeed) { curMoveSpeed = maxMoveSpeed; }
                }

                rigid.velocity = new Vector3(moveDir.x * curMoveSpeed * Runner.DeltaTime, rigid.velocity.y, moveDir.z * curMoveSpeed * Runner.DeltaTime);
                preMoveDir = moveDir;
            }

            else if (inputDir.y == 0)
            {
                curMoveSpeed -= acceleration;
                curReverseSpeed -= acceleration;

                if (curMoveSpeed <= 0) { curMoveSpeed = 0; }
                if (curReverseSpeed <= 0) { curReverseSpeed = 0; }

                rigid.velocity = new Vector3(preMoveDir.x * curMoveSpeed * Runner.DeltaTime, rigid.velocity.y, preMoveDir.z * curMoveSpeed * Runner.DeltaTime);

            }
            else
            {
                if (curMoveSpeed > 0)
                {
                    curMoveSpeed -= 4 * acceleration;
                    rigid.velocity = new Vector3(preMoveDir.x * curMoveSpeed * Runner.DeltaTime,
                        rigid.velocity.y, preMoveDir.z * curMoveSpeed * Runner.DeltaTime);
                }
                else
                {
                    curReverseSpeed = Mathf.Lerp(curReverseSpeed, maxReverseSpeed, 0.1f * Runner.DeltaTime);
                    if (curReverseSpeed >= maxReverseSpeed) { curReverseSpeed = maxReverseSpeed; }
                    rigid.velocity = new Vector3(moveDir.x * curReverseSpeed * Runner.DeltaTime, rigid.velocity.y, moveDir.z * curReverseSpeed * Runner.DeltaTime);
                }
            }
        }
        else
        {
            curMoveSpeed = Mathf.Lerp(curMoveSpeed, 0, 0.5f * Runner.DeltaTime) ;
            rigid.velocity = new Vector3(transform.forward.x * curMoveSpeed* Runner.DeltaTime, rigid.velocity.y, transform.forward.z * curMoveSpeed * Runner.DeltaTime);
        }



        //transform.rotation = Quaternion.Euler(0, (float)input.yaw, 0);

    }
    void Drift()
    {
        NetworkInputData input = Buttonssetting();
        transform.Rotate(0, input.yaw * driftRotationSpeed * Runner.DeltaTime, 0);
        rigid.velocity = new Vector3(preMoveDir.x * curMoveSpeed * Runner.DeltaTime, rigid.velocity.y, preMoveDir.z * curMoveSpeed * Runner.DeltaTime);
    }
    private NetworkInputData Buttonssetting()
    {
        buttons = default;

        if (GetInput<NetworkInputData>(out var input))
        {
            buttons = input.buttons;
        }

        pressed = buttons.GetPressed(PrevButtons);
        released = buttons.GetReleased(PrevButtons);

        PrevButtons = buttons;

        inputDir = Vector2.zero;

        if (buttons.IsSet(Buttons.forward))
        {
            inputDir += Vector2.up;
        }
        if (buttons.IsSet(Buttons.back))
        {
            inputDir -= Vector2.up;
        }
        if (buttons.IsSet(Buttons.left))
        {
            inputDir += Vector2.left;
        }
        if (buttons.IsSet(Buttons.right))
        {
            inputDir += Vector2.right;
        }


        return input;
    }
    private void AnimatorController()
    {
        if (curMoveSpeed <= 0f)
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
        }
        else if (curMoveSpeed > 0 && curMoveSpeed < runStartSpeed)
        {
            anim.SetBool("Walk", true);
            anim.SetBool("Run", false);
        }
        else
        {
            anim.SetBool("Run", true);
            anim.SetBool("Walk", false);
        }

        anim.SetFloat("moveSpeed", curMoveSpeed / maxMoveSpeed);
    }
    public static void OnNickNameChanged(Changed<KartMove> changed)
    {
        changed.Behaviour.SetNickName();
    }
    public void SetNickName()
    {
        nickNameText.text = NickName.Value;
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.StateAuthority)] //sources는 InputAuthority권한을 가지고 있는 사람, target은 StateAuthority권한을 가지고 있는 사람에게 RPC보냄
    public void RPC_SendNickName(NetworkString<_16> message)
    {
        NickName = message;
    }
}
