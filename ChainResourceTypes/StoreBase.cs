namespace Mize.ChainResourceTypes
{
    public abstract class StoreBase<T> : IStore<T>
    {
        internal static object lockFile = new Object();
        internal bool _isLocked;

        public StorageType Type { get; }
        public TimeSpan ExpirationTime { get; }
        public DateTime LastUpdated { get; set; }

        internal abstract bool Get(out T value, TimeSpan timeout);

        public bool GetValue(out T value, TimeSpan timeout)
        {
            var tik = DateTime.UtcNow;
            DateTime tok;

            while (_isLocked)
            {
                tok = DateTime.UtcNow;
                if (tok - tik > timeout)
                {
                    Console.WriteLine("File is locked, failed to read value");
                    value = default(T);
                    return false;
                }

                Thread.Sleep(100);
            }

            return Get(out value, timeout);
        }
        
        internal abstract Task Update(T value);

        public async Task UpdateValue(T value)
        {
            if (Type == StorageType.Read)
                return;

            lock (lockFile)
            {
                _isLocked = true;
                Update(value).ContinueWith(_ => LastUpdated = DateTime.UtcNow);
            }

            _isLocked = false;
        }
        
        protected StoreBase(StorageType type, TimeSpan expiration)
        {
            Type = type;
            ExpirationTime = expiration;
        }

        protected StoreBase(StorageType read)
        {
            Type = read;
        }
    }
}
