using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    //purpose of the graph to translate the ones and zeros into a array of nodes

    public Node[,] nodes; // While we have a node class in mapdata by creating a secondary one we get more flexibility in how we handle our data
    public List<Node> walls = new List<Node>(); //walls created in the map

    int[,] _mapData; //cache our map data
    int _width; //Representing our width ahd height of our map data
    int _heigth;
    public MapData map;
    public int Width { get { return _width; } }
    public int Height { get { return _heigth; } }

    //readonly as stated is only read not changed and we declare it as a static as not have multiple versions of this variable
    //
    public static readonly Vector2[] allDirections =
        {
        //we cover all possible movement posstions we can take 
           new Vector2(0f,1f),
           new Vector2(1f,1f),
           new Vector2(1f,0f),
           new Vector2(-1f,0),
           new Vector2(0,-1),
           new Vector2(-1,-1),
           new Vector2(1,-1),
           new Vector2(-1,1)


        };

   
    public void Init(int[,] mapData)
    {
        
        _mapData = mapData;
        
        if(!map.NotCreating)
        {
            _width = mapData.GetLength(0); //defining what the width and height represnt on the map data/ so width represents the first part of the array and height the second
            _heigth = mapData.GetLength(1);
            //fillling our node walls with the y and x scale of our map, this gets filled in with our x and y looping values 
            nodes = new Node[_width, _heigth];

            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _heigth; y++)
                {
                    //get map data from each element and convert it to a node type
                    //so we get the nodetype function from node which allows us to define the open and closed areas in the graph
                    NodeType type = (NodeType)mapData[x, y];
                    //creating a new node taking the classes contstructor
                    //So the Constructor defined in Node we can now fill with information
                    Node newNode = new Node(x, y, type);
                    nodes[x, y] = newNode;

                    newNode.position = new Vector3(x, 0, y);//placing the node in a 3d position that corresponds to it's x,y postion in the array. Due to it being a representation in 3d space the z represernts the foward postion of the grid which is where we put the y

                    //we the node we look at is closed then we add it to the closed part of the array
                    if (type == NodeType.ClosedSet)
                    {
                        walls.Add(newNode);
                    }
                }

            }
            //we go through our map and get our neigbours as long as its not a closed node.
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _heigth; y++)
                {
                    if (nodes[x, y].nodeType != NodeType.ClosedSet)
                    {
                        nodes[x, y].neighbours = GetNeighbours(x, y);
                    }
                }
            }
        }
       

    }

    public bool IsWithInBounds(int x, int y)
    {
        return (x >= 0 && x < _width && y >= 0 && y < _heigth); //we check that we dont leave the bounds of our movement map , making sure we dont go below zero position and below our height and width
    }

    List<Node> GetNeighbours(int x, int y, Node[,] nodeArray, Vector2[] directions)//this is where we get our neighbours, we get the size of the map, then we look through the node and see which directions we can take
    {
        List<Node> neighbourNodes = new List<Node>();

        foreach (Vector2 dir in directions)// looping through the directions we can take
        {
            int newX = x + (int)dir.x; // We convert our float direction to a int, we get the array of the x size of the map and we increase our movement by the direction amount on the x so we are at [0,0] if the dir on x is 1 we have [1,0]
            int newY = y + (int)dir.y;

            if (IsWithInBounds(newX, newY) && nodeArray[newX, newY] != null && nodeArray[newX, newY].nodeType != NodeType.ClosedSet) // we check if the new movement coordinates are within our delcared bounds,that we have a valid num  and that we arent going into a wall
            {
                neighbourNodes.Add(nodeArray[newX, newY]); // if all is correct we add to the neighbour nodes array with our new coordinates
            }

            


        }

        return neighbourNodes; //remember return functions come at the end of the brackets as the data should be complete and not still changing.
    }
    List<Node> GetNeighbours(int x, int y)  //Overloading get Neigbours //Method overloading is having different implementations of the same function which share the same name for example Console. has 19 different implemntations
    {
        return GetNeighbours(x, y, nodes, allDirections);
    }

    public float GetNodeDistance(Node source, Node target)
    {
        int dx = Mathf.Abs(source.xIndex - target.xIndex);
        int dy = Mathf.Abs(source.yIndex - target.yIndex);

        int min = Mathf.Min(dx, dy);
        int max = Mathf.Max(dx, dy);

        int diagonalSteps = min;
        int straightSteps = max - min;

        return (1.4f * diagonalSteps + straightSteps);
    }
}
