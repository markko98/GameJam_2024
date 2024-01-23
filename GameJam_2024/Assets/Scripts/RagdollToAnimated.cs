using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollToAnimated : MonoBehaviour
{
    public Collider collider;
    public float respawnTime = 30f;

    private Rigidbody[] rigibodies;
    private bool isRagdoll;
    private Animator animator;
    
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Walking = Animator.StringToHash("Walking");

    private Camera camera;


    private CharacterController characterController;
    private float speed = 5f;
    private float turnSpeed = 20f;

    void Start()
    {
        rigibodies = GetComponentsInChildren<Rigidbody>();
        animator = GetComponent<Animator>();

        characterController = GetComponent<CharacterController>();
        ToggleRagdoll(true);

        camera = Camera.main;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!isRagdoll)
        {
            ToggleRagdoll(false);
            StartCoroutine(GetBackUp());
        }
    }

    private IEnumerator GetBackUp()
    {
        yield return new WaitForSeconds(respawnTime);
        ToggleRagdoll(true);
    }

    private void ToggleRagdoll(bool isAnimating)
    {
        isRagdoll = !isAnimating;

        collider.enabled = isAnimating;
        foreach (var bone in rigibodies)
        {
            bone.isKinematic = isAnimating;
        }

        animator.enabled = isAnimating;
        if (isAnimating)
        {
            animator.SetBool(Idle, true);            
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleRagdoll(false);
            StartCoroutine(GetBackUp());
        }

        if (isRagdoll)
        {
            animator.SetBool(Idle, true);
            animator.SetBool(Walking, false);
            return;
        }
        
        var moveVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        characterController.Move(moveVector * (speed * Time.deltaTime));
        
        Vector3 currentRotation = new Vector3(Input.GetAxis("Horizontal"), Quaternion.identity.y, Input.GetAxis("Vertical"));
        Quaternion lookRotation = Quaternion.LookRotation(currentRotation);
       
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
        
        
        animator.SetBool(Idle, moveVector == Vector3.zero);
        animator.SetBool(Walking, moveVector != Vector3.zero);

        
    }
}
