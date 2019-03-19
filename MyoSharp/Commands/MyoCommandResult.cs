using System;
using System.Collections.Generic;

using System.Text;

using MyoSharp.Communication;

namespace MyoSharp.Commands
{
    public class MyoCommandResult : IMyoCommandResult
    {
        #region Fields
        private readonly MyoResult _result;
        private readonly IntPtr _errorHandle;
        #endregion

        #region Constructors
        private MyoCommandResult(MyoResult result, IntPtr errorHandle)
        {

            _result = result;
            _errorHandle = errorHandle;
        }
        #endregion

        #region Properties
        /// <inheritdoc />
        public MyoResult Result 
        {
            get
            {
                return _result;
            }
        }

        /// <inheritdoc />
        public IntPtr ErrorHandle
        {
            get
            {
                return _errorHandle;
            }
        }
        #endregion

        #region Methods
        public static IMyoCommandResult Create(MyoResult result, IntPtr errorHandle)
        {

            return new MyoCommandResult(result, errorHandle);
        }
        private void ObjectInvariant()
        {
        }
        #endregion
    }
}
