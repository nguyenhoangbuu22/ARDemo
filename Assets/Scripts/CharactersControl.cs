using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharactersControl : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotateDuration = 0.2f;
    public float jumpForce = 1;
    public bool jumping = false;


    [SerializeField] private Button btnTop;
    [SerializeField] private Button btnBottom;
    [SerializeField] private Button btnLeft;
    [SerializeField] private Button btnRight;
    [SerializeField] private Button btnJump;
    [SerializeField] private Button btnAtk;

    private Character currentCharacter;
    private List<Character> Characters = new();

    private Vector3 moveDirection = Vector3.zero;

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Onjump();
        }
#endif

        if (currentCharacter == null || moveDirection == Vector3.zero || jumping)
            return;
        Vector3 targetPos = currentCharacter.transform.position + moveDirection * moveSpeed * Time.deltaTime;

        currentCharacter.transform.DOKill();
        currentCharacter.transform.DOMove(targetPos, 0.1f).SetEase(Ease.Linear);

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        currentCharacter.transform.DORotateQuaternion(targetRotation, rotateDuration);
    }


    public void OnTopDown()
    {
        currentCharacter.MoveCharacter(true);
        moveDirection = Vector3.forward;
    }
    public void OnBottomDown()
    {
        currentCharacter.MoveCharacter(true);
        moveDirection = Vector3.back;
    }
    public void OnLeftDown()
    {
        currentCharacter.MoveCharacter(true);
        moveDirection = Vector3.left;
    }
    public void OnRightDown()
    {
        currentCharacter.MoveCharacter(true);
        moveDirection = Vector3.right;
    }
    public void OnAttack()
    {
        currentCharacter.Attack();
    }

    public void OnButtonUp()
    {
        currentCharacter.MoveCharacter(false);
        moveDirection = Vector3.zero;
    }

    public void Onjump()
    {
        if(currentCharacter.IsGrounded())
        {
            jumping = true;
            float jumpHeight = currentCharacter.transform.position.y + jumpForce;
            currentCharacter.transform.DOMoveY(jumpHeight, 0.7f).SetEase(Ease.OutQuad);
            DOVirtual.DelayedCall(0.9f, () =>
            {
                jumping = false;
            });
        }
    }

    public void SelectCharacter(Character character)
    {
        currentCharacter = character;
        ResetCharacters();
        character.ShowSelected();
    }

    public void AddACharacter(Character character)
    {
        Characters.Add(character);
    }

    public void RemoveACharacter(Character character)
    {
        Characters.Remove(character);
        character.DestroyCharacter();
    }

    public void ResetCharacters()
    {
        Characters.ForEach(c => c.HideSelected());
    }

    public void RemoveCurrCharacter()
    {
        RemoveACharacter(currentCharacter);
    }

}
