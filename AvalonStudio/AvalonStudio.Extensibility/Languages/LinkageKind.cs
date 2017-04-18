namespace AvalonStudio.Languages
{
    /// <summary>
    ///     Describe the linkage of the entity referred to by a cursor.
    /// </summary>
    public enum LinkageKind
    {
        /// <summary>
        ///     This value indicates that no linkage information is available
        ///     for a provided CXCursor.
        /// </summary>
        Invalid,

        /// <summary>
        ///     This is the linkage for variables, parameters, and so on that
        ///     have automatic storage.  This covers normal (non-extern) local variables.
        /// </summary>
        NoLinkage,

        /// <summary>
        ///     This is the linkage for static variables and static functions.
        /// </summary>
        Internal,

        /// <summary>
        ///     This is the linkage for entities with external linkage that live
        ///     in C++ anonymous namespaces.
        /// </summary>
        UniqueExternal,

        /// <summary>
        ///     This is the linkage for entities with true, external linkage.
        /// </summary>
        External
    }
}