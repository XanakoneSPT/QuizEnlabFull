using QuizEnlab.Pages;

namespace QuizEnlab
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private async void onStartClicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new QuizPage());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Failed to start quiz: " + ex.Message, "OK");
            }
            
        }
    }

}
