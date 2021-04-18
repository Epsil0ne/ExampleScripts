using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour {
   
    [Header("UI")]
    //min 0% max 100%
    [SerializeField] private int health = 50;
    [SerializeField] private int power = 50;

    ///==============================

    public void ConsumeHealth (int amount)
    {       
        if ((health - amount) < 0) health = 0;//player is dead;
        else health -= amount;       
    }

    public void ConsumePower(int amount)
    {
        if ((power - amount) < 0) power = 0;
        else power -= amount;      
    }

    public void FillHealth(int amount)
    {
        if ((health + amount) > 100) health = 100;
        else health += amount;
    }

    public void FillPower(int amount)
    {
        if ((power+ amount) > 100) power = 100;
        else power += amount;
    }
    
    public bool CanConsumePower(int amount)
    {
        return amount<= power;
    }    
}
