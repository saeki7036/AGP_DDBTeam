using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectButtonBetween : SelectButtonWindow
{
    [SerializeField] RectTransform LeftIcon;//�J�[�\���̍��̈ʒu
    [SerializeField] RectTransform RightIcon;//�J�[�\���̉E�̈ʒu
    [SerializeField] float IconAdjustmentValue = 20f;//�{�^���ƃJ�[�\���܂ł̋���

    /// <summary>
    /// �{�^����Index���ړ�������(�O������̓��͗p)
    /// </summary>
    /// <param name="buttonPos">�ʒu</param>
    public override void PushOnlyButtonEvent(int buttonPos)
    {
        //�I������buttonPos�ƌ��݂̃{�^���̈ʒu�������Ȃ�return
        if (buttonPos == currentButtonIndex) return;
        //�O�����͂���͈͊O�Ȃ�return
        if (buttonPos < 0 || buttonPos >= Buttons.Length) return;
        //SE�Đ�
        audioSE?.PlayOneShot(audioSE.clip);
        //�I������buttonPos�ƌ��݂̃{�^���̈ʒu�������Ȃ�
        if (currentButtonIndex == buttonPos)
        {
            //Button�̃C�x���g�����s
            ButtonInvoke();
        }
        else
        {
            //Index��I������buttonPos�Ɉړ�
            currentButtonIndex = buttonPos;
            //�J�[�\����I������buttonPos�Ɉړ�
            cursol.transform.localPosition = Buttons[currentButtonIndex].transform.localPosition;
            //�J�[�\���A�C�R���̏���
            SetIconBetween();
        }  
    }

    protected override void SetNextSelect(int next)
    {
        //���͂�0�Ȃ�return
        if (next == 0) return;
        //Index���ړ�
        currentButtonIndex += next;
        //���͂��󂯕t���̐ݒ�
        buttonType = next < 0 ? ButtonType.Up : ButtonType.Down;
        //�J�[�\���̈ړ�
        cursol.transform.localPosition = Buttons[currentButtonIndex].transform.localPosition;
        //SE�Đ�
        audioSE?.PlayOneShot(audioSE.clip);
        //�J�[�\���A�C�R���̏���
        SetIconBetween();       
    }

    /// <summary>
    /// Icon�̗��[�̈ʒu��ݒ�
    /// </summary>
    void SetIconBetween()
    {
        //�{�^����RectTransform���擾
        RectTransform button = Buttons[currentButtonIndex].GetComponent<RectTransform>();
        //�{�^���̃J�[�\���̈ʒu���v�Z(button�̉����̔����ɋ����p�����[�^[IconAdjustmentValue]�����Z)
        Vector3 iconPos = Vector3.right * (button.rect.width/2) + Vector3.right * IconAdjustmentValue;
        //�{�^���̃J�[�\���ɑ��
        RightIcon.localPosition = iconPos;
        //�{�^���̃J�[�\���Ƀ}�C�i�X��t���ĉE�Ƌt�̈ʒu�ɑ��
        LeftIcon.localPosition = -iconPos;
    }
}
