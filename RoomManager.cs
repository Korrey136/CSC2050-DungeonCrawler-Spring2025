using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public GameObject[] theDoors;
    public GameObject mmRoomPrefab;

    private Dungeon theDungeon;

    // Dictionary to store the mini-map room game objects using a grid coordinate key,
    // ensuring that the same room is not duplicated.
    private Dictionary<Vector2Int, GameObject> miniMapRooms = new Dictionary<Vector2Int, GameObject>();

    // Player's current position on the mini-map grid.
    private Vector2Int playerMapCoord = Vector2Int.zero;

    // Spacing between mini-map rooms (adjust as needed for your layout).
    private float spacing = 1.2f;

    void Start()
    {
        Core.thePlayer = new Player("Mike");
        theDungeon = new Dungeon();

        // Instantiate the starting mini-map room at (0,0)
        Vector3 mmWorldPos = GetMiniMapWorldPosition(playerMapCoord);
        GameObject startingMMRoom = Instantiate(mmRoomPrefab, mmWorldPos, Quaternion.identity);
        miniMapRooms.Add(playerMapCoord, startingMMRoom);

        setupRoom();
    }

    // Disables all doors.
    private void resetRoom()
    {
        foreach (GameObject door in theDoors)
        {
            door.SetActive(false);
        }
    }

    // Sets up the doors based on the exits available in the current room.
    private void setupRoom()
    {
        Room currentRoom = Core.thePlayer.getCurrentRoom();
        theDoors[0].SetActive(currentRoom.hasExit("north"));
        theDoors[1].SetActive(currentRoom.hasExit("south"));
        theDoors[2].SetActive(currentRoom.hasExit("east"));
        theDoors[3].SetActive(currentRoom.hasExit("west"));
    }

    // Converts a grid coordinate into a world position for the mini-map room.
    private Vector3 GetMiniMapWorldPosition(Vector2Int coord)
    {
        // Assume the mini-map is built on the XZ plane
        return new Vector3(coord.x * spacing, 0f, coord.y * spacing);
    }

    // Update is called once per frame.
    void Update()
    {
        bool didChangeRoom = false;
        Vector2Int movement = Vector2Int.zero; // movement vector for grid advancement

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Try to go north.
            didChangeRoom = Core.thePlayer.getCurrentRoom().tryToTakeExit("north");
            movement = new Vector2Int(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Try to go west.
            didChangeRoom = Core.thePlayer.getCurrentRoom().tryToTakeExit("west");
            movement = new Vector2Int(-1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Try to go east.
            didChangeRoom = Core.thePlayer.getCurrentRoom().tryToTakeExit("east");
            movement = new Vector2Int(1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // Try to go south.
            didChangeRoom = Core.thePlayer.getCurrentRoom().tryToTakeExit("south");
            movement = new Vector2Int(0, -1);
        }

        if (didChangeRoom)
        {
            // Update the player's mini-map coordinate.
            playerMapCoord += movement;

            // Instantiate a new mini-map room if one does not already exist at this coordinate.
            if (!miniMapRooms.ContainsKey(playerMapCoord))
            {
                Vector3 mmWorldPosition = GetMiniMapWorldPosition(playerMapCoord);
                GameObject newMMRoom = Instantiate(mmRoomPrefab, mmWorldPosition, Quaternion.identity);
                miniMapRooms.Add(playerMapCoord, newMMRoom);
            }

            // Update the door configuration for the new room.
            setupRoom();
        }
    }
}
