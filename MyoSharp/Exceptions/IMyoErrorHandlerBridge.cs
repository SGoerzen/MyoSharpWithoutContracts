using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace MyoSharp.Exceptions
{
    public interface IMyoErrorHandlerBridge
    {
        #region Methods
        string LibmyoErrorCstring32(IntPtr errorHandle);

        string LibmyoErrorCstring64(IntPtr errorHandle);

        void LibmyoFreeErrorDetails32(IntPtr errorHandle);

        void LibmyoFreeErrorDetails64(IntPtr errorHandle);
        #endregion
    }
}
