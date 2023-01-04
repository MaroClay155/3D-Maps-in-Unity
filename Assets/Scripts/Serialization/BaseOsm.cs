using System;
using System.Xml;



/// Base Open Street Map (OSM) data node.
class BaseOsm
{
    /// <summary>
    /// Get an attribute's value from the collection using the given 'attrName'. 
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    /// <param name="attrName">Name of the attribute</param>
    /// <param name="attributes">Node's attribute collection</param>
    /// <returns>The value of the attribute converted to the required type</returns>
    protected T GetAttribute<T>(string attrName, XmlAttributeCollection attributes)
    {
        // TODO: We are going to assume 'attrName' exists in the collection
        string strValue = attributes[attrName].Value;
        return (T)Convert.ChangeType(strValue, typeof(T));
    }
}

