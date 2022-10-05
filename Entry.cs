using CommunityToolkit.Mvvm.ComponentModel;

namespace Lab2Solution {
    /// <summary>
    /// Entry class that contains information about crossword entries
    /// </summary>
    public class Entry : ObservableObject {
        string clue;
        string answer;
        int difficulty;
        string date;
        int id;

        public string Clue {
            get { return clue; }
            set { SetProperty(ref clue, value); }
        }

        public string Answer {
            get { return answer; }
            set { SetProperty(ref answer, value); }
        }

        public int Difficulty {
            get { return difficulty; }
            set { SetProperty(ref difficulty, value); }
        }

        public string Date {
            get { return date; }
            set { SetProperty(ref date, value); }
        }

        public int Id {
            get { return id; }
            set { SetProperty(ref id, value); }
        }

        /// <summary>
        /// Entry constructor
        /// </summary>
        /// <param name="clue">entry clue</param>
        /// <param name="answer">entry answer</param>
        /// <param name="difficulty">entry difficulty</param>
        /// <param name="date">entry date</param>
        /// <param name="id">entry id</param>
        public Entry(string clue, string answer, int difficulty, string date, int id) {
            this.clue = clue;
            this.answer = answer;
            this.difficulty = difficulty;
            this.date = date;
            this.id = id;
        }
    }
}
