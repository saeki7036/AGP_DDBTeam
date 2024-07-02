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

    [SerializeField] GameObject camera;
    PlayerRay playerRay;
    GameObject objParent;

    // Start is called before the first frame update
    void Start()
    {
        playerParent = transform.parent.gameObject;
        playerRay = camera.GetComponent<PlayerRay>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(playerParent.transform.eulerAngles);

        // 現オブジェクトからメインカメラ方向のベクトルを取得する
        Vector3 direction = camera.transform.position - this.transform.position;
        Vector3 keep = new Vector3(0f, playerParent.transform.rotation.y, 0f);

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
        Debug.Log("倒した敵:" + objParent.name);

        if (context.phase == InputActionPhase.Performed && objParent != null)
        {
            Debug.Log("Change");
            Vector3 quaternion = objParent.transform.eulerAngles;
            Debug.Log(quaternion);

            //親をEnemyに
            transform.parent.gameObject.tag = "Enemy";
            //playerParent.transform.rotation = Quaternion.identity;

            //親の付け替え
            this.gameObject.transform.parent = objParent.transform;
            playerParent = this.transform.parent.gameObject;

            //親をPlayerに
            this.transform.parent.gameObject.tag = "Player";

            this.transform.position = objParent.transform.position;
            Vector3 correction = new Vector3(0f, 1.5f, 0f);
            this.transform.position += correction;

            Vector3 cameraCorrection = new Vector3(0f, 2.5f, -5f);

            playerParent.transform.eulerAngles = quaternion;
            Debug.Log("change:"+playerParent.transform.eulerAngles);

            //camera.transform.position += cameraCorrection;
            //camera.transform.eulerAngles = quaternion;
            //this.transform.localRotation = Quaternion.identity;

            //objParent = null;
        }
    }

}
