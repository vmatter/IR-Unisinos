using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace SearchStringHandler
{
    //TODO: Documentar.
    public static class Utils
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
                    if (c == '\"' || c == '(' || c == ')' || Char.IsLetterOrDigit(c) || Char.IsWhiteSpace(c))
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
        public static Stack<string> TokenizeSearchString(string searchStringCleaned, bool areValidParentheses)
        {

            Stack<string> searchStringHandlerStack = new Stack<string>();
            Stack<string> searchStringTokenizedStack = new Stack<string>();

            Console.WriteLine("\n" + searchStringCleaned);

            string[] stringValidator = searchStringCleaned.Split(" ");

            List<string> andListTest = new List<string>();

            int qtdParentheses = 0;
            bool hasQuotation = false;
            bool closeParentheses = false;
            string quotationString = "";
            string stringBeforeParentheses = "";
            string searchWord = "";

            // TODO: Verificar o hasQuotation para os parênteses

            foreach (string word in stringValidator)
            {
                searchWord = word;
                if (areValidParentheses)
                {
                    if (searchWord[0] == '(')
                    {
                        closeParentheses = false;
                        qtdParentheses++;
                        searchStringHandlerStack.Push("(");
                        searchWord = word.Substring(1);
                    }
                    else if (searchWord[searchWord.Length - 1] == ')')
                    {
                        closeParentheses = true;
                        stringBeforeParentheses = searchWord.Remove(searchWord.Length - 1);
                    }
                }

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
                        quotationString += searchWord;
                        continue;
                    }
                }

                if ((searchWord != "and" && searchWord != "or") && (searchStringHandlerStack.Peek() != "and" && searchStringHandlerStack.Peek() != "or" && searchStringHandlerStack.Peek() != "(" && searchStringHandlerStack.Peek() != ")") && !hasQuotation)
                {
                    searchStringHandlerStack.Push("and");
                    searchStringHandlerStack.Push(searchWord);
                }
                else
                {
                    if (hasQuotation)
                    {
                        searchStringHandlerStack.Push(quotationString);
                        hasQuotation = false;
                        quotationString = "";
                    }
                    else if (closeParentheses)
                    {
                        searchStringHandlerStack.Push(stringBeforeParentheses);
                        searchStringHandlerStack.Push(")");
                        closeParentheses = false;
                    }
                    else
                    {
                        searchStringHandlerStack.Push(searchWord);
                    }
                }
            }

            Console.WriteLine("\nstackTest --> [" + string.Join(", ", searchStringHandlerStack) + "]");

            foreach (string word in searchStringHandlerStack)
            {
                searchStringTokenizedStack.Push(word);
            }

            Console.WriteLine("\nstackPrint --> [" + string.Join(", ", searchStringTokenizedStack) + "]");

            return searchStringTokenizedStack;
        }

        #region SearchTokensInPdf
        public static void SearchTokensInPDF(string filePath, string searchString, int contQuery)
        {
            string searchStringCleaned = CleanSearchString(searchString);
            //TokenizeSearchString(searchStringCleaned);

            /*using (PdfReader reader = new PdfReader(@filePath))
            {
                var texto = new System.Text.StringBuilder();

                // [0] == desenvolvimento , [1] = aplicacao

                // projeto,projeto não

                /*using StreamWriter file = new(@"E:\vitor_desktop\iCybersec\C#\Trabalho I\Busca-por-strings-em-documentos\testes\text55.txt");
                {
                    file.WriteLine($"*****************************************");
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        string aux = PdfTextExtractor.GetTextFromPage(reader, i);
                        string[] linhas = aux.Split('\n');
                        foreach (string linha in linhas)
                        {

                            foreach (string word in searchStringCleaned)
                            {
                                if (linha.Contains(@"" + word))
                                {
                                    texto.Append($"{word}{"\n"}");
                                    file.WriteLine(word);

                                }
                            }
                        }
                    }
                    file.WriteLine($"*****************************************");
                    Console.WriteLine(texto);
                }

            } */
            //contQuery++;
            //ShowHistoryReport(contQuery, filePath, searchString, ocurrences);

            Environment.Exit(0);

            Console.WriteLine("\nSearch String --> " + searchStringCleaned);

            Dictionary<string, int> ocurrences = new Dictionary<string, int>();

            List<string> listOfAND = new List<string>();
            List<string> listOfOR = new List<string>();
            List<string> listAll = new List<string>();

            //Environment.Exit(0);
            //Console.WriteLine("\nstackTest --> [" + string.Join(", ", stackTest) + "]\n");

            //Environment.Exit(0);

            // [(, desenvolvimento, and, aplicacao, ), and, (, teste, or, validacao, or vacina, )]

            // ["(, "texto and info" ,), "or" , "(", "a and b", ")"] 

            //string text = "Um texto contendo desenvolvimento, validacao";
            bool valid = false;
            //["desenvolvimento and aplicacao and teste or valor or verdade"]

            if (searchStringCleaned.Contains(" and "))
            {

                listAll = searchStringCleaned.Split(" and ").ToList();
                listOfAND = listAll.ToList();

                //Console.WriteLine("\ntext --> " + text);
                //Console.WriteLine("\nlistAll --> [" + string.Join(", ", listAll) + "]\n");
                foreach (string item in listAll)
                {
                    if (item.Contains("or"))
                    {
                        listOfOR.AddRange(item.Split(" or "));
                        foreach (string val in listOfOR)
                        {
                            // OR condition
                            /* if (text.Contains(val))
                            {
                                valid = true;
                            } */

                            // AND condition
                            /* if (text.Contains(val))
                            {
                                valid = true;
                            } else 
                            {
                                valid = false;
                            }  */
                            //Console.WriteLine(val);
                        }
                        //listOfAND.Remove(item);
                        //listOfAND.Remove(item);
                        //Console.WriteLine("isValid --> " + valid);
                    }
                    //Console.WriteLine("\nlistOfOR --> [" + string.Join(", ", listOfOR) + "]\n");
                }

                // (desenvolvimento or aplicacao) and (teste or validacao or vacina)
                // busca primeira lista de or
                // busca segunda lista de or
                // compara o and

                /* if (listOfAND.Contains("or"))
                {
                    listOfAND.Remove
                } */


            }

            /* if (searchStringCleaned.Contains("or")) 
            {
                listOfOR = (searchStringCleaned.Split(" or ").ToList());
            } */
            // (palavraA AND palavraB) AND (palavraC AND palavraD) OR (palavraE AND palavraF)
            // {[and]["palavraA", "palavraB"], [and]["palavraC", "palavraD"], [and]["palavraE", "palavraF"]}
            // AND --> Contains ("palavraA") &&  Contains ("palavraB")
            // OR  --> Contains ("palavraA") ||  Contains ("palavraB")

            // lista de palavras
            // dicionário com a palavra + qtdDePalavras
            // 1

            // (banana AND maca AND pera) --> "banana, maca, pera"
            // (desenvolvimento OR teste) --> "desenvolvimento, teste"
            // (desenvolvimento AND teste) OR (banana AND maca AND pera) --> "desenvolvimento, teste", "banana, maca, pera"
            // (a OR b) AND (c OR d) --> "a, b" e "c"

            // lista de dicionarios {[and]["banana, maca, pera"], }
            // [and]["banana, maca, pera"]
            // [or]["desenvolvimento, teste"]

            //"{desenvolvimento, aplicacao, teste}"

            //Console.WriteLine("\n" + searchStringCleaned + "\n");

            //string[] splits = searchStringCleaned.Split(new string[] { " or ", " and " }, StringSplitOptions.TrimEntries);

            //Console.WriteLine("\n-------------------------------------------------------");
            Console.WriteLine("\nlistAll --> [" + string.Join(", ", listAll) + "]\n");
            Console.WriteLine("\nlistOfAND --> [" + string.Join(", ", listOfAND) + "]\n");
            Console.WriteLine("\nlistOfOR --> [" + string.Join(", ", listOfOR) + "]\n");

            // [stringBusca][stringType]
            // [key][value]

            // dicionario 
            // pos 1 --> ["desenvolvimento, teste"], ["and"]
            // pos 2 --> "banana, maca, pera", ["and"]

            // if contains pos1 || contains pos2


        }
        #endregion

        #region GenerateReport
        public static void GenerateReport(int contQuery, string filePath, string searchString, Dictionary<string, int> occurrences)
        {
            int fileNameIndex = filePath.LastIndexOf(@"\") + 1;

            string fileName = filePath.Substring(fileNameIndex, (filePath.Length - fileNameIndex));

            Console.WriteLine(fileName);


        }
        #endregion

        public static bool ValidateStringConditions(string searchStringCleaned)
        {
            bool areValidParentheses = false;
            int countOpenParentheses = 0;
            int countClosedParentheses = 0;

            if (searchStringCleaned[0] == '\"' && searchStringCleaned[searchStringCleaned.Length - 1] == '\"')
            {
                throw new InvalidOperationException("String de busca está toda entre aspas.");
            }

            if (searchStringCleaned.Length > 0)
            {
                // Count how many parentheses exist.
                countOpenParentheses = searchStringCleaned.Length - searchStringCleaned.Replace("(", "").Length;
                countClosedParentheses = searchStringCleaned.Length - searchStringCleaned.Replace(")", "").Length;
            }
            else
            {
                throw new InvalidOperationException("String de busca está vazia.");
            }

            if ((countOpenParentheses - countClosedParentheses) != 0)
            {
                throw new InvalidOperationException("String de busca apresenta parênteses irregulares.");
            }
            else
            {
                areValidParentheses = true;
            }

            return areValidParentheses;
        }
        #endregion

    }

}