// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AvalonStudio.Extensibility.Utils
{
    public partial class SemanticVersion
    {
        // Reusable set of empty release labels
        internal static readonly string[] EmptyReleaseLabels = new string[0];

        /// <summary>
        /// Parses a SemVer string using strict SemVer rules.
        /// </summary>
        public static SemanticVersion Parse(string value)
        {
            SemanticVersion ver = null;
            if (!TryParse(value, out ver))
            {
                throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, "{0} this is not a valie version string", value), nameof(value));
            }

            return ver;
        }

        /// <summary>
        /// Parse a version string
        /// </summary>
        /// <returns>false if the version is not a strict semver</returns>
        public static bool TryParse(string value, out SemanticVersion version)
        {
            version = null;

            if (value != null)
            {
                Version systemVersion = null;

                var sections = ParseSections(value);

                // null indicates the string did not meet the rules
                if (sections != null
                    && Version.TryParse(sections.Item1, out systemVersion))
                {
                    // validate the version string
                    var parts = sections.Item1.Split('.');

                    if (parts.Length != 3)
                    {
                        // versions must be 3 parts
                        return false;
                    }

                    foreach (var part in parts)
                    {
                        if (!IsValidPart(part, false))
                        {
                            // leading zeros are not allowed
                            return false;
                        }
                    }

                    // labels
                    if (sections.Item2 != null)
                    {
                        for (int i = 0; i < sections.Item2.Length; i++)
                        {
                            if (!IsValidPart(sections.Item2[i], allowLeadingZeros: false))
                            {
                                return false;
                            }
                        }
                    }

                    // build metadata
                    if (sections.Item3 != null
                        && !IsValid(sections.Item3, true))
                    {
                        return false;
                    }

                    var ver = NormalizeVersionValue(systemVersion);

                    version = new SemanticVersion(version: ver,
                        releaseLabels: sections.Item2,
                        metadata: sections.Item3 ?? string.Empty);

                    return true;
                }
            }

            return false;
        }

        internal static bool IsLetterOrDigitOrDash(char c)
        {
            var x = (int)c;

            // "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-"
            return (x >= 48 && x <= 57) || (x >= 65 && x <= 90) || (x >= 97 && x <= 122) || x == 45;
        }

        internal static bool IsDigit(char c)
        {
            var x = (int)c;

            // "0123456789"
            return (x >= 48 && x <= 57);
        }

        internal static bool IsValid(string s, bool allowLeadingZeros)
        {
            var parts = s.Split('.');

            // Check each part individually
            for (int i = 0; i < parts.Length; i++)
            {
                if (!IsValidPart(parts[i], allowLeadingZeros))
                {
                    return false;
                }
            }

            return true;
        }

        internal static bool IsValidPart(string s, bool allowLeadingZeros)
        {
            if (s.Length == 0)
            {
                // empty labels are not allowed
                return false;
            }

            // 0 is fine, but 00 is not. 
            // 0A counts as an alpha numeric string where zeros are not counted
            if (!allowLeadingZeros
                && s.Length > 1
                && s[0] == '0')
            {
                var allDigits = true;

                // Check if all characters are digits.
                // The first is already checked above
                for (int i = 1; i < s.Length; i++)
                {
                    if (!IsDigit(s[i]))
                    {
                        allDigits = false;
                        break;
                    }
                }

                if (allDigits)
                {
                    // leading zeros are not allowed in numeric labels
                    return false;
                }
            }

            for (int i = 0; i < s.Length; i++)
            {
                // Verify that the part contains only allowed characters
                if (!IsLetterOrDigitOrDash(s[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Parse the version string into version/release/build
        /// The goal of this code is to take the most direct and optimized path
        /// to parsing and validating a semver. Regex would be much cleaner, but
        /// due to the number of versions created in NuGet Regex is too slow.
        /// </summary>
        internal static Tuple<string, string[], string> ParseSections(string value)
        {
            string versionString = null;
            string[] releaseLabels = null;
            string buildMetadata = null;

            var dashPos = -1;
            var plusPos = -1;

            var end = false;
            for (var i = 0; i < value.Length; i++)
            {
                end = (i == value.Length - 1);

                if (dashPos < 0)
                {
                    if (end
                        || value[i] == '-'
                        || value[i] == '+')
                    {
                        var endPos = i + (end ? 1 : 0);
                        versionString = value.Substring(0, endPos);

                        dashPos = i;

                        if (value[i] == '+')
                        {
                            plusPos = i;
                        }
                    }
                }
                else if (plusPos < 0)
                {
                    if (end || value[i] == '+')
                    {
                        var start = dashPos + 1;
                        var endPos = i + (end ? 1 : 0);
                        var releaseLabel = value.Substring(start, endPos - start);

                        releaseLabels = releaseLabel.Split('.');

                        plusPos = i;
                    }
                }
                else if (end)
                {
                    var start = plusPos + 1;
                    var endPos = i + (end ? 1 : 0);
                    buildMetadata = value.Substring(start, endPos - start);
                }
            }

            return new Tuple<string, string[], string>(versionString, releaseLabels, buildMetadata);
        }

        internal static Version NormalizeVersionValue(Version version)
        {
            var normalized = version;

            if (version.Build < 0
                || version.Revision < 0)
            {
                normalized = new Version(
                    version.Major,
                    version.Minor,
                    Math.Max(version.Build, 0),
                    Math.Max(version.Revision, 0));
            }

            return normalized;
        }

        private static string[] ParseReleaseLabels(string releaseLabels)
        {
            if (!string.IsNullOrEmpty(releaseLabels))
            {
                return releaseLabels.Split('.');
            }

            return null;
        }
    }

    /// <summary>
    /// A strict SemVer implementation
    /// </summary>
    public partial class SemanticVersion
    {
        // store as array to avoid enumerator allocations
        internal readonly string[] _releaseLabels;
        internal readonly string _metadata;
        internal readonly Version _version;

        /// <summary>
        /// Creates a SemanticVersion from an existing SemanticVersion
        /// </summary>
        /// <param name="version">Version to clone.</param>
        public SemanticVersion(SemanticVersion version)
            : this(version.Major, version.Minor, version.Patch, version.ReleaseLabels, version.Metadata)
        {
        }

        /// <summary>
        /// Creates a SemanticVersion X.Y.Z
        /// </summary>
        /// <param name="major">X.y.z</param>
        /// <param name="minor">x.Y.z</param>
        /// <param name="patch">x.y.Z</param>
        public SemanticVersion(int major, int minor, int patch)
            : this(major, minor, patch, Enumerable.Empty<string>(), null)
        {
        }

        /// <summary>
        /// Creates a NuGetVersion X.Y.Z-alpha
        /// </summary>
        /// <param name="major">X.y.z</param>
        /// <param name="minor">x.Y.z</param>
        /// <param name="patch">x.y.Z</param>
        /// <param name="releaseLabel">Prerelease label</param>
        public SemanticVersion(int major, int minor, int patch, string releaseLabel)
            : this(major, minor, patch, ParseReleaseLabels(releaseLabel), null)
        {
        }

        /// <summary>
        /// Creates a NuGetVersion X.Y.Z-alpha#build01
        /// </summary>
        /// <param name="major">X.y.z</param>
        /// <param name="minor">x.Y.z</param>
        /// <param name="patch">x.y.Z</param>
        /// <param name="releaseLabel">Prerelease label</param>
        /// <param name="metadata">Build metadata</param>
        public SemanticVersion(int major, int minor, int patch, string releaseLabel, string metadata)
            : this(major, minor, patch, ParseReleaseLabels(releaseLabel), metadata)
        {
        }

        /// <summary>
        /// Creates a NuGetVersion X.Y.Z-alpha.1.2#build01
        /// </summary>
        /// <param name="major">X.y.z</param>
        /// <param name="minor">x.Y.z</param>
        /// <param name="patch">x.y.Z</param>
        /// <param name="releaseLabels">Release labels that have been split by the dot separator</param>
        /// <param name="metadata">Build metadata</param>
        public SemanticVersion(int major, int minor, int patch, IEnumerable<string> releaseLabels, string metadata)
            : this(new Version(major, minor, patch, 0), releaseLabels, metadata)
        {
        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="version">Version</param>
        /// <param name="releaseLabel">Full release label</param>
        /// <param name="metadata">Build metadata</param>
        protected SemanticVersion(Version version, string releaseLabel = null, string metadata = null)
            : this(version, ParseReleaseLabels(releaseLabel), metadata)
        {
        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="major">X.y.z</param>
        /// <param name="minor">x.Y.z</param>
        /// <param name="patch">x.y.Z</param>
        /// <param name="revision">x.y.z.R</param>
        /// <param name="releaseLabel">Prerelease label</param>
        /// <param name="metadata">Build metadata</param>
        protected SemanticVersion(int major, int minor, int patch, int revision, string releaseLabel, string metadata)
            : this(major, minor, patch, revision, ParseReleaseLabels(releaseLabel), metadata)
        {
        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        /// <param name="patch"></param>
        /// <param name="revision"></param>
        /// <param name="releaseLabels"></param>
        /// <param name="metadata"></param>
        protected SemanticVersion(int major, int minor, int patch, int revision, IEnumerable<string> releaseLabels, string metadata)
            : this(new Version(major, minor, patch, revision), releaseLabels, metadata)
        {
        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="version">Version</param>
        /// <param name="releaseLabels">Release labels</param>
        /// <param name="metadata">Build metadata</param>
        protected SemanticVersion(Version version, IEnumerable<string> releaseLabels, string metadata)
        {
            if (version == null)
            {
                throw new ArgumentNullException(nameof(version));
            }

            _version = NormalizeVersionValue(version);
            _metadata = metadata;

            if (releaseLabels != null)
            {
                // If the labels are already an array use it
                var asArray = releaseLabels as string[];

                if (asArray != null)
                {
                    _releaseLabels = asArray;
                }
                else
                {
                    // enumerate the list
                    _releaseLabels = releaseLabels.ToArray();
                }

                if (_releaseLabels.Length < 1)
                {
                    // Avoid storing an empty array of labels, this field is checked for null everywhere
                    _releaseLabels = null;
                }
            }
        }

        /// <summary>
        /// Major version X (X.y.z)
        /// </summary>
        public int Major
        {
            get { return _version.Major; }
        }

        /// <summary>
        /// Minor version Y (x.Y.z)
        /// </summary>
        public int Minor
        {
            get { return _version.Minor; }
        }

        /// <summary>
        /// Patch version Z (x.y.Z)
        /// </summary>
        public int Patch
        {
            get { return _version.Build; }
        }

        /// <summary>
        /// A collection of pre-release labels attached to the version.
        /// </summary>
        public IEnumerable<string> ReleaseLabels
        {
            get { return _releaseLabels ?? EmptyReleaseLabels; }
        }

        /// <summary>
        /// The full pre-release label for the version.
        /// </summary>
        public string Release
        {
            get
            {
                if (_releaseLabels != null)
                {
                    if (_releaseLabels.Length == 1)
                    {
                        // There is exactly 1 label
                        return _releaseLabels[0];
                    }
                    else
                    {
                        // Join all labels
                        return string.Join(".", _releaseLabels);
                    }
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// True if pre-release labels exist for the version.
        /// </summary>
        public virtual bool IsPrerelease
        {
            get
            {
                if (_releaseLabels != null)
                {
                    for (int i = 0; i < _releaseLabels.Length; i++)
                    {
                        if (!string.IsNullOrEmpty(_releaseLabels[i]))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// True if metadata exists for the version.
        /// </summary>
        public virtual bool HasMetadata
        {
            get { return !string.IsNullOrEmpty(Metadata); }
        }

        /// <summary>
        /// Build metadata attached to the version.
        /// </summary>
        public virtual string Metadata
        {
            get { return _metadata; }
        }
    }
}