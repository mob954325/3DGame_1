using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// �÷��̾� ��ǲ ���� �ൿ�� �ٷ�� Ŭ����
/// </summary>
public class Player : MonoBehaviour
{
    // Action
    /// <summary>
    /// �÷��̾ ������ �� �����ϴ� ��������Ʈ
    /// </summary>
    Action OnAttack;
    /// <summary>
    /// �÷��̾ �и��� ���� ���� �� �����ϴ� ��������Ʈ
    /// </summary>
    Action OnPlayerParrying;
    /// <summary>
    /// �÷��̾� ���ͷ��� ��������Ʈ
    /// </summary>
    public Action OnInteractionAction;

    // components
    PlayerInputActions actions;
    Animator animator;
    Rigidbody rigid;
    WeaponControl weapon;
    public HSEnemy enemy;

    // player input values
    public Vector3 playerInput;
    public Vector2 mouseInput;

    // player Stats
    float baseSpeed; // �÷��̾� ���ǵ� ���� ����
    public float moveSpeed = 5.0f; // �÷��̾��� ���� ���ǵ� ����
    public float rotSpeed = 15f; // �÷��̾� ȸ�� �ӵ�
    [Range(0f,1f)]
    public float rotationPower = 5.0f; // �÷��̾� ȸ����
    public float jumpPower = 5.0f; // ������
    [Range(1.5f,3f)]
    public float attackDelayTime = 3.0f; // ���� ������ �ð�
    const float defenceAnimTime = 2.0f; // ��� �ִϸ��̼� ������ �ð�
    public float defenceDelayTime = 3.0f; // ��� ������ �ð�

    // Hp
    public int maxhp = 5;
    int hp;
    public int HP // �÷��̾� ü�� ������Ƽ
    {
        get => hp;
        set
        {
            hp = value;
            //Debug.Log($"�÷��̾��� ü���� [{hp}]��ŭ ���ҽ��ϴ�");

            if (hp < 0)
            {
                hp = 0;
                Die();
            }
        }
    }

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
    bool isDamaged = false;
    bool isDefence = false; // �÷��̾ ��� �ڼ��� �Ϸ����� Ȯ���ϴ� flag
    bool defenceDelayActive = false; // �÷��̾ �� �� �� �ִ��� Ȯ���ϴ� flag ���� ( true : ��� ����, false : ��� ����) 

    bool isLockOn = false; // �÷��̾ ������ Ȱ��ȭ �ߴ��� Ȯ���ϴ� flag
    float checkEnemyAngle = 0f;

    [SerializeField] bool canInteraction = false; // �÷��̾ ��ȣ�ۿ��� �������� Ȯ���ϴ� flag


    void Awake()
    {
        // ������Ʈ
        actions = new PlayerInputActions();
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        enemy = FindAnyObjectByType<HSEnemy>();
        weapon = GetComponentInChildren<WeaponControl>();
        cameraFollowTransform = FindAnyObjectByType<FollowCamera>().transform;
        playerModel = transform.GetChild(0);

        // ���� �ʱ�ȭ
        HP = maxhp;
        baseSpeed = moveSpeed;

        // ��������Ʈ
        OnAttack += weapon.ChangeColliderEnableState;
        //OnPlayerParrying += () => enemy.CheckParrying();
    }

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked; // Ŀ�� ����
        //Cursor.visible = false; // Ŀ�� ������

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
            //GameUIManager.Instance.infoPanel.GetComponent<UI_Info>().ActiveUI();
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

        // ī�޶� ����
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
        // �� ���� ����
        if (other.gameObject.CompareTag("EnemyAttack") && !isDamaged)
        {
            animator.SetTrigger(damagedToHash);
            StartCoroutine(HitDelay());
        }

        // check interaction Object
        if (other.CompareTag("Interaction"))
        {
            //GameUIManager.Instance.info.targetObj = other.gameObject; // Ÿ�� ������Ʈ ����
            canInteraction = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // check interaction Object
        if (other.CompareTag("Interaction"))
        {
            canInteraction = false;
            //GameUIManager.Instance.infoPanel.GetComponent<UI_Info>().gameObject.SetActive(false);
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
        rotDirection.Normalize(); // ȸ�� ���� ����

        // �Է�Ű ���� �� ȸ���� + ī�޶� ȸ���� = ���� �÷��̾� ���� ȸ���� y��
        if(!isLockOn)
        {
            if(rotDirection.magnitude > 0.01f)
            {
                float lookAngle = Mathf.Atan2(rotDirection.x, rotDirection.z) * Mathf.Rad2Deg; // ȸ���� ����
                float lerpLookAngle = Mathf.LerpAngle(playerModel.localRotation.eulerAngles.y, 
                                                      lookAngle + cameraFollowTransform.rotation.eulerAngles.y, 
                                                      rotSpeed * Time.fixedDeltaTime);

                playerModel.localRotation = Quaternion.Euler(0, lerpLookAngle, 0); // rotate Player model
            }

        }
        else if(isLockOn)
        {
            // ī�޶� ���� ���� �� ȸ��
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
                moveSpeed = 0f;
                animator.SetTrigger(attackToHash);
                StartCoroutine(AttackDelay());
            }
        }
    }

    IEnumerator AttackDelay()
    {
        isAttack = true;
        OnAttack?.Invoke();

        float time = GetAnimClipLength("Player_Attack"); // �ִϸ��̼� �ð� �ޱ�
        yield return new WaitForSeconds(time);

        OnAttack?.Invoke();
        isAttack = false;

        moveSpeed = baseSpeed;
    }

    // Defence
    private void OnDefenceInput(InputAction.CallbackContext context)
    {
        if (context.performed && !defenceDelayActive)
        {
            defenceDelayActive = true; 

            // Set animator paramaters
            isDefence = true;

            animator.SetTrigger(ActiveDefenceToHash);
            animator.SetBool(defenceToHash, isDefence);

            // check Enemay Attack Angle
            checkEnemyAngle = Vector3.SignedAngle(playerModel.transform.forward, enemy.transform.forward, transform.up);
            if (checkEnemyAngle >= -180 && checkEnemyAngle <= -90 || checkEnemyAngle <= 180 && checkEnemyAngle >= 90) // �÷��̾ ���� �ٶ󺸰� ������ ��� ����
            {
                //canDefence = true;
            }
            else
            {
               //canDefence = false;
            }

            StartCoroutine(DefenceDelay()); // ��� ������ �ڷ�ƾ
        }
        if (context.canceled)
        {
            StartCoroutine(AfterDefence());
        }
    }

    /// <summary>
    /// ��� ���� ���� �ð� �ڷ�ƾ(Defencing attack + 0.5f)
    /// </summary>
    /// <returns></returns>
    IEnumerator AfterDefence()
    {
        animator.SetBool(defenceToHash, false);

        OnPlayerParrying?.Invoke(); // ���� ��ġ�� �߿� �и��� �� �� �ִ��� Ȯ��

        isDefence = false; // ���� ��ġ�� �ִϸ��̼� ����
        yield return new WaitForSeconds(defenceAnimTime);

        //canDefence = false; // ��� ����
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

    IEnumerator HitDelay()
    {
        isDamaged = true;

        HP--;
        yield return new WaitForSeconds(2f);
        isDamaged = false;
    }

    /// <summary>
    /// �÷��̾� ��� �Լ�
    /// </summary>
    void Die()
    {
        animator.SetTrigger("Die");
    }

    /// <summary>
    /// Ư�� �ִϸ��̼� �ð��� ã�� �Լ�
    /// </summary>
    /// <param name="clipName">ã�� �ִϸ��̼� �̸�</param>
    /// <returns>clipName �ִϸ��̼� �ð�</returns>
    float GetAnimClipLength(string clipName)
    {
        float time = 0;
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;

        for(int i = 0; i < ac.animationClips.Length; i++)
        {
            if(ac.animationClips[i].name == clipName)
            {
                time = ac.animationClips[i].length;
            }
        }
        return time;
    }
}
