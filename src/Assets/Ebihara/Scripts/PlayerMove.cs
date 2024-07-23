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

    float moveSpeed = 6f; //�ړ����x

    Vector3 velocity = Vector3.zero; //�ړ�����

    GameObject playerParent;
    GunStatus gun;

    [SerializeField] GameObject camera;
    PlayerRay playerRay;
    GameObject objParent;

    // �v���p�e�B
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
        // ���I�u�W�F�N�g���烁�C���J���������̃x�N�g�����擾����
        Vector3 direction = camera.transform.position - this.transform.position;

        Vector3 lookdirection=new Vector3(direction.x,0.0f,direction.z);

        playerParent.transform.rotation = Quaternion.LookRotation(-1.0f * lookdirection);

        //�O��ړ�
        moveZ = input.y;
        //���E�ړ�
        moveX = input.x;

        velocity = new Vector3(moveX, 0, moveZ).normalized * moveSpeed * Time.deltaTime;
        playerParent.transform.Translate(velocity.x, velocity.y, velocity.z);
    }

    public void SetplayerParent(GameObject gameObject)
    {
        playerParent = gameObject;
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

    public void ChangeEnemy(GameObject game)
    {
        objParent = game;

        if ( objParent != null)
        {
            Debug.Log("�|�����G:" + objParent.name);

            Debug.Log("Change");
            Vector3 quaternion = objParent.transform.eulerAngles;
            Debug.Log(quaternion);

            //�e��Enemy��
            transform.parent.gameObject.tag = "Enemy";

            //�e�̕t���ւ�
            this.gameObject.transform.parent = objParent.transform;
            playerParent = this.transform.parent.gameObject;
            playerParent.transform.eulerAngles = quaternion;

            //�e��Player��
            this.transform.parent.gameObject.tag = "Player";

            //Player�̈ʒu����
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
