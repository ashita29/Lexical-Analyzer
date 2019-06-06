The main objective of this Remote Package Dependency Analysis is to build an automated scanner that will do code analysis for Code Repositories.
In building this application, C# language with .NET framework version 4.6.1 and Visual Studio 2017 is used.
This system also contains the the usage of Windows Communication Framework (WCF) for establishing communication between the Server (code analyzer functionality), Client (user) and remote Repository and Test Stub.
This application also implements the Windows Presentation Foundation (WPF) which will provide GUI to the client for connecting a channel to Server.</br>
As this project is build in modules, so the first module i.e. Lexical Scanner, which used Tokenizer and SemiExpression is based on State-Pattern, in simple terms a state pattern changes class behavior based on its state. State patterns allows us to create objects representing the different states and one context object whose behavior will vary as the state object will change.
The second module called 'Type-Based Package Dependency Analysis' which has complete functionality to create type-tables, dependency table to detect file dependency and implementing the Tarjan’s algorithm to detect files which form strong components.
In the last module we have provided the GUI to the client and establish the communication channel between the key modules of the application.

The below screenshot is the GUI of the application.

![](/Images/GUI.png)
