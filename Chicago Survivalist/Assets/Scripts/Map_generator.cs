using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_generator : MonoBehaviour {
    public static Map_generator singleton;
    //
    public GameObject player_object;
    public Destination_trigger exit_object;
    public GameObject right_bound;
    public GameObject left_bound;
    public GameObject up_bound;
    public GameObject lower_bound;

    public static float map_size = 150;
    static Vector2 bottomleft = -Vector2.one * map_size;
    static Vector2 topright = Vector2.one * map_size;
    static float sidewalk_height = 0.1f;
    static float exit_trig_width = 1f;

    public float road_w_min = 2;
    public float road_w_max = 6;
    public float building_w_min = 6;
    public float building_w_max = 15;
    public float building_h_min = 15;
    public float building_h_max = 50;
    public float building_w_contract_limit_min = 0.1f;//side walk
    public float building_w_contract_limit_max = 0.3f;

    public GameObject[] building_template;
    public GameObject sidewalk_template;

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
    List<t1_road> horizontal_roads = new List<t1_road>();
    List<t1_road> vertical_roads = new List<t1_road>();
    List<List<t2_building>> buildings = new List<List<t2_building>>();
    //Pool objects
    public List<GameObject> building_pool = new List<GameObject>();
    List<GameObject> sidewalk_pool = new List<GameObject>();

    // Use this for initialization
    void Start () {
        singleton = this;
        map_generate();
        
    }
	// Update is called once per frame
	void Update () {
		//
	}

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
            
            //Debug.Log("building l: " + xboundl+ " vs r: "+ xboundr);
            //Debug.Log("building width: " + (xboundr - xboundl));
            //
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
        //Build cubemap for reflective buildings
        for (int i = 0; i < building_pool.Count; i++)
        {
            One_time_cubemap cubemap_script = building_pool[i].GetComponent<One_time_cubemap>();
            if (cubemap_script != null)
            {
                cubemap_script.Render();
            }
        }

        //Place player=================================================================
        int avenue = Random.Range(0, vertical_roads.Count - 1);
        int street = Random.Range(0, horizontal_roads.Count - 1);
        float playerx = (vertical_roads[avenue].upper + vertical_roads[avenue].lower)/2;
        float playery = (horizontal_roads[street].upper + horizontal_roads[street].lower) / 2;
        player_object.transform.position = new Vector3(playerx,3,playery);

        //Place bounds=================================================================
        Vector3 bound_vec = right_bound.transform.position;
        bound_vec.x = building_pool[building_pool.Count-1].transform.position.x;
        bound_vec.z = (building_pool[building_pool.Count - 1].transform.position.z + building_pool[0].transform.position.z)/2;
        right_bound.transform.position = bound_vec;

        bound_vec = left_bound.transform.position;
        bound_vec.x = building_pool[0].transform.position.x;
        bound_vec.z = (building_pool[building_pool.Count - 1].transform.position.z + building_pool[0].transform.position.z) / 2;
        left_bound.transform.position = bound_vec;

        bound_vec = up_bound.transform.position;
        bound_vec.z = building_pool[building_pool.Count-1].transform.position.z;
        bound_vec.x = (building_pool[building_pool.Count - 1].transform.position.x + building_pool[0].transform.position.x) / 2;
        up_bound.transform.position = bound_vec;

        bound_vec = lower_bound.transform.position;
        bound_vec.z = building_pool[0].transform.position.z;
        bound_vec.x = (building_pool[building_pool.Count - 1].transform.position.x + building_pool[0].transform.position.x) / 2;
        lower_bound.transform.position = bound_vec;

        bound_vec = right_bound.transform.localScale;
        bound_vec.z = up_bound.transform.position.z - lower_bound.transform.position.z;
        right_bound.transform.localScale = bound_vec;

        bound_vec = left_bound.transform.localScale;
        bound_vec.z = up_bound.transform.position.z - lower_bound.transform.position.z;
        left_bound.transform.localScale = bound_vec;

        bound_vec = up_bound.transform.localScale;
        bound_vec.x = right_bound.transform.position.x - left_bound.transform.position.x;
        up_bound.transform.localScale = bound_vec;

        bound_vec = lower_bound.transform.localScale;
        bound_vec.x = right_bound.transform.position.x - left_bound.transform.position.x;
        lower_bound.transform.localScale = bound_vec;


        //Place exit=================================================================
        int min_distance = 2;
        int ran_avenue = Mathf.Abs(vertical_roads.Count - 1 - (avenue + min_distance)) + Mathf.Abs(avenue - min_distance);
        int exit_avenue = Random.Range(0, ran_avenue);
        if(exit_avenue >= Mathf.Abs(avenue - min_distance))
        {
            exit_avenue = exit_avenue + 1 - Mathf.Abs(avenue - min_distance) + avenue + min_distance;
        }
        else// <= 1
        {
            exit_avenue = avenue - min_distance - 1;
        }
        int ran_street = Mathf.Abs(horizontal_roads.Count - 1 - (street + min_distance)) + Mathf.Abs(street - min_distance);
        int exit_street = Random.Range(0, ran_street);
        if (exit_street > Mathf.Abs(street - min_distance))
        {
            exit_street = exit_street + 1 - Mathf.Abs(street - min_distance) + street + min_distance;
        }
        else
        {
            exit_street = street - min_distance - 1;
        }
        //Calculate entrance direction
        int face = Random.Range(0,3);//up0,right1,bottom2,left3
        int building_index = exit_avenue * buildings[0].Count + exit_street;
        building_index = Mathf.Clamp(building_index, 0, building_pool.Count - 1);
        GameObject building = building_pool[building_index];
        if (exit_avenue == 0)
        {
            if(exit_street == 0)//Bottom Left Corner, move right & face UP
            {
                building_index++;
                face = 0;
            }
            else if(exit_street == horizontal_roads.Count)//Top left, move right & face DOWN
            {
                building_index++;
                face = 2;
            }
            else//Left, face RIGHT
            {
                face = 1;
            }
        }
        else if(exit_avenue == vertical_roads.Count)
        {
            if (exit_street == 0)//Bottom right Corner, move left & face UP
            {
                building_index--;
                face = 0;
            }
            else if (exit_street == horizontal_roads.Count)//Top Right, move left & face DOWN
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
            if (exit_street == 0)//Bottom, face UP
            {
                face = 0;
            }
            else if (exit_street == horizontal_roads.Count)//Top, face DOWN
            {
                face = 2;
            }
        }

        //Position and scale exit
        Vector3 temp_vec = building.transform.position;
        temp_vec.y = 2.5f;
        if (face == 0)//up
        {
            temp_vec.z += building.transform.localScale.z / 2;
            exit_object.transform.position = temp_vec;
            temp_vec.z = exit_trig_width;
            temp_vec.x = building.transform.localScale.x / 2;
            exit_object.transform.localScale = temp_vec;
            exit_object.set_entrance_dir(CONSTANTS.DIRECTION.UP);
        }
        else if (face == 2)//down
        {
            temp_vec.z -= building.transform.localScale.z / 2;
            exit_object.transform.position = temp_vec;
            temp_vec.z = exit_trig_width;
            temp_vec.x = building.transform.localScale.x / 2;
            exit_object.transform.localScale = temp_vec;
            exit_object.set_entrance_dir(CONSTANTS.DIRECTION.DOWN);
        }
        else if(face == 1)//right
        {
            temp_vec.x += building.transform.localScale.x / 2;
            exit_object.transform.position = temp_vec;
            temp_vec.x = exit_trig_width;
            temp_vec.z = building.transform.localScale.z / 2;
            exit_object.transform.localScale = temp_vec;
            exit_object.set_entrance_dir(CONSTANTS.DIRECTION.RIGHT);
        }
        else if (face == 3)//left
        {
            temp_vec.x -= building.transform.localScale.x / 2;
            exit_object.transform.position = temp_vec;
            temp_vec.x = exit_trig_width;
            temp_vec.z = building.transform.localScale.z / 2;
            exit_object.transform.localScale = temp_vec;
            exit_object.set_entrance_dir(CONSTANTS.DIRECTION.LEFT);
        }
        exit_object.levelPass = true;
        //Game_watcher.singleton.set_start_end(player_object.transform.position, exit_object.transform.position);

    }


}
