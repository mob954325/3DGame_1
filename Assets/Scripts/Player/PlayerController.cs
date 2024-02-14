using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 플레이어 인풋 관련 행동을 다루는 클래스
/// </summary>
public class PlayerController : MonoBehaviour
{
    // Action
    /// <summary>
    /// 적한테 공격을 하는지 확인하는 델리게이트
    /// </summary>
    Action OnPlayerAttackToEnemy;
    /// <summary>
    /// 플레이어가 패링을 성공 했을 때 실행하는 델리게이트
    /// </summary>
    Action OnPlayerParrying;

    public Action OnInteractionAction; // 인터렉션 델리게이트

    // components
    PlayerInputActions actions;
    Animator animator;
    Rigidbody rigid;
    public EnemyBase enemy;

    // player input values
    public Vector3 playerInput;
    public Vector2 mouseInput;

    // player Stats
    public float moveSpeed = 5.0f;
    public float rotSpeed = 0.3f;
    [Range(0f,1f)]
    public float rotationPower = 5.0f; // 플레이어 회전력
    public float jumpPower = 5.0f; // 점프력
    public float attackDelayTime = 3.0f; // 공격 딜레이 시간
    const float defenceAnimTime = 2.0f; // 방밀 애니메이션 딜레이 시간
    public float defenceDelayTime = 3.0f; // 방어 딜레이 시간

    // player's Transform objects
    public Transform cameraFollowTransform;
    public Transform playerModel;

    public float rotationLerp = 1.2f;

    // player movement
    [Header("Movement")]
    Vector3 moveDirection;
    float inputVertical = 0f;
    float inputHorizontal = 0f;

    // player animator
    readonly int inputVerticalToHash = Animator.StringToHash("Vertical"); // input.z
    readonly int inputHorizontalToHash = Animator.StringToHash("Horizontal"); // input.x
    readonly int jumpToHash = Animator.StringToHash("Jump");
    readonly int attackToHash = Animator.StringToHash("Attack");
    readonly int damagedToHash = Animator.StringToHash("Damaged");
    readonly int defenceToHash = Animator.StringToHash("isDefence");
    readonly int ActiveDefenceToHash = Animator.StringToHash("ActiveDefence");
    readonly int DieToHash = Animator.StringToHash("Die"); // ?

    // player flag
    bool isJump = false;
    bool isAttack = false;

    bool isDefence = false; // 플레이어가 방어 자세를 하려는지 확인하는 flag
    bool canDefence = false; // 플레이어가 방어를 성공할 수 있는지 확인하는 flag (계속 방어를 하고 있는 중)
    bool defenceDelayActive = false; // 플레이어가 방어를 할 수 있는지 확인하는 flag 변수 ( true : 방어 못함, false : 방어 가능) 

    bool isLockOn = false; // 플레이어가 락온을 활성화 했는지 확인하는 flag
    float checkEnemyAngle = 0f;

    [SerializeField] bool canInteraction = false; // 플레이어가 상호작용이 가능한지 확인하는 flag


    void Awake()
    {
        actions = new PlayerInputActions();
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        enemy = FindAnyObjectByType<EnemyBase>();

        playerModel = transform.GetChild(0);

        gameObject.GetComponent<Player>().onDamaged += () => OnDamagedAnimation(); // 피격 델리게이트
        OnPlayerAttackToEnemy += () => enemy.Enemy_ChangeAttackFlag();
        OnPlayerParrying += () => enemy.CheckParrying();
    }

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked; // 커서 고정
        //Cursor.visible = false; // 커서 가리기

        rigid.freezeRotation = true;
    }

    void OnEnable()
    {
        actions.Player.Enable();
        actions.Player.Move.performed += OnMoveInput;
        actions.Player.Move.canceled += OnMoveInput;
        actions.Player.Jump.performed += OnJumpInput;
        actions.Player.Jump.canceled += OnJumpInput;
        actions.Player.Look.performed += OnLookInput;
        actions.Player.Look.canceled += OnLookInput;
        actions.Player.Attack.performed += OnAttackInput;
        actions.Player.Attack.canceled += OnAttackInput;
        actions.Player.Defence.performed += OnDefenceInput;
        actions.Player.Defence.canceled += OnDefenceInput;
        actions.Player.LockOn.performed += OnLockCameraInput;
        actions.Player.LockOn.canceled += OnLockCameraInput;
        actions.Player.Interaction.performed += OnInteraction;
        actions.Player.Interaction.canceled += OnInteraction;
    }

    private void OnInteraction(InputAction.CallbackContext context)
    {
        if(context.performed && canInteraction)
        {
            GameUIManager.Instance.infoPanel.GetComponent<UI_Info>().ActiveUI();
            OnInteractionAction?.Invoke();
        }
    }

    void OnDisable()
    {
        actions.Player.Interaction.canceled -= OnInteraction;
        actions.Player.Interaction.performed -= OnInteraction;
        actions.Player.LockOn.canceled -= OnLockCameraInput;
        actions.Player.LockOn.performed -= OnLockCameraInput;
        actions.Player.Defence.canceled -= OnDefenceInput;
        actions.Player.Defence.performed -= OnDefenceInput;
        actions.Player.Attack.canceled -= OnAttackInput;
        actions.Player.Attack.performed -= OnAttackInput;
        actions.Player.Look.canceled -= OnLookInput;
        actions.Player.Look.performed -= OnLookInput;
        actions.Player.Jump.canceled -= OnJumpInput;
        actions.Player.Jump.performed -= OnJumpInput;
        actions.Player.Move.canceled -= OnMoveInput;
        actions.Player.Move.performed -= OnMoveInput;
        actions.Player.Disable();
    }

    void FixedUpdate()
    {
        playerMove();
        GetPlayerMoveInput();
        PlayerRotate();
        rotateCamera();

        PlayAnimMove();

        // 카메라 락온
        if(isLockOn)
            cameraFollowTransform.LookAt(enemy.transform);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isJump = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // check interaction Object
        if (other.CompareTag("Interaction"))
        {
            GameUIManager.Instance.infoPanel.GetComponent<UI_Info>().targetObj = other.gameObject;
            canInteraction = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // check interaction Object
        if (other.CompareTag("Interaction"))
        {
            canInteraction = false;
            GameUIManager.Instance.infoPanel.GetComponent<UI_Info>().gameObject.SetActive(false);
        }
    }

    private void OnLookInput(InputAction.CallbackContext context)
    {
        mouseInput = context.ReadValue<Vector2>();
    }

    private void OnMoveInput(InputAction.CallbackContext context)
    {
        playerInput = context.ReadValue<Vector3>();
    }

    void GetPlayerMoveInput()
    {
        // CALL AFTER playerMove()
        inputVertical = playerInput.z;
        inputHorizontal = playerInput.x;
    }

    private void OnJumpInput(InputAction.CallbackContext context)
    {
        if(!isJump)
        {
            animator.SetTrigger(jumpToHash);
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isJump = true;
        }
    }

    void rotateCamera()
    {
        if(isLockOn)
            return;

        #region Vertical Rotation

        // Rotate the follow target transfrom based on input

        // leftright
        cameraFollowTransform.transform.rotation *= Quaternion.AngleAxis(mouseInput.x * rotationPower, Vector3.up);

        // updown
        cameraFollowTransform.transform.rotation *= Quaternion.AngleAxis(mouseInput.y * -rotationPower, Vector3.right);

        Vector3 angles = cameraFollowTransform.transform.localEulerAngles;
        angles.z = 0;

        float angle = cameraFollowTransform.transform.localEulerAngles.x;

        // Clamp the up/down rotation
        if(angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if(angle < 180 && angle > 40)
        {
            angles.x = 40;
        }

        cameraFollowTransform.transform.localEulerAngles = angles;

        #endregion

        // Reset the y rotation of the look transform
        cameraFollowTransform.transform.localEulerAngles = new Vector3(angles.x, angles.y, 0);

    }

    /// <summary>
    /// player model rotate
    /// </summary>
    void PlayerRotate()
    {
        if (isDefence)
            return;

        Vector3 rotDirection = Vector3.zero;
        rotDirection.x = inputHorizontal;
        rotDirection.z = inputVertical;
        rotDirection.Normalize(); // 회전 방향 백터

        // 입력키 기준 모델 회전값 + 카메라 회전값 = 실제 플레이어 모델이 회전할 y값
        if(!isLockOn)
        {
            if(rotDirection.magnitude > 0.01f)
            {
                float lookAngle = Mathf.Atan2(rotDirection.x, rotDirection.z) * Mathf.Rad2Deg; // 회전할 방향
                float lerpLookAngle = Mathf.LerpAngle(playerModel.localRotation.eulerAngles.y, 
                                                      lookAngle + cameraFollowTransform.rotation.eulerAngles.y, 
                                                      rotSpeed * Time.fixedDeltaTime);

                playerModel.localRotation = Quaternion.Euler(0, lerpLookAngle, 0); // rotate Player model
            }

        }
        else if(isLockOn)
        {
            // 카메라 방향 기준 모델 회전
            float angle = Mathf.LerpAngle(playerModel.localRotation.eulerAngles.y, cameraFollowTransform.rotation.eulerAngles.y, rotSpeed * Time.fixedDeltaTime);
            playerModel.localRotation = Quaternion.Euler(0, angle, 0); // rotate Player model
        }
    }

    void playerMove()
    {
        if (isDefence)
            return;

        // calculate movement direction
        moveDirection = cameraFollowTransform.forward * inputVertical + cameraFollowTransform.right * inputHorizontal; // Player Move Direction
        moveDirection.y = 0f;

        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * moveDirection.normalized * moveSpeed); // player Move position

        playerModel.position = transform.position + moveDirection.normalized * Time.fixedDeltaTime;
    }

    void PlayAnimMove()
    {
        // check input value
        animator.SetFloat(inputVerticalToHash, inputVertical);
        animator.SetFloat(inputHorizontalToHash, inputHorizontal);
    }

    private void OnAttackInput(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(!isAttack)
            {
                animator.SetTrigger(attackToHash);
                StartCoroutine(AttackDelay());
            }
        }
    }

    IEnumerator AttackDelay()
    {
        isAttack = true;
        OnPlayerAttackToEnemy?.Invoke();
        yield return new WaitForSeconds(attackDelayTime);
        isAttack = false;
        OnPlayerAttackToEnemy?.Invoke();
    }

    void OnDamagedAnimation()
    {
        animator.SetTrigger(damagedToHash);
    }

    // Defence
    private void OnDefenceInput(InputAction.CallbackContext context)
    {
        if (context.performed && !defenceDelayActive)
        {
            defenceDelayActive = true; 

            // Set animator paramaters
            isDefence = true;
            CheckisDefence();

            animator.SetTrigger(ActiveDefenceToHash);
            animator.SetBool(defenceToHash, isDefence);

            // check Enemay Attack Angle
            checkEnemyAngle = Vector3.SignedAngle(playerModel.transform.forward, enemy.transform.forward, transform.up);
            //Debug.Log(checkEnemyAngle);
            if (checkEnemyAngle >= -180 && checkEnemyAngle <= -90 || checkEnemyAngle <= 180 && checkEnemyAngle >= 90) // 플레이어가 적을 바라보고 있으면 방어 가능
            {
                canDefence = true;
            }
            else
            {
                canDefence = false;
            }

            CheckCanDefence();
            StartCoroutine(DefenceDelay()); // 방어 딜레이 코루틴
        }
        if (context.canceled)
        {
            StartCoroutine(AfterDefence());
        }
    }

    /// <summary>
    /// Player에게 canDefence flage boradcasting
    /// </summary>
    void CheckCanDefence()
    {
        BroadcastMessage("GetCanDefence", canDefence);
    }

    /// <summary>
    /// Player에게 isDefence flage boradcasting
    /// </summary>
    void CheckisDefence()
    {
        BroadcastMessage("GetIsDefence", isDefence);
    }

    /// <summary>
    /// 방어 무적 판정 시간 코루틴(Defencing attack + 0.5f)
    /// </summary>
    /// <returns></returns>
    IEnumerator AfterDefence()
    {
        animator.SetBool(defenceToHash, false);

        OnPlayerParrying?.Invoke(); // 방패 밀치기 중에 패링을 할 수 있는지 확인

        isDefence = false; // 방패 밀치기 애니메이션 시작
        yield return new WaitForSeconds(defenceAnimTime);

        canDefence = false; // 방어 못함

        // Broadcast
        CheckisDefence();
        CheckCanDefence();
    }

    IEnumerator DefenceDelay()
    {
        yield return new WaitForSecondsRealtime(defenceDelayTime);
        defenceDelayActive = false;
    }

    private void OnLockCameraInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isLockOn = !isLockOn;
        }
    }
}
