using UnityEngine;
using System.Collections;

public class Sparx : MonoBehaviour {

  enum Flavour
  {
    Clockwise,
    Anticlockwise
  }

  enum Direction
  {
    StartToEnd,
    EndToStart
  }

  Flavour flavour;
  Line currentLine;
  PlayerMovement pm;
  Direction dir;

  public float speed;

	// Use this for initialization
	void Start () 
  {
    pm = GameObject.Find("Player").GetComponent<PlayerMovement>();

    initialized = false;
    Init();
	}

  bool initialized;

  private void Init()
  {
    // Hard-code for now...
    if (!initialized && (pm.lines != null) && (pm.lines.Count > 0))
    {
      currentLine = pm.lines[0];
      transform.position = currentLine.start;
      dir = Direction.StartToEnd;
      speed = 1.0f;
      initialized = true;
    }
  }
	
	// Update is called once per frame
	void Update () 
  {
    float distanceToTravel = speed * Time.deltaTime;

    Init();

    if (!initialized)
    {
      return;
    }

    int loopy = 0;

    do
    {
      loopy++;

      MWRDebug.Log("Loopy: " + loopy, MWRDebug.DebugLevels.INFLOOP1);
      MWRDebug.Log("Distance: " + distanceToTravel, MWRDebug.DebugLevels.INFLOOP1);
      MWRDebug.Log("Dir: " + dir, MWRDebug.DebugLevels.INFLOOP1);
      MWRDebug.Log("Line: " + currentLine.start + "->" + currentLine.end, MWRDebug.DebugLevels.INFLOOP1);
      
      switch (dir)
      {
        case Direction.StartToEnd:
          {
            float distToEnd = 0.0f;
            Vector3 end = Vector3.zero;

            distToEnd = (currentLine.end - transform.position).magnitude;

            // Can't reach the end
            if (distToEnd < distanceToTravel)
            {
              pm.KillIfPlayerInLine(new Line(transform.position, currentLine.end));

              distanceToTravel -= distToEnd;
              transform.position = currentLine.end;

              MWRDebug.Log("Need new line!", MWRDebug.DebugLevels.INFLOOP1);

              foreach (Line line in pm.lines)
              {
                if (line.ContainsBound(currentLine.end) && (line != currentLine))
                {
                  currentLine = line;

                  if ((line.end - transform.position).magnitude < (line.start - transform.position).magnitude)
                  {
                    // Go to the long way
                    dir = Direction.EndToStart;
                  }
                  else
                  {
                    dir = Direction.StartToEnd;
                  }

                  MWRDebug.Log("Found Line: " + line.start + "->" + line.end, MWRDebug.DebugLevels.INFLOOP1);
                  break;
                }
              }
            }
            else
            {

              Vector3 newPos = (transform.position + distanceToTravel * (currentLine.end - transform.position).normalized);

              pm.KillIfPlayerInLine(new Line(transform.position, newPos));
              transform.position = newPos;

              distanceToTravel = 0.0f;
            }
          }

          break;

        case Direction.EndToStart:
          {
            float distToEnd = 0.0f;
            Vector3 end = Vector3.zero;

            distToEnd = (currentLine.start - transform.position).magnitude;

            // Can't reach the end
            if (distToEnd < distanceToTravel)
            {
              pm.KillIfPlayerInLine(new Line(transform.position, currentLine.start));
              
              distanceToTravel -= distToEnd;
              transform.position = currentLine.start;

              foreach (Line line in pm.lines)
              {
                if (line.ContainsBound(currentLine.start) && (line != currentLine))
                {
                  currentLine = line;

                  if ((line.end - transform.position).magnitude < (line.start - transform.position).magnitude)
                  {
                    // Go to the long way
                    dir = Direction.EndToStart;
                  }
                  else
                  {
                    dir = Direction.StartToEnd;
                  }
                }

                break;
              }
            }
            else
            {
              Vector3 newPos = (transform.position + distanceToTravel * (currentLine.start - transform.position).normalized);

              pm.KillIfPlayerInLine(new Line(transform.position, newPos));
              transform.position = newPos;

              distanceToTravel = 0.0f;
            }
          }

          break;
      }

    } while ((distanceToTravel > 0.0f) && (loopy < 5));

    if (loopy == 5)
    {
      MWRDebug.Log("Oh dear", MWRDebug.DebugLevels.INFLOOP1);
    }
	}
}
