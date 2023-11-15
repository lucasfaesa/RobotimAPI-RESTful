using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;

namespace API_Mestrado_Lucas
{
    public class CompleteLevelSessionDTO : DataTransferObject
    {
        private int? levelId;
        private int? quizId;
        private bool? finished;
        private int totalTries;
        private int totalRepetitions;
        private int totalFails;
        private int maxScore;
        private DateTime? finishedDate;
        private float totalElapsedTime;
        private int totalPlays;
        private DateTime? lastPlayedDate;
        private LevelDTO level;
        private int totalScore;
        private QuizDTO quiz;

        public int? LevelId { get => levelId; set => levelId = value; }
        public int? QuizId { get => quizId; set => quizId = value; }
        public bool? Finished { get => finished; set => finished = value; }
        public int TotalTries { get => totalTries; set => totalTries = value; }
        public int TotalScore { get => totalScore; set => totalScore = value; }
        public int TotalRepetitions { get => totalRepetitions; set => totalRepetitions = value; }
        public int TotalFails { get => totalFails; set => totalFails = value; }
        public int MaxScore { get => maxScore; set => maxScore = value; }
        public DateTime? FinishedDate { get => finishedDate; set => finishedDate = value; }
        public float TotalElapsedTime { get => totalElapsedTime; set => totalElapsedTime = value; }
        public int TotalPlays { get => totalPlays; set => totalPlays = value; }
        public DateTime? LastPlayedDate { get => lastPlayedDate; set => lastPlayedDate = value; }

        
        public virtual LevelDTO Level { get => level; set => level = value; }

        public virtual QuizDTO Quiz { get => quiz; set => quiz = value; }
    }
}
