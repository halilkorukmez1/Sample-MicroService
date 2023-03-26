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

    public async Task Delete(List<Guid> ids) 
        => await _outboxMessages.DeleteManyAsync(Builders<OutboxMessage>.Filter.In(d => d.Id, ids));

    public async Task<OutboxMessage> GetMessage(Guid id) 
        => await _outboxMessages.Find(Builders<OutboxMessage>.Filter.Where(d => d.Id == id)).FirstOrDefaultAsync();

    public async Task<IEnumerable<Guid>> GetUnprocessedMessageIds()
    {
        var result = await _outboxMessages.FindAsync(d => !d.Processed.HasValue && d.Status == false && d.Retry <= 5);
        return result.ToList().Select(c => c.Id);
    }

    public async Task SetMessageToProcessed(Guid id)
    {
        var message = await _outboxMessages.Find(x => x.Id == id).FirstOrDefaultAsync();
        message.Status = true;
        message.Processed = DateTime.Now;

        var result = await _outboxMessages.ReplaceOneAsync(Builders<OutboxMessage>.Filter.Eq("Id", id), message);

        if (result.ModifiedCount == default)
            throw new Exception($"Did not modify message '{id}'");
    }
}
