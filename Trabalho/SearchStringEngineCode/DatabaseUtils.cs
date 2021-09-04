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
        private static void DBConnection (string path)
        {
            sqliteConnection = new SQLiteConnection(path);
            sqliteConnection.Open();

        }

        // Use SQLite site to create tables and 
        private static void CreateDB (string nome)
        {
            //SQLiteConnection.CreateFile(nome);
            using (var cmd = sqliteConnection.CreateCommand())
            {
                cmd.CommandText = "CREATE TABLE WORDS (ID INTEGER NOT NULL, WORD VARCHAR, PRIMARY KEY(ID AUTOINCREMENT));";
                cmd.CommandText = "CREATE TABLE FILES (ID INTEGER NOT NULL, FILENAME VARCHAR, PRIMARY KEY(ID AUTOINCREMENT));";
                cmd.CommandText = "CREATE TABLE WORDSANDFILES (ID INTEGER NOT NULL, WORD_ID INTEGER NOT NULL, FILE_ID INTEGER NOT NULL, OCCURRENCES	INTEGER NOT NULL, FOREIGN KEY(FILE_ID) REFERENCES FILES, PRIMARY KEY(ID AUTOINCREMENT), FOREIGN KEY(WORD_ID) REFERENCES WORDS);";
            }
        }

        // Execute just one time;
        // private static void CreateTable ()
        // {
        //     using (var cmd = sqliteConnection.CreateComand())
        //     {
        //         cmd.CommandText = "CREATE TABLE IF NOT EXISTS FileData(File Varchar(40), SearchWord Varchar(30), NumberRep int)"

        //     }
        // }

        private static void WordValidation(string FNamee, string SWordd)
        {
            string Fname = Fnamee;
            string SWord = SWordd;
            //TODO: Verify SQL return, and do a IF condition to prevent duplicate words.
            using (var cmd = sqliteConnection.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(*) FROM FileData WHERE File LIKE '@FName' AND SearchWord LIKE '@SWord';";
                cmd.Parameters.AddWithValue(@FName,Fname);
                cmd.Parameters.AddWithValue(@SWord,SWord);
                cmd.ExecuteNonQuery();
            }
        }

        private static void SaveIntoBD(string FNamee, string SWordd, int NRepp)
        {
             string Fname = Fnamee;
             string SWord = SWordd;
             string NRep = NRepp;

            using (var cmd = sqliteConnection.CreateCommand())
            {
                cmd.CommandText = @"INSERT INTO FileData(File, SearchWord, NumberRep) values (@FName, @SWord, @NRep);";

                cmd.Parameters.AddWithValue(@FName,Fname);
                cmd.Parameters.AddWithValue(@SWord,SWord);
                cmd.Parameters.AddWithValue(@NRep,NRep);
                cmd.ExecuteNonQuery();
            }
        }

        private static void ReadDB()
        {
            using (var cmd = new System.Data.SQLite.SQLiteCommand(sqliteConnection))
            {
                cmd.CommandText = "SELECT File, SearchWord, NumberRep from FileData;";


            }

        }


        #endregion


    }


}