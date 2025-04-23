using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainGameGunsUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI remainBulletText;
    [SerializeField] Image mainGunImage;
    [SerializeField] Image subGunImage;

    [SerializeField] Animator animator;//�e��UI�̃A�j���[�^�[

    [SerializeField] PlayerMove player;//Player�̈ړ��N���X�BGunStatus�̃A�N�Z�X�̂��߂Ɏ擾

    /// <summary>
    /// Player�����GunStatus�ɃA�N�Z�X
    /// </summary>
    GunStatus playerGunStatus => player.Gun;

    // Start is called before the first frame update
    void Start()
    {
        //SerializeField�Őݒ肳��ĂȂ����ɋN��
        if (player != null) return;
        //PlayerMove���擾
        player = FindPlayerMove();
    }

    /// <summary>
    /// �v���C���[��T�����ăt���O�Ǘ��ɕK�v�Ȍp�����擾
    /// </summary>
    /// <returns>PlayerMove(�V�[���ɒP��ő���)</returns>
    PlayerMove FindPlayerMove()
    {
        //Player�̃I�u�W�F�N�g��T��
        GameObject playerObject = GameObject.FindWithTag("Player");
        //Player�̃I�u�W�F�N�g����Transform��T��
        Transform childTransform = playerObject.transform.Find("perspective");
        //childTransform��null�`�F�b�N
        if (childTransform == null) return null;
        //PlayerMove���擾
        return childTransform.GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�c�i���X�V
        RemainText();
        //animator�̃p�����[�^�ݒ�
        animator.SetBool("MainGun", playerGunStatus.IsSubWeapon);

        //�ȉ�Debug�Ɏg�p
        //animator.SetBool("ChangeHead", PauseManager.IsSlow);
        //animator.SetBool("SetHead", !PauseManager.IsSlow);
    }

    /// <summary>
    /// ���𓊂����Ƃ��ɔ�������UI�J��
    /// </summary>
    public void AnimatorChangeHead()
    {
        //animator�̃p�����[�^�ݒ�
        animator.SetBool("ChangeHead", true);
        animator.SetBool("SetHead", false);
    }

    /// <summary>
    /// �����h�b�L���O�������ɔ�������UI�J��
    /// </summary>
    public void AnimatorSetHead()
    {
        //animator�̃p�����[�^�ݒ�
        animator.SetBool("ChangeHead", false);
        animator.SetBool("MainGun", playerGunStatus.IsSubWeapon);
        animator.SetBool("SetHead", true);
        //�E�G�|���̉摜�ύX
        GunsImage();
    }

    void GunsImage()
    {
        //�T�u�E�G�|���̎��摜��ύX
        if (player.Gun.IsSubWeapon == false)
            mainGunImage.sprite = playerGunStatus.WeaponImage;
    }

    void RemainText() 
    {
        //�c��̎c�i����\��
        remainBulletText.SetText("{0}", playerGunStatus.RemainBullets);
        //�c��̎c�i������F��ύX
        remainBulletText.color = SetUITextColor(playerGunStatus.RemainBullets);
    }

    Color SetUITextColor(int Bullets)
    {
        if (Bullets == 0)// �c�e��0�̂Ƃ�
        {
            return Color.red;//�ԐF
        }
        else // �c�e��0����񕜂����Ƃ��i����؂�ւ�������ڂ莞�j
        {
            return Color.white;//���F
        }
    }
}
