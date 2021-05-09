using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PathFinder : MonoBehaviour
{
    public enum Mode
    {
        BreadthFirstSearch = 0,
        Dijkstra=1
    }
    public Mode mode =Mode.BreadthFirstSearch;

    Node _startNode;
    Node _goalNode;

    Graph _graph;
    GraphView _graphView;

    ProrityQueue<Node> _frontierNodes;
    List<Node> _exploredNodes;
    List<Node> _pathNodes;

    public Color startColor = Color.green;
    public Color goalColor = Color.red;
    public Color frontierColor = Color.magenta;
    public Color exploredColor = Color.gray;
    public Color pathColor = Color.cyan;
    public Color arrowColor = Color.white;
    public Color highLightColor = Color.yellow;

    public bool complete = false;
    public bool showIterations = true;
    public bool showColors = true;
    public bool showArrows = true;
    public bool exitOnGoal = true;
    int _iterations = 0;

    public void Init (Graph graph, GraphView graphView,Node start,Node goal)
    {

        if (start == null || goal == null || graphView == null)
        {
            Debug.LogWarning("Pathfinder Init error: Missing Components");
            return;
        }
        if (start.nodeType == NodeType.ClosedSet || goal.nodeType == NodeType.ClosedSet)
        {
            Debug.LogWarning("Pathfinder Init error: No way to goal");
        }
        _graph = graph;
        _graphView = graphView;
        _startNode = start;
        _goalNode = goal;

        //we get the start and end nodes and assign their color
        ShowColours(graphView, start, goal);

        _frontierNodes = new ProrityQueue<Node>();
        _frontierNodes.Enqueue(start);
        _exploredNodes = new List<Node>();
        _pathNodes = new List<Node>();

        for (int x = 0; x < _graph.Width; x++)//looping across the width and height of the nodes and taking off the previous node we were on
        {
            for (int y = 0; y < _graph.Height; y++)
            {
                _graph.nodes[x, y].Reset();
            }
        }
        // when init is finished we want to go back to the default state
        complete = false;
        _iterations = 0;
        _startNode.distanceTravelled = 0;//replacing mathf.infinity with real numbers as we start exploring the nodes
    }

    //extracted the show colors from init to form its own function for code readability
    private void ShowColours(GraphView graphView, Node start, Node goal)
    {
        //we check if any of the parameters are null if so stop
        if (graphView == null && _startNode==null&&_goalNode==null)//Issue before, you said if your graph is not null then dont run. What you need was to run if it was null
        {
            return;
        }
        //otherwise if frontier or explored nodes are not null lets add them to the graphview list and color them according to their set purpose
        if(_frontierNodes != null)
        {
            graphView.ColorNodes(_frontierNodes.ToList(), frontierColor);
        }
        if (_exploredNodes != null)
        {
            graphView.ColorNodes(_exploredNodes, exploredColor);
        }
        if(_pathNodes !=null && _pathNodes.Count > 0)
        {
            _graphView.ColorNodes(_pathNodes, pathColor);
        }

        NodeView startNodeView = graphView.nodeViews[start.xIndex, start.yIndex]; //Nodeview is the tile representation of our nodes which is the formation of our created data

        if (startNodeView != null)//Null checks
        {
            startNodeView.ColorNode(startColor);
        }
        NodeView endNodeView = graphView.nodeViews[goal.xIndex, goal.yIndex];

        if (endNodeView != null)
        {
            endNodeView.ColorNode(goalColor);
        }
    }

    void ShowColours()//Overloaded method
    {
        ShowColours(_graphView, _startNode, _goalNode);
    }

    
    public IEnumerator SearchRoutine(float timeStep=0.1f)
    {
        float timeStart = Time.time;
        yield return null;
        while(!complete) //keep looping until complete is false
        {
            if (_frontierNodes.Count > 0) // if our frontier is not yet at zero, we add the frontier nodes to a current node container which is used for our explored nodes list
            {
                Node currentNode = _frontierNodes.Dequeue();
                _iterations++; //how many iterations we have been going on for
                if (!_exploredNodes.Contains(currentNode))
                {
                    _exploredNodes.Add(currentNode);
                }
                if(mode==Mode.BreadthFirstSearch)
                {
                    ExpandFrontierBreadthFirst(currentNode);
                }
                else if(mode==Mode.Dijkstra)
                {
                    ExpandFrontierDijkstra(currentNode);
                }
                
                if(_frontierNodes.Contains(_goalNode))
                {
                    _pathNodes= GetPathNodes(_goalNode);
                    if (exitOnGoal)
                    {
                        complete = true;
                    }
                }
                if(showIterations)
                {
                    ShowDiagnostics();
                    yield return new WaitForSeconds(timeStep);
                }

            }
            else
            {
                complete = true; //only becomes true if there are no neigbours or the frontier has nothing left to explore
            }
            
        }
        ShowDiagnostics();
        Debug.Log("PathFinder SearchRoutine: Elapsed time = " + (Time.time - timeStart).ToString()  + "seconds");
    }

    private void ShowDiagnostics()
    {
        if (showColors)
        {
            ShowColours();
        }

        if (_graphView != null && showArrows)
        {
            _graphView.ShowNodeArrows(_frontierNodes.ToList(), arrowColor);

            if (_frontierNodes.Contains(_goalNode))
            {
                _graphView.ShowNodeArrows(_pathNodes, highLightColor); //path nodes are the ones that form the path to the goal
            }
        }
    }

    void ExpandFrontierBreadthFirst(Node node)
    {
        if(node!=null)
        {
            for (int i = 0; i < node.neighbours.Count; i++)
            {
                if (!_exploredNodes.Contains(node.neighbours[i]) && !_frontierNodes.Contains(node.neighbours[i])) 
                {
                    node.neighbours[i].previous = node; //if it is a node we have not explored then we add our previous node to the breadcrumb trail
                    node.neighbours[i].prority = _exploredNodes.Count;
                    _frontierNodes.Enqueue(node.neighbours[i]);//then the new neighbour gets added to the frontier node  //so if you there is something in front you latch onto your previous neighbour
                }
                
            }
        }
    }

    void ExpandFrontierDijkstra(Node node)
    {
        if (node != null)
        {
            for (int i = 0; i < node.neighbours.Count; i++)
            {
                if (!_exploredNodes.Contains(node.neighbours[i]))
                {
                    float distanceToNeigbour = _graph.GetNodeDistance(node, node.neighbours[i]);
                    float newDistanceTraveled = distanceToNeigbour + node.distanceTravelled;

                    if(float.IsPositiveInfinity(node.neighbours[i].distanceTravelled)|| newDistanceTraveled < node.neighbours[i].distanceTravelled)
                    {
                        node.neighbours[i].previous = node;
                        node.neighbours[i].distanceTravelled = newDistanceTraveled;
                    }
                     if(!_frontierNodes.Contains(node.neighbours[i]))
                    {
                        node.neighbours[i].prority = node.neighbours[i].distanceTravelled;
                        _frontierNodes.Enqueue(node.neighbours[i]);
                    }
                   
                }

            }
        }
    }

    List<Node> GetPathNodes(Node endNode) //building a path back to the begining
    {
        List<Node> path = new List<Node>();
        if(endNode==null)
        {
            return path;
        }
        path.Add(endNode);

        Node currentNode = endNode.previous;

        while (currentNode!=null)
        {
            path.Insert(0, currentNode);
            currentNode = currentNode.previous;
        }
        return path;
    }
}
