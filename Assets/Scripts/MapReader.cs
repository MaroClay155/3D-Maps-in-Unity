using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

class MapReader : MonoBehaviour
{
    [HideInInspector]
    public Dictionary<ulong, OsmNode> nodes;

    [HideInInspector]
    public List<OsmWay> ways;
    
    [HideInInspector]
    public OsmBounds bounds;

    public GameObject groundPlane;

    //[Tooltip("The resource file that contains the OSM map data")]////////////old
    //public string resourceFile;////////////old
    /// <marwan>
    [SerializeField]
    private LocationHandler location;

    private string resourceFile;
    /// </marwan>

    public bool IsReady { get; private set; }

	// Use this for initialization
	void Start ()
    {
        //added by marwan
        resourceFile = location.LocationString;
        Debug.Log("OSM Map of " + resourceFile);
        ReloadMap(location.LocationString);
        ////
    }

    void Update()
    {
        ////added by marwan
        if (resourceFile != location.LocationString)
        {
            Debug.Log("Old Map: " + resourceFile + ", new OSM Map: " + location.LocationString);
            ReloadMap(location.LocationString);
            resourceFile = location.LocationString;
        }
        ///
        foreach (OsmWay w in ways)
        {
            if (w.Visible)
            {
                Color c = Color.cyan;               // cyan for buildings
                if (!w.IsBoundary) c = Color.red; // red for roads

                for (int i = 1; i < w.NodeIDs.Count; i++)
                {
                    OsmNode p1 = nodes[w.NodeIDs[i - 1]];
                    OsmNode p2 = nodes[w.NodeIDs[i]];

                    Vector3 v1 = p1 - bounds.Centre;
                    Vector3 v2 = p2 - bounds.Centre;

                    Debug.DrawLine(v1, v2, c);                   
                }
            }
        }
    }

    void GetWays(XmlNodeList xmlNodeList)
    {
        foreach (XmlNode node in xmlNodeList)
        {
            OsmWay way = new OsmWay(node);
            ways.Add(way);
        }
    }

    void GetNodes(XmlNodeList xmlNodeList)
    {
        foreach (XmlNode n in xmlNodeList)
        {
            OsmNode node = new OsmNode(n);
            nodes[node.ID] = node;
        }
    }

    void SetBounds(XmlNode xmlNode)
    {
        bounds = new OsmBounds(xmlNode);
    }

    void ReloadMap(string res)
    {
        Debug.Log("reloading map of: " + res);
        nodes = new Dictionary<ulong, OsmNode>();
        ways = new List<OsmWay>();
        var txtAsset = Resources.Load<TextAsset>(res);
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(txtAsset.text);

        SetBounds(doc.SelectSingleNode("/osm/bounds"));
        GetNodes(doc.SelectNodes("/osm/node"));
        GetWays(doc.SelectNodes("/osm/way"));

        float minx = (float)MercatorProjection.lonToX(bounds.MinLon);
        float maxx = (float)MercatorProjection.lonToX(bounds.MaxLon);
        float miny = (float)MercatorProjection.latToY(bounds.MinLat);
        float maxy = (float)MercatorProjection.latToY(bounds.MaxLat);

        groundPlane.transform.localScale = new Vector3((maxx - minx) / 2, 1, (maxy - miny) / 2);

        IsReady = true;
    }
}
