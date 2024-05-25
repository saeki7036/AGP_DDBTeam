using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] PlayerInput Input;
    [SerializeField] GameObject system;
    bool aButton, bButton;
    Vector2 lStick, rStick;

    Rigidbody rb;
    GameSystemClass game;

    float Speed  = 1.0f;
    public float NomalSpeed = 4f;
    public float AttackSpeed = 3f;
    public float NoAttackSpeed = 1.5f;

    bool Attack;
    // Start is called before the first frame update
    void Start()
    {
        game = system.GetComponent<GameSystemClass>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (Input == null) return;
        // ÉfÉäÉQÅ[Égìoò^
        Input.onActionTriggered += OnAbutton;
        Input.onActionTriggered += OnBbutton;
        Input.onActionTriggered += OnLstick;
    }

    private void OnDisable()
    {
        if (Input == null) return;
        // ÉfÉäÉQÅ[Égìoò^âèú
        Input.onActionTriggered -= OnAbutton;
        Input.onActionTriggered -= OnBbutton;
        Input.onActionTriggered -= OnLstick;
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
        // ì¸óÕÇï€éù
        aButton = isButton;
    }
    private void OnBbutton(InputAction.CallbackContext context)
    {
        if (context.action.name != "Bbutton") return;
        // ActionÇÃì¸óÕílÇéÊìæ
        var isButton = context.ReadValueAsButton();
        // ì¸óÕÇï€éù
        bButton = isButton;
    }
    private void OnLstick(InputAction.CallbackContext context)
    {
        if (context.action.name != "Lstick") return;
        // ActionÇÃì¸óÕílÇéÊìæ
        var isStick = context.ReadValue<Vector2>();
        // ì¸óÕÇï€éù
        lStick = isStick;
    }


    // Update is called once per frame
    private void FixedUpdate()
    {
        isAttack();
        //if (lStick != Vector2.zero)
        isMove();
       
    }

    void isMove()
    {
        //Debug.Log(lStick);
        Vector3 vector3 = Vector3.zero;
        if (lStick.x == 0)
            vector3.x = 0f;
        else if (lStick.x < -0.4f)
            vector3.x = -Speed;
        else if(lStick.x > 0.4f)
            vector3.x = Speed;

        if (lStick.y == 0)
            vector3.z = 0f;
        else if(lStick.y < -0.4f)
            vector3.z = -Speed;
        else if (lStick.y > 0.4f)
            vector3.z = Speed;
             
        rb.velocity = vector3;
    }
    void isAttack()
    {
        if (bButton)
        {
            Attack = true;
        }
        else
        {
            Attack = false;
            Speed = NomalSpeed;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wall") && Attack)
        {
            Vector2Int vector2Int = new Vector2Int((int)other.transform.position.x, (int)other.transform.position.z);
            Map_State wall = game.info.ypos[vector2Int.y].xpos[vector2Int.x].state;
            if (wall== Map_State.Destructible)
            {
                Speed = AttackSpeed;
                //soundController.isPlaySE(Clip);
                game.ChengeObject(vector2Int.y, vector2Int.x);
            }
            if (wall == Map_State.Indestructible)
            {
                Speed = NoAttackSpeed;
                //soundController.isPlaySE(Clip2);
            }
        }
    }
}
