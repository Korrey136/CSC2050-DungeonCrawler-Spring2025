using UnityEngine;
using System.Collections.Generic;

// Core class with a static Player variable
public static class Core
{
    public static Player thePlayer;
}

// Player class
public class Player
{
    private string name;
    private Room currentRoom;

    public Player(string name)
    {
        this.name = name;
    }

    public void SetCurrentRoom(Room room)
    {
        this.currentRoom = room;
    }

    public Room GetCurrentRoom()
    {
        return currentRoom;
    }
}

// Dungeon class (basic structure)
public class Dungeon
{
    // Add your dungeon logic here
}

// Room class
public class Room
{
    private Dictionary<string, Room> exits = new Dictionary<string, Room>();

    public Room() { }

    public void SetExit(string direction, Room neighbor)
    {
        exits[direction] = neighbor;
    }

    public bool HasExit(string direction)
    {
        return exits.ContainsKey(direction);
    }

    public Room GetExit(string direction)
    {
        if (exits.ContainsKey(direction))
        {
            return exits[direction];
        }
        return null;
    }

    public bool TryToTakeExit(string direction)
    {
        if (exits.ContainsKey(direction))
        {
            Core.thePlayer.SetCurrentRoom(exits[direction]);
            return true;
        }
        return false;
    }
}

// RoomManager class
public class RoomManager : MonoBehaviour
{
    public GameObject[] theDoors;
    private Dungeon theDungeon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Core.thePlayer = new Player("Mike");
        this.theDungeon = new Dungeon();
        SetupRoom();
    }

    // Disable all doors
    private void ResetRoom()
    {
        foreach (GameObject door in theDoors)
        {
            door.SetActive(false);
        }
    }

    // Show the doors appropriate to the current room
    private void SetupRoom()
    {
        Room currentRoom = Core.thePlayer.GetCurrentRoom();
        theDoors[0].SetActive(currentRoom.HasExit("north"));
        theDoors[1].SetActive(currentRoom.HasExit("south"));
        theDoors[2].SetActive(currentRoom.HasExit("east"));
        theDoors[3].SetActive(currentRoom.HasExit("west"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Try to go to the north
            if (Core.thePlayer.GetCurrentRoom().TryToTakeExit("north"))
            {
                SetupRoom();
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Try to go to the west
            if (Core.thePlayer.GetCurrentRoom().TryToTakeExit("west"))
            {
                SetupRoom();
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Try to go to the east
            if (Core.thePlayer.GetCurrentRoom().TryToTakeExit("east"))
            {
                SetupRoom();
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // Try to go to the south
            if (Core.thePlayer.GetCurrentRoom().TryToTakeExit("south"))
            {
                SetupRoom();
            }
        }
    }
}


   
