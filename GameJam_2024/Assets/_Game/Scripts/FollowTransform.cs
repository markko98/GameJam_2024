using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        var pos = target.position;
        pos.y = transform.position.y;
        transform.position = pos;
    }
}
