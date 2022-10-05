using System.Text.Json;
using System.Collections.ObjectModel;
using Npgsql;
using Java.Sql;

// https://www.dotnetperls.com/serialize-list
// https://www.daveoncsharp.com/2009/07/xml-serialization-of-collections/


namespace Lab2Solution {

    /// <summary>
    /// This is the database class, currently a FlatFileDatabase
    /// </summary>
    public class RelationalDatabase : IDatabase {
        String connectionString;
        /// <summary>
        /// A local version of the database, we *might* want to keep this in the code and merely
        /// adjust it whenever Add(), Delete() or Edit() is called
        /// Alternatively, we could delete this, meaning we will reach out to bit.io for everything
        /// What are the costs of that?
        /// There are always tradeoffs in software engineering.
        /// </summary>
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
        public bool AddEntry(Entry entry) {
            try {
                entry.Id = entries.Count + 1;
                entries.Add(entry);

                // write the SQL to INSERT entry into bit.io

                return true;
            }
            catch (IOException ioe) {
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
        /// 
        /// <param name="entry">An entry, which is presumed to exist</param>
        public bool DeleteEntry(Entry entry) {
            try {
                var result = entries.Remove(entry);
                // Write the SQL to DELETE entry from bit.io. You have its id, that should be all that you 
                return true;
            }
            catch (IOException ioe) {
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
                       // write the SQL to UPDATE the entry. Again, you have its id, which should be all you need.

                       return true;
                    } catch (IOException ioe) {
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
                Entry storedEntry = new Entry(reader[1] as String, reader[2] as String, (int)reader[3], reader[4] as String, (int)reader[0]);
                entries.Add(storedEntry);
            }

            con.Close();

            return entries;
        }

        /// <summary>
        /// Creates the connection string to be utilized throughout the program
        /// 
        /// </summary>
        public String InitializeConnectionString() {
            var bitHost = "db.bit.io";
            var bitApiKey = "v2_3ub36_rHYqdxbHDc2GZTU4gRCiRYg"; // from the "Password" field of the "Connect" menu
            var bitUser = "WilLaLonde";
            var bitDbName = "WilLaLonde/Lab3Database";
            return $"Host={bitHost};Username={bitUser};Password={bitApiKey};Database={bitDbName}";
        }

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
