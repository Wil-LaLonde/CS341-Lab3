namespace Lab2Solution {

    public partial class MainPage : ContentPage {
        /// <summary>
        /// MainPage constructor. Gathers all current entries from the database.
        /// </summary>
        public MainPage() {
            InitializeComponent();
            EntriesLV.ItemsSource = MauiProgram.ibl.GetEntries();
        }

        /// <summary>
        /// AddEntry gathers all user input. This is sent to the 
        /// BusinessLogic layer to get checked. If there was
        /// a problem, display the errror to the user.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        void AddEntry(Object sender, EventArgs e) {
            string clue = clueENT.Text;
            string answer = answerENT.Text;
            string date = dateENT.Text;

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

        /// <summary>
        /// DeleteEntry gets the selected input from the user.
        /// This is checked in the BusinessLogic if it somehow 
        /// doesn't exist.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
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

        /// <summary>
        /// EditEntry gathers all user input. This is sent to the 
        /// BusinessLogic layer to get checked. If there was
        /// a problem, display the errror to the user.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
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

        /// <summary>
        /// ClueSort will reset the current list shown to the user
        /// with a list sorted by Clue.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        void ClueSort(Object sender, EventArgs e) {
            //Updating the list view to the most up to date clue sorted list.
            EntriesLV.ItemsSource = MauiProgram.ibl.EntryListSort(SortType.ClueSort);
        }

        /// <summary>
        /// AnswerSort will reset the current list shown to the user
        /// with a list sorted by Answer.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
        void AnswerSort(Object sender, EventArgs e) {
            //Updating the list view to the most up to date answer sorted list.
            EntriesLV.ItemsSource = MauiProgram.ibl.EntryListSort(SortType.AnswerSort);
        }

        /// <summary>
        /// When a user selects an item, populate all fields with 
        /// the information of their selection.
        /// </summary>
        /// <param name="sender">Object sender</param>
        /// <param name="e">EventArgs e</param>
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
