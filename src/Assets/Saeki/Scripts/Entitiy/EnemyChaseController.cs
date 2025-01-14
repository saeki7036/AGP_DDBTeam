using UnityEngine;

public class EnemyChaseController : EnemyBaseClass
{
    protected override Vector3 GetTargetPos() { return TargetSetting.transform.position; }
}