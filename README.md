# Busca por String em documentos

# Contribuidores
| <img src="https://avatars.githubusercontent.com/u/22084856?v=4" width=150px height=150px><br> [Paulo Backes](https://github.com/JrBackes)| <img src="https://avatars1.githubusercontent.com/u/43481916?s=400&u=2683d479631afcd710a45ec6cae3e82ba1a846bf&v=4" width=150px height=150px><br> [Vitor Matter](https://github.com/vmatter) |
|---|---|

# Dados da aplicação
Projeto tem como premissa, utilizar uma String de busca informada pelo usuário, para buscar as palavras em um arquivo em formato PDF.

# Regras do negócio
- As palavras que deverão ser localizadas no texto serão informadas em letras minúsculas.
- No caso de múltiplas palavras deverá ser informado o operador lógico entre elas. Os operadores lógicos aceitos serão **AND** e **OR**.
- Os operadores deverão ser escritos em letras maiúsculas, enquanto as palavras consultadas em letras minúsculas.
- A aplicação deverá retornar os seguintes resultados, com base na consulta realizada.
- Caso o operador OR seja utilizado, a aplicação deverá retornar o número de ocorrências de cada uma das palavras consultadas.
- Caso o operador AND seja utilizado, a aplicação deverá retornar se as duas palavras foram encontradas no texto, juntamente com o número de ocorrências de cada uma delas. 
É importante frisar que a as palavras poderão constar no documento em mais de um formato. Por exemplo, a palavra aplicação pode constar também como Aplicação, APLICAÇÃO, aplicacao, etc.

# Métodos

## Métodos usados internamente

Método utilizado para receber a pasta e arquivo PDF:

    string  TestSearchStrings(fileDirectory: string , fileName: string)
Método desenvolvido para ler o texto do PDF:
    
    string ReadTextInPdf(fileDirectory: string,fileName: string)
    
Método usado para fazer a normalização do texto:

    string NormalizeAndCleanText(textString: string)
    
Método responsável por fazer as validações da String:

    bool  ValidateStringExceptions(searchStringCleaned: string)
    
Tokenização da String

    List<string> TokenizeSearchString(searchStringCleaned: string)
    
Utlizado para separar a expressão e montar a String de busca em lista:

    List<List<string>> SeparateExpressions(searchStringTokens: List<string>)
   
Método para criação de um dicionário para armazenamento da String:
    
    Dictionary<string, int> FindExpressionsInPdf(searchStringTokens: List<List<string>>,filePath: string)
    
Fará a geração do relatório no arquivo txt: 

    void  GenerateReport(contQuery: int, filePath: string,  searchString: string,searchTokensdictionary: Dictionary<string, int>)
    
Imprime as saídas na tela

    void  PrintOutputs<T>(outputName: string,outputPrimitive: string,outputList: List<T>,outputListOfLists: List<List<T>>)
    
Reponsável pela verificação da expressão:

    List<Tuple<string, string>> VerifyExpressions(searchStringTokens: List<List<string>>,   normalizedText: string)
    
Utilizado para validar a expressão:
    
    bool  ValidateExpression(expression: string,normalizedText: string,isAnd: bool,isOr: bool)
    
Utilizado para contar as palavras:

    Dictionary<string, int> CountWords()

   ## Método usado em tempo de execução
Utilizado para montar o menu:
    void  ShowMenu()
    
    Opções do Menu:

	Opção 1: Manual Search -> Utilizada para digitar uma string no software.
	Opção 3: Search using TXT. File -> Utilizada para referenciar o local de um arquivo TXT com as strings de busca.
	Opção 5: Exit -> Utilizado para encerrar o programa.


**Opção 1**:

 - Solicita a String de busca;
 - Executa a validação do diretório;
 - Executa a validação e verifica se o arquivo PDF informado existe;
 - Gera o relatório;
 - Apresenta para o usuário o resultado da busca;

**Opção 3** :

 - Executa a validação do diretório do TXT de busca;
 - Executa a validação e verifica se o arquivo TXT informado existe;
 - Executa a validação do diretório;
 - Executa a validação e verifica se o arquivo PDF informado existe;
 - Gera o relatório;
 - Apresenta para o usuário o resultado da busca;

**Opção 5** :

 - Finaliza a execução do programa;

