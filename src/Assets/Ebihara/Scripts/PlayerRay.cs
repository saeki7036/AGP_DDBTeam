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
    [SerializeField] float distance = 50.0f;//���o�\�ȋ���
    Transform transforms;
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
        //�J�����̈ʒu����Ƃ΂�
        var rayStartPosition = this.transform.position;
        //�J�����������Ă�����ɂƂ΂�
        var rayDirection = this.transform.forward.normalized;
        Debug.DrawRay(rayStartPosition, rayDirection * distance, Color.red);
        playerMove.Gun.transform.forward = rayDirection;
    }

    public GameObject GetObj(){ return game; }

    public void Change(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            //var center = transform.position;

            //// CapsuleCast�ɂ�铖���蔻��
            //var isHit = Physics.CapsuleCast(
            //    center + new Vector3(0, 0.5f, 0), // �n�_
            //    center + new Vector3(0, -0.5f, 0), // �I�_
            //    0.5f, // �L���X�g���镝
            //    Vector3.forward, // �L���X�g����
            //    out var hit // �q�b�g���
            //);

            //if (isHit == true)
            //{
            //    game = hit.collider.GameObject();
            //    change.ChangeEnemy(game);
            //}
            //�J�����̈ʒu����Ƃ΂�
            var rayStartPosition = this.transform.position;

            //�J�����������Ă�����ɂƂ΂�
            var rayDirection = this.transform.forward.normalized;

            //Hit�����I�u�W�F�N�g�i�[�p
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
            // �e�e���ˏ����ȑO�̏����A�O�̈׎���Ă����Ă��邪�s�v�ɂȂ莟�����
            ////�J�����̈ʒu����Ƃ΂�
            //var rayStartPosition = this.transform.position;

            ////�J�����������Ă�����ɂƂ΂�
            //var rayDirection = this.transform.forward.normalized;

            ////Hit�����I�u�W�F�N�g�i�[�p
            //RaycastHit raycastHit;

            //Debug.DrawRay(rayStartPosition, rayDirection * distance, Color.red);

            //if (Physics.Raycast(rayStartPosition, rayDirection, out raycastHit, distance))
            //{
            //    // Log��Hit�����I�u�W�F�N�g�����o��
            //    //Debug.Log(context.phase);
            //    Debug.Log("HitObject : " + raycastHit.collider.gameObject.name);

            //    if (raycastHit.collider.tag == "Enemy")
            //    {
            //        Debug.Log("EnemyHit");
            //        transforms = raycastHit.transform;
            //        game=raycastHit.collider.gameObject;
            //    }
            //}

            // PlayerMove���猻�ݑ��쒆�̃L�����N�^�[���擾���A���̃L�����N�^�[�̎�����̒e�ۂ𔭎˗\��
            playerMove.Gun.Shoot(transform.position, playerMove.Gun.transform.forward, "Player", false);
        }
    }

    public void CheckCanPossess(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            //�J�����̈ʒu����Ƃ΂�
            var rayStartPosition = this.transform.position;

            //�J�����������Ă�����ɂƂ΂�
            var rayDirection = this.transform.forward.normalized;

            //Hit�����I�u�W�F�N�g�i�[�p
            RaycastHit raycastHit;

            Debug.DrawRay(rayStartPosition, rayDirection * distance, Color.red);

            if (Physics.Raycast(rayStartPosition, rayDirection, out raycastHit, distance))
            {
                // Log��Hit�����I�u�W�F�N�g�����o��
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
