using QuizEnlab.Models;
using QuizEnlab.Services;

namespace QuizEnlab.Pages;

public partial class QuizPage : ContentPage
{
    private List<Questions> _questions;
    private int _currentQuestionIndex;
    private List<Button> _optionButtons;
    private Button _selectedButton;
    private int _correctAnswers;
    private DateTime _quizStartTime;
    private readonly DatabaseService _databaseService;
    private List<QuestionReview> _questionReviews;

    private async void OnCloseClicked(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Quit Quiz", "Are you sure you want to quit? Your progress will be lost.", "Yes", "No");
        if (answer)
        {
            await Navigation.PopAsync();
        }
    }

    public async void Reset()
    {
        _currentQuestionIndex = 0;
        _correctAnswers = 0;
        _quizStartTime = DateTime.Now;
        _selectedButton = null;
        _questionReviews = new List<QuestionReview>();
        
        // Ensure questions are loaded before showing first question
        await _databaseService.InitializeDefaultQuestionsAsync();
        _questions = await _databaseService.GetQuestionsAsync();
        
        if (_questions != null && _questions.Count > 0)
        {
            ShowCurrentQuestion();
        }
    }

    public QuizPage()
    {
        InitializeComponent();
        _databaseService = new DatabaseService();
        
        // Initialize option buttons list
        _optionButtons = new List<Button> { Option1Button, Option2Button, Option3Button, Option4Button };

        // Initialize hardcoded questions
        // _questions = new List<Questions>
        // {
        //     new Questions
        //     {
        //         Id = 1,
        //         Text = "What is the capital of France?",
        //         Option1 = "Berlin",
        //         Option2 = "Madrid",
        //         Option3 = "Paris",
        //         Option4 = "Rome",
        //         CorrectOption = 3 // Paris is option 3
        //     },
        //     new Questions
        //     {
        //         Id = 2,
        //         Text = "Which planet is known as the Red Planet?",
        //         Option1 = "Earth",
        //         Option2 = "Mars",
        //         Option3 = "Jupiter",
        //         Option4 = "Venus",
        //         CorrectOption = 2 // Mars is option 2
        //     },
        //     new Questions
        //     {
        //         Id = 3,
        //         Text = "In any programming language, what is the most common way to iterate through an array?",
        //         Option1 = "For loop",
        //         Option2 = "While loop",
        //         Option3 = "Recursion",
        //         Option4 = "Switch statement",
        //         CorrectOption = 1 // For loop is option 1
        //     },
        // };

        _currentQuestionIndex = 0;
        _correctAnswers = 0;
        _quizStartTime = DateTime.Now;
        _questionReviews = new List<QuestionReview>();
        
        LoadQuestionsAndShowFirst();
    }

    private async void LoadQuestionsAndShowFirst()
    {
        await _databaseService.InitializeDefaultQuestionsAsync();
        _questions = await _databaseService.GetQuestionsAsync();
        ShowCurrentQuestion();
    }

    private void ShowCurrentQuestion()
    {
        // Safety checks
        if (_questions == null || _questions.Count == 0)
        {
            return;
        }

        if (_currentQuestionIndex >= _questions.Count)
        {
            return;
        }

        var question = _questions[_currentQuestionIndex];
        if (question == null)
        {
            return;
        }

        // Update question text and number
        QuestionText.Text = question.Text;
        QuestionNumberLabel.Text = $"Question {_currentQuestionIndex + 1}/{_questions.Count}";

        // Update option buttons
        Option1Button.Text = question.Option1;
        Option2Button.Text = question.Option2;
        Option3Button.Text = question.Option3;
        Option4Button.Text = question.Option4;

        // Reset button states
        foreach (var button in _optionButtons)
        {
            button.BackgroundColor = Colors.Transparent;
            button.TextColor = Colors.White;
            button.BorderColor = Colors.White;
            button.IsEnabled = true;  // Re-enable buttons for the new question
        }

        // Reset selection state and feedback
        _selectedButton = null;
        FeedbackLabel.IsVisible = false;
        FeedbackLabel.Text = "";
        NextButton.IsEnabled = false;
        NextButton.BackgroundColor = Color.FromArgb("#999999");
    }

    private void OnOptionSelected(object sender, EventArgs e)
    {
        var selectedButton = sender as Button;
        if (selectedButton == null) return;

        // Safety check for current question
        if (_currentQuestionIndex >= _questions.Count)
        {
            return;
        }

        // Disable all option buttons to prevent further selection
        foreach (var button in _optionButtons)
        {
            button.IsEnabled = false;
        }

        // Reset all buttons to default state
        foreach (var button in _optionButtons)
        {
            button.BackgroundColor = Colors.Transparent;
            button.TextColor = Colors.White;
            button.BorderColor = Colors.White;
        }

        // Get the selected option index and check if it's correct
        var selectedOptionIndex = _optionButtons.IndexOf(selectedButton) + 1;
        var currentQuestion = _questions[_currentQuestionIndex];
        bool isCorrect = selectedOptionIndex == currentQuestion.CorrectOption;

        // Update button appearance based on correctness
        if (isCorrect)
        {
            selectedButton.BorderColor = Colors.Green;
            FeedbackLabel.Text = "Correct! ✓";
            FeedbackLabel.TextColor = Colors.Green;
        }
        else
        {
            selectedButton.BorderColor = Colors.Red;
            FeedbackLabel.Text = "Incorrect ✗";
            FeedbackLabel.TextColor = Colors.Red;

            // Highlight the correct answer
            var correctButton = _optionButtons[currentQuestion.CorrectOption - 1];
            correctButton.BorderColor = Colors.Green;
        }

        FeedbackLabel.IsVisible = true;
        _selectedButton = selectedButton;

        // Enable Next button
        NextButton.IsEnabled = true;
        NextButton.BackgroundColor = Colors.Red;
    }

    private async void OnNextClicked(object sender, EventArgs e)
    {
        if (_selectedButton == null) return;

        var selectedOptionIndex = _optionButtons.IndexOf(_selectedButton) + 1;
        var currentQuestion = _questions[_currentQuestionIndex];
        bool isCorrect = selectedOptionIndex == currentQuestion.CorrectOption;

        // Store the question review
        _questionReviews.Add(new QuestionReview
        {
            QuestionNumber = _currentQuestionIndex + 1,
            QuestionText = currentQuestion.Text,
            UserAnswer = _selectedButton.Text,
            CorrectAnswer = _optionButtons[currentQuestion.CorrectOption - 1].Text,
            IsCorrect = isCorrect
        });

        if (isCorrect)
        {
            _correctAnswers++;
        }

        // Move to next question
        _currentQuestionIndex++;

        // Check if this was the last question
        if (_currentQuestionIndex >= _questions.Count)
        {
            // Calculate final time
            var finalTime = DateTime.Now - _quizStartTime;

            // Navigate to the ResultPage with the quiz results
            await Navigation.PushAsync(new ResultPage(_correctAnswers, _questions.Count, finalTime, _questionReviews));
        }
        else
        {
            ShowCurrentQuestion();
        }
    }

    private string FormatTime(TimeSpan timeSpan)
    {
        if (timeSpan.TotalHours >= 1)
        {
            return $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }
        else
        {
            return $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }
    }
}