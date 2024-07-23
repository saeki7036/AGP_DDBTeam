using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBaseClass : MonoBehaviour
{
    [SerializeField] private float HitPoint = 1f;

    [SerializeField] private GameObject Target;
    [SerializeField] private SearchColliderScript collScript;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private NavMeshAgent Agent;

    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float rotationSpeed = 0.1f;

    [SerializeField] private GameObject Bullet;
    [SerializeField] private int remainingBullets;
    [SerializeField] private float fireIntarval = 3f;

    private float remainingIntarval = 0;

    public GameObject TargetSetting
    {
        get { return Target; }  //取得用
        private set { Target = value; } //値入力用
    }

    protected float GetDistanseForNavmesh() { return Agent.remainingDistance; }

    void Start()
    {
        Agent.speed = moveSpeed;
    }

    public void LostHitPoint()
    {
        Agent.enabled = false;
        rb.isKinematic = false;
    }

    void TargetChase()
    {
        if (Agent.enabled && Agent.isOnNavMesh)
        {
            if (Agent.pathStatus == NavMeshPathStatus.PathInvalid)
                Destroy(this.gameObject);
            else
                Agent.destination = GetTargetPos();
        }
    }
    protected virtual Vector3 GetTargetPos() { return this.transform.position; }
    void OnFire()
    {
        remainingBullets--;
        remainingIntarval = 0f;
        Debug.Log("FIRE!!");
        GameObject.Instantiate(Bullet, transform.position, Quaternion.identity);
    }

    void StopChase()
    {
        //Agent.remainingDistance < distance
        if (collScript.FindPlayer)
        {
            Agent.speed = 0f;
            // ターゲットの方向への回転
            Vector3 direction = Target.transform.position - transform.position;
            direction.y = 0.0f;
            Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, rotationSpeed);

            remainingIntarval += Time.deltaTime;
        }
        else
        {
            remainingIntarval = 0f;
            Agent.speed = moveSpeed;
        }
    }

    private bool HealthCheck() {  return this.gameObject.tag == "Enemy" && HitPoint > 0; }

    private bool ShotCheck() { return remainingIntarval > fireIntarval && remainingBullets > 0; }

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
