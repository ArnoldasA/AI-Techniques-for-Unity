using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Graph))] //will force graph view to always have graph
public class GraphView : MonoBehaviour
{
    public GameObject nodeViewPrefab;
    public NodeView[,] nodeViews;
    public Color baseColor = Color.white;
    public Color wallColor = Color.red;
    public MapData map;
    public void intit(Graph graph)
    {
        if(!map.NotCreating)
        {
            if (graph == null)
            {
                Debug.Log("No Graph");
                return;
            }
            nodeViews = new NodeView[graph.Width, graph.Height];
            foreach (Node n in graph.nodes)
            {
                GameObject instance = Instantiate(nodeViewPrefab, Vector3.zero, Quaternion.identity);
                NodeView nodeView = instance.GetComponent<NodeView>(); // we make a instance of our Nodeview class which contains our nodes
                if (nodeView != null) //if the nodeview is not we wont run otherwise we get each node and we assign either the open or closed set colors
                {
                    nodeView.Init(n);
                    nodeViews[n.xIndex, n.yIndex] = nodeView;

                    if (n.nodeType == NodeType.ClosedSet)
                    {
                        nodeView.ColorNode(wallColor);
                    }
                    else
                    {
                        nodeView.ColorNode(baseColor);
                    }
                }
            }
        }
       
    }
    public void ColorNodes(List<Node> nodes, Color color)
    {
        foreach (Node n in nodes)
        {
            if (n != null)
            {
                NodeView nodeView = nodeViews[n.xIndex, n.yIndex];
                if (nodeView != null)
                {
                    nodeView.ColorNode(color);
                }
            }
        }
    }
    //show arrows if there are nodes
    public void ShowNodeArrows(Node node, Color color)
    {
        if(node!=null)
        {
            NodeView nodeView = nodeViews[node.xIndex, node.yIndex];
            if(nodeView!= null)
            {
                nodeView.ShowArrow(color);
            }
        }
    }

    public void ShowNodeArrows(List<Node> nodes,Color color)
    {
        foreach (var n in nodes)
        {
            ShowNodeArrows(n,color);
        }
    }
}
