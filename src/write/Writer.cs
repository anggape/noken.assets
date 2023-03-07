namespace Noken.Assets;

public sealed class Writer : IDisposable
{
    private List<string> _files;
    private Stream _stream;

    /// <summary>
    /// Create new assets writer instance.
    /// </summary>
    /// <param name="stream">output stream</param>
    public Writer(Stream stream)
        : this(stream, new string[0]) { }

    /// <summary>
    /// Create new assets writer instance.
    /// </summary>
    /// <param name="stream">output stream</param>
    /// <param name="files">list of files</param>
    /// <exception cref="FileNotFoundException"></exception>
    public Writer(Stream stream, IEnumerable<string> files)
    {
        foreach (var file in files)
        {
            if (!File.Exists(file))
                throw new FileNotFoundException(file);
        }

        _files = new(files);
        _stream = stream;
    }

    /// <summary>
    /// Dispose and start writing assets.
    /// </summary>
    public void Dispose()
    {
        using var writer = new BinaryWriter(_stream);
        var buffers = new List<byte>();

        writer.Write(Helper.Signature.ToCharArray());
        writer.Write(new byte[] { (byte)Helper.Version.Major, (byte)Helper.Version.Minor });
        writer.Write(_files.Count);

        foreach (var file in _files)
        {
            var buffer = File.ReadAllBytes(file);

            writer.WriteNullTerminatedString(file);
            writer.Write(buffers.Count);
            writer.Write(buffer.Length);

            buffers.AddRange(buffer);
        }

        writer.Write(buffers.ToArray());

        GC.SuppressFinalize(this);
    }
}
