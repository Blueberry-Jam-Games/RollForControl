using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    private bool once = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!once)
            {
                Debug.Log("Triggering flow manager title screen");
                once = true;
                FlowManager.Instance.TitleScreenEnter();
            }
        }
    }
}
