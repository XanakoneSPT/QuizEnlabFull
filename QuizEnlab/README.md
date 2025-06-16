# Quiz Application

A .NET MAUI-based quiz application that allows users to test their knowledge through an interactive quiz interface.

## Features

- Interactive quiz interface with multiple-choice questions
- Real-time feedback on answer selection
- Quiz completion statistics including:
  - Time taken
  - Score
  - Pass/Fail status
- Answer review functionality
- Option to retry or exit after completion

## Screenshots

[Add screenshots here]

## User Flow Diagram

[Add user flow diagram here]

## Database Schema

[Add E-R diagram here]

## Technical Implementation

### Getting Questions
```csharp
public async Task<List<Questions>> GetQuestionsAsync()
{
    return await _database.Table<Questions>().ToListAsync();
}
```

### Answer Validation
```csharp
private void OnOptionSelected(object sender, EventArgs e)
{
    var selectedOptionIndex = _optionButtons.IndexOf(selectedButton) + 1;
    var currentQuestion = _questions[_currentQuestionIndex];
    bool isCorrect = selectedOptionIndex == currentQuestion.CorrectOption;
    
    // Update UI with feedback
    if (isCorrect)
    {
        selectedButton.BorderColor = Colors.Green;
        FeedbackLabel.Text = "Correct! ✓";
    }
    else
    {
        selectedButton.BorderColor = Colors.Red;
        FeedbackLabel.Text = "Incorrect ✗";
    }
}
```

### Quiz Results
```csharp
private async void OnNextClicked(object sender, EventArgs e)
{
    if (_currentQuestionIndex >= _questions.Count)
    {
        var finalTime = DateTime.Now - _quizStartTime;
        await Navigation.PushAsync(new ResultPage(
            _correctAnswers, 
            _questions.Count, 
            finalTime, 
            _questionReviews
        ));
    }
}
```

## Setup Instructions

1. Clone the repository
2. Open the solution in Visual Studio 2022
3. Build and run the application

## Requirements

- .NET 7.0 or later
- Visual Studio 2022 with .NET MAUI workload
- SQLite support

## License

[Add your license information here] 