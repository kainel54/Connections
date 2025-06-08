using UnityEngine;

public class SkillTargetingObj : SkillObj
{
    public void DestroyObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    public void DestroyObject(GameObject gameObject, float delay)
    {
        Destroy(gameObject, delay);
    }
}