using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Text;

namespace MyoSharp.Device
{
    /// <summary>
    /// A class that contains EMG information on a per-sensor basis.
    /// </summary>
    public sealed class EmgData : IEmgData
    {
        #region Fields
        private readonly int[] _emgData;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="EmgData"/> class.
        /// </summary>
        /// <param name="emgData">The raw EMG data per sensor. Cannot be null.</param>
        /// <exception cref="System.ArgumentNullException">
        /// The exception that is thrown when <paramref name="emgData"/> is <c>null</c>.
        /// </exception>
        private EmgData(int[] emgData)
        {

            _emgData = emgData;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new <see cref="IEmgData"/> instance.
        /// </summary>
        /// <param name="emgData">The raw EMG data per sensor. Cannot be null.</param>
        /// <returns>Returns a new <see cref="IEmgData"/> instance.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// The exception that is thrown when <paramref name="emgData"/> is <c>null</c>.
        /// </exception>
        public static IEmgData Create(int[] emgData)
        {
            
            return new EmgData(emgData);
        }

        /// <inheritdoc />
        public int GetDataForSensor(int sensor)
        {
            return sensor >= _emgData.Length
                ? 0
                : _emgData[sensor];
        }
        private void ObjectInvariant()
        {
        }
        #endregion
    }
}
