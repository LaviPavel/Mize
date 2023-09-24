using Mize.ChainResourceTypes;
using System.Text;

public class FileStore<T> : StoreBase<T>
{
    private string _filePath;

    public FileStore(string filePath, TimeSpan expiration) : base(StorageType.Write, expiration)
    {
        _filePath = filePath;
    }

    internal override bool Get(out T value)
    {
        value = default(T);

        if (!File.Exists(_filePath)) return false;

        var data = File.ReadAllText(_filePath, Encoding.Unicode);
        if (string.IsNullOrEmpty(data))
            return false;

        value = (T)Convert.ChangeType(data, typeof(T));
        return true;
    }

    internal override Task Update(T value)
    {
        using (FileStream file = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.None))
        using (StreamWriter writer = new StreamWriter(file, Encoding.Unicode))
        {
            writer.Write(value.ToString());
        }

        return Task.CompletedTask;
    }
}