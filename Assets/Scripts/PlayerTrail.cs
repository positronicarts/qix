using UnityEngine;
using System.Collections;

public class PlayerTrail : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
  {
	
	}
	
	// Update is called once per frame
	void Update () 
  {
    PlayerMovement mov = GameObject.Find("Player").GetComponent<PlayerMovement>();

    var lines = mov.GetDrawingLinesInclLive().ToArray();
    GetComponent<MeshFilter>().mesh = DynamicLines.GetMesh(lines, 0.02f);
	}
}
