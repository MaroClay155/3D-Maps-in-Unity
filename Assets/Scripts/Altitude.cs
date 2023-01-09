using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Altitude : MonoBehaviour
{
    public Text AltitudeValue;
    private  float altitude;
    // Start is called before the first frame update
    void Start()
    {
        altitude = GetComponent<Transform>().position.y;
    }

    // Update is called once per frame
    void Update()
    {
        //print(altitude);
        altitude = GetComponent<Transform>().position.y;
        AltitudeValue.text = altitude.ToString();
    }
}
