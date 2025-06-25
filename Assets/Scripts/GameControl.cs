using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public static GameControl Ins;
    public CharactersControl characterController;
    public ARTapToPlaceObject ARTapToPlaceObject;
    public UIManager UIManager;
    public SkillManager SkillManager;

    public bool isBattled;

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
        if(Ins == null)
        {
            Ins = this;
        }    
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Bạn đã nhấn vào: " + hit.collider.gameObject.name);
                ClickOnCharater(hit.collider.gameObject.GetComponent<Character>());
            }
            else
            {
                UIManager.ShowUIWhenSelectCharacter(false);
            }    
        }
#else
        // Touch trên thiết bị di động
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
           if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                Debug.Log("B----- ");
                return;
            }

            Ray ray = mainCam.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Bạn đã nhấn vào: " + hit.collider.gameObject.name);
                ClickOnCharater(hit.collider.gameObject.GetComponent<Character>());
            }
            else
            {
                UIManager.ShowUIWhenSelectCharacter(false);
            }    
        }
#endif
    }

    private void ClickOnCharater(Character character)
    {
        if(character)
        {
            characterController.SelectCharacter(character);
            UIManager.ShowUIWhenSelectCharacter(true);
        }
    }

    public void StartBattle()
    {
        isBattled = true;
        characterController.StartBattle();
    }

    public void CheckEndGame()
    {
        if (!characterController.ValidateEnemyAlive(out int deadCount))
        {
            isBattled = false;
            UIManager.ShowResult(true, deadCount);
        }
    }

    public void RestartGame()
    {
        characterController.RemoveAllCharacters();
        UIManager.ResetUI();
    }
}