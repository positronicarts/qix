using UnityEngine;
using System.Collections;

public class Line
{
  public Vector3 start;
  public Vector3 end;

  public Line(Vector3 start, Vector3 end)
  {
    this.start = start;
    this.end = end;
  }

  public bool ContainsUnbound(Vector3 point)
  {
    bool rc;

    Vector3 v1 = point - start;
    Vector3 v2 = point - end;

    rc = (Vector3.Cross(v1, v2).magnitude == 0.0f);

    //MWRDebug.Log("ContainsUnbound: " + rc);

    return rc;
  }

  public bool ContainsBound(Vector3 point)
  {
    bool rc = (ContainsUnbound(point) &&
               ((start - point).magnitude <= (start - end).magnitude) &&
               ((end - point).magnitude <= (start - end).magnitude));

    //MWRDebug.Log("ContainsBound: " + rc);

    return rc;
  }
}

