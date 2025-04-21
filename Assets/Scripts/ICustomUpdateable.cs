using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICustomUpdateable
{
    void CustomStart();
    void CustomUpdate();
    void CustomFixedUpdate();
    void CustomLateUpdate();
    void CustomOnPause();
}

