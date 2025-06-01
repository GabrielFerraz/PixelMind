using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car_Movement : MonoBehaviour
{
    public Transform transform;
    public float speed = 5f;

    void Start()
    {
        transform = GetComponent<Transform>();
    }

    void Update()
    {
        transform.position -= new Vector3(0, speed * Time.deltaTime, 0);

        if(transform.position.y <= -10)
        {
            Destroy(gameObject);
        }
    }
}