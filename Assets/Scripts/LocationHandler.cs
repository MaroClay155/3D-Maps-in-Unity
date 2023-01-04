using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationHandler : MonoBehaviour
{
    //public Text LocationText;///old
    public string LocationString = "elqala";
    // Start is called before the first frame update
    void Start()
    {

        var location = transform.GetComponent<Dropdown>();

        //location.options.Clear();

        //List<string> items = new List<string>();
        //items.Add("paris");
        //items.Add("ny");
        //items.Add("toronto");
        //items.Add("mountflorida");

        //foreach (var item in items)
        //{
        //    location.options.Add(new Dropdown.OptionData() { text = item });
        //}
        LocationsItemSelected(location);
        location.onValueChanged.AddListener(delegate { LocationsItemSelected(location); });

    }

    void LocationsItemSelected(Dropdown location)
    {
        //int index = location.value;
        //LocationText.text = location.options[index].text;///old
        Debug.Log("You are currently viewing OSM of "+location.captionText.text.ToString());
        LocationString = location.captionText.text.ToString();
        
    }

}



