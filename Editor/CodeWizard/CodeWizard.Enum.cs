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
  /// <summary> Enum file template. </summary>
  public sealed partial class CodeWizard
  {
    /// <summary> Creates a Enum script. </summary>
    public static void CreateEnum(string fileName = default(string))
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

          // Begin enum.
          string enumName = FileNameToClassName(Path.GetFileNameWithoutExtension(scriptFilePath));
          WriteBeginBlock($"public enum {enumName}");

          // End enum.
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
  }
}

