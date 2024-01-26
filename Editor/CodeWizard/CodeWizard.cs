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
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace FronkonGames.Tools.CodeWizard
{
  /// <summary> Code Wizard. </summary>
  public sealed partial class CodeWizard
  {
    private static int indentLevel = 0;

    private static StreamWriter streamWriter = null;

    private static void OpenStream(string filePath)
    {
      CloseStream();

      streamWriter = File.CreateText(filePath);

      indentLevel = 0;
    }

    private static void CloseStream()
    {
      if (streamWriter != null)
      {
        streamWriter.Close();
        streamWriter.Dispose();
      }
    }

    private static void WriteHeader()
    {
      if (CodeWizardSettings.Instance.License != SoftwareLicenses.NoLicense)
      {
        string filePath = string.Empty;

        if (CodeWizardSettings.Instance.License == SoftwareLicenses.Custom && string.IsNullOrEmpty(CodeWizardSettings.Instance.CustomLicensePath) == false)
          filePath = $"{Application.dataPath}{CodeWizardSettings.Instance.CustomLicensePath}";
        else
        {
          string[] guids = AssetDatabase.FindAssets("t:TextAsset");
          for (int i = 0; i < guids.Length; ++i)
          {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            path = path.Replace("\\", "/");

            if (path.Contains($"CodeWizard/Licenses/{CodeWizardSettings.Instance.License}.txt") == true)
            {
              filePath = $"{Application.dataPath}/{path.Replace("Assets/", string.Empty)}";
              break;
            }
          }

          if (string.IsNullOrEmpty(filePath) == true)
            Debug.LogWarning($"[FronkonGames.CodeWizard] Text file for license '{CodeWizardSettings.Instance.License}' not found. Please reinstall the asset.");
        }

        if (string.IsNullOrEmpty(filePath) == false)
        {
          string[] stringLicense = new string[0];

          if (File.Exists(filePath) == true)
          {
            stringLicense = File.ReadAllLines(filePath);

            for (int i = 0; i < stringLicense.Length; ++i)
              stringLicense[i] = $"// {HeaderKeywords(stringLicense[i])}";
          }

          if (stringLicense.Length > 0)
          {
            streamWriter.WriteLine("///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////");

            for (int i = 0; i < stringLicense.Length; ++i)
              streamWriter.WriteLine(stringLicense[i]);

            streamWriter.WriteLine("///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////");
          }
        }
      }
    }

    private static void WriteHistory()
    {
      if (CodeWizardSettings.Instance.HeaderHistory == true)
      {
        if (CodeWizardSettings.Instance.License == SoftwareLicenses.NoLicense)
          streamWriter.WriteLine("///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////");

        streamWriter.WriteLine("// Name                          Date                Description");
        streamWriter.WriteLine("// --------------------------------------------------------------------------------------------------------------------------------");

        string authorName = CodeWizardSettings.Instance.AuthorName;
        if (authorName.Length > 30)
          authorName = authorName[..30];

        string creationDate = DateTime.Now.ToString("d", CultureInfo.CurrentCulture);

        streamWriter.WriteLine("// {0}{1}{2}{3}Created.", authorName, new string(' ', 30 - authorName.Length), creationDate, new string(' ', 20 - creationDate.Length));

        streamWriter.WriteLine("///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////");
      }
    }

    private static void WriteUsings()
    {
      streamWriter.WriteLine("using System;");
      WriteEmptyLine();

      streamWriter.WriteLine("using UnityEngine;");
      WriteEmptyLine();
    }

    private static void WriteEmptyLine()
    {
      streamWriter.WriteLine(string.Empty);
    }

    private static bool WriteRegion(string region, bool addEmptyLine = false)
    {
      if (CodeWizardSettings.Instance.UseRegions == true)
      {
        if (addEmptyLine == true)
          WriteEmptyLine();

        streamWriter.WriteLine($"{IndentToString(indentLevel)}#region {region}");
      }

      return CodeWizardSettings.Instance.UseRegions;
    }

    private static void WriteEndRegion()
    {
      if (CodeWizardSettings.Instance.UseRegions == true)
        streamWriter.WriteLine($"{IndentToString(indentLevel)}#endregion");
    }

    private static void WriteCode(string code)
    {
      streamWriter.WriteLine($"{IndentToString(indentLevel)}{code}");
    }

    private static void WriteBeginBlock(string code)
    {
      string stringIndent = IndentToString(indentLevel);

      if (CodeWizardSettings.Instance.IndentStyle == IndentStyles.K_AND_R)
        streamWriter.WriteLine($"{stringIndent}{code} {{");
      else
      {
        streamWriter.WriteLine($"{stringIndent}{code}");
        streamWriter.WriteLine($"{stringIndent}{{");
      }

      indentLevel++;
    }

    private static void WriteEndBlock()
    {
      indentLevel--;

      streamWriter.WriteLine($"{IndentToString(indentLevel)}}}");
    }

    private static string HeaderKeywords(string license)
    {
      // Time.
      license = license.Replace("{SHORT_DATE}", DateTime.Now.ToShortDateString());

      license = license.Replace("{LONG_DATE}", DateTime.Now.ToLongDateString());

      license = license.Replace("{SHORT_TIME}", DateTime.Now.ToShortTimeString());

      license = license.Replace("{LONG_TIME}", DateTime.Now.ToLongTimeString());

      license = license.Replace("{YEAR}", DateTime.Now.Year.ToString());

      license = license.Replace("{DAY}", DateTime.Now.DayOfWeek.ToString());

      // Company.
      if (string.IsNullOrEmpty(CodeWizardSettings.Instance.CompanyName) == false)
        license = license.Replace("{COMPANY_NAME}", CodeWizardSettings.Instance.CompanyName);
      else if (string.IsNullOrEmpty(CodeWizardSettings.Instance.AuthorName) == false)       // If companyName is empty, try with authorName.
        license = license.Replace("{AUTHOR_NAME}", CodeWizardSettings.Instance.AuthorName);

      if (string.IsNullOrEmpty(CodeWizardSettings.Instance.CompanyWebsite) == false)
        license = license.Replace("{COMPANY_WEB}", CodeWizardSettings.Instance.CompanyWebsite);
      else if (string.IsNullOrEmpty(CodeWizardSettings.Instance.AuthorWebsite) == false)       // If companyWeb is empty, try with AuthorWeb.
        license = license.Replace("{AUTHOR_WEB}", CodeWizardSettings.Instance.AuthorWebsite);

      if (string.IsNullOrEmpty(CodeWizardSettings.Instance.CompanyEmail) == false)
        license = license.Replace("{COMPANY_EMAIL}", CodeWizardSettings.Instance.CompanyEmail);
      else if (string.IsNullOrEmpty(CodeWizardSettings.Instance.AuthorEmail) == false)     // If companyEmail is empty, try with AuthorEmail.
        license = license.Replace("{AUTHOR_EMAIL}", CodeWizardSettings.Instance.AuthorEmail);

      if (string.IsNullOrEmpty(CodeWizardSettings.Instance.CompanyLocation) == false)
        license = license.Replace("{COMPANY_LOCATION}", CodeWizardSettings.Instance.CompanyLocation);

      if (string.IsNullOrEmpty(CodeWizardSettings.Instance.CompanyAddress) == false)
        license = license.Replace("{COMPANY_ADDRESS}", CodeWizardSettings.Instance.CompanyAddress);

      // Product.
      if (string.IsNullOrEmpty(CodeWizardSettings.Instance.ProductName) == false)
        license = license.Replace("{PRODUCT_NAME}", CodeWizardSettings.Instance.ProductName);

      // Author.
      if (string.IsNullOrEmpty(CodeWizardSettings.Instance.AuthorName) == false)
        license = license.Replace("{AUTHOR_NAME}", CodeWizardSettings.Instance.AuthorName);

      if (string.IsNullOrEmpty(CodeWizardSettings.Instance.AuthorWebsite) == false)
        license = license.Replace("{AUTHOR_WEB}", CodeWizardSettings.Instance.AuthorWebsite);

      if (string.IsNullOrEmpty(CodeWizardSettings.Instance.AuthorEmail) == false)
        license = license.Replace("{AUTHOR_EMAIL}", CodeWizardSettings.Instance.AuthorEmail);

      // Misc.
      string currentFile = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();
      if (string.IsNullOrEmpty(currentFile) == false)
        license = license.Replace("{FILENAME}", Path.GetFileName(currentFile));

      return license;
    }

    private static string IndentToString(int level)
    {
      string stringIndent = string.Empty;

      if (level > 0)
      {
        if (CodeWizardSettings.Instance.CodingStyle == CodingStyles.Spaces)
          stringIndent = new string(' ', level * CodeWizardSettings.Instance.SpaceCount);
        else
          stringIndent = new string('\t', level);
      }

      return stringIndent;
    }

    private static string FileNameToClassName(string fileName)
    {
      Regex regex = new(@"[^\p{Ll}\p{Lu}\p{Lt}\p{Lo}\p{Nd}\p{Nl}\p{Mn}\p{Mc}\p{Cf}\p{Pc}\p{Lm}]");

      string className = regex.Replace(fileName, string.Empty);

      if (char.IsLetter(className, 0) == false)
        className = className.Insert(0, "_");

      return className.Replace(" ", string.Empty);
    }

    private static string GetSelectedPath()
    {
      string path = string.Empty;

      foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
      {
        path = AssetDatabase.GetAssetPath(obj);
        if (string.IsNullOrEmpty(path) == false && File.Exists(path))
        {
          path = Path.GetDirectoryName(path);

          break;
        }
      }

      return path.Replace("\\", "/");
    }

    private static void AfterCreateFile(string scriptFilePath)
    {
      CloseStream();

      AssetDatabase.Refresh();

      if (CodeWizardSettings.Instance.SelectFile == true || CodeWizardSettings.Instance.PingFile == true)
      {
        string assetPath = $"Assets{scriptFilePath.Replace(Application.dataPath, string.Empty)}";
        UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(MonoScript));
        if (asset != null)
        {
          if (CodeWizardSettings.Instance.SelectFile == true)
            Selection.activeObject = asset;

          if (CodeWizardSettings.Instance.PingFile == true)
            EditorGUIUtility.PingObject(asset);
        }
      }
    }
  }
}
