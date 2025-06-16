using SQLite;
using System.ComponentModel.DataAnnotations;

namespace QuizEnlab.Models
{
    public class Questions
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public string Option1 { get; set; }

        [Required]
        public string Option2 { get; set; }

        [Required]
        public string Option3 { get; set; }

        [Required]
        public string Option4 { get; set; }

        [Required]
        public int CorrectOption { get; set; } // 1-4 representing the correct option
    }
}
