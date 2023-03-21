using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Product.Infrastructure.Outbox.Stores.MongoDb;
public class MongoDbOutboxStore : IOutboxStore
{
    private readonly IMongoCollection<OutboxMessage> _outboxMessages;

    public MongoDbOutboxStore(IOptions<MongoDbOutboxOptions> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var database = client.GetDatabase(options.Value.DatabaseName);
        _outboxMessages = database.GetCollection<OutboxMessage>(options.Value.CollectionName);
    }

    public async Task Add(OutboxMessage message) => await _outboxMessages.InsertOneAsync(message);

    public async Task Delete(IEnumerable<Guid> ids) 
        => await _outboxMessages.DeleteManyAsync(Builders<OutboxMessage>.Filter.In(d => d.Id, ids));

    public async Task<OutboxMessage> GetMessage(Guid id) 
        => await _outboxMessages.Find(Builders<OutboxMessage>.Filter.Where(d => d.Id == id)).FirstOrDefaultAsync();

    public async Task<IEnumerable<Guid>> GetUnprocessedMessageIds()
    {
        var filter = Builders<OutboxMessage>.Filter.Where(d => !d.Processed.HasValue);
        var cursor = await _outboxMessages.Find(filter).ToCursorAsync();
        return cursor.ToList().Select(c => c.Id);
    }

    public async Task SetMessageToProcessed(Guid id)
    {
        var filter = Builders<OutboxMessage>.Filter.Where(d => d.Id == id);
        var update = Builders<OutboxMessage>.Update.Set(x => x.Processed, DateTime.UtcNow);

        var result = await _outboxMessages.UpdateOneAsync(filter, update);

        if (result.ModifiedCount == default)
            throw new Exception($"Did not modify message '{id}'");
        
    }
}
