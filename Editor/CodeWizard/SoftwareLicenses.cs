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

namespace FronkonGames.Tools.CodeWizard
{
  /// <summary> Software licenses. </summary>
  public enum SoftwareLicenses
  {
    /// <summary> Make your own license. </summary>
    Custom,

    /// <summary> No license. </summary>
    NoLicense,

    /// <summary>
    /// Academic Free License 3.0.
    ///
    /// Linking:          Permissive.
    /// Distribution:     ?
    /// Modification:     Permissive.
    /// Patent grant:     ?
    /// Private use:      ?
    /// Sublicensing:     ?
    /// Trademark grant:  ?
    /// </summary>
    AcademicFreeLicense,

    /// <summary>
    /// Apache License 2.0.
    ///
    /// Linking:          Permissive.
    /// Distribution:     Permissive.
    /// Modification:     Permissive.
    /// Patent grant:     Yes.
    /// Private use:      Yes.
    /// Sublicensing:     Permissive.
    /// Trademark grant:  No.
    /// </summary>
    ApacheLicense,

    /// <summary>
    /// Apple Public Source License 2.0.
    ///
    /// Linking:          Permissive.
    /// Distribution:     ?
    /// Modification:     Limited.
    /// Patent grant:     ?
    /// Private use:      ?
    /// Sublicensing:     ?
    /// Trademark grant:  ?
    /// </summary>
    ApplePublicSourceLicense,

    /// <summary>
    /// Artistic License 2.0.
    ///
    /// Linking:          With restrictions.
    /// Distribution:     ?
    /// Modification:     With restrictions.
    /// Patent grant:     ?
    /// Private use:      ?
    /// Sublicensing:     ?
    /// Trademark grant:  ?
    /// </summary>
    ArtisticLicense,

    /// <summary>
    /// Beerware License revision 42.
    ///
    /// Linking:          ?.
    /// Distribution:     ?
    /// Modification:     ?.
    /// Patent grant:     ?
    /// Private use:      ?
    /// Sublicensing:     ?
    /// Trademark grant:  ?
    /// </summary>
    Beerware,

    /// <summary>
    /// Boost Software License 1.0.
    ///
    /// Linking:          Permissive.
    /// Distribution:     ?
    /// Modification:     Permissive.
    /// Patent grant:     ?
    /// Private use:      ?
    /// Sublicensing:     ?
    /// Trademark grant:  ?
    /// </summary>
    BoostSoftwareLicense,

    /// <summary>
    /// BSD License with "advertising clause".
    ///
    /// Linking:          Permissive.
    /// Distribution:     Permissive.
    /// Modification:     Permissive.
    /// Patent grant:     Manually.
    /// Private use:      Yes.
    /// Sublicensing:     Permissive.
    /// Trademark grant:  Manually.
    /// </summary>
    BSDLicense,

    /// <summary>
    /// BSD License 2.0.
    ///
    /// Linking:          Permissive.
    /// Distribution:     Permissive.
    /// Modification:     Permissive.
    /// Patent grant:     Manually.
    /// Private use:      Yes.
    /// Sublicensing:     Permissive.
    /// Trademark grant:  Manually.
    /// </summary>
    BSDNewLicense,

    /// <summary>
    /// Simplified BSD License.
    ///
    /// Linking:          Permissive.
    /// Distribution:     Permissive.
    /// Modification:     Permissive.
    /// Patent grant:     Manually.
    /// Private use:      Yes.
    /// Sublicensing:     Permissive.
    /// Trademark grant:  Manually.
    /// </summary>
    BSDSimpleLicense,

    /// <summary>
    /// Common Development and Distribution License 1.0.
    ///
    /// Linking:          Permissive.
    /// Distribution:     ?
    /// Modification:     Limited.
    /// Patent grant:     ?
    /// Private use:      ?
    /// Sublicensing:     ?
    /// Trademark grant:  ?
    /// </summary>
    CommonDevelopmentAndDistributionLicense,

    /// <summary>
    /// Common Public License 1.0.
    ///
    /// Linking:          Permissive.
    /// Distribution:     ?
    /// Modification:     Copylefted.
    /// Patent grant:     ?
    /// Private use:      ?
    /// Sublicensing:     ?
    /// Trademark grant:  ?
    /// </summary>
    CommonPublicLicense,

    /// <summary>
    /// Creative Commons Attribution 4.0 International.
    ///
    /// Linking:          Permissive.
    /// Distribution:     Permissive.
    /// Modification:     Permissive.
    /// Patent grant:     No.
    /// Private use:      Yes.
    /// Sublicensing:     Permissive.
    /// Trademark grant:  ?
    /// </summary>
    CreativeCommons_BY,

    /// <summary>
    /// Creative Commons Attribution + ShareAlike 4.0 International.
    ///
    /// Linking:          Copylefted.
    /// Distribution:     Copylefted.
    /// Modification:     Copylefted.
    /// Patent grant:     No.
    /// Private use:      Yes.
    /// Sublicensing:     No.
    /// Trademark grant:  ?
    /// </summary>
    CreativeCommons_BY_SA,

    /// <summary>
    /// Creative Commons Zero 1.0.
    ///
    /// Linking:          Public domain.
    /// Distribution:     Public domain.
    /// Modification:     Public domain.
    /// Patent grant:     No.
    /// Private use:      Public domain.
    /// Sublicensing:     Public domain.
    /// Trademark grant:  No.
    /// </summary>
    CreativeCommonsZero,

    /// <summary>
    /// Do What The Fuck You Want To Public License.
    ///
    /// Linking:          Permissive / Public domain.
    /// Distribution:     ?
    /// Modification:     Permissive / Public domain
    /// Patent grant:     ?
    /// Private use:      ?
    /// Sublicensing:     ?
    /// Trademark grant:  ?
    /// </summary>
    DoWhatTheFuckYouWantToPublicLicense,

    /// <summary>
    /// Eclipse Public License 1.0.
    ///
    /// Linking:          Limited.
    /// Distribution:     Limited.
    /// Modification:     Limited.
    /// Patent grant:     Yes.
    /// Private use:      Yes.
    /// Sublicensing:     Limited.
    /// Trademark grant:  Manually.
    /// </summary>
    EclipsePublicLicense,

    /// <summary>
    /// Educational Community License 2.0.
    ///
    /// Linking:          Permissive.
    /// Distribution:     ?
    /// Modification:     Permissive.
    /// Patent grant:     ?
    /// Private use:      ?
    /// Sublicensing:     ?
    /// Trademark grant:  ?
    /// </summary>
    EducationalCommunityLicense,

    /// <summary>
    /// Eiffel Forum License 2.0.
    ///
    /// Linking:          ?
    /// Distribution:     ?
    /// Modification:     ?
    /// Patent grant:     ?
    /// Private use:      ?
    /// Sublicensing:     ?
    /// Trademark grant:  ?
    /// </summary>
    EiffelForumLicense,

    /// <summary>
    /// European Union Public License 1.1.
    ///
    /// Linking:          Limited.
    /// Distribution:     ?
    /// Modification:     With an explicit compatibility list.
    /// Patent grant:     ?
    /// Private use:      ?
    /// Sublicensing:     ?
    /// Trademark grant:  ?
    /// </summary>
    EuropeanUnionPublicLicense,

    /// <summary>
    /// GNU Affero General Public License 3.0.
    ///
    /// Linking:          Copylefted.
    /// Distribution:     Copyleft except for the GNU AGPL.
    /// Modification:     Copyleft.
    /// Patent grant:     ?
    /// Private use:      Yes.
    /// Sublicensing:     ?
    /// Trademark grant:  ?
    /// </summary>
    GNUAfferoGeneralPublicLicense,

    /// <summary>
    /// GNU General Public License 3.0.
    ///
    /// Linking:          GPLv3 compatible only.
    /// Distribution:     Copylefted.
    /// Modification:     Copylefted.
    /// Patent grant:     Yes.
    /// Private use:      Yes.
    /// Sublicensing:     Copylefted.
    /// Trademark grant:  Yes.
    /// </summary>
    GNUGeneralPublicLicense,

    /// <summary>
    /// GNU Lesser General Public License 3.0.
    ///
    /// Linking:          With restrictions.
    /// Distribution:     Copylefted.
    /// Modification:     Copylefted.
    /// Patent grant:     Yes.
    /// Private use:      Yes.
    /// Sublicensing:     Copylefted.
    /// Trademark grant:  Yes.
    /// </summary>
    GNULesserGeneralPublicLicense,

    /// <summary>
    /// IBM Public License.
    ///
    /// Linking:          Copylefted.
    /// Distribution:     ?
    /// Modification:     Copylefted.
    /// Patent grant:     ?
    /// Private use:      ?
    /// Sublicensing:     ?
    /// Trademark grant:  ?
    /// </summary>
    IBMPublicLicense,

    /// <summary>
    /// Intel Open Source License.
    ///
    /// Linking:          ?
    /// Distribution:     ?
    /// Modification:     ?
    /// Patent grant:     ?
    /// Private use:      ?
    /// Sublicensing:     ?
    /// Trademark grant:  ?
    /// </summary>
    IntelOpenSourceLicense,

    /// <summary>
    /// ISC License.
    ///
    /// Linking:          Permissive.
    /// Distribution:     Permissive.
    /// Modification:     Permissive.
    /// Patent grant:     ?
    /// Private use:      ?
    /// Sublicensing:     ?
    /// Trademark grant:  ?
    /// </summary>
    ISCLicense,

    /// <summary>
    /// The MIT License.
    ///
    /// Linking:          Permissive.
    /// Distribution:     Permissive.
    /// Modification:     Permissive.
    /// Patent grant:     Manually.
    /// Private use:      Yes.
    /// Sublicensing:     Permissive.
    /// Trademark grant:  Manually.
    /// </summary>
    MITLicense,

    /// <summary>
    /// Mozilla Public License.
    ///
    /// Linking:          Permissive.
    /// Distribution:     Copylefted.
    /// Modification:     Copylefted.
    /// Patent grant:     Yes.
    /// Private use:      Yes.
    /// Sublicensing:     Copylefted.
    /// Trademark grant:  No.
    /// </summary>
    MozillaPublicLicense,

    /// <summary>
    /// Netscape Public License.
    ///
    /// Linking:          ?
    /// Distribution:     Limited.
    /// Modification:     ?
    /// Patent grant:     Limited.
    /// Private use:      ?
    /// Sublicensing:     ?
    /// Trademark grant:  ?
    /// </summary>
    NetscapePublicLicense,

    /// <summary>
    /// Open Software License.
    ///
    /// Linking:          Permissive.
    /// Distribution:     ?
    /// Modification:     Copylefted.
    /// Patent grant:     ?
    /// Private use:      ?
    /// Sublicensing:     ?
    /// Trademark grant:  ?
    /// </summary>
    OpenSoftwareLicense,

    /// <summary>
    /// OpenSSL License.
    ///
    /// Linking:          Permissive.
    /// Distribution:     ?
    /// Modification:     Permissive.
    /// Patent grant:     ?
    /// Private use:      ?
    /// Sublicensing:     ?
    /// Trademark grant:  ?
    /// </summary>
    OpenSSLLicense,

    /// <summary>
    /// PHP License.
    ///
    /// Linking:          ?
    /// Distribution:     ?
    /// Modification:     ?
    /// Patent grant:     ?
    /// Private use:      ?
    /// Sublicensing:     ?
    /// Trademark grant:  ?
    /// </summary>
    PHPLicense,

    /// <summary>
    /// Python Software Foundation License.
    ///
    /// Linking:          Permissive.
    /// Distribution:     ?
    /// Modification:     Permissive.
    /// Patent grant:     ?
    /// Private use:      ?
    /// Sublicensing:     ?
    /// Trademark grant:  ?
    /// </summary>
    PythonSoftwareFoundationLicense,

    /// <summary>
    /// Unlicense.org.
    ///
    /// Linking:          Permissive / Public domain.
    /// Distribution:     Permissive / Public domain.
    /// Modification:     Permissive / Public domain.
    /// Patent grant:     ?
    /// Private use:      Permissive / Public domain.
    /// Sublicensing:     Permissive / Public domain.
    /// Trademark grant:  ?
    /// </summary>
    Unlicense,

    /// <summary>
    /// XFree86 1.1 License.
    ///
    /// Linking:          Permissive.
    /// Distribution:     ?
    /// Modification:     Permissive.
    /// Patent grant:     ?
    /// Private use:      ?
    /// Sublicensing:     ?
    /// Trademark grant:  ?
    /// </summary>
    XFree86License,

    /// <summary>
    /// The zlib/libpng License.
    ///
    /// Linking:          Permissive.
    /// Distribution:     ?
    /// Modification:     Permissive.
    /// Patent grant:     ?
    /// Private use:      ?
    /// Sublicensing:     ?
    /// Trademark grant:  ?
    /// </summary>
    ZLIBLicense,

    /// <summary>
    /// Zope Public License.
    ///
    /// Linking:          ?
    /// Distribution:     ?
    /// Modification:     ?
    /// Patent grant:     ?
    /// Private use:      ?
    /// Sublicensing:     ?
    /// Trademark grant:  ?
    /// </summary>
    ZopePublicLicense,
  }
}

