using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class TankMovementTest_E : EnemyBaseClass
{
    [SerializeField]
    GameObject TankBody, TankCanon, TankCatapera;
    private int cornersCount = 0;
    private Vector3 directionToNext;
    void BodyRotete()
    {
        Vector3 directionToTarget = Target.transform.position - TankBody.transform.position;
        directionToTarget.y = 0; // 水平方向だけに向く場合、Y軸の回転を無視する
        LookSlerp(TankBody, directionToTarget);
    }
    void CataperaRotete()
    {
        var corners = Agent.path.corners;
        if (cornersCount == corners.Length)
        {
            cornersCount = corners.Length;
            directionToNext = corners[1] - corners[0];
            directionToNext.y = 0; // 水平方向だけに向く場合、Y軸の回転を無視する
        }
        LookSlerp(TankCatapera, directionToNext);
    }
    void CanonRotete()
    {
        Vector3 directionToTarget = Target.transform.position - TankCanon.transform.position;
        directionToTarget.x = 0;
        LookSlerp(TankCanon,directionToTarget);
    }

    void LookSlerp(GameObject tankPart,Vector3 dir)
    {
        float angle = Vector3.Angle(tankPart.transform.forward, dir);

        // 一定以上の角度差があれば、ターゲットの方向を向く
        if (angle > 0.1f)
        {
            // ターゲット方向に向けた回転を計算
            Quaternion targetRotation = Quaternion.LookRotation(dir);

            // 徐々にターゲットの方向に回転
            tankPart.transform.rotation = Quaternion.Slerp(tankPart.transform.rotation,
                targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    protected override void StopChase()
    {
        CataperaRotete();

        if (FindCheck())
        {
            Agent.speed = 0f;
            // ターゲットの方向への回転
            BodyRotete();
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
        }
    }
}
