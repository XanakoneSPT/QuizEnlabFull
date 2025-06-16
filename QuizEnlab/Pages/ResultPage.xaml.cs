using QuizEnlab.Models;

namespace QuizEnlab.Pages;

public partial class ResultPage : ContentPage
{
	private readonly List<QuestionReview> _questionReviews;

	public ResultPage(int score, int totalQuestions, TimeSpan time, List<QuestionReview> questionReviews)
	{
		InitializeComponent();
		_questionReviews = questionReviews;
		
		// Calculate score percentage
		double scorePercentage = (double)score / totalQuestions * 100;
		
		// Set the score and time
		ScoreLabel.Text = $"{score}/{totalQuestions}";
		
		// Set result title, message and icon based on score percentage
		if (scorePercentage >= 50)
		{
			ResultTitle.Text = "Congratulations!!";
			ResultMessage.Text = $"You're amazing!! \n\n {score}/{totalQuestions} correct answers in {FormatTime(time)}";
			ResultIcon.Source = "success.png";
		}
		else
		{
			ResultTitle.Text = "Completed!";
			ResultMessage.Text = $"Better luck next time! \n\n {score}/{totalQuestions} correct answers in {FormatTime(time)}";
			ResultIcon.Source = "fail.png";
		}
	}

	private async void OnTryAgainClicked(object sender, EventArgs e)
	{
		// Find the existing QuizPage in the navigation stack
		var quizPage = Navigation.NavigationStack
			.FirstOrDefault(p => p is QuizPage) as QuizPage;

		if (quizPage != null)
		{
			// Reset the existing QuizPage
			quizPage.Reset();
			await Navigation.PopAsync();
		}
		else
		{
			// Fallback to creating a new QuizPage if not found
			await Navigation.PopAsync();
			await Navigation.PushAsync(new QuizPage());
		}
	}

	private async void OnReviewAnswersClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new ReviewAnswersPage(_questionReviews));
	}

	private void OnExitClicked(object sender, EventArgs e)
	{
		Application.Current.Quit();
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