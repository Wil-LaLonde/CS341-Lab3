namespace Lab2Solution {
    //Different EntryErrors that could occur
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
    //Different sorting types
    public enum SortType {
        ClueSort,
        AnswerSort
    }
}
