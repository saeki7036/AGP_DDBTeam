using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class SelectButtonMovement : SelectButtonWindow
{
    [SerializeField] RectTransform[] ButtonTransforms;//�{�^����RectTransform
    [SerializeField] float movementValue = 200f;//�{�^���̗�

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
            //�{�^���̈ʒu���ړ�������
            ButtonMovememts(buttonPos - currentButtonIndex);
            //Index��I������buttonPos�Ɉړ�
            currentButtonIndex = buttonPos;
            //�J�[�\����I������buttonPos�Ɉړ�
            cursol.transform.localPosition = Buttons[currentButtonIndex].transform.localPosition;    
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
        //�{�^���̈ʒu���ړ�������
        ButtonMovememts(next);
        //�J�[�\���̈ړ�
        cursol.transform.localPosition = Buttons[currentButtonIndex].transform.localPosition;
        //SE�Đ�
        audioSE?.PlayOneShot(audioSE.clip);
    }

    /// <summary>
    /// �{�^���̈ʒu���ړ�������
    /// </summary>
    /// <param name="count">�ړ���</param>
    void ButtonMovememts(int count)
    {
        //�{�^���̈ړ��ʂ��v�Z
        Vector3 movement = AddPositions(count);
        for(int i = 0; i < ButtonTransforms.Length; i++)
        {
            //�{�^���̈ʒu���ړ�
            ButtonTransforms[i].localPosition += movement;
        }
    }

    /// <summary>
    /// ��x�Ɉړ�����ړ��ʂ��擾
    /// </summary>
    /// <param name="moveValue">�ړ���</param>
    /// <returns>��x�Ɉړ�����ړ���</returns>
    Vector3 AddPositions(int moveValue)
    {
        //�{�^���̈ړ��ʂ��v�Z
        Vector3 add = Vector3.one * movementValue * moveValue;
        //z����0��
        add.z = 0f;
        //���͕����̐ݒ�ɉ����Ĉ������0�ɂ���
        if (input == input_or.Vertical)
        {
            add.x = 0f;//�c�����Ɉړ�
        }
        else
        {
            add.y = 0f;//�������Ɉړ�
        }
        //�Ō�Ƀ}�C�i�X��������
        add *= -1;
        //�v�Z�����l��Ԃ�
        return add;
    }
}
