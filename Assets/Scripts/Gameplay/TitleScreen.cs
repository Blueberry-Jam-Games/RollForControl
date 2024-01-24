using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    private bool once = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!once)
            {
                once = true;
                FlowManager.Instance.TitleScreenEnter();
            }
        }
    }
}
