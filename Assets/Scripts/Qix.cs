using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Qix : MonoBehaviour 
{
  Vector3 center;
  Vector3 movement;
  Vector3 norm;
  float len;

  Vector3 start;
  Vector3 end;

  float speed;

  float changeTime = 1.0f;
  float timeSinceLastChange;
  float maxAngle = Mathf.PI / 4.0f;

	// Use this for initialization
	void Start () 
  {
    center = new Vector3(0.0f, 0.0f, 0.0f);
    movement = GetRandomUnitVector(out angle);
    timeSinceLastChange = 0.0f;
    timeSinceLastEnque = 0.0f;
    len = 0.2f;
    speed = 0.8f;

    lineQueue = new Queue<Line>();

    position = Vector3.zero;
	}

  float angle;

  public static Vector3 GetRandomUnitVector(out float angle)
  {
    angle = Random.Range(0.0f, Mathf.PI * 2.0f);

    Vector3 rc = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0.0f);

    return rc;
  }

  public static Vector3 GetRandomUnitVector(ref float angle, float withinAngle)
  {
    angle = Random.Range(angle - withinAngle, angle + withinAngle);

    Vector3 rc = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0.0f);

    return rc;
  }

  Queue<Line> lineQueue;
  int maxQueueLength = 6;
  float enquePeriod = 0.1f;
  float timeSinceLastEnque;

  public Vector3 position { get; private set; }

	// Update is called once per frame
	void Update () 
  {
    timeSinceLastChange += Time.deltaTime;

    if (timeSinceLastChange > changeTime)
    {
      timeSinceLastChange -= changeTime;
      movement = GetRandomUnitVector(ref angle, maxAngle);
    }

    Vector3 newPos = (position + (movement * speed * Time.deltaTime));

    int checker = 0;

    while (!PlayerMovement.ValidToMoveTo(newPos) && (++checker < 500))
    {
      movement = GetRandomUnitVector(out angle);
      newPos = (position + (movement * speed * Time.deltaTime));
    }

    if (checker == 500)
    {
      MWRDebug.Log("QixError1!!!", MWRDebug.DebugLevels.INFLOOP1);
    }

    Line moveLine = new Line(position, newPos);
    PlayerMovement pm = GameObject.Find("Player").GetComponent<PlayerMovement>();

    if (pm.DrawingLineIntersects(moveLine))
    {
      pm.Dead();
    }

    position = newPos;

    norm = DynamicLines.GetNormal(ref movement).normalized;

    start = position + norm * len;
    end = position - norm * len;

    Line currentVec = new Line(start, end);

    pm.KillIfDrawingLineIntersects(currentVec);

    timeSinceLastEnque+= Time.deltaTime;

    if (timeSinceLastEnque > enquePeriod)
    {
      timeSinceLastEnque -= enquePeriod;
      lineQueue.Enqueue(currentVec);

      while (lineQueue.Count > maxQueueLength)
      {
        lineQueue.Dequeue();
      }
    }

    GetComponent<MeshFilter>().mesh = DynamicLines.GetMesh(lineQueue.ToArray(), 0.01f);
	}
}
