using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum neighbor
{
	RightTop = 0,
	RightBottom = 1,
	Top = 2,
	Bottom = 3,
	LeftTop = 4,
	LeftBottom = 5
}

public class CreateMap : MonoBehaviour {
    public Transform tileprefeb;
    public float edgeLength = 1;
    // [HideInInspector]
    // public List<Transform> tilelist;
    public Transform tilegroup;
    public Material blockMaterial;
    public Material navMaterial;

    public bool navigation(Transform starttile, List<Transform> tilelist, Transform desttile)
    {
        List<Transform> openlist = new List<Transform>();
        List<Transform> closelist = new List<Transform>();

        foreach (Transform tile in tilelist)
        {
            if (tile.GetComponent<Tile>().getTerrain().Equals(Eterrain.mountain))
            {
                closelist.Add(tile);
            }
        }


        starttile.GetComponent<Tile>().Navigation_G = 0;
        starttile.GetComponent<Tile>().Navigation_F = distance(starttile, desttile);
        // starttile.GetComponent<Tile>().Navigation_father = null;
        // starttile.Log("start f1"+start.F1);

        openlist.Add(starttile);

        while (openlist.Count != 0) {
            Transform current = lowest(openlist);
       
            if (current == desttile) {
                reconstruct_path(current);

                return true;
            }
            openlist.Remove(current);
            closelist.Add(current);
            foreach (Transform neibor in current.GetComponent<Tile>().Neibor) {

                if (naborInCloseSet(neibor, closelist))
                {
                    continue;
                }

                float tentative_gScore = current.GetComponent<Tile>().Navigation_G + terrainCost(neibor);
                if (!naborInOpenSet(neibor, openlist))
                {
                    openlist.Add(neibor);
                }

                // if this is already in the openlist
                else if (tentative_gScore > neibor.GetComponent<Tile>().Navigation_G)
                {
                    continue;
                }
                neibor.GetComponent<Tile>().Navigation_father = current;
                neibor.GetComponent<Tile>().Navigation_G = tentative_gScore;
                neibor.GetComponent<Tile>().Navigation_F = tentative_gScore + distance(neibor, desttile);
            }
        }
        return false;
    }


    public void reconstruct_path(Transform current) {

        while(current.GetComponent<Tile>().Navigation_father!=null){
            current.GetComponent<Tile>().Navigation_father.GetComponent<Renderer>().material = navMaterial;
            current.GetComponent<Tile>().Navigation_father = current.GetComponent<Tile>().Navigation_father.GetComponent<Tile>().Navigation_father;
        }
    }
  

    public int terrainCost(Transform tile)
    {
        if (tile.GetComponent<Tile>().getTerrain().Equals(Eterrain.plane))
        {
            return 1;
        }else if (tile.GetComponent<Tile>().getTerrain().Equals(Eterrain.mountain))
        {
            return 2;
        }
        return -1;
    }


    public bool gScoreIsBigger(float tentative_gScore, List<Transform> openlist)
    {
        bool gScoreIsBigger = false;

        foreach(Transform tile in openlist)
        {
            //if (tile.GetComponent<Tile>().getTerrain().GetHashCode()+1 == 0)
            //{
            //    continue;
            //}
            if (tentative_gScore> tile.GetComponent<Tile>().Navigation_G)
            {
                gScoreIsBigger = true;
            }
        }

        return gScoreIsBigger;
    }


    public bool naborInCloseSet(Transform neibor,List<Transform>closelist)
    {
        bool naborInCloseSet = false;
        foreach(Transform tile in closelist)
        {
            if(neibor == tile)
            {
                naborInCloseSet = true;
            }
        }
        return naborInCloseSet;
    }

    public bool naborInOpenSet(Transform neibor, List<Transform> openlist)
    {
        bool naborInOpenSet = false;
        foreach (Transform tile in openlist)
        {
            if (neibor == tile)
            {
                naborInOpenSet = true;
            }
        }
        return naborInOpenSet;
    }

    //could couse problems
    public Transform lowest( List<Transform> openlist)
    {
        if (openlist.Count ==0 )
        {
            Debug.Log("openlistisempty");
           
        }
        Transform reTransform = null;
       
        float val = openlist[0].GetComponent<Tile>().Navigation_F;
      
        foreach (Transform tile in openlist)
        {
            if (tile.GetComponent<Tile>().Navigation_F <= val)
            {
                val = tile.GetComponent<Tile>().Navigation_F;
                reTransform = tile;
            }
        }
        return reTransform;
    }

    //function reconstruct_path(cameFrom, current)
    //    total_path := [current]
    //    while current in cameFrom.Keys:
    //        current:= cameFrom[current]
    //        total_path.append(current)
    //    return total_path


    public bool isBlocked(Transform tile)
    {
        bool isblk;
        isblk = tile.GetComponent<Tile>().IsBlock;
            return isblk;
    }

    public float distance(Transform start,Transform destination)
    {
        float x = Math.Abs(start.GetComponent<Tile>().getvirtualX()-destination.GetComponent<Tile>().getvirtualX());
        float y = Math.Abs(start.GetComponent<Tile>().getvirtualY() - destination.GetComponent<Tile>().getvirtualY());
        float z = Math.Abs(start.GetComponent<Tile>().getvirtualZ() - destination.GetComponent<Tile>().getvirtualZ());
        return (x+y+z) / 2;
    }


    public void createMap(Transform centertile,List<Transform> tilelist,int range)
    {
       if(centertile.GetComponent<Tile>().IsDrawed==true)
        {
            return;
        }
        if(Mathf.Abs(centertile.GetComponent<Tile>().getvirtualX())== range||
           Mathf.Abs(centertile.GetComponent<Tile>().getvirtualY()) == range ||
           Mathf.Abs(centertile.GetComponent<Tile>().getvirtualZ()) == range)
        {
            
            return;
        }
        drawTile(centertile, neighbor.RightTop, tilelist);
        drawTile(centertile, neighbor.RightBottom, tilelist);
        drawTile(centertile, neighbor.Top, tilelist);

        drawTile(centertile, neighbor.Bottom, tilelist);
        drawTile(centertile, neighbor.LeftTop, tilelist);
        drawTile(centertile, neighbor.LeftBottom, tilelist);

        centertile.GetComponent<Tile>().IsDrawed = true;
       
        for(int i= 0; i < tilelist.Count; i++)
        {
                createMap(tilelist[i], tilelist, range);
        }
      
    }
    public void drawTile(Transform centertile,neighbor neib, List<Transform> tilelist)
    {
        float realX = centertile.position.x;
        float realY = 0;
        float realZ = centertile.position.z;
        float virtualX = 0;
        float virtualY = 0;
        float virtualZ = 0;
        Tile centerTileComponent = centertile.GetComponent<Tile>();
        float centerTileVirtualX = centerTileComponent.getvirtualX();
        float centerTileVirtualY = centerTileComponent.getvirtualY();
        float centerTileVirtualZ = centerTileComponent.getvirtualZ();

        switch (neib)
        {
            case neighbor.RightTop:
                realX = realX - 1.5f*edgeLength;
                realZ = realZ - (Mathf.Sqrt(3) / 2) * edgeLength;
                virtualX = centerTileVirtualX;
                virtualY = centerTileVirtualY + 1;
                virtualZ = centerTileVirtualZ - 1;
                break;
            case neighbor.RightBottom:  
                realX = realX - 1.5f * edgeLength;
                realZ = realZ + (Mathf.Sqrt(3) / 2) * edgeLength;
                virtualX = centerTileVirtualX + 1;
                virtualY = centerTileVirtualY;
                virtualZ = centerTileVirtualZ - 1;
                break;
            case neighbor.Top:  
                realZ = realZ - (Mathf.Sqrt(3)) * edgeLength;
                virtualX = centerTileVirtualX - 1;
                virtualY = centerTileVirtualY + 1;
                virtualZ = centerTileVirtualZ;
                break;
            case neighbor.Bottom:
                realZ = realZ + (Mathf.Sqrt(3)) * edgeLength;
                virtualX = centerTileVirtualX + 1;
                virtualY = centerTileVirtualY - 1;
                virtualZ = centerTileVirtualZ;
                break;
            case neighbor.LeftTop:
                realX = realX + 1.5f * edgeLength;
                realZ = realZ - (Mathf.Sqrt(3)/2) * edgeLength;
                virtualX = centerTileVirtualX - 1;
                virtualY = centerTileVirtualY;
                virtualZ = centerTileVirtualZ + 1;
                break;
            case neighbor.LeftBottom:
                realX = realX + 1.5f * edgeLength;
                realZ = realZ + (Mathf.Sqrt(3) / 2) * edgeLength;
                virtualX = centerTileVirtualX;
                virtualY = centerTileVirtualY - 1;
                virtualZ = centerTileVirtualZ + 1;
                break;
        }           
        if (isInList(virtualX, virtualY, virtualZ, tilelist))
        {
            return;
        }
        Transform newtile = Instantiate(tileprefeb);
        newtile.transform.SetParent(tilegroup);
        Vector3 position = new Vector3();
        position.x = realX;
        position.y = realY;
        position.z = realZ;
        newtile.transform.position = position;
        
        newtile.GetComponent<Tile>().setvirtualX(virtualX);
        newtile.GetComponent<Tile>().setvirtualY(virtualY);
        newtile.GetComponent<Tile>().setvirtualZ(virtualZ);

       

        //temporary test
        if (UnityEngine.Random.Range(0, 100) < 40)
        {
            newtile.GetComponent<Tile>().IsBlock = true ;
            newtile.GetComponent<Tile>().setTerrain(Eterrain.mountain);
            newtile.GetComponent<Renderer>().material = blockMaterial;
        }
        else {
            newtile.GetComponent<Tile>().IsBlock = false;
            newtile.GetComponent<Tile>().setTerrain(Eterrain.plane);
        };
        tilelist.Add(newtile);
      
    }
    public bool isInList(float virtualX,float virtualY,float virtualZ,List<Transform> tilelist )
    {
        bool isInList = false;
        if (tilelist.Count == 0)
        {
            Debug.Log("nothing in tilelist");
        }
 
        foreach (Transform tile in tilelist)
        {
           if (tile.GetComponent<Tile>().getvirtualX() == virtualX&&
                tile.GetComponent<Tile>().getvirtualY() == virtualY&&
                tile.GetComponent<Tile>().getvirtualZ() == virtualZ
                )
            {
                isInList = true;
            }     
        }
        return isInList;
    }
    public void findNeibor(List<Transform> tilelist)
    {
        foreach (Transform tile in tilelist)
        {
            foreach (Transform neibor in tilelist)
            {
               
                float tileVirtualX = tile.GetComponent<Tile>().getvirtualX();
                float tileVirtualY = tile.GetComponent<Tile>().getvirtualY();
                float tileVirtualZ = tile.GetComponent<Tile>().getvirtualZ();

                float startVirtualX = neibor.GetComponent<Tile>().getvirtualX();
                float startVirtualY = neibor.GetComponent<Tile>().getvirtualY();
                float startVirtualZ = neibor.GetComponent<Tile>().getvirtualZ();

                if (            (tileVirtualX == startVirtualX &&
                                tileVirtualY == startVirtualY + 1 &&
                                tileVirtualZ == startVirtualZ - 1) ||

                                (tileVirtualX == startVirtualX + 1 &&
                                tileVirtualY == startVirtualY &&
                                tileVirtualZ == startVirtualZ - 1) ||

                                (tileVirtualX == startVirtualX - 1 &&
                                tileVirtualY == startVirtualY + 1 &&
                                tileVirtualZ == startVirtualZ) ||

                                (tileVirtualX == startVirtualX + 1 &&
                                tileVirtualY == startVirtualY - 1 &&
                                tileVirtualZ == startVirtualZ) ||

                                (tileVirtualX == startVirtualX - 1 &&
                                tileVirtualY == startVirtualY &&
                                tileVirtualZ == startVirtualZ + 1) ||

                                (tileVirtualX == startVirtualX &&
                                tileVirtualY == startVirtualY - 1 &&
                                tileVirtualZ == startVirtualZ + 1)
                                ) {
                        
                            tile.GetComponent<Tile>().Neibor.Add(neibor);
                     }
             }
        }
    }

    void Start () {
        List<Transform> tilelist = new List<Transform>();

        Transform centertile = Instantiate(tileprefeb);
        centertile.GetComponent<Tile>().transform.SetParent(tilegroup);
        centertile.GetComponent<Tile>().setvirtualX(0);
        centertile.GetComponent<Tile>().setvirtualY(0);
        centertile.GetComponent<Tile>().setvirtualZ(0);
        Vector3 position = new Vector3();
        position.x = 0;
        position.y = 0;
        position.z = 0;
        centertile.transform.position = position;

        Eterrain centerterrain = Eterrain.mountain;
        centertile.GetComponent<Tile>().setTerrain(centerterrain);
        tilelist.Add(centertile);
        centertile.GetComponent<Tile>().IsDrawed = false;
        centertile.GetComponent<Tile>().IsBlock = false;
        //DateTime.Now
        int time = DateTime.Now.Second;
        createMap(centertile, tilelist,30);
        Debug.Log(" creatmap"+ (DateTime.Now.Second - time));
        time = DateTime.Now.Second;
        findNeibor(tilelist);
        Debug.Log(" findneibor"+ (DateTime.Now.Second - time));
        navigation(centertile, tilelist, tilelist[232]);
        time = DateTime.Now.Second;
        Debug.Log(" navigation"+ (DateTime.Now.Second - time));
    }
	
	
}
