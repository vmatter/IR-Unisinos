using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text.RegularExpressions;

namespace SearchStringHandler
{
    // TODO: Documentar.
    // TODO: Criar uma função de split que mantém o separador.
    public static class SearchStringUtils
    {
        #region CleanSearchString
        /*
         * Method responsible for removing unnecessary characters from the search string.
          @see https://stackoverflow.com/questions/11395775/clean-the-string-is-there-any-better-way-of-doing-it
        */
        public static string CleanSearchString(string searchString)
        {
            string normalizedSearchString = searchString.ToLower().Normalize(NormalizationForm.FormD);

            string textCleaned = "";

            foreach (char c in normalizedSearchString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);

                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    if (c == '(')
                    {
                        textCleaned += c + " ";
                    }
                    else if (c == ')')
                    {
                        textCleaned += " " + c;
                    }
                    else if (c == '\"' || Char.IsLetterOrDigit(c) || Char.IsWhiteSpace(c))
                    {
                        textCleaned += c;
                    }

                }

            }

            return textCleaned.Normalize(NormalizationForm.FormC);
        }
        #endregion 

        #region TokenizeSearchString
        //* Method responsible for tokenizing the search string.
        // @see https://stackoverflow.com/questions/16265247/printing-all-contents-of-array-in-c-sharp
        //Console.WriteLine("\n[" + string.Join(", ", splits) + "]\n");
        public static List<string> TokenizeSearchString(string searchStringCleaned, bool areValidParentheses)
        {

            List<string> searchStringHandlerList = new List<string>();

            string[] stringValidator = searchStringCleaned.Split(" ");

            bool hasQuotation = false;
            string quotationString = "";
            string searchWord = "";

            Console.WriteLine("\n[" + string.Join(", ", stringValidator) + "]\n");

            foreach (string word in stringValidator)
            {
                searchWord = word;

                if (searchWord[0] == '\"')
                {
                    hasQuotation = true;
                    quotationString = searchWord;
                    continue;
                }
                else if (hasQuotation)
                {
                    if (searchWord[searchWord.Length - 1] == '\"')
                    {

                        quotationString += " " + searchWord;
                    }
                    else
                    {
                        quotationString += " " + searchWord;
                        continue;
                    }
                }

                if ((searchStringHandlerList.Count != 0) && (searchWord != "and" && searchWord != "or") && (searchWord != "(" && searchWord != ")") && (searchStringHandlerList.Last() != "and" && searchStringHandlerList.Last() != "or" && searchStringHandlerList.Last() != "(" && searchStringHandlerList.Last() != ")") && !hasQuotation)
                {
                    searchStringHandlerList.Add("and");
                    searchStringHandlerList.Add(searchWord);
                }
                else
                {
                    if (hasQuotation)
                    {
                        searchStringHandlerList.Add(quotationString);
                        hasQuotation = false;
                        quotationString = "";
                    }
                    else
                    {
                        searchStringHandlerList.Add(searchWord);
                    }
                }
            }

            Console.WriteLine("\nsearchStringHandlerList --> [" + string.Join(", ", searchStringHandlerList) + "]");

            return searchStringHandlerList;
        }

        #region SearchTokensInPdf
        public static Dictionary<string, int> SearchTokensInPDF(List<string> searchStringTokens, string filePath)
        {
            Dictionary<string, int> searchTokensdictionary = new Dictionary<string, int>();

            string regexString = string.Join(" ", searchStringTokens);

            Console.WriteLine("regexString --> " + regexString);

            List<string> regexResult = new List<string>();

            string pattern = @"\(([^\()]+)\)";

            while (Regex.Match(regexString, pattern, RegexOptions.IgnoreCase).Success)
            {
                if (regexString[0] != '(')
                {
                    int indexRegexString = regexString.IndexOf('(');
                    regexResult.Add(regexString[0..indexRegexString]);
                    regexString = regexString.Remove(0, indexRegexString);
                }
                Match regexMatch = Regex.Match(regexString, pattern, RegexOptions.IgnoreCase);
                Console.WriteLine("Found '{0}' at position {1}.", regexMatch.Value, regexMatch.Index);
                regexResult.Add(regexMatch.Value);
                int initialMatch = regexString.IndexOf(regexMatch.Value);
                regexString = regexString.Remove(initialMatch, regexMatch.Value.Count());
                //Console.WriteLine("regexString --> " + regexString);
            }

            if (regexString != "")
            {
                regexResult.Add(regexString);
            }

            Console.WriteLine("regexResult --> [" + string.Join(",", regexResult) + "]");

            string text = "texto e a";

            foreach (var validation in regexResult)
            {
                // TODO: revisar TokenizeSearchString.
                List<string> tokenizedValidation = TokenizeSearchString(validation,true);
            }

            Environment.Exit(0);

            List<List<string>> groups = new List<List<string>>();

            

            Environment.Exit(0);

            return searchTokensdictionary;
        }
        #endregion

        #region GenerateReport
        public static void GenerateReport(int contQuery, string filePath, string searchString, Dictionary<string, int> searchTokensdictionary)
        {
            int fileNameIndex = filePath.LastIndexOf(@"\") + 1;

            string fileName = filePath.Substring(fileNameIndex, (filePath.Length - fileNameIndex));

            searchTokensdictionary = new Dictionary<string, int>();

            searchTokensdictionary.Add("desenvolvimento", 1);
            searchTokensdictionary.Add("aplicação", 3);

            var report = new StringBuilder();

            var occurrences = new StringBuilder();

            report.AppendLine("*****************************************");
            report.AppendLine($"Número da consulta: {contQuery}");
            report.AppendLine($"Nome do documento: {fileName}");
            report.AppendLine($"String de busca: {searchString}");

            var lastToken = searchTokensdictionary.Last();

            foreach (var token in searchTokensdictionary)
            {
                if (!token.Equals(lastToken))
                {
                    occurrences.Append($"{token.Key}({token.Value}), ");
                }
                else
                {
                    occurrences.Append($"{token.Key}({token.Value})");
                }
            }
            report.AppendLine($"Ocorrências: {occurrences}");
            report.AppendLine("*****************************************");

            Console.WriteLine("\n" + report);
        }
        #endregion

        public static bool ValidateStringConditions(string searchStringCleaned)
        {
            bool areValidParentheses = false;
            int countOpenParentheses = 0;
            int countClosedParentheses = 0;

            // TODO: Validar se a string for vazia.

            if (searchStringCleaned[0] == '\"' && searchStringCleaned[searchStringCleaned.Length - 1] == '\"')
            {
                throw new InvalidOperationException("\nString de busca está toda entre aspas.");
            }

            if (searchStringCleaned.Length > 0)
            {
                // Count how many parentheses exist.
                countOpenParentheses = searchStringCleaned.Length - searchStringCleaned.Replace("(", "").Length;
                countClosedParentheses = searchStringCleaned.Length - searchStringCleaned.Replace(")", "").Length;
            }
            else
            {
                throw new InvalidOperationException("\nString de busca está vazia.");
            }

            if ((countOpenParentheses - countClosedParentheses) != 0)
            {
                throw new InvalidOperationException("\nString de busca apresenta parênteses irregulares.");
            }
            else
            {
                areValidParentheses = true;
            }

            return areValidParentheses;
        }
        #endregion

        // TODO: Se der tempo criar um split com separador. Adicionar validações.
        private static char[] SplitWithSeparator(string stringToSplit)
        {
            //stringToSplit.Split
            //char[] splits = new char
            return new char[2];
        }

    }

}