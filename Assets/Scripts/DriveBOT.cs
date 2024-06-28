using System.Collections;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class DriveBOT : MonoBehaviour
{
    public GameObject enemy;
    public GameObject tank;
    public float rotationSpd = 2;
    public float range;
    public float speed = 5;
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward*10, Color.red, Time.deltaTime);
        AutoPilot();
    }

    void AutoPilot()
    {  
        FaceEnemy();
        DriveToEnemy();
    }
    void FaceEnemy()
    {
        var crossProduct = CalculateCross();
        var dotProduct = CalculateAngle();

        if (crossProduct.y > 0)
            dotProduct *= -1;

        transform.Rotate(new Vector3(0, dotProduct, 0) * rotationSpd * Time.deltaTime);
    }

    void DriveToEnemy()
    {
        if (CalculateDistance() > 10)
        {
            this.transform.position += transform.forward * speed * Time.deltaTime;
        }       
        else
        {
            this.transform.position -= transform.forward * speed * Time.deltaTime;
        }
    }
 
    public double CalculateDistance()
    {
       return (enemy.transform.position - transform.position).magnitude;
    }

    private float CalculateAngle()
    {
        Vector3 toEnemy = enemy.transform.position - transform.position;
        Vector3 tankForward = transform.forward;

        //Części składowe zdania
        float dotX = tankForward.x * toEnemy.x;
        float dotZ = tankForward.z * toEnemy.z;
        //---
        var nominator = dotX + dotZ;
        var denominator = Math.Sqrt(toEnemy.sqrMagnitude * tankForward.sqrMagnitude);
        var  fraction = nominator / denominator;
        //Wyliczeniewyniku działania
        var dotProduct = Math.Acos(fraction);

        return float.Parse((dotProduct * Mathf.Rad2Deg).ToString()); 
    }

    private Vector3 CalculateCross()
    {
        Vector3 toEnemy = enemy.transform.position - transform.position;
        Vector3 tankForward = transform.forward;

        var crossProduct = new Vector3
        (
            (toEnemy.y * tankForward.z) - (toEnemy.z * tankForward.z),
            (toEnemy.z * tankForward.x) - (toEnemy.x * tankForward.z),
            (toEnemy.x * tankForward.y) - (toEnemy.y * tankForward.x)
        );

        return crossProduct;
    }
}
