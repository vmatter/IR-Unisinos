using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Text.RegularExpressions;
using iTextSharp.text;

namespace SearchStringHandler
{
    /*
     * Class that contains functions to deal with search strings.
    */
    public static class SearchStringUtils
    {

        #region TestSearchStrings
        /*
         * Function responsible for reading search strings from a .txt file. 
         TODO: (Future work) - Will be used in the option '2' to read search string from a .txt file.
        */
        public static string TestSearchStrings(string fileDirectory = "searchStrings", string fileName = "searchStrings")
        {
            string filePath = "";
            StringBuilder pdfText = new StringBuilder();

            if (!fileName.Contains(".txt"))
            {
                filePath = Directory.GetCurrentDirectory() + $@"\{fileDirectory}\" + fileName + ".txt";
            }
            else
            {
                filePath = Directory.GetCurrentDirectory() + $@"\{fileDirectory}\" + fileName;
            }

            // Read file using StreamReader. Reads file line by line    
            using (StreamReader file = new StreamReader(filePath))
            {
                string searchString;

                while ((searchString = file.ReadLine()) != null)
                {
                    Console.WriteLine(searchString);
                }
                file.Close();
            }

            return "";
        }
        #endregion

        #region ReadTextFromPdf
        /*
         * Reads the text from a PDF file and creates the PDF text in the verifyPdfText.txt file inside \tests.
        */
        public static string ReadTextFromPdf(string fileDirectory, string fileName)
        {
            string filePath = "";
            StringBuilder pdfText = new StringBuilder();

            if (!fileName.Contains(".pdf"))
            {
                filePath = Directory.GetCurrentDirectory() + $@"\{fileDirectory}\" + fileName + ".pdf";
            }
            else
            {
                filePath = Directory.GetCurrentDirectory() + $@"\{fileDirectory}\" + fileName;
            }

            using (PdfReader reader = new PdfReader(filePath))
            {
                using StreamWriter testFile = new(Directory.GetCurrentDirectory() + @"\tests\" + @"\verifyPdfText.txt", append: false);
                {
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        string aux = PdfTextExtractor.GetTextFromPage(reader, i);
                        string[] linhas = aux.Split('\n');
                        foreach (string linha in linhas)
                        {
                            pdfText.Append($"{linha}{"\n"}");
                            testFile.WriteLine(linha);
                        }
                    }
                }
            }

            return pdfText.ToString();
        }
        #endregion

        #region NormalizeAndCleanText
        /*
         * Fuction responsible for removing unnecessary characters from the search string.
         * @see https://stackoverflow.com/questions/11395775/clean-the-string-is-there-any-better-way-of-doing-it
        */
        public static string NormalizeAndCleanText(string textString)
        {
            string normalizedSearchString = textString.ToLower().Normalize(NormalizationForm.FormD);

            string textCleaned = "";

            for (int i = 0; i < normalizedSearchString.Length; i++)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(normalizedSearchString[i]);

                if (unicodeCategory != UnicodeCategory.NonSpacingMark && normalizedSearchString[i] != '\n' && normalizedSearchString[i] != '\r' && normalizedSearchString[i] != '\t')
                {
                    if (normalizedSearchString[i] == '(')
                    {
                        textCleaned += normalizedSearchString[i] + " ";
                    }
                    else if (normalizedSearchString[i] == ')')
                    {
                        textCleaned += " " + normalizedSearchString[i];
                    }
                    else if (normalizedSearchString[i] == '\"' || Char.IsLetterOrDigit(normalizedSearchString[i]) || Char.IsWhiteSpace(normalizedSearchString[i]))
                    {
                        textCleaned += normalizedSearchString[i];
                    }
                }
            }

            return textCleaned.Normalize(NormalizationForm.FormC);
        }
        #endregion

        #region ValidateStringExceptions
        /*
         * Function that validates the strings expressions.
        */
        public static bool ValidateStringExceptions(string searchStringCleaned)
        {
            bool areValidParentheses = false;
            int countOpenParentheses = 0;
            int countClosedParentheses = 0;

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
        /* 
         * Function responsible for tokenizing the search string.
         * @see https://stackoverflow.com/questions/16265247/printing-all-contents-of-array-in-c-sharp
        */
        public static List<string> TokenizeSearchString(string searchStringCleaned)
        {

            string[] stringValidator = searchStringCleaned.Trim().Split(" ");

            List<string> searchStringHandlerList = new List<string>();
            bool hasQuotationMarks = false;
            string quotationString = "";
            string searchWord = "";

            foreach (string word in stringValidator)
            {
                searchWord = word;

                if ((searchWord != "") && (searchWord[0] == '\"'))
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

                if (AddAndValidation(searchStringHandlerList, searchWord, hasQuotationMarks))
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

        #region SeparateExpressionsFromParentheses
        /*
         * Function that separates the expressions from parentheses.
        */
        public static List<List<string>> SeparateExpressionsFromParentheses(List<string> searchStringTokens)
        {
            string regexString = string.Join(" ", searchStringTokens);

            List<string> regexResult = new List<string>();

            string pattern = @"\(([^\()]+)\)";

            while (Regex.Match(regexString, pattern, RegexOptions.IgnoreCase).Success)
            {
                if (regexString[0] != '(')
                {
                    int indexRegexString = regexString.IndexOf('(');
                    string aux = regexString[0..indexRegexString];
                    if (!String.IsNullOrWhiteSpace(aux))
                    {
                        regexResult.Add(aux);
                    }
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

        #region CountSearchTokensInPdf
        /*
         * Function that counts the search strings in the PDF file.
        */
        public static Dictionary<string, int> CountSearchTokensInPdf(List<Tuple<string, string>> searchStringTokens, string pdfText)
        {

            Dictionary<string, int> repeatedTokensDictionary = new Dictionary<string, int>();

            string normalizedText = NormalizeAndCleanText(pdfText);

            int searchCount = 0;

            string teste = "desenvolvimento de aplicacoes";

            List<int> listTest = new List<int>();

            int index = 0;

            // Metodo que retorna 
            /* while (index != -1)
            {
                index = normalizedText.IndexOf(teste, index + teste.Length);

                if (index >= 0)
                {
                    listTest.Add(index);
                }
            } */

            //Console.WriteLine("\nlistTest Count --> " + listTest.Count());

            //Environment.Exit(0);

            string[] splittedText = normalizedText.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            string[] expressionsSplitted = null;

            int countRepeatedTokens = 0;

            for (int i = 0; i < searchStringTokens.Count; i++)
            {
                if (searchStringTokens[i].Item2 == "True")
                {
                    if ((searchStringTokens[i].Item1.Contains("and ")) || (searchStringTokens[i].Item1.Contains(" and ")) || (searchStringTokens[i].Item1.Contains(" and")) && (searchStringTokens[i].Item1 != "operator"))
                    {
                        expressionsSplitted = searchStringTokens[i].Item1.Split(new string[] { " and", " and ", "and " }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (string expressionWord in expressionsSplitted)
                        {
                            countRepeatedTokens = 0;
                            foreach (string item in splittedText)
                            {
                                if (item.Equals(expressionWord.Trim()))
                                {
                                    countRepeatedTokens++;
                                }
                            }
                            if (repeatedTokensDictionary.ContainsKey(expressionWord.Trim()))
                            {
                                repeatedTokensDictionary[expressionWord.Trim()] += countRepeatedTokens;
                            }
                            else
                            {
                                repeatedTokensDictionary.Add(expressionWord.Trim(), countRepeatedTokens);
                            }
                        }
                    }
                    else if ((searchStringTokens[i].Item1.Contains("or ")) || (searchStringTokens[i].Item1.Contains(" or ")) || (searchStringTokens[i].Item1.Contains(" or")) && (searchStringTokens[i].Item1 != "operator"))
                    {

                        expressionsSplitted = searchStringTokens[i].Item1.Split(new string[] { " or", " or ", "or " }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string expressionWord in expressionsSplitted)
                        {
                            countRepeatedTokens = 0;
                            foreach (string item in splittedText)
                            {
                                if (item.Equals(expressionWord.Trim()))
                                {
                                    countRepeatedTokens++;
                                }
                            }
                            if (repeatedTokensDictionary.ContainsKey(expressionWord.Trim()))
                            {
                                repeatedTokensDictionary[expressionWord.Trim()] += countRepeatedTokens;
                            }
                            else
                            {
                                repeatedTokensDictionary.Add(expressionWord.Trim(), countRepeatedTokens);
                            }
                        }
                    }
                }
            }

            return repeatedTokensDictionary;
        }
        #endregion

        #region GenerateReport
        /*
        * Function that generates the report containing the general results of the string search.
        */
        public static string GenerateReport(int countQuery, string fileName, string searchString, Dictionary<string, int> searchTokensInDictionary)
        {
            StringBuilder report = new StringBuilder();

            StringBuilder occurrences = new StringBuilder();

            report.AppendLine("\n\n*****************************************");
            report.AppendLine($"Número da consulta: {countQuery}");
            report.AppendLine($"Nome do documento: {fileName}");
            report.AppendLine($"String de busca: {searchString}");

            var lastToken = searchTokensInDictionary.Last();

            foreach (var token in searchTokensInDictionary)
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

            string directoryPath = $@"{Directory.GetCurrentDirectory()}\generatedReport";

            // Handles writing in the .txt file.
            File.AppendAllText($@"{directoryPath}\generatedReport.txt", report.ToString());

            // Handles writing in the .pdf file.
            StreamReader txtReport = new StreamReader($@"{directoryPath}\generatedReport.txt");
            Document pdfReport = new Document();
            PdfWriter.GetInstance(pdfReport, new FileStream($@"{directoryPath}\generatedReport.pdf", FileMode.Create));
            pdfReport.Open();
            pdfReport.Add(new Paragraph(txtReport.ReadToEnd()));
            pdfReport.Close();

            return report.ToString();
        }
        #endregion

        #region PrintOutputs
        /*
         * Function responsible for handling the print outputs of different functions.
        */
        public static void PrintOutputs<T>(string outputName, string outputPrimitive = null, List<T> outputList = null, List<List<T>> outputListOfLists = null, Dictionary<string, int> outputDictionary = null)
        {
            if (outputPrimitive != null)
            {
                Console.WriteLine($"\n{outputName} ({outputPrimitive.GetType().Name})\t\t-->\t{outputPrimitive}");
            }
            else if (outputList != null)
            {
                Console.WriteLine($"\n{outputName} ({outputList.GetType().Name})\t\t-->\t[" + string.Join(", ", outputList) + "]");
            }
            else if (outputListOfLists != null)
            {
                StringBuilder listOfListsOutput = new StringBuilder();

                List<T> outerLastValue = outputListOfLists[outputListOfLists.Count() - 1];

                foreach (var sentence in outputListOfLists)
                {
                    T innerLastValue = sentence[sentence.Count() - 1];
                    foreach (var word in sentence)
                    {
                        if (!word.Equals(innerLastValue))
                        {
                            listOfListsOutput.Append((string.Join(" ", word)) + " ");
                        }
                        else
                        {
                            listOfListsOutput.Append((string.Join(" ", word)));
                        }
                    }
                    if (!sentence.Equals(outerLastValue))
                    {
                        listOfListsOutput.Append(", ");

                    }
                }
                Console.WriteLine($"\n{outputName} ({outputListOfLists.GetType().Name})({outputListOfLists.GetType().Name})\t-->\t[{listOfListsOutput}]");
            }
            else if (outputDictionary != null)
            {
                StringBuilder dictionaryOutput = new StringBuilder();

                string lastDictionaryKey = outputDictionary.Keys.Last().ToString();

                foreach (var token in outputDictionary)
                {

                    if (!token.Key.Equals(lastDictionaryKey))
                    {
                        dictionaryOutput.Append((string.Join(" ", token)) + " ");
                    }
                    else
                    {
                        dictionaryOutput.Append((string.Join(" ", token)));
                    }
                }
                Console.WriteLine($"\n{outputName} ({dictionaryOutput.GetType().Name})\t-->\t[{dictionaryOutput}]");
            }
        }
        #endregion

        #region VerifyExpressions
        /*
         * Function that verifies the expressions in order to validate which ones should be printed. 
         TODO: (Future Workd) - This function can be optimized to a binary tree, now it is not working properly.
        */
        public static List<Tuple<string, string>> VerifyExpressions(List<List<string>> searchStringTokens, string pdfText)
        {

            bool isAnd = false;
            bool isOr = false;
            string keyExpression = "";
            string auxString = "";
            List<Tuple<string, string>> expressionValidatorTuple = new List<Tuple<string, string>>();

            string normalizedText = NormalizeAndCleanText(pdfText);

            List<string> lastExpression = searchStringTokens.Last();

            foreach (List<string> expression in searchStringTokens)
            {
                keyExpression = "";
                isAnd = false;
                isOr = false;

                for (int i = 0; i < expression.Count; i++)
                {

                    if (expression[i].Contains("\""))
                    {
                        keyExpression += " " + expression[i].Trim().Replace("\"", "");
                    }
                    else
                    {
                        keyExpression += " " + expression[i];
                    }

                    if (expression[i] == "and")
                    {
                        if (isOr)
                        {
                            auxString = keyExpression.Substring(0, keyExpression.LastIndexOf("and")).Trim();
                            expressionValidatorTuple.Add(new Tuple<string, string>(auxString, (ValidateExpression(auxString, normalizedText, isOr: true)).ToString()));
                            expressionValidatorTuple.Add(new Tuple<string, string>("and", "operator"));
                            keyExpression = "";
                            isAnd = false;
                            isOr = false;
                            continue;
                        }

                        isAnd = true;
                        isOr = false;
                    }
                    else if (expression[i] == "or")
                    {
                        if (isAnd)
                        {
                            auxString = keyExpression.Substring(0, keyExpression.LastIndexOf("or")).Trim();
                            expressionValidatorTuple.Add(new Tuple<string, string>(auxString, (ValidateExpression(auxString, normalizedText, isAnd: true)).ToString()));
                            expressionValidatorTuple.Add(new Tuple<string, string>("or", "operator"));
                            keyExpression = "";
                            isAnd = false;
                            isOr = false;
                            continue;
                        }
                        isAnd = false;
                        isOr = true;
                    }

                    if (i == expression.Count - 1)
                    {
                        if (expression[i] == "and")
                        {
                            auxString = keyExpression.Substring(0, keyExpression.LastIndexOf("and")).Trim();

                            if (auxString != "")
                            {
                                expressionValidatorTuple.Add(new Tuple<string, string>(auxString, (ValidateExpression(auxString, normalizedText, isAnd: true)).ToString()));
                            }
                            expressionValidatorTuple.Add(new Tuple<string, string>("and", "operator"));
                        }
                        else if (expression[i] == "or")
                        {
                            auxString = keyExpression.Substring(0, keyExpression.LastIndexOf("or")).Trim();

                            if (auxString != "")
                            {
                                expressionValidatorTuple.Add(new Tuple<string, string>(auxString, (ValidateExpression(auxString, normalizedText, isOr: true)).ToString()));
                            }
                            expressionValidatorTuple.Add(new Tuple<string, string>("or", "operator"));
                        }
                        else
                        {
                            if (isAnd)
                            {
                                expressionValidatorTuple.Add(new Tuple<string, string>(keyExpression.Trim(), (ValidateExpression(keyExpression, normalizedText, isAnd: true)).ToString()));
                            }
                            else if (isOr)
                            {
                                expressionValidatorTuple.Add(new Tuple<string, string>(keyExpression.Trim(), (ValidateExpression(keyExpression, normalizedText, isOr: true)).ToString()));
                            }
                            else
                            {
                                auxString = expression[i - 1];

                                if (auxString == "and")
                                {
                                    expressionValidatorTuple.Add(new Tuple<string, string>(keyExpression.Trim(), (ValidateExpression(keyExpression, normalizedText, isAnd: true)).ToString()));
                                }
                                else if (auxString == "or")
                                {
                                    expressionValidatorTuple.Add(new Tuple<string, string>(keyExpression.Trim(), (ValidateExpression(keyExpression, normalizedText, isOr: true)).ToString()));
                                }
                            }
                        }
                    }
                }
            }

            return expressionValidatorTuple;
        }
        #endregion

        #region ValidateExpression
        /*
         * Function that validates if the expression is valid or not.
        */
        private static bool ValidateExpression(string expression, string normalizedText, bool isAnd = false, bool isOr = false)
        {
            bool isValidExpression = true;
            string[] splittedExpression = null;
            int countWordForOr = 0;

            if (isAnd)
            {
                splittedExpression = expression.Split(new string[] { " and", " and ", "and " }, StringSplitOptions.RemoveEmptyEntries);
            }
            else if (isOr)
            {
                splittedExpression = expression.Split(new string[] { " or", " or ", "or " }, StringSplitOptions.RemoveEmptyEntries);
            }

            foreach (string word in splittedExpression)
            {

                string searchWord = " " + word.Trim() + " ";

                if (isAnd)
                {
                    if (!normalizedText.Contains(searchWord))
                    {
                        isValidExpression = false;
                        break;
                    }
                }
                else if (isOr)
                {
                    if (normalizedText.Contains(searchWord))
                    {
                        countWordForOr++;
                    }

                }
            }

            if (isOr)
            {
                if (countWordForOr == 0)
                {
                    isValidExpression = false;
                }
            }

            return isValidExpression;
        }
        #endregion

        #region AddAndValidation

        /*
         * Function that contains all validations to add "and" to words that does not contain an operator.
        */
        private static bool AddAndValidation(List<string> searchStringHandlerList, string searchWord, bool hasQuotationMarks)
        {

            bool shouldAddAnd = true;

            if ((searchWord == "and" || searchWord == "or") || (searchWord == "(" || searchWord == ")"))
            {
                return false;
            }

            if (searchStringHandlerList.Count > 0)
            {
                if ((searchStringHandlerList.Last() == "and") || (searchStringHandlerList.Last() == "or"))
                {
                    return false;
                }
            }
            else if (searchStringHandlerList.Count == 0)
            {
                return false;
            }

            if (hasQuotationMarks)
            {
                return false;
            }

            return shouldAddAnd;

        }
        #endregion
    }
    // TODO: List of TODOs of the project that will be implemented in a not so distant future.
    // TODO: Implement the option '2' of the menu that will load the search strings from a .txt file (TestSearchStrings function).
    // TODO: Add better commentaries to ensure the documentation quality.
    // TODO: Implement a Split function that keeps the separator. 
    // TODO: Try to SeparateExpressions without using Regex.
    // TODO: Verify if C# has an implementation of FileSeparator like Java.
    // TODO: Change the dictionary to an OrderedDictionary.
    // TODO: Rework the VerifyExpressions using a BinaryTree.
    // TODO: Review all the code after Marcio`s reivions.
    // TODO: Validate if all files and directories exists else create them.
}