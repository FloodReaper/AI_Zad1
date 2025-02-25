using System;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class AutoPilot : MonoBehaviour
{
    public GameObject enemy;

    public float rotateSpeed = 2f;
    public float speed = 1f;
    private bool isActive = false;

    private void LateUpdate()
    {
        LookAtEnemy();
        Active();
    }

    private void Active()
    {
        if (CalcDistance() > 0.2)
            isActive = true;
        else
            isActive = false;

        if (isActive)
            transform.position += transform.forward * speed * Time.deltaTime; 
    }

    private double CalcDistance()
    {
        return (enemy.transform.position - transform.position).magnitude;
    }

    private void LookAtEnemy()
    {
        var cross = CalcCross();
        var angle = CalcAngle();

        if (angle < 10)
            return;

        if (cross.y > 0.0001)
            angle *= -1;

        transform.Rotate(new Vector3(0, angle, 0) * rotateSpeed * Time.deltaTime);
    }

    private Vector3 CalcCross()
    {
        Vector3 enemyDirection = enemy.transform.position - transform.position;
        Vector3 myForward = transform.forward;

        var cross = new Vector3((enemyDirection.y * myForward.z) - (enemyDirection.z * myForward.z),
                                (enemyDirection.z * myForward.x) - (enemyDirection.x * myForward.z),
                                (enemyDirection.x * myForward.y) - (enemyDirection.y * myForward.x));

        return cross;
    }

    private float CalcAngle()
    {
        Vector3 enemyDirection = enemy.transform.position - transform.position;
        Vector3 myForward = transform.forward;

        var dotX = enemyDirection.x * myForward.x;
        var dotY = enemyDirection.z * myForward.z;
        var dot = dotX + dotY;

        //Debug.Log($"Dot Value: {dot}");

        var denominator = Math.Sqrt(enemyDirection.sqrMagnitude * myForward.sqrMagnitude);
        var toAcrosValue = dot / denominator;

        var angle = Math.Acos(toAcrosValue);

        //Debug.Log($"Angle value (deg): {angle * Mathf.Rad2Deg}");

        return float.Parse((angle * Mathf.Rad2Deg).ToString());
    }

    
}