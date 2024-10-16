# OneStreamAssessment
OneStreamAssessment using microservice Architecture

# Introduction 
Vendor-Hub project is a Vehicle Management System built using ASP.NET Core, Entity Framework Core, and Blazor using Clean Architecure in .Net 8. 
It provides functionality to manage vehicles, including creating, updating, deleting, and retrieving vehicle details, brands, and owners.
User needs to Login firt to mange and it currently supports to define differe Role typesS (Admin, BusineesUser, Supportuser). the initial version has two API Account and Vehicle

Features
User Authentication: Secure user login and role management using JWT token flow.
Vehicle Management: Add, update, delete, and retrieve vehicle details.
Vehicle Brand Management: Manage vehicle brands.
Vehicle Owner Management: Manage vehicle owners.
Presentiaton:
Web api: Followed RESTful API's that include Entity framework access and migrations
Responsive UI: A user-friendly interface built using Blazor,.Net8
Component Architecture: Followed Clean architecure,Design is flexible to enhance and scale up horizontally, 
Middleware & Configuration: Need to setup middleware ,depedency injections, configuring services, components with data-binding, routing between them.
Unit Testing: Written possible test cases to test web api's / components using xUnit.

# Getting Started
1. Prerquisite:
	Visaual studio 2022
	SqlExpress/Sql Server
		Create a datbase called  "StreamAssessment"
  EventBus connection string
  Topic: s1_int_event_bus 
		
3.	Installation process
	Step 1: Clone the repository:	
	Step2: Open the solution OneStream.Assessment in Visual Studio 2022, All 18 projects should get loaded.
   # OneStream.Assessment.sln
			BuildingBlocks ( All common modules/classes used across the solution)
				Identity.Security.csproj	 - for Authentication & Authorization
				HybridModelBinding.csproj - handling model binding from multiple sources
				Common.csproj - other common class, Exception,Http, Filter, Validations. Util etc.,
    
			#FEApiGateway
				Web.Bff.VendorConfiguration				
					Web.VendorConfiguration.HttpAggregator.csproj - BFF ( Backend for Front End project for VendorConfiguration API which are externally accessible with valid Authenticaion) 				
			
			Presentation
				VendorHub.WebUI - User Interface Blazor Project
				Application
				Domain
				Infrastructure		
			
			Services
				Vehicle -     Vehicle Micro Service ( Private APIs)
					Vehicle.API
					Vehicle.Application
					Vehicle.Domain
					Vehicle.Infrastructure
					Vehicle.Tests
					
				VendorConfiguration
					VendorConfiguration.Api    VendorConfiguration Micro Service (Private APIs)
					VendorConfiguration.Application
					VendorConfiguration.Domain
					VendorConfiguration.Infrastructure
					VendorConfiguration.Tests

  	
	Step3: Update the database connection string in appsettings.json fo API project located under Presentation folder to match your SQL Server configuration.
	
	Step4: Apply migrations to set up the database ( ensure you are selecting the Infrastructure project as Default project from package manager console drop down.
			execute "dotnet ef database update" from package manager console and wait for all the table to be crated. 

	Step5: Right click on the OneStream.Assessment.sln file and select "Configure Startup Projects". select "Multiple Startup Project" Radio buttion option from window and select Action as start for the following projects.        
      Web.VendorConfiguration.HttpAggregator
       Vehicle.API
       VendorConfiguration.Api
       VendorHub.WebUI
     
# Build and Test
step 1: Clean & Build the application from Visual Studio Command prompt or the Solution explorer. 
              dotnet clean
              dotnet clean

Three API projects ( Web.VendorConfiguration.HttpAggregator ( public),  Vehicle.API (Private),  VendorConfiguration.Api (Private)  and VehicleHub UI project loads on seperate browser windows).
First time users  Invoke Account/Setup API from initial admin account creation  Vehicle.API window which creates the admin user  admin@admin.com user with Admin@123 as password. 
you can login to the VendorHub UI applicaion using admin@admin.com/Admin@123
Admin users can peform following activities,
    Add User
    List Users
    List Countries

Home/Records is for both Admin, User roles which fetches the Vehicle, Vehicle Brand and Vehicle Owners details. 

Note :the authentication for BFFendorcofiguration and Core VendorConfiguration.Api, client needs to pass the JWTToken header with "C4CC0DE5-139E-4557-91F0-FAA7DEC2F7DB" as this will validate as token as the Ping/Auth service is fully functional. Happy to explain if you have any questions. 

# Contribute
please provide any suggnestions, recommendations and review comments to be fixed. Thank you.  

If you want to learn more about creating good readme files then refer the following [guidelines](https://docs.microsoft.com/en-us/azure/devops/repos/git/create-a-readme?view=azure-devops). You can also seek inspiration from the below readme files:
- [ASP.NET Core](https://github.com/aspnet/Home)
- [Visual Studio Code](https://github.com/Microsoft/vscode)
- [Chakra Core](https://github.com/Microsoft/ChakraCore)
