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

    protected float remainingCount = 0,lockonCount = 0;
    protected bool remainingCheck = false, lockonCheck = false;

    private MeshRenderer mesh;
    public GameObject TargetSetting
    {
        get { return Target; }  //取得用
        private set { Target = value; } //値入力用
    }
    /// <summary>
    /// 外部からのTargetの変更
    /// </summary>
    public void ChengeTarget(GameObject Set) { TargetSetting = Set; }

    protected float GetDistanseForNavmesh()
    {
        if (Agent.pathPending)
            return float.MaxValue;
        return 
            Agent.remainingDistance;
    }

    void Start()
    {
        Target = GameObject.FindWithTag("Player");
        Agent.speed = moveSpeed;
        mesh = GetComponent<MeshRenderer>();
        Agent.destination = GetTargetPos();       
        StartSetUp();//基底クラスの処理
    }

    public void LostHitPoint()
    {
        Agent.enabled = false;
        rb.isKinematic = false;

        //仮死亡処理(3Dモデル実装後削除)
        if (mesh.material != material)
        {
            mesh.material = material;
        }
    }

    void TargetChase()
    {
        if (Agent.enabled && Agent.isOnNavMesh)
        {
            CorrectTargetPlayer();
            if (Agent.pathStatus == NavMeshPathStatus.PathInvalid)
                Destroy(this.gameObject);
            else
                Agent.destination = GetTargetPos();
        }
    }
    protected virtual Vector3 GetTargetPos() { return this.transform.position; }
    
    void CorrectTargetPlayer()
    {
        if(!Target.CompareTag("Player"))
        {
            Target = GameObject.FindWithTag("Player");
        }
    }
    void OnFire()
    {
        Debug.Log("FIRE!!");
        remainingCount = 0f;
        if(guns.Shoot(gunObject.transform.position, gunObject.transform.forward, this.tag, true))
        {
            remainingBullets--;
            if (!remainingCheck) remainingCheck = true;
            Debug.Log("FIRE!!");
        }
        //GameObject.Instantiate(Bullet, transform.position, Quaternion.identity);
    }

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

            remainingCount += Time.deltaTime;
            
            if(lockonCount < lockonIntarval) 
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

    public bool HealthCheck() 
    {
        //Debug.Log(Hp);
        return this.gameObject.tag == "Enemy" && Hp > 0; 
    }

    private bool ShotCheck()
    { 
        if(remainingCheck)
            return remainingCount > guns.DefaultIntarval && remainingBullets > 0;
        else
            return remainingCount > guns.FirstIntarval && remainingBullets > 0;
    }

    private void MoveEnemy()
    {
        if (HealthCheck())
        {
            TargetChase();
            StopChase();
            if (ShotCheck())
                OnFire();
        }
        else
            LostHitPoint();
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemy();
    }
}
