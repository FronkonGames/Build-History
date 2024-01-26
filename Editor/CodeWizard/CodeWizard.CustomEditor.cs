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
using UnityEngine;

namespace FronkonGames.Tools.CodeWizard
{
  /// <summary> Custom code template. </summary>
  public sealed partial class CodeWizard
  {
    /// <summary> Creates a Custom Editor script. </summary>
    public static void CreateEditor(string fileName = default(string))
    {
      string selectedPath = GetSelectedPath();

      if (string.IsNullOrEmpty(selectedPath) == false)
      {
        selectedPath = Path.Combine(Application.dataPath.Remove(Application.dataPath.Length - 7), selectedPath);
        selectedPath = selectedPath.Replace("\\", "/");

        if (string.IsNullOrEmpty(fileName) == true)
          fileName = CodeWizardSettings.Instance.DefaultFilename;

        string scriptFilePath = $"{selectedPath}/{fileName}.cs";

        int fileIndex = 0;
        while (File.Exists(scriptFilePath) == true)
          scriptFilePath = $"{selectedPath}/{fileName}{fileIndex++:00}.cs";

        try
        {
          OpenStream(scriptFilePath);

          WriteHeader();

          WriteHistory();

          WriteUsings();

          // Begin namespace.
          if (string.IsNullOrEmpty(CodeWizardSettings.Instance.Namespace) == false)
            WriteBeginBlock($"namespace {CodeWizardSettings.Instance.Namespace}");

          WriteCode("/// <summary>");
          WriteCode("/// ");
          WriteCode("/// </summary>");

          string className = FileNameToClassName(Path.GetFileNameWithoutExtension(scriptFilePath));

          // Attribute.
          WriteCode($"[CustomEditor(typeof({className.Replace("Editor", string.Empty)}))]");

          // Begin class.
          WriteBeginBlock($"public class {className} : Editor");

          // Fields.
          bool addEmptyLine = WriteRegion("Fields.");
          WriteEndRegion();

          // Properties.
          addEmptyLine = WriteRegion("Properties.", addEmptyLine);
          WriteEndRegion();

          // Methods.
          addEmptyLine = WriteRegion("Methods.", addEmptyLine);
          WriteEndRegion();

          if (CodeWizardSettings.Instance.AddMonoBehaviourMethods == true)
            WriteCustomEditorMethods(addEmptyLine);

          // End class.
          WriteEndBlock();

          // End namespace.
          if (string.IsNullOrEmpty(CodeWizardSettings.Instance.Namespace) == false)
            WriteEndBlock();
        }
        catch (Exception ex)
        {
          throw new Exception($"[FronkonGames.CodeWizard] Failed to write file '{scriptFilePath}' : '{ex}'");
        }
        finally
        {
          AfterCreateFile(scriptFilePath);
        }
      }
    }

    private static void WriteCustomEditorMethods(bool addEmptyLine)
    {
      if (addEmptyLine == true)
        WriteEmptyLine();

      // Unity methods.
      WriteRegion("Editor methods.");

      // OnEnable().
      WriteCode("/// <summary>");
      WriteCode("/// Called when the object becomes enabled and active.");
      WriteCode("/// </summary>");
      WriteBeginBlock($"private void OnEnable{(CodeWizardSettings.Instance.SpaceBeforeBrace == true ? " " : string.Empty)}()");
      WriteEndBlock();

      WriteEmptyLine();

      // OnDisable().
      WriteCode("/// <summary>");
      WriteCode("/// Called when the behaviour becomes disabled or inactive.");
      WriteCode("/// </summary>");
      WriteBeginBlock($"private void OnDisable{(CodeWizardSettings.Instance.SpaceBeforeBrace == true ? " " : string.Empty)}()");
      WriteEndBlock();

      WriteEmptyLine();

      // OnInspectorGUI().
      WriteCode("/// <summary>");
      WriteCode("/// Implement this function to make a custom inspector.");
      WriteCode("/// </summary>");
      WriteBeginBlock($"public override void OnInspectorGUI{(CodeWizardSettings.Instance.SpaceBeforeBrace == true ? " " : string.Empty)}()");
      WriteEndBlock();

      WriteEndRegion();
    }
  }
}
