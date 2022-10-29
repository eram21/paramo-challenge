using Sat.Recruitment.Api.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sat.Recruitment.Api.Models
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string Messages { get; set; }

        public static Result Ok(string message)
        {
            return new Result
            {
                IsSuccess = true,
                Messages = message
            };
        }

        public static Result Error(string message)
        {
            return new Result
            {
                IsSuccess = false,
                Messages = message
            };
        }

        public static Result Error(Exception exception)
        {
            return new Result
            {
                IsSuccess = false,
                Messages = exception.GetaAllMessages()
            };
        }
    }
}