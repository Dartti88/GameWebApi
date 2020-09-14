using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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
                    players.list_players.RemoveAt(i);
                    File.WriteAllText("game-dev.txt", JsonConvert.SerializeObject(players));
                    return null;
                }
            }

            //File.WriteAllText("game-dev.txt", JsonConvert.SerializeObject(players));
            return null;
        }

        public async Task<Player> Get(Guid id)
        {
            ListHolder players = await ReadFile();
            var result = new Player();
            foreach (var p in players.list_players)
            {
                if (p.Id == id)
                {
                    result = p;
                    break;
                }
            }
            return result;
        }

        public async Task<Player[]> GetAll()
        {
            //string stations = await response.Content.ReadAsStringAsync();
            //BikeRentalStationList list = JsonConvert.DeserializeObject<BikeRentalStationList>(stations);
            ListHolder players = await ReadFile();
            //Task<Player[]> players = JsonConvert.DeserializeObject<Task<Player[]>>(text);
            return players.list_players.ToArray();
        }

        public async Task<Player> Modify(Guid id, ModifiedPlayer player)
        {
            ListHolder players = await ReadFile();
            var result = new Player();

            foreach (var p in players.list_players)
            {
                if (p.Id == id)
                {
                    p.Score = player.Score;
                    File.WriteAllText("game-dev.txt", JsonConvert.SerializeObject(players));
                    break;
                }
            }
            return result;
        }

        public async Task<ListHolder> ReadFile()
        {
            var players = new ListHolder();
            string json = await File.ReadAllTextAsync("game-dev.txt");
            //return JsonConvert.DeserializeObject<ListHolder>(json);

            if (File.ReadAllText("game-dev.txt").Length != 0)
            {
                return JsonConvert.DeserializeObject<ListHolder>(json);
            }

            return players;
        }

        public void WriteFile(String text)
        {
            File.WriteAllText("game-dev.txt", JsonConvert.SerializeObject(text));
        }

    }
}