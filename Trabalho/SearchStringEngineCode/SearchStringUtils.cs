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
    // TODO: Criar uma função de split que mantém o separador. Tentar criar outro método sem regex.
    public static class SearchStringUtils
    {

        #region TestSearchStrings
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
                int counter = 0;
                string searchString;

                while ((searchString = file.ReadLine()) != null)
                {
                    Console.WriteLine(searchString);
                }
                file.Close();
                //Console.WriteLine($ "File has {counter} lines.");
            }

            return "";
        }
        #endregion

        #region ReadTextInPdf
        public static string ReadTextInPdf(string fileDirectory, string fileName)
        {
            // TODO: Adicionar o diretorio escolhido, directoryName.
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

                using StreamWriter testFile = new(Directory.GetCurrentDirectory() + @"\pdfs\" + @"\testPdf.txt", append: false);
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
         * Method responsible for removing unnecessary characters from the search string.
          @see https://stackoverflow.com/questions/11395775/clean-the-string-is-there-any-better-way-of-doing-it
        */
        public static string NormalizeAndCleanText(string textString)
        {
            string normalizedSearchString = textString.ToLower().Normalize(NormalizationForm.FormD);

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

        #region FindExpressionsInPdf
        public static Dictionary<string, int> FindExpressionsInPdf(List<Tuple<string, string>> searchStringTokens, string pdfText)
        {
            List<Tuple<string, string>> expressionValidatorTuple = new List<Tuple<string, string>>();

            string normalizedText = NormalizeAndCleanText(pdfText);

            string[] splittedText = normalizedText.Split(" ");

            int countRepeatedTokens = 0;

            // searchStringTokens

            for (int i = 0; i < searchStringTokens.Count; i++)
            {

            }

            //Console.WriteLine("\nexpressionValidatorDict --> " + string.Join(", ", expressionValidatorTuple));

            Environment.Exit(0);

            Dictionary<string, int> repeatedTokensDictionary = new Dictionary<string, int>();

            /* Console.WriteLine("repeatedTokensDictionary --> " + string.Join(", ", repeatedTokensDictionary));

            Environment.Exit(0); */

            return repeatedTokensDictionary;
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

        #region PrintOutputs
        public static void PrintOutputs<T>(string outputName, string outputPrimitive = null, List<T> outputList = null, List<List<T>> outputListOfLists = null)
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
        }
        #endregion

        #region VerifyExpressions
        public static List<Tuple<string, string>> VerifyExpressions(List<List<string>> searchStringTokens, string normalizedText)
        {
            bool isAnd = false;
            bool isOr = false;
            string keyExpression = "";
            List<Tuple<string, string>> expressionValidatorTuple = new List<Tuple<string, string>>();

            List<string> lastExpression = searchStringTokens.Last();

            string auxString = "";

            // "teste and (verificação de linguagens)"
            // "a or b and c and d or e"

            foreach (List<string> expression in searchStringTokens)
            {
                keyExpression = "";
                isAnd = false;
                isOr = false;

                for (int i = 0; i < expression.Count; i++)
                {

                    keyExpression += " " + expression[i];

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
                                // TODO: Revisar esse caso onde só sobra uma palavra, talvez não precise ver se é and ou or
                                // TODO: só verificar se a palavra existe.
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
            // TODO: Procurar FileSeparator.

            return expressionValidatorTuple;
        }
        #endregion

        #region ValidateExpression
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

        #region CountWords

        public static Dictionary<string, int> CountWords()
        {
            Dictionary<string, int> repeatedTokensDictionary = new Dictionary<string, int>();
            return repeatedTokensDictionary;
        }
        #endregion

        #region AddAndValidation
        public static bool AddAndValidation(List<string> searchStringHandlerList, string searchWord, bool hasQuotationMarks)
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

            if (hasQuotationMarks)
            {
                return false;
            }

            return shouldAddAnd;

        }
        #endregion
    }
}