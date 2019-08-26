using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawRects : MonoBehaviour {

	// Use this for initialization
	void Start () 
  {
    rects = new List<Rect>();	
	}
	
	// Update is called once per frame
	void Update () 
  {
    if (newRects)
    {
      NewWay();
    }
	}

  Vector3[] vertices;
  Vector2[] uv;
  int[] triangles;

  Mesh mesh;
  float score = 0.0f;

  private void NewWay()
  {
    MWRDebug.Log("New rects");

 

    
    List<Rect> sourceRects;
    float z = 0.03f;



    int verticesOffset;
    int uvOffset;
    int triangleOffset;

    if ((newRectList != null) && true)
    {
      // OK, adding entries
      //mesh =  GetComponent<MeshFilter>().mesh;

      //vertices = mesh.vertices;
      //uv = mesh.uv;
      //triangles = mesh.triangles;

      verticesOffset = vertices.Length;
      uvOffset = uv.Length;
      triangleOffset = triangles.Length;

      System.Array.Resize(ref vertices, vertices.Length + newRectList.Count * 4);
      System.Array.Resize(ref uv, uv.Length + newRectList.Count * 4);
      System.Array.Resize(ref triangles, triangles.Length + newRectList.Count * 6);

      //verticesOffset = vertices.Length;
      //uvOffset = uv.Length;
      //triangleOffset = triangles.Length;

      //vertices = new Vector3[newRectList.Count * 4];
      //uv = new Vector2[newRectList.Count * 4];
      //triangles = new int[newRectList.Count * 6];



      sourceRects = newRectList;
    }
    else
    {
      // OK, completely new list

      mesh = new Mesh();

      vertices = new Vector3[rects.Count * 4];
      uv = new Vector2[rects.Count * 4];
      triangles = new int[rects.Count * 6];

      verticesOffset = 0;
      uvOffset = 0;
      triangleOffset = 0;

      sourceRects = rects;

      score = 0.0f;
    }
    
    Rect rect;

    for (int ii = 0; ii < sourceRects.Count; ii++)
    {
      rect = sourceRects[ii];

      vertices[verticesOffset + ii * 4 + 0] = new Vector3(rect.xMax, rect.yMax, z);
      vertices[verticesOffset + ii * 4 + 1] = new Vector3(rect.xMax, rect.yMin, z);
      vertices[verticesOffset + ii * 4 + 2] = new Vector3(rect.xMin, rect.yMax, z);
      vertices[verticesOffset + ii * 4 + 3] = new Vector3(rect.xMin, rect.yMin, z);

      uv[uvOffset + ii * 4 + 0] = new Vector2(1, 1);
      uv[uvOffset + ii * 4 + 1] = new Vector2(1, 0);
      uv[uvOffset + ii * 4 + 2] = new Vector2(0, 1);
      uv[uvOffset + ii * 4 + 3] = new Vector2(0, 0);

      triangles[triangleOffset + ii * 6 + 0] = (verticesOffset + 4 * ii + 0);
      triangles[triangleOffset + ii * 6 + 1] = (verticesOffset + 4 * ii + 1);
      triangles[triangleOffset + ii * 6 + 2] = (verticesOffset + 4 * ii + 2);
      triangles[triangleOffset + ii * 6 + 3] = (verticesOffset + 4 * ii + 2);
      triangles[triangleOffset + ii * 6 + 4] = (verticesOffset + 4 * ii + 1);
      triangles[triangleOffset + ii * 6 + 5] = (verticesOffset + 4 * ii + 3);

      score += (rect.width * rect.height);
    }

    mesh.vertices = vertices; 
    mesh.uv = uv;
    mesh.triangles = triangles;
    mesh.RecalculateNormals();

    ScoreManager.Score = score;

    GetComponent<MeshFilter>().mesh = mesh;

    newRects = false;
  }

  private void OldWay()
  {
    MWRDebug.Log("New rects");

    float score = 0.0f;

    Mesh mesh = new Mesh();
    float z = 0.03f;

    List<Vector3> verticesList = new List<Vector3>();
    List<Vector2> uvList = new List<Vector2>();
    List<int> triangleList = new List<int>();

    int rectCount = 0;

    foreach (var rect in rects)
    {
      MWRDebug.Log("Rect + " + rectCount);
      MWRDebug.Log("Rect:" + " " + rect.xMin + " " + rect.xMax + " " + rect.yMin + " " + rect.yMax);

      verticesList.Add(new Vector3(rect.xMax, rect.yMax, z));
      verticesList.Add(new Vector3(rect.xMax, rect.yMin, z));
      verticesList.Add(new Vector3(rect.xMin, rect.yMax, z));
      verticesList.Add(new Vector3(rect.xMin, rect.yMin, z));

      uvList.Add(new Vector2(1, 1));
      uvList.Add(new Vector2(1, 0));
      uvList.Add(new Vector2(0, 1));
      uvList.Add(new Vector2(0, 0));

      triangleList.Add(4 * rectCount + 0);
      triangleList.Add(4 * rectCount + 1);
      triangleList.Add(4 * rectCount + 2);
      triangleList.Add(4 * rectCount + 2);
      triangleList.Add(4 * rectCount + 1);
      triangleList.Add(4 * rectCount + 3);

      rectCount++;
      score += (rect.width * rect.height);
    }

    ScoreManager.Score = score;

    Vector3[] vertices = verticesList.ToArray();
    Vector2[] uv = uvList.ToArray();
    int[] triangles = triangleList.ToArray();

    mesh.vertices = vertices;
    mesh.uv = uv;
    mesh.triangles = triangles;
    mesh.RecalculateNormals();

    MWRDebug.Log("V" + mesh.vertexCount + "," + mesh.vertices.Length);
    MWRDebug.Log("U" + mesh.uv.Length);
    MWRDebug.Log("T" + mesh.triangles.Length);

    GetComponent<MeshFilter>().mesh = mesh;

    newRects = false;
    newRectList = null;
  }

  static List<Rect> rects;
  static List<Rect> newRectList;
  static bool newRects = true;

  public static void SetRects(List<Rect> rects)
  {
    DrawRects.rects = rects;
    newRects = true;
  }

  public static void AddRects(List<Rect> rects)
  {
    DrawRects.rects.AddRange(rects);
    DrawRects.newRectList = rects;
    newRects = true;
  }

  public static Rect hitRect;

  public static bool InRects(Vector3 point)
  {
    bool rc = false;

    foreach (Rect rect in rects)
    {
      if (rect.Contains(point))
      {
        hitRect = rect;
        rc = true;
        break;
      }
    }

    return rc;
  }
}

