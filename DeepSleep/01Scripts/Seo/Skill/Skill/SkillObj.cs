using System;
using UnityEngine;

public class SkillObj : MonoBehaviour
{
    public event Action OnSkillDestroyEvent;

    public void CallDestroyEvent()
    {
        Debug.Log("CallDestroyEvent");
        OnSkillDestroyEvent?.Invoke();
    }
}
