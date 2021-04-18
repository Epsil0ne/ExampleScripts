using System.Collections;
using UnityEngine;

public class JumpAbility : MonoBehaviour
{
    [Header("Gameplay choices")]
    //determine if the player have to wait before he can perform the second jump or not
    [SerializeField] private bool forceDelayBeforeSecondJump = true;

    [Header("Parameters to tune")]
    [SerializeField] private float jumpSpeed = 11f;
    [SerializeField] private float distanceToGround = 0.2f; //error allowed between ground and character

    [Header("Object Setup")]
    //Points below the character used to determine when it is grounded or not.
    //the character is grounded when at least one point is close enough (see distanceToGround) to the ground
    [SerializeField] private Transform contact1;
    [SerializeField] private Transform contact2;


    private new Rigidbody rigidbody;
    private Animator animator;
    private DashAbility dashAbility;

    private bool mustJump = false;
    private bool isJumping; //used to force a delay before the second jump

    //be carefull: these two values update only when character try to jump
    private bool isInFirstJump = false;
    private bool isInSecondJump = false;

    ///==============================

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        dashAbility = GetComponent<DashAbility>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            mustJump = true;
    }

    private void FixedUpdate()
    {
        if (IsDashing()) {
            mustJump = false;
            return;
        }
        if (mustJump) {
            mustJump = false;
            TryToJump();
        }
    }

    private IEnumerator SetIsJumping()
    {
        isJumping = true;
        yield return new WaitForSeconds(0.4f);
        isJumping = false;
    }

    private bool IsGrounded()
    {
        //  Debug.DrawRay(logic.position, -Vector3.up * distanceToGround);
        return Physics.Raycast(contact1.position, -Vector3.up, distanceToGround) ||
                Physics.Raycast(contact2.position, -Vector3.up, distanceToGround);
    }

    private void TryToJump()
    {
        if (IsGrounded()) {
            isInFirstJump = true;
            isInSecondJump = false;
            Jump();
        }
        else if (isInFirstJump && !isInSecondJump && (!isJumping || !forceDelayBeforeSecondJump)) {
            isInFirstJump = false;
            isInSecondJump = true;
            Jump();
        }
    }

    private void Jump()
    {
        rigidbody.velocity += new Vector3(0, jumpSpeed, 0);
        animator.SetTrigger("Jump");
        StartCoroutine(SetIsJumping());
    }

    private bool IsDashing()
    {
        if (dashAbility == null) return false;
        else return dashAbility.CurrentState != 0;
    }
}



