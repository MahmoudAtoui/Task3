using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Task3.Models;
namespace Task3.Services;

public class LogsService
{
    private readonly IMongoCollection<Log> _logs;
    
    public LogsService(IOptions<UmsdbSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _logs = database.GetCollection<Log>(settings.Value.LogsCollectionName);
    }
    
    public async Task<List<Log>> GetAsync() =>
        await _logs.Find(log => true).ToListAsync();
    
    public async Task<Log?> GetAsync(string id) =>
        await _logs.Find(log => log.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(Log log) =>
        await _logs.InsertOneAsync(log);

    public async Task UpdateAsync(string id, Log log) =>
        await _logs.ReplaceOneAsync(log => log.Id == id, log);
    
    public async Task RemoveAsync(string id) =>
        await _logs.DeleteOneAsync(log => log.Id == id);
    
}