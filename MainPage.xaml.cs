
namespace Lab2Solution {

    public partial class MainPage : ContentPage {
        public MainPage() {
            InitializeComponent();
            EntriesLV.ItemsSource = MauiProgram.ibl.GetEntries();
        }

        void AddEntry(Object sender, EventArgs e) {
            String clue = clueENT.Text;
            String answer = answerENT.Text;
            String date = dateENT.Text;

            bool validDifficulty = int.TryParse(difficultyENT.Text, out int difficulty);
            if (validDifficulty) {
                EntryError invalidFieldError = MauiProgram.ibl.AddEntry(clue, answer, difficulty, date);
                if(invalidFieldError != EntryError.NoError) {
                    DisplayAlert("An error has occurred while adding an entry", $"{invalidFieldError}", "OK");
                }
            } else {
                DisplayAlert("Difficulty", $"Please enter a valid number", "OK");
            }
        }

        void DeleteEntry(Object sender, EventArgs e) {
            Entry selectedEntry = EntriesLV.SelectedItem as Entry;
            if (selectedEntry != null) {
                EntryError entryDeletionError = MauiProgram.ibl.DeleteEntry(selectedEntry.Id);
                if (entryDeletionError != EntryError.NoError) {
                    DisplayAlert("An error has occurred while deleting an entry", $"{entryDeletionError}", "OK");
                }
                //Resetting the selected list item.
                EntriesLV.SelectedItem = null;
            } else {
                DisplayAlert("Please select an entry", EntryError.EntryNotSelected.ToString(), "OK");
            }
        }

        void EditEntry(Object sender, EventArgs e) {
            Entry selectedEntry = EntriesLV.SelectedItem as Entry;
            if (selectedEntry != null) {
                selectedEntry.Clue = clueENT.Text;
                selectedEntry.Answer = answerENT.Text;
                selectedEntry.Date = dateENT.Text;

                bool validDifficulty = int.TryParse(difficultyENT.Text, out int difficulty);
                if (validDifficulty) {
                    selectedEntry.Difficulty = difficulty;
                    Console.WriteLine($"Difficuilt is {selectedEntry.Difficulty}");
                    EntryError entryEditError = MauiProgram.ibl.EditEntry(selectedEntry.Clue, selectedEntry.Answer, selectedEntry.Difficulty, selectedEntry.Date, selectedEntry.Id);
                    if (entryEditError != EntryError.NoError) {
                        DisplayAlert("An error has occurred while editing an entry", $"{entryEditError}", "OK");
                    }
                    //Resetting the selected list item.
                    EntriesLV.SelectedItem = null;
                }
            } else {
                DisplayAlert("Please select an entry", EntryError.EntryNotSelected.ToString(), "OK");
            }
        }

        void ClueSort(Object sender, EventArgs e) {
            //Updating the list view to the most up to date clue sorted list.
            EntriesLV.ItemsSource = MauiProgram.ibl.EntryListSort(SortType.ClueSort);
        }

        void AnswerSort(Object sender, EventArgs e) {
            //Updating the list view to the most up to date answer sorted list.
            EntriesLV.ItemsSource = MauiProgram.ibl.EntryListSort(SortType.AnswerSort);
        }

        void EntriesLV_ItemSelected(Object sender, SelectedItemChangedEventArgs e) {
            Entry selectedEntry = e.SelectedItem as Entry;
            if (selectedEntry != null) {
                clueENT.Text = selectedEntry.Clue;
                answerENT.Text = selectedEntry.Answer;
                difficultyENT.Text = selectedEntry.Difficulty.ToString();
                dateENT.Text = selectedEntry.Date;
            } 
        }
    }
}
