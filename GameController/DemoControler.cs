using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoControler : MonoBehaviour
{
    public MapData mapData;
    public Graph graph;
    public PathFinder pathFinder;
    public int startX = 0;
    public int startY = 0;

    public int goalX = 15;
    public int goalY = 1;
    int[,] mapInstance;
    public float TimeControl=0.1f;
    bool test;
    private void Update()
    {
        //we here call the intilization methods from previous scripts, for example graph.init runs through the mapData information to form the map  and then graphview is a second check to make sure graphview is not null
      
    }


    public void Start()
    {

        if (!mapData.NotCreating && graph != null)
        {
             mapInstance = mapData.MakeMap();
            graph.Init(mapInstance);

            GraphView graphView = graph.gameObject.GetComponent<GraphView>();

            if (graphView != null)
            {
                graphView.intit(graph);
            }
            if (graph.IsWithInBounds(startX, startY) && graph.IsWithInBounds(goalX, goalY) && pathFinder != null)
            {
                Node startNode = graph.nodes[startX, startY];
                Node endNode = graph.nodes[goalX, goalY];
                pathFinder.Init(graph, graphView, startNode, endNode);
                StartCoroutine(pathFinder.SearchRoutine(TimeControl));
            }

        }
          
        }
      
        

    }
    

