using UnityEngine;

public class EnemyChaseController : EnemyBaseClass
{
    /// <summary>
    /// ��ɓG��ǂ������鏈��
    /// </summary>
    /// <returns>Target�̈ʒu</returns>
    protected override Vector3 GetTargetPos() { return TargetSetting.transform.position; }
}