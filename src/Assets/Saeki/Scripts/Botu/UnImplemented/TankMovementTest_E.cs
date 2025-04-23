using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class TankMovementTest_E : EnemyBaseClass
{
    [SerializeField]
    GameObject TankBody, TankCanon, TankCatapera;
    [SerializeField] private float XRotationLimit = 30f; // X����]�̍ŏ��p�x
    private int cornersCount = 0;
    private Vector3 directionToNext;

    protected override Vector3 GetTargetPos() { return TargetSetting.transform.position; }
    void BodyRotete()
    {
        Vector3 directionToTarget = Target.transform.position - TankBody.transform.position;
        directionToTarget.y = 0; // �������������Ɍ����ꍇ�AY���̉�]�𖳎�����
        LookSlerp(TankBody, directionToTarget);
    }
    void CataperaRotete()
    {
        var corners = Agent.path.corners;
        if (2 > corners.Length)
            return;

        directionToNext = corners[1] - corners[0];
        directionToNext.y = 0; // �������������Ɍ����ꍇ�AY���̉�]�𖳎�����

        LookSlerp(TankCatapera, directionToNext);
    }
    void CanonRotete()
    {
        // �L���m���C����^�[�Q�b�g�ւ̃x�N�g�����v�Z
        Vector3 directionToTarget = Target.transform.position - TankCanon.transform.position;

        // Y����Z���𖳎����AX����]�݂̂��v�Z
        float targetXRotation = Mathf.Atan2(directionToTarget.y, directionToTarget.z) * Mathf.Rad2Deg;

        // X���̉�]�p�x��-30����30�x�ɐ���
        targetXRotation = Mathf.Clamp(targetXRotation, -XRotationLimit, XRotationLimit);

        // ���݂̃L���m���C�̉�]���擾���AX���̂ݕύX
        Vector3 newRotation = new Vector3(-targetXRotation, TankCanon.transform.localEulerAngles.y, TankCanon.transform.localEulerAngles.z);

        // �L���m���C�̉�]�����X�ɍX�V
        TankCanon.transform.localRotation = Quaternion.Slerp(TankCanon.transform.localRotation, Quaternion.Euler(newRotation), Time.deltaTime * rotationSpeed);

        return;


        directionToTarget.y = 0;
        directionToTarget.z = 0;

        float angle = Vector3.Angle(TankCanon.transform.forward, directionToTarget);

        // ���ȏ�̊p�x��������΁A�^�[�Q�b�g�̕���������
        if (angle > 0.1f)
        {
            // �^�[�Q�b�g�����Ɍ�������]���v�Z
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            float xRotate = Mathf.Atan2(directionToTarget.y, directionToTarget.z) * Mathf.Rad2Deg;//targetRotation.eulerAngles.x;
            Vector3 newvec = new(xRotate, TankCanon.transform.eulerAngles.y, TankCanon.transform.eulerAngles.z);

            // ���X�Ƀ^�[�Q�b�g�̕����ɉ�]
            TankCanon.transform.localRotation = Quaternion.Lerp(TankCanon.transform.localRotation,
                Quaternion.Euler(newvec), rotationSpeed * Time.deltaTime);
        }
    }

    void LookSlerp(GameObject tankPart,Vector3 dir)
    {
        float angle = Vector3.Angle(tankPart.transform.forward, dir);

        // ���ȏ�̊p�x��������΁A�^�[�Q�b�g�̕���������
        if (angle > 0.1f)
        {
            // �^�[�Q�b�g�����Ɍ�������]���v�Z
            Quaternion targetRotation = Quaternion.LookRotation(dir);

            // ���X�Ƀ^�[�Q�b�g�̕����ɉ�]
            tankPart.transform.rotation = Quaternion.Slerp(tankPart.transform.rotation,
                targetRotation, rotationSpeed * rotationSpeed * Time.deltaTime);
        }
    }
    protected override void StopChase()
    {
        CataperaRotete();
        BodyRotete();

        //�v
        /*
        if (FindCheck())
        {
            Agent.speed = 0f;

            // �^�[�Q�b�g�̕����ւ̉�]          
            CanonRotete();

            remainingCount += Time.deltaTime;

            if (lockonCount < lockonIntarval)
            {
                lockonCount += Time.deltaTime;
                lockonCheck = true;
            }
            else
            {
                lockonCheck = false;
            }
        }
        else
        {
            remainingCheck = false;
            remainingCount = 0f;
            lockonCount = 0f;
            Agent.speed = moveSpeed;
        }*/
    }
}
