using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationUI : MonoBehaviour
{
    [SerializeField]
    private GameObject menuUI;

    [SerializeField]
    private RadioButtons characterGroup;

    [SerializeField]
    private RadioButtons weaponGroup;

    [SerializeField]
    private RadioButtons colourGroup;

    [SerializeField]
    private List<Sprite> waifuIcons;

    [SerializeField]
    private Image progressImage;

    [SerializeField]
    private float levelLength = 10f;

    private float initialX = 0f;

    private bool uiOpen;

    private GameObject playerRef;

    public Material laserColor;

    private void Start()
    {
        if (menuUI.activeInHierarchy) menuUI.SetActive(false);
        playerRef = GameObject.FindWithTag("Waifu");
        initialX = playerRef.transform.position.x;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!uiOpen)
            {
                // open
                uiOpen = true;
                menuUI.SetActive(true);
                PauseControl.Instance.PauseLayer(0);

                MapToPlayer();
            }
            else
            {
                // close
                uiOpen = false;
                menuUI.SetActive(false);
                PauseControl.Instance.UnpauseLayer(0);

                UpdateColor(colourGroup.GetSelected());

                UpdatePlayer();
            }
        }

        Debug.Log($"Started at {initialX}, at {playerRef.transform.position.x} went {playerRef.transform.position.x} distance out of {levelLength} for a percentage of {(playerRef.transform.position.x - initialX) / levelLength}");
        float percentComplete = Mathf.Abs((playerRef.transform.position.x - initialX) / levelLength);
        progressImage.rectTransform.anchoredPosition = new Vector3(percentComplete * 600f, 0.0f);
    }

    private void UpdateColor(int index)
    {
        if(index == 0)
        {
            laserColor.SetColor("_Color", Color.red);
        }
        else if(index == 1)
        {
            laserColor.SetColor("_Color", Color.yellow);
        }
        else if(index == 2)
        {
            laserColor.SetColor("_Color", Color.green);
        }
        else if(index == 3)
        {
            laserColor.SetColor("_Color", Color.blue);
        }
        else if(index == 4)
        {
            laserColor.SetColor("_Color", Color.cyan);
        }
        else if(index == 5)
        {
            laserColor.SetColor("_Color", Color.magenta);
        }
    }

    private void MapToPlayer()
    {
        RigidBodyMovement rbm = playerRef.GetComponent<RigidBodyMovement>();

        characterGroup.SelectValue(rbm.currentcharacter);
        weaponGroup.SelectValue(rbm.currentWand);
    }

    private void UpdatePlayer()
    {
        RigidBodyMovement rbm = playerRef.GetComponent<RigidBodyMovement>();

        rbm.SetCharacterAndWand(characterGroup.GetSelected(), weaponGroup.GetSelected());

        progressImage.sprite = waifuIcons[characterGroup.GetSelected()];
    }
}
