using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerRay : MonoBehaviour
{
    [SerializeField] float distance = 50.0f;//検出可能な距離
    Transform transforms;//倒した敵の保存
    GameObject game;

    // Start is called before the first frame update
    void Start() 
    {

    }

    // Update is called once per frame
    void Update()
    {
        //カメラの位置からとばす
        var rayStartPosition = this.transform.position;

        //カメラが向いてる方向にとばす
        var rayDirection = this.transform.forward.normalized;

        Debug.DrawRay(rayStartPosition, rayDirection * distance, Color.red);
    }

    public GameObject GetObj()
    {
        return game;
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            //カメラの位置からとばす
            var rayStartPosition = this.transform.position;

            //カメラが向いてる方向にとばす
            var rayDirection = this.transform.forward.normalized;

            //Hitしたオブジェクト格納用
            RaycastHit raycastHit;

            Debug.DrawRay(rayStartPosition, rayDirection * distance, Color.red);

            if (Physics.Raycast(rayStartPosition, rayDirection, out raycastHit, distance))
            {
                // LogにHitしたオブジェクト名を出力
                //Debug.Log(context.phase);
                Debug.Log("HitObject : " + raycastHit.collider.gameObject.name);

                if (raycastHit.collider.tag == "Enemy")
                {
                    Debug.Log("EnemyHit");
                    transforms = raycastHit.transform;
                    game=raycastHit.collider.gameObject;
                }
            }
        }
    }
}
