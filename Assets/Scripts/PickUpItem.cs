using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour {

    [Header("Gameplay choices")]
    [SerializeField] private bool isHealth = true;//if false, it is Power.
    [SerializeField] private int amount = 5;

    [Header("Object Setup")]
    [SerializeField] private GameObject destructionParticles;

    ///==============================
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
        
        PlayerStateController playerStateController = other.GetComponent<PlayerStateController>();

        if (playerStateController != null) { 
        if (isHealth)
            playerStateController.FillHealth(amount);
        else
            playerStateController.FillPower(amount);
        }

        Disappear();
    }

    private void Disappear()
    {
        Instantiate(destructionParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
