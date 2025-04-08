using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public MapData mapData;
    public Graph graph;
    GraphView graphView;
    int[,] mapCopy;
    public float timeStep = 0.1f;
    bool paused = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if (mapData != null && graph != null) {
            int[,] mapInstance = mapData.MakeMap();
            graph.Init(mapInstance);
            mapCopy = mapData.MakeMap();

            graphView = graph.GetComponent<GraphView>();
            if (graphView != null) {
                graphView.Init(graph, this);
            } else {
                Debug.Log("No graph view is found");
            }
            ShowColors();
            StartCoroutine(GameRoutine());
        }
    }
    public IEnumerator GameRoutine() {
        yield return null;
        while (true) {

            // stop the game while paused
            while (paused) {
                // foreach loop to update mapCopy when a node is clicked
                foreach (Node n in graph.nodes) {
                    mapCopy[(int)n.position.x, (int)n.position.z] = (int)n.nodeType;
                }

                yield return new WaitForSeconds(timeStep);
            }

            // standard game loop
            for (int r = 0; r < graph.getWidth(); r++) {
                for (int c = 0; c < graph.getHeight(); c++) {
                    Node current = graph.nodes[r, c];
                    int liveNeighbors = current.CountAliveNeighbors();

                    // overpopulation
                    if (current.nodeType == NodeType.alive && liveNeighbors > 3) {
                        // graphCopy.nodes[r, c].nodeType = NodeType.dead;
                        mapCopy[r, c] = (int)NodeType.dead;
                    }
                    // underpopulation
                    if (current.nodeType == NodeType.alive && liveNeighbors < 2) {
                        // graphCopy.nodes[r, c].nodeType = NodeType.dead;
                        Debug.Log("Cell: (" + r + ", " + c + ") Live Neighbors: " + liveNeighbors);
                        mapCopy[r, c] = (int)NodeType.dead;
                    }
                    if (current.nodeType == NodeType.dead && liveNeighbors == 3) {
                        // graphCopy.nodes[r, c].nodeType = NodeType.alive;
                        mapCopy[r, c] = (int)NodeType.alive;
                    }
                }
            }
            // put mapCopy into graph
            graph.UpdateMapData(mapCopy);
            ShowColors();
            yield return new WaitForSeconds(timeStep);
        }
    }

    public void Clear() {
        foreach (Node n in graph.nodes) {
            n.nodeType = NodeType.dead;
        }
        graph.UpdateDeadAndAlive();
        ShowColors();
    }
    public void ShowColors() {
        if (graphView == null) {
            return;
        }

        graphView.ColorNodes(graph.aliveNodes, graphView.nodeViews[0, 0].aliveColor);
        graphView.ColorNodes(graph.deadNodes, graphView.nodeViews[0, 0].deadColor);

        // NodeView startNodeView = graphView.nodeViews[start.xIndex, start.yIndex];
        // NodeView goalNodeView = graphView.nodeViews[goal.xIndex, goal.yIndex];

        // if (frontierNodes != null) {
        //     graphView.ColorNodes(frontierNodes.ToList(), frontierColor);
        // }
        // if (exploreNodes != null) {
        //     graphView.ColorNodes(exploreNodes, exploreColor);
        // }
        // if (pathNodes != null) {
        //     graphView.ColorNodes(pathNodes, pathColor);
        // }

        // if (startNodeView != null) {
        //     startNodeView.ColorNode(startColor);
        // } else {
        //     Debug.LogWarning("StartNodeView does not exist");
        // }
        // if (goalNodeView != null) {
        //     goalNodeView.ColorNode(goalColor);
        // } else {
        //     Debug.LogWarning("GoalNodeView does not exist");
        // }
    }

    public void SetPaused(bool paused) {
        this.paused = paused;
    }
    public bool GetPaused() {
        return paused;
    }
}
