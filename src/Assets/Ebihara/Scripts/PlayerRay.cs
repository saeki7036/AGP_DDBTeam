using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRay : MonoBehaviour
{
    private CinemachineVirtualCamera _cam;
    [SerializeField] Camera fpsCam;
    [SerializeField] float distance = 50.0f;    //検出可能な距離

    // Start is called before the first frame update
    void Start() 
    {
       _cam= GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            //カメラの位置からとばす
            var rayStartPosition = fpsCam.transform.position;

            //カメラが向いてる方向にとばす
            var rayDirection = fpsCam.transform.forward.normalized;

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
                    Debug.Log("Hit");
                    //親をEnemyに
                    transform.parent.gameObject.tag = "Enemy";
                    //親の付け替え
                    this.gameObject.transform.parent = raycastHit.transform;
                    //親をPlayerに
                    transform.parent.gameObject.tag = "Player";

                    _cam.Follow = raycastHit.transform;
                    this.transform.position = raycastHit.transform.position;
                }
            }
        }
    }
}
