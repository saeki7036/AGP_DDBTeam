using UnityEngine;

public class EnemyChaseController : EnemyBaseClass
{
    /// <summary>
    /// 常に敵を追いかける処理
    /// </summary>
    /// <returns>Targetの位置</returns>
    protected override Vector3 GetTargetPos() { return TargetSetting.transform.position; }
}