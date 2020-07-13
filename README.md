# WBank Web App
Fullstack banking web app implemented with .NET Core and React.
Currently the app simulates  the deposit, withdraw and payment functionalities of a bank account and rentabilizes the balance on a daily basis. 

## How to run
### First time setup
-Create a "banking" database on your MySql instance\
-Git clone the repository\
-Acess the Banking.API/ folder\
-Open appsettings.json and configure the connection string (set Server, Uid and Pwd for your DB instance)\
-Run migrations\
-Acess the front/ folder\
-Install the required packages with npm install

### Startup
-Acess the Banking.API/ folder\
-Run application with dotnet run\
&nbsp;&nbsp;&nbsp;-API will be acessible at http://localhost:5000/api/... \
&nbsp;&nbsp;&nbsp;-Swagger can be acessed at http://localhost:5000/swagger (email "admin@gmail.com" and password "admin" can be used for authentication)\
-Acess the Banking.API/ folder\
-Run frontend application with yarn start\
 -frontend will be acessible at http://localhost:3000/

## Frontend
Developed using React with Material-UI and Recharts components.\
Includes an account details page, rentability history and operation commands.\
The default (hardcoded login) user's credentials are email = "diogo@gmail.com" and password = "diogo" (necessary for deposits, withdrawals and payments).
#### Account details:
![Image of account](https://user-images.githubusercontent.com/51866503/87248647-05ebc300-c431-11ea-910e-0c8ffccfa427.PNG)
#### Rentability history:
![Image of history](https://user-images.githubusercontent.com/51866503/87248654-0dab6780-c431-11ea-8752-8d3306abf983.PNG)
#### Commands:
![Image of command](https://user-images.githubusercontent.com/51866503/87248657-113eee80-c431-11ea-91cb-4f92a8a529f2.PNG)

## Backend and API
![Image of architecture](https://user-images.githubusercontent.com/51866503/87249343-f53d4c00-c434-11ea-8148-aa8a27ce055b.PNG)  
The backend architecture is based on the Domain Driven Design concept and implemented with C#/.NET Core, MySQL and the EF Core ORM, with code-first and migrations.\
![Image of Swagger](https://user-images.githubusercontent.com/51866503/87248595-a42b5900-c430-11ea-9dd9-45e66a0bf8f7.PNG)

## Coming soon
-Account and user creation\
-Definitive UI layout and design and general improvements\
-Transference UI (backend already functional)\
-Tax (IOF and IR) rules on remuneration service\
-Login page\
-Password cryptography\
-Repository pattern\
-Background service tests\
-Frontend tests
