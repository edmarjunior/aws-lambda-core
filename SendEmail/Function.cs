using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Newtonsoft.Json;
using System;
using System.Linq;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SendEmail
{
    public class Function
    {
        public void FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
        {
            try
            {
                Console.WriteLine("Init function");

                if (!IsValid(sqsEvent, out var email))
                {
                    return;
                }

                Console.WriteLine("Sending email...");

                email.Send();

                Console.WriteLine("Success: Sent");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fail:Exception: " + ex.Message);
            }
        }

        private bool IsValid(SQSEvent sqsEvent, out Email email)
        {
            Console.WriteLine("validing body...");

            email = null;

            var body = sqsEvent.Records.FirstOrDefault()?.Body;

            if (string.IsNullOrEmpty(body))
            {
                Console.WriteLine("Fail: Email not received");
                return false;
            }

            email = JsonConvert.DeserializeObject<Email>(body);

            if (email == null)
            {
                Console.WriteLine("Fail: Email not received");
                return false;
            }

            if (string.IsNullOrEmpty(email.To.Trim()))
            {
                Console.WriteLine("Fail: Missing parameter [To]");
                return false;
            }

            if (string.IsNullOrEmpty(email.Body.Trim()))
            {
                Console.WriteLine("Fail: Missing parameter [Body]");
                return false;
            }

            if (string.IsNullOrEmpty(email.Subject.Trim()))
            {
                Console.WriteLine("Fail: Missing parameter [Subject]");
                return false;
            }

            Console.WriteLine("Valid Ok");

            return true;
        }
    }
}
