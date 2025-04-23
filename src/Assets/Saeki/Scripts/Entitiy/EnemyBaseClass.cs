using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBaseClass : CharacterStatus
{
    [SerializeField] protected GameObject Target;//Target�ƂȂ�Player���i�[
    [SerializeField] private SearchColliderScript collScript;//���E����̃X�N���v�g

    [SerializeField] private Rigidbody rb;
    [SerializeField] protected NavMeshAgent Agent;
    [SerializeField] private Material material;

    [SerializeField] protected float moveSpeed = 3.5f;
    [SerializeField] protected float rotationSpeed = 0.1f;

    [SerializeField] private GunStatus guns;
    [SerializeField] private GameObject gunObject;

    [SerializeField] AudioClip deadSound;//SE�f�[�^
    [SerializeField] EnemyHeadBlowScript deadHead;

    [Header("�G���g�p����R���g���[���["), SerializeField] RuntimeAnimatorController enemyAliveController;
    [Header("�v���C���[�����ڂ����Ƃ��Ɏg�p����R���g���[���["), SerializeField] RuntimeAnimatorController playerController;

    [Space]
    [SerializeField] private int deadEnemyLayer = 12;//���S���ɐݒ肷�郌�C���[�ԍ�

    [SerializeField] private float fireTimerMax = 0.2f;//���˒��܂ł̎���

    [SerializeField] protected float lockonIntarval = 3f;//���b�N�I����ԃC���^�[�o��

    [SerializeField] private int remainingBullets;//�c�e��

    private float fireTimer = 0f;//���˒��܂ł̎��ԃJ�E���^�[
    private bool isFire = false;//���˒����̏�Ԕ���

    private float remainingCount = 0;//���˃C���^�[�o���J�E���^�[

    private bool remainingCheck = false;//���˂������̏�Ԕ���

    private float lockonCount = 0;//���b�N�I����ԃJ�E���^�[
    private bool lockonCheck = false;//���b�N�I����Ԕ���

    private float WatchCount = 0;//������ԃJ�E���^�[
    private bool isWatched = false;//������Ԕ���

    private bool isEnemyDead = false;//���S�t���O

    /// <summary>
    /// Player�𔭌���������
    /// </summary>
    public void Watch() 
    { 
        isWatched = true;
        WatchCount = 0; 
    }

    /// <summary>
    /// Player�̃I�u�W�F�N�g��getset
    /// </summary>
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
        //�܂��v�Z���̏ꍇ
        if (Agent.pathPending)
            return float.MaxValue;//float�̍ő�l��Ԃ�
        return
            Agent.remainingDistance;//Navmesh���狗����Ԃ�
    }

    void Start()
    {
        //���S�t���O������
        isEnemyDead = false;
        //Player��tag����
        Target = GameObject.FindWithTag("Player");
        //NavMesh�̑��x�ݒ�
        Agent.speed = moveSpeed;
        //�^�[�Q�b�g�̈ʒu��ݒ�
        Agent.destination = GetTargetPos();
        //���N���X�̏���
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
        //NavMesh���L���� && Agent��NavMesh��ɂ��邩�ǂ���
        if (Agent.enabled && Agent.isOnNavMesh)
        {
            //Target��Player���`�F�b�N
            CorrectTargetPlayer();
            //NavMesh�̌o�H�������̏ꍇ(�����Ȃ疾�m�ȕs�������)
            if (Agent.pathStatus == NavMeshPathStatus.PathInvalid)
            { 
                //�I�u�W�F�N�g�̔�\���őΏ�
                this.gameObject.SetActive(false); 
            }
            else
            {
                //������Ԃ̏ꍇ
                if (isWatched)
                {
                    //�J�E���g�����Z
                    WatchCount += Time.deltaTime;
                    //������Ԃ������Ă����ꍇ
                    if (WatchCount > lockonIntarval)
                    {
                        //������ԉ���
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
        //���Ă���Ȃ�Player�̈ʒu��Ԃ�
        if (isWatched)
            return Target.transform.position;
        //���ɐi�ވʒu��T��
        else
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
        //Target�̃I�u�W�F�N�g��Tag��Player�łȂ��ꍇ
        if (!Target.CompareTag("Player"))
        {
            //Tag����
            Target = GameObject.FindWithTag("Player");
        }
    }

    /// <summary>
    /// ���˂̐���
    /// </summary>
    void OnFire()
    {
        //���˃C���^�[�o���J�E���g������
        remainingCount = 0f;
        //�e�𑊎�Ɍ�����
        gunObject.transform.LookAt(TargetManeger.getPlayerObj().transform.position + Vector3.up * 0.4f);
        //�e�ۂ𔭎˂�����
        if(guns.Shoot(gunObject.transform.position, gunObject.transform.forward, this.tag, true))
        {
            //�c�e������
            remainingBullets--;
            //�ŏ��̈ꔭ�łȂ��ꍇ
            if (!remainingCheck) 
                remainingCheck = true;//�t���O�X�V
            //�v���C���[�̎��͂̓G���C��������
            TargetManeger.WatchTarget();
            //���˃t���O�X�V
            isFire = true;
            //���˂܂ł̎��Ԃ��ő�l��
            fireTimer = fireTimerMax;
            //Debug.Log("FIRE!!");
        }
    }

    /// <summary>
    /// ���E���A�܂��́ALockOn�̍Œ����ǂ���
    /// </summary>
    /// <returns>�ǂ��炩�𖞂����Ă�����true</returns>
    public bool FindCheck() { return (collScript.IsFindPlayer || lockonCheck); }

    protected virtual void StopChase()
    {
        //Player�����Ă�����
        if (FindCheck())
        {
            //�ړ����~�߂�
            Agent.speed = 0f;
            // �^�[�Q�b�g�̕����ւ̉�]
            Vector3 direction = Target.transform.position - transform.position;
            //y���ŉ�]
            direction.y = 0.0f;
            //��������v�Z
            Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            //���������Lerp�œ�����
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            //�e�̔��˃J�E���g���Z
            remainingCount += Time.deltaTime;
            //���b�N�I����Ԃ��؂�ւ�
            if (lockonCount < lockonIntarval) 
            {
                //�J�E���g���Z
                lockonCount += Time.deltaTime;
                //���b�N�I�����
                lockonCheck = true;
            }
            else
            {
                //���b�N�I����ԉ���
                lockonCheck = false;
            }
        }
        else
        {
            //�ŏ��̈ꔭ�̃t���O�X�V
            remainingCheck = false;
            //���x��߂�
            Agent.speed = moveSpeed;
            //�J�E���g������
            remainingCount = 0f;
            lockonCount = 0f;          
        }
    }

    /// <summary>
    /// Enemy�̏�Ԃ𔻒f����
    /// </summary>
    /// <returns>Enemy��tag�����ڂ�Ȃ���ԂȂ�true</returns>
    public bool HealthCheck() 
    {
        //Enemy��tag�����ڂ�Ȃ���ԂȂ�true
        return this.gameObject.tag == "Enemy" && !CanPossess; 
    }

    /// <summary>
    /// ���ˊԊu�̔���
    /// </summary>
    /// <returns>���˂ł���Ȃ�true</returns>
    private bool ShotIntarvalCheck()
    {
        //�ŏ��̈ꔭ�Ƃ��̌�Ŕ��ˊԊu�̌v�Z��ύX
        //�񔭖ڈڍs
        if (remainingCheck)
            return remainingCount > guns.DefaultIntarval && remainingBullets > 0;
        //�ꔭ��
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
            if (ShotIntarvalCheck())
                OnFire();
            //�A�j���[�V�����m�F
            FireAnimationCheck();
        }
        else//����ł���ꍇ
        {
            //���S�������̏󋵍X�V
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
        //���S�p�����[�^�ݒ�
        CharacterAnimator.SetBool("Dead", true);
    }

    /// <summary>
    /// Enemy�̓��̏���ύX����
    /// </summary>
    private void enemyHeadSetting()
    {
        //����\����ݒ肷��N���X�̎擾
        PlayerHeadManager headManager = GetComponent<PlayerHeadManager>();
        //�G�̓���enabled��ύX
        headManager.EnemyHead.enabled = false;
        //������΂����𐶐�
        EnemyHeadBlowScript enemyHeadBlowScript = Instantiate(deadHead, transform.position, transform.rotation);
        //���𐁂���΂�
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
        if (IsDead && !isEnemyDead)
        {
            //SE�Đ�
            SR_SoundController.instance.PlaySEOnce(deadSound);
            //���S�t���O�X�V
            isEnemyDead = true;
            //���C���[�X�V
            this.gameObject.layer = deadEnemyLayer;
        }
    }
}
