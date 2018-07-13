using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace MyoSharp.Exceptions
{
    public interface IMyoErrorHandlerDriver
    {
        #region Methods
        string GetErrorString(IntPtr errorHandle);

        void FreeMyoError(IntPtr errorHandle);
        #endregion
    }
}
