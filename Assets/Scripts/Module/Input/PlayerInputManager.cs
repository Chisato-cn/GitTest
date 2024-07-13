using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager
{
    private PlayerInputAction action = new PlayerInputAction();

    public virtual void OnEnable()
    {
        action?.Enable();
    }

    public virtual void OnDisable()
    {
        action?.Disable();
    }
}
