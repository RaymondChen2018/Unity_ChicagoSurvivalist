using UnityEngine;

public class Weather : MonoBehaviour {
    private static float temperature = 20;
    public const float WIND_INFLUENCE_OFFSET = -10;
    const float MAX_RATE_TEMPERATURE = 5;
    const float MAX_RATE_CHANGE_TEMPERATURE = 5;
    const float MIN_RATE_CHANGE_TEMPERATURE = 1;
    const float MAX_TEMPERATURE = 30;
    const float MIN_TEMPERATURE = -40;
    float time_to_change_termperature = 0;
    // Use this for initialization
    void Start () {
        time_to_change_termperature = Random.Range(MIN_RATE_CHANGE_TEMPERATURE, MAX_RATE_CHANGE_TEMPERATURE);
    }
	
	// Update is called once per frame
	void Update () {
        //Temperature vibrates
		if(Time.time > time_to_change_termperature)
        {
            temperature = Mathf.Clamp(temperature + Random.Range(-MAX_RATE_TEMPERATURE, MAX_RATE_TEMPERATURE), MIN_TEMPERATURE, MAX_TEMPERATURE);
            time_to_change_termperature = Time.time + Random.Range(MIN_RATE_CHANGE_TEMPERATURE, MAX_RATE_CHANGE_TEMPERATURE);
        }
	}
    public void debug_setTemperature(float _temperature)
    {
        setTemperature(_temperature);
    }
    public static float getTemperature()
    {
        return temperature;
    }
    static void setTemperature(float _temperature)
    {
        temperature = _temperature;
    }
}
