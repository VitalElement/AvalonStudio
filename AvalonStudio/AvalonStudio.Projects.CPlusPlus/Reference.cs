namespace AvalonStudio.Projects.CPlusPlus
{
	public class Reference
	{
		/// <summary>
		///     This is the name of the reference, this is set if a reference is to be found locally.
		///     This field is always required and specifies the directory git urls get cloned to.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///     This is the GitUrl of the reference, in which case the reference will be pulled in.
		/// </summary>
		public string GitUrl { get; set; }

		/// <summary>
		///     This is the revision of the reference to use, this can be head, a tag, or a specific sha. This is
		///     only valid for referencing via git url.
		/// </summary>
		public string Revision { get; set; }

		//    var result = Repository.ListRemoteReferences(gitUrl);
		//{

		//public static bool IsValidGitRepo (string gitUrl)

		//    return true;
		//}
	}
}