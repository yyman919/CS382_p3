using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheck : MonoBehaviour
{
    [System.Flags]
    public enum eScreenLocs{
        onScreen = 0,   // 0000 in binary (zero)
        offRight = 1,   // 0001 in binary
        offLeft = 2,    // 0010 in binary
        offUp = 4,      // 0100 in binary
        offDown = 8     // 1000 in binary
    }

    public enum eType {center, inset, outset};//Allows us to control whether the BoundsCheck is centered,inset, or outset

    [Header("Inscribed")]
    public eType boundsType = eType.center;//is set to 0
    public float radius = 1f;
    public bool keepOnScreen = true;

    [Header("Dynamic")]
    public eScreenLocs screenLocs = eScreenLocs.onScreen;
    public float camWidth;
    public float camHeight;

    void Awake(){
        camHeight = Camera.main.orthographicSize;//Camera.main gives access to first camera; ortho gives you the size number from Camera inspector
        camWidth = camHeight * Camera.main.aspect;//aspect ratio (gets the distance from the orgion to the left or right edge of the screen)
    }

    void LateUpdate(){ //Called every fram after update

        float checkRadius = 0;
        if(boundsType == eType.inset) checkRadius = -radius; //set -radius shrinking the size of the screen bounds by radius
        if(boundsType == eType.outset) checkRadius = radius; //set positive radius expanding the size of the screen bounds

        Vector3 pos = transform.position;
        screenLocs = eScreenLocs.onScreen;

        //Restrict x pos to camWidth
        if(pos.x > camWidth + checkRadius){
            pos.x = camWidth + checkRadius;
            screenLocs |= eScreenLocs.offRight;
        }

        if(pos.x < -camWidth - checkRadius){
            pos.x = -camWidth - checkRadius;
            screenLocs |= eScreenLocs.offLeft;
        }

        //Restrict y pos to camHeight
        if(pos.y > camHeight + checkRadius){
            pos.y = camHeight + checkRadius;
            screenLocs |= eScreenLocs.offUp;
        }

        if(pos.y < -camHeight - checkRadius){
            pos.y = -camHeight - checkRadius;
            screenLocs |= eScreenLocs.offDown;
        }

        if(keepOnScreen && !isOnScreen){
            transform.position = pos;
            screenLocs = eScreenLocs.onScreen;
        }
    }

    public bool isOnScreen {
        get {return (screenLocs == eScreenLocs.onScreen);}
    }

    public bool LocIs(eScreenLocs checkLoc){
        if(checkLoc == eScreenLocs.onScreen) return isOnScreen;
        return ( (screenLocs & checkLoc) == checkLoc);
    }
}
