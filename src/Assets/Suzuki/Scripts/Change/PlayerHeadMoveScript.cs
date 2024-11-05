using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerHeadMoveScript : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;

    Change change;

    void Start()
    {
        change = GameObject.FindObjectOfType<Change>();
    }
    public IEnumerator MoveHead(Vector3 start, Vector3 end, GameObject changeObj)
    {
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
        Destroy(gameObject);
    }
}
