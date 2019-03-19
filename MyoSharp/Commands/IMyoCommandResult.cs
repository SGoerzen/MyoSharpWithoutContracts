using System;
using System.Collections.Generic;

using System.Text;

using MyoSharp.Communication;

namespace MyoSharp.Commands
{
    public interface IMyoCommandResult
    {
        #region Properties
        MyoResult Result { get; }

        IntPtr ErrorHandle { get; }
        #endregion
    }
}
