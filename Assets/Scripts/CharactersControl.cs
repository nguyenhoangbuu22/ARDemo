using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
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
        if (currentCharacter == null) return;
        currentCharacter.MoveCharacter(true);
        moveDirection = GetCameraRelativeDirection(Vector3.forward);
    }
    public void OnBottomDown()
    {
        if (currentCharacter == null) return;
        currentCharacter.MoveCharacter(true);
        moveDirection = GetCameraRelativeDirection(Vector3.back);
    }
    public void OnLeftDown()
    {
        if (currentCharacter == null) return;
        currentCharacter.MoveCharacter(true);
        moveDirection = GetCameraRelativeDirection(Vector3.left);
    }
    public void OnRightDown()
    {
        if (currentCharacter == null) return;
        currentCharacter.MoveCharacter(true);
        moveDirection = GetCameraRelativeDirection(Vector3.right);
    }
    public void OnAttack()
    {
        if (currentCharacter == null) return;
        currentCharacter.Attack();
    }

    public void OnButtonUp()
    {
        if (currentCharacter == null) return;
        currentCharacter.MoveCharacter(false);
        moveDirection = Vector3.zero;
    }

    public void Onjump()
    {
        if(currentCharacter == null) return;
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
        if (currentCharacter == null) return;
        RemoveACharacter(currentCharacter);
    }

    public void StartBattle()
    {
        Characters.ForEach(c =>
        {
            if(c != currentCharacter)
            {
                c.AttackTarget(currentCharacter);
            }    
        });
    }

    public bool ValidateEnemyAlive(out int deadCount)
    {
        deadCount = 0;
        foreach (var item in Characters)
        {
            if (item != currentCharacter)
            {
                if (item.IsDead())
                    deadCount++;
                else
                    return true;
            }
        }

        return false;
    }

    public int TotalEnemyDead()
    {
        int count = 0;
        foreach (var character in Characters)
        {
            if (character != currentCharacter && character.IsDead())
            {
                count++;
            }
        }
        return count;
    }

    public void RemoveAllCharacters()
    {
        Characters.ForEach(c => c.DestroyCharacter());
        Characters.Clear();
    }

    private Vector3 GetCameraRelativeDirection(Vector3 inputDirection)
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();
        Vector3 direction = inputDirection.z * forward + inputDirection.x * right;
        return direction.normalized;
    }

}
