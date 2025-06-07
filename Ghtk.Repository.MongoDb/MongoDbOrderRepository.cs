using Ghtk.Repository.Entities;
using MongoDB.Driver;

namespace Ghtk.Repository
{
    public class MongoDbOrderRepository : IOrderRepository
    {
        private readonly MongoClient mongoClient;
        private readonly IMongoDatabase mongoDatabase;
        private readonly IMongoCollection<Order> orderCollections;

        public MongoDbOrderRepository(MongoClient mongoClient)
        {
            this.mongoClient = mongoClient;
            this.mongoDatabase = this.mongoClient.GetDatabase("ghtk");
            this.orderCollections = this.mongoDatabase.GetCollection<Order>("orders");
        }

        public async Task<bool> CancelOrderAsync(string trackingId, string partnerId)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.TrackingId, trackingId)
                & Builders<Order>.Filter.Eq(p => p.PartnerId, partnerId);
            var update = Builders<Order>.Update.Set(o => o.Status, -1);
            var res = await orderCollections.UpdateOneAsync(filter, update);

            return res.ModifiedCount > 0;
        }

        public async Task CreateOrderAsync(Order orderEntity)
        {
            await orderCollections.InsertOneAsync(orderEntity);
        }

        public async Task<Order> FindOrderAsync(string trackingId, string partnerId)
        {
            var filter = Builders<Order>.Filter.Eq(o => o.TrackingId, trackingId) 
                & Builders<Order>.Filter.Eq(p => p.PartnerId, partnerId);
            return await orderCollections.Find(filter).FirstOrDefaultAsync();
        }
    }
}
