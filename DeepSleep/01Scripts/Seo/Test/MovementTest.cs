using UnityEngine;

public class MovementTest : MonoBehaviour
{

    public float Radius;
    public float angle;

    public float timer;

    public float zigzagInterval = 1;
    public float _zigzagDirection = 1;

    public bool isOneTime = true;


    public Transform trm;
    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        angle += Time.fixedDeltaTime * 10;
        Vector3 zigzagDir = (int)timer % 2 == 0 ? new Vector3(-5, 0, 5) : new Vector3(5, 0, 5);
        Vector3 dir = zigzagDir + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * Radius;

        transform.position += dir * Time.fixedDeltaTime;

        trm.position += zigzagDir * Time.fixedDeltaTime;
    }
}
