using QuizEnlab.Models;

namespace QuizEnlab.Pages;

public partial class ReviewAnswersPage : ContentPage
{
    public ReviewAnswersPage(List<QuestionReview> questionReviews)
    {
        InitializeComponent();
        QuestionsCollection.ItemsSource = questionReviews;
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}

// Model for question review
public class QuestionReview
{
    public int QuestionNumber { get; set; }
    public string QuestionText { get; set; }
    public string UserAnswer { get; set; }
    public string CorrectAnswer { get; set; }
    public bool IsCorrect { get; set; }
} 