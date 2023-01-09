﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Road infrastructure maker.
/// </summary>
class RoadMaker : InfrastructureBehaviour
{
    public Material roadMaterial;

    /// <summary>
    /// Create the roads.
    /// </summary>
    /// <returns></returns>
    IEnumerator Start()
    {
        // Wait for the map to become ready
        while (!map.IsReady)
        {
            yield return null;
        }

        // Iterate through the roads and build each one
        foreach (var way in map.ways.FindAll((w) => { return w.IsRoad; }))
        {
            CreateObject(way, roadMaterial, way.Name);
            yield return null;
        }
    }

    protected override void OnObjectCreated(OsmWay way, Vector3 origin, List<Vector3> vectors, List<Vector3> normals, List<Vector2> uvs, List<int> indices)
    {
        for (int i = 1; i < way.NodeIDs.Count; i++)
        {
            OsmNode p1 = map.nodes[way.NodeIDs[i - 1]];
            OsmNode p2 = map.nodes[way.NodeIDs[i]];

            Vector3 s1 = p1 - origin;
            Vector3 s2 = p2 - origin;

            Vector3 diff = (s2 - s1).normalized;

            // https://en.wikipedia.org/wiki/Lane
            // According to the article, it's 3.7m in Canada
            var cross = Vector3.Cross(diff, Vector3.up) * 3.7f * way.Lanes;

            // Create points that represent the width of the road
            Vector3 v1 = s1 + cross;
            Vector3 v2 = s1 - cross;
            Vector3 v3 = s2 + cross;
            Vector3 v4 = s2 - cross;

            vectors.Add(v1);
            vectors.Add(v2);
            vectors.Add(v3);
            vectors.Add(v4);

            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(1, 1));

            normals.Add(Vector3.up);
            normals.Add(Vector3.up);
            normals.Add(Vector3.up);
            normals.Add(Vector3.up);

            int idx1, idx2, idx3, idx4;
            idx4 = vectors.Count - 1;
            idx3 = vectors.Count - 2;
            idx2 = vectors.Count - 3;
            idx1 = vectors.Count - 4;

            // first triangle v1, v3, v2
            indices.Add(idx1);
            indices.Add(idx3);
            indices.Add(idx2);

            // second         v3, v4, v2
            indices.Add(idx3);
            indices.Add(idx4);
            indices.Add(idx2);
        }
    }
    
}
