using System;
using UnityEngine;

public class CameraMouvementController : MonoBehaviour
{
    [Header("Parameters to tune")]
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float VerticalRotationSpeed = 300f;
    [SerializeField] private float maxVerticalAngle = 20f;

    [Header("Object Setup")]
    [SerializeField] private Transform targetCameraPosition;
    [SerializeField] private Transform cameraPivot;


    private Vector3 velocity = Vector3.zero;
    private int layerMask;

    ///==============================

    private void Awake()
    {
        maxVerticalAngle = Mathf.Abs(maxVerticalAngle);//force value to be positive

        //The layer Player with not be taken into account during the obstacle checking
        layerMask = LayerMask.GetMask("Player");
        layerMask = ~layerMask;

        //initialise the camera at the right position
        transform.position = CorrectPositionIfObstacles(targetCameraPosition.transform.position);
    }

    private void Update()
    {
        HandleYrotation();

        Vector3 targetPosition = targetCameraPosition.transform.position;
        targetPosition = CorrectPositionIfObstacles(targetPosition);

        //smooth version of transform.position = targetPosition;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        //smooth version of transform.rotation = targetCameraPosition.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetCameraPosition.rotation, smoothTime);
    }

    private Vector3 CorrectPositionIfObstacles(Vector3 cameraTargetPosition)
    {       
        RaycastHit hit = new RaycastHit();

        //the linecast begin at the character head
        Vector3 characterPosition = cameraPivot.position + new Vector3(0, 1.7f, 0);
        if (Physics.Linecast(characterPosition, cameraTargetPosition, out hit, layerMask))
            return hit.point;

        return cameraTargetPosition;
    }

    private void HandleYrotation()
    {
        Vector3 oldAngles = cameraPivot.transform.eulerAngles;
        float newAngleX = oldAngles.x + (-Input.GetAxis("Mouse Y") * Time.deltaTime * VerticalRotationSpeed);

        newAngleX = ClampAngle(newAngleX);
        cameraPivot.transform.eulerAngles = new Vector3(newAngleX, oldAngles.y, 0);
    }

    private float ClampAngle(float angle)
    {
        if (IsBetween(angle, maxVerticalAngle, 180)) {
            angle = maxVerticalAngle;
        }
        else if ((IsBetween(angle, -180, -maxVerticalAngle)) ||
            IsBetween(angle, 180, 360 - maxVerticalAngle)) {
            angle = 360 - maxVerticalAngle;
        }

        return angle;
    }

    private static bool IsBetween(float evaluated, float min, float max)
    {
        return evaluated < max && evaluated > min;
    }
}
