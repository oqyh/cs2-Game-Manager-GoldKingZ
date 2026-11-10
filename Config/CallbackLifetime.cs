namespace Game_Manager_GoldKingZ;
public sealed class CallbackLifetime
{
    private long generation;
    private readonly Dictionary<int, long> connections = new();
    public long Capture() => Interlocked.Read(ref generation);
    public bool IsCurrent(long captured) => Capture() == captured;
    public long CaptureConnection(int slot) => connections.GetValueOrDefault(slot);
    public void InvalidateConnection(int slot) => connections[slot] = CaptureConnection(slot) + 1;
    public void Invalidate()
    {
        Interlocked.Increment(ref generation);
        connections.Clear();
    }
}
