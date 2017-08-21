# apis

This repo includes one solution and 3 projects for WebAPI and the "Hello World" message.  
One project is the HelloWorldConsole which uses HttpClient to get the "Hello World" message
using the Web API GET request.  App.config for this project contains the deviceType that
the application shall write to, currently set to Console.

The second project is "WebAPI-1" with the HelloWorldController.cs containing the
implementation of the GET, GETbyId, POST, PUT, DELETE actions.  Although the only requirement can
satisfied with the GETbyId API.  Other implementations are towards addressing future needs.

The third project is the WebAPI-1.Tests containing VS UnitTests.  The GETbyId UnitTest
varifies the functionality of the requirement.