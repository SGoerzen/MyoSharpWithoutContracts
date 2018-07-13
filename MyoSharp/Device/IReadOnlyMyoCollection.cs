using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace MyoSharp.Device
{
    public interface IReadOnlyMyoCollection : IEnumerable<IMyo>
    {
        #region Properties
        /// <summary>
        /// Gets the number of Myos in the collection.
        /// </summary>
        int Count { get; }
        #endregion
    }
}
