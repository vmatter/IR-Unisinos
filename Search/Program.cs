using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SearchStringHandler
{
    public class Program
    {
        public static string searchStringInput = "";
        public static void Main(string[] args)
        {
            //TODO: Implementar metodo de leitura.
            //string searchStringInput = SearchStringUtils.InputSearchString();

            // Handle the file.
            string filePath = @"E:\vitor_desktop\iCybersec\C#\Trabalho I\Busca-por-strings-em-documentos\testes\Enunciado - Projeto 1.pdf";
            int count = 0;
            int maxTries = 3;

            //? Testar strings
            //string text = "Em um mundo onde a informação tornou-se um dos recursos abundantes mais relevantes para a sociedade, é imprescindível que além da extração segura dos dados, realizar uma classificação significativa dos dados adquiridos também deve ser possível, visto que estes podem conter informações sensíveis de entidades. Uma das formas mais utilizadas de extração de informação é através de textos, portanto técnicas de Processamento de Linguagem Natural (PLN) vêm sendo vastamente exploradas. Levando isso em consideração, o objetivo deste trabalho foi encontrar arquiteturas sistêmicas capazes de aplicar classificação em textos e extrair com sucesso informações relevantes. Uma revisão sistemática da literatura (RSL) foi conduzida para analisar artigos acadêmicos publicados de 2010 até o início de janeiro de 2021. O processo de triagem resultou em uma população final de 21 estudos de um total de 234 analisados. A filtragem inclui a remoção de artigos não relacionados a uma classificação de texto ou arquitetura sistêmica de classificação de informações. Neste artigo, propostas e resultados que contribuem para os desafios de classificação de texto são apresentados considerando quatro questões de pesquisa. A conclusão do estudo atestou que não existe uma arquitetura sistêmica ou algoritmo de classificação específico capaz de ser considerado o estado da arte no campo da classificação de texto.";
            //string searchStringInput = "@@#((\"Classificação''' :::;';'';'~~de ;'';'~~Texto\"!@#$%$%@ OR##@! !@#!@#Classificação ;'';'~~de ;'';'~~Informação);'';'~~ AND PLN)";
            //string searchStringInput = "(texto and info) or (a and b)";
            string searchStringInput = "(texto and info) or (a and b)";
            //string searchStringInput = "desenvolvimento AND aplicação";
            //string searchStringInput = "(\"texto info\" banana and opcao)";
            //string searchStringInput = "\"(\"texto info\" banana and opcao)\"";
            //string searchStringInput = "\"teste\"";

            SearchStringUtils.PrintOutputs<string>(outputName: "searchStringInput", outputPrimitive: searchStringInput);

            //? Testar com o While depois.
            /* while (true)
            { */
            //Console.WriteLine("Digite sua String de Busca");
            //searchStringInput = Console.ReadLine();

            string cleanedSearchStrings = SearchStringUtils.NormalizeAndCleanText(searchStringInput);
            SearchStringUtils.PrintOutputs<string>(outputName: "normalizedSearchString", outputPrimitive: cleanedSearchStrings);

            // While
            try
            {
                SearchStringUtils.ValidateStringExceptions(cleanedSearchStrings);
            }
            catch (System.Exception exception)
            {
                Console.Error.WriteLine("\n" + exception.ToString());
                if (++count == maxTries) throw exception;
            }

            List<string> tokenizedSearchStrings = SearchStringUtils.TokenizeSearchString(cleanedSearchStrings);
            SearchStringUtils.PrintOutputs(outputName: "tokenizedSearchStrings", outputList: tokenizedSearchStrings);

            List<List<string>> separatedExpressions = SearchStringUtils.SeparateExpressions(tokenizedSearchStrings);
            SearchStringUtils.PrintOutputs(outputName: "separatedExpressions", outputListOfLists: separatedExpressions);

            Dictionary<string, int> searchTokensdictionary = SearchStringUtils.FindExpressionsInPdf(separatedExpressions, filePath);

            //SearchStringUtils.GenerateReport(++contQuery, filePath, searchStringInput, searchTokensdictionary);
            /* } */

            // End While

            // }

        }
    }
}