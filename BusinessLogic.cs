using System.Collections.ObjectModel;

namespace Lab2Solution {

    /// <summary>
    /// Handles the BusinessLogic
    /// </summary>
    public class BusinessLogic : IBusinessLogic {
        //Different constants used to check user input.
        const int MIN_CLUE_ANSWER_LENGTH = 1;
        const int MAX_CLUE_LENGTH = 250;
        const int MAX_ANSWER_LENGTH = 25;
        const int MIN_DIFFICULTY = 0;
        const int MAX_DIFFICULTY = 2;
        const string DATE_FORMAT = "mm/dd/yyyy";
        //Keeping track of the id number
        int latestId = 0;

        //IDatabase db; // Local Database
        IDatabase relationalDb; //bit.io Database

        /// <summary>
        /// BusinessLogic constructor. Makes a new relationDb object.
        /// </summary>
        public BusinessLogic() {
            //db = new FlatFileDatabase();
            relationalDb = new RelationalDatabase();
        }

        /// <summary>
        /// Represents all entries
        /// This also could have been a property
        /// </summary>
        /// <returns>ObservableCollection of entries</returns>
        public ObservableCollection<Entry> GetEntries() {
            return relationalDb.GetEntries();
        }

        /// <summary>
        /// FindEntry call the database to try and find an entry.
        /// </summary>
        /// <param name="id">id to try and find</param>
        /// <returns>an entry if it was found, otherwise null</returns>
        public Entry FindEntry(int id) {
            return relationalDb.FindEntry(id);
        }

        /// <summary>
        /// Verifies that all the entry fields are valid
        /// </summary>
        /// <param name="clue">entry clue</param>
        /// <param name="answer">entry answer</param>
        /// <param name="difficulty">entry difficulty</param>
        /// <param name="date">entry date</param>
        /// <returns>an error if there is an error, EntryError.NoError otherwise</returns>
        private EntryError CheckEntryFields(string clue, string answer, int difficulty, string date) {
            //Clue check
            if (clue.Length < MIN_CLUE_ANSWER_LENGTH || clue.Length > MAX_CLUE_LENGTH) {
                return EntryError.InvalidClueLength;
            }
            //Answer check
            if (answer.Length < MIN_CLUE_ANSWER_LENGTH || answer.Length > MAX_ANSWER_LENGTH) {
                return EntryError.InvalidAnswerLength;
            }
            //Date check
            bool invalidDate = !DateTime.TryParseExact(date, DATE_FORMAT,
                                                    System.Globalization.CultureInfo.InvariantCulture,
                                                    System.Globalization.DateTimeStyles.None,
                                                    out _);
            if (invalidDate) {
                return EntryError.InvalidDate;
            }
            //Difficulty check
            if (difficulty < MIN_DIFFICULTY || difficulty > MAX_DIFFICULTY) {
                return EntryError.InvalidDifficulty;
            }
            return EntryError.NoError;
        }


        /// <summary>
        /// Adds an entry
        /// </summary>
        /// <param name="clue">entry clue</param>
        /// <param name="answer">entry answer</param>
        /// <param name="difficulty">entry difficulty</param>
        /// <param name="date">entry date</param>
        /// <returns>an error if there is an error, EntryError.NoError otherwise</returns>
        public EntryError AddEntry(string clue, string answer, int difficulty, string date) {
            //Checking entry data.
            var result = CheckEntryFields(clue, answer, difficulty, date);
            if (result != EntryError.NoError) {
                return result;
            }
            //Making a new Entry.
            Entry entry = new Entry(clue, answer, difficulty, date, ++latestId);
            //Calling the database
            bool success = relationalDb.AddEntry(entry);
            //If there was a problem, return the error message.
            if (!success) {
                return EntryError.DBAddError;
            }
            return EntryError.NoError;
        }

        /// <summary>
        /// Deletes an entry
        /// </summary>
        /// <param name="entryId">entry Id</param>
        /// <returns>an erreor if there is one, EntryError.NoError otherwise</returns>
        public EntryError DeleteEntry(int entryId) {
            //Seeing if the entry exists
            var entry = relationalDb.FindEntry(entryId);
            //If so, make a call to the database.
            if (entry != null) {
                bool success = relationalDb.DeleteEntry(entry);
                //If the database delete was good, return no error.
                if (success) {
                    return EntryError.NoError;

                } else {
                    return EntryError.DBDeleteError;
                }
            } else {
                return EntryError.EntryNotFound;
            }
        }

        /// <summary>
        /// Edits an Entry
        /// </summary>
        /// <param name="clue">entry clue</param>
        /// <param name="answer">entry answer</param>
        /// <param name="difficulty">entry difficulty</param>
        /// <param name="date">entry date</param>
        /// <param name="id">entry id</param>
        /// <returns>an error if there is one, EntryError.NoError otherwise</returns>
        public EntryError EditEntry(string clue, string answer, int difficulty, string date, int id) {
            //Checking entry data.
            var fieldCheck = CheckEntryFields(clue, answer, difficulty, date);
            if (fieldCheck != EntryError.NoError) {
                return fieldCheck;
            }
            //Checking if the entry exists.
            var entry = relationalDb.FindEntry(id);
            //If so, make a call to the database
            if (entry != null) {
                entry.Clue = clue;
                entry.Answer = answer;
                entry.Difficulty = difficulty;
                entry.Date = date;

                //Making the database call.
                bool success = relationalDb.EditEntry(entry);
                if (!success) {
                    return EntryError.DBEditError;
                }
            } else {
                return EntryError.EntryNotFound;
            }
            return EntryError.NoError;
        }

        /// <summary>
        /// EntryListSort makes a call to the database to
        /// sort all our different entries. Really, it is
        /// gathering the local list and sorting that one
        /// instead of making a call to the database.
        /// </summary>
        /// <param name="sortType">sorting type</param>
        /// <returns>new sorted ObservableCollection</returns>
        public ObservableCollection<Entry> EntryListSort(SortType sortType) {
            //Checking what sorting type
            return relationalDb.EntryListSort(sortType);
        }
    }
}
