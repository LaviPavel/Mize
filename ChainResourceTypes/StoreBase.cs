namespace Mize.ChainResourceTypes
{
    public abstract class StoreBase<T> : IStore<T>
    {
        internal static object lockFile = new Object();
        internal bool _isLocked;
        internal bool _useBaseLock;
        private DateTime _lastUpdated;

        public StorageType Type { get; }
        public TimeSpan Expiration { get; }

        protected StoreBase(StorageType type, TimeSpan expiration, bool useBaseLock = true)
        {
            Type = type;
            Expiration = expiration;
            _useBaseLock = useBaseLock;
        }

        internal abstract bool Get(out T value);

        public bool GetValue(out T value)
        {
            if (DateTime.UtcNow - _lastUpdated < Expiration || Type == StorageType.Read)
            {
                var isValueReady = Get(out value);
                return isValueReady;
            }

            value = default(T);
            return false;
        }
        
        internal abstract Task Update(T value);

        public async Task UpdateValue(T value)
        {
            if (Type == StorageType.Read)
                return;

            if (_useBaseLock)
            {
                lock (lockFile)
                {
                    _isLocked = true;
                    Update(value).ContinueWith(_ => _lastUpdated = DateTime.UtcNow);
                }

                _isLocked = false;
            }
        }
    }
}
