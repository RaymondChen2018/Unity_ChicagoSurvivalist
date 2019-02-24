using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_generator : MonoBehaviour {
    //
    
    [SerializeField] private Destination_trigger exit_object;
    [SerializeField] private GameObject right_bound;
    [SerializeField] private GameObject left_bound;
    [SerializeField] private GameObject up_bound;
    [SerializeField] private GameObject lower_bound;
    
    public const float map_size = 150;
    static Vector2 bottomleft = -Vector2.one * map_size;
    static Vector2 topright = Vector2.one * map_size;
    static float sidewalk_height = 0.1f;
    static float exit_trig_width = 1f;

    [SerializeField] private float road_w_min = 2;
    [SerializeField] private float road_w_max = 6;
    [SerializeField] private float building_w_min = 6;
    [SerializeField] private float building_w_max = 15;
    [SerializeField] private float building_h_min = 15;
    [SerializeField] private float building_h_max = 50;
    [SerializeField] private float building_w_contract_limit_min = 0.1f;//side walk
    [SerializeField] private float building_w_contract_limit_max = 0.3f;

    [SerializeField] private GameObject[] building_template;
    [SerializeField] private GameObject sidewalk_template;


    static Vector3 spawnLocation = new Vector3();
    static Vector3 destinLocation = new Vector3();

    //Temp variable
    Vector3 tempVec = new Vector3();

    public class t1_road
    {
        public float upper;
        public float lower;
    }

    public class t2_building
    {
        public List<Vector2> vertices = new List<Vector2>();
        public float height = 50;
        public bool isMerged = false;
    };

    //Generation stat; Map dimension: (-150, -150) to (150, 150)
    /// <summary>
    /// 0 bottom-most; max top-most
    /// </summary>
    List<t1_road> horizontal_roads = new List<t1_road>();
    /// <summary>
    /// 0 left-most; max right-most
    /// </summary>
    List<t1_road> vertical_roads = new List<t1_road>();
    /// <summary>
    /// Buildings cover the edge, not roads
    /// </summary>
    List<List<t2_building>> buildings = new List<List<t2_building>>();
    //Pool objects
    public List<GameObject> building_pool = new List<GameObject>();
    List<GameObject> sidewalk_pool = new List<GameObject>();


    //Initialize cluster and building stats, create objects/modify pool to build city
    public void map_generate()
    {

        //Initialize roads structs=================================================================
        vertical_roads.Clear();
        horizontal_roads.Clear();
        float road_pointer = -map_size;
        while(road_pointer + building_w_min < map_size)
        {
            road_pointer += Random.Range(building_w_min, building_w_max);
            t1_road road = new t1_road();
            road.lower = road_pointer;
            road.upper = road_pointer + Random.Range(road_w_min, road_w_max);
            road_pointer = road.upper;
            vertical_roads.Add(road);
        }
        road_pointer = -map_size;
        while (road_pointer + building_w_min < map_size)
        {
            road_pointer += Random.Range(building_w_min, building_w_max);
            t1_road road = new t1_road();
            road.lower = road_pointer;
            road.upper = road_pointer + Random.Range(road_w_min, road_w_max);
            road_pointer = road.upper;
            horizontal_roads.Add(road);
        }

        //Place buildings =================================================================
        //Instantiate buildings structs
        for (int i = 0; i < buildings.Count; i++)
        {
            buildings[i].Clear();
        }
        buildings.Clear();
        int temp = vertical_roads.Count + 1 - buildings.Count;
        for (int i = 0; i < temp; i++)
        {
            buildings.Add(new List<t2_building>());
        } 
        for (int i = 0; i < buildings.Count; i++)
        {
            temp = horizontal_roads.Count + 1 - buildings[i].Count;
            for (int j = 0; j < temp; j++)
            {
                buildings[i].Add(new t2_building());
            }
        }
        //Create building gameobjects
        int building_count = buildings.Count * buildings[0].Count;
        temp = building_count - building_pool.Count;
        for (int i = 0; i < temp; i++)
        {
            building_pool.Add(Instantiate(building_template[Random.Range(0,building_template.Length)]));
            sidewalk_pool.Add(Instantiate(sidewalk_template));
        }
        temp = building_pool.Count - building_count;
        for (int i = 0; i < temp; i++)
        {
            Destroy(building_pool[0]);
            Destroy(sidewalk_pool[0]);
            building_pool.RemoveAt(0);
            sidewalk_pool.RemoveAt(0);
        }
        //Set buildings gameobjects
        for (int i = 0; i < buildings.Count; i++)
        {
            //Defining bounds
            float xboundl, xboundr;
            if (i == 0)
            {
                xboundl = -map_size;
            }
            else
            {
                xboundl = vertical_roads[i - 1].upper;
            }
            if (i == buildings.Count - 1)
            {
                if(map_size - vertical_roads[vertical_roads.Count - 1].upper >= building_w_min)
                {
                    xboundr = map_size;
                }
                else
                {
                    xboundr = vertical_roads[vertical_roads.Count - 1].upper + building_w_min;
                }
            }
            else
            {
                xboundr = vertical_roads[i].lower;
            }

            for (int j = 0; j < buildings[i].Count; j++)
            {
                //Defining y bound
                float yboundl, yboundu;
                if (j == 0)
                {
                    yboundl = -map_size;
                }
                else
                {
                    yboundl = horizontal_roads[j - 1].upper;
                }
                if (j == buildings[i].Count - 1)
                {
                    if (map_size - horizontal_roads[horizontal_roads.Count - 1].upper >= building_w_min)
                    {
                        yboundu = map_size;
                    }
                    else
                    {
                        yboundu = horizontal_roads[horizontal_roads.Count - 1].upper + building_w_min;
                    }
                }
                else
                {
                    yboundu = horizontal_roads[j].lower;
                }
                //
                buildings[i][j].isMerged = false;
                buildings[i][j].vertices.Clear();
                
                buildings[i][j].vertices.Add(new Vector2(xboundl, yboundu));
                buildings[i][j].vertices.Add(new Vector2(xboundr, yboundu));
                buildings[i][j].vertices.Add(new Vector2(xboundr, yboundl));
                buildings[i][j].vertices.Add(new Vector2(xboundl, yboundl));
                buildings[i][j].height = Random.Range(building_h_min, building_h_max);
                float building_w =  (xboundr - xboundl);
                float building_h =  (yboundu - yboundl);
                sidewalk_pool[i * buildings[0].Count + j].transform.localScale = new Vector3(building_w, sidewalk_height, building_h);
                sidewalk_pool[i * buildings[0].Count + j].transform.position = new Vector3((xboundr - xboundl) / 2 + xboundl, sidewalk_height / 2, (yboundu - yboundl) / 2 + yboundl);
                building_w *= Random.Range(1 - building_w_contract_limit_max, 1 - building_w_contract_limit_min);
                building_h *= Random.Range(1 - building_w_contract_limit_max, 1 - building_w_contract_limit_min);
                building_pool[i * buildings[0].Count + j].transform.localScale = new Vector3(building_w, buildings[i][j].height, building_h);
                building_pool[i * buildings[0].Count + j].transform.position = new Vector3((xboundr - xboundl) / 2 + xboundl, buildings[i][j].height/2, (yboundu - yboundl) / 2 + yboundl);

                
            }
        }
        //Rebake cubemap for reflective buildings
        bakeCubeMap();

        //Place bounds=================================================================
        setBound();

        //Place exit=================================================================
        setDestinationRandomPos();

        //Place player=================================================================
        setPlayerRandomPos();
    }

    public void Reset()
    {
        
    }

    void bakeCubeMap()
    {
        for (int i = 0; i < building_pool.Count; i++)
        {
            One_time_cubemap cubemap_script = building_pool[i].GetComponent<One_time_cubemap>();
            if (cubemap_script != null)
            {
                cubemap_script.Render();
            }
        }
    }

    void setBound()
    {
        tempVec = right_bound.transform.position;
        tempVec.x = building_pool[building_pool.Count - 1].transform.position.x;
        tempVec.z = (building_pool[building_pool.Count - 1].transform.position.z + building_pool[0].transform.position.z) / 2;
        right_bound.transform.position = tempVec;

        tempVec = left_bound.transform.position;
        tempVec.x = building_pool[0].transform.position.x;
        tempVec.z = (building_pool[building_pool.Count - 1].transform.position.z + building_pool[0].transform.position.z) / 2;
        left_bound.transform.position = tempVec;

        tempVec = up_bound.transform.position;
        tempVec.z = building_pool[building_pool.Count - 1].transform.position.z;
        tempVec.x = (building_pool[building_pool.Count - 1].transform.position.x + building_pool[0].transform.position.x) / 2;
        up_bound.transform.position = tempVec;

        tempVec = lower_bound.transform.position;
        tempVec.z = building_pool[0].transform.position.z;
        tempVec.x = (building_pool[building_pool.Count - 1].transform.position.x + building_pool[0].transform.position.x) / 2;
        lower_bound.transform.position = tempVec;

        tempVec = right_bound.transform.localScale;
        tempVec.z = up_bound.transform.position.z - lower_bound.transform.position.z;
        right_bound.transform.localScale = tempVec;

        tempVec = left_bound.transform.localScale;
        tempVec.z = up_bound.transform.position.z - lower_bound.transform.position.z;
        left_bound.transform.localScale = tempVec;

        tempVec = up_bound.transform.localScale;
        tempVec.x = right_bound.transform.position.x - left_bound.transform.position.x;
        up_bound.transform.localScale = tempVec;

        tempVec = lower_bound.transform.localScale;
        tempVec.x = right_bound.transform.position.x - left_bound.transform.position.x;
        lower_bound.transform.localScale = tempVec;
    }

    void setPlayerRandomPos()
    {
        int avenue = Random.Range(0, vertical_roads.Count - 1);
        int street = Random.Range(0, horizontal_roads.Count - 1);
        tempVec.x = (vertical_roads[avenue].upper + vertical_roads[avenue].lower) / 2;
        tempVec.z = (horizontal_roads[street].upper + horizontal_roads[street].lower) / 2;
        spawnLocation = tempVec;
        Game_watcher.setPlayerPos(tempVec);
    }

    void setDestinationRandomPos()
    {
        int avenue = Random.Range(0, vertical_roads.Count - 1);
        int street = Random.Range(0, horizontal_roads.Count - 1);
        //Choose building
        int face = Random.Range(0, 3);//up0,right1,bottom2,left3
        int building_index = avenue * buildings[0].Count + street;
        building_index = Mathf.Clamp(building_index, 0, building_pool.Count - 1);
        GameObject exitBuilding = building_pool[building_index];
        if (avenue == 0)
        {
            if (street == 0)//Bottom Left Corner, move right & face UP
            {
                building_index++;
                face = 0;
            }
            else if (street == horizontal_roads.Count)//Top left, move right & face DOWN
            {
                building_index++;
                face = 2;
            }
            else//Left, face RIGHT
            {
                face = 1;
            }
        }
        else if (avenue == vertical_roads.Count)
        {
            if (street == 0)//Bottom right Corner, move left & face UP
            {
                building_index--;
                face = 0;
            }
            else if (street == horizontal_roads.Count)//Top Right, move left & face DOWN
            {
                building_index--;
                face = 2;
            }
            else//Right, face LEFT
            {
                face = 3;
            }
        }
        else
        {
            if (street == 0)//Bottom, face UP
            {
                face = 0;
            }
            else if (street == horizontal_roads.Count)//Top, face DOWN
            {
                face = 2;
            }
        }

        //Position and scale exit
        tempVec = exitBuilding.transform.position;
        tempVec.y = 2.5f;
        if (face == 0)//up
        {
            tempVec.z += exitBuilding.transform.localScale.z / 2;
            exit_object.transform.position = tempVec;
            tempVec.z = exit_trig_width;
            tempVec.x = exitBuilding.transform.localScale.x / 2;
            exit_object.transform.localScale = tempVec;
            exit_object.setEntranceDir(CONSTANTS.DIRECTION.UP);
        }
        else if (face == 2)//down
        {
            tempVec.z -= exitBuilding.transform.localScale.z / 2;
            exit_object.transform.position = tempVec;
            tempVec.z = exit_trig_width;
            tempVec.x = exitBuilding.transform.localScale.x / 2;
            exit_object.transform.localScale = tempVec;
            exit_object.setEntranceDir(CONSTANTS.DIRECTION.DOWN);
        }
        else if (face == 1)//right
        {
            tempVec.x += exitBuilding.transform.localScale.x / 2;
            exit_object.transform.position = tempVec;
            tempVec.x = exit_trig_width;
            tempVec.z = exitBuilding.transform.localScale.z / 2;
            exit_object.transform.localScale = tempVec;
            exit_object.setEntranceDir(CONSTANTS.DIRECTION.RIGHT);
        }
        else if (face == 3)//left
        {
            tempVec.x -= exitBuilding.transform.localScale.x / 2;
            exit_object.transform.position = tempVec;
            tempVec.x = exit_trig_width;
            tempVec.z = exitBuilding.transform.localScale.z / 2;
            exit_object.transform.localScale = tempVec;
            exit_object.setEntranceDir(CONSTANTS.DIRECTION.LEFT);
        }

        destinLocation = exit_object.transform.position;
    }


    public static float getDistanceTravelled()
    {
        return Vector3.Distance(spawnLocation, destinLocation);
    }
}
