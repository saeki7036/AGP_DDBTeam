using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDiscoveryController : EnemyBaseClass
{
    private bool discovery;//発見しているかのフラグ

    /// <summary>
    /// 外部からの発見判定を設定
    /// </summary>
    public void IsDiscobery() { discovery = true; }

    protected override void SetUpOverride()
    {
        discovery = false;//発見していない状態で初期化
    }

    protected override Vector3 GetTargetPos()
    {
        //発見しているならTargetの位置を返す
        if (discovery)
        {
            return TargetSetting.transform.position;
        }
        //視認しているなら発見判定
        if (FindCheck())
        {
            discovery = true;
        }
        //見つからない場合待機
        return this.transform.position;
    }
}
