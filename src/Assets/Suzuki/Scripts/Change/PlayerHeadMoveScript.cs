using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerHeadMoveScript : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;

    Change change;
    CinemachineVirtualCamera virtualCamera;

    GameObject vCamera;
    CinemachineFramingTransposer framingTransposer;
    float headDistance = 5.0f;
    float playerDistance = 0.3f;

    void Start()
    {
        Debug.Log("Start");
        change = GameObject.FindObjectOfType<Change>();
        //vCamera = GameObject.FindWithTag("VCamera");
        //virtualCamera= vCamera.GetComponent<CinemachineVirtualCamera>();
        //virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }
    public IEnumerator MoveHead(Vector3 start, Vector3 end, GameObject changeObj)
    {
        transform.LookAt(end);
        ChangeCameraTarget(transform, headDistance);

        float totalTime = Vector3.Distance(start, end) / moveSpeed;
        float timer = 0f;

        while(timer < totalTime)
        {
            timer += Time.deltaTime;

            Vector3 position = Vector3.Lerp(start, end, timer / totalTime);
            transform.position = position;
            yield return null;
        }
        change.ChangeCameraTarget(changeObj);
        ChangeCameraTarget(change.gameObject.transform, playerDistance);
        Destroy(gameObject);
    }

    void ChangeCameraTarget(Transform target,float distance)
    {
        Debug.Log("target:" + target.name);

        //change = GameObject.FindObjectOfType<Change>();
        vCamera = GameObject.FindWithTag("VCamera");
        virtualCamera = vCamera.GetComponent<CinemachineVirtualCamera>();
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        virtualCamera.Follow = target;
        virtualCamera.LookAt = target;
        framingTransposer.m_CameraDistance = distance;
    }
}
