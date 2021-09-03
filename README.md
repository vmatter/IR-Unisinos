# Search in documents by Strings

# Contributors
| <img src="https://avatars.githubusercontent.com/u/22084856?v=4" width=150px height=150px><br> [Paulo Backes](https://github.com/JrBackes)| <img src="https://avatars1.githubusercontent.com/u/43481916?s=400&u=2683d479631afcd710a45ec6cae3e82ba1a846bf&v=4" width=150px height=150px><br> [Vitor Matter](https://github.com/vmatter) |
|---|---|

# Application data
The project's premise is to use a search String informed by the user, to find the words in a file in PDF format.

# Business rules
- The words that are located in the text will be informed in lowercase.
- In the case of multiple words, the logical operator between them must be informed. The accepted logical operators will be **AND** and **OR**.
- Operators must be written in capital letters, while the words consulted in small letters.

The application should return the following results, based on the query performed.
- If OR operator is used, the application must return the number of occurrences of each of the queried words.
- If AND operator is used, the application must return if the two words were found in the text, together with the number of occurrences of each one of them.
It is important to note that the words may appear in the document in more than one format. For example, the word aplicação can also appear as Aplicação, APLICAÇÃO, aplicacao, etc. 

# Methods

## Methods used for validation

Method used to receive the PDF folder and file: 

    string  TestSearchStrings(fileDirectory: string , fileName: string)
Method developed to read the PDF text: 
    
    string ReadTextInPdf(fileDirectory: string,fileName: string)
    
Method used to normalize text: 

    string NormalizeAndCleanText(textString: string)
    
Method responsible for performing String validations: 

    bool  ValidateStringExceptions(searchStringCleaned: string)
    
String Tokenization:

    List<string> TokenizeSearchString(searchStringCleaned: string)
    
Used to separate the expression and assemble the search string into a list: 

    List<List<string>> SeparateExpressions(searchStringTokens: List<string>)
   
Method for creating a dictionary to store the String: 
    
    Dictionary<string, int> FindExpressionsInPdf(searchStringTokens: List<List<string>>,filePath: string)
    
It will generate the report in the txt file: 

    void  GenerateReport(contQuery: int, filePath: string,  searchString: string,searchTokensdictionary: Dictionary<string, int>)
    
Show outputs to screen:

    void  PrintOutputs<T>(outputName: string,outputPrimitive: string,outputList: List<T>,outputListOfLists: List<List<T>>)
    
This one verify the expression:

    List<Tuple<string, string>> VerifyExpressions(searchStringTokens: List<List<string>>,   normalizedText: string)
    
Validate the expression:
    
    bool  ValidateExpression(expression: string,normalizedText: string,isAnd: bool,isOr: bool)
    
Counting words:

    Dictionary<string, int> CountWords()

   ## Runtime Method
Used to build the menu: 
    void  ShowMenu()
    
    Menu Options:

	Option 1: Manual Search -> Used to type a string into the software .
	Option 3: Search using TXT. File -> Used to reference the location of a TXT file with search strings.
	Option 5: Exit -> Used to terminate the program. 


**Option 1**:

 - Request the search string;;
 - Perform directory validation;
 - Performs validation and checks if the PDF file entered exists;
 - Generates the report;
 - Shows the search result to the user; 

**Option 3** :

 - Performs the validation of the search TXT directory;
 - Executes the validation if the TXT file informed exists;
 - Perform directory validation;
 - Performs validation and checks if the PDF file entered exists;
 - Generates a report;
 - Print the search result to the user; 

**Option 5** :

 - Ends program execution; 

# Próximos passos

 - TO DO: Implement the option '2' of the menu that will load the search strings from a .txt file (TestSearchStrings function).
 - TO DO: Add better commentaries to ensure the documentation quality.
 - TO DO: Implement a Split function that keeps the separator. 
 - TO DO: Try to SeparateExpressions without using Regex.
 - TO DO: Verify if C# has an implementation of FileSeparator like Java.
 - TO DO: Change the dictionary to an OrderedDictionary.
 - TO DO: Rework the VerifyExpressions using a BinaryTree.
 - TO DO: Review all the code after Marcio`s reivions.
 - TO DO: Validate if all files and directories exists else create them.

