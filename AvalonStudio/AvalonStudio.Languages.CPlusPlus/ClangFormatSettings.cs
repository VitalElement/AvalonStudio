namespace AvalonStudio.Languages.CPlusPlus
{
	public class ClangFormatSettings
	{
		public enum BraceStyleOption
		{
			Attach,
			Linux,
			Stroustrup,
			Allman,
			GNU
		}

		public enum LanguageOption
		{
			None,
			Cpp,
			JavaScript,
			Proto
		}

		public enum NamespaceIndentationOption
		{
			None,
			Inner,
			All
		}

		public enum SpaceBeforeParenthesisOption
		{
			Never,
			ControlStatements,
			Always
		}

		public enum StandardOption
		{
			Cpp03,
			Cpp11,
			Auto
		}

		public enum UseTabOption
		{
			Never,
			ForIndentation,
			Always
		}

		private static readonly string formatString =
			@"
AccessModifierOffset: {0},
AlignEscapedNewlinesLeft: {1},
AlignTrailingComments: {2},
AllowAllParametersOfDeclarationOnNextLine: {3},
AllowShortFunctionsOnASingleLine: {4},
AllowShortIfStatementsOnASingleLine: {5},
AllowShortLoopsOnASingleLine: {6},
AlwaysBreakBeforeMultilineStrings: {7},
AlwaysBreakTemplateDeclarations: {8},
BinPackParameters: {9},
BreakBeforeBinaryOperators: {10},
BreakBeforeBraces: {11},
BreakBeforeTernaryOperators: {12},
BreakConstructorInitializersBeforeComma: {13},
ColumnLimit: {14},
CommentPragmas: '{15}',
ConstructorInitializerAllOnOneLineOrOnePerLine: {16},
ConstructorInitializerIndentWidth: {17},
ContinuationIndentWidth: {18},
Cpp11BracedListStyle: {19},
DerivePointerBinding: {20},
IndentCaseLabels: {21},
IndentFunctionDeclarationAfterType: {22},
IndentWidth: {23},
Language: {24},
MaxEmptyLinesToKeep: {25},
NamespaceIndentation: {26},
ObjCSpaceAfterProperty: {27},
ObjCSpaceBeforeProtocolList: {28},
PenaltyBreakBeforeFirstCallParameter: {29},
PenaltyBreakComment: {30},
PenaltyBreakFirstLessLess: {31},
PenaltyBreakString: {32},
PenaltyExcessCharacter: {33},
PenaltyReturnTypeOnItsOwnLine: {34},
PointerBindsToType: {35},
SpaceBeforeAssignmentOperators: {36},
SpaceBeforeParens: {37},
SpaceInEmptyParentheses: {38},
SpacesBeforeTrailingComments: {39},
SpacesInAngles: {40},
SpacesInCStyleCastParentheses: {41},
SpacesInContainerLiterals: {42},
SpacesInParentheses: {43},
Standard: {44},
TabWidth: {45},
UseTab: {46}";

		public static ClangFormatSettings Default
		{
			get
			{
				var result = new ClangFormatSettings();

				result.AlignAfterOpenBracket = false;
				result.AlignConsecutiveAssignments = true;
				result.AccessModifierOffset = -2;
				result.ConstructorInitializerIndentWidth = 4;
				result.AlignTrainlingComments = true;
				result.AllowAllParameterDeclarationsOnNextLine = true;
				result.AlignEscapedNewLinesLeft = true;
				result.AllowShortIfStatementsOnASingleLine = false;
				result.AllowShortFunctionsOnASingleLine = false;
				result.BreakBeforeTernaryOperators = true;
				result.BinPackParameters = true;
				result.BreakBeforeBraces = BraceStyleOption.Allman;
				result.ColumnLimit = 120;
				result.IndentationWidth = 4;
				result.Language = LanguageOption.Cpp;
				result.MaxEmptyLinesToKeep = 2;
				result.ObjCSpaceAfterProperty = true;
				result.ObjCSpaceBeforeProtocolList = true;
				result.PenaltyBreakBeforeFirstCallParameter = 19;
				result.PenaltyBreakComment = 300;
				result.PenaltyBreakString = 1000;
				result.PenaltyBreakFirstLessLess = 120;
				result.PenaltyExcessCharacter = 1000000;
				result.PenaltyReturnTypeOnItsOwnLine = 60;
				result.PointerBindsToType = true;
				result.SpaceBeforeParenthesis = SpaceBeforeParenthesisOption.Always;
				result.SpaceBeforeAssignmentOperators = true;
				result.SpacesBeforeTrailingComments = 1;
				result.Standard = StandardOption.Cpp11;
				result.TabWidth = 4;
				result.UseTab = UseTabOption.Never;

				return result;
			}
		}

		public int AccessModifierOffset { get; set; }
		public bool AlignAfterOpenBracket { get; set; }
		public bool AlignConsecutiveAssignments { get; set; }
		public bool AlignEscapedNewLinesLeft { get; set; }
		public bool AlignTrainlingComments { get; set; }
		public bool AllowAllParameterDeclarationsOnNextLine { get; set; }
		public bool AllowShortFunctionsOnASingleLine { get; set; }
		public bool AllowShortIfStatementsOnASingleLine { get; set; }
		public bool AllowShortLoopsOnASingleLine { get; set; }
		public bool AlwaysBreakBeforeMultiLineStrings { get; set; }
		public bool AlwaysBreakTemplateDeclarations { get; set; }
		public bool BinPackParameters { get; set; }
		public bool BreakBeforeBinaryOperators { get; set; }
		public BraceStyleOption BreakBeforeBraces { get; set; }
		public bool BreakBeforeTernaryOperators { get; set; }
		public bool BreakConstructorInitializersBeforeComma { get; set; }
		public int ColumnLimit { get; set; }
		public string CommentPragmas { get; set; }
		public bool ConstructorInitializerAllOnOneLineOrOnePerLine { get; set; }
		public int ConstructorInitializerIndentWidth { get; set; }
		public int ContinuationIndentWidth { get; set; }
		public bool Cpp11BracedListStyle { get; set; }
		public bool DerivePointerBinding { get; set; }
		public bool IndentCaseLabels { get; set; }
		public bool IndentFunctionDeclarationAfterType { get; set; }
		public int IndentationWidth { get; set; }
		public LanguageOption Language { get; set; }
		public int MaxEmptyLinesToKeep { get; set; }
		public NamespaceIndentationOption NamespaceIndentation { get; set; }
		public bool ObjCSpaceAfterProperty { get; set; }
		public bool ObjCSpaceBeforeProtocolList { get; set; }
		public int PenaltyBreakBeforeFirstCallParameter { get; set; }
		public int PenaltyBreakComment { get; set; }
		public int PenaltyBreakFirstLessLess { get; set; }
		public int PenaltyBreakString { get; set; }
		public int PenaltyExcessCharacter { get; set; }
		public int PenaltyReturnTypeOnItsOwnLine { get; set; }
		public bool PointerBindsToType { get; set; }
		public bool SpaceBeforeAssignmentOperators { get; set; }
		public SpaceBeforeParenthesisOption SpaceBeforeParenthesis { get; set; }
		public bool SpaceInEmptyParenthesis { get; set; }
		public int SpacesBeforeTrailingComments { get; set; }
		public bool SpacesInAngles { get; set; }
		public bool SpacesInCStyleCastParenthesis { get; set; }
		public bool SpacesInContainerLiterals { get; set; }
		public bool SpacesInParenthesis { get; set; }
		public StandardOption Standard { get; set; }
		public int TabWidth { get; set; }
		public UseTabOption UseTab { get; set; }

		public ClangFormatSettings Clone()
		{
			return (ClangFormatSettings) MemberwiseClone();
		}

		public override string ToString()
		{
			return string.Format(formatString.Replace("\r\n", ""),
				AccessModifierOffset,
				AlignEscapedNewLinesLeft.ToString().ToLower(),
				AlignTrainlingComments.ToString().ToLower(),
				AllowAllParameterDeclarationsOnNextLine.ToString().ToLower(),
				AllowShortFunctionsOnASingleLine.ToString().ToLower(),
				AllowShortIfStatementsOnASingleLine.ToString().ToLower(),
				AllowShortLoopsOnASingleLine.ToString().ToLower(),
				AlwaysBreakBeforeMultiLineStrings.ToString().ToLower(),
				AlwaysBreakTemplateDeclarations.ToString().ToLower(),
				BinPackParameters.ToString().ToLower(),
				BreakBeforeBinaryOperators.ToString().ToLower(),
				BreakBeforeBraces,
				BreakBeforeTernaryOperators.ToString().ToLower(),
				BreakConstructorInitializersBeforeComma.ToString().ToLower(),
				ColumnLimit,
				CommentPragmas,
				ConstructorInitializerAllOnOneLineOrOnePerLine.ToString().ToLower(),
				ConstructorInitializerIndentWidth,
				ContinuationIndentWidth,
				Cpp11BracedListStyle.ToString().ToLower(),
				DerivePointerBinding.ToString().ToLower(),
				IndentCaseLabels.ToString().ToLower(),
				IndentFunctionDeclarationAfterType.ToString().ToLower(),
				IndentationWidth,
				Language,
				MaxEmptyLinesToKeep,
				NamespaceIndentation,
				ObjCSpaceAfterProperty.ToString().ToLower(),
				ObjCSpaceBeforeProtocolList.ToString().ToLower(),
				PenaltyBreakBeforeFirstCallParameter,
				PenaltyBreakComment,
				PenaltyBreakFirstLessLess,
				PenaltyBreakString,
				PenaltyExcessCharacter,
				PenaltyReturnTypeOnItsOwnLine,
				PointerBindsToType.ToString().ToLower(),
				SpaceBeforeAssignmentOperators.ToString().ToLower(),
				SpaceBeforeParenthesis,
				SpaceInEmptyParenthesis.ToString().ToLower(),
				SpacesBeforeTrailingComments,
				SpacesInAngles.ToString().ToLower(),
				SpacesInCStyleCastParenthesis.ToString().ToLower(),
				SpacesInContainerLiterals.ToString().ToLower(),
				SpacesInParenthesis.ToString().ToLower(),
				Standard,
				TabWidth,
				UseTab,
				AlignAfterOpenBracket.ToString().ToLower(),
				AlignConsecutiveAssignments.ToString().ToLower()).Insert(0, "{") + "}";
		}
	}
}