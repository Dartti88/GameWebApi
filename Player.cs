using System;
using System.Threading.Tasks;

namespace GameWebApi
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public int Level { get; set; }
        public bool IsBanned { get; set; }
        public DateTime CreationTime { get; set; }

        public static implicit operator Task<object>(Player v)
        {
            throw new NotImplementedException();
        }
    }
}