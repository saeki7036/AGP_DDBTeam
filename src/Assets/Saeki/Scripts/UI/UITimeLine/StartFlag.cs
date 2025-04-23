using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class StartFlag : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] string animText;

    private bool startCheck;

    // Start is called before the first frame update
    void Start()
    {
        //timeScale��0�ɂ���FixedUpdete���~�߂�
        Time.timeScale = 0;
        //������
        startCheck = false;
    }
   
    // Update is called once per frame
    void Update()
    {
        //���N���b�N�Ǝl�����̈ړ����擾
        bool MouseLeft = Input.GetMouseButton(0);
        float inputVertical = Input.GetAxisRaw("Vertical");
        float inputHorizontal = Input.GetAxisRaw("Horizontal");
       
        //���N���b�N�Ǝl�����̓��͂𔻒�
        if (!startCheck && (inputVertical != 0 || inputHorizontal != 0 || MouseLeft))
        {
            //animation���N��
            AnimationOnePlay();
            //timeScale��1�ɂ���FixedUpdete�𓮂���
            Time.timeScale = 1;

            //TimeLine�g�p�̂��߂������
            //StartCounting();
        }
    }

    void AnimationOnePlay()
    {
        //�A�j���[�V�����N��
        anim.Play(animText, 0, 1f);
    }

    //TimeLine�g�p�̂��߂������

    /*
    /// <summary>
    /// ��莞�Ԍ�ɔ�\���ɐݒ�
    /// </summary>
    async Task StartCounting()
    {
        if (startCheck)
        {
            Debug.LogWarning("���ɃJ�E���g���ł��I");
            return;
        }

        startCheck = true;
       
        float startTime = Time.realtimeSinceStartup;

        // �񓯊��őҋ@
        while (Time.realtimeSinceStartup - startTime < 0.2f)
        {
            await Task.Yield(); // �񓯊��Ŏ��̃t���[���܂őҋ@
        }

        this.gameObject.SetActive(false);
    }*/
}
