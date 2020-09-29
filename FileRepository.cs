using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace GameWebApi
{
    public class ListHolder
    {
        public List<Player> list_players = new List<Player>();
    }

    public class FileRepository : IRepository
    {
        public async Task<Player> Create(Player player)
        {
            ListHolder players = await ReadFile();
            players.list_players.Add(player);
            File.WriteAllText("game-dev.txt", JsonConvert.SerializeObject(players));

            return player;
        }

        public async Task<Player> Delete(Guid id)
        {
            ListHolder players = await ReadFile();

            for (int i = 0; i < players.list_players.Count; i++)
            {
                if (players.list_players[i].Id == id)
                {
                    Player deleted_p = players.list_players[i];
                    players.list_players.RemoveAt(i);
                    File.WriteAllText("game-dev.txt", JsonConvert.SerializeObject(players));
                    return deleted_p;
                }
            }

            //File.WriteAllText("game-dev.txt", JsonConvert.SerializeObject(players));
            return null;
        }

        public async Task<Player> Get(Guid id)
        {
            ListHolder players = await ReadFile();

            foreach (var p in players.list_players)
            {
                if (p.Id == id)
                {
                    return p;
                }
            }
            return null;
        }

        public async Task<Player[]> GetAll()
        {
            ListHolder players = await ReadFile();
            return players.list_players.ToArray();
        }

        public async Task<Player> Modify(Guid id, ModifiedPlayer player)
        {
            ListHolder players = await ReadFile();
            /*
            foreach (var p in from p in players.list_players
                              where p.Id == id
                              select p)
            {
                p.Score = player.Score;
                File.WriteAllText("game-dev.txt", JsonConvert.SerializeObject(players));
                return p;
            }
            */
            foreach (var p in players.list_players)
            {
                if (p.Id == id)
                {
                    p.Score = player.Score;
                    File.WriteAllText("game-dev.txt", JsonConvert.SerializeObject(players));
                    return p;
                }
            }
            return null;
        }

        public async Task<ListHolder> ReadFile()
        {
            var players = new ListHolder();
            string json = await File.ReadAllTextAsync("game-dev.txt");
            //return JsonConvert.DeserializeObject<ListHolder>(json);

            if (json.Length != 0)
            {
                return JsonConvert.DeserializeObject<ListHolder>(json);
            }

            return players;
        }
        /*
                public void WriteFile(String text)
                {
                    File.WriteAllText("game-dev.txt", JsonConvert.SerializeObject(text));
                }
        */

        public async Task<Item> CreateItem(Guid playerId, Item item)
        {
            return null;
        }
        public async Task<Item> GetItem(Guid playerId, Guid itemId)
        {
            return null;
        }
        public async Task<Item[]> GetAllItems(Guid playerId)
        {
            return null;
        }
        public async Task<Item> UpdateItem(Guid playerId, Item item)
        {
            return null;
        }
        public async Task<Item> DeleteItem(Guid playerId, Item item)
        {
            return null;
        }

        public Task<Player[]> GetPlayersWithXscore(int x)
        {
            throw new NotImplementedException();
        }

        public Task<Player[]> GetTop10Players()
        {
            throw new NotImplementedException();
        }

        public Task<Player> DeleteItemAndScore(Guid player_id, Guid item_id)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateResult> PushItem(Guid id, Item new_item)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateResult> IncrementScore(Guid id, int score_add)
        {
            throw new NotImplementedException();
        }

        public Task<UpdateResult> UpdatePlayerName(Guid id, string new_name)
        {
            throw new NotImplementedException();
        }

        public Task<Player[]> GetPlayersWithXItems(int itemAmount)
        {
            throw new NotImplementedException();
        }

        public Task<Player[]> GetItemWithProperty(int level)
        {
            throw new NotImplementedException();
        }

        public Task<Player[]> GetPlayersWithTag(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Player> GetPlayerWithName(string name)
        {
            throw new NotImplementedException();
        }
    }
}