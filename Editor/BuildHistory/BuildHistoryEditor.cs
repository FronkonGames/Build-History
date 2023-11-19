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
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;

namespace FronkonGames.Tools.BuildHistory
{
  /// <summary> History and statistics of all builds. </summary>
  public class BuildHistoryEditor : EditorWindow
  {
    private static GUIStyle BuildEntryStyle
    {
      get
      {
        styleBuildEntry ??= new GUIStyle(GUI.skin.label) { margin = new RectOffset(0, 0, 0, 0), alignment = TextAnchor.MiddleRight };
        
        return styleBuildEntry;
      }
    }
    
    private readonly GUIStyle stylePlatform;

    private Vector2 scrollPosition;

    private List<BuildEntry> buildHistory = new();

    private static GUIStyle styleBuildEntry;

    private bool showChartError = true;
    private bool showChartDuration = false;
    private bool showChartFiles = false;
    private bool showChartSize = false;

    private LineChart chartError;
    private LineChart chartDuration;
    private LineChart chartFiles;
    private LineChart chartSize;

    private enum Columns
    {
      BuildResult,
      BuildTarget,
      Date,
      Duration,
      Files,
      Size,
      Errors,
    }

    private static readonly string SortColumnKey        = "FronkonGames.BuildHistory.SortColumn";
    private static readonly string SortColumnOrderKey   = "FronkonGames.BuildHistory.SortColumnOrder";
    
    private static readonly string ShowChartErrorsKey   = "FronkonGames.BuildHistory.ShowChartErrors";
    private static readonly string ShowChartDurationKey = "FronkonGames.BuildHistory.ShowChartDuration";
    private static readonly string ShowChartFilesKey    = "FronkonGames.BuildHistory.ShowChartFilesKey";
    private static readonly string ShowChartSizeKey     = "NFronkonGames.BuildHistory.ShowChartSizeKey";
    
    [MenuItem("Window/Fronkon Games/Build History %&h")]
    private static void Launch()
    {
      BuildHistoryEditor buildHistoryEditor = GetWindow<BuildHistoryEditor>();
      buildHistoryEditor.titleContent = new GUIContent("Build History");
      buildHistoryEditor.ShowUtility();
    }      
    
    private void OnEnable()
    {
      buildHistory = BuildData.Load();

      UpdateCharts();

      SortColumn((Columns)EditorPrefs.GetInt(SortColumnKey, 0));

      BuildHook.BuildFinished += BuildFinished;
    }

    private void OnDisable()
    {
      BuildHook.BuildFinished -= BuildFinished;
    }

    private void OnGUI()
    {
      GUILayout.BeginVertical("Box");
      {
        BuildHistoryGUI();

        GUILayout.FlexibleSpace();

        if (buildHistory.Count > 0)
          ChartsGUI();

        EditorHelper.Space();

        GUILayout.BeginHorizontal();
        {
#if false
          if (GUILayout.Button("π", GUI.skin.label) == true)
          {
            int index = BuildData.FirstFreeIndex();

            DateTime start = new(2020, 1, 1);
            int range = (DateTime.Today - start).Days;

            Array buildTargetValues = Enum.GetValues(typeof(BuildTarget));
            Array buildResultValues = Enum.GetValues(typeof(BuildResult));

            BuildEntry buildEntry = new(string.Empty)
            {
              name = BuildData.GetKeyName(index),
              buildStartedAt = start.AddDays(UnityEngine.Random.Range(0, range)).AddHours(UnityEngine.Random.Range(0, 24)).AddMinutes(UnityEngine.Random.Range(0, 60)),
              platform = (BuildTarget)buildTargetValues.GetValue(UnityEngine.Random.Range(0, buildTargetValues.Length)),
              totalFiles = UnityEngine.Random.Range(200, 600)
            };
            buildEntry.totalSize = (ulong)buildEntry.totalFiles * (ulong)UnityEngine.Random.Range(100 * 1024, 5 * 1024 * 1024);
            buildEntry.totalSeconds = buildEntry.totalFiles * UnityEngine.Random.Range(15, 60);

            if (UnityEngine.Random.Range(0.0f, 1.0f) < 0.7f)
              buildEntry.result = BuildResult.Succeeded;
            else
              buildEntry.result = (BuildResult)buildResultValues.GetValue(UnityEngine.Random.Range(0, buildResultValues.Length));

            buildEntry.totalErrors = buildEntry.result == BuildResult.Succeeded ? 0 : UnityEngine.Random.Range(0, 50);
            buildEntry.totalWarnings = UnityEngine.Random.Range(0, buildEntry.result == BuildResult.Succeeded ? 10 : 100);

            buildHistory.Add(buildEntry);

            BuildData.Save(buildEntry);

            UpdateCharts();
          }
#endif            
          GUILayout.FlexibleSpace();

          if (GUILayout.Button("Import") == true)
            ImportCSV();
          
          if (GUILayout.Button("Export") == true)
            ExportCSV();

          EditorHelper.Space();
          EditorHelper.Space();

          if (GUILayout.Button("Remove all builds") == true)
          {
            buildHistory.Clear();
            
            BuildData.DeleteAll();
            
            UpdateCharts();
          }
          
          EditorHelper.Margin();
        }
        GUILayout.EndHorizontal();
      }
      GUILayout.EndVertical();
    }

    private void BuildHistoryGUI()
    {
      int indexToRemove = -1;
      bool odd = false;

      GUILayout.BeginHorizontal(EditorHelper.EntryEven);
      {
        EditorHelper.Margin();

        int sortColumn = EditorPrefs.GetInt(SortColumnKey, 0);
        int sortColumnOrder = EditorPrefs.GetInt(SortColumnOrderKey, 0);

        if (GUILayout.Button(sortColumn == (int)Columns.BuildResult ? (sortColumnOrder == 1 ? "\u25B2 " : "\u25BC ") : "-  ", BuildEntryStyle, GUILayout.Width(25)) == true)
          SortColumn(Columns.BuildResult);

        EditorHelper.Margin();

        if (GUILayout.Button(sortColumn == (int)Columns.BuildTarget ? (sortColumnOrder == 1 ? "Platform \u25B2" : "Platform \u25BC") : "Platform  - ", BuildEntryStyle, GUILayout.Width(170 + 25)) == true)
          SortColumn(Columns.BuildTarget);
        
        GUILayout.FlexibleSpace();

        if (GUILayout.Button(sortColumn == (int)Columns.Date ? (sortColumnOrder == 1 ? "Date \u25B2" : "Date \u25BC") : "Date  - ", BuildEntryStyle, GUILayout.Width(70 + 40 + 30)) == true)
          SortColumn(Columns.Date);

        GUILayout.FlexibleSpace();
        
        if (GUILayout.Button(sortColumn == (int)Columns.Duration ? (sortColumnOrder == 1 ? "Time \u25B2" : "Time \u25BC") : "Time  - ", BuildEntryStyle, GUILayout.Width(40 + 25)) == true)
          SortColumn(Columns.Duration);

        GUILayout.FlexibleSpace();
        
        if (GUILayout.Button(sortColumn == (int)Columns.Files ? (sortColumnOrder == 1 ? "Files \u25B2" : "Files \u25BC") : "Files  - ", BuildEntryStyle, GUILayout.Width(50 + 25)) == true)
          SortColumn(Columns.Files);

        GUILayout.FlexibleSpace();
        
        if (GUILayout.Button(sortColumn == (int)Columns.Size ? (sortColumnOrder == 1 ? "Size \u25B2" : "Size \u25BC") : "Size  - ", BuildEntryStyle, GUILayout.Width(50)) == true)
          SortColumn(Columns.Size);

        GUILayout.FlexibleSpace();
        
        if (GUILayout.Button(sortColumn == (int)Columns.Errors ? (sortColumnOrder == 1 ? "Errors & Warnings \u25B2" : "Errors & Warnings \u25BC") : "Errors & Warnings  - ", BuildEntryStyle, GUILayout.Width(40 + 25 + EditorHelper.margin + 40 + 25)) == true)
          SortColumn(Columns.Errors);

        GUILayout.FlexibleSpace();
        
        GUILayout.Space(25 + EditorHelper.margin);
      }
      GUILayout.EndHorizontal();

      if (buildHistory.Count > 0)
      {
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        {
          for (int i = 0; i < buildHistory.Count; ++i)
          {
            GUILayout.BeginHorizontal((odd = !odd) ? EditorHelper.EntryOdd : EditorHelper.EntryEven);
            {
              EditorHelper.Margin();

              switch (buildHistory[i].result)
              {
                case BuildResult.Cancelled: GUILayout.Label(new GUIContent(EditorHelper.Cancel, "Cancelled"), GUILayout.Width(25)); break;
                case BuildResult.Failed:    GUILayout.Label(new GUIContent(EditorHelper.Fail, "Fail"), GUILayout.Width(25)); break;
                case BuildResult.Succeeded: GUILayout.Label(new GUIContent(EditorHelper.Success, "Succeeded"), GUILayout.Width(25)); break;
                case BuildResult.Unknown:   GUILayout.Label(new GUIContent(EditorHelper.Ignored, "Unknown"), GUILayout.Width(25)); break;
              }

              EditorHelper.Margin();
              
              BuildPlatform buildPlatform = BuildPlatform.FromBuildTarget(buildHistory[i].platform);

              GUILayout.Label(buildPlatform.name, EditorHelper.RightLabel, GUILayout.Width(170));
              GUILayout.Label(buildPlatform.ToGUIContent(), GUILayout.Width(25));
              
              GUILayout.FlexibleSpace();

              string londDate = $"{buildHistory[i].buildStartedAt.ToShortDateString()} {buildHistory[i].buildStartedAt.ToLongTimeString()}";
              GUILayout.Label(new GUIContent(buildHistory[i].buildStartedAt.ToShortDateString(), londDate), EditorHelper.RightLabel, GUILayout.Width(70));
              GUILayout.Label(new GUIContent(buildHistory[i].buildStartedAt.ToString("HH:mm"), londDate), EditorHelper.RightLabel, GUILayout.Width(40));
              GUILayout.Label(EditorHelper.Date, GUILayout.Width(25));

              GUILayout.FlexibleSpace();
              
              TimeSpan time = TimeSpan.FromSeconds(buildHistory[i].totalSeconds);
              GUILayout.Label(time.ToString(@"hh\:mm"), EditorHelper.RightLabel, GUILayout.Width(40));
              GUILayout.Label(EditorHelper.Clock, GUILayout.Width(25));

              GUILayout.FlexibleSpace();

              GUILayout.Label(new GUIContent(buildHistory[i].totalFiles.ToString(), $"{buildHistory[i].totalFiles} files"), EditorHelper.RightLabel, GUILayout.Width(50));
              GUILayout.Label(EditorHelper.Files, GUILayout.Width(25));

              GUILayout.FlexibleSpace();

              GUILayout.Label(BytesToString(buildHistory[i].totalSize), EditorHelper.RightLabel, GUILayout.Width(50));
              GUILayout.Label(EditorHelper.TotalSize, GUILayout.Width(25));

              GUILayout.FlexibleSpace();

              GUILayout.Label(new GUIContent(buildHistory[i].totalErrors.ToString(), $"{buildHistory[i].totalErrors} errors"), EditorHelper.RightLabel, GUILayout.Width(40));
              GUILayout.Label(new GUIContent(EditorHelper.Error, $"{buildHistory[i].totalErrors} errors"), GUILayout.Width(25));

              EditorHelper.Margin();
              
              GUILayout.Label(new GUIContent(buildHistory[i].totalWarnings.ToString(), $"{buildHistory[i].totalWarnings} warnings"), EditorHelper.RightLabel, GUILayout.Width(40));
              GUILayout.Label(new GUIContent(EditorHelper.Warning, $"{buildHistory[i].totalWarnings} warnings"), GUILayout.Width(25));
              
              GUILayout.FlexibleSpace();

              if (GUILayout.Button(new GUIContent(EditorHelper.ToolbarMinus, "Remove build"), EditorHelper.EntryDesc, GUILayout.Width(25)) == true)
                indexToRemove = i;
              
              EditorHelper.Margin();
            }
            GUILayout.EndHorizontal();
          }
        }
        GUILayout.EndScrollView();          
      }

      if (indexToRemove >= 0)
      {
        BuildData.Delete(buildHistory[indexToRemove].name);
        
        buildHistory.RemoveAt(indexToRemove);

        UpdateCharts();
      }

      EditorHelper.Space();
    }

    private void ChartsGUI()
    {
      int totalTime = 0;

      for (int i = 0; i < buildHistory.Count; ++i)
        totalTime += buildHistory[i].totalSeconds;

      TimeSpan totalTimeSpan = TimeSpan.FromSeconds(totalTime);
      TimeSpan averagelTimeSpan = TimeSpan.FromSeconds(totalTime / buildHistory.Count);

      showChartDuration = EditorPrefs.GetBool(ShowChartDurationKey, true);
      showChartDuration = GUILayout.Toggle(showChartDuration, $" Duration (min), total {totalTimeSpan:dd\\:hh\\:mm}, average {averagelTimeSpan:hh\\:mm}", GUILayout.Width(Screen.width));
      EditorPrefs.SetBool(ShowChartDurationKey, showChartDuration);

      if (showChartDuration == true)
        chartDuration.DrawChart();

      showChartFiles = EditorPrefs.GetBool(ShowChartFilesKey, true);
      showChartFiles = GUILayout.Toggle(showChartFiles, " Files", GUILayout.Width(Screen.width));
      EditorPrefs.SetBool(ShowChartFilesKey, showChartFiles);
      
      if (showChartFiles == true)
        chartFiles.DrawChart();

      showChartSize = EditorPrefs.GetBool(ShowChartSizeKey, true);
      showChartSize = GUILayout.Toggle(showChartSize, " Size (mb)", GUILayout.Width(Screen.width));
      EditorPrefs.SetBool(ShowChartSizeKey, showChartSize);

      if (showChartSize == true)
        chartSize.DrawChart();
      
      showChartError = EditorPrefs.GetBool(ShowChartErrorsKey, true);
      showChartError = GUILayout.Toggle(showChartError, " Error and Warnings", GUILayout.Width(Screen.width));
      EditorPrefs.SetBool(ShowChartErrorsKey, showChartError);

      if (showChartError == true)
        chartError.DrawChart();
    }

    private void SortColumn(Columns column)
    {
      EditorPrefs.SetInt(SortColumnKey, (int)column);
      
      int sort = EditorPrefs.GetInt(SortColumnOrderKey, 1);

      switch (column)
      {
        case Columns.BuildResult: buildHistory.Sort((x, y) => sort * x.result.CompareTo(y.result)); break;
        case Columns.BuildTarget: buildHistory.Sort((x, y) => sort * x.platform.CompareTo(y.platform)); break;
        case Columns.Date:        buildHistory.Sort((x, y) => sort * x.buildStartedAt.CompareTo(y.buildStartedAt)); break;
        case Columns.Duration:    buildHistory.Sort((x, y) => sort * x.totalSeconds.CompareTo(y.totalSeconds)); break;
        case Columns.Files:       buildHistory.Sort((x, y) => sort * x.totalFiles.CompareTo(y.totalFiles)); break;
        case Columns.Size:        buildHistory.Sort((x, y) => sort * x.totalSize.CompareTo(y.totalSize)); break;
        case Columns.Errors:
          buildHistory.Sort((x, y) =>
          {
            int result = x.totalErrors.CompareTo(y.totalErrors);
            if (result == 0)
              result = x.totalWarnings.CompareTo(y.totalWarnings);

            return sort * result;
          });
          break;
      }
      
      EditorPrefs.SetInt(SortColumnOrderKey, sort * -1);
    }

    private void UpdateCharts()
    {
      const float height = 125.0f;

      // Error chart.
      if (chartError == null)
      {
        chartError = new LineChart(this, height);
        chartError.data = new List<float>[2];
        chartError.data[0] = new List<float>();
        chartError.data[1] = new List<float>();
        chartError.axisLabels = new List<string>();
      
        chartError.colors = new List<Color>{ Color.red, Color.yellow };
      }
      
      chartError.data[0].Clear();
      chartError.data[1].Clear();

      // Duration chart.
      if (chartDuration == null)
      {
        chartDuration = new LineChart(this, height);
        chartDuration.data = new List<float>[1];
        chartDuration.data[0] = new List<float>();
        chartDuration.axisLabels = new List<string>();
      
        chartDuration.colors = new List<Color>{ Color.green };
      }

      chartDuration.data[0].Clear();
      
      // Files chart.
      if (chartFiles == null)
      {
        chartFiles = new LineChart(this, height);
        chartFiles.data = new List<float>[1];
        chartFiles.data[0] = new List<float>();
        chartFiles.axisLabels = new List<string>();
      
        chartFiles.colors = new List<Color>{ Color.cyan };
      }

      chartFiles.data[0].Clear();

      // Size chart.
      if (chartSize == null)
      {
        chartSize = new LineChart(this, height);
        chartSize.data = new List<float>[1];
        chartSize.data[0] = new List<float>();
        chartSize.axisLabels = new List<string>();
      
        chartSize.colors = new List<Color>{ Color.magenta };
      }

      chartSize.data[0].Clear();
      
      // Update.
      List<BuildEntry> clone = buildHistory;
      clone.Sort((x, y) => x.buildStartedAt.CompareTo(y.buildStartedAt));
      
      Dictionary<string, Tuple<int, int>> errorsByDay = new();
      Dictionary<string, int> durationByDay = new();
      Dictionary<string, int> filesByDay = new();
      Dictionary<string, ulong> sizeByDay = new();

      for (int i = 0; i < clone.Count; ++i)
      {
        string key = clone[i].buildStartedAt.ToString("dd");

        if (errorsByDay.ContainsKey(key) == true)
          errorsByDay[key] = new Tuple<int, int>(errorsByDay[key].Item1 + clone[i].totalErrors, errorsByDay[key].Item2 + clone[i].totalWarnings);
        else
          errorsByDay.Add(key, new Tuple<int, int>(clone[i].totalErrors, clone[i].totalWarnings));
        
        if (durationByDay.ContainsKey(key) == true)
          durationByDay[key] += clone[i].totalSeconds / 60;
        else
          durationByDay.Add(key, clone[i].totalSeconds / 60);

        if (filesByDay.ContainsKey(key) == true)
          filesByDay[key] = (filesByDay[key] + clone[i].totalFiles) / 2;
        else
          filesByDay.Add(key, clone[i].totalFiles);

        if (sizeByDay.ContainsKey(key) == true)
          sizeByDay[key] = (sizeByDay[key] + (clone[i].totalSize / (1024 * 1024))) / 2;
        else
          sizeByDay.Add(key, clone[i].totalSize / (1024 * 1024));
      }

      foreach (KeyValuePair<string, Tuple<int, int>> kv in errorsByDay)
      {
        chartError.data[0].Add(kv.Value.Item1);
        chartError.data[1].Add(kv.Value.Item2);
        chartError.axisLabels.Add(kv.Key);
      }

      foreach (KeyValuePair<string, int> kv in durationByDay)
      {
        chartDuration.data[0].Add(kv.Value);
        chartDuration.axisLabels.Add(kv.Key);
      }
      
      foreach (KeyValuePair<string, int> kv in filesByDay)
      {
        chartFiles.data[0].Add(kv.Value);
        chartFiles.axisLabels.Add(kv.Key);
      }
      
      foreach (KeyValuePair<string, ulong> kv in sizeByDay)
      {
        chartSize.data[0].Add(kv.Value);
        chartSize.axisLabels.Add(kv.Key);
      }
    }

    private void BuildFinished(BuildReport buildReport)
    {
      buildHistory.Clear();
      
      buildHistory = BuildData.Load();
      
      this.Repaint();
    }

    private void ExportCSV()
    {
      string filePath = EditorUtility.SaveFilePanel("Export", Application.dataPath, "BuildHistory.csv", "csv");
      if (string.IsNullOrEmpty(filePath) == false)
      {
        if (File.Exists(filePath) == true)
          File.Delete(filePath);

        using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
          StreamWriter writer = new StreamWriter(fileStream, Encoding.UTF8);

          for (int i = 0; i < buildHistory.Count; ++i)
            writer.WriteLine(buildHistory[i].ToString());
          
          writer.Close();
        }
      }
    }

    private void ImportCSV()
    {
      string filePath = EditorUtility.OpenFilePanel("Inport", Application.dataPath, "csv");
      if (string.IsNullOrEmpty(filePath) == false && File.Exists(filePath) == true)
      {
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
          buildHistory.Clear();
          
          BuildData.DeleteAll();
          
          StreamReader reader = new StreamReader(fileStream, Encoding.UTF8);
          while (reader.Peek() >= 0)
          {
            string entry = reader.ReadLine();
            
            BuildEntry buildEntry = new BuildEntry(entry);
            
            buildHistory.Add(buildEntry);
            
            BuildData.Save(buildEntry);
          }
          
          reader.Close();
        }

        UpdateCharts();
      }
    }

    private static string BytesToString(ulong bytes)
    {
      string[] sizes = { "B", "KB", "MB", "GB", "TB" };
      
      int order = 0;
      while (bytes >= 1024 && order < sizes.Length - 1) {
        order++;
        bytes = bytes / 1024;
      }

      return $"{bytes:0.##} {sizes[order]}";        
    }
  }
}