using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerRay : MonoBehaviour
{
    [SerializeField] Change change;
    [SerializeField] float distance = 50.0f;//検出可能な距離
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

    public GameObject GetObj(){ return game; }

    public void Change(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            //var center = transform.position;

            //// CapsuleCastによる当たり判定
            //var isHit = Physics.CapsuleCast(
            //    center + new Vector3(0, 0.5f, 0), // 始点
            //    center + new Vector3(0, -0.5f, 0), // 終点
            //    0.5f, // キャストする幅
            //    Vector3.forward, // キャスト方向
            //    out var hit // ヒット情報
            //);

            //if (isHit == true)
            //{
            //    game = hit.collider.GameObject();
            //    change.ChangeEnemy(game);
            //}
            //カメラの位置からとばす
            var rayStartPosition = this.transform.position;

            //カメラが向いてる方向にとばす
            var rayDirection = this.transform.forward.normalized;

            //Hitしたオブジェクト格納用
            RaycastHit raycastHit;

            Debug.DrawRay(rayStartPosition, rayDirection * distance, Color.red);

            if (Physics.Raycast(rayStartPosition, rayDirection, out raycastHit, distance) && raycastHit.collider.tag == "Enemy")
            {
                game = raycastHit.collider.gameObject;
                change.ChangeEnemy(game);
            }

        }
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
                    //Debug.Log("EnemyHit");
                    
                    game=raycastHit.collider.gameObject;
                    game.GetComponent<CharacterStatus>().TakeDamage(100f);
                    if (game.GetComponent<CharacterStatus>().IsDead == true)
                    {
                        Debug.Log("殺した");
                    }
                }
            }
        }
    }
}
