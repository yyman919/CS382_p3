using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Inscribed")]
    public float rotationPerSecond = 0.1f;

    [Header("Dynamic")]
    public int levelShown = 0;

    //None public varaible variable that will not appear in the Inspector
    Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;//Gets the material from Unity
    }

    void Update()
    {
        //Read current shield level from Hero Singleton
        int currLevel = Mathf.FloorToInt(Hero.S.shieldLevel);
        if(levelShown != currLevel){ //If different from levelShown
            levelShown = currLevel;
            //Adjust the texture of the shiled to show diff level
            mat.mainTextureOffset = new Vector2(0.2f*levelShown, 0);
        }

        //Rotate shield a little bit every fram in a time based way
        float rZ = -(rotationPerSecond*Time.time*360) % 360f;
        transform.rotation = Quaternion.Euler(0,0,rZ);
    }
}
