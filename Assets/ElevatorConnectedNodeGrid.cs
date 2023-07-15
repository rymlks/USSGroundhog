using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Consequences;
using StaticUtils;
using UnityEngine;
using static StaticUtils.MathUtil;
using Random = System.Random;

public class ElevatorConnectedNodeGrid : MonoBehaviour
{
    public GameObject goalPrefab;
    public GameObject nodePrefab;
    public Vector2Int gridDimensions;

    protected Vector2Int[] gridDirections = {Vector2Int.down, Vector2Int.left, Vector2Int.right, Vector2Int.up};

    protected const float widthOfNode = 23.5f;
    protected const float heightOfNode = 32.5f;

    void Start()
    {
        if (goalPrefab == null)
        {
            goalPrefab = nodePrefab;
        }

        Generate();
    }

    private void Generate()
    {
        GenerateIntermediateNodes();
        GenerateGoalNode();
        DesignateGoalElevator();
    }

    private void DesignateGoalElevator()
    {
        Random random = new Random();
        //TODO: this should not be actually random and should privilege the center of the grid over the sides
        Vector2Int goalElevatorCoordinates =
            new Vector2Int(random.Next(this.gridDimensions.x), random.Next(gridDimensions.y));
        Vector2Int goalElevatorDirection = (Vector2Int) MathUtil.chooseRandom<Vector2Int>(random, gridDirections.ToList());
        string cardinalDirectionToString2D = UnityUtil.cardinalDirectionToString2D(goalElevatorDirection) + "Elevator";
        Transform elevators = GameObject.Find(coordinatesToName(goalElevatorCoordinates)).transform.Find("ElevatorLobbyElevators");
        SetElevatorDestination(elevators, cardinalDirectionToString2D, 999, 999);
    }

    private GameObject GenerateGoalNode()
    {
        GameObject instantiatedNode = instantiateNode(999, 999);
        instantiatedNode.transform.position = new Vector3(0, instantiatedNode.transform.position.y - 100, 0);
        //instantiate goal props
        return instantiatedNode;
    }

    private void GenerateIntermediateNodes()
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
                //instantiate unique props per generated room
            }
        }
    }

    protected void SetElevatorDestination(Transform root, string elevatorName, int destinationGridX,
        int destinationGridZ)
    {
        root.Find(elevatorName).GetComponentInChildren<MoveLobbiesConsequence>().destinationLobbyGridCoordinates =
            new Vector2Int(ClampPositiveInt(this.gridDimensions.x, destinationGridX), ClampPositiveInt(this.gridDimensions.y, destinationGridZ));
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