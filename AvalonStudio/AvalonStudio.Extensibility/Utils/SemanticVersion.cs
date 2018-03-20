// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.DotNet.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AvalonStudio.Extensibility.Utils
{
    /// <summary>
    /// Version comparison modes.
    /// </summary>
    public enum VersionComparison
    {
        /// <summary>
        /// Semantic version 2.0.1-rc comparison with additional compares for extra NuGetVersion fields.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Compares only the version numbers.
        /// </summary>
        Version = 1,

        /// <summary>
        /// Include Version number and Release labels in the compare.
        /// </summary>
        VersionRelease = 2,

        /// <summary>
        /// Include all metadata during the compare.
        /// </summary>
        VersionReleaseMetadata = 3
    }


    /// <summary>
    /// IVersionComparer represents a version comparer capable of sorting and determining the equality of
    /// SemanticVersion objects.
    /// </summary>
    public interface IVersionComparer : IEqualityComparer<SemanticVersion>, IComparer<SemanticVersion>
    {
    }

    public sealed class VersionComparer : IVersionComparer
    {
        private readonly VersionComparison _mode;

        /// <summary>
        /// Creates a VersionComparer using the default mode.
        /// </summary>
        public VersionComparer()
        {
            _mode = VersionComparison.Default;
        }

        /// <summary>
        /// Creates a VersionComparer that respects the given comparison mode.
        /// </summary>
        /// <param name="versionComparison">comparison mode</param>
        public VersionComparer(VersionComparison versionComparison)
        {
            _mode = versionComparison;
        }

        /// <summary>
        /// Determines if both versions are equal.
        /// </summary>
        public bool Equals(SemanticVersion x, SemanticVersion y)
        {
            if (ReferenceEquals(x, y))
            {
                return true;
            }

            if (ReferenceEquals(y, null))
            {
                return false;
            }

            if (ReferenceEquals(x, null))
            {
                return false;
            }

            if (_mode == VersionComparison.Default || _mode == VersionComparison.VersionRelease)
            {
                // Compare the version and release labels
                return (x.Major == y.Major
                    && x.Minor == y.Minor
                    && x.Patch == y.Patch
                    && GetRevisionOrZero(x) == GetRevisionOrZero(y)
                    && AreReleaseLabelsEqual(x, y));
            }

            // Use the full comparer for non-default scenarios
            return Compare(x, y) == 0;
        }

        /// <summary>
        /// Compares the given versions using the VersionComparison mode.
        /// </summary>
        public static int Compare(SemanticVersion version1, SemanticVersion version2, VersionComparison versionComparison)
        {
            IVersionComparer comparer = new VersionComparer(versionComparison);
            return comparer.Compare(version1, version2);
        }

        /// <summary>
        /// Compare versions.
        /// </summary>
        public int Compare(SemanticVersion x, SemanticVersion y)
        {
            if (ReferenceEquals(x, y))
            {
                return 0;
            }

            if (ReferenceEquals(y, null))
            {
                return 1;
            }

            if (ReferenceEquals(x, null))
            {
                return -1;
            }

            // compare version
            var result = x.Major.CompareTo(y.Major);
            if (result != 0)
            {
                return result;
            }

            result = x.Minor.CompareTo(y.Minor);
            if (result != 0)
            {
                return result;
            }

            result = x.Patch.CompareTo(y.Patch);
            if (result != 0)
            {
                return result;
            }

            if (_mode != VersionComparison.Version)
            {
                // compare release labels
                var xLabels = GetReleaseLabelsOrNull(x);
                var yLabels = GetReleaseLabelsOrNull(y);

                if (xLabels != null
                    && yLabels == null)
                {
                    return -1;
                }

                if (xLabels == null
                    && yLabels != null)
                {
                    return 1;
                }

                if (xLabels != null
                    && yLabels != null)
                {
                    result = CompareReleaseLabels(xLabels, yLabels);
                    if (result != 0)
                    {
                        return result;
                    }
                }

                // compare the metadata
                if (_mode == VersionComparison.VersionReleaseMetadata)
                {
                    result = StringComparer.OrdinalIgnoreCase.Compare(x.Metadata ?? string.Empty, y.Metadata ?? string.Empty);
                    if (result != 0)
                    {
                        return result;
                    }
                }
            }

            return 0;
        }

        /// <summary>
        /// A default comparer that compares metadata as strings.
        /// </summary>
        public static readonly IVersionComparer Default = new VersionComparer(VersionComparison.Default);

        /// <summary>
        /// A comparer that uses only the version numbers.
        /// </summary>
        public static readonly IVersionComparer Version = new VersionComparer(VersionComparison.Version);

        /// <summary>
        /// Compares versions without comparing the metadata.
        /// </summary>
        public static readonly IVersionComparer VersionRelease = new VersionComparer(VersionComparison.VersionRelease);

        /// <summary>
        /// A version comparer that follows SemVer 2.0.0 rules.
        /// </summary>
        public static IVersionComparer VersionReleaseMetadata = new VersionComparer(VersionComparison.VersionReleaseMetadata);

        /// <summary>
        /// Compares sets of release labels.
        /// </summary>
        private static int CompareReleaseLabels(string[] version1, string[] version2)
        {
            var result = 0;

            var count = Math.Max(version1.Length, version2.Length);

            for (var i = 0; i < count; i++)
            {
                var aExists = i < version1.Length;
                var bExists = i < version2.Length;

                if (!aExists && bExists)
                {
                    return -1;
                }

                if (aExists && !bExists)
                {
                    return 1;
                }

                // compare the labels
                result = CompareRelease(version1[i], version2[i]);

                if (result != 0)
                {
                    return result;
                }
            }

            return result;
        }

        /// <summary>
        /// Release labels are compared as numbers if they are numeric, otherwise they will be compared
        /// as strings.
        /// </summary>
        private static int CompareRelease(string version1, string version2)
        {
            var version1Num = 0;
            var version2Num = 0;
            var result = 0;

            // check if the identifiers are numeric
            var v1IsNumeric = int.TryParse(version1, out version1Num);
            var v2IsNumeric = int.TryParse(version2, out version2Num);

            // if both are numeric compare them as numbers
            if (v1IsNumeric && v2IsNumeric)
            {
                result = version1Num.CompareTo(version2Num);
            }
            else if (v1IsNumeric || v2IsNumeric)
            {
                // numeric labels come before alpha labels
                if (v1IsNumeric)
                {
                    result = -1;
                }
                else
                {
                    result = 1;
                }
            }
            else
            {
                // Ignoring 2.0.0 case sensitive compare. Everything will be compared case insensitively as 2.0.1 specifies.
                result = StringComparer.OrdinalIgnoreCase.Compare(version1, version2);
            }

            return result;
        }

        /// <summary>
        /// Returns an array of release labels from the version, or null.
        /// </summary>
        private static string[] GetReleaseLabelsOrNull(SemanticVersion version)
        {
            string[] labels = null;

            // Check if labels exist
            if (version.IsPrerelease)
            {
                // Try to use string[] which is how labels are normally stored.
                var enumerable = version.ReleaseLabels;
                labels = enumerable as string[];

                if (labels != null && enumerable != null)
                {
                    // This is not the expected type, enumerate and convert to an array.
                    labels = enumerable.ToArray();
                }
            }

            return labels;
        }

        /// <summary>
        /// Compare release labels
        /// </summary>
        private static bool AreReleaseLabelsEqual(SemanticVersion x, SemanticVersion y)
        {
            var xLabels = GetReleaseLabelsOrNull(x);
            var yLabels = GetReleaseLabelsOrNull(y);

            if (xLabels == null && yLabels != null)
            {
                return false;
            }

            if (xLabels != null && yLabels == null)
            {
                return false;
            }

            if (xLabels != null && yLabels != null)
            {
                // Both versions must have the same number of labels to be equal
                if (xLabels.Length != yLabels.Length)
                {
                    return false;
                }

                // Check if the labels are the same
                for (var i = 0; i < xLabels.Length; i++)
                {
                    if (!StringComparer.OrdinalIgnoreCase.Equals(xLabels[i], yLabels[i]))
                    {
                        return false;
                    }
                }
            }

            // labels are equal
            return true;
        }

        /// <summary>
        /// Returns the fourth version number or zero.
        /// </summary>
        private static int GetRevisionOrZero(SemanticVersion version)
        {
            return 0;
        }

        public int GetHashCode(SemanticVersion obj)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Custom formatter for NuGet versions.
    /// </summary>
    public class VersionFormatter : IFormatProvider, ICustomFormatter
    {
        /// <summary>
        /// A static instance of the VersionFormatter class.
        /// </summary>
        public static readonly VersionFormatter Instance = new VersionFormatter();

        /// <summary>
        /// Format a version string.
        /// </summary>
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (arg == null)
            {
                throw new ArgumentNullException(nameof(arg));
            }

            string formatted = null;
            var argType = arg.GetType();

            if (argType == typeof(IFormattable))
            {
                formatted = ((IFormattable)arg).ToString(format, formatProvider);
            }
            else if (!String.IsNullOrEmpty(format))
            {
                var version = arg as SemanticVersion;

                if (version != null)
                {
                    // single char identifiers
                    if (format.Length == 1)
                    {
                        formatted = Format(format[0], version);
                    }
                    else
                    {
                        var sb = new StringBuilder(format.Length);

                        for (var i = 0; i < format.Length; i++)
                        {
                            var s = Format(format[i], version);

                            if (s == null)
                            {
                                sb.Append(format[i]);
                            }
                            else
                            {
                                sb.Append(s);
                            }
                        }

                        formatted = sb.ToString();
                    }
                }
            }

            return formatted;
        }

        /// <summary>
        /// Get version format type.
        /// </summary>
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter)
                || formatType == typeof(SemanticVersion))
            {
                return this;
            }

            return null;
        }

        /// <summary>
        /// Create a normalized version string. This string is unique for each version 'identity' 
        /// and does not include leading zeros or metadata.
        /// </summary>
        private static string GetNormalizedString(SemanticVersion version)
        {
            var normalized = Format('V', version);

            if (version.IsPrerelease)
            {
                normalized = $"{normalized}-{version.Release}";
            }

            return normalized;
        }

        /// <summary>
        /// Create the full version string including metadata. This is primarily for display purposes.
        /// </summary>
        private static string GetFullString(SemanticVersion version)
        {
            var fullString = GetNormalizedString(version);

            if (version.HasMetadata)
            {
                fullString = $"{fullString}+{version.Metadata}";
            }

            return fullString;
        }

        private static string Format(char c, SemanticVersion version)
        {
            string s = null;

            switch (c)
            {
                case 'N':
                    s = GetNormalizedString(version);
                    break;
                case 'R':
                    s = version.Release;
                    break;
                case 'M':
                    s = version.Metadata;
                    break;
                case 'V':
                    s = FormatVersion(version);
                    break;
                case 'F':
                    s = GetFullString(version);
                    break;
                case 'x':
                    s = string.Format(CultureInfo.InvariantCulture, "{0}", version.Major);
                    break;
                case 'y':
                    s = string.Format(CultureInfo.InvariantCulture, "{0}", version.Minor);
                    break;
                case 'z':
                    s = string.Format(CultureInfo.InvariantCulture, "{0}", version.Patch);
                    break;
            }

            return s;
        }

        private static string FormatVersion(SemanticVersion version)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}{3}", version.Major, version.Minor, version.Patch, null);
        }
    }

    /// <summary>
    /// A base version operations
    /// </summary>
    public partial class SemanticVersion : IFormattable, IComparable, IComparable<SemanticVersion>, IEquatable<SemanticVersion>
    {
        /// <summary>
        /// Gives a normalized representation of the version.
        /// This string is unique to the identity of the version and does not contain metadata.
        /// </summary>
        public virtual string ToNormalizedString()
        {
            return ToString("N", VersionFormatter.Instance);
        }

        /// <summary>
        /// Gives a full representation of the version include metadata.
        /// This string is not unique to the identity of the version. Other versions 
        /// that differ on metadata will have a different full string representation.
        /// </summary>
        public virtual string ToFullString()
        {
            return ToString("F", VersionFormatter.Instance);
        }

        /// <summary>
        /// Get the normalized string.
        /// </summary>
        public override string ToString()
        {
            return ToNormalizedString();
        }

        /// <summary>
        /// Custom string format.
        /// </summary>
        public virtual string ToString(string format, IFormatProvider formatProvider)
        {
            string formattedString = null;

            if (formatProvider == null
                || !TryFormatter(format, formatProvider, out formattedString))
            {
                formattedString = ToString();
            }

            return formattedString;
        }

        /// <summary>
        /// Internal string formatter.
        /// </summary>
        protected bool TryFormatter(string format, IFormatProvider formatProvider, out string formattedString)
        {
            var formatted = false;
            formattedString = null;

            if (formatProvider != null)
            {
                var formatter = formatProvider.GetFormat(this.GetType()) as ICustomFormatter;
                if (formatter != null)
                {
                    formatted = true;
                    formattedString = formatter.Format(format, this, formatProvider);
                }
            }

            return formatted;
        }

        /// <summary>
        /// Hash code
        /// </summary>
        public override int GetHashCode()
        {
            return VersionComparer.Default.GetHashCode(this);
        }

        /// <summary>
        /// Object compare.
        /// </summary>
        public virtual int CompareTo(object obj)
        {
            return CompareTo(obj as SemanticVersion);
        }

        /// <summary>
        /// Compare to another SemanticVersion.
        /// </summary>
        public virtual int CompareTo(SemanticVersion other)
        {
            return CompareTo(other, VersionComparison.Default);
        }

        /// <summary>
        /// Equals
        /// </summary>
        public override bool Equals(object obj)
        {
            return Equals(obj as SemanticVersion);
        }

        /// <summary>
        /// Equals
        /// </summary>
        public virtual bool Equals(SemanticVersion other)
        {
            return VersionComparer.Default.Equals(this, other);
        }

        /// <summary>
        /// True if the VersionBase objects are equal based on the given comparison mode.
        /// </summary>
        public virtual bool Equals(SemanticVersion other, VersionComparison versionComparison)
        {
            var comparer = new VersionComparer(versionComparison);
            return comparer.Equals(this, other);
        }

        /// <summary>
        /// Compares NuGetVersion objects using the given comparison mode.
        /// </summary>
        public virtual int CompareTo(SemanticVersion other, VersionComparison versionComparison)
        {
            var comparer = new VersionComparer(versionComparison);
            return comparer.Compare(this, other);
        }

        /// <summary>
        /// Equals
        /// </summary>
        public static bool operator ==(SemanticVersion version1, SemanticVersion version2)
        {
            return Equals(version1, version2);
        }

        /// <summary>
        /// Not equal
        /// </summary>
        public static bool operator !=(SemanticVersion version1, SemanticVersion version2)
        {
            return !Equals(version1, version2);
        }

        /// <summary>
        /// Less than
        /// </summary>
        public static bool operator <(SemanticVersion version1, SemanticVersion version2)
        {
            return Compare(version1, version2) < 0;
        }

        /// <summary>
        /// Less than or equal
        /// </summary>
        public static bool operator <=(SemanticVersion version1, SemanticVersion version2)
        {
            return Compare(version1, version2) <= 0;
        }

        /// <summary>
        /// Greater than
        /// </summary>
        public static bool operator >(SemanticVersion version1, SemanticVersion version2)
        {
            return Compare(version1, version2) > 0;
        }

        /// <summary>
        /// Greater than or equal
        /// </summary>
        public static bool operator >=(SemanticVersion version1, SemanticVersion version2)
        {
            return Compare(version1, version2) >= 0;
        }

        private static int Compare(SemanticVersion version1, SemanticVersion version2)
        {
            return VersionComparer.Default.Compare(version1, version2);
        }
    }

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