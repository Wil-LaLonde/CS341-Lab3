using System.Collections.ObjectModel;
using Npgsql;

// https://www.dotnetperls.com/serialize-list
// https://www.daveoncsharp.com/2009/07/xml-serialization-of-collections/

namespace Lab2Solution {

    /// <summary>
    /// This is the database class. This uses a bit.io database
    /// </summary>
    public class RelationalDatabase : IDatabase {
        string connectionString;
        ObservableCollection<Entry> entries = new ObservableCollection<Entry>();

        /// <summary>
        /// Here or thereabouts initialize a connectionString that will be used in all the SQL calls
        /// </summary>
        public RelationalDatabase() {
            connectionString = InitializeConnectionString();
        }

        /// <summary>
        /// Adds an entry to the database
        /// </summary>
        /// <param name="entry">the entry to add</param>
        /// <returns>true/false</returns>
        public bool AddEntry(Entry entry) {
            try {
                entry.Id = entries.Count + 1;
                entries.Add(entry);
                //Creating and opening the database connection.
                using var con = new NpgsqlConnection(connectionString);
                con.Open();
                //Writing the SQL insert statement.
                var sql = $"INSERT INTO Entry VALUES ({entry.Id}, '{entry.Clue}', '{entry.Answer}', {entry.Difficulty}, '{entry.Date}');";
                //Executing the query.
                using var cmd = new NpgsqlCommand(sql, con);
                cmd.ExecuteNonQuery();
                //Closing the connection.
                con.Close();
                return true;
            } catch (NpgsqlException ioe) {
                Console.WriteLine("Error while adding entry: {0}", ioe);
            }
            return false;
        }

        /// <summary>
        /// Finds a specific entry
        /// </summary>
        /// <param name="id">id to find</param>
        /// <returns>the Entry (if available)</returns>
        public Entry FindEntry(int id) {
            foreach (Entry entry in entries) {
                if (entry.Id == id) {
                    return entry;
                }
            }
            return null;
        }

        /// <summary>
        /// Deletes an entry 
        /// </summary>
        /// <param name="entry">An entry, which is presumed to exist</param>
        /// <returns>true/false</returns>
        public bool DeleteEntry(Entry entry) {
            try {
                var result = entries.Remove(entry);
                //Creating and opening the database connection.
                using var con = new NpgsqlConnection(connectionString);
                con.Open();
                //Writing the SQL delete statement.
                var sql = $"DELETE FROM Entry WHERE id = {entry.Id};";
                //Executing the query.
                using var cmd = new NpgsqlCommand(sql, con);
                cmd.ExecuteNonQuery();
                //Closing the connection.
                con.Close();
                return true;
            } catch (NpgsqlException ioe) {
                Console.WriteLine("Error while deleting entry: {0}", ioe);
            }
            return false;
        }

        /// <summary>
        /// Edits an entry
        /// </summary>
        /// <param name="replacementEntry"></param>
        /// <returns>true if editing was successful</returns>
        public bool EditEntry(Entry replacementEntry) {
            // iterate through entries until we find the Entry in question
            foreach (Entry entry in entries) {
                // found it
                if (entry.Id == replacementEntry.Id) {
                    entry.Answer = replacementEntry.Answer;
                    entry.Clue = replacementEntry.Clue;
                    entry.Difficulty = replacementEntry.Difficulty;
                    entry.Date = replacementEntry.Date;

                    try {
                        //Creating and opening the database connection.
                        using var con = new NpgsqlConnection(connectionString);
                        con.Open();
                        //Writing the SQL update statement.
                        var sql = $"UPDATE Entry SET clue = '{entry.Clue}', answer = '{entry.Answer}', difficulty = {entry.Difficulty}, date = '{entry.Date}' WHERE id = {entry.Id}";
                        //Executing the query.
                        using var cmd = new NpgsqlCommand(sql, con);
                        cmd.ExecuteNonQuery();
                        //Closing the connection.
                        con.Close();
                        return true;
                    } catch (NpgsqlException ioe) {
                        Console.WriteLine("Error while replacing entry: {0}", ioe);
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Retrieves all the entries
        /// </summary>
        /// <returns>all of the entries</returns>
        public ObservableCollection<Entry> GetEntries() {
            //Clear entries first before gathering all other entries.
            entries.Clear();
            try {
                using var con = new NpgsqlConnection(connectionString);
                con.Open();

                var sql = "SELECT * FROM Entry;";

                using var cmd = new NpgsqlCommand(sql, con);

                using NpgsqlDataReader reader = cmd.ExecuteReader();

                //Columns read as follows: id, clue, answer, difficulty, date
                //When making a new Entry, values are passed in as: clue, answer, difficulty, date, id

                //Database: id[0], clue[1], answer[2], difficulty[3], date[4]
                //Entry: clue[0], answer[1], difficulty[2], date[3], id[4]
                while (reader.Read()) {
                    //Creating a new Entry
                    Entry storedEntry = new Entry(reader[1] as string, reader[2] as string, (int)reader[3], reader[4] as string, (int)reader[0]);
                    entries.Add(storedEntry);
                }

                con.Close();
            } catch(NpgsqlException ieo) {
                Console.WriteLine("Error while gathering entries: {0}", ieo);
            }
            return entries;
        }

        /// <summary>
        /// Creates the connection string to be utilized throughout the program
        /// 
        /// </summary>
        public string InitializeConnectionString() {
            var bitHost = "db.bit.io";
            var bitApiKey = "v2_3ub36_rHYqdxbHDc2GZTU4gRCiRYg"; // from the "Password" field of the "Connect" menu
            var bitUser = "WilLaLonde";
            var bitDbName = "WilLaLonde/Lab3Database";
            return $"Host={bitHost};Username={bitUser};Password={bitApiKey};Database={bitDbName}";
        }

        /// <summary>
        /// Sorts the list based on the given sort type
        /// </summary>
        /// <param name="sortType">sort type</param>
        /// <returns>new sorted ObservableCollection</returns>
        public ObservableCollection<Entry> EntryListSort(SortType sortType) {
            //Checking to see what sorting type we are
            switch (sortType) {
                case SortType.ClueSort:
                    entries = new ObservableCollection<Entry>(entries.OrderBy(entry => entry.Clue));
                    break;
                case SortType.AnswerSort:
                    entries = new ObservableCollection<Entry>(entries.OrderBy(entry => entry.Answer));
                    break;
            }
            return entries;
        }
    }
}
