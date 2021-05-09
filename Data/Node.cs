using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Enums are unchangeable functions that others read information from
public enum NodeType
{
    OpenSet=0,
    ClosedSet=1
}

public class Node : IComparable <Node>   //here we are implemeting a interface of I comparable, in this case we must use the interfaces memebers for it to work. Also we will use this to compare nodes
{
    //Node is a container of data
    //with some limited funconality 
    //by also not inheriting from mono we cut a lot of overhead cost associated with the library
    //but it also means we are not able to use its functions
    public NodeType nodeType = NodeType.OpenSet; //here we declare a nodetype which can inherit our enum values
    public float distanceTravelled = Mathf.Infinity;
    public float prority; // we will compare the prority of two nodes to decide on the best one // we want a min prority queque meaning small nums go first
    public int xIndex = -1;  //Negative one will serve as telling us if a incorrect index has been set, as negative one is invalid in an array
    public int yIndex = -1;

    public Vector3 position; //storing our position

    //we need to check what nodes our node is in proximaty to
    public List<Node> neighbours = new List<Node>();

    //One of our nodes will be special// the previous node as it will keep track of our previous node
    public Node previous = null;

    //one of the powers of not using mono is using constructors which host all are pre declared variables and can be called from other scripts for use
    public Node (int xIndex,int yIndex, NodeType nodeType)
    {
        this.xIndex = xIndex;
        this.yIndex = yIndex;
        this.nodeType = nodeType;
    }

    public int CompareTo(Node other) //The node argument is the node we compare to
    {
        if(this.prority<other.prority)
        {
            return -1; //we the node we are on has a smaller prority then we wont go to the next one
        }
        else if(this.prority>other.prority)
        {
            return +1; //if this node is bigger we want to move
        }
        else
        {
            return 0; // otherwise it's equal
        }
    }
    // to invoke we would do node.Compare to(othernode)
    //Sometimes we need to reset and clear the node from previous attemtps 

    public void Reset()
    {
        previous = null;
    }

}
