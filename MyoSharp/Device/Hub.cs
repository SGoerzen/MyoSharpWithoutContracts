using System;
using System.Collections.Generic;


using MyoSharp.Communication;
using MyoSharp.Discovery;
using MyoSharp.Exceptions;

namespace MyoSharp.Device
{
    /// <summary>
    /// A class that manages a collection of Myos.
    /// </summary>
    public class Hub : IHub
    {
        #region Fields
        private readonly Dictionary<IntPtr, IMyo> _myos;
        private readonly IDeviceListener _deviceListener;
        private readonly ReadOnlyMyoCollection _readonlyMyos;

        private bool _disposed;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="Hub"/> class.
        /// </summary>
        /// <param name="channelListener">The channel listener.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the channel listener is null.</exception>
        protected Hub(IChannelListener channelListener)
            : this(DeviceListener.Create(channelListener))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Hub" /> class.
        /// </summary>
        /// <param name="deviceListener">The device listener.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the device listener is null.</exception>
        protected Hub(IDeviceListener deviceListener)
        {

            _myos = new Dictionary<IntPtr, IMyo>();
            _readonlyMyos = new ReadOnlyMyoCollection(_myos);

            _deviceListener = deviceListener;
            _deviceListener.Paired += DeviceListener_Paired;
            _deviceListener.Unpaired += DeviceListener_Unpaired;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Hub"/> class.
        /// </summary>
        ~Hub()
        {
            Dispose(false);
        }
        #endregion

        #region Events
        /// <summary>
        /// The event that is triggered when a Myo has connected.
        /// </summary>
        public event EventHandler<MyoEventArgs> MyoConnected;

        /// <summary>
        /// The event that is triggered when a Myo has disconnected.
        /// </summary>
        public event EventHandler<MyoEventArgs> MyoDisconnected;
        #endregion

        #region Properties
        /// <summary>
        /// Gets the collection of Myos being managed by this hub.
        /// </summary>
        public IReadOnlyMyoCollection Myos
        {
            get { return _readonlyMyos; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates a new <see cref="IHub"/> instance.
        /// </summary>
        /// <param name="channelListener">The channel listener.</param>
        /// <returns>A new <see cref="IHub"/> instance.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the channel listener is null.</exception>
        public static IHub Create(IChannelListener channelListener)
        {

            return new Hub(DeviceListener.Create(channelListener));
        }

        /// <summary>
        /// Creates a new <see cref="IHub"/> instance.
        /// </summary>
        /// <param name="deviceListener">The device listener.</param>
        /// <returns>A new <see cref="IHub"/> instance.</returns>
        /// <exception cref="System.ArgumentNullException">Thrown when the device listener is null.</exception>
        public static IHub Create(IDeviceListener deviceListener)
        {

            return new Hub(deviceListener);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Creates a new <see cref="IMyo" /> instance.
        /// </summary>
        /// <param name="channelListener">The channel listener.</param>
        /// <param name="myoDeviceDriver">The Myo device driver.</param>
        /// <returns>
        /// Returns a new <see cref="IMyo" /> instance.
        /// </returns>
        protected virtual IMyo CreateMyo(IChannelListener channelListener, IMyoDeviceDriver myoDeviceDriver)
        {

            return Myo.Create(channelListener, myoDeviceDriver);
        }

        /// <summary>
        /// Creates a new <see cref="IMyoDeviceDriver" />.
        /// </summary>
        /// <param name="myoHandle">The Myo handle.</param>
        /// <param name="myoDeviceBridge">The Myo device bridge. Cannot be <c>null</c>.</param>
        /// <returns>
        /// Returns a new <see cref="IMyoDeviceDriver" /> instance.
        /// </returns>
        [Obsolete("Please use the CreateMyoDeviceDriver method that accepts an IMyoErrorHandlerDriver reference.")]
        protected virtual IMyoDeviceDriver CreateMyoDeviceDriver(IntPtr myoHandle, IMyoDeviceBridge myoDeviceBridge)
        {

            return CreateMyoDeviceDriver(
                myoHandle, 
                myoDeviceBridge, 
                MyoErrorHandlerDriver.Create(MyoErrorHandlerBridge.Create()));
        }

        /// <summary>
        /// Creates a new <see cref="IMyoDeviceDriver" />.
        /// </summary>
        /// <param name="myoHandle">The Myo handle.</param>
        /// <param name="myoDeviceBridge">The Myo device bridge. Cannot be <c>null</c>.</param>
        /// <param name="myoErrorHandlerDriver">The myo error handler driver. Cannot be <c>null</c>.</param>
        /// <returns>
        /// Returns a new <see cref="IMyoDeviceDriver" /> instance.
        /// </returns>
        protected virtual IMyoDeviceDriver CreateMyoDeviceDriver(IntPtr myoHandle, IMyoDeviceBridge myoDeviceBridge, IMyoErrorHandlerDriver myoErrorHandlerDriver)
        {

            return MyoDeviceDriver.Create(myoHandle, myoDeviceBridge, myoErrorHandlerDriver);
        }

        /// <summary>
        /// Creates a new <see cref="IMyoDeviceBridge"/>.
        /// </summary>
        /// <returns>Returns a new <see cref="IMyoDeviceBridge"/> instance.</returns>
        protected virtual IMyoDeviceBridge CreateMyoDeviceBridge()
        {

            return MyoDeviceBridge.Create();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> to release both managed and unmanaged resources; 
        /// <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            try
            {
                if (disposing)
                {
                    foreach (var myo in _myos.Values)
                    {

                        UnhookMyoEvents(myo);
                        myo.Dispose();
                    }

                    _myos.Clear();

                    _deviceListener.Paired -= DeviceListener_Paired;
                    _deviceListener.Unpaired -= DeviceListener_Unpaired;
                }
            }
            finally
            {
                _disposed = true;
            }
        }

        /// <summary>
        /// Hooks the Myo events.
        /// </summary>
        /// <param name="myo">The Myo to hook onto.</param>
        protected virtual void HookMyoEvents(IMyoEventGenerator myo)
        {

            myo.Connected += Myo_Connected;
            myo.Disconnected += Myo_Disconnected;
        }

        /// <summary>
        /// Unhooks the Myo events.
        /// </summary>
        /// <param name="myo">The myo to hook onto.</param>
        protected virtual void UnhookMyoEvents(IMyoEventGenerator myo)
        {

            myo.Connected -= Myo_Connected;
            myo.Disconnected -= Myo_Disconnected;
        }

        /// <summary>
        /// Raises the <see cref="E:MyoConnected" /> event.
        /// </summary>
        /// <param name="e">The <see cref="MyoEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMyoConnected(MyoEventArgs e)
        {

            var handler = MyoConnected;
            if (handler != null)
            {
                handler.Invoke(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:MyoDisconnected" /> event.
        /// </summary>
        /// <param name="e">The <see cref="MyoEventArgs"/> instance containing the event data.</param>
        protected virtual void OnMyoDisconnected(MyoEventArgs e)
        {

            var handler = MyoDisconnected;
            if (handler != null)
            {
                handler.Invoke(this, e);
            }
        }
        #endregion

        #region Event Handlers
        private void DeviceListener_Paired(object sender, PairedEventArgs e)
        {

            if (_myos.ContainsKey(e.MyoHandle))
            {
                return;
            }

            var myoDeviceBridge = CreateMyoDeviceBridge();

            // TODO: replace this obsolete call with the one below
#pragma warning disable 0618
            var myoDeviceDriver = CreateMyoDeviceDriver(
                e.MyoHandle,
                myoDeviceBridge);
#pragma warning restore 0618

            // TODO: use this call once the obsolete method is removed
            ////var myoErrorHandlerDriver = MyoErrorHandlerDriver.Create(MyoErrorHandlerBridge.Create());
            ////var myoDeviceDriver = CreateMyoDeviceDriver(
            ////    e.MyoHandle,
            ////    myoDeviceBridge, 
            ////    myoErrorHandlerDriver);

            var myo = CreateMyo(
                ((IDeviceListener)sender).ChannelListener,
                myoDeviceDriver);

            _myos[myo.Handle] = myo;
            HookMyoEvents(myo);
        }

        private void DeviceListener_Unpaired(object sender, PairedEventArgs e)
        {

            IMyo myo;
            if (!_myos.TryGetValue(e.MyoHandle, out myo) || myo == null)
            {
                return;
            }

            UnhookMyoEvents(myo);
            _myos.Remove(myo.Handle);
            myo.Dispose();
        }

        private void Myo_Disconnected(object sender, MyoEventArgs e)
        {

            OnMyoDisconnected(e);
        }

        private void Myo_Connected(object sender, MyoEventArgs e)
        {

            OnMyoConnected(e);
        }
        private void ObjectInvariant()
        {
        }
        #endregion

        #region Classes
        private class ReadOnlyMyoCollection : IReadOnlyMyoCollection
        {
            #region Fields
            private readonly Dictionary<IntPtr, IMyo> _myos;
            #endregion

            #region Constructors
            internal ReadOnlyMyoCollection(Dictionary<IntPtr, IMyo> myos)
            {

                _myos = myos;
            }
            #endregion

            #region Properties
            public int Count
            {
                get { return _myos.Count; }
            }
            #endregion

            #region Methods
            public IEnumerator<IMyo> GetEnumerator()
            {
                return _myos.Values.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return _myos.Values.GetEnumerator();
            }
            private void ObjectInvariant()
            {
            }
            #endregion
        }
        #endregion
    }
}
