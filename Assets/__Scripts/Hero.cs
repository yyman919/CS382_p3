using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero S {get; private set;} //Singleton property; automatic property

    [Header("Inscribed")]
    //Control movement of the ship
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public GameObject projectilePrefab;
    public float projectileSpeed = 40;
    public Weapon[] weaponlist;
    public WeaponFireDelegate[] weapons;

    [Header("Dynamic")][Range(0,4)]     //Adds a slider of 0-4 in the Inspector
    private float  _shieldLevel = 1;    // Remember the underscore
    [Tooltip( "This field holds a reference to the last triggering GameObject")]
    private GameObject lastTriggerGo = null;

    //Declare a new delegate type WeaponFireDelegate
    public delegate void WeaponFireDelegate();
    //Create a WeaponFireDelegate event named fireEvent
    public event  WeaponFireDelegate fireEvent;


    void Awake(){
        if(S == null) {
            S = this;//Set singleton is null
        }
        else {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");
        }
        // fireEvent += TempFire;

        // Reset the weapons to start _Hero with 1 blaster
        ClearWeapons();
        weaponlist[0].SetType(eWeaponType.blaster);
    }


    void Update()
    {
        //Grabs user input
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        //Change transform.pos base don the axes
        Vector3 pos = transform.position;
        pos.x += hAxis * speed * Time.deltaTime;
        pos.y += vAxis * speed * Time.deltaTime;
        transform.position = pos;

        //Rotate the ship to make it feel more dynamic (a bit of rotation based on speed)
        transform.rotation = Quaternion.Euler(vAxis*pitchMult, hAxis*rollMult, 0);

        //Use the fireevent to fire Weapons when the spacebar is pressed
        if(Input.GetAxis("Jump") == 1 && fireEvent != null){
            fireEvent();
        }
    }

    //Comment out TempFire() function
    /*
    void TempFire(){
        GameObject projGO = Instantiate<GameObject>(projectilePrefab);
        projGO.transform.position = transform.position;
        Rigidbody rigidB = projGO.GetComponent<Rigidbody>();
        //rigidB.velocity = Vector3.up * projectileSpeed;

        ProjectileHero proj = projGO.GetComponent<ProjectileHero>();
        proj.type = eWeaponType.blaster;
        float tSpeed = Main.GET_WEAPON_DEFINITION(proj.type).velocity;
        rigidB.velocity = Vector3.up * tSpeed;
    }
    */


    void OnTriggerEnter(Collider other)
    {
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        // Debug.Log("Shield trigger hit by: " +go.gameObject.name);
        
        // Make sure it's not the same triggering go as last time
        if ( go == lastTriggerGo ) return;
        lastTriggerGo = go;

        Enemy enemy = go.GetComponent<Enemy>();
        PowerUp pUp = go.GetComponent<PowerUp>();
        if (enemy != null)      // If the shield was triggered by an enemy
        {
            shieldLevel--;      // Decrease the level of the shield by 1
            Destroy(go);        // ... and Destroy the enemy
        }else if (pUp != null)  // If the shield hit a PowerUp
        {
            AbsorbPowerUp(pUp); // ... absorb the PowerUp
        }
        else
        {
            Debug.Log("Shield trigger hit by non-Enemy: "+go.name);
        }
    }

    public void AbsorbPowerUp( PowerUp pUp ) {
        Debug.Log( "Absorbed PowerUp: " + pUp.type );
        switch (pUp.type) {
            case eWeaponType.shield:
                shieldLevel++;
                break;

            default:
                if (pUp.type == weaponlist[0].type) {
                    Weapon weap = GetEmptyWeaponSlot();
                    if (weap != null) {
                        // Set it to pUp.type
                        weap.SetType(pUp.type);
                    }
                } else {
                    ClearWeapons();
                    weaponlist[0].SetType(pUp.type);
                }
                break;
        }
        pUp.AbsorbedBy( this.gameObject );
    }

    public float shieldLevel
    {
        get { return (_shieldLevel ); }
        private set
        {
            _shieldLevel = Mathf.Min( value, 4 );
            // If the shield is going to be set to less than zero...
            if (value < 0)
            {
                Destroy(this.gameObject);   // Destroy the hero
                Main.HERO_DIED();//Link to the Main.cs file for hero death
            }
        }
    }

    /// <summary>
    /// Finds the first empty Weapon slot (i.e., type=none) and returns it.
    /// </summary>
    /// <returns>The first empty Weapon slot or null if none are empty</returns>
    Weapon GetEmptyWeaponSlot() {
        for (int i=0; i <weaponlist.Length; i++){
            if (weaponlist[i].type == eWeaponType.none ) {
                return( weaponlist[i] );
            }
        }
        return ( null );
    }

    /// <summary>
    /// Sets the type of all Weapon slots to none
    /// </summary>
    void ClearWeapons() {
        foreach( Weapon w in weaponlist) {
            w.SetType(eWeaponType.none);
        }
    }
}
