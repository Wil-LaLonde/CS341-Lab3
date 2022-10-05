namespace Lab2Solution {
    public enum EntryError {
        InvalidClueLength,
        InvalidAnswerLength,
        InvalidDifficulty,
        InvalidDate,
        EntryNotFound,
        EntryNotSelected,
        DBAddError,
        DBDeleteError,
        DBEditError,
        NoError
    }

    public enum SortType {
        ClueSort,
        AnswerSort
    }
}
