using System.Text.Json;
using ForeignCard.Common.Abstractions;
using Microsoft.Extensions.Caching.Distributed;

namespace ForeignCard.Operation.Cache;

public class RedisCacheService : ICacheService
{
    // Redis hata verince bu süre boyunca hiç denenmez (devre kesici)
    private static readonly TimeSpan BreakDuration = TimeSpan.FromSeconds(30);

    private readonly IDistributedCache _cache;

    // Devrenin açık kalacağı son an (UTC tick) ve Redis'in kapalı olup olmadığı
    private long _skipUntilTicks;
    private int _isDown; // 0 = çalışıyor, 1 = kapalı

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key)
    {
        if (IsBreakerOpen())
            return default;

        try
        {
            var json = await _cache.GetStringAsync(key);
            MarkUp();

            return json == null ? default : JsonSerializer.Deserialize<T>(json);
        }
        catch (Exception ex)
        {
            Trip("okuma", key, ex);
            return default;
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan duration)
    {
        if (IsBreakerOpen())
            return;

        try
        {
            var json = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, json, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = duration
            });
            MarkUp();
        }
        catch (Exception ex)
        {
            Trip("yazma", key, ex);
        }
    }

    public async Task RemoveAsync(string key)
    {
        // Silme, devre açıkken de denenir: veri değiştiyse eski kayıt Redis'te kalmamalı
        try
        {
            await _cache.RemoveAsync(key);
            MarkUp();
            Console.WriteLine($">>> Cache silindi: {key}");
        }
        catch (Exception ex)
        {
            Trip("silme", key, ex);
        }
    }

    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan duration)
    {
        if (IsBreakerOpen())
        {
            Console.WriteLine($">>> Cache devre disi, veritabanindan okunuyor: {key}");
            return await factory();
        }

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

    // ---- Devre kesici yardımcıları ----

    private bool IsBreakerOpen()
        => DateTime.UtcNow.Ticks < Interlocked.Read(ref _skipUntilTicks);

    private void Trip(string action, string key, Exception ex)
    {
        Interlocked.Exchange(ref _skipUntilTicks, DateTime.UtcNow.Add(BreakDuration).Ticks);

        // Uzun hata metni yerine sadece ilk kısmı; "kapandı" mesajı bir kez yazılır
        var shortMessage = ex.Message.Split(" - ")[0];
        if (Interlocked.Exchange(ref _isDown, 1) == 0)
            Console.WriteLine($">>> Redis'e ulasilamadi ({action}: {key}). {BreakDuration.TotalSeconds} sn boyunca cache atlaniyor. Sebep: {shortMessage}");
    }

    private void MarkUp()
    {
        if (Interlocked.Exchange(ref _isDown, 0) == 1)
            Console.WriteLine(">>> Redis tekrar erisilebilir, cache yeniden aktif.");
    }
}