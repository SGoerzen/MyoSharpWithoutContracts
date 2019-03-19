using System;
using System.Collections.Generic;

using System.Text;

using MyoSharp.Communication;
using MyoSharp.Exceptions;
using MyoSharp.Internal;

namespace MyoSharp.Exceptions
{
    public class MyoErrorHandlerDriver : IMyoErrorHandlerDriver
    {
        #region Fields
        private readonly IMyoErrorHandlerBridge _myoErrorHandlerBridge;
        #endregion

        #region Constructors
        private MyoErrorHandlerDriver(IMyoErrorHandlerBridge myoErrorHandlerBridge)
        {

            _myoErrorHandlerBridge = myoErrorHandlerBridge;
        }
        #endregion

        #region Methods
        public static IMyoErrorHandlerDriver Create(IMyoErrorHandlerBridge myoErrorHandlerBridge)
        {

            return new MyoErrorHandlerDriver(myoErrorHandlerBridge);
        }

        /// <inheritdoc />
        public string GetErrorString(IntPtr errorHandle)
        {
            return PlatformInvocation.Running32Bit
                ? _myoErrorHandlerBridge.LibmyoErrorCstring32(errorHandle)
                : _myoErrorHandlerBridge.LibmyoErrorCstring64(errorHandle);
        }

        /// <inheritdoc />
        public void FreeMyoError(IntPtr errorHandle)
        {
            if (errorHandle == IntPtr.Zero)
            {
                return;
            }

            if (PlatformInvocation.Running32Bit)
            {
                _myoErrorHandlerBridge.LibmyoFreeErrorDetails32(errorHandle);
            }
            else
            {
                _myoErrorHandlerBridge.LibmyoFreeErrorDetails64(errorHandle);
            }
        }
        private void ObjectInvariant()
        {
        }
        #endregion
    }
}
