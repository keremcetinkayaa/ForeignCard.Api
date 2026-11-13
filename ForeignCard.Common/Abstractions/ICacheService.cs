namespace ForeignCard.Common.Abstractions;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan duration);
    Task RemoveAsync(string key);

    // Cache'de varsa oradan ver; yoksa factory ile getir, anahtar ve süreyle kaydet
    Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan duration);
}