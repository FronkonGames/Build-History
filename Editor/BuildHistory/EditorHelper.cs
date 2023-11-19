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
using UnityEditor;
using UnityEngine;

namespace FronkonGames.Tools.BuildHistory
{
  /// <summary> Editor tools. </summary>
  public static class EditorHelper
  {
    public static Color PastelYellow  = new(255.0f / 255.0f, 250.0f / 255.0f, 129.0f / 255.0f);
    public static Color PastelGreen   = new(191.0f / 255.0f, 228.0f / 255.0f, 118.0f / 255.0f);
    public static Color PastelBlue    = new(154.0f / 255.0f, 206.0f / 255.0f, 223.0f / 255.0f);
    public static Color PastelPurple  = new(193.0f / 255.0f, 179.0f / 255.0f, 215.0f / 255.0f);
    public static Color PastelOrange  = new(252.0f / 255.0f, 169.0f / 255.0f, 133.0f / 255.0f);

    // Unity Editor icons https://github.com/halak/unity-editor-icons.
    public static Texture2D ToolbarMinus  { get { if (toolbarMinus == null) toolbarMinus = LoadEditorIcon("d_TreeEditor.Trash"); return toolbarMinus; } }
    public static Texture2D ToolbarPlus   { get { if (toolbarPlus == null) toolbarPlus = LoadEditorIcon("Toolbar Plus"); return toolbarPlus; } }
    public static Texture2D Favorite      { get { if (favorite == null) favorite = LoadEditorIcon("d_Favorite"); return favorite; } }
    public static Texture2D Files         { get { if (files == null) files = LoadEditorIcon("d_Package Manager"); return files; } }
    public static Texture2D TotalSize     { get { if (totalSize == null) totalSize = LoadEditorIcon("d_PreMatCube"); return totalSize; } }
    public static Texture2D Clock         { get { if (clock == null) clock = LoadEditorIcon("TestStopwatch"); return clock; } }
    public static Texture2D Date          { get { if (date == null) date = LoadEditorIcon("d_UnityEditor.AnimationWindow"); return date; } }

    public static Texture2D Fail    { get { if (fail == null) fail = LoadEditorIcon("TestFailed"); return fail; } }
    public static Texture2D Ignored { get { if (ignored == null) ignored = LoadEditorIcon("TestIgnored"); return ignored; } }
    public static Texture2D Cancel  { get { if (cancel == null) cancel = LoadEditorIcon("TestInconclusive"); return cancel; } }
    public static Texture2D Success { get { if (success == null) success = LoadEditorIcon("TestPassed"); return success; } }
    public static Texture2D Error   { get { if (error == null) error = LoadEditorIcon("console.erroricon.sml"); return error; } }
    public static Texture2D Warning { get { if (warning == null) warning = LoadEditorIcon("console.warnicon.sml"); return warning; } }

    public static GUIStyle Box        { get { styleBox ??= new GUIStyle(GUIStyle.none) { margin = new RectOffset(margin, margin, margin, margin) }; return styleBox; } }
    public static GUIStyle EntryDesc  { get { styleDesc ??= new GUIStyle(GUI.skin.label) { wordWrap = true }; return styleDesc; } }
    public static GUIStyle EntryOdd   { get { if (styleEntryOdd == null) { styleEntryOdd = new GUIStyle(GUIStyle.none) { margin = new RectOffset(0, 0, 0, 0) }; styleEntryOdd.normal.background = MakeTex(8, 8, Color.white * 0.4f); } return styleEntryOdd; } }
    public static GUIStyle EntryEven  { get { if (styleEntryEven == null) { styleEntryEven = new GUIStyle(GUIStyle.none) { margin = new RectOffset(0, 0, 0, 0) }; styleEntryEven.normal.background = MakeTex(8, 8, Color.white * 0.1f); } return styleEntryEven; } }
    public static GUIStyle EntryName  { get { styleEntryName ??= new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft }; return styleEntryName; } }
    public static GUIStyle RightLabel { get { styleRightLabel ??= new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight }; return styleRightLabel; } }

    private static Texture2D toolbarMinus, toolbarPlus, favorite, files, totalSize, clock, date;
    private static Texture2D fail, ignored, cancel, success, error, warning;
    private static GUIStyle styleBox, styleEntryOdd, styleEntryEven, styleEntryName, styleRightLabel, styleDesc;
    
    public const float space = 10.0f;
    public const int margin = 5;

    /// <summary> Space. </summary>
    public static void Space() => GUILayout.Space(space);

    /// <summary> Margin. </summary>
    public static void Margin() => GUILayout.Space(margin);
    
    /// <summary> TextField with tooltips. </summary>
    public static string Text(string label, string tooltip, string text) => EditorGUILayout.TextField(new GUIContent(label, tooltip), text);

    /// <summary> Toggle with tooltips. </summary>
    public static bool Toggle(string label, string tooltip, bool value) => EditorGUILayout.Toggle(new GUIContent(label, tooltip), value);
    
    private static Texture2D LoadEditorIcon(string iconId) => EditorGUIUtility.IconContent(iconId).image as Texture2D;
    
    private static Texture2D MakeTex(int width, int height, Color col)
    {
      Color[] pix = new Color[width * height];

      for (int i = 0; i < pix.Length; ++i)
        pix[i] = col;

      Texture2D result = new Texture2D(width, height);
      result.SetPixels(pix);
      result.Apply();

      return result;
    }      
  }
}
