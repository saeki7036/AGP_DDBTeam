using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RayTPS : MonoBehaviour
{
    [SerializeField] Camera tpsCam;
    [SerializeField] float distance = 50.0f;    //検出可能な距離
    GameObject objParent;
    PlayerMove playerMove;
    Transform transforms;

    // Start is called before the first frame update
    void Start()
    {
        objParent = transform.parent.gameObject;
        playerMove= objParent.GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        //カメラの位置からとばす
        var rayStartPosition = tpsCam.transform.position;

        //カメラが向いてる方向にとばす
        var rayDirection = tpsCam.transform.forward.normalized;

        Debug.DrawRay(rayStartPosition, rayDirection * distance, Color.red);
    }

    public void Fire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            //カメラの位置からとばす
            var rayStartPosition = tpsCam.transform.position;

            //カメラが向いてる方向にとばす
            var rayDirection = tpsCam.transform.forward.normalized;

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
                }
            }
        }
    }

    public void ChangeEnemy(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && transforms != null)
        {
            Debug.Log("Change");

            //親をEnemyに
            transform.parent.gameObject.tag = "Enemy";
            objParent.transform.rotation = Quaternion.identity;
            playerMove.enabled = false;

            //親の付け替え
            this.gameObject.transform.parent = transforms;
            objParent = transform.parent.gameObject;
            playerMove = objParent.GetComponent<PlayerMove>();

            //親をPlayerに
            transform.parent.gameObject.tag = "Player";
            playerMove.enabled = true;

            //_cam.Follow = raycastHit.transform;
            this.transform.position = transforms.position;
            this.transform.localPosition = new Vector3(0f, 1.5f, -3f);
            this.transform.localRotation = Quaternion.identity;

            transforms = null;
        }
    }
}
