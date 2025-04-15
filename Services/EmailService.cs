using Microsoft.OpenApi.Models;
using Project_Management_System.Interfaces;
using RestSharp;

namespace Project_Management_System.Services
{
    public class EmailService : IEmail
    {

        public string Generate2FAToken()
        {
            var random = new Random();
            int token = random.Next(100000, 999999); // Generates a 6-digit number
            return token.ToString(); // Returns it as a string
        }

        public bool SendEmail(string ToEmail, string Subject, string Content)
        {
            var client = new RestClient("https://sandbox.api.mailtrap.io/api/send/3566115");
            var request = new RestRequest();
            request.AddHeader("Authorization", "Bearer 61af4b23bc4537a557d25b439a2c1129");
            request.AddHeader("Content-Type", "application/json");

            // Corrected string interpolation for dynamic subject and content
            var jsonPayload = $@"
    {{
        ""from"": {{
            ""email"": ""hello@example.com"",
            ""name"": ""Mailtrap Test""
        }},
        ""to"": [
            {{
                ""email"": ""{ToEmail}""
            }}
        ],
        ""subject"": ""{Subject}"",
        ""text"": "" Your Two Factor Authentication Code is : {Content}, Do not Share it With anyone!"",
        ""category"": ""Integration Test""
    }}";

            // Add the JSON payload as the body of the request
            request.AddParameter("application/json", jsonPayload, ParameterType.RequestBody);

            // Send the request and return whether it was successful
            var response = client.Post(request);
            return response.IsSuccessful;
        }

    }
}
