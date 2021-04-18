using System.Collections;
using UnityEngine;

public class FireBallItem : MonoBehaviour
{
    [Header("Parameters to tune")]
    [SerializeField] private float moveSpeed = 4;
    [SerializeField] private float maxLiveTime = 15;

    [Header("Object Setup")]
    [SerializeField] private GameObject launcherParticles;
    [SerializeField] private GameObject destructionParticles;

    ///==============================

    private void Awake()
    {
        Instantiate(launcherParticles, transform.position, Quaternion.identity);
        StartCoroutine(DestroyAfterDelay(maxLiveTime));
    }

    private void Update()
    {
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(destructionParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
