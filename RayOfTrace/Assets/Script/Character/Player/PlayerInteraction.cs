﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private InteractionObject interactionObject = null;

    public InteractionObject _InteractionObject
    {
        set
        {
            this.interactionObject = value;
        }
        get
        {
            return this.interactionObject;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            Interaction();
    }

    public void Interaction()
    {
        if (interactionObject == null)
            return;

        interactionObject.Interaction();
    }
}

public interface InteractionObject
{
    void Interaction();
}