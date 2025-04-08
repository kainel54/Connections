using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoSingleton<SkillManager>
{
    private Dictionary<Type, Skill> _skills;

    private void Start()
    {
        _skills = new Dictionary<Type, Skill>(); 

        foreach (var skill in GetComponentsInChildren<Skill>())
        {
            Type type = skill.GetType();
            _skills.Add(type, skill);
        }
    }

    public T GetSkill<T>() where T : Skill 
    {                                      
        Type t = typeof(T);
        if (_skills.TryGetValue(t, out Skill value))
        {
            return value as T;
        }
        return null;
    }
    
    public Skill GetSkill(Type t)
    {
        return _skills.GetValueOrDefault(t);
    }
}
