using System;

namespace AvalonStudio.Projects.OmniSharp.ProjectTypes
{
    class CSharpProjectType : DotNetCoreCSharpProjectType
    {
        public static Guid CSharpProjectTypeId = Guid.Parse("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}");

        public override Guid ProjectTypeId => CSharpProjectTypeId;
    }
}
