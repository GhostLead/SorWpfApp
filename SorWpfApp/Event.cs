using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SorWpfApp
{
    public class Event
    {
        public int EventID { get; set; }
        public string EventName { get; set; }
        public DateOnly EventDate { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }

        public Event(MySqlDataReader olvaso)
        {
            EventID = olvaso.GetInt32(0);
            EventName = olvaso.GetString(1);
            EventDate = DateOnly.Parse($"{olvaso.GetDateTime(2).Year}-{olvaso.GetDateTime(2).Month}-{olvaso.GetDateTime(2).Day}");
            Category = olvaso.GetString(3);
            Location = olvaso.GetString(4);
        }


    }
}
