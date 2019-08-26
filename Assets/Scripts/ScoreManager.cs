using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour {

  public GUIText guitext;
  public static float Score;
  public static int Target;
  public static bool Dead;

	// Use this for initialization
	void Start () 
  {
    Score = 0.0f;
    Target = 70;
    Dead = false;
	}
	
	// Update is called once per frame
	void Update () 
  {
	
	}

  void OnGUI()
  {
    int pc = (int)(100.0f * Score / 4.0f);
    guitext.text = "Score: " + pc + "%";
    guitext.text += "\nTarget: " + Target + "%";

    if (Dead)
    {
      guitext.text += "\nYOU LOSE!";
    }
    else if (pc > Target)
    {
      guitext.text += "\nYOU WIN!";
    }
  }


}
