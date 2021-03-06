﻿using UnityEngine;
using System.Collections;

public class RedRocketPathing : MonoBehaviour 
{

    public float speed;
    Vector3 moveDirection;
    CharacterController controller;
    private float diff;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () 
    {
        /**** Move Ship Input ****/
        float horizontal = 0;
        float vertical = 0;
        horizontal = (Mathf.Abs(Input.GetAxis("X axis")) > Mathf.Abs(Input.GetAxis("MoveHorizontalKey"))) ? Input.GetAxis("X axis") : Input.GetAxis("MoveHorizontalKey");
        vertical = (Mathf.Abs(Input.GetAxis("Y axis")) > Mathf.Abs(Input.GetAxis("MoveVerticalKey"))) ? Input.GetAxis("Y axis") : Input.GetAxis("MoveVerticalKey");

        if (horizontal != 0 || vertical != 0)
        {
            var relativePos = (transform.position + new Vector3(horizontal, 0, vertical)) - transform.position;
            var rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y - 90, stransform.eulerAngles.z);
            transform.rotation = rotation;
            moveDirection = new Vector3(horizontal, 0, vertical) * speed;
            controller.Move(moveDirection * Time.deltaTime);
        }
        else
        {   
            controller.Move(transform.forward * speed * Time.deltaTime);
        }

	}
}
