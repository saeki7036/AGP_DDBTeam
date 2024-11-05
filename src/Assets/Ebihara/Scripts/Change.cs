using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Change : MonoBehaviour
{
    //GameObject changeObj;

    PlayerMove playerMove;
    CharacterStatus characterStatus;


    [SerializeField] GameObject camera;
    PlayerRay playerRay;

　　[SerializeField] TargetManeger targetManeger;
    [SerializeField] EnemyManager enemyManager;

    [SerializeField] GameObject playerHead;
    bool changed;

    public bool Changed
    {
        get { return changed; }
    }
    // Start is called before the first frame update
    void Start()
    {
        playerRay = camera.GetComponent<PlayerRay>();
        playerMove = this.GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(this.transform.localEulerAngles.ToString());
    }

    public void ChangeEnemy(GameObject changeObj)
    {
        //仮
        //changeObj = playerRay.GetObj();
        characterStatus = changeObj.GetComponent<CharacterStatus>();

        //乗り移り処理
        if (characterStatus.IsDead == true)
        {
            //Debug.Log("乗り移る敵:" + changeObj.name);
            Debug.Log("Change");

            // 頭を飛ばしてからカメラ変更
            PlayerHeadMoveScript playerHeadMoveScript = Instantiate(playerHead, transform.position, Quaternion.identity).GetComponent<PlayerHeadMoveScript>();// プレイヤーの頭の位置からの生成に変更予定
            StartCoroutine(playerHeadMoveScript.MoveHead(transform.position, characterStatus.transform.position, changeObj));

            ////親をEnemyに
            //transform.parent.gameObject.tag = "Enemy";

            ////親の付け替え
            //this.gameObject.transform.parent = changeObj.transform;
            //playerMove.SetplayerParent(this.transform.parent.gameObject);

            ////親をPlayerに
            //this.transform.parent.gameObject.tag = "Player";

            ////銃の変更
            //playerMove.SetGunObject();

            ////Playerの位置調整
            //this.transform.position = changeObj.transform.position;
            //Vector3 correction = new Vector3(0f, 1.5f, 0f);
            //this.transform.position += correction;

            //Vector3 angles = this.transform.localEulerAngles;
            //angles.y = 0f;
            //this.transform.localEulerAngles = angles;
            //Debug.Log(this.transform.localEulerAngles.ToString());

            //enemyManager.ResetSearch(playerMove.transform.position);
            //changeObj = null;

            ////if (targetManeger != null)
            //TargetManeger.SetTarget(this.transform.parent.gameObject);

            //if(!changed)
            //{
            //    StartCoroutine(SetChangedTrueForSeconds(0.2f));
            //}
        }
    }

    public void ChangeCameraTarget(GameObject changeObj)
    {

        //親をEnemyに
        transform.parent.gameObject.tag = "Enemy";

        //親の付け替え
        this.gameObject.transform.parent = changeObj.transform;
        playerMove.SetplayerParent(this.transform.parent.gameObject);

        //親をPlayerに
        this.transform.parent.gameObject.tag = "Player";

        //銃の変更
        playerMove.SetGunObject();

            //Playerの位置調整
            this.transform.position = changeObj.transform.position;
            Vector3 correction = new Vector3(0f, 0.4f, 0f);
            this.transform.position += correction;

        Vector3 angles = this.transform.localEulerAngles;
        angles.y = 0f;
        this.transform.localEulerAngles = angles;
        Debug.Log(this.transform.localEulerAngles.ToString());

        enemyManager.ResetSearch(playerMove.transform.position);
        changeObj = null;

        //if (targetManeger != null)
        TargetManeger.SetTarget(this.transform.parent.gameObject);

        if (!changed)
        {
            StartCoroutine(SetChangedTrueForSeconds(0.2f));
        }
    }

    IEnumerator SetChangedTrueForSeconds(float second)
    {
        changed = true;
        yield return new WaitForSeconds(second);
        changed = false;
    }
}
