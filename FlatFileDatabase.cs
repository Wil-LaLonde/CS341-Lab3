﻿using System.Text.Json;
using System.Collections.ObjectModel;

// https://www.dotnetperls.com/serialize-list
// https://www.daveoncsharp.com/2009/07/xml-serialization-of-collections/

namespace Lab2Solution {

    /// <summary>
    /// This is the database class, currently a FlatFileDatabase
    /// Leaving this in the codebase since this could be used for much faster local testing.
    /// </summary>
    public class FlatFileDatabase : IDatabase {
        string path = "";
        string filename = "clues.json";
        ObservableCollection<Entry> entries = new ObservableCollection<Entry>();
        JsonSerializerOptions options;

        /// <summary>
        /// FlatFileDatabase constructor
        /// </summary>
        public FlatFileDatabase() {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            path = $"{appDataPath}/{filename}"; // /data/user/0/com.companyname.basicdotnetmauiproject/files/clues.json
            Console.WriteLine($"We've got your path right here: {path}");

            GetEntries();
            options = new JsonSerializerOptions { WriteIndented = true };
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

                string jsonString = JsonSerializer.Serialize(entries, options);
                File.WriteAllText(path, jsonString);
                return true;
            } catch (IOException ioe) {
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
                string jsonString = JsonSerializer.Serialize(entries, options);
                File.WriteAllText(path, jsonString);
                return true;
            } catch (IOException ioe) {
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
                    entry.Date = replacementEntry.Date;         // change it then write it out

                    try {
                        string jsonString = JsonSerializer.Serialize(entries, options);
                        File.WriteAllText(path, jsonString);
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
            if (!File.Exists(path)) {
                File.CreateText(path);
                entries = new ObservableCollection<Entry>();
                return entries;
            }

            string jsonString = File.ReadAllText(path);
            if (jsonString.Length > 0) {
                entries = JsonSerializer.Deserialize<ObservableCollection<Entry>>(jsonString);
            }
            return entries;
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
