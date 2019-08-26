using UnityEngine;
using System;
using System.Collections.Generic;

public class MWRDebug
{
  public enum DebugLevels
  {
    ALWAYS,
    NONE,
    DEBUG,
    REGULAR,
    INFLOOP1,
    CUST2
  }

  public const DebugLevels currentLevel = DebugLevels.ALWAYS;

  public static void Log(string s, DebugLevels lev = DebugLevels.DEBUG)
  {
    if ((currentLevel == lev) || (lev == DebugLevels.ALWAYS))
    {
      Debug.Log(s);
    }
  }

  private const string DUMP_FILENAME = "mwr.txt";

  public static void DumpToTraceFile(string s)
  {
    // Write the string to a file.
    System.IO.StreamWriter file = new System.IO.StreamWriter(DUMP_FILENAME);
    file.WriteLine(s);

    file.Close();
  }
}

