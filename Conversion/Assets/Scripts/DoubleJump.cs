﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour
{

    public PlayerAbility move;
   



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }


    private void OnTriggerEnter(Collider other)
    {

        move.Resetsaut();

    }
}
