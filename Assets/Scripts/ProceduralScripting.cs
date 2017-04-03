using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class Room
{
    public GameObject roomGameObject;
    public Room next;
    public Room previous;

    public Room(GameObject room)
    {
        roomGameObject = room;   
    }

    public void Remove()
    {
        if (previous != null)
        {
            previous.next = next;
        }
        if (next != null)
        {
            next.previous = previous;
        }
    }
    
    public static Room SearchRooms(Room pointOfStart, int index)
    {
        Room room = pointOfStart;
        if(index == 1)
        {
            return room.next;
        }
        for(int i = 0; i < index - 1; i++)
        {
            room = room.next;
        }
        return room;
    }

    public static Room SearchRoomWithGameObject(Room pointOfStart, GameObject gameObjectToFind)
    {
        Room room = pointOfStart;
        while(true)
        {
            if(room.roomGameObject.Equals(gameObjectToFind))
            {
                break;
            }
            room = room.next;
        }
        return room;
    }
}

public class ProceduralScripting : MonoBehaviour
{
    [SerializeField]
    GameObject[] roomsGameObject;
    List<Room> rooms = new List<Room>();
    GameObject[,] grid = new GameObject[3,3];
    System.Random rnd = new System.Random();

    void Awake()
    {
        GenerateGrid();   
        
        foreach(GameObject r in grid)
        {
            Debug.Log(r.name);
        }
    }

    void GenerateGrid()
    {
        foreach (GameObject r in roomsGameObject)
        {
            rooms.Add(new Room(r));
        }

        for (int i = 0; i < rooms.Count; i++)
        {
            if (i == 0)
            {
                rooms[i].next = rooms[i + 1];
            }
            else if (i == rooms.Count - 1)
            {
                rooms[i].previous = rooms[i - 1];
            }
            else
            {
                rooms[i].next = rooms[i + 1];
                rooms[i].previous = rooms[i - 1];
            }
        }

        for (int i = 0; i <= 2; i++)
        {
            for (int j = 0; j <= 2; j++)
            {
                if (i.Equals(1) && j.Equals(1))
                {
                    grid[i, j] = rooms[0].roomGameObject;
                }
                else
                {
                    grid[i, j] = Room.SearchRooms(rooms[0], rnd.Next(1, rooms.Count - (2 + j + i))).roomGameObject;
                    Room.SearchRoomWithGameObject(rooms[0], grid[i, j]).Remove();
                }
            }
        }
    }
}
