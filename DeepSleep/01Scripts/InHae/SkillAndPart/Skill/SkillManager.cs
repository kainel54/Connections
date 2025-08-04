using System;
using System.Collections.Generic;

public class SkillManager : MonoSingleton<SkillManager>
{
    private Dictionary<Type, Skill> _skills;
    private Dictionary<string, Type> _skillTypes = new ();

    private void Start()
    {
        _skills = new Dictionary<Type, Skill>();
        _skillTypes = new Dictionary<string, Type>();
        
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
    
    public Skill GetSkill(string skillTypeName)
    {
        if (!_skillTypes.ContainsKey(skillTypeName))
            _skillTypes[skillTypeName] = Type.GetType(skillTypeName);
        
        Type t = _skillTypes[skillTypeName];
        return _skills.GetValueOrDefault(t);
    }}
