using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController_Botu : MonoBehaviour
{
    [SerializeField] PlayerInput Input;
    bool aButton, bButton;
    Vector2 lStick, rStick;

    [SerializeField]
    Camera playerCamera;

    float rotaSpeed = 100f;
    [SerializeField]
    float Speed  = 3.0f;

    float upLim = 315f;   //��p�x���
    float downLim = 45f;   //���p�x���

    bool Attack;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        if (Input == null) return;
        // �f���Q�[�g�o�^
        Input.onActionTriggered += OnAbutton;
        Input.onActionTriggered += OnBbutton;
        Input.onActionTriggered += OnLstick;
        Input.onActionTriggered += OnRstick;
    }

    private void OnDisable()
    {
        if (Input == null) return;
        // �f���Q�[�g�o�^����
        Input.onActionTriggered -= OnAbutton;
        Input.onActionTriggered -= OnBbutton;
        Input.onActionTriggered -= OnLstick;
        Input.onActionTriggered -= OnRstick;
    }
    private void OnAbutton(InputAction.CallbackContext context)
    {
        if (context.action.name != "Abutton") return;
        if (!context.performed)
        {
            aButton = false;
            return;  
        }
        Debug.Log("Press");
        var isButton = context.ReadValueAsButton();
        // ���͂�ێ�
        aButton = isButton;
    }
    private void OnBbutton(InputAction.CallbackContext context)
    {
        if (context.action.name != "Bbutton") return;
        // Action�̓��͒l���擾
        var isButton = context.ReadValueAsButton();
        // ���͂�ێ�
        bButton = isButton;
    }
    private void OnLstick(InputAction.CallbackContext context)
    {
        if (context.action.name != "Lstick") return;
        // Action�̓��͒l���擾
        var isStick = context.ReadValue<Vector2>();
        // ���͂�ێ�
        lStick = isStick;
    }
    private void OnRstick(InputAction.CallbackContext context)
    {
        if (context.action.name != "Rstick") return;
        // Action�̓��͒l���擾
        var isStick = context.ReadValue<Vector2>();
        // ���͂�ێ�
        rStick = isStick;
    }

    // Update is called once per frame
    private void Update()
    {
        isAttack();
        isMovePlayer();
        //isMoveCamera();
    }
    private void FixedUpdate()
    {
        Debug.Log("Time.timeScale = 999");
    }
    void isMovePlayer()
    {
        Vector3 velocity = new Vector3(lStick.x, 0, lStick.y).normalized * Speed * Time.unscaledDeltaTime;
        this.transform.Translate(velocity);

        //Vector3 rotate = new Vector3(rStick.x, rStick.y, 0) * rotaSpeed * Time.deltaTime;
       
        //transform.Rotate(-rotate.y, rotate.x, 0);
    }
    void isMoveCamera()
    {
        Vector3 rotation = new Vector3(rStick.x, rStick.y, 0) * rotaSpeed * Time.deltaTime;

        if (rotation.x > downLim)
        {
            if (rotation.x > 180f)
            {
                if (upLim > rotation.x)
                {
                    rotation.x = upLim;
                }
            }
            else
            {
                rotation.x = downLim;
            }
        }
        playerCamera.transform.RotateAround(transform.position, Vector3.up, -rStick.x);
        playerCamera.transform.RotateAround(transform.position, Vector3.right, rStick.y);

    }
    void isAttack()
    {
        if (bButton)
        {
            Attack = true;
            Time.timeScale = 0;
        }
        else
        {
            Attack = false;
            Time.timeScale = 1;
            Speed = 5f;
        }
    }
}
