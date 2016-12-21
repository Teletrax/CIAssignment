# 4C Continuous Integration Assignment

### Create one or more powershell scripts that perform the following actions:

#### Phase 1

1. Install Git for Windows
1. Clone the https://github.com/Teletrax/CIAssignment.git repository into some folder
1. Install .NET Core SDK
1. Restore .NET Core Package
1. Build / Publish (Release Mode)
  1. FourC.Worker.Api
  1. FourC.Worker.Backend
1. Create ZIP archive with published artifacts (5.i, 5.ii)

#### Phase 2

1. Install MSDTC
1. Install MSMQ
1. Install IIS
1. Install ASP.NET Core
1. Install SQL Server 2014/2016 (Express)
1. Extract ZIP archive (From Phase 1 Step 5)
1. Run SQL script that will create the database and table structures. (Located inside the Scripts folder on repository)
1. Configure and host the REST endpoint (Web Application) on IIS
1. Configure and start (in background) the backend application
	
### Test

To test if the application is working properly you can use Postman and perform a test request to the REST API:

```
POST http://localhost/v1/worker

Content-Type: application/json

Body:

{
   "user":"Test",
   "content":"Test",
   "timestamp":"2016-12-16T02:00:00Z"
}
```

Connect to the SQL Server instance you created and perform a select on Work table, if this table has contents the application is successfully deployed.

### Assume

* We suggest you to install/use the Windows Server into a new VM Machine that you need to create using the evaluation version of Windows and can be directly downloaded at Microsoft website.
* The REST endpoint (Web Application/API) should be configured to run on "localhost" on port 80.
* You need to do some research and come up with a powershell script that connects to SQL Server and run the SQL Script we provided to setup the database table structure.
* You can send us an e-mail (fabricio.polonio@4cinsights.com, bulat.gafurov@4cinsights.com) if you have any question.

### Notes

* Given a clean Windows 2012R2/2016 Server machine, this powershell script should run and install/deploy all those components without errors.
* You can also suggest improvements on that build pipeline or point security issues.
* You can "Fork" this repository on Github and commit your solution into some folder, ex: "Solution".
