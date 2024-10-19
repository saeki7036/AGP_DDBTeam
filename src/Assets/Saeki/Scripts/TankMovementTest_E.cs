using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovementTest_E : EnemyBaseClass
{
    [SerializeField]
    GameObject TankBody, TankCanon, TankCatapera;
    void CataperaRotete()
    {
        Vector3 directionToTarget = Target.transform.position - transform.position;
        directionToTarget.y = 0; // 水平方向だけに向く場合、Y軸の回転を無視する

        // 現在の前方向とターゲットの方向の角度を計算
        float angle = Vector3.Angle(transform.forward, directionToTarget);

        // 一定以上の角度差があれば、ターゲットの方向を向く
        if (angle > 0.1f)
        {
            // ターゲット方向に向けた回転を計算
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            // 徐々にターゲットの方向に回転
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    protected override void StopChase()
    {
        if (FindCheck())
        {
            Agent.speed = 0f;
            // ターゲットの方向への回転
            Vector3 direction = Target.transform.position - transform.position;
            direction.y = 0.0f;
            Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

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
        }
    }
}
