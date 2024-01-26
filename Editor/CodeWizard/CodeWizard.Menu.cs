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
  /// <summary> Code Templates Editor menu. </summary>
  public static class CodeTemplatesEditorMenu
  {
    [MenuItem("Assets/Create/Code Wizard/MonoBehaviour", false, 1)]
    private static void CreateMonoBehavior()
    {
      if (CodeWizardSettings.Instance != null)
      {
        if (CodeWizardSettings.Instance.AskFilename == true)
          AskFilenameWindow.Launch();
        else
          CodeWizard.CreateMonoBehaviour();
      }
    }

    [MenuItem("Assets/Create/Code Wizard/Editor", false, 2)]
    private static void CreateEditor()
    {
      if (CodeWizardSettings.Instance != null)
      {
        if (CodeWizardSettings.Instance.AskFilename == true)
          AskFilenameWindow.Launch();
        else
          CodeWizard.CreateEditor();
      }
    }
    
    [MenuItem("Assets/Create/Code Wizard/Enum", false, 3)]
    private static void CreateEnum()
    {
      if (CodeWizardSettings.Instance != null)
      {
        if (CodeWizardSettings.Instance.AskFilename == true)
          AskFilenameWindow.Launch();
        else
          CodeWizard.CreateEnum();
      }
    }

    [MenuItem("Assets/Create/Code Wizard/Empty", false, 4)]
    private static void CreateEmpty()
    {
      if (CodeWizardSettings.Instance != null)
      {
        if (CodeWizardSettings.Instance.AskFilename == true)
          AskFilenameWindow.Launch();
        else
          CodeWizard.CreateEmpty();
      }
    }

    [MenuItem("Assets/Create/Code Wizard/Create settings", true)]
    static bool CheckCreateSettings() => AssetDatabase.FindAssets("t:CodeWizardSettings", null).Length == 0;

    [MenuItem("Assets/Create/Code Wizard/Create settings", false, 100)]
    private static void CreateSettings()
    {
      CodeWizardSettings codeWizardSettings = ScriptableObject.CreateInstance<CodeWizardSettings>();
      AssetDatabase.CreateAsset(codeWizardSettings, AssetDatabase.GenerateUniqueAssetPath("Assets/CodeWizardSettings.asset"));
      AssetDatabase.SaveAssets();
    }
  }
}
