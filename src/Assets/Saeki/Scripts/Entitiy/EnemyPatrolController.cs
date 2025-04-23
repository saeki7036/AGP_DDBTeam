using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolController : EnemyBaseClass
{ 
    [SerializeField] private float nextPosDistance = 4f;//ポイント切り替えまでの距離
    [SerializeField] private Transform[] wayPoints;//移動先の座標
    [SerializeField] private bool isRoop = true;//巡回するかのフラグ
   
    private int nextPoint = 0;
    protected override Vector3 GetTargetPos() { return NextPointUpdate(); }

    private void GoNextPoint()
    {
        // 配列内の次の位置を目標地点に設定
        nextPoint++;
        // 一巡したら最初の地点に移動
        if (isRoop && nextPoint == wayPoints.Length)
        {
            nextPoint = 0;
        }
    }
    private Vector3 NextPointUpdate()
    {        
        // 地点がなにも設定されていないときに返す
        if (wayPoints.Length == 0)
            return this.transform.position;
        // 地点が１箇所だけ設定されているときに返す
        else if (wayPoints.Length == 1)
        {
            //初期地点
            return wayPoints[0].position;
        }
        //距離を判定
        else if (GetDistanseForNavmesh() < nextPosDistance)
            GoNextPoint();//次のポイントへ移動

        // エージェントが現在設定された目標地点に行くように設定
        return wayPoints[nextPoint].position;
    }
}
