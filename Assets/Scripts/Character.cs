using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Infor----------")]
    [SerializeField] float heal = 10f;
    [SerializeField] float moveSpeed = 10f;
    [Header("Componant--------------")]
    [SerializeField]  GameObject IconSelect;
    [SerializeField] Animator Animator;
    [SerializeField] float raycastDistance = 3.0f;
    [SerializeField] LayerMask arPlaneLayerMask;
    [SerializeField] SkillType skillType;
    [SerializeField] Transform tranPosSkill;
    [SerializeField] GameObject skillPrefab;
    [SerializeField] GameObject skillPrefab2;
    [SerializeField] HealthBar healthBar;
    [SerializeField] Material materialDefault;
    [SerializeField] Material materialRed;
    [SerializeField] SkinnedMeshRenderer skinned;
    public float distanceAtk = 1f;

    bool isGrounded;
    bool isChasing;
    bool isAtk;
    bool isDead;
    List<InforSkill> inforSkills = new();
    Character target;

    private void Awake()
    {
        isDead = false;
        isChasing = false;
        skillPrefab.tag = gameObject.CompareTag("CharacterEnemy") ? "SkillEnemy" : "SkillAlly";
        inforSkills.Add(new InforSkill(skillPrefab.GetComponent<Skill>().IDSkill, skillPrefab));
        inforSkills.Add(new InforSkill(skillPrefab2.GetComponent<Skill>().IDSkill, skillPrefab2));
        healthBar.Init(heal);
        healthBar.unbloodyAction = () =>
        {
            if (isDead) return;
            isDead = true;
            Animator.SetTrigger("Dead");
            
            if(IconSelect.activeSelf == true) { 
                GameControl.Ins.UIManager.ShowResult(false, GameControl.Ins.characterController.TotalEnemyDead()); 
            }
            else { 
                GameControl.Ins.CheckEndGame(); 
            }

            healthBar.gameObject.SetActive(false);
        };
    }

    void OnCollisionEnter(Collision collision)
    {
        //if(collision.gameObject.CompareTag("SkillAlly") && gameObject.CompareTag("CharacterEnemy"))
        //{
        //    Damage();
        //}
        if (collision.gameObject.CompareTag("SkillAlly") || gameObject.CompareTag("SkillEnemy"))
            Damage();
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("SkillAlly") && gameObject.CompareTag("CharacterEnemy"))
    //    {
    //        Damage();
    //    }
    //}

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

        if (target != null && !isDead) { UpdateAtkTarget(); }
    }

    private void UpdateAtkTarget()
    {
        Vector3 targetPos = target.transform.position;
        float distance = Vector3.Distance(transform.position, targetPos);
        float distanceATKTagert = distanceAtk;
        if (distance > distanceATKTagert && isChasing)
        {
            MoveCharacter(true);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            transform.LookAt(new Vector3(targetPos.x, transform.position.y, targetPos.z));
        }
        else if (!isAtk)
        {
            isAtk = true;
            DOVirtual.DelayedCall(0.5f, () =>
            {
                isAtk = false;
            });
            Attack();
            transform.LookAt(new Vector3(targetPos.x, transform.position.y, targetPos.z));
            isChasing = Vector3.Distance(transform.position, target.transform.position) > distanceATKTagert;
        }
    }

    public bool IsGrounded() => isGrounded;

    public bool IsDead() => isDead;

    public void ShowSelected()
    {
        IconSelect.SetActive(true);
        healthBar.ChangeMaterial(true);
        gameObject.tag = "CharacterAlly";
        //inforSkill.skillPrefab.tag = "SkillAlly";
    }

    public void HideSelected()
    {
        IconSelect.SetActive(false);
        healthBar.ChangeMaterial(false);
        gameObject.tag = "CharacterEnemy";
        //inforSkill.skillPrefab.tag = "SkillEnemy";
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

    public void AttackTarget(Character _target)
    {
        skinned.material = materialRed;
        target = _target;
        isChasing = true;
    }

    private void SkillLaunch(float deplay = 0)
    {
        InforSkill inforSkill = gameObject.tag == "CharacterAlly" ? inforSkills[1] : inforSkills[0];
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
        return 0f;
    }

    private void Damage()
    {
        Animator.SetTrigger("Damage");
        healthBar.TakeDamage(1);

    }    

}
