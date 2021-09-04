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

        private static void DBConnection (string path){
            sqliteConnection = new SQLiteConnection(path);
            sqliteConnection.Open();

        }

        private static void CreateDB (string nome){
        SQLiteConnection.CreateFile(nome);
        }


        #endregion


    }


}