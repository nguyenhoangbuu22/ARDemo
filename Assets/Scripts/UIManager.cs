using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button btnCreateCharacter;
    [SerializeField] private Button btnRemoveCharacter;

    private void Start()
    {
        btnRemoveCharacter.onClick.AddListener(OnClickRemoveCharacter);
        btnCreateCharacter.onClick.AddListener(OnClickCreaterCharacter);
        ShowUIWhenSelectCharacter(false);
    }

    public void ShowUIWhenSelectCharacter(bool isSelect)
    {
        btnCreateCharacter.gameObject.SetActive(!isSelect);
        btnRemoveCharacter.gameObject.SetActive(isSelect);
    }

    public void OnClickRemoveCharacter()
    {
        GameControl.Ins.characterController.RemoveCurrCharacter();
    }

    public void OnClickCreaterCharacter()
    {
        GameControl.Ins.ARTapToPlaceObject.CreateCharacter();
    }
}
