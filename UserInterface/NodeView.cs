using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeView : MonoBehaviour
{
    public GameObject tile;
    public GameObject Wall;
    public Node _node;
    public GameObject arrow;
    public int x;
    public int y;

     //scaling down the flat plane, triming edges and limit the size

    [Range(0,0.5f)]
    public float borderSize = 0.15f;

    public void Init(Node node)
    {
        
        //initilise node view object
        //if we have a tile we want to give the tile the name of the index postion it represents
        //then that new tile will also be at the index postions we decalred
        if(tile!=null)
        {
            Wall.SetActive(false);
            gameObject.name = "Node(" + node.xIndex + "," + node.yIndex + ")";
            x = node.xIndex;
            y = node.yIndex;
            gameObject.transform.position = node.position;
            tile.transform.localScale = new Vector3(1f - borderSize, 1f, 1f - borderSize);
            _node = node;
            EnableObject(arrow, false);
        }
        if(node.nodeType == NodeType.ClosedSet )
        {
            Wall.SetActive(true);
        }
       
    }

    void ColorNode(Color color, GameObject go) // of our gameobject is not null we get the renderer and then if the renderer is not null we assign a color
    {
        if (go!=null){
            Renderer goRender = go.GetComponent<Renderer>();

            if(goRender!=null)
            {
                goRender.material.color = color;

            }
        }
    }

    public void ColorNode(Color color)//overloaded method were we give the color and the tile
    {
        ColorNode(color, tile);
    }

    void EnableObject(GameObject go, bool state)
    {
        if(go!=null)
        {
            go.SetActive(state);
        }

    }
    //Checking if we have a previous node and then changing our rotation to point to that node, we also use enable object which activates the object 
    public void ShowArrow(Color color)
    {
        if(_node!=null && arrow !=null && _node.previous!=null)
        {
            EnableObject(arrow, true);

            Vector3 dirToPrevious = (_node.previous.position - _node.position).normalized;
            arrow.transform.rotation = Quaternion.LookRotation(dirToPrevious);
            Renderer arrowRender = arrow.GetComponent<Renderer>();
            if(arrowRender!=null)
            {
                arrowRender.material.color = color;
            }
        }
    }
}
