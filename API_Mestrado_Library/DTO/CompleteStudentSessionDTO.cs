using System;

namespace API_Mestrado_Lucas
{
    public class CompleteStudentSessionDTO : DataTransferObject
    {
        private int studentId;
        private int maxScore;
        private int totalScore;
        private int maxRepetitions;
        private int totalRepetitions;
        private int maxTries;
        private int totalTries;
        private int maxFails;
        private int totalFails;
        private float maxElapsedTime;
        private float totalElapsedTime;
        private DateTime? minFinishedDate;
        private DateTime? maxFinishedDate;
        private int levelsFinishedQuantity;
        private int levelsPendingQuantity;
        private int maxPlays;
        private int totalPlays;
        private DateTime lastPlayedDate;
        private DateTime lastLoginDate;
        private StudentDTO student;
        private SessionDTO levelMaxScore;
        private SessionDTO levelMaxRepetitions;
        private SessionDTO levelMaxTries;
        private SessionDTO levelMaxFails;
        private SessionDTO levelMaxPlays;
        private SessionDTO levelMaxElapsedTime;

        public int StudentId { get => studentId; set => studentId = value; }
        public int MaxScore { get => maxScore; set => maxScore = value; }
        public int TotalScore { get => totalScore; set => totalScore = value; }
        public SessionDTO LevelMaxScore { get => levelMaxScore; set => levelMaxScore = value; }
        public int MaxRepetitions { get => maxRepetitions; set => maxRepetitions = value; }
        public int TotalRepetitions { get => totalRepetitions; set => totalRepetitions = value; }
        public SessionDTO LevelMaxRepetitions { get => levelMaxRepetitions; set => levelMaxRepetitions = value; }
        public int MaxTries { get => maxTries; set => maxTries = value; }
        public int TotalTries { get => totalTries; set => totalTries = value; }
        public SessionDTO LevelMaxTries { get => levelMaxTries; set => levelMaxTries = value; }
        public int MaxFails { get => maxFails; set => maxFails = value; }
        public int TotalFails { get => totalFails; set => totalFails = value; }
        public SessionDTO LevelMaxFails { get => levelMaxFails; set => levelMaxFails = value; }
        public float MaxElapsedTime { get => maxElapsedTime; set => maxElapsedTime = value; }
        public float TotalElapsedTime { get => totalElapsedTime; set => totalElapsedTime = value; }
        public SessionDTO LevelMaxElapsedTime { get => levelMaxElapsedTime; set => levelMaxElapsedTime = value; }
        public DateTime? MinFinishedDate { get => minFinishedDate; set => minFinishedDate = value; }
        public DateTime? MaxFinishedDate { get => maxFinishedDate; set => maxFinishedDate = value; }
        public int LevelsFinishedQuantity { get => levelsFinishedQuantity; set => levelsFinishedQuantity = value; }
        public int LevelsPendingQuantity { get => levelsPendingQuantity; set => levelsPendingQuantity = value; }
        public int MaxPlays { get => maxPlays; set => maxPlays = value; }
        public int TotalPlays { get => totalPlays; set => totalPlays = value; }
        public SessionDTO LevelMaxPlays { get => levelMaxPlays; set => levelMaxPlays = value; }
        public DateTime LastPlayedDate { get => lastPlayedDate; set => lastPlayedDate = value; }
        public DateTime LastLoginDate { get => lastLoginDate; set => lastLoginDate = value; }

        public virtual StudentDTO Student { get => student; set => student = value; }
    }
}
