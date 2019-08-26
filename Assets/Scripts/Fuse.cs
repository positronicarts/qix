using UnityEngine;
using System.Collections;

public class Fuse : MonoBehaviour {

	// Use this for initialization
	void Start () 
  {
    inMotion = false;
    lineIndex = 0;
    dead = false;
	}

  bool inMotion;
  bool givingHeadStart;

  int lineIndex;
  public float speed;
  bool dead;

  float startWaitTime;
  float headstart = 2.0f;

	// Update is called once per frame
	void Update () 
  {
    //MWRDebug.Log("Fuse update");

    if (dead)
    {
      MWRDebug.Log("Dead");
      return;
    }

    PlayerMovement mov = GameObject.Find("Player").GetComponent<PlayerMovement>();

    var lines = mov.GetDrawingLinesInclLive().ToArray();

    if (lines.Length == 0)
    {
      //MWRDebug.Log("No line");
      inMotion = false;
      givingHeadStart = false;
      gameObject.GetComponent<Renderer>().enabled = false;
    }
    else if (!givingHeadStart)
    {
      MWRDebug.Log("Giving headstart");
      givingHeadStart = true;
      startWaitTime = Time.time;
    }
    else if ((!inMotion) && ((Time.time - startWaitTime) > headstart))
    {
      MWRDebug.Log("Starting");
      gameObject.GetComponent<Renderer>().enabled = true;
      inMotion = true;

      transform.position = lines[0].start;
      lineIndex = 0;
    }

    if (inMotion)
    {
      MWRDebug.Log("In motion");

      float distToMove = speed * Time.deltaTime;

      int checker = 0;
      while ((distToMove >= 0.0f) && (++checker < 5))
      {
        Vector3 toEndOfLine = lines[lineIndex].end - transform.position;
        float distToEndOfLine = toEndOfLine.magnitude;

        MWRDebug.Log("Dists " + distToEndOfLine + " vs. " + distToMove + ".  Indices " + lineIndex + " vs. " + lines.Length);

        if (distToEndOfLine > distToMove)
        {
          MWRDebug.Log("End of line too far");
          transform.position += toEndOfLine.normalized * distToMove;
          break;
        }
        else
        {
          MWRDebug.Log("Can reach end of line");

          transform.position = lines[lineIndex].end;
          distToMove -= distToEndOfLine;

          if (lineIndex == (lines.Length - 1))
          {
            MWRDebug.Log("Fused!");
            dead = true;
            ScoreManager.Dead = true;
            break;
          }
          else
          {
            MWRDebug.Log("Move to next segment");
            lineIndex++;
          }
        }
      }

      if (checker == 5)
      {
        MWRDebug.Log("FuseERRROR", MWRDebug.DebugLevels.INFLOOP1);
      }
    }
	}
}
