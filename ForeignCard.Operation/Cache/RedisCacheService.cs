using System.Text.Json;
using ForeignCard.Common.Abstractions;
using Microsoft.Extensions.Caching.Distributed;

namespace ForeignCard.Operation.Cache;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var json = await _cache.GetStringAsync(key);
            if (json == null)
                return default;

            return JsonSerializer.Deserialize<T>(json);
        }
        catch (Exception ex)
        {
            // Redis kapalıysa çökme, "cache'de yok" say
            Console.WriteLine($">>> Cache okunamadi ({key}): {ex.Message}");
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan duration)
    {
        try
        {
            var json = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, json, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = duration
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($">>> Cache yazilamadi ({key}): {ex.Message}");
        }
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            await _cache.RemoveAsync(key);
            Console.WriteLine($">>> Cache silindi: {key}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($">>> Cache silinemedi ({key}): {ex.Message}");
        }
    }

    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan duration)
    {
        var cached = await GetAsync<T>(key);
        if (cached != null)
        {
            Console.WriteLine($">>> CACHE HIT: {key}");
            return cached;
        }

        Console.WriteLine($">>> CACHE MISS: {key} (veritabanindan okunuyor)");
        var value = await factory();

        if (value != null)
            await SetAsync(key, value, duration);

        return value;
    }
}