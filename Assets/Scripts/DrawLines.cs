using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DrawLines : MonoBehaviour {

	// Use this for initialization
	void Start () 
  {
    lines = new List<Line>();
	}
	
	// Update is called once per frame
	void Update () 
  {
    if (newLines)
    {
      Mesh mesh = new Mesh();
      float w = 0.01f;

      List<Vector3> verticesList = new List<Vector3>();
      List<Vector2> uvList = new List<Vector2>();
      List<int> trianleList = new List<int>();

      Vector3 start, end;
      int lineCounter = 0;

      foreach (var line in lines)
      {
        start = line.start;
        end = line.end;

        if (start.x < end.x)
        {
          //MWRDebug.Log("xxx1:" + lineCounter);
          verticesList.Add(end + new Vector3(w, w, 0.0f));
          verticesList.Add(end + new Vector3(w, -w, 0.0f));
          verticesList.Add(start + new Vector3(-w, w, 0.0f));
          verticesList.Add(start + new Vector3(-w, -w, 0.0f));
        }
        else if (start.x > end.x)
        {
          //MWRDebug.Log("xxx2:" + lineCounter);
          verticesList.Add(start + new Vector3(w, w, 0.0f));
          verticesList.Add(start + new Vector3(w, -w, 0.0f));
          verticesList.Add(end + new Vector3(-w, w, 0.0f));
          verticesList.Add(end + new Vector3(-w, -w, 0.0f));
        }
        else if (start.y < end.y)
        {
          //MWRDebug.Log("xxx3:" + lineCounter);
          verticesList.Add(end + new Vector3(w, w, 0.0f));
          verticesList.Add(start + new Vector3(w, -w, 0.0f));
          verticesList.Add(end + new Vector3(-w, w, 0.0f));
          verticesList.Add(start + new Vector3(-w, -w, 0.0f));
        }
        else
        {
          //MWRDebug.Log("xxx4:" + lineCounter);
          verticesList.Add(start + new Vector3(w, w, 0.0f));
          verticesList.Add(end + new Vector3(w, -w, 0.0f));
          verticesList.Add(start + new Vector3(-w, w, 0.0f));
          verticesList.Add(end + new Vector3(-w, -w, 0.0f));
        }

        uvList.Add(new Vector2(1, 1));
        uvList.Add(new Vector2(1, 0));
        uvList.Add(new Vector2(0, 1));
        uvList.Add(new Vector2(0, 0));

        trianleList.Add(4 * lineCounter + 0);
        trianleList.Add(4 * lineCounter + 1);
        trianleList.Add(4 * lineCounter + 2);
        trianleList.Add(4 * lineCounter + 2);
        trianleList.Add(4 * lineCounter + 1);
        trianleList.Add(4 * lineCounter + 3);

        lineCounter++;
      }

      Vector3[] vertices = verticesList.ToArray();
      Vector2[] uv = uvList.ToArray();
      int[] triangles = trianleList.ToArray();

      mesh.vertices = vertices;
      mesh.uv = uv;
      mesh.triangles = triangles;
      mesh.RecalculateNormals();

      MWRDebug.Log("V" + mesh.vertexCount + "," + mesh.vertices.Length);
      MWRDebug.Log("U" + mesh.uv.Length);
      MWRDebug.Log("T" + mesh.triangles.Length);

      GetComponent<MeshFilter>().mesh = mesh;

      newLines = false;
    }
	}

  static List<Line> lines;
  static bool newLines = true;

  public static void SetLines(List<Line> lines)
  {
    DrawLines.lines = lines;
    newLines = true;
  }
}
