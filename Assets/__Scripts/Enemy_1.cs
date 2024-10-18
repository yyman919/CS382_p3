using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    [Header("Enemy_1 Inscribed Fields")]
    [Tooltip("# of seconds for a full sine wave")]
    public float waveFrequency = 2;
    [Tooltip("Sine wave width in meters")]
    public float waveWidth = 4;
    [Tooltip("Amount the ship will roll left and right with the sine wave")]
    public float waveRotY = 45;

    private float x0; //intial val of pos
    private float birthTime;



    void Start()
    {
        x0 = pos.x;
        birthTime = Time.time;  
    }

    public override void Move(){
        //bc pos is a property you cant directly set pos.x
        Vector3 tempPos = pos;
        float age = Time.time - birthTime;
        float theta = Mathf.PI*2*age / waveFrequency;
        float sin = Mathf.Sin(theta);
        tempPos.x = x0 + waveWidth * sin;
        pos = tempPos;

        //Rotate a bit about y
        Vector3 rot = new Vector3(0, sin*waveRotY, 0);
        this.transform.rotation = Quaternion.Euler(rot);

        //handles movement down in y
        base.Move();

        //print(bndCheck.isOnScreen);


    }


}
