using API_Mestrado_Lucas.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Mestrado_Lucas.Context
{
    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupClassSubjectTheme>()
            .HasKey(gl => new { gl.GroupClassId , gl.SubjectThemeId});

            modelBuilder.Entity<GroupClassSubjectTheme>()
                        .HasOne(gl => gl.GroupClass)
                        .WithMany(b => b.GroupClassSubjectThemes)
                        .HasForeignKey(bc => bc.GroupClassId);

            modelBuilder.Entity<GroupClassSubjectTheme>()
                        .HasOne(gl => gl.SubjectTheme)
                        .WithMany(c => c.GroupClassSubjectThemes)
                        .HasForeignKey(bc => bc.SubjectThemeId);



            #region Seed do banco de dados

            #region Seed Students
            /*
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = 1,
                    TeacherId = 1,
                    Username = "Luke12",
                    Password = "abc123",
                    Name = "Lucas",
                    GroupClassId = 1,
                    CreationDate = new DateTime(2021, 12, 11),
                    LastLoginDate = new DateTime(2021, 12, 11)
                }
            );

            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = 2,
                    TeacherId = 1,
                    Username = "Joao44",
                    Password = "algo123",
                    Name = "João",
                    GroupClassId = 1,
                    CreationDate = new DateTime(2021, 11, 30),
                    LastLoginDate = new DateTime(2021, 12, 9)
                }
            );

            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = 3,
                    TeacherId = 1,
                    Username = "Mar5",
                    Password = "something123",
                    Name = "Maria",
                    GroupClassId = 1,
                    CreationDate = new DateTime(2021, 12, 1),
                    LastLoginDate = new DateTime(2021, 12, 3)
                }
            );
            */
            #endregion

            #region Seed GroupClasses
            /*
            modelBuilder.Entity<GroupClass>().HasData(
                new GroupClass
                {
                    Id = 1,
                    Name = "Turma 6B - Vespertino",
                    TeacherId = 1
                }
            );
            */
            #endregion

            #region Seed GroupClassesLevels
            /*
            modelBuilder.Entity<GroupClassSubjectTheme>().HasData(
               new GroupClassSubjectTheme
               {
                  GroupClassId = 1,
                  SubjectThemeId = 1
               }
            );

            modelBuilder.Entity<GroupClassSubjectTheme>().HasData(
               new GroupClassSubjectTheme
               {
                   GroupClassId = 1,
                   SubjectThemeId = 2
               }
            );

            modelBuilder.Entity<GroupClassSubjectTheme>().HasData(
               new GroupClassSubjectTheme
               {
                   GroupClassId = 1,
                   SubjectThemeId = 3
               }
            );
            */
            #endregion

            #region Seed Levels
            modelBuilder.Entity<Level>().HasData(
                new Level
                {
                    Id = 1,
                    SubjectThemeId = 1,
                    Difficulty = 1,
                    MidScoreThreshold = 65,
                    HighScoreThreshold = 80
                }
            );

            modelBuilder.Entity<Level>().HasData(
                new Level
                {
                    Id = 2,
                    SubjectThemeId = 1,
                    Difficulty = 2,
                    MidScoreThreshold = 65,
                    HighScoreThreshold = 80
                }
            );

            modelBuilder.Entity<Level>().HasData(
                new Level
                {
                    Id = 3,
                    SubjectThemeId = 1,
                    Difficulty = 5,
                    MidScoreThreshold = 45,
                    HighScoreThreshold = 82
                }
            );

            modelBuilder.Entity<Level>().HasData(
               new Level
               {
                   Id = 4,
                   SubjectThemeId = 1,
                   Difficulty = 10,
                   MidScoreThreshold = 56,
                   HighScoreThreshold = 78
               }
           );

            modelBuilder.Entity<Level>().HasData(
               new Level
               {
                   Id = 5,
                   SubjectThemeId = 2,
                   Difficulty = 1,
                   MidScoreThreshold = 150,
                   HighScoreThreshold = 180
               }
           );



            modelBuilder.Entity<Level>().HasData(
             new Level
             {
                 Id = 6,
                 SubjectThemeId = 4,
                 Difficulty = 1,
                 MidScoreThreshold = 71,
                 HighScoreThreshold = 100
             }
         );
            modelBuilder.Entity<Level>().HasData(
             new Level
             {
                 Id = 7,
                 SubjectThemeId = 4,
                 Difficulty = 2,
                 MidScoreThreshold = 200,
                 HighScoreThreshold = 250
             }
         );
            modelBuilder.Entity<Level>().HasData(
             new Level
             {
                 Id = 8,
                 SubjectThemeId = 4,
                 Difficulty = 3,
                 MidScoreThreshold = 220,
                 HighScoreThreshold = 280
             }
         );

        modelBuilder.Entity<Level>().HasData(
        new Level
        {
            Id = 11,
            SubjectThemeId = 6,
            Difficulty = 1,
            MidScoreThreshold = 70,
            HighScoreThreshold = 100
        }
        );
            modelBuilder.Entity<Level>().HasData(
        new Level
        {
            Id = 12,
            SubjectThemeId = 6,
            Difficulty = 2,
            MidScoreThreshold = 70,
            HighScoreThreshold = 100
        }
    );
            modelBuilder.Entity<Level>().HasData(
        new Level
        {
            Id = 13,
            SubjectThemeId = 6,
            Difficulty = 3,
            MidScoreThreshold = 70,
            HighScoreThreshold = 100
        }
    );
            modelBuilder.Entity<Level>().HasData(
        new Level
        {
            Id = 14,
            SubjectThemeId = 6,
            Difficulty = 4,
            MidScoreThreshold = 70,
            HighScoreThreshold = 100
        }
    );

            modelBuilder.Entity<Level>().HasData(
           new Level
           {
               Id = 15,
               SubjectThemeId = 2,
               Difficulty = 2,
               MidScoreThreshold = 150,
               HighScoreThreshold = 180
           }
        );
        modelBuilder.Entity<Level>().HasData(
            new Level
            {
                Id = 16,
                SubjectThemeId = 7,
                Difficulty = 1,
                MidScoreThreshold = 90,
                HighScoreThreshold = 100
            }
        );
        modelBuilder.Entity<Level>().HasData(
            new Level
            {
                Id = 17,
                SubjectThemeId = 7,
                Difficulty = 2,
                MidScoreThreshold = 90,
                HighScoreThreshold = 100
            }
        );
        modelBuilder.Entity<Level>().HasData(
            new Level
            {
                Id = 18,
                SubjectThemeId = 7,
                Difficulty = 3,
                MidScoreThreshold = 90,
                HighScoreThreshold = 100
            }
        );
        modelBuilder.Entity<Level>().HasData(
            new Level
            {
                Id = 19,
                SubjectThemeId = 7,
                Difficulty = 4,
                MidScoreThreshold = 90,
                HighScoreThreshold = 100
            }
        );

            //SubjectThemeId : 1=Meteoro, 2=Village, 4=WorldTravel, 6= School Room, 7=Environment Runner
            #endregion

            #region Seed Subjects

            modelBuilder.Entity<Subject>().HasData(
              new Subject
              {
                  Id = 1,
                  Name = "Matemática",
              }
            );
            modelBuilder.Entity<Subject>().HasData(
              new Subject
              {
                  Id = 2,
                  Name = "Geografia",
              }
            );
            modelBuilder.Entity<Subject>().HasData(
              new Subject
              {
                  Id = 3,
                  Name = "Ciências",
              }
            );
            modelBuilder.Entity<Subject>().HasData(
                new Subject
            {
                Id = 4,
                Name = "Meio Ambiente",
            }
            );
            #endregion

            #region Seed SubjectsThemes

            modelBuilder.Entity<SubjectTheme>().HasData(
        new SubjectTheme
        {
            Id = 1,
            Name = "Destrua os Meteoros",
            Code = "mat001",
            Description = "Resolução de operações matemáticas como soma, subtração, divisão e multiplicação",
            SubjectId = 1
        }
        );

        modelBuilder.Entity<SubjectTheme>().HasData(
        new SubjectTheme
        {
            Id = 2,
            Name = "Robôs precisam de ajuda",
            Code = "mat002",
            Description = "Resolução de problemas baseados em Ponto, Reta e Plano",
            SubjectId = 1
        }
        );

        modelBuilder.Entity<SubjectTheme>().HasData(
            new SubjectTheme
            {
                Id = 4,
                Name = "Demanda de Suprimentos",
                Code = "geo001",
                Description = "Aluno deve entregar suprimentos para os países corretos, perguntas são baseadas nas organizações internacionais mundiais",
                SubjectId = 2
            }
        );

        modelBuilder.Entity<SubjectTheme>().HasData(
            new SubjectTheme
            {
                Id = 6,
                Name = "Pesquisa com Rob",
                Code = "cie001",
                Description = "Aluno deve responder corretamente perguntas sobre doenças e profilaxia",
                SubjectId = 3
            }
        );
        modelBuilder.Entity<SubjectTheme>().HasData(
            new SubjectTheme
            {
                Id = 7,
                Name = "Restauração Ambiental",
                Code = "amb001",
                Description = "Aluno deve coletar os itens positivos e desviar da poluição para restaurar o meio ambiente.",
                SubjectId = 4
            }
        );

            #endregion

            #region Seed Quizes
            /*modelBuilder.Entity<Quiz>().HasData(
            new Quiz
            {
                Id = 1,
                Name = "Quiz Exemplo",
                TeacherId = null,
            }
            );*/

            #endregion

            #region Seed Questions
            #region Quiz Example
            /*modelBuilder.Entity<Question>().HasData(
            new Question
            {
                Id = 1,
                QuizId = 1,
                QuestionTitle = "Retas paralelas são:",
                QuestionTimeLimit = 60f,
                QuestionScoreValue = 10
            }
            ) ;

            modelBuilder.Entity<Question>().HasData(
            new Question
            {
                Id = 2,
                QuizId = 1,
                QuestionTitle = "Retas concorrentes são:",
                QuestionTimeLimit = 60f,
                QuestionScoreValue = 10
            }
            );

            modelBuilder.Entity<Question>().HasData(
            new Question
            {
                Id = 3,
                QuizId = 1,
                QuestionTitle = "Pode ser considerado um vértice:",
                QuestionTimeLimit = 60f,
                QuestionScoreValue = 10
            }
            );

            modelBuilder.Entity<Question>().HasData(
            new Question
            {
                Id = 4,
                QuizId = 1,
                QuestionTitle = "Pode ser considerado uma aresta:",
                QuestionTimeLimit = 60f,
                QuestionScoreValue = 10
            }
            );

            modelBuilder.Entity<Question>().HasData(
            new Question
            {
                Id = 5,
                QuizId = 1,
                QuestionTitle = "Vivemos em um mundo cercado de formas geométricas que podem ser classificadas como figuras " +
                                "planas e figuras espaciais. Das alternativas a seguir, marque aquela que corresponde a uma " +
                                "figura espacial.",
                QuestionTimeLimit = 60f,
                QuestionScoreValue = 10
            }
            );

            modelBuilder.Entity<Question>().HasData(
            new Question
            {
                Id = 6,
                QuizId = 1,
                QuestionTitle = "Quais das alternativas abaixo são consideradas exemplos de objetos que possui a forma de uma reta?",
                QuestionTimeLimit = 60f,
                QuestionScoreValue = 10
            }
            );

            modelBuilder.Entity<Question>().HasData(
            new Question
            {
                Id = 7,
                QuizId = 1,
                QuestionTitle = "Em relação ao Ponto é correto afirmar que ele:",
                QuestionTimeLimit = 60f,
                QuestionScoreValue = 10
            }
            );

            modelBuilder.Entity<Question>().HasData(
            new Question
            {
               Id = 8,
                QuizId = 1,
                QuestionTitle = "Um tampo de uma mesa pode ser considerado:",
               QuestionTimeLimit = 60f,
               QuestionScoreValue = 10
            }
            );*/
            #endregion

            #endregion

            #region Seed QuestionAnswers
            #region Quiz Example
            /*modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 1,
                QuestionId = 1,
                AnswerString = "Duas retas que não se encontram",
                IsCorrectAnswer = true
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 2,
                QuestionId = 1,
                AnswerString = "Duas retas que se cruzam",
                IsCorrectAnswer = false
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 3,
                QuestionId = 1,
                AnswerString = "O encontro de dois vértices",
                IsCorrectAnswer = false
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 4,
                QuestionId = 1,
                AnswerString = "Todas as alternativas",
                IsCorrectAnswer = false
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 5,
                QuestionId = 2,
                AnswerString = "Duas retas que não se encontram",
                IsCorrectAnswer = false
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 6,
                QuestionId = 2,
                AnswerString = "Duas retas que se cruzam",
                IsCorrectAnswer = true
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 7,
                QuestionId = 2,
                AnswerString = "O encontro de arestas",
                IsCorrectAnswer = false
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 8,
                QuestionId = 2,
                AnswerString = "Todas as alternativas",
                IsCorrectAnswer = false
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
           new QuestionAnswer
           {
               Id = 9,
               QuestionId = 3,
               AnswerString = "O encontro de duas ou mais arestas",
               IsCorrectAnswer = true
           }
           );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 10,
                QuestionId = 3,
                AnswerString = "Três retas paralelas",
                IsCorrectAnswer = false
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 11,
                QuestionId = 3,
                AnswerString = "Um modelo 3D avançado",
                IsCorrectAnswer = false
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 12,
                QuestionId = 3,
                AnswerString = "Todas as alternativas",
                IsCorrectAnswer = false
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
          new QuestionAnswer
          {
              Id = 13,
              QuestionId = 4,
              AnswerString = "Segmento de reta que liga dois vértices",
              IsCorrectAnswer = true
          }
          );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 14,
                QuestionId = 4,
                AnswerString = "Ponto único no espaço",
                IsCorrectAnswer = false
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 15,
                QuestionId = 4,
                AnswerString = "O encontro de 3 ou mais vértices",
                IsCorrectAnswer = false
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 16,
                QuestionId = 4,
                AnswerString = "Todas as alternativas",
                IsCorrectAnswer = false
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 17,
                QuestionId = 5,
                AnswerString = "Retângulo",
                IsCorrectAnswer = false
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 18,
                QuestionId = 5,
                AnswerString = "Círculo",
                IsCorrectAnswer = false
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 19,
                QuestionId = 5,
                AnswerString = "Paralelogramo",
                IsCorrectAnswer = false
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 20,
                QuestionId = 5,
                AnswerString = "Cubo",
                IsCorrectAnswer = true
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 21,
                QuestionId = 6,
                AnswerString = "Mesa e cadeira.",
                IsCorrectAnswer = false
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 22,
                QuestionId = 6,
                AnswerString = "Régua e trena",
                IsCorrectAnswer = true
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 23,
                QuestionId = 6,
                AnswerString = "Livro e lápis",
                IsCorrectAnswer = false
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 24,
                QuestionId = 6,
                AnswerString = "Celular e caneta",
                IsCorrectAnswer = false
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 25,
                QuestionId = 7,
                AnswerString = "Não possui dimensões.",
                IsCorrectAnswer = true
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 26,
                QuestionId = 7,
                AnswerString = "É imaginado sem espessura.",
                IsCorrectAnswer = false
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 27,
                QuestionId = 7,
                AnswerString = "Não tem começo nem fim.",
                IsCorrectAnswer = false
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 28,
                QuestionId = 7,
                AnswerString = "É impossível desenhá-lo no papel.",
                IsCorrectAnswer = false
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 29,
                QuestionId = 8,
                AnswerString = "Um plano.",
                IsCorrectAnswer = true
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 30,
                QuestionId = 8,
                AnswerString = "Um ponto.",
                IsCorrectAnswer = false
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 31,
                QuestionId = 8,
                AnswerString = "Um circulo.",
                IsCorrectAnswer = false
            }
            );

            modelBuilder.Entity<QuestionAnswer>().HasData(
            new QuestionAnswer
            {
                Id = 32,
                QuestionId = 8,
                AnswerString = "Uma reta.",
                IsCorrectAnswer = false
            }
            );*/
            #endregion

            #endregion

            #region SeedTeachers
            modelBuilder.Entity<Teacher>().HasData(
             new Teacher
             {
                 Id = 1,
                 Name = "Lucas Teacher",
                 Username = "Lucas1234",
                 Password = "Luk123",
                 CreationDate = new DateTime(2022, 07, 05, 12, 40, 35),
                 LastLoginDate = DateTime.Now
             }
            );
            #endregion

            #region Seed Sessions
            /*
            modelBuilder.Entity<Session>().HasData(
             new Session
             {
                 Id = 1,
                 StudentId = 1,
                 LevelId = 1,
                 Finished = true,
                 Score = 100,
                 FinishedDate = new DateTime(2021,12,11),
                 ElapsedTime = 120,
                 PlayedDate = new DateTime(2021,12,10),
             }
            );
            
            modelBuilder.Entity<Session>().HasData(
             new Session
             {
                 Id = 2,
                 StudentId = 1,
                 LevelId = 1,
                 Finished = true,
                 Score = 135,
                 FinishedDate = new DateTime(2021, 12, 9),
                 ElapsedTime = 60,
                 PlayedDate = new DateTime(2021, 12, 9),
             }
            );

            modelBuilder.Entity<Session>().HasData(
             new Session
             {
                 Id = 3,
                 StudentId = 1,
                 LevelId = 2,
                 Finished = false,
                 Score = 0,
                 FinishedDate = null,
                 ElapsedTime = 30,
                 PlayedDate = new DateTime(2021, 12, 11),
             }
            );

            modelBuilder.Entity<Session>().HasData(
             new Session
             {
                 Id = 4,
                 StudentId = 1,
                 LevelId = 3,
                 Finished = true,
                 Score = 140,
                 FinishedDate = new DateTime(2021, 12, 12),
                 ElapsedTime = 140,
                 PlayedDate = new DateTime(2021, 12, 12),
             }
            );

            modelBuilder.Entity<Session>().HasData(
             new Session
             {
                 Id = 5,
                 StudentId = 2,
                 LevelId = 1,
                 Finished = false,
                 Score = 0,
                 FinishedDate = null,
                 ElapsedTime = 20,
                 PlayedDate = new DateTime(2021, 12, 11),
             }
            );

            modelBuilder.Entity<Session>().HasData(
             new Session
             {
                 Id = 6,
                 StudentId = 2,
                 LevelId = 2,
                 Finished = false,
                 Score = 0,
                 FinishedDate = null,
                 ElapsedTime = 70,
                 PlayedDate = new DateTime(2021, 12, 11),
             }
            );

            modelBuilder.Entity<Session>().HasData(
             new Session
             {
                 Id = 7,
                 StudentId = 3,
                 LevelId = 1,
                 Finished = true,
                 Score = 145,
                 FinishedDate = new DateTime(2021, 12, 9),
                 ElapsedTime = 140,
                 PlayedDate = new DateTime(2021, 12, 9),
             }
            );

            modelBuilder.Entity<Session>().HasData(
             new Session
             {
                 Id = 8,
                 StudentId = 3,
                 LevelId = 3,
                 Finished = false,
                 Score = 0,
                 FinishedDate = null,
                 ElapsedTime = 20,
                 PlayedDate = new DateTime(2021, 12, 9),
             }
            );

            modelBuilder.Entity<Session>().HasData(
             new Session
             {
                 Id = 9,
                 StudentId = 3,
                 LevelId = 3,
                 Finished = false,
                 Score = 0,
                 FinishedDate = null,
                 ElapsedTime = 10,
                 PlayedDate = new DateTime(2021, 12, 10),
             }
            );

            modelBuilder.Entity<Session>().HasData(
             new Session
             {
                 Id = 10,
                 StudentId = 3,
                 LevelId = 4,
                 Finished = true,
                 Score = 146,
                 FinishedDate = new DateTime(2021, 12, 12),
                 ElapsedTime = 150,
                 PlayedDate = new DateTime(2021, 12, 12),
             }
            );

            modelBuilder.Entity<Session>().HasData(
            new Session
            {
                Id = 11,
                StudentId = 2,
                LevelId = 5,
                Finished = true,
                Score = 122,
                FinishedDate = new DateTime(2021, 09, 28),
                ElapsedTime = 40,
                PlayedDate = new DateTime(2021, 09, 28),
            }
           );

            modelBuilder.Entity<Session>().HasData(
            new Session
            {
                Id = 12,
                StudentId = 2,
                LevelId = 5,
                Finished = true,
                Score = 45,
                FinishedDate = new DateTime(2021, 09, 16),
                ElapsedTime = 60,
                PlayedDate = new DateTime(2021, 09, 28),
            }
            );

            modelBuilder.Entity<Session>().HasData(
            new Session
            {
                Id = 13,
                StudentId = 3,
                LevelId = 5,
                Finished = true,
                Score = 132,
                FinishedDate = new DateTime(2021, 09, 18),
                ElapsedTime = 80,
                PlayedDate = new DateTime(2021, 09, 18),
            }
            );
            */
            #endregion
            #endregion
        }


        public DbSet<Student> Students { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubjectTheme> SubjectThemes { get; set; }
        public DbSet<GroupClass> GroupClasses { get; set; }
        public DbSet<GroupClassSubjectTheme> GroupClassSubjectThemes { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswer { get; set; }
        public DbSet<StudentWrongAnswers> StudentWrongAnswers { get; set; }
        public DbSet<API_Mestrado_Lucas.Models.Quiz> Quiz { get; set; }
    }
}
