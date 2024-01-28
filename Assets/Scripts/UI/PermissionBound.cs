using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermissionBound : MonoBehaviour
{
    public string permission;

    public void RespondToPermissions()
    {
        if (FlowManager.Instance.CheckPermission(permission))
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        RespondToPermissions();
    }
}
