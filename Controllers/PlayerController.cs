using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameWebApi.Controllers
{
    [ApiController]
    [Route("player")]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerController> _logger;
        private readonly IRepository _irepository;

        public PlayerController(ILogger<PlayerController> logger, IRepository irepository)
        {
            _logger = logger;
            _irepository = irepository;
        }

        [HttpPost] //{"Name":"Tommi"}
        [Route("create")]
        public async Task<Player> Create([FromBody] NewPlayer player)
        {
            DateTime localDate = DateTime.UtcNow;

            Player new_player = new Player();
            new_player.Name = player.Name;
            new_player.Id = Guid.NewGuid();
            new_player.Score = 0;
            new_player.Level = 0;
            new_player.IsBanned = false;
            new_player.CreationTime = localDate;

            await _irepository.Create(new_player);
            return new_player;
        }
        /*
        [HttpGet]
        [Route("create/{name}")]
        public void new_new_player(string name)
        {
            NewPlayer new_player = new NewPlayer();
            new_player.Name = name;
        Create(new_player);

        //_logger.LogInformation("test");
    }

    public Player Create(NewPlayer _player)
    {
        DateTime localDate = DateTime.Now;

        Player new_player = new Player();
        new_player.Name = _player.Name;
        new_player.Id = Guid.NewGuid();
        new_player.Score = 0;
        new_player.Level = 0;
        new_player.IsBanned = false;
        new_player.CreationTime = localDate;
        _irepository.Create(new_player);
        return new_player;
    }
*/
        [HttpGet]
        [Route("ListPlayers")]
        public Task<Player[]> GetAll()
        {
            Task<Player[]> list_players = _irepository.GetAll();
            return list_players;
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<Player> Delete(Guid id)
        {
            await _irepository.Delete(id);
            return null;
        }


        [HttpGet]
        [Route("Get/{id:Guid}")]
        public async Task<Player> Get(Guid id)
        {
            return await _irepository.Get(id);
        }

        [HttpPost] //{"Score":5}
        [Route("Modify/{id:Guid}")]
        public async Task<Player> Modify(Guid id, [FromBody] ModifiedPlayer player)
        {
            await _irepository.Modify(id, player);
            return null;
        }

        [HttpGet] //GetPlayersWithXscore?minScore=x
        [Route("GetPlayersWithXscore")]
        public async Task<Player> GetPlayersWithXscore(int minScore)
        {
            await _irepository.GetPlayersWithXscore(minScore);
            return null;
        }

        [HttpGet] //"Anna"
        [Route("name:string")]
        public async Task<Player> GetPlayerWithName(string name)
        {
            await _irepository.GetPlayerWithName(name);
            return null;
        }

        [HttpGet] //"Anna"
        [Route("GetPlayersWithTag/tag:string")]
        public async Task<Player> GetPlayersWithTag(string tag)
        {
            await _irepository.GetPlayersWithTag(tag);
            return null;
        }

        [HttpGet] //"Anna"
        [Route("GetItemWithProperty/level:int")]
        public async Task<Player> GetItemWithProperty(int level)
        {
            await _irepository.GetItemWithProperty(level);
            return null;
        }

        [HttpGet] //"Anna"
        [Route("GetPlayersWithXItems/itemAmount:int")]
        public async Task<Player> GetPlayersWithXItems(int itemAmount)
        {
            await _irepository.GetPlayersWithXItems(itemAmount);
            return null;
        }

        [HttpGet] //"Anna"
        [Route("UpdatePlayerName/new_name:string")]
        public async Task<Player> UpdatePlayerName(Guid id, string new_name)
        {
            await _irepository.UpdatePlayerName(id, new_name);
            return null;
        }

        [HttpGet]
        [Route("IncrementScore/score_add:int")]
        public async Task<Player> IncrementScore(Guid id, int score_add)
        {
            await _irepository.IncrementScore(id, score_add);
            return null;
        }

        [HttpPost] //{"Score":5}
        [Route("PushItem/{id:Guid}")]
        public async Task<Player> PushItem(Guid id, [FromBody] Item item)
        {
            await _irepository.PushItem(id, item);
            return null;
        }


        [HttpDelete]
        [Route("{player_id:Guid}/items/{items_id:Guid}")]
        public async Task<Player> DeleteItemAndScore(Guid player_id, Guid items_id)
        {
            await _irepository.DeleteItemAndScore(player_id, items_id);
            return null;
        }

        [HttpGet]
        [Route("GetTop10Players")]
        public async Task<Player> GetTop10Players()
        {
            await _irepository.GetTop10Players();
            return null;
        }

    }
}