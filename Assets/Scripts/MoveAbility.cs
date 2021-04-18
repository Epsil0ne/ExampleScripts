using UnityEngine;

public class MoveAbility : MonoBehaviour
{   
    [Header("Parameters to tune")]
    [SerializeField] private float HorizontalRotationSpeed = 60f;
    [SerializeField] private float walkSpeed = 6f;


    private Rigidbody rigidBody;
    private Animator animator;
    private DashAbility dashAbility;

    ///==============================

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        dashAbility = GetComponent<DashAbility>();
    }
    
    private void Update()
    {
        //rotate horizontaly the character
        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * Time.deltaTime * HorizontalRotationSpeed);
    }

    private void FixedUpdate()
    {
        if (IsDashing()) return;

        
        Vector3 mouvement = new Vector3(0, 0, 0);

        mouvement += LongitudinalMovement();
        mouvement += TransversalMovement();

        mouvement.y = rigidBody.velocity.y;//to keep falling from gravity 

        rigidBody.velocity = mouvement;
    }

    private Vector3 LongitudinalMovement()
    {
        animator.SetFloat("LongitudinalSpeed", Input.GetAxis("Longitudinal"));
        return Input.GetAxisRaw("Longitudinal") * transform.forward * walkSpeed;

    }

    private Vector3 TransversalMovement()
    {
        animator.SetFloat("TransversalSpeed", Input.GetAxis("Transversal"));
        return Input.GetAxisRaw("Transversal") * transform.right * walkSpeed;
    }

    private bool IsDashing()
    {
        if (dashAbility == null) return false;
        else
            return dashAbility.CurrentState != 0;
    }
}
