using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum Eterrain
    {
        plane = 0,
        mountain = 1
    }

public class Tile : MonoBehaviour {

    private float virtualX;
    private float virtualY;
    private float virtualZ;
    private Eterrain terrain;
    private bool isDrawed;
    
    //temporary value for test
    private bool isBlock;

    private float navigation_F;
    private float navigation_G;
    private Transform navigation_father;

    private List<Transform> neibor = new List<Transform>();

    public bool IsDrawed
    {
        get
        {
            return isDrawed;
        }

        set
        {
            isDrawed = value;
        }
    }

    public bool IsBlock
    {
        get
        {
            return isBlock;
        }

        set
        {
            isBlock = value;
        }
    }

    public List<Transform> Neibor
    {
        get
        {
            return neibor;
        }

        set
        {
            neibor = value;
        }
    }

    public float Navigation_F
    {
        get
        {
            return navigation_F;
        }

        set
        {
            navigation_F = value;
        }
    }



    //public Transform Navigation_father
    //{
    //    get
    //    {
    //        return navigation_father;
    //    }

    //    set
    //    {
    //        navigation_father = value;
    //    }
    //}

    public float Navigation_G
    {
        get
        {
            return navigation_G;
        }

        set
        {
            navigation_G = value;
        }
    }

    public Transform Navigation_father
    {
        get
        {
            return navigation_father;
        }

        set
        {
            navigation_father = value;
        }
    }

    public float getvirtualX()
    {
        return virtualX;
    }
    public void setvirtualX(float xVal)
    {
        virtualX = xVal;
    }
    public float getvirtualY()
    {
        return virtualY;
    }
    public void setvirtualY(float yVal)
    {
        virtualY = yVal;
    }
    public float getvirtualZ()
    {
        return virtualZ;
    }
    public void setvirtualZ(float zVal)
    {
        virtualZ = zVal;
    }
    //[HideInInspector] private Transform self;
    public Eterrain getTerrain()
    {
        return terrain;
    }
    public void setTerrain(Eterrain terrainVal)
    {
        terrain = terrainVal;
    }

    
}
