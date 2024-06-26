using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;


public class PlayerMoveCameraPerspective : MonoBehaviour
{
    Vector2 input, inputR;

    float moveZ;
    float moveX;

    float moveSpeed = 6f; //移動速度

    Vector3 velocity = Vector3.zero; //移動方向

    GameObject playerParent;

    [SerializeField] GameObject camera;
    [SerializeField] PlayerRay playerRay;
    GameObject tra;
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
        tra = playerRay.GetTransform();
        Debug.Log("倒した敵:" + tra.name);

        if (context.phase == InputActionPhase.Performed && tra != null)
        {
            Debug.Log("Change");

            //親をEnemyに
            transform.parent.gameObject.tag = "Enemy";
            playerParent.transform.rotation = Quaternion.identity;

            //親の付け替え
            this.gameObject.transform.parent = tra.transform;
            playerParent = this.transform.parent.gameObject;

            //親をPlayerに
            this.transform.parent.gameObject.tag = "Player";

            this.transform.position = tra.transform.position;
            Vector3 correction = new Vector3(0f, 1.5f, -3f);

            camera.transform.position += correction; 
            //this.transform.localRotation = Quaternion.identity;

            tra = null;
        }
    }

}
