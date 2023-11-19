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
using System;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace FronkonGames.Tools.BuildHistory
{
  /// <summary> Information of a build. </summary>
  public class BuildEntry
  {
    /// <summary> Internal name. </summary>
    public string name;
    
    /// <summary> Date it started. </summary>
    public DateTime buildStartedAt;

    /// <summary> Platform selected for the build. </summary>
    public BuildTarget platform;

    /// <summary> Number of files processed in the build. </summary>
    public int totalFiles;
    
    /// <summary> Size in bytes of the build. </summary>
    public ulong totalSize;

    /// <summary> Time in second that the build lasted. </summary>
    public int totalSeconds;

    /// <summary> Number of errors at the end of the build. </summary>
    public int totalErrors;

    /// <summary> Number of warnings at the end of the build. </summary>
    public int totalWarnings;

    /// <summary> Result of the build. </summary>
    public BuildResult result;

    /// <summary> Constructor.  </summary>
    public BuildEntry(BuildReport report)
    {
      BuildSummary summary = report.summary;

      int buildIndex = BuildData.FirstFreeIndex();
      if (buildIndex != -1)
      {
        name = BuildData.GetKeyName(buildIndex);
        buildStartedAt = summary.buildStartedAt;
        platform = summary.platform;
        totalFiles = report.files.Length; 
        totalSize = summary.totalSize;
        totalSeconds = (int)summary.totalTime.TotalSeconds;
        totalErrors = summary.totalErrors;
        totalWarnings = summary.totalWarnings;
        result = summary.result;
      }
    }
    
    /// <summary> Constructor.  </summary>
    public BuildEntry(string text)
    {
      string[] values = text.Split(',');
      if (values.Length == 9)
      {
        name = values[0];
        buildStartedAt = new DateTime(long.Parse(values[1]));
        platform = StringToBuildTarget(values[2]);
        totalFiles = int.Parse(values[3]);
        totalSize = ulong.Parse(values[4]);
        totalSeconds = int.Parse(values[5]);
        totalErrors = int.Parse(values[6]);
        totalWarnings = int.Parse(values[7]);
        result = StringToBuildResult(values[8]);
      }
    }

    private static BuildTarget StringToBuildTarget(string target)
    {
      Enum.TryParse(target, out BuildTarget buildTarget);

      return buildTarget;
    }

    private static BuildResult StringToBuildResult(string build)
    {
      Enum.TryParse(build, out BuildResult buildResult);

      return buildResult;
    }

    public override string ToString() => $"{name}," +
                                         $"{buildStartedAt.ToBinary()}," +
                                         $"{platform}," +
                                         $"{totalFiles}," +
                                         $"{totalSize}," +
                                         $"{totalSeconds}," +
                                         $"{totalErrors}," +
                                         $"{totalWarnings}," +
                                         $"{result}";
  }
}

