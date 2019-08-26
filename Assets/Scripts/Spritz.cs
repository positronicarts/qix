using UnityEngine;
using System.Collections;

public class Spritz : MonoBehaviour {

  Vector3 center;
  Vector3 movement;
  Vector3 norm;

  public float speed;
  public int StartDirIndex;

  Vector3[] dirs = new Vector3[]
  {
    new Vector3(1.0f, 1.0f, 0.0f),
    new Vector3(1.0f, -1.0f, 0.0f),
    new Vector3(-1.0f, -1.0f, 0.0f),
    new Vector3(-1.0f, 1.0f, 0.0f)
  };

  int curDirIndex;

  // Use this for initialization
  void Start()
  {
    center = new Vector3(0.0f, 0.0f, 0.0f);

    curDirIndex = StartDirIndex;

    TryNextDir(1);

    transform.position = Vector3.zero;
  }

  private void TryNextDir(int delta)
  {
    curDirIndex = ((curDirIndex + delta) % dirs.Length);
    movement = dirs[curDirIndex];
  }

  // Update is called once per frame
  void Update()
  {
    Vector3 newPos = (transform.position + (movement * speed * Time.deltaTime));

    int checker = 0;

    int delta = 1;

    while (!PlayerMovement.ValidToMoveTo(newPos) && (++checker < 500))
    {
      TryNextDir(delta);
      delta++;

      newPos = (transform.position + (movement * speed * Time.deltaTime));
    }

    if ((checker == 500) || (!PlayerMovement.ValidToMoveTo(newPos)))
    {
      MWRDebug.Log("Spritz Error!!!", MWRDebug.DebugLevels.INFLOOP1);
    }

    Line moveLine = new Line(transform.position, newPos);
    PlayerMovement pm = GameObject.Find("Player").GetComponent<PlayerMovement>();

    if (pm.DrawingLineIntersects(moveLine))
    {
      pm.Dead();
    }

    Line currentVec = new Line(transform.position, newPos);
    pm.KillIfDrawingLineIntersects(currentVec);

    transform.position = newPos;
  }
}
