using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GameWebApi
{
    public class MongoDbRepository : IRepository
    {
        private readonly IMongoCollection<Player> _playerCollection;
        private readonly IMongoCollection<BsonDocument> _bsonDocumentCollection;

        public MongoDbRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("game");
            _playerCollection = database.GetCollection<Player>("players");

            _bsonDocumentCollection = database.GetCollection<BsonDocument>("players");
        }

        public async Task<Player> Create(Player player)
        {
            await _playerCollection.InsertOneAsync(player);
            return player;
        }

        public async Task<Player> Delete(Guid id)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, id);
            return await _playerCollection.FindOneAndDeleteAsync(filter);
        }

        public async Task<Player> Get(Guid id)
        {
            var filter = Builders<Player>.Filter.Eq(player => player.Id, id);
            return await _playerCollection.Find(filter).FirstAsync();
        }

        public async Task<Player[]> GetAll()
        {
            var filter = Builders<Player>.Filter.Empty;
            var players = await _playerCollection.Find(filter).ToListAsync();
            //var players = await _playerCollection.Find(new BsonDocument()).ToListAsync();
            return players.ToArray();
        }

        public async Task<Player> Modify(Guid id, ModifiedPlayer player)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, id);
            Player returnPlayer = await _playerCollection.Find(filter).FirstAsync();
            returnPlayer.Score = player.Score;
            await _playerCollection.ReplaceOneAsync(filter, returnPlayer);
            return returnPlayer;
        }

        public async Task<Item> CreateItem(Guid playerId, Item item)
        {
            Player player = await Get(playerId);
            player.list_items.Add(item);
            var filter = Builders<Player>.Filter.Eq(player => player.Id, playerId);
            await _playerCollection.ReplaceOneAsync(filter, player);
            return item;
        }
        public async Task<Item> GetItem(Guid playerId, Guid itemId)
        {
            Player player = await Get(playerId);
            //var filter = Builders<Item>.Filter.Eq(item => item.Id, itemId);

            for (int i = 0; i < player.list_items.Count; i++)
            {
                if (player.list_items[i].Id == itemId)
                    return player.list_items[i];
            }

            return null;
        }
        public async Task<Item[]> GetAllItems(Guid playerId)
        {
            Player player = await Get(playerId);
            return player.list_items.ToArray();
        }

        public async Task<Item> UpdateItem(Guid playerId, Item item)
        {
            Player player = await Get(playerId);

            foreach (var i in player.list_items)
            {
                if (i.Id == item.Id)
                {
                    i.Level = item.Level;
                    var filter_player = Builders<Player>.Filter.Eq(player => player.Id, playerId);
                    await _playerCollection.ReplaceOneAsync(filter_player, player);
                    return i;
                }
            }

            return null;
        }
        public async Task<Item> DeleteItem(Guid playerId, Item item)
        {
            Player player = await Get(playerId);

            for (int i = 0; i < player.list_items.Count; i++)
            {
                if (player.list_items[i].Id == item.Id)
                {
                    player.list_items.RemoveAt(i);
                    var filter_player = Builders<Player>.Filter.Eq(player => player.Id, playerId);
                    await _playerCollection.ReplaceOneAsync(filter_player, player);
                    return item;
                }
            }

            return null;
        }

        public async Task<Player[]> GetPlayersWithXscore(int x)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Gte("Score", x);
            List<Player> players = await _playerCollection.Find(filter).ToListAsync();

            return players.ToArray();
        }

        public async Task<Player> GetPlayerWithName(string name)
        {
            var filter = Builders<Player>.Filter.Eq("Name", name);
            return await _playerCollection.Find(filter).FirstAsync();
        }

        public async Task<Player[]> GetPlayersWithTag(string name)
        {
            var filter = Builders<Player>.Filter.Eq("Tag", "active");
            var players = await _playerCollection.Find(filter).ToListAsync();
            return players.ToArray();
        }

        public async Task<Player[]> GetItemWithProperty(int level)
        {
            var filter = Builders<Player>.Filter.ElemMatch<Item>(p => p.list_items, Builders<Item>.Filter.Eq(i => i.Level, level));
            List<Player> players = await _playerCollection.Find(filter).ToListAsync();

            return players.ToArray(); ;
        }

        public async Task<Player[]> GetPlayersWithXItems(int itemAmount)
        {
            var filter = Builders<Player>.Filter.Size(p => p.list_items, itemAmount);
            List<Player> players = await _playerCollection.Find(filter).ToListAsync();

            return players.ToArray(); ;
        }

        public async Task<UpdateResult> UpdatePlayerName(Guid id, string new_name)
        {
            var filter = Builders<Player>.Filter.Eq("Id", id);
            var update = Builders<Player>.Update.Set("Name", new_name);
            return await _playerCollection.UpdateOneAsync(filter, update);
        }

        public async Task<UpdateResult> IncrementScore(Guid id, int score_add)
        {
            var filter = Builders<Player>.Filter.Eq("Id", id);
            var update = Builders<Player>.Update.Inc("Score", score_add);
            return await _playerCollection.UpdateOneAsync(filter, update);
        }

        public async Task<UpdateResult> PushItem(Guid id, Item new_item)
        {
            var filter = Builders<Player>.Filter.Eq("Id", id);
            var update = Builders<Player>.Update.Push("list_items", new_item);
            return await _playerCollection.UpdateOneAsync(filter, update);
        }

        public async Task<Player> DeleteItemAndScore(Guid player_id, Guid item_id)
        {
            /*
            await IncrementScore(player_id, 10);
            var filter_1 = Builders<Player>.Filter.Eq("Id", player_id);
            var filter_2 = Builders<Player>.Filter.ElemMatch<Item>(p => p.list_items, Builders<Item>.Filter.Eq(i => i.Id, item_id));
            var andFilter = Builders<Player>.Filter.And(filter_1, filter_2);
            return await _playerCollection.DeleteOneAsync(andFilter);
            */
            var playerFilter = Builders<Player>.Filter.Eq(p => p.Id, player_id);
            var updateScore = Builders<Player>.Update.Inc("Score", 10);
            await _playerCollection.FindOneAndUpdateAsync(playerFilter, updateScore);

            var filterItem = Builders<Player>.Filter.ElemMatch<Item>(p => p.list_items, Builders<Item>.Filter.Eq(i => i.Id, item_id));
            return await _playerCollection.FindOneAndDeleteAsync(filterItem);
        }

        public async Task<Player[]> GetTop10Players()
        {
            //return await _playerCollection.Find("").Sort(Builders<Player>.Sort.Descending("Score")).Limit(10).ToListAsync<Player>();

            var filter = Builders<Player>.Filter.Empty;
            SortDefinition<Player> sortDef = Builders<Player>.Sort.Descending("Score");
            List<Player> players = await _playerCollection.Find(filter).Sort(sortDef).Limit(10).ToListAsync();
            return players.ToArray();
        }

        public async Task<LevelCount> FindMostCommonLevel()
        {
            return await (Task<LevelCount>)_playerCollection
                .Aggregate()
                .Project(p => p.Level)
                .Group(l => l, p => new LevelCount { Id = p.Key, Count = p.Sum() })
                .SortByDescending(l => l.Count)
                .Limit(1);
        }
    }

    public class LevelCount
    {
        public int Id { get; set; }
        public int Count { get; set; }
    };

}