using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SorWpfApp
{
    internal class Bettors
    {
        public int bettorsID { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }
        public int balance { get; set; }
        public string? email { get; set; }
        public DateOnly joinDate { get; set; }
        public bool isActive { get; set; }

        public Bettors(MySqlDataReader olvaso)
        {
            bettorsID = olvaso.GetInt32(0);
            username = olvaso.GetString(1);
            password = olvaso.GetString(2);
            balance = olvaso.GetInt32(3);
            email = olvaso.GetString(4);
            joinDate = DateOnly.Parse($"{olvaso.GetDateTime(5).Year}-{olvaso.GetDateTime(5).Month}-{olvaso.GetDateTime(5).Day}");
            isActive = olvaso.GetBoolean(6);
        }
    }
}
