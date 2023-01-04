using System.Collections.Generic;
using System.Xml;


 
/// An OSM object that describes an arrangement of OsmNodes into a shape or road.
 
class OsmWay : BaseOsm
{
    /// Way ID.
    public ulong ID { get; private set; }

    /// True if visible.
    public bool Visible { get; private set; }

    /// List of node IDs.
    public List<ulong> NodeIDs { get; private set; }

    /// True if the way is a boundary.
    public bool IsBoundary { get; private set; }

    /// True if the way is a building.
    public bool IsBuilding { get; private set; }

    /// True if the way is a road.
    public bool IsRoad { get; private set; }

    /// Height of the structure.
    public float Height { get; private set; }

    /// The name of the object.
    public string Name { get; private set; }

    /// The number of lanes on the road. Default is 1 for contra-flow
    public int Lanes { get; private set; }

    /// Constructor.
    /// <param name="node"></param>
    public OsmWay(XmlNode node)
    {
        NodeIDs = new List<ulong>();
        Height = 3.0f; // Default height for structures is 1 story (approx. 3m)
        Lanes = 1;      // Number of lanes either side of the divide 
        Name = "";

        // Get the data from the attributes
        ID = GetAttribute<ulong>("id", node.Attributes);
        Visible = GetAttribute<bool>("visible", node.Attributes);

        // Get the nodes
        XmlNodeList nds = node.SelectNodes("nd");
        foreach(XmlNode n in nds)
        {
            ulong refNo = GetAttribute<ulong>("ref", n.Attributes);
            NodeIDs.Add(refNo);
        }

        if (NodeIDs.Count > 1)
        {
            IsBoundary = NodeIDs[0] == NodeIDs[NodeIDs.Count - 1];
        }

        // Read the tags
        XmlNodeList tags = node.SelectNodes("tag");
        foreach (XmlNode t in tags)
        {
            string key = GetAttribute<string>("k", t.Attributes);
            if (key == "building:levels")
            {
                Height = 3.0f * GetAttribute<float>("v", t.Attributes);
            }
            else if (key == "height")
            {
                Height = 0.3048f * GetAttribute<float>("v", t.Attributes);
            }
            else if (key == "building")
            {
                IsBuilding = true; // GetAttribute<string>("v", t.Attributes) == "yes";
            }
            else if (key == "highway")
            {
                IsRoad = true;
            }
            else if (key=="lanes")
            {
                Lanes = GetAttribute<int>("v", t.Attributes);
            }
            else if (key=="name")
            {
                Name = GetAttribute<string>("v", t.Attributes);
            }
        }
    }
}

