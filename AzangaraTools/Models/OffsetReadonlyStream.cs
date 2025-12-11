namespace AzangaraTools.Models;

internal class OffsetReadOnlyStream : Stream
{
    private readonly Stream _baseStream;
    private long _offset;
    private long _size;

    public OffsetReadOnlyStream(Stream baseStream, int offset, int size)
    {
        _baseStream = baseStream ?? throw new ArgumentNullException(nameof(baseStream));
        if (offset < 0 || offset >= baseStream.Length)
            throw new ArgumentOutOfRangeException(nameof(offset), "Offset is out of the bounds of the stream.");

        _offset = offset;
        _size = size;

        // Seek to the offset in the base stream
        _baseStream.Seek(_offset, SeekOrigin.Begin);
    }

    public override bool CanRead => _baseStream.CanRead;
    public override bool CanSeek => false; // Read-only
    public override bool CanWrite => false; // Read-only
    public override long Length => _size;

    public override long Position
    {
        get => _baseStream.Position - _offset;
        set => throw new NotSupportedException("Setting position is not supported on a read-only stream.");
    }

    public override void Flush()
    {
        // No-op for read-only stream
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        if (buffer == null) throw new ArgumentNullException(nameof(buffer));
        if (offset < 0 || count < 0 || offset + count > buffer.Length) throw new ArgumentOutOfRangeException();

        return _baseStream.Read(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return origin switch
        {
            SeekOrigin.Begin => _baseStream.Seek(offset + _offset, origin),
            SeekOrigin.Current => _baseStream.Seek(offset, origin),
            _ => _baseStream.Seek(_offset + _size - offset, SeekOrigin.Begin)
        };
    }

    public override void SetLength(long value)
    {
        throw new NotSupportedException("Setting length is not supported on a read-only stream.");
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotSupportedException("Writing is not supported on a read-only stream.");
    }
}