using DG.Tweening;
using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] GameObject IconSelect;
    [SerializeField] Animator Animator;

    [SerializeField] float raycastDistance = 3.0f;
    [SerializeField] LayerMask arPlaneLayerMask;
    [SerializeField] SkillType skillType;
    [SerializeField] Transform tranPosSkill;
    [SerializeField] GameObject skillPrefab;
    public float distanceAtk = 1f;

    bool isGrounded;
    InforSkill inforSkill;

    private void Start()
    {
        inforSkill = new InforSkill(skillPrefab.GetComponent<Skill>().IDSkill, skillPrefab);
    }

    void Update()
    {
        Vector3 origin = transform.position + Vector3.up * 0.2f;
        isGrounded = Physics.Raycast(origin, Vector3.down, raycastDistance, arPlaneLayerMask);

        if (!GameControl.Ins.characterController.jumping)
        {
            Ray ray = new Ray(origin, Vector3.down);
            Debug.DrawRay(origin, Vector3.down * raycastDistance, Color.green);
            if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, arPlaneLayerMask))
            {
                Vector3 pos = transform.position;
                pos.y = hit.point.y;
                transform.DOMove(pos, 0.7f);
            }
        }
    }

    public bool IsGrounded() => isGrounded;

    public void ShowSelected()
    {
        IconSelect.SetActive(true);
    }

    public void HideSelected()
    {
        IconSelect.SetActive(false);
    }

    public void DestroyCharacter()
    {
        Destroy(gameObject);
    }    

    public void MoveCharacter(bool isMove)
    {
        Animator.SetBool("Moving", isMove);
    }

    public virtual void Attack()
    {
        if (!Animator.GetBool("Atk"))
        {
            Animator.SetBool("Atk", true);

            float duration = GetClipDuration("MouseAtk");
            SkillLaunch(duration - (duration * 0.6f));
            DOVirtual.DelayedCall(duration, () =>
            {
                Animator.SetBool("Atk", false);
            });
        }
        else
        {
            SkillLaunch();
        }    
    }

    private void SkillLaunch(float deplay = 0)
    {
        DOVirtual.DelayedCall(deplay, () =>
        {
            Skill skill = GameControl.Ins.SkillManager.GetSkill(inforSkill, tranPosSkill.position);
            Vector3 direction = transform.forward.normalized;
            Vector3 targetPos = tranPosSkill.position + direction * distanceAtk;
            skill.MoveAttack(tranPosSkill.position, targetPos);
        });

    }    

    private float GetClipDuration(string clipName)
    {
        foreach (var clip in Animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
                return clip.length;
        }
        Debug.LogWarning("Không tìm thấy clip: " + clipName);
        return 0.5f; // fallback time
    }
}
