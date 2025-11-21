using UnityEngine;

/// <summary>
/// Simple player representation
/// All interaction and navigation is handled through the UI menu system
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField] private int playerNumber = 1; // 1 or 2

    public int PlayerNumber => playerNumber;
}
