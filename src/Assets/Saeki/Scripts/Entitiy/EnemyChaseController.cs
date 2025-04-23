using UnityEngine;

public class EnemyChaseController : EnemyBaseClass
{
    /// <summary>
    /// í‚É“G‚ğ’Ç‚¢‚©‚¯‚éˆ—
    /// </summary>
    /// <returns>Target‚ÌˆÊ’u</returns>
    protected override Vector3 GetTargetPos() { return TargetSetting.transform.position; }
}