using CAF.Core.Enums;

using System;
using System.Collections.Generic;
using System.Text;

namespace CAF.Core.Service
{
    public interface IAllFileService
    {
        void AddFile(Guid relationId, byte[] file, string fileName, FileCategory category);
    }
}
