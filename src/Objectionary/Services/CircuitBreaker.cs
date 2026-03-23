namespace Objectionary.Services;

public class CircuitBreaker
{
    private readonly int _maxRequests;
    private int _count;

    public int Count => _count;

    public CircuitBreaker(int maxRequests)
    {
        _maxRequests = maxRequests;
    }

    public bool TryAcquire()
    {
        var current = Interlocked.Increment(ref _count);
        if (current > _maxRequests)
        {
            return false;
        }
        return true;
    }
}
