using System;
using System.IO;

namespace Sat.Recruitment.Api.Utils
{
    public static class Utils
    {
        public static string NormalizeEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return string.Empty;

            var aux = email.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);

            var atIndex = aux[0].IndexOf("+", StringComparison.Ordinal);

            aux[0] = atIndex < 0 ? aux[0].Replace(".", string.Empty) : aux[0].Replace(".", string.Empty).Remove(atIndex);

            return string.Join("@", new string[] { aux[0], aux[1] });
        }

        public static StreamReader ReadFromFile(string pathFile)
        {
            FileStream fileStream = new FileStream(pathFile, FileMode.Open);

            StreamReader reader = new StreamReader(fileStream);

            return reader;
        }
    }
}