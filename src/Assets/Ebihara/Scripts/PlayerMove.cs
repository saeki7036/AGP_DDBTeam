using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class PlayerMove : MonoBehaviour
{
    Vector2 input, inputR;

    float moveZ;
    float moveX;

    float moveSpeed = 6f; //移動速度

    Vector3 velocity = Vector3.zero; //移動方向

    GameObject playerParent;
    GunStatus gun;

    [SerializeField] GameObject camera;
    PlayerRay playerRay;
    GameObject objParent;

    // プロパティ
    public GameObject PlayerParent
    {
        get { return playerParent; }
    }
    public GunStatus Gun
    {
        get { return gun; }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerParent = transform.parent.gameObject;
        playerRay = camera.GetComponent<PlayerRay>();
        SetGunObject();
    }

    // Update is called once per frame
    void Update()
    {
        // 現オブジェクトからメインカメラ方向のベクトルを取得する
        Vector3 direction = camera.transform.position - this.transform.position;

        Vector3 lookdirection=new Vector3(direction.x,0.0f,direction.z);

        playerParent.transform.rotation = Quaternion.LookRotation(-1.0f * lookdirection);

        //前後移動
        moveZ = input.y;
        //左右移動
        moveX = input.x;

        velocity = new Vector3(moveX, 0, moveZ).normalized * moveSpeed * Time.deltaTime;
        playerParent.transform.Translate(velocity.x, velocity.y, velocity.z);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
        //Debug.Log("Move");
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        inputR = context.ReadValue<Vector2>();
        //Debug.Log("Look");
    }

    public void ChangeEnemy(InputAction.CallbackContext context)
    {
        objParent = playerRay.GetObj();

        if (context.phase == InputActionPhase.Performed && objParent != null)
        {
            Debug.Log("倒した敵:" + objParent.name);

            Debug.Log("Change");
            Vector3 quaternion = objParent.transform.eulerAngles;
            Debug.Log(quaternion);

            //親をEnemyに
            transform.parent.gameObject.tag = "Enemy";

            //親の付け替え
            this.gameObject.transform.parent = objParent.transform;
            playerParent = this.transform.parent.gameObject;
            playerParent.transform.eulerAngles = quaternion;

            //親をPlayerに
            this.transform.parent.gameObject.tag = "Player";

            //Playerの位置調整
            this.transform.position = objParent.transform.position;
            Vector3 correction = new Vector3(0f, 1.5f, 0f);
            this.transform.position += correction;

            objParent = null;
        }
    }

    void SetGunObject()
    {
        gun = playerParent.GetComponentInChildren<GunStatus>();
    }
}
