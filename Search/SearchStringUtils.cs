﻿using System;
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
    // TODO: Criar uma função de split que mantém o separador. Tentar criar outro método sem regex.
    public static class SearchStringUtils
    {

        #region InputSearchString
        public static string InputSearchString()
        {
            // Verify if is VS Code or VS Studio
            /* if (Directory.GetFiles(Directory.GetCurrentDirectory(), ".sln").Length > 0)
            {

            } */
            return "";
        }
        #endregion

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

        #region ValidateStringExceptions
        public static bool ValidateStringExceptions(string searchStringCleaned)
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

        #region TokenizeSearchString
        //* Method responsible for tokenizing the search string.
        // @see https://stackoverflow.com/questions/16265247/printing-all-contents-of-array-in-c-sharp
        //Console.WriteLine("\n[" + string.Join(", ", splits) + "]\n");
        public static List<string> TokenizeSearchString(string searchStringCleaned)
        {

            List<string> searchStringHandlerList = new List<string>();

            string[] stringValidator = searchStringCleaned.Trim().Split(" ");

            bool hasQuotationMarks = false;
            string quotationString = "";
            string searchWord = "";

            foreach (string word in stringValidator)
            {
                searchWord = word;

                if (searchWord[0] == '\"')
                {
                    hasQuotationMarks = true;
                    quotationString = searchWord;
                    continue;
                }
                else if (hasQuotationMarks)
                {
                    quotationString += " " + searchWord;

                    if (searchWord[searchWord.Length - 1] != '\"')
                    {
                        continue;
                    }
                }

                if ((searchStringHandlerList.Count != 0) && (searchWord != "and" && searchWord != "or") && (searchWord != "(" && searchWord != ")") && (searchStringHandlerList.Last() != "and" && searchStringHandlerList.Last() != "or" && searchStringHandlerList.Last() != "(" && searchStringHandlerList.Last() != ")") && !hasQuotationMarks)
                {
                    searchStringHandlerList.Add("and");
                    searchStringHandlerList.Add(searchWord);
                }
                else
                {
                    if (hasQuotationMarks)
                    {
                        searchStringHandlerList.Add(quotationString);
                        hasQuotationMarks = false;
                        quotationString = "";
                    }
                    else
                    {
                        searchStringHandlerList.Add(searchWord);
                    }
                }
            }

            return searchStringHandlerList;
        }
        #endregion

        #region SeparateExpressions
        public static List<List<string>> SeparateExpressions(List<string> searchStringTokens)
        {
            string regexString = string.Join(" ", searchStringTokens);

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
                regexResult.Add(regexMatch.Groups[1].Value.Trim());
                int initialMatch = regexString.IndexOf(regexMatch.Value);
                regexString = regexString.Remove(initialMatch, regexMatch.Value.Count());
            }

            if (regexString != "")
            {
                regexResult.Add(regexString);
            }

            List<List<string>> tokenizedValidation = new List<List<string>>();

            foreach (var validation in regexResult)
            {
                tokenizedValidation.Add(TokenizeSearchString(validation));
            }

            return tokenizedValidation;
        }
        #endregion

        #region FindExpressionsInPdf
        public static Dictionary<string, int> FindExpressionsInPdf(List<List<string>> searchStringTokens, string filePath)
        {
            Dictionary<string, int> searchTokensdictionary = new Dictionary<string, int>();

            foreach (var sentence in searchStringTokens)
            {

            }

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

        #region  PrintLogs
        public static void PrintLogs<T>(T input, string name)
        {
            if (input.GetType() == typeof(string))
            {
                Console.WriteLine("string");
            }
            else if (input.GetType() == typeof(List<string>))
            {
                Console.WriteLine("List<string>");
            }
            else if (input.GetType() == typeof(List<List<string>>))
            {
                Console.WriteLine("List<List<string>>");
            }
        }
        #endregion
    }
}