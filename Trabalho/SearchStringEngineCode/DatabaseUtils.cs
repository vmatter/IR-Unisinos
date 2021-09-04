using System.Data;
using System.Data.SQLite;

// This class is used to connect the Database

namespace SearchStringHandler
{
    public static class DatabaseUtils
    {
        #region Connection
        //Create the conection with Database
        private static SQLiteConnection sqliteConnection;

        // Connect DB
        private static void DBConnection (string path){
            sqliteConnection = new SQLiteConnection(path);
            sqliteConnection.Open();

        }
        // Use SQLite site to create tables and 
        private static void CreateDB (string nome){
            SQLiteConnection.CreateFile(nome);
        }

        private static void ReadDB()
        {
            using (var cmd = new System.Data.SQLite.SQLiteCommand(sqliteConnection))
            {
                
            }

        }


        #endregion


    }


}