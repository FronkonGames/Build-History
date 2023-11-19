////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Martin Bustos @FronkonGames <fronkongames@gmail.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of
// the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace FronkonGames.Tools.BuildHistory
{
  /// <summary> Data storage. </summary>
  public static class BuildData
  {
    private const string BuildPrefix = "BuildEntry_";

    private const int MaxEntries = 1000;

    /// <summary> Returns all the information of the builds. </summary>
    public static List<BuildEntry> Load()
    {
      List<BuildEntry> buildHistory = new();

      for (int i = 0; i < MaxEntries; ++i)
      {
        string buildValue = EditorPrefs.GetString(GetKeyName(i));
        if (string.IsNullOrEmpty(buildValue) == false)
          buildHistory.Add(new BuildEntry(buildValue));
      }

      return buildHistory;
    }

    /// <summary> Store all build information. </summary>
    public static void Save(BuildReport buildReport)
    {
      if (buildReport != null)
      {
        int buildIndex = 0;
        while (EditorPrefs.GetString(GetKeyName(buildIndex)) != string.Empty)
          buildIndex++;

        if (buildIndex < MaxEntries)
          EditorPrefs.SetString(GetKeyName(buildIndex), new BuildEntry(buildReport).ToString());
        else
          Debug.LogError("[FrokonGames.BuildHistory] Maximum number of builds exceeded.");
      }
    }

    /// <summary> Store build information. </summary>
    public static void Save(BuildEntry buildEntry)
    {
      if (buildEntry != null)
        EditorPrefs.SetString(buildEntry.name, buildEntry.ToString());
    }
    
    /// <summary> Delete key. </summary>
    public static void Delete(string key) => EditorPrefs.DeleteKey(key);
    
    /// <summary> Delete key. </summary>
    public static void Delete(int index) => EditorPrefs.DeleteKey(GetKeyName(index));
    
    /// <summary> Delete all keys. </summary>
    public static void DeleteAll()
    {
      for (int i = 0; i < MaxEntries; ++i)
        EditorPrefs.DeleteKey(GetKeyName(i));
    }

    /// <summary> First free index to create a new entry. </summary>
    public static int FirstFreeIndex()
    {
      int buildIndex = 0;
      while (EditorPrefs.GetString(GetKeyName(buildIndex)) != string.Empty)
        buildIndex++;

      if (buildIndex >= MaxEntries)
        buildIndex = -1;

      return buildIndex;
    }
    
    /// <summary> Name of an entry. </summary>
    public static string GetKeyName(int index) => $"{BuildPrefix}{index:000}";
  }
}

