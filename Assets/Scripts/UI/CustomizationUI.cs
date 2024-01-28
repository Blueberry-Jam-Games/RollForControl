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

                UpdatePlayer();
            }
        }

        Debug.Log($"Started at {initialX}, at {playerRef.transform.position.x} went {playerRef.transform.position.x} distance out of {levelLength} for a percentage of {(playerRef.transform.position.x - initialX) / levelLength}");
        float percentComplete = Mathf.Abs((playerRef.transform.position.x - initialX) / levelLength);
        progressImage.rectTransform.anchoredPosition = new Vector3(percentComplete * 600f, 0.0f);
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
