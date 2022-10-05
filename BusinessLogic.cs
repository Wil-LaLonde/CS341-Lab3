using System.Collections.ObjectModel;

namespace Lab2Solution {

    /// <summary>
    /// Handles the BusinessLogic
    /// </summary>
    public class BusinessLogic : IBusinessLogic {
        const int MIN_CLUE_ANSWER_LENGTH = 1;
        const int MAX_CLUE_LENGTH = 250;
        const int MAX_ANSWER_LENGTH = 25;
        const int MIN_DIFFICULTY = 0;
        const int MAX_DIFFICULTY = 2;
        const String DATE_FORMAT = "mm/dd/yyyy";
        int latestId = 0;

        //IDatabase db; // Local Database
        IDatabase relationalDb; //bit.io Database

        public BusinessLogic() {
            //db = new FlatFileDatabase();
            relationalDb = new RelationalDatabase();
        }

        /// <summary>
        /// Represents all entries
        /// This also could have been a property
        /// </summary>
        /// <returns>ObservableCollection of entrties</returns>
        public ObservableCollection<Entry> GetEntries() {
            return relationalDb.GetEntries();
        }

        public Entry FindEntry(int id) {
            return relationalDb.FindEntry(id);
        }

        /// <summary>
        /// Verifies that all the entry fields are valied
        /// </summary>
        /// <param name="clue"></param>
        /// <param name="answer"></param>
        /// <param name="difficulty"></param>
        /// <param name="date"></param>
        /// <returns>an error if there is an error, InvalidFieldError.None otherwise</returns>
        private EntryError CheckEntryFields(String clue, String answer, int difficulty, String date) {
            if (clue.Length < MIN_CLUE_ANSWER_LENGTH || clue.Length > MAX_CLUE_LENGTH) {
                return EntryError.InvalidClueLength;
            }
            if (answer.Length < MIN_CLUE_ANSWER_LENGTH || answer.Length > MAX_ANSWER_LENGTH) {
                return EntryError.InvalidAnswerLength;
            }
            bool invalidDate = !DateTime.TryParseExact(date, DATE_FORMAT,
                                                    System.Globalization.CultureInfo.InvariantCulture,
                                                    System.Globalization.DateTimeStyles.None,
                                                    out _);
            if (invalidDate) {
                return EntryError.InvalidDate;
            }
            if (difficulty < MIN_DIFFICULTY || difficulty > MAX_DIFFICULTY) {
                return EntryError.InvalidDifficulty;
            }
            return EntryError.NoError;
        }


        /// <summary>
        /// Adds an entry
        /// </summary>
        /// <param name="clue"></param>
        /// <param name="answer"></param>
        /// <param name="difficulty"></param>
        /// <param name="date"></param>
        /// <returns>an error if there is an error, InvalidFieldError.None otherwise</returns>
        public EntryError AddEntry(string clue, string answer, int difficulty, string date) {
            var result = CheckEntryFields(clue, answer, difficulty, date);
            if (result != EntryError.NoError) {
                return result;
            }
            Entry entry = new Entry(clue, answer, difficulty, date, ++latestId);
            bool success = relationalDb.AddEntry(entry);
            if (!success) {
                return EntryError.DBAddError;
            }
            return EntryError.NoError;
        }

        /// <summary>
        /// Deletes an entry
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns>an erreor if there is one, EntryDeletionError.NoError otherwise</returns>
        public EntryError DeleteEntry(int entryId) {

            var entry = relationalDb.FindEntry(entryId);

            if (entry != null) {
                bool success = relationalDb.DeleteEntry(entry);
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
        /// <param name="clue"></param>
        /// <param name="answer"></param>
        /// <param name="difficulty"></param>
        /// <param name="date"></param>
        /// <param name="id"></param>
        /// <returns>an error if there is one, EntryEditError.None otherwise</returns>
        public EntryError EditEntry(string clue, string answer, int difficulty, string date, int id) {

            var fieldCheck = CheckEntryFields(clue, answer, difficulty, date);
            if (fieldCheck != EntryError.NoError) {
                return fieldCheck;
            }

            var entry = relationalDb.FindEntry(id);
            if (entry != null) {
                entry.Clue = clue;
                entry.Answer = answer;
                entry.Difficulty = difficulty;
                entry.Date = date;

                bool success = relationalDb.EditEntry(entry);
                if (!success) {
                    return EntryError.DBEditError;
                }
            } else {
                return EntryError.EntryNotFound;
            }

            return EntryError.NoError;
        }

        public ObservableCollection<Entry> EntryListSort(SortType sortType) {
            //Checking what sorting type
            return relationalDb.EntryListSort(sortType);
        }
    }
}
