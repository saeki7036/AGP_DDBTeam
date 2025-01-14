using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameras : MonoBehaviour
{
    // �q�G�����L�[���Cinemachine�J�������V���A���C�Y�Ŏw��
    [SerializeField] private CinemachineFreeLook freeLookCamera;

    public CinemachineVirtualCamera v;
    // X����Y���̃X�s�[�h���V���A���C�Y
    [SerializeField] private float xAxisSpeed = 2f;
    [SerializeField] private float yAxisSpeed = 2f;

    void Start()
    {      
            var pov = v.GetCinemachineComponent<CinemachinePOV>();
       
            pov.m_HorizontalAxis.m_MaxSpeed = xAxisSpeed;
            pov.m_VerticalAxis.m_MaxSpeed = yAxisSpeed;              
    }
}
