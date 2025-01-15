using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellularAutomataGrid : MonoBehaviour
{
    public GameObject Air;
    public GameObject Solid;
    public GameObject Liquid;
    public GameObject Spawner;

    public bool isStarted = false;

    public int XSize = 10;
    public int YSize = 10;
    public int ZSize = 10;

    public int FlowSpeed = 200;

    public float TickDelay = 0.1f; // delay between ticks in seconds
    public enum Shape
    {
        OpenBox, // Box filled with liquid
        FillingBox, // Box with liquid falling from above
        WalledBox,  // Box split in half with walls,
                    // one half full of liquid, wall has a 1 voxel gap

        PipeBox     // Box with a wall in the middle, one half full of liquid,
                    // wall has a 1 voxel tall gap on the bottom
    }
    public Shape ContainerShape = Shape.OpenBox;

    private GameObject[,,] voxelGrid;
    private float tickCounter = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        voxelGrid = new GameObject[XSize, YSize, ZSize];

        for(int x =  0; x < XSize; x++)
        {
            for (int y = 0; y < YSize; y++)
            {
                for(int z = 0; z < ZSize; z++)
                {
                    Vector3 position = new Vector3 (x, y, z);
                    if(ContainerShape == Shape.OpenBox)
                    {
                        if (x == 0 || y == 0 || z == 0 || x == XSize - 1 || z == ZSize - 1)
                        {
                            GameObject voxel = GameObject.Instantiate(Solid, position, new Quaternion(0, 0, 0, 0), this.transform);
                            voxelGrid[x, y, z] = voxel;
                        }
                        else
                        {
                            GameObject voxel = GameObject.Instantiate(Liquid, position, new Quaternion(0, 0, 0, 0), this.transform);
                            voxel.GetComponent<Renderer>().material.color = Color.cyan;
                            voxelGrid[x, y, z] = voxel;
                        }
                    }
                    else if (ContainerShape == Shape.FillingBox)
                    {
                        if (x == 0 || y == 0 || z == 0 || x == XSize - 1 || z == ZSize - 1)
                        { 
                            // add walls to container
                            GameObject voxel = GameObject.Instantiate(Solid, position, new Quaternion(0, 0, 0, 0), this.transform);
                            voxelGrid[x, y, z] = voxel;
                        }
                        else if (x == XSize / 2 && y == YSize - 2 && z == ZSize / 2)
                        {
                            /*
                            // add overfilled liquid voxel simulating adding liquid
                            GameObject voxel = GameObject.Instantiate(Liquid, position, new Quaternion(0, 0, 0, 0), this.transform);
                            voxel.GetComponent<Renderer>().material.color = Color.cyan;
                            // volume should result in a roughly half-filled box
                            voxel.GetComponent<CellularAutomataLiquid>().volume = 1000 * (XSize-2) * (YSize-1) * (ZSize-2);
                            
                            voxelGrid[x, y, z] = voxel;*/
                            
                            // add spawner voxel simulating adding liquid
                            GameObject voxel = GameObject.Instantiate(Spawner, position, new Quaternion(0, 0, 0, 0), this.transform);
                            voxel.GetComponent<Renderer>().material.color = Color.blue;

                            voxelGrid[x, y, z] = voxel;
                            
                        }
                        else
                        {
                            // fill container with air
                            GameObject voxel = GameObject.Instantiate(Air, position, new Quaternion(0, 0, 0, 0), this.transform);
                            voxel.GetComponent<Renderer>().material.color = Color.cyan;
                            voxelGrid[x, y, z] = voxel;
                        }
                    }
                    else if (ContainerShape == Shape.WalledBox)
                    {
                        if (x == 0 || y == 0 || z == 0 || x == XSize - 1 || z == ZSize - 1)
                        {
                            // add walls to container
                            GameObject voxel = GameObject.Instantiate(Solid, position, new Quaternion(0, 0, 0, 0), this.transform);
                            voxelGrid[x, y, z] = voxel;
                        }
                        else if (x == XSize / 2 && z != ZSize - 2 && z != ZSize - 3)
                        {
                            // add wall in the middle
                            GameObject voxel = GameObject.Instantiate(Solid, position, new Quaternion(0, 0, 0, 0), this.transform);
                            voxelGrid[x, y, z] = voxel;
                        }
                        else if (x < XSize / 2)
                        {
                            // fill half of the container with liquid
                            GameObject voxel = GameObject.Instantiate(Liquid, position, new Quaternion(0, 0, 0, 0), this.transform);
                            voxel.GetComponent<Renderer>().material.color = Color.cyan;
                            voxelGrid[x, y, z] = voxel;
                        }
                        else
                        {
                            GameObject voxel = GameObject.Instantiate(Air, position, new Quaternion(0, 0, 0, 0), this.transform);
                            voxel.GetComponent<Renderer>().material.color = Color.cyan;
                            voxelGrid[x, y, z] = voxel;
                        }
                    }
                    else // Pipe box
                    {
                        if (x == 0 || y == 0 || z == 0 || x == XSize - 1 || z == ZSize - 1)
                        {
                            // add walls to container
                            GameObject voxel = GameObject.Instantiate(Solid, position, new Quaternion(0, 0, 0, 0), this.transform);
                            voxelGrid[x, y, z] = voxel;
                        }
                        else if (x == XSize / 2 && y > 1)
                        {
                            // add wall in the middle with a gap on the bottom
                            GameObject voxel = GameObject.Instantiate(Solid, position, new Quaternion(0, 0, 0, 0), this.transform);
                            voxelGrid[x, y, z] = voxel;
                        }
                        else if (x < XSize / 2)
                        {
                            // fill half of the container with liquid
                            GameObject voxel = GameObject.Instantiate(Liquid, position, new Quaternion(0, 0, 0, 0), this.transform);
                            voxel.GetComponent<Renderer>().material.color = Color.cyan;
                            voxelGrid[x, y, z] = voxel;
                        }
                        else
                        {
                            GameObject voxel = GameObject.Instantiate(Air, position, new Quaternion(0, 0, 0, 0), this.transform);
                            voxel.GetComponent<Renderer>().material.color = Color.cyan;
                            voxelGrid[x, y, z] = voxel;
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isStarted)
        {
            tickCounter += Time.deltaTime;
            if (tickCounter > TickDelay)
            {
                checkGrid();
                tickCounter = 0;
            }
        }
    }
    private void checkGrid()
    {
        
        for (int x = 0; x < XSize; x++)
        {
            for (int y = 0; y < YSize; y++)
            {
                for (int z = 0; z < ZSize; z++)
                {
                    checkVoxel(x, y, z);
                }
            }
        }
    }
    private void checkVoxel(int x, int y, int z)
    {
        CellularAutomataLiquid curVoxel = voxelGrid[x, y, z].GetComponent<CellularAutomataLiquid>();
        if (curVoxel.isSpawner)
        {
            voxelGrid[x, y - 1, z].GetComponent<CellularAutomataLiquid>().volume = 1000;
            return;
        }
        if (curVoxel.volume == 0 || curVoxel.isSolid)
        {
            return;
        }

        int maxFlow;
        int flow;
        List<CellularAutomataLiquid> flowDestinations = new List<CellularAutomataLiquid>();
        

        if (y != 0) // flow down
        {
            CellularAutomataLiquid lowerNeighbor = voxelGrid[x, y - 1, z].GetComponent<CellularAutomataLiquid>();
            if (lowerNeighbor.isSolid != true &&
                lowerNeighbor.volume < 1000)
            {
                // flows downward at any speed
                flow = Mathf.Min(1000 - lowerNeighbor.volume, curVoxel.volume);

                lowerNeighbor.volume += flow;
                curVoxel.volume -= flow;
                
                lowerNeighbor.UpdateVolume();
                curVoxel.UpdateVolume();
            }
        }
        if (curVoxel.volume == 0)
        {
            return;
        }
        if(x != 0) // check x-1 (west)
        {
            CellularAutomataLiquid neighbor = voxelGrid[x - 1, y, z].GetComponent<CellularAutomataLiquid>();
            if (neighbor.isSolid != true &&
                neighbor.volume < curVoxel.volume)
            {
                flowDestinations.Add(neighbor);
            }
        }
        if(x != XSize - 1) // check x+1 (east)
        {
            CellularAutomataLiquid neighbor = voxelGrid[x + 1, y, z].GetComponent<CellularAutomataLiquid>();
            if (neighbor.isSolid != true &&
                neighbor.volume < curVoxel.volume)
            {
                flowDestinations.Add(neighbor);
            }
        }
        if(z != 0) // check z-1 (south)
        {
            CellularAutomataLiquid neighbor = voxelGrid[x, y, z - 1].GetComponent<CellularAutomataLiquid>();
            if (neighbor.isSolid != true &&
                neighbor.volume < curVoxel.volume)
            {
                flowDestinations.Add(neighbor);
            }
        }
        if(z !=  ZSize - 1) // check z+1 (north)
        {
            CellularAutomataLiquid neighbor = voxelGrid[x, y, z + 1].GetComponent<CellularAutomataLiquid>();
            if (neighbor.isSolid != true &&
                neighbor.volume < curVoxel.volume)
            {
                flowDestinations.Add(neighbor);
            }
        }

        maxFlow = FlowSpeed;
        foreach (CellularAutomataLiquid destination in flowDestinations)
        {
            flow = Mathf.Min((curVoxel.volume - destination.volume) / 2, maxFlow, curVoxel.volume);
            if (curVoxel.isLogged)
            {
                Debug.Log(((curVoxel.volume - destination.volume) / 2).ToString() + " "
                + curVoxel.volume.ToString() + " " + destination.volume.ToString() + " flow: " + flow.ToString()
                + " neighbors: " + flowDestinations.Count);
            };
            destination.volume += flow;
            curVoxel.volume -= flow;

            destination.UpdateVolume();
            curVoxel.UpdateVolume();
        }
    }
}
