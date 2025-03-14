using UnityEngine;
using System.Collections.Generic;

// A simple static class to hold global game state.
public static class Core
{
    public static Player thePlayer;
}

// A minimal Player class.
public class Player
{
    // For simplicity, the player holds a current room.
    private Room currentRoom;

    public Player(string name)
    {
        // When the player is created, they start in a new Room.
        currentRoom = new Room();
    }

    // Returns the player's current room.
    public Room getCurrentRoom()
    {
        return currentRoom;
    }

    // You can add a setter or other logic depending on how you manage room transitions.
    public void setCurrentRoom(Room newRoom)
    {
        currentRoom = newRoom;
    }
}

// A basic Room class that represents a dungeon room.
public class Room
{
    // Dictionary to track which exits (directions) exist for this room.
    // In a more advanced implementation, these values might vary.
    private Dictionary<string, bool> exits = new Dictionary<string, bool>();

    public Room()
    {
        // For demonstration, all four exits exist.
        exits["north"] = true;
        exits["south"] = true;
        exits["east"]  = true;
        exits["west"]  = true;
    }

    // Returns whether an exit in a given direction is present.
    public bool hasExit(string direction)
    {
        return exits.ContainsKey(direction) && exits[direction];
    }

    // Attempt to take an exit. In your real game this would update the player's current room.
    public bool tryToTakeExit(string direction)
    {
        if (hasExit(direction))
        {
            Debug.Log("Moving " + direction);
            // For this demo, simply return true.
            // You could update the player's room (e.g. Core.thePlayer.setCurrentRoom(newRoom)) here.
            return true;
        }
        Debug.Log("No exit " + direction + " available.");
        return false;
    }
}

// A very basic Dungeon class.
// In a more complete implementation, you might build the entire dungeon layout here.
public class Dungeon
{
    public Dungeon()
    {
        // Build your dungeon or load layout data here.
    }
}

// The RoomManager handles moving between rooms and updating the mini-map.
public class RoomManager : MonoBehaviour
{
    public GameObject[] theDoors;
    public GameObject mmRoomPrefab;

    private Dungeon theDungeon;

    // Dictionary to store mini-map room GameObjects keyed by their grid coordinate.
    private Dictionary<Vector2Int, GameObject> miniMapRooms = new Dictionary<Vector2Int, GameObject>();

    // The player's current position on the mini-map grid.
    private Vector2Int playerMapCoord = Vector2Int.zero;

    // Spacing between mini-map rooms on the UI.
    private float spacing = 1.2f;

    void Start()
    {
        Core.thePlayer = new Player("Mike");
        theDungeon = new Dungeon();

        // Place the starting mini-map room at grid coordinate (0, 0).
        Vector3 mmWorldPos = GetMiniMapWorldPosition(playerMapCoord);
        GameObject startingMMRoom = Instantiate(mmRoomPrefab, mmWorldPos, Quaternion.identity);
        miniMapRooms.Add(playerMapCoord, startingMMRoom);

        SetupRoom();
    }

    // Disable all door GameObjects.
    private void ResetRoom()
    {
        foreach (GameObject door in theDoors)
        {
            door.SetActive(false);
        }
    }

    // Shows or hides the doors based on the exits available in the current room.
    private void SetupRoom()
    {
        Room currentRoom = Core.thePlayer.getCurrentRoom();
        theDoors[0].SetActive(currentRoom.hasExit("north"));
        theDoors[1].SetActive(currentRoom.hasExit("south"));
        theDoors[2].SetActive(currentRoom.hasExit("east"));
        theDoors[3].SetActive(currentRoom.hasExit("west"));
    }

    // Convert a grid coordinate (Vector2Int) into a world position for the mini-map.
    private Vector3 GetMiniMapWorldPosition(Vector2Int coord)
    {
        // Assuming the mini-map is laid out on the XZ plane.
        return new Vector3(coord.x * spacing, 0f, coord.y * spacing);
    }

    // Called once per frame.
    void Update()
    {
        bool didChangeRoom = false;
        Vector2Int movement = Vector2Int.zero; // Determines the movement on the grid.

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Try moving north.
            didChangeRoom = Core.thePlayer.getCurrentRoom().tryToTakeExit("north");
            movement = new Vector2Int(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Try moving west.
            didChangeRoom = Core.thePlayer.getCurrentRoom().tryToTakeExit("west");
            movement = new Vector2Int(-1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Try moving east.
            didChangeRoom = Core.thePlayer.getCurrentRoom().tryToTakeExit("east");
            movement = new Vector2Int(1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // Try moving south.
            didChangeRoom = Core.thePlayer.getCurrentRoom().tryToTakeExit("south");
            movement = new Vector2Int(0, -1);
        }

        if (didChangeRoom)
        {
            // Update the player's mini-map grid coordinate.
            playerMapCoord += movement;

            // Only instantiate a new mini-map room if it doesn't already exist.
            if (!miniMapRooms.ContainsKey(playerMapCoord))
            {
                Vector3 mmWorldPosition = GetMiniMapWorldPosition(playerMapCoord);
                GameObject newMMRoom = Instantiate(mmRoomPrefab, mmWorldPosition, Quaternion.identity);
                miniMapRooms.Add(playerMapCoord, newMMRoom);
            }

            // Update the door display for the new room on the mini-map.
            SetupRoom();
        }
    }
}
