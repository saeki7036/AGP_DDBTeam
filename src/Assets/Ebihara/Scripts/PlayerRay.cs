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
    PlayerMove playerMove;

    // Start is called before the first frame update
    void Start() 
    {
        playerMove = FindObjectOfType<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        //カメラの位置からとばす
        var rayStartPosition = this.transform.position;

        //カメラが向いてる方向にとばす
        var rayDirection = this.transform.forward.normalized;

        Debug.DrawRay(rayStartPosition, rayDirection * distance, Color.red);
        playerMove.Gun.transform.forward = rayDirection;
    }

    public GameObject GetObj()
    {
        return game;
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            // 銃弾発射処理以前の処理、念の為取っておいてあるが不要になり次第消す
            ////カメラの位置からとばす
            //var rayStartPosition = this.transform.position;

            ////カメラが向いてる方向にとばす
            //var rayDirection = this.transform.forward.normalized;

            ////Hitしたオブジェクト格納用
            //RaycastHit raycastHit;

            //Debug.DrawRay(rayStartPosition, rayDirection * distance, Color.red);

            //if (Physics.Raycast(rayStartPosition, rayDirection, out raycastHit, distance))
            //{
            //    // LogにHitしたオブジェクト名を出力
            //    //Debug.Log(context.phase);
            //    Debug.Log("HitObject : " + raycastHit.collider.gameObject.name);

            //    if (raycastHit.collider.tag == "Enemy")
            //    {
            //        Debug.Log("EnemyHit");
            //        transforms = raycastHit.transform;
            //        game=raycastHit.collider.gameObject;
            //    }
            //}

            // PlayerMoveから現在操作中のキャラクターを取得し、そのキャラクターの持つ武器の弾丸を発射予定
            playerMove.Gun.Shoot(transform.position, playerMove.Gun.transform.forward, "Player", false);
        }
    }

    public void CheckCanPossess(InputAction.CallbackContext context)
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
                    if (raycastHit.collider.TryGetComponent<CharacterStatus>(out CharacterStatus character) && character.CanPossess)
                    {
                        Debug.Log("EnemyHit");
                        transforms = raycastHit.transform;
                        game = raycastHit.collider.gameObject;
                    }
                }
            }
        }
        if(context.phase == InputActionPhase.Canceled)
        {
            transforms = null;
            game = null;
        }
    }
}
