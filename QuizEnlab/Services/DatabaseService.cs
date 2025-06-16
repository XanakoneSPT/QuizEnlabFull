using SQLite;
using QuizEnlab.Models;

namespace QuizEnlab.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;

        public DatabaseService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "quiz.db");
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Questions>().Wait();
        }

        public async Task<List<Questions>> GetQuestionsAsync()
        {
            return await _database.Table<Questions>().ToListAsync();
        }

        public async Task<int> AddQuestionAsync(Questions question)
        {
            return await _database.InsertAsync(question);
        }

        public async Task InitializeDefaultQuestionsAsync()
        {
            var count = await _database.Table<Questions>().CountAsync();
            if (count == 0)
            {
                var defaultQuestions = new List<Questions>
                {
                    new Questions
                    {
                        Text = "In programming, what is the most common way to iterate through an array?",
                        Option1 = "For loop",
                        Option2 = "While loop",
                        Option3 = "Recursion",
                        Option4 = "Switch statement",
                        CorrectOption = 1
                    },
                    new Questions
                    {
                        Text = "Which programming language is primarily used for iOS development?",
                        Option1 = "Java",
                        Option2 = "Python",
                        Option3 = "Swift",
                        Option4 = "C#",
                        CorrectOption = 3
                    },
                    new Questions
                    {
                        Text = "What does HTML stand for?",
                        Option1 = "Hyper Text Markup Language",
                        Option2 = "High Tech Modern Language",
                        Option3 = "Hyper Transfer Markup Language",
                        Option4 = "Home Tool Markup Language",
                        CorrectOption = 1
                    },
                    new Questions
                    {
                        Text = "Which of these is NOT a JavaScript framework?",
                        Option1 = "React",
                        Option2 = "Angular",
                        Option3 = "Django",
                        Option4 = "Vue",
                        CorrectOption = 3
                    },
                    new Questions
                    {
                        Text = "Which data structure uses LIFO (Last In, First Out)?",
                        Option1 = "Queue",
                        Option2 = "Stack",
                        Option3 = "Array",
                        Option4 = "Tree",
                        CorrectOption = 2
                    },
                    new Questions
                    {
                        Text = "What is the primary purpose of CSS in web development?",
                        Option1 = "Database Management",
                        Option2 = "Server Configuration",
                        Option3 = "Styling and Layout",
                        Option4 = "User Authentication",
                        CorrectOption = 3
                    },
                    new Questions
                    {
                        Text = "Which of these is a valid HTTP status code for 'Not Found'?",
                        Option1 = "200",
                        Option2 = "404",
                        Option3 = "500",
                        Option4 = "302",
                        CorrectOption = 2
                    },
                    new Questions
                    {
                        Text = "What type of AI model is ChatGPT?",
                        Option1 = "Convolutional Neural Network",
                        Option2 = "Large Language Model",
                        Option3 = "Expert System",
                        Option4 = "Decision Tree",
                        CorrectOption = 2
                    },
                    new Questions
                    {
                        Text = "Which encryption method is commonly used for HTTPS?",
                        Option1 = "ROT13",
                        Option2 = "Base64",
                        Option3 = "TLS/SSL",
                        Option4 = "MD5",
                        CorrectOption = 3
                    },
                    new Questions
                    {
                        Text = "Which cloud service model provides virtual machines and infrastructure?",
                        Option1 = "SaaS (Software as a Service)",
                        Option2 = "IaaS (Infrastructure as a Service)",
                        Option3 = "PaaS (Platform as a Service)",
                        Option4 = "FaaS (Function as a Service)",
                        CorrectOption = 2
                    }
                };

                foreach (var question in defaultQuestions)
                {
                    await AddQuestionAsync(question);
                }
            }
        }
    }
} 