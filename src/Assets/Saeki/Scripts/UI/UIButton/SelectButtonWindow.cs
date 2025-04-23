using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;

public class SelectButtonWindow : MonoBehaviour
{
    [SerializeField] protected Button[] Buttons;//���삷��{�^��
    [SerializeField] protected GameObject cursol;//�J�[�\���̃I�u�W�F�N�g
    [SerializeField] protected AudioSource audioSE;//���ʉ�
    [SerializeField] protected input_or input = input_or.Vertical;//�C���v�b�g�̓��͕���
    [SerializeField] protected int currentButtonIndex = 0;//�ŏ���Index�̈ʒu

    [SerializeField] float InputResponseValue = 0.3f;//���͂̃p�����[�^
    [SerializeField] float InputRetryValue = 0.1f;//�ē��͂̃p�����[�^

    //�C���v�b�g�̓��͏��
    protected enum ButtonType
    {
        Normal,
        Up,
        Down
    }
    //�C���v�b�g�̓��͕���
    protected enum input_or
    {
        Vertical,
        Horizontal
    }

    protected ButtonType buttonType = ButtonType.Normal;

    /// <summary>
    /// �{�^����Index���ړ�������(�O������̓��͗p)
    /// </summary>
    /// <param name="buttonPos">�ʒu</param>
    public virtual void PushOnlyButtonEvent(int buttonPos)
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
        }
    }

    // Update is called once per frame
    void Update()
    {
        //���͂��擾
        float inputValue = GetInputAxis(); Debug.Log(inputValue);
        //���͂�int�ɕϊ�
        int NextSelect = GetNextSelect(inputValue);
        //Index���ɐݒ�
        SetNextSelect(NextSelect);
        //�ē��͂̔����ݒ�
        buttonType = ResetButtonType(inputValue);
        //�{�^���̋N���̔���
        EnterButton();
    }

    float GetInputAxis()
    {
        //input_or�ŉ��Ŕ������邩�c�Ŕ������邩�ύX
        return input == input_or.Vertical ? Input.GetAxisRaw("Vertical") : -Input.GetAxisRaw("Horizontal");
    }

    int GetNextSelect(float value)
    {
        //���͗ʂ𔻒�B���ȉ��Ȃ�return
        if (Math.Abs(value) < InputResponseValue) return 0;
        //���͂ł��邩�𔻒�B���͂����ꂽ�܂܂Ȃ�return
        if (buttonType != ButtonType.Normal) return 0;
        //���͗ʂ���1or-1�ɐ��K��
        int NextButtonIndex = value < 0 ? 1 : -1;
        //�͈͊O�`�F�b�N(Button�̐��`0�̊Ԃ̊O�ɏo����return)
        if (NextButtonIndex + currentButtonIndex >= Buttons.Length ||
           NextButtonIndex + currentButtonIndex < 0) return 0;
        //���͗ʂ�����͒l��Ԃ�
        return NextButtonIndex;

    }

    protected virtual void SetNextSelect(int next)
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
    }

    /// <summary>
    /// ���͏�Ԃɂ����ɖ߂�
    /// </summary>
    /// <param name="value">����</param>
    /// <returns>�ē��͂��ł���Ȃ�Normal</returns>
    ButtonType ResetButtonType(float value)
    {
        //���͗ʂ���ē��͂ł���܂ł̔���
        if (Math.Abs(value) < InputRetryValue)
            if (buttonType != ButtonType.Normal)
                return ButtonType.Normal;//�����͏�ԂɕύX
        //���̓��͂�Ԃ�
        return buttonType;
    }

    void EnterButton()
    {
        //�Q�[���p�b�h�̃{�^�����������̓X�y�[�X�L�[
        if (Input.GetKeyDown("joystick button 0") || Input.GetKeyDown("space"))
        {
            //Button�̃C�x���g�����s
            ButtonInvoke();
        }
    }

    protected void ButtonInvoke()
    {
        //Button����o�^���Ă���C�x���g�����s
        Buttons[currentButtonIndex].onClick.Invoke();
    }
}
