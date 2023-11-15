using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas.Models
{
    public class Session
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int? LevelId { get; set; }
        public int? QuizId { get; set; }
        public bool? Finished { get; set; } // true para sim e false para não e null para null, vai subir quando abrir a fase
        public int Score { get; set; } //vai subir quando abrir a fase, com pontuação 0
        public DateTime? FinishedDate { get; set; }
        public float ElapsedTime { get; set; } //vai subir quando abrir a fase como 0
        public DateTime PlayedDate { get; set; } //vai subir quando abrir a fase

        public virtual Student Student { get; set; }
        public virtual Level Level { get; set; }
        public virtual Quiz Quiz { get; set; }
    }
}
