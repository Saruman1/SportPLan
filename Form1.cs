namespace SportPlan
{
    using System.Net.Http.Json;
    using System.Threading.Tasks;


    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public async Task GetData(string language)
        {
            string apiKey = "Your API key";

            string endpoint = "https://api.openai.com/v1/chat/completions";

            List<Message> messages = new List<Message>();

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");


            var content = "";

            if (language == "Ukrainian")
            {
                content = $"Using the provided data, please create a personalized sports nutrition plan that takes into account the user's body mass: {TxtBodyMass.Text}, body fat percentage: {TxtBodyFat.Text}, type of sport: {TxtKindOfSport.Text}, number of trainings per week: {TxtNumberOfTrainings.Text}, personal goal: {TxtPersonalGoal.Text}, restrictions: {TxtRestrictions.Text}, and wishes regarding food: {TxtWishesRegardingsFood.Text}. Your plan should include specific recommendations for macronutrient intake (carbohydrates, protein, and fats), as well as suggestions for meal timing and hydration. Please provide clear explanations for your recommendations, citing relevant research studies or expert opinions where appropriate. Additionally, please ensure that your plan is flexible enough to accommodate any dietary preferences or restrictions while still meeting the user's nutritional needs. Your goal is to provide a comprehensive and practical nutrition plan that will help the user achieve their desired goals in their chosen sport. Please format your response in an organized and easy-to-follow manner. It must be in Ukrainian.";

            }
            else if (language == "English")
            {
                content = $"Using the provided data, please create a personalized sports nutrition plan that takes into account the user's body mass: {TxtBodyMass.Text}, body fat percentage: {TxtBodyFat.Text}, type of sport: {TxtKindOfSport.Text}, number of trainings per week: {TxtNumberOfTrainings.Text}, personal goal: {TxtPersonalGoal.Text}, restrictions: {TxtRestrictions.Text}, and wishes regarding food: {TxtWishesRegardingsFood.Text}. Your plan should include specific recommendations for macronutrient intake (carbohydrates, protein, and fats), as well as suggestions for meal timing and hydration. Please provide clear explanations for your recommendations, citing relevant research studies or expert opinions where appropriate. Additionally, please ensure that your plan is flexible enough to accommodate any dietary preferences or restrictions while still meeting the user's nutritional needs. Your goal is to provide a comprehensive and practical nutrition plan that will help the user achieve their desired goals in their chosen sport. Please format your response in an organized and easy-to-follow manner.";

            }


            if (content is not { Length: > 0 })
                errorProvider1.DataSource = "Error!";

            var message = new Message() { Role = "user", Content = content };

            messages.Add(message);


            var requestData = new Request()
            {
                ModelId = "gpt-3.5-turbo",
                Messages = messages
            };

            using var response = await httpClient.PostAsJsonAsync(endpoint, requestData);


            if (!response.IsSuccessStatusCode)
            {
                errorProvider1.DataSource = $"{(int)response.StatusCode} {response.StatusCode}";

            }

            ResponseData? responseData = await response.Content.ReadFromJsonAsync<ResponseData>();

            var choices = responseData?.Choices ?? new List<Choice>();
            if (choices.Count == 0)
            {
                errorProvider1.DataSource = "No choices were returned by the API";

            }
            var choice = choices[0];
            var responseMessage = choice.Message;

            messages.Add(responseMessage);
            var responseText = responseMessage.Content.Trim();
            TxtOutPut.Text = responseText;
            return;

        }

        private void BtnStart_Click(object sender, EventArgs e)
        {

            GetData("English");
        }

        private void BtnStartUkrainian_Click(object sender, EventArgs e)
        {
            GetData("Ukrainian");
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}