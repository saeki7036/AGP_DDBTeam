using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainGameGunsUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI remainBulletText;
    [SerializeField] Image mainGunImage, subGunImage;
    [SerializeField] Animator animator;
    [SerializeField] PlayerMove player;
    bool IsSubGun;
    // Start is called before the first frame update
    void Start()
    {
        IsSubGun = false;
        //SerializeField�Őݒ肳��ĂȂ����ɋN��
        if (player != null) return;

        player = FindPlayerMove();
    }
    /// <summary>
    /// �v���C���[��T�����ăt���O�Ǘ��ɕK�v�Ȍp�����擾
    /// </summary>
    /// <returns>PlayerMove(�V�[���ɒP��ő���)</returns>
    PlayerMove FindPlayerMove()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        Transform childTransform = playerObject.transform.Find("perspective");
        if (childTransform == null) return null;
        return childTransform.GetComponent<PlayerMove>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        RemainText();
        animator.SetBool("MainGun", player.Gun.IsSubWeapon);
        //animator.SetBool("ChangeHead", PauseManager.IsSlow);
        //animator.SetBool("SetHead", !PauseManager.IsSlow);
    }

    /// <summary>
    /// ���𓊂����Ƃ��ɔ�������UI�J��
    /// </summary>
    public void AnimatorChangeHead()
    {
        animator.SetBool("ChangeHead", true);
        animator.SetBool("SetHead", false);
    }

    /// <summary>
    /// �����h�b�L���O�������ɔ�������UI�J��
    /// </summary>
    public void AnimatorSetHead()
    {
        animator.SetBool("ChangeHead", false);
        animator.SetBool("MainGun", player.Gun.IsSubWeapon);
        animator.SetBool("SetHead", true);

        GunsImage();
    }
    void GunsImage()
    {
        //�T�u�E�G�|���̎��摜��ύX
        if (player.Gun.IsSubWeapon == false)
            mainGunImage.sprite = player.Gun.WeaponImage;
    }

    void RemainText() 
    {
        //�c��̎c�i����\��
        remainBulletText.SetText("{0}", player.Gun.RemainBullets);
        remainBulletText.color = SetUITextColor(player.Gun.RemainBullets);
    }

    Color SetUITextColor(int Bullets)
    {
        if (Bullets == 0)// �c�e��0�̂Ƃ�
        {
            return Color.red;
        }
        else // �c�e��0����񕜂����Ƃ��i����؂�ւ�������ڂ莞�j
        {
            return Color.white;
        }
    }
}
