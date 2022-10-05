using System.Collections.ObjectModel;

namespace Lab2Solution {
    /// <summary>
    /// IDatabase interface
    /// </summary>
    public interface IDatabase {
        bool AddEntry(Entry entry);
        bool DeleteEntry(Entry entry);
        Entry FindEntry(int id);
        ObservableCollection<Entry> GetEntries();
        bool EditEntry(Entry replacementEntry);
        ObservableCollection<Entry> EntryListSort(SortType sortType);
    }
}