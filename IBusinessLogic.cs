using System.Collections.ObjectModel;

namespace Lab2Solution {
    /// <summary>
    /// The interface for BusinessLogic
    /// </summary>
    public interface IBusinessLogic {
        EntryError AddEntry(string clue, string answer, int difficulty, string date);
        EntryError DeleteEntry(int entryId);
        EntryError EditEntry(string clue, string answer, int difficulty, string date, int id);
        Entry FindEntry(int id);
        ObservableCollection<Entry> GetEntries();
        ObservableCollection<Entry> EntryListSort(SortType sortType);
    }
}