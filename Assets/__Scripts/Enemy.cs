using System.Collections;           // Required for some Arrays manipulation
using System.Collections.Generic;   // Required for Lists and Dictionaries
using UnityEngine;                  // Reuqired for Unity

[RequireComponent(typeof(BoundsCheck) )]
public class Enemy : MonoBehaviour
{
    [Header("Inscribed")]
    public float speed      = 10f;      // The movement speed is 10 m/s
    public float firerate   = 0.3f;      // Seconds/shot (Unused)
    public float health     = 10;       // Damage needed to destroy this enemy
    public int score        = 100;      // Points earned for destroying this
    public float powerUpDropChance = 1f; // Chance to drop a PowerUp


    protected bool calledShipDestroyed = false;
    protected BoundsCheck bndCheck;//Changed from private to public to be seen by subclasses

    void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }

    // This is a Property: A method that acts like a field
    public Vector3 pos
    {
        get
        {
            return this.transform.position;
        }
        set
        {
            this.transform.position = value;
        }
    }

    void Update()
    {
        Move();

        // Check whether this Enemy has gone off the bottom of the screen
        if (bndCheck.LocIs(BoundsCheck.eScreenLocs.offDown))
        {
            Destroy(gameObject);
        }
    }

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    void OnCollisionEnter(Collision coll){
        GameObject otherGo = coll.gameObject;

        // Check for collisions with ProjectileHero
        ProjectileHero p = otherGo.GetComponent<ProjectileHero>();
        if ( p != null ) {
            // Only damage this Enemy if it's on screen
            if ( bndCheck.isOnScreen ) {
                // Get the damage amount from the Main WEAP_DICT.
                health -= Main.GET_WEAPON_DEFINITION( p.type ).damageOnHit;
                if ( health <= 0 ) {
                    // Tell Main that this ship was destroyed
                    if (!calledShipDestroyed){
                        calledShipDestroyed = true;
                        Main.SHIP_DESTROYED( this );
                    }
                    // Destroy this Enemy
                    Destroy( this.gameObject );
                }
            }
            // Destroy the ProjectileHero regardless
            Destroy( otherGo );
        } else {
            print( "Enemy hit by non-ProjectileHero: " + otherGo.name );
        }
    }
}
