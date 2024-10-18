using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(BoundsCheck))]
public class ProjectileHero : MonoBehaviour
{   
    private BoundsCheck bndCheck;
    private Renderer rend;

    [Header("Dynamic")]
    public Rigidbody rigid;
    [SerializeField]
    private eWeaponType _type;

    //This public property masks the private field _type
    public eWeaponType type{
        get{return(_type);}
        set{SetType(value);}
    }

    void Awake(){
        bndCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }

    void Update(){
        if(bndCheck.LocIs(BoundsCheck.eScreenLocs.offUp)){
            Destroy(gameObject);
        }
    }

    //Set the _type private field and colors this projectile to match the WeaponDefintion
    public void SetType(eWeaponType eType){
        _type = eType;
        WeaponDefinition def = Main.GET_WEAPON_DEFINITION(_type);
        rend.material.color = def.projectileColor;
    }

    //Allows Weapon to easily set the velocity of this ProjectileHero
    public Vector3 vel{
        get{return rigid.velocity;}
        set{rigid.velocity = value;}
    }

}
