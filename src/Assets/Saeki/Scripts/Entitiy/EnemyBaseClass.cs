using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBaseClass : CharacterStatus
{
    [SerializeField] protected GameObject Target;//TargetとなるPlayerを格納
    [SerializeField] private SearchColliderScript collScript;//視界判定のスクリプト

    [SerializeField] private Rigidbody rb;
    [SerializeField] protected NavMeshAgent Agent;
    [SerializeField] private Material material;

    [SerializeField] protected float moveSpeed = 3.5f;
    [SerializeField] protected float rotationSpeed = 0.1f;

    [SerializeField] private GunStatus guns;
    [SerializeField] private GameObject gunObject;

    [SerializeField] AudioClip deadSound;//SEデータ
    [SerializeField] EnemyHeadBlowScript deadHead;

    [Header("敵が使用するコントローラー"), SerializeField] RuntimeAnimatorController enemyAliveController;
    [Header("プレイヤーが乗り移ったときに使用するコントローラー"), SerializeField] RuntimeAnimatorController playerController;

    [Space]
    [SerializeField] private int deadEnemyLayer = 12;//死亡時に設定するレイヤー番号

    [SerializeField] private float fireTimerMax = 0.2f;//発射中までの時間

    [SerializeField] protected float lockonIntarval = 3f;//ロックオン状態インターバル

    [SerializeField] private int remainingBullets;//残弾数

    private float fireTimer = 0f;//発射中までの時間カウンター
    private bool isFire = false;//発射中かの状態判定

    private float remainingCount = 0;//発射インターバルカウンター

    private bool remainingCheck = false;//発射したかの状態判定

    private float lockonCount = 0;//ロックオン状態カウンター
    private bool lockonCheck = false;//ロックオン状態判定

    private float WatchCount = 0;//発見状態カウンター
    private bool isWatched = false;//発見状態判定

    private bool isEnemyDead = false;//死亡フラグ

    /// <summary>
    /// Playerを発見した処理
    /// </summary>
    public void Watch() 
    { 
        isWatched = true;
        WatchCount = 0; 
    }

    /// <summary>
    /// Playerのオブジェクトのgetset
    /// </summary>
    public GameObject TargetSetting
    {
        get { return Target; }  //取得用
        private set { Target = value; } //値入力用
    }

    /// <summary>
    /// 外部からのTargetの変更
    /// </summary>
    public void ChangeTarget(GameObject Set) { TargetSetting = Set; }

    /// <summary>
    /// Navmeshから距離を取得
    /// </summary>
    /// <returns>プレイヤーまでの距離</returns>
    protected float GetDistanseForNavmesh()
    {
        //まだ計算中の場合
        if (Agent.pathPending)
            return float.MaxValue;//floatの最大値を返す
        return
            Agent.remainingDistance;//Navmeshから距離を返す
    }

    void Start()
    {
        //死亡フラグ初期化
        isEnemyDead = false;
        //Playerをtag検索
        Target = GameObject.FindWithTag("Player");
        //NavMeshの速度設定
        Agent.speed = moveSpeed;
        //ターゲットの位置を設定
        Agent.destination = GetTargetPos();
        //基底クラスの処理
        StartSetUp();

        /*
       if(this.TryGetComponent<MeshRenderer>(out MeshRenderer Mesh))
       {
           mesh = Mesh; isGetMesh = true;
       }
       */
    }

    protected virtual void SetUpOverride()
    {
        //基底クラスのため、ruturn
        return;
    }

    public void LostHitPoint()
    {
        //死亡時にNavmeshとRigidBodyの設定を変更
        Agent.enabled = false;
        rb.isKinematic = false;

        /*
        //仮死亡処理(3Dモデル実装後削除)
        if (isGetMesh && mesh.material != material)
        {
            mesh.material = material;
        }
        */
    }

    void TargetChase()
    {
        //NavMeshが有効か && AgentがNavMesh上にあるかどうか
        if (Agent.enabled && Agent.isOnNavMesh)
        {
            //TargetがPlayerかチェック
            CorrectTargetPlayer();
            //NavMeshの経路が無効の場合(無効なら明確な不具合がある)
            if (Agent.pathStatus == NavMeshPathStatus.PathInvalid)
            { 
                //オブジェクトの非表示で対処
                this.gameObject.SetActive(false); 
            }
            else
            {
                //発見状態の場合
                if (isWatched)
                {
                    //カウントを加算
                    WatchCount += Time.deltaTime;
                    //発見状態が続いていた場合
                    if (WatchCount > lockonIntarval)
                    {
                        //発見状態解除
                        isWatched = false;
                    }
                }
                //ターゲットの位置を設定
                Agent.destination = GetPoison();
            }        
        }
    }

    private Vector3 GetPoison()
    {
        //見ているならPlayerの位置を返す
        if (isWatched)
            return Target.transform.position;
        //次に進む位置を探索
        else
            return GetTargetPos(); 
    }
    protected virtual Vector3 GetTargetPos() 
    {
        //基底クラスのため、現在値を返す
        return this.transform.position; 
    }

    /// <summary>
    /// targetにオブジェクトがnullならTag検索
    /// </summary>
    void CorrectTargetPlayer()
    {
        //TargetのオブジェクトのTagがPlayerでない場合
        if (!Target.CompareTag("Player"))
        {
            //Tag検索
            Target = GameObject.FindWithTag("Player");
        }
    }

    /// <summary>
    /// 発射の制御
    /// </summary>
    void OnFire()
    {
        //発射インターバルカウント初期化
        remainingCount = 0f;
        //銃を相手に向ける
        gunObject.transform.LookAt(TargetManeger.getPlayerObj().transform.position + Vector3.up * 0.4f);
        //弾丸を発射したか
        if(guns.Shoot(gunObject.transform.position, gunObject.transform.forward, this.tag, true))
        {
            //残弾を減少
            remainingBullets--;
            //最初の一発でない場合
            if (!remainingCheck) 
                remainingCheck = true;//フラグ更新
            //プレイヤーの周囲の敵を気がつかせる
            TargetManeger.WatchTarget();
            //発射フラグ更新
            isFire = true;
            //発射までの時間を最大値に
            fireTimer = fireTimerMax;
            //Debug.Log("FIRE!!");
        }
    }

    /// <summary>
    /// 視界内、または、LockOnの最中かどうか
    /// </summary>
    /// <returns>どちらかを満たしていたらtrue</returns>
    public bool FindCheck() { return (collScript.IsFindPlayer || lockonCheck); }

    protected virtual void StopChase()
    {
        //Playerを見ていたら
        if (FindCheck())
        {
            //移動を止める
            Agent.speed = 0f;
            // ターゲットの方向への回転
            Vector3 direction = Target.transform.position - transform.position;
            //y軸で回転
            direction.y = 0.0f;
            //見る方向計算
            Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            //見る方向にLerpで動かす
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            //銃の発射カウント加算
            remainingCount += Time.deltaTime;
            //ロックオン状態か切り替え
            if (lockonCount < lockonIntarval) 
            {
                //カウント加算
                lockonCount += Time.deltaTime;
                //ロックオン状態
                lockonCheck = true;
            }
            else
            {
                //ロックオン状態解除
                lockonCheck = false;
            }
        }
        else
        {
            //最初の一発のフラグ更新
            remainingCheck = false;
            //速度を戻す
            Agent.speed = moveSpeed;
            //カウント初期化
            remainingCount = 0f;
            lockonCount = 0f;          
        }
    }

    /// <summary>
    /// Enemyの状態を判断する
    /// </summary>
    /// <returns>Enemyのtagかつ乗り移れない状態ならtrue</returns>
    public bool HealthCheck() 
    {
        //Enemyのtagかつ乗り移れない状態ならtrue
        return this.gameObject.tag == "Enemy" && !CanPossess; 
    }

    /// <summary>
    /// 発射間隔の判定
    /// </summary>
    /// <returns>発射できるならtrue</returns>
    private bool ShotIntarvalCheck()
    {
        //最初の一発とその後で発射間隔の計算を変更
        //二発目移行
        if (remainingCheck)
            return remainingCount > guns.DefaultIntarval && remainingBullets > 0;
        //一発目
        else
            return remainingCount > guns.FirstIntarval && remainingBullets > 0;
    }

    /// <summary>
    /// 敵の挙動を制御
    /// </summary>
    private void MoveEnemy()
    {
        //生きているかを判定
        if (HealthCheck())
        {
            //ターゲットを追いかけるNavmesh設定
            TargetChase();
            //立ち止まる判定を制御
            StopChase();
            //発射が可能かどうか
            if (ShotIntarvalCheck())
                OnFire();
            //アニメーション確認
            FireAnimationCheck();
        }
        else//死んでいる場合
        {
            //死亡した時の状況更新
            LostHitPoint();
            // アニメーターコントローラーの差し替えするか
            if (CharacterAnimator.runtimeAnimatorController == enemyAliveController)
            {
                //アニメーターコントローラを差し替え
                changeAnimatorController();
                //敵の頭を差し替え
                enemyHeadSetting();
            }
        }
    }

    /// <summary>
    /// EnemyのAnimetorを変更する。
    /// </summary>
    private void changeAnimatorController()
    {
        //Animetorを変更。
        GetComponent<Animator>().runtimeAnimatorController = playerController;
        //死亡パラメータ設定
        CharacterAnimator.SetBool("Dead", true);
    }

    /// <summary>
    /// Enemyの頭の情報を変更する
    /// </summary>
    private void enemyHeadSetting()
    {
        //頭を表示を設定するクラスの取得
        PlayerHeadManager headManager = GetComponent<PlayerHeadManager>();
        //敵の頭のenabledを変更
        headManager.EnemyHead.enabled = false;
        //吹っ飛ばす頭を生成
        EnemyHeadBlowScript enemyHeadBlowScript = Instantiate(deadHead, transform.position, transform.rotation);
        //頭を吹っ飛ばす
        enemyHeadBlowScript.BlowOff(TargetManeger.getPlayerObj().transform.position);
    }

    /// <summary>
    /// 銃の発射アニメーションを設定
    /// </summary>
    private void FireAnimationCheck()
    {
        if(isFire)
        {
            fireTimer -= Time.deltaTime;
            if(fireTimer <= 0)
            {
                isFire = false;
            }
        }
        //Animetorを設定
        CharacterAnimator.SetBool("Fire", isFire);
        CharacterAnimator.SetBool("AmmoKeep", guns.RemainBullets > 0);
        CharacterAnimator.SetInteger("WeaponCategory", (int)guns.WeaponType);
    }

    // Update is called once per frame
    override protected void Update()
    {
        //継承元のUpdateを呼び出す
        base.Update();
        //Enemyの行動の動作
        MoveEnemy();
        //死亡判定
        if (IsDead && !isEnemyDead)
        {
            //SE再生
            SR_SoundController.instance.PlaySEOnce(deadSound);
            //死亡フラグ更新
            isEnemyDead = true;
            //レイヤー更新
            this.gameObject.layer = deadEnemyLayer;
        }
    }
}
