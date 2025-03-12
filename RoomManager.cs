// Room class
public class Room
{
    private Dictionary<string, Room> exits = new Dictionary<string, Room>();

    public Room() { }

    public void setExit(string direction, Room neighbor)
    {
        exits[direction] = neighbor;
    }

    public bool hasExit(string direction)
    {
        return exits.ContainsKey(direction);
    }

    public Room getExit(string direction)
    {
        if (exits.ContainsKey(direction))
        {
            return exits[direction];
        }
        return null;
    }
    
    public bool tryToTakeExit(string direction)
    {
        if (exits.ContainsKey(direction))
        {
            Core.thePlayer.setCurrentRoom(exits[direction]);
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

    void Start()
    {
        Core.thePlayer = new Player("Mike");
        this.theDungeon = new Dungeon();
        this.setupRoom();
    }

    private void resetRoom()
    {
        foreach (GameObject door in theDoors)
        {
            door.SetActive(false);
        }
    }

    private void setupRoom()
    {
        Room currentRoom = Core.thePlayer.getCurrentRoom();
        theDoors[0].SetActive(currentRoom.hasExit("north"));
        theDoors[1].SetActive(currentRoom.hasExit("south"));
        theDoors[2].SetActive(currentRoom.hasExit("east"));
        theDoors[3].SetActive(currentRoom.hasExit("west"));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (Core.thePlayer.getCurrentRoom().tryToTakeExit("north"))
            {
                setupRoom();
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (Core.thePlayer.getCurrentRoom().tryToTakeExit("west"))
            {
                setupRoom();
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Core.thePlayer.getCurrentRoom().tryToTakeExit("east"))
            {
                setupRoom();
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (Core.thePlayer.getCurrentRoom().tryToTakeExit("south"))
            {
                setupRoom();
            }
        }
    }
}

   
