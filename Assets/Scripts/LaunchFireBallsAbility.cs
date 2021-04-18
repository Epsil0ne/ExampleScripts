using System.Collections;
using UnityEngine;

public class LaunchFireBallsAbility : MonoBehaviour
{
    [Header("Parameters to tune")]
    [SerializeField] private float delayBetweenAttacks = 0.4f;

    [Header("Object Setup")]
    [SerializeField] private GameObject fireBallPrefab;
    [SerializeField] private Transform launchingPoint;
    [SerializeField] private Transform cameraLookAt; //to determine the launch angle of the projectile


    private Animator animator;
    private DashAbility dashAbility;

    private bool canAttack = true; //used to force a delay between attack

    ///==============================

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        dashAbility = GetComponent<DashAbility>();
    }

    private void FixedUpdate()
    {
        if (IsDashing()) return;

        bool buttonPressed = Input.GetButton("Fireball");

        animator.SetBool("PressFireBall", buttonPressed);

        if (!buttonPressed) animator.ResetTrigger("FireBall");
        else if (buttonPressed && canAttack) Attack();
    }

    private void Attack()
    {
        animator.SetTrigger("FireBall");

        Instantiate(fireBallPrefab, launchingPoint.position, Quaternion.Euler(cameraLookAt.eulerAngles.x, transform.eulerAngles.y, 0));

        StartCoroutine(SetIsAttacking());
    }

    private IEnumerator SetIsAttacking()
    {
        canAttack = false;
        yield return new WaitForSeconds(delayBetweenAttacks);
        canAttack = true;
    }

    private bool IsDashing()
    {
        if (dashAbility == null) return false;
        else return dashAbility.CurrentState != 0;
    }
}
