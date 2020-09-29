using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace GameWebApi
{
    public interface IRepository
    {
        Task<Player> Get(Guid id);
        Task<Player[]> GetAll();
        Task<Player> Create(Player player);
        Task<Player> Modify(Guid id, ModifiedPlayer player);
        Task<Player> Delete(Guid id);

        Task<Item> CreateItem(Guid playerId, Item item);
        Task<Item> GetItem(Guid playerId, Guid itemId);
        Task<Item[]> GetAllItems(Guid playerId);
        Task<Item> UpdateItem(Guid playerId, Item item);
        Task<Item> DeleteItem(Guid playerId, Item item);


        Task<Player[]> GetTop10Players();
        Task<Player> DeleteItemAndScore(Guid player_id, Guid item_id);
        Task<UpdateResult> PushItem(Guid id, Item new_item);
        Task<UpdateResult> IncrementScore(Guid id, int score_add);
        Task<UpdateResult> UpdatePlayerName(Guid id, string new_name);
        Task<Player[]> GetPlayersWithXItems(int itemAmount);
        Task<Player[]> GetItemWithProperty(int level);
        Task<Player[]> GetPlayersWithTag(string name);
        Task<Player> GetPlayerWithName(string name);
        Task<Player[]> GetPlayersWithXscore(int x);

    }
}