using Sat.Recruitment.Api.Utils;
using System;
using System.Diagnostics;

namespace Sat.Recruitment.Api.Models
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string Messages { get; set; }

        public static Result Ok(string message = "")
        {
            Debug.WriteLine(message);
            return new Result
            {
                IsSuccess = true,
                Messages = message
            };
        }

        public static Result Error(string message)
        {
            Debug.WriteLine(message);
            return new Result
            {
                IsSuccess = false,
                Messages = message
            };
        }

        public static Result Error(Exception exception)
        {
            var messages = exception.GetaAllMessages();
            Debug.WriteLine(messages);
            return new Result
            {
                IsSuccess = false,
                Messages = messages
            };
        }
    }
}