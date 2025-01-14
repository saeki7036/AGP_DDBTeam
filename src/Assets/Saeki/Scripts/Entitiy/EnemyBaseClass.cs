using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBaseClass : CharacterStatus
{
    [SerializeField] protected GameObject Target;
    [SerializeField] private SearchColliderScript collScript;
    [SerializeField] private Rigidbody rb;
    [SerializeField] protected NavMeshAgent Agent;
    [SerializeField] private Material material;

    [SerializeField] protected float moveSpeed = 3.5f;
    [SerializeField] protected float rotationSpeed = 0.1f;

    [SerializeField] private GunStatus guns;
    [SerializeField] private GameObject gunObject;
    [SerializeField] private int remainingBullets;

    [SerializeField] protected float lockonIntarval = 3f;
    [SerializeField] AudioClip deadSound;
    [SerializeField] EnemyHeadBlowScript deadHead;

    [Header("敵が使用するコントローラー"), SerializeField] RuntimeAnimatorController enemyAliveController;
    [Header("プレイヤーが乗り移ったときに使用するコントローラー"), SerializeField] RuntimeAnimatorController playerController;

    protected float remainingCount = 0, lockonCount = 0;
    protected bool remainingCheck = false, lockonCheck = false;

    private float WatchCount = 0;
    private bool isWatched = false, isFire = false, isDead = false;//, isGetMesh = false;
    private float fireTimer = 0f, fireTimerMax = 0.2f;
    private int deadEnemyLayer = 12;

    //private MeshRenderer mesh;

    public void Watch() { isWatched = true; WatchCount = 0; }


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
        if (Agent.pathPending)
            return float.MaxValue;
        return
            Agent.remainingDistance;
    }

    void Start()
    {
        isDead = false;
        Target = GameObject.FindWithTag("Player");
        Agent.speed = moveSpeed;
        Agent.destination = GetTargetPos();
        StartSetUp();//基底クラスの処理

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
        //NavMeshが有効か&&NavMesh上にあるかどうか
        if (Agent.enabled && Agent.isOnNavMesh)
        {
            CorrectTargetPlayer();

            //NavMeshの経路が無効の場合
            if (Agent.pathStatus == NavMeshPathStatus.PathInvalid)
            { 
                this.gameObject.SetActive(false); 
            }
            else
            {
                if (isWatched)
                {
                    WatchCount += Time.deltaTime;
                    if(WatchCount > lockonIntarval)
                    {
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
        //見られているならtargetを返す
        if (isWatched)
            return Target.transform.position;

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
        if(!Target.CompareTag("Player"))
        {
            Target = GameObject.FindWithTag("Player");
        }
    }

    /// <summary>
    /// 発射の制御
    /// </summary>
    void OnFire()
    {
        remainingCount = 0f;
        //銃を相手に向ける
        gunObject.transform.LookAt(TargetManeger.getPlayerObj().transform.position + Vector3.up * 0.4f);
        if(guns.Shoot(gunObject.transform.position, gunObject.transform.forward, this.tag, true))
        {
            //残弾を減少
            remainingBullets--;
            if (!remainingCheck) remainingCheck = true;

            //プレイヤーの周囲の敵を気がつかせる
            TargetManeger.WatchTarget();

            isFire = true;
            fireTimer = fireTimerMax;
            //Debug.Log("FIRE!!");
        }
    }
    /// <summary>
    /// 視界内、または、LockOnの最中かどうか
    /// </summary>
    /// <returns></returns>
    public bool FindCheck() { return (collScript.IsFindPlayer || lockonCheck); }

    protected virtual void StopChase()
    {
        //Agent.remainingDistance < distance
        if (FindCheck())
        {
            Agent.speed = 0f;
            // ターゲットの方向への回転
            Vector3 direction = Target.transform.position - transform.position;
            direction.y = 0.0f;
            Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            //銃の発射レート加算
            remainingCount += Time.deltaTime;

            //見失う状態かを制御
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
            //見失い移動状態に戻る
            remainingCheck = false;
            remainingCount = 0f;
            lockonCount = 0f;
            Agent.speed = moveSpeed;
        }
    }

    public bool HealthCheck() 
    {
        //Debug.Log(Hp);
        return this.gameObject.tag == "Enemy" && !CanPossess; 
    }

    private bool ShotCheck()
    {
        //最初の一発とその後で発射間隔を変更
        if (remainingCheck)
            return remainingCount > guns.DefaultIntarval && remainingBullets > 0;
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
            if (ShotCheck())
                OnFire();

            //アニメーション確認
            FireAnimationCheck();
        }
        else
        {
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
        CharacterAnimator.SetBool("Dead", true);
    }

    /// <summary>
    /// Enemyの頭の情報を変更する。
    /// </summary>
    private void enemyHeadSetting()
    {
        //頭を表示を設定する。
        PlayerHeadManager headManager = GetComponent<PlayerHeadManager>();
        headManager.EnemyHead.enabled = false;

        //頭を吹っ飛ばす。
        EnemyHeadBlowScript enemyHeadBlowScript = Instantiate(deadHead, transform.position, transform.rotation);
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
        if (IsDead && !isDead)
        {
            SR_SoundController.instance.PlaySEOnce(deadSound);
            isDead = true;
            this.gameObject.layer = deadEnemyLayer;
        }
    }
}
