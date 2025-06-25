using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button btnCreateCharacter;
    [SerializeField] private Button btnRemoveCharacter;
    [SerializeField] private Button btnOptions;
    [SerializeField] private Button btnEdit;
    [SerializeField] private Button btnShowControlChar;
    [SerializeField] private Button btnStartBattle;
    [SerializeField] private GameObject objPopup;
    [SerializeField] private GameObject objControl;
    [SerializeField] private GameObject objEdit;
    [SerializeField] private PopupResult popupResult;

    private void Start()
    {
        btnRemoveCharacter.onClick.AddListener(OnClickRemoveCharacter);
        btnCreateCharacter.onClick.AddListener(OnClickCreaterCharacter);
        btnOptions.onClick.AddListener(OnClickOptions);
        btnEdit.onClick.AddListener(OnClickEdit);
        btnShowControlChar.onClick.AddListener(OnClickControlChar);
        btnStartBattle.onClick.AddListener(OnClickStartBattle);
        ShowUIWhenSelectCharacter(false);
        Init();
    }

    private void Init()
    {
        objPopup.SetActive(false);
        objEdit.SetActive(true);
        objControl.SetActive(false);
        popupResult.Hide();
    }

    private void OnClickEdit()
    {
        objPopup.SetActive(false);
        objEdit.SetActive(true);
        objControl.SetActive(false);
    }

    private void OnClickControlChar()
    {
        objPopup.SetActive(false);
        objEdit.SetActive(false);
        objControl.SetActive(true);
    }

    private void OnClickStartBattle()
    {
        GameControl.Ins.characterController.StartBattle();
        OnClickControlChar();
    }

    private void OnClickOptions()
    {
        objPopup.SetActive(!objPopup.activeSelf);
    }

    public void ShowUIWhenSelectCharacter(bool isSelect)
    {
        btnCreateCharacter.gameObject.SetActive(!isSelect);
        btnRemoveCharacter.gameObject.SetActive(isSelect);
    }

    public void OnClickRemoveCharacter()
    {
        GameControl.Ins.characterController.RemoveCurrCharacter();
        ShowUIWhenSelectCharacter(false);
    }

    public void OnClickCreaterCharacter()
    {
        GameControl.Ins.ARTapToPlaceObject.CreateCharacter();
    }

    public void ResetUI()
    {
        Init();
    }

    public void ShowResult(bool isWin, int point)
    {
        popupResult.Show(isWin, point);
    }
}
