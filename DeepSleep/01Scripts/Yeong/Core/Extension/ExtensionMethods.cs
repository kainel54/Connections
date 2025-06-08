using System.Linq;
using UnityEngine;

public static class ExtensionMethods
{
    public static float GetAnimationDuration(this string text, float speed)
    {
        float totalDuration = 0f;
        float characterDelay = 1f / speed;

        for (int i = 0; i < text.Length; i++)
        {
            if (text[i] == '{')
            {
                int closeIndex = text.IndexOf('}', i);
                if (closeIndex != -1)
                {
                    string delayStr = text.Substring(i + 1, closeIndex - i - 1);
                    if (float.TryParse(delayStr, out float additionalDelay))
                    {
                        totalDuration += additionalDelay;
                    }

                    i = closeIndex;
                    continue;
                }
            }

            totalDuration += characterDelay;
        }
        return totalDuration;
    }
}
