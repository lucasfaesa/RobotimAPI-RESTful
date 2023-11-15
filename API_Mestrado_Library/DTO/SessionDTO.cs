using System;

namespace API_Mestrado_Lucas
{
    public class SessionDTO : DataTransferObject
    {
        private int id;
        private int studentId;
        private int? levelId;
        private int? quizId;
        private bool? finished;
        private int score;
        private DateTime? finishedDate;
        private float elapsedTime;
        private DateTime playedDate;
        private StudentDTO student;
        private LevelDTO level;
        private QuizDTO quiz;

        public int Id { get => id; set => id = value; }
        public int StudentId { get => studentId; set => studentId = value; }
        public int? LevelId { get => levelId; set => levelId = value; }
        public int? QuizId { get => quizId; set => quizId = value; }
        public bool? Finished { get => finished; set => finished = value; } // true para sim e false para não e null para null, vai subir quando abrir a fase
        public int Score { get => score; set => score = value; } //vai subir quando abrir a fase, com pontuação 0
        public DateTime? FinishedDate { get => finishedDate; set => finishedDate = value; }
        public float ElapsedTime { get => elapsedTime; set => elapsedTime = value; } //vai subir quando abrir a fase como 0
        public DateTime PlayedDate { get => playedDate; set => playedDate = value; } //vai subir quando abrir a fase

        public virtual StudentDTO Student { get => student; set => student = value; }
        public virtual LevelDTO Level { get => level; set => level = value; }
        public virtual QuizDTO Quiz { get => quiz; set => quiz = value; }
    }
}
