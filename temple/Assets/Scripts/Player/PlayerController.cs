using UnityEngine;

/// <summary>
/// Simple player representation
/// All interaction and navigation is handled through the UI menu system
/// </summary>
public class PlayerController : MonoBehaviour
{
    public NavigationNode currentNode;

    public void SetStartingNode(NavigationNode startingNode)
    {
        currentNode = startingNode;
        transform.position = startingNode.transform.position;
        // TODO activate current puzzles.
    }
}