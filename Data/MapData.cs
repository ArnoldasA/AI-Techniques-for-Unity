using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//using System.Linq;
public class MapData : MonoBehaviour
{
    //storing our map as a two dimensional array of 1 and 0s // look at word to see live look
    // Start is called before the first frame update

    //declaring the width and height of the map we want to create
    public int width = 10;
    public int height = 5;
   
    int[,] map;
    public bool NotCreating;


    private void Update()
    {
        GetNodeInfo();
    }

    public void GetNodeInfo()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                //int xx, yy;
                
                if(hit.collider.gameObject.GetComponentInParent<NodeView>())
                {
                    NodeView node = hit.collider.gameObject.GetComponentInParent<NodeView>();

                    Debug.Log(node.x +"," +node.y);
                    
                    map[node.x, node.y] = 1;
                }


            }
        }
    }

    public int[,] MakeMap()
    {
        
        map = new int[width, height];
        GetNodeInfo();



        //currently HardCoded walls
        map[1, 0] = 1;
        map[1, 2] = 1;
        map[2, 0] = 1;
        map[3, 4] = 1;
        map[10, 5] = 1;
        map[3, 2] = 1;
        map[3, 5] = 1;
        map[6, 0] = 1;
        map[6, 2] = 1;
        map[5, 3] = 1;
        map[5, 4] = 1;
        map[15, 4] = 1;
        map[13, 1] = 1;
        map[13, 4] = 1;


        return map;

    }
    

  
}
