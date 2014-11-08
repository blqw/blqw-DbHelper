using blqw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace demo
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = DBHelper.Create("default"))
            {
                db.Begin();
                var i = db.ExecuteNonQuery("INSERT INTO test2 (aaaa)VALUES('a')");
                InnerEvent();
                i = db.ExecuteNonQuery("INSERT INTO test2 (aaaa)VALUES('b')");
                db.Commit();
                Console.WriteLine(i);
            }
        }

        static void InnerEvent()
        {
            using (var db = DBHelper.Create("default"))
            {
                db.Begin();
                db.ExecuteNonQuery("INSERT INTO test2 (aaaa)VALUES('c')");
                db.Rollback();
            }
        }
    }
}
