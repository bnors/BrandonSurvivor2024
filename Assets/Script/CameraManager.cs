using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    GameObject target;
    float moveSpeed = 0.75f;
    void Start()
    {
        target = Player.GetInstance().gameObject;
    }

    private void FixedUpdate()
    {
        FollowTarget();
    }


    private void FollowTarget()
    {
        if (target != null)
        {
            Vector3 FinalPosition = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z - 10);
            Vector3 transitionPosition = Vector3.Lerp(transform.position, FinalPosition, moveSpeed * Time.fixedDeltaTime);
            transform.position = transitionPosition;
        }
    }
}
