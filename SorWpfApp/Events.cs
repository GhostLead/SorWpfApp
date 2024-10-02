using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SorWpfApp
{
    internal class Events
    {
        public int eventID { get; set; }
        public string? eventName { get; set; }
        public DateOnly eventDate { get; set; }
        public string? category { get; set; }
        public string? location { get; set; }

        public Events(MySqlDataReader olvaso)
        {
            eventID = olvaso.GetInt32(0);
            eventName = olvaso.GetString(1);
            eventDate = DateOnly.Parse($"{olvaso.GetDateTime(2).Year}-{olvaso.GetDateTime(2).Month}-{olvaso.GetDateTime(2).Day}");
            category = olvaso.GetString(3);
            location = olvaso.GetString(4);
        }


    }
}
