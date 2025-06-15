 using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class Road_Movement : MonoBehaviour

{

    public Renderer meshRenderer;

    public float speed = 1f;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = 0.5f;
    }



    

    void Update()

    {

        meshRenderer.material.mainTextureOffset += new Vector2(0, speed * Time.deltaTime);
        audioSource.pitch = Mathf.Lerp(0.5f, 1f, speed);
    }

}


