using System;
using UnityEngine;
using System.Collections.Generic;


class MeshItem
{
    public List<Vector3> points { get; private set; }

    public MeshItem()
    {
        points = new List<Vector3>();
    }

    public void AddPoint(Vector3 point)
    {
        points.Add(point);
    }

    public void Clear()
    {
        points.Clear();
    }
}
