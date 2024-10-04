using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SorWpfApp
{
    internal class Bet
    {
        public int BetsID { get; set; }
        public DateOnly BetDate { get; set; }
        public float Odds { get; set; }
        public int Amount { get; set; }
        public int BettorsID { get; set; }
        public int EventID { get; set; }
        public int Status { get; set; }

        public Bet(MySqlDataReader olvaso)
        {
            BetsID = olvaso.GetInt32(0);
            BetDate = DateOnly.Parse($"{olvaso.GetDateTime(1).Year}-{olvaso.GetDateTime(1).Month}-{olvaso.GetDateTime(1).Day}");
            Odds = olvaso.GetFloat(2);
            Amount = olvaso.GetInt32(3);
            BettorsID = olvaso.GetInt32(4);
            EventID = olvaso.GetInt32(5);
            Status = olvaso.GetInt32(6);
        }



    }
}
