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

    [Header("�G���g�p����R���g���[���["), SerializeField] RuntimeAnimatorController enemyAliveController;
    [Header("�v���C���[�����ڂ����Ƃ��Ɏg�p����R���g���[���["), SerializeField] RuntimeAnimatorController playerController;

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
        get { return Target; }  //�擾�p
        private set { Target = value; } //�l���͗p
    }
    /// <summary>
    /// �O�������Target�̕ύX
    /// </summary>
    public void ChangeTarget(GameObject Set) { TargetSetting = Set; }

    /// <summary>
    /// Navmesh���狗�����擾
    /// </summary>
    /// <returns>�v���C���[�܂ł̋���</returns>
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
        StartSetUp();//���N���X�̏���

        /*
       if(this.TryGetComponent<MeshRenderer>(out MeshRenderer Mesh))
       {
           mesh = Mesh; isGetMesh = true;
       }
       */
    }

    protected virtual void SetUpOverride()
    {
        //���N���X�̂��߁Aruturn
        return;
    }

    public void LostHitPoint()
    {
        //���S����Navmesh��RigidBody�̐ݒ��ύX
        Agent.enabled = false;
        rb.isKinematic = false;
        /*
        //�����S����(3D���f��������폜)
        if (isGetMesh && mesh.material != material)
        {
            mesh.material = material;
        }
        */
    }

    void TargetChase()
    {
        //NavMesh���L����&&NavMesh��ɂ��邩�ǂ���
        if (Agent.enabled && Agent.isOnNavMesh)
        {
            CorrectTargetPlayer();

            //NavMesh�̌o�H�������̏ꍇ
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
                //�^�[�Q�b�g�̈ʒu��ݒ�
                Agent.destination = GetPoison();
            }        
        }
    }


    private Vector3 GetPoison()
    {
        //�����Ă���Ȃ�target��Ԃ�
        if (isWatched)
            return Target.transform.position;

        return GetTargetPos(); 
    }
    protected virtual Vector3 GetTargetPos() 
    {
        //���N���X�̂��߁A���ݒl��Ԃ�
        return this.transform.position; 
    }

    /// <summary>
    /// target�ɃI�u�W�F�N�g��null�Ȃ�Tag����
    /// </summary>
    void CorrectTargetPlayer()
    {
        if(!Target.CompareTag("Player"))
        {
            Target = GameObject.FindWithTag("Player");
        }
    }

    /// <summary>
    /// ���˂̐���
    /// </summary>
    void OnFire()
    {
        remainingCount = 0f;
        //�e�𑊎�Ɍ�����
        gunObject.transform.LookAt(TargetManeger.getPlayerObj().transform.position + Vector3.up * 0.4f);
        if(guns.Shoot(gunObject.transform.position, gunObject.transform.forward, this.tag, true))
        {
            //�c�e������
            remainingBullets--;
            if (!remainingCheck) remainingCheck = true;

            //�v���C���[�̎��͂̓G���C��������
            TargetManeger.WatchTarget();

            isFire = true;
            fireTimer = fireTimerMax;
            //Debug.Log("FIRE!!");
        }
    }
    /// <summary>
    /// ���E���A�܂��́ALockOn�̍Œ����ǂ���
    /// </summary>
    /// <returns></returns>
    public bool FindCheck() { return (collScript.IsFindPlayer || lockonCheck); }

    protected virtual void StopChase()
    {
        //Agent.remainingDistance < distance
        if (FindCheck())
        {
            Agent.speed = 0f;
            // �^�[�Q�b�g�̕����ւ̉�]
            Vector3 direction = Target.transform.position - transform.position;
            direction.y = 0.0f;
            Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

            //�e�̔��˃��[�g���Z
            remainingCount += Time.deltaTime;

            //��������Ԃ��𐧌�
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
            //�������ړ���Ԃɖ߂�
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
        //�ŏ��̈ꔭ�Ƃ��̌�Ŕ��ˊԊu��ύX
        if (remainingCheck)
            return remainingCount > guns.DefaultIntarval && remainingBullets > 0;
        else
            return remainingCount > guns.FirstIntarval && remainingBullets > 0;
    }

    /// <summary>
    /// �G�̋����𐧌�
    /// </summary>
    private void MoveEnemy()
    {
        //�����Ă��邩�𔻒�
        if (HealthCheck())
        {
            //�^�[�Q�b�g��ǂ�������Navmesh�ݒ�
            TargetChase();
            //�����~�܂锻��𐧌�
            StopChase();
            //���˂��\���ǂ���
            if (ShotCheck())
                OnFire();

            //�A�j���[�V�����m�F
            FireAnimationCheck();
        }
        else
        {
            LostHitPoint();
            // �A�j���[�^�[�R���g���[���[�̍����ւ����邩
            if (CharacterAnimator.runtimeAnimatorController == enemyAliveController)
            {
                //�A�j���[�^�[�R���g���[���������ւ�
                changeAnimatorController();
                //�G�̓��������ւ�
                enemyHeadSetting();
            }
        }
    }
    /// <summary>
    /// Enemy��Animetor��ύX����B
    /// </summary>
    private void changeAnimatorController()
    {
        //Animetor��ύX�B
        GetComponent<Animator>().runtimeAnimatorController = playerController;
        CharacterAnimator.SetBool("Dead", true);
    }

    /// <summary>
    /// Enemy�̓��̏���ύX����B
    /// </summary>
    private void enemyHeadSetting()
    {
        //����\����ݒ肷��B
        PlayerHeadManager headManager = GetComponent<PlayerHeadManager>();
        headManager.EnemyHead.enabled = false;

        //���𐁂���΂��B
        EnemyHeadBlowScript enemyHeadBlowScript = Instantiate(deadHead, transform.position, transform.rotation);
        enemyHeadBlowScript.BlowOff(TargetManeger.getPlayerObj().transform.position);
    }

    /// <summary>
    /// �e�̔��˃A�j���[�V������ݒ�
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

        //Animetor��ݒ�
        CharacterAnimator.SetBool("Fire", isFire);
        CharacterAnimator.SetBool("AmmoKeep", guns.RemainBullets > 0);
        CharacterAnimator.SetInteger("WeaponCategory", (int)guns.WeaponType);
    }

    // Update is called once per frame
    override protected void Update()
    {
        //�p������Update���Ăяo��
        base.Update();
        //Enemy�̍s���̓���
        MoveEnemy();

        //���S����
        if (IsDead && !isDead)
        {
            SR_SoundController.instance.PlaySEOnce(deadSound);
            isDead = true;
            this.gameObject.layer = deadEnemyLayer;
        }
    }
}
