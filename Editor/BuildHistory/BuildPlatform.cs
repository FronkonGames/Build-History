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
using UnityEngine;

namespace FronkonGames.Tools.BuildHistory
{
  /// <summary> Build platform. </summary>
  public class BuildPlatform
  {
    public readonly string id;
    public readonly string name;
    
    private static List<BuildPlatform> All
    {
      get
      {
        if (allBuildPlatforms == null)
        {
          allBuildPlatforms = new List<BuildPlatform>
          {
            new("PC, Mac & Linux Standalone",  "Standalone"),
            new("Android",                     "Android"),
            new("WebGL",                       "WebGL"),
            new("Universal Windows Platform",  "Metro"),
            new("tvOS",                        "tvOS"),
            new("PS4",                         "PS4"),
            new("PS5",                         "PS5"),
            new("iOS",                         "iPhone"),
            new("Xbox One",                    "XboxOne"),
            new("Magic Leap OS",               "Lumin"),
            new("Switch",                      "Switch"),
          };
        }

        return allBuildPlatforms;
      }
    }
    
    private readonly Texture2D iconSmall;
    private readonly Texture2D icon;
    
    private static List<BuildPlatform> allBuildPlatforms;
    
    public BuildPlatform(string name, string id)
    {
      this.name = name;
      this.id = id;
      this.iconSmall = EditorGUIUtility.IconContent($"BuildSettings.{id}.Small").image as Texture2D;
      this.icon = EditorGUIUtility.IconContent($"BuildSettings.{id}").image as Texture2D;
    }

    public GUIContent ToGUIContent(bool small = true)
    {
      return new GUIContent(small == true ? iconSmall : icon, name);
    }

    public static BuildPlatform FromBuildTarget(BuildTarget target)
    {
      switch (target)
      {
        case BuildTarget.StandaloneLinux64:
        case BuildTarget.LinuxHeadlessSimulation:
        case BuildTarget.StandaloneOSX:
        case BuildTarget.StandaloneWindows:
        case BuildTarget.StandaloneWindows64:       return All[0];
        case BuildTarget.Android:                   return All[1];
        case BuildTarget.WebGL:                     return All[2];
        case BuildTarget.WSAPlayer:                 return All[3];
        case BuildTarget.tvOS:                      return All[4];
        case BuildTarget.PS4:                       return All[5];
        case BuildTarget.PS5:                       return All[6];
        case BuildTarget.iOS:                       return All[7];
        case BuildTarget.GameCoreXboxSeries:
        case BuildTarget.GameCoreXboxOne:
        case BuildTarget.XboxOne:                   return All[8];
        case BuildTarget.Lumin:                     return All[9];
        case BuildTarget.Switch:                    return All[10];
      }

      return All[0];
    }
  }
}

