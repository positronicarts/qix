using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DynamicLines
{
  public static Mesh GetMesh(Line line, float w)
  {
    return GetMesh(new Line[] { line }, w);
  }

  public static Mesh GetMesh(Line[] lines, float w)
  {
    return GetMesh(lines, w, Vector3.zero);
  }

  public static Mesh GetMesh(Line[] lines, float w, Vector3 delta)
  {
    //MWRDebug.Log("Lines: " + lines.Length + " to render");
    Mesh mesh = new Mesh();

    Vector3 start, end;
    int lineCounter = 0;

    var vertices = new Vector3[lines.Length * 4];
    var uv = new Vector2[lines.Length * 4];
    var triangles = new int[lines.Length * 6];

    for (int ii = 0; ii < lines.Length; ii++)
    {
      var line = lines[ii];

      start = line.start;
      end = line.end;

      Vector3 dir = (end - start);
      Vector3 norm = GetNormal(ref dir);

      Vector3 corn1 = (dir.normalized * w + norm.normalized * w);
      Vector3 corn2 = (dir.normalized * w - norm.normalized * w);

      vertices[ii * 4 + 0] = (end + corn1);
      vertices[ii * 4 + 1] = (end + corn2);
      vertices[ii * 4 + 2] = (start - corn2);
      vertices[ii * 4 + 3] = (start - corn1);

      uv[ii * 4 + 0] = new Vector2(1, 1);
      uv[ii * 4 + 1] = new Vector2(1, 0);
      uv[ii * 4 + 2] = new Vector2(0, 1);
      uv[ii * 4 + 3] = new Vector2(0, 0);

      triangles[ii * 6 + 0] = (4 * lineCounter + 0);
      triangles[ii * 6 + 1] = (4 * lineCounter + 1);
      triangles[ii * 6 + 2] = (4 * lineCounter + 2);
      triangles[ii * 6 + 3] = (4 * lineCounter + 2);
      triangles[ii * 6 + 4] = (4 * lineCounter + 1);
      triangles[ii * 6 + 5] = (4 * lineCounter + 3);

      lineCounter++;
    }

    mesh.vertices = vertices;
    mesh.uv = uv;
    mesh.triangles = triangles;
    mesh.RecalculateNormals();
    
    return mesh;
  }

  public static Vector3 GetNormal(ref Vector3 dir)
  {
    Vector3 norm = new Vector3(-dir.y, dir.x, 0.0f);
    return norm;
  }
}
