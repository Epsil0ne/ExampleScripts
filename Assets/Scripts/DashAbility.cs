using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * During a dash, only the camera and orientation can move.
 * The dash is interupted as soon as the dash button is released.
 * To have the complete dash sequence, the player must not release the button before the character has finished its dashing mouvement. It will automatiquely perform the dashing mouvement as soon as the charging time is finished
 */
public class DashAbility : MonoBehaviour
{
    [Header("Parameters to tune")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float chargingTime = 1.5f;
    [SerializeField] private float dachingDuration = 1.5f;

    [Header("Object Setup")]
    [SerializeField] private GameObject activationParticles;//boule bleue
    [SerializeField] private GameObject chargingParticles;
    [SerializeField] private GameObject dashingParticles;
    [SerializeField] private GameObject stoppingParticles;

    private new Rigidbody rigidbody;
    private Animator animator;

    GameObject currentParticles;
    /*
   0 Not dashing
   1 Activation
   2 Charging dash
   3 Dashing
   4 Stop dash
   */
    private int currentState = 0;
    public int CurrentState { get { return currentState; } }

    ///==============================

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Dash") && currentState == 0) 
            NextState();        

        if (Input.GetButtonUp("Dash") && (currentState == 1 || currentState == 2 || currentState == 3))
            InterruptDash();
    }

    private void FixedUpdate()
    {
        if (currentState == 3)
            rigidbody.velocity = transform.forward * dashSpeed;
    }

    private void InterruptDash()
    {
        Destroy(currentParticles);
        StopCoroutine("ActivationState1");
        StopCoroutine("ChargingState2");
        StopCoroutine("DashingState3");
        StopCoroutine("StoppingState4");
        ResetState();
    }

    private void NextState()
    {
        currentState++;
        animator.SetInteger("DashState", currentState);

        switch (currentState) {
            case 1:
            StartCoroutine("ActivationState1");
            break;
            case 2:
            StartCoroutine("ChargingState2");
            break;
            case 3:
            StartCoroutine("DashingState3");
            break;
            case 4:
            StartCoroutine("StoppingState4");
            break;

            default:
            break;
        }
    }

    private void ResetState()
    {
        currentState = 0;
        animator.SetInteger("DashState", currentState);
    }

    private IEnumerator ActivationState1()
    {
         currentParticles = Instantiate(activationParticles, getPosition(), Quaternion.identity, transform);
        yield return new WaitForSeconds(0.5f);
        Destroy(currentParticles);
        if (currentState != 0) NextState();
    }

    private IEnumerator ChargingState2()
    {
        currentParticles =  Instantiate(chargingParticles, getPosition(), Quaternion.identity, transform);
        yield return new WaitForSeconds(chargingTime);
        Destroy(currentParticles);
        if (currentState != 0) NextState();
    }

    private IEnumerator DashingState3()
    {
         currentParticles = Instantiate(dashingParticles, getPosition(), Quaternion.identity, transform);
        yield return new WaitForSeconds(dachingDuration);
        Destroy(currentParticles);
        if (currentState != 0) NextState();
    }

    private IEnumerator StoppingState4()
    {
        Instantiate(stoppingParticles, getPosition(), Quaternion.identity);
        rigidbody.velocity = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(0.1f);
        ResetState();
    }

    private Vector3 getPosition()
    {
        return transform.position + new Vector3(0, 1.5f, 0);
    }
}
