using System.Collections;
using System.Collections.Generic;
using Consequences;
using UnityEngine;

public class ElevatorConnectedNodeGrid : MonoBehaviour
{
    public GameObject nodePrefab;
    public Vector2Int gridDimensions;

    protected const float widthOfNode = 23.5f;
    protected const float heightOfNode = 32.5f;

    void Start()
    {
        Generate();
    }

    private void Generate()
    {
        for (int x = 0; x < gridDimensions.x; x++)
        {
            for (int z = 0; z < gridDimensions.y; z++)
            {
                GameObject instantiatedNode = instantiateNode(x, z);
                Transform elevators = instantiatedNode.transform.Find("ElevatorLobbyElevators");
                SetElevatorDestination(elevators, "PositiveXElevator", x + 1, z);
                SetElevatorDestination(elevators, "NegativeXElevator", x - 1, z);
                SetElevatorDestination(elevators, "PositiveZElevator", x, z + 1);
                SetElevatorDestination(elevators, "NegativeZElevator", x, z - 1);
                //instantiate unique props
            }
        }

        //generate start (or place player at start, if it's on the grid)
        //generate goal (if it's not on the grid)
    }

    protected void SetElevatorDestination(Transform root, string elevatorName, int destinationGridX,
        int destinationGridZ)
    {
        root.Find(elevatorName).GetComponentInChildren<MoveLobbiesConsequence>().destinationLobbyGridCoordinates =
            new Vector2Int(clampToGrid(this.gridDimensions.x, destinationGridX), clampToGrid(this.gridDimensions.y, destinationGridZ));
    }

    private int clampToGrid(int gridWidth, int value)
    {
        if (value < 0)
        {
            return gridWidth - 1;
        }
        else if (value >= gridWidth)
        {
            return 0;
        }
        else
        {
            return value;
        }
    }

    protected GameObject instantiateNode(int x, int z)
    {
        GameObject instantiated = GameObject.Instantiate(nodePrefab, new Vector3(widthOfNode * x, 0, heightOfNode * z),
            Quaternion.identity, this.gameObject.transform);
        instantiated.name = coordinatesToName(new Vector2Int(x, z));
        return instantiated;
    }

    protected string coordinatesToName(Vector2Int coordinates)
    {
        return coordinates.Equals(Vector2Int.zero) ? "PuzzleStart" :
            coordinates.Equals(new Vector2Int(999, 999)) ? "PuzzleGoal" :
            (coordinates.x == 0 ? "" : coordinates.x.ToString() + "X") +
            (coordinates.y == 0 ? "" : coordinates.y.ToString() + "Z");
    }
}