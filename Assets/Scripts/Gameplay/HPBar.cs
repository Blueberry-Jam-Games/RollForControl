using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField]
    private GameObject fillHP;

    private void Start()
    {
        fillHP.transform.localScale = Vector3.one;
    }

    public void UpdateHealth(float percentLeft)
    {
        if (percentLeft < Mathf.Epsilon)
        {
            percentLeft = 0.0f;
        }

        fillHP.transform.localScale = new Vector3(percentLeft, 1.0f, 1.0f);
    }

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        UpdateHealth(currentHealth / maxHealth);
    }
}
