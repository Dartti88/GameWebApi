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
            DateTime localDate = DateTime.Now;

            Player new_player = new Player();
            new_player.Name = player.Name;
            new_player.Id = Guid.NewGuid();
            new_player.Score = 0;
            new_player.Level = 0;
            new_player.IsBanned = false;
            new_player.CreationTime = localDate;

            await _irepository.Create(new_player);
            return null;
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

        [HttpGet]
        [Route("Delete/{id:Guid}")]
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


    }
}