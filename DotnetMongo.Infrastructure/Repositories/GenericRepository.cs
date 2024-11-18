using DotnetMongo.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

namespace DotnetMongo.Infrastructure.Repositories
{
    // Implementation of the generic repository for MongoDB
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IMongoCollection<T> _collection;

        // Constructor initializes the collection by accessing the MongoDB database
        public GenericRepository(IMongoDatabase database, string collectionName)
        {
            // Get the collection from MongoDB database using the provided collection name
            _collection = database.GetCollection<T>(collectionName);
        }
        // Adds a new entity to the MongoDB collection
        public async Task AddAsync(T entity)
        {
            // Insert the entity into the collection
            await _collection.InsertOneAsync(entity);
        }

        // Retrieves all entities of type T from the MongoDB collection
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            // Find all documents in the collection and convert them to a list
            return await _collection.Find(_ => true).ToListAsync();
        }

        // Retrieves a single entity by its ID from the MongoDB collection
        public async Task<T> GetByIdAsync(string id)
        {
            // Build the filter to find the entity by its ID and return the first matching result
            return await _collection.Find(Builders<T>.Filter.Eq("_id", id)).FirstOrDefaultAsync();
        }

        // Updates an existing entity in the MongoDB collection
        public async Task UpdateAsync(T entity)
        {
            // Build a filter using the entity's ID to find the document to update
            var filter = Builders<T>.Filter.Eq("_id", entity.GetType().GetProperty("Id").GetValue(entity, null));

            // Replace the old entity with the new one based on the filter
            await _collection.ReplaceOneAsync(filter, entity);
        }

        // Deletes an entity from the MongoDB collection by its ID
        public async Task DeleteAsync(string id)
        {
            // Build a filter to find the entity by its ID
            var filter = Builders<T>.Filter.Eq("_id", id);

            // Delete the entity matching the filter
            await _collection.DeleteOneAsync(filter);
        }
    }
}

// For Simple Applications:

// If your application has relatively simple requirements with basic CRUD operations for
// each collection, using a generic repository can be a good idea to reduce boilerplate and centralize your
// database operations.


// For Complex Applications:
// If your application has complex business logic, multiple different types of queries, or if MongoDB collections
// have different data structures, a specific repository for each collection may be more appropriate.