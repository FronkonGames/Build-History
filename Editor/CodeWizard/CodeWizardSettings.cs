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
  /// <summary> Code Wizard settings. </summary>
  public sealed class CodeWizardSettings : ScriptableObject
  {
    #region Code Templates

    public string AuthorName
    {
      get => EditorPrefs.GetString(AuthorNameKey, "Unnamed");
      set => EditorPrefs.SetString(AuthorNameKey, value);
    }

    public string AuthorEmail
    {
      get => EditorPrefs.GetString(AuthorEmailKey, string.Empty);
      set => EditorPrefs.SetString(AuthorEmailKey, value);
    }

    public string AuthorWebsite
    {
      get => EditorPrefs.GetString(AuthorWebKey, string.Empty);
      set => EditorPrefs.SetString(AuthorWebKey, value);
    }

    public bool AskFilename
    {
      get => EditorPrefs.GetBool(AskFilenameKey, false);
      set => EditorPrefs.SetBool(AskFilenameKey, value);
    }

    public bool PingFile
    {
      get => EditorPrefs.GetBool(PingFileKey, false);
      set => EditorPrefs.SetBool(PingFileKey, value);
    }

    public bool SelectFile
    {
      get => EditorPrefs.GetBool(SelectFileKey, true);
      set => EditorPrefs.SetBool(SelectFileKey, value);
    }

    public string DefaultFilename
    {
      get => EditorPrefs.GetString(DefaultFilenameKey, "NewMonoBehaviour");
      set => EditorPrefs.SetString(DefaultFilenameKey, value);
    }

    public string CompanyName => companyName;
    public string CompanyWebsite => companyWebsite;
    public string CompanyEmail => companyEmail;
    public string CompanyLocation => companyLocation;
    public string CompanyAddress => companyAddress;

    public string ProductName => productName;

    public SoftwareLicenses License => license;
    public string CustomLicensePath { get { return customLicensePath; } set { customLicensePath = value; } }
    public bool HeaderHistory => headerHistory;
    public CodingStyles CodingStyle => codingStyle;
    public IndentStyles IndentStyle => indentStyle;
    public int SpaceCount => spaceCount;
    public bool SpaceBeforeBrace => spaceBeforeBrace;
    public string Namespace => codeNamespace;
    public bool UseRegions => useRegions;
    public bool AddMonoBehaviourMethods => addMonoBehaviourMethods;

    [Header("Company")]

    [SerializeField]
    private string companyName;

    [SerializeField]
    private string companyWebsite;

    [SerializeField]
    private string companyEmail;

    [SerializeField]
    private string companyLocation;

    [SerializeField]
    private string companyAddress;

    [Space(), Header("Product")]

    [SerializeField]
    private string productName;

    [SerializeField]
    private SoftwareLicenses license = SoftwareLicenses.NoLicense;

    [SerializeField]
    private string customLicensePath;

    [Space(), Header("Code")]

    [SerializeField]
    private CodingStyles codingStyle = CodingStyles.Spaces;

    [SerializeField]
    private IndentStyles indentStyle = IndentStyles.Allman;

    [SerializeField]
    private int spaceCount = 2;

    [SerializeField]
    private bool spaceBeforeBrace;

    [SerializeField]
    private string codeNamespace;

    [SerializeField]
    private bool headerHistory;

    [SerializeField]
    private bool useRegions;

    [SerializeField]
    private bool addMonoBehaviourMethods;

    #endregion

    // IActiveBuildTargetChanged.
    public int callbackOrder { get { return 0; } }

    public static CodeWizardSettings Instance
    {
      get
      {
        if (instance == null)
        {
          string settingsPath = string.Empty;

          string[] results = AssetDatabase.FindAssets("t:CodeWizardSettings", null);
          if (results.Length > 0)
            settingsPath = AssetDatabase.GUIDToAssetPath(results[0]);

          Debug.Log(settingsPath);

          if (string.IsNullOrEmpty(settingsPath) == false)
            instance = AssetDatabase.LoadAssetAtPath<CodeWizardSettings>(settingsPath);

          if (instance == null)
          {
            instance = CreateInstance<CodeWizardSettings>();

            AssetDatabase.CreateAsset(instance, Application.persistentDataPath);
            AssetDatabase.SaveAssets();
          }
        }

        return instance;
      }
    }

    private static CodeWizardSettings instance = null;

    private const string AuthorNameKey = "FronkonGames.CodeWizard.CodeTemplates.AuthorName";
    private const string AuthorWebKey = "FronkonGames.CodeWizard.CodeTemplates.AuthorWeb";
    private const string AuthorEmailKey = "FronkonGames.CodeWizard.CodeTemplates.AuthorEmail";

    private const string AskFilenameKey = "FronkonGames.CodeWizard.CodeTemplates.AskFilename";
    private const string PingFileKey = "FronkonGames.CodeWizard.CodeTemplates.PingFile";
    private const string SelectFileKey = "FronkonGames.CodeWizard.CodeTemplates.SelectFile";
    private const string DefaultFilenameKey = "FronkonGames.CodeWizard.CodeTemplates.DefaultFilename";
  }
}