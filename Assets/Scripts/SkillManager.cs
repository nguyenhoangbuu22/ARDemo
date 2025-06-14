using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public enum SkillType
{
    Skill1,
    Skill2,
    Skill3,
    Skill4
}

public class SkillManager : MonoBehaviour
{
    private readonly List<Skill> skills = new();

    public Skill GetSkill(InforSkill inforSkill, Vector3 posSkill)
    {
        Skill skill = skills.Find(s => s.IDSkill == inforSkill.IDSkill && !s.used );
        if(skill == null)
        {
            GameObject skillObj = Instantiate(inforSkill.skillPrefab, posSkill, Quaternion.identity, transform);
            skill = skillObj.GetComponent<Skill>();
            skills.Add(skill);
        }
        skill.used = true;
        return skill;
    }    
}
