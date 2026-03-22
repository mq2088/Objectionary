using Objectionary.Services;

namespace Objectionary.Tests.Services;

public class CircuitBreakerTests
{
    [Fact]
    public void Allows_requests_under_limit()
    {
        var breaker = new CircuitBreaker(maxRequests: 5);
        Assert.True(breaker.TryAcquire());
        Assert.True(breaker.TryAcquire());
    }

    [Fact]
    public void Blocks_requests_at_limit()
    {
        var breaker = new CircuitBreaker(maxRequests: 3);
        Assert.True(breaker.TryAcquire());
        Assert.True(breaker.TryAcquire());
        Assert.True(breaker.TryAcquire());
        Assert.False(breaker.TryAcquire());
    }

    [Fact]
    public void Count_tracks_acquisitions()
    {
        var breaker = new CircuitBreaker(maxRequests: 100);
        breaker.TryAcquire();
        breaker.TryAcquire();
        Assert.Equal(2, breaker.Count);
    }
}
