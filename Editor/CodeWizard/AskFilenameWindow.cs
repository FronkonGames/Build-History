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
using UnityEngine;
using UnityEditor;

namespace FronkonGames.Tools.CodeWizard
{
  /// <summary> 'Ask filename' dialog. </summary>
  public class AskFilenameWindow : EditorWindow
  {
    private static AskFilenameWindow window;

    private static string fileName;

    public static void Launch()
    {
      fileName = CodeWizardSettings.Instance.DefaultFilename;

      window = (AskFilenameWindow)GetWindow(typeof(AskFilenameWindow), true, "Select a name for the file");
      window.minSize = window.maxSize = new Vector2(420.0f, 45.0f);

      window.ShowUtility();
      window.Focus();

      EditorGUI.FocusTextInControl("FileNameTextField");
    }

    private void OnGUI()
    {
      EditorGUIUtility.labelWidth = 75.0f;
      EditorGUIUtility.fieldWidth = 225.0f;

      GUILayout.Space(2.0f);

      GUILayout.BeginHorizontal();
      {
        GUI.SetNextControlName("FileNameTextField");

        fileName = EditorGUILayout.TextField("Filename", fileName);

        GUI.enabled = !string.IsNullOrEmpty(fileName);

        GUILayout.Space(2.0f);

        if (GUILayout.Button("Done", GUILayout.Width(45.0f)) == true || WasEnterPressed() == true)
          this.Close();

        GUI.enabled = true;
      }
      GUILayout.EndHorizontal();

      GUILayout.Space(2.0f);

      if (string.IsNullOrEmpty(GUI.GetNameOfFocusedControl()) == true)
        GUI.FocusControl("FileNameTextField");
    }

    private void OnDestroy()
    {
      GUI.SetNextControlName(string.Empty);

      if (string.IsNullOrEmpty(fileName) == false)
        CodeWizard.CreateMonoBehaviour(fileName);
    }

    private static bool WasEnterPressed()
    {
      return GUI.enabled == true && Event.current != null && Event.current.isKey == true && Event.current.keyCode == KeyCode.Return;
    }
  }
}
