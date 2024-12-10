<h1>Movie Manager - Stackoverflow-ers</h1>

<p>Welcome to Movie Manager, a powerful application for managing movies, reviews, users, carts, and tickets. This project allows users to browse movies, purchase tickets, and leave reviews.</p>

<h2>Features</h2>

<ul>
  <li>Movie Management: Add, edit, and remove movies from the database.</li>
  <li>User System: Users can register, update profiles, and securely log in.</li>
  <li>Ticketing: Add tickets to your cart, remove them, and make payments.</li>
  <li>Review System: Users can leave reviews, add comments, and like/dislike them.</li>
  <li>Payment Processing: Secure payment system for ticket purchases.</li>
  <li>Cart Functionality: Users can add and remove items from their cart.</li>
</ul>

<h2>Technologies Used</h2>

<ul>
  <li>C# (Back-End Language)</li>
  <li>ASP.NET Core (Web API)</li>
  <li>Entity Framework Core (Database Management)</li>
  <li>SQL Server (Database)</li>
  <li>Swagger (API Documentation)</li>
  <li>Postman (API Testing)</li>
  <li>Azure Data Studio/SQL Server Management Studio (Database Development)</li>
</ul>

<h2>Installation and set up</h3>
<ol>
  <li><a href="https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16">Install SQL Server Managment Studio</a></li>
  <li><a href="https://nodejs.org/en">Install Node.js (npm version 10.8.2  and Node version v20.18.0 is known to work, other versions do not)</a></li>
  <li>Install Visual Studio and the ASAP.NET and .NET 8 Runtime (Long Term Support) components (must be .NET 8, not 9).</li>
  <li><a href="https://dotnet.microsoft.com/en-us/download">Install Dotnet</a></li>
  <li><a href="https://www.microsoft.com/en-us/sql-server/sql-server-downloads">Install SQL Server Express. In the installer, select the basic install. You can connect to the server in SQL Server Managment Studio by selecting the server in the "server name" dropdown, selecting "optional" for encryption, and checking the "trust server certificate" box.</a></li>
  <li>In SQL Server Managment Studio, right click on the databases folder icon and create a new database called "Movies".</li>
  <li><p>Right click on the Movies db icon and run the following query (don't need to add your password if you followed these directions). Add ";TrustServerCertificate=True" to the end of the result and save it as your connection string.</p>
   select
    'data source=' + @@servername +
    ';initial catalog=' + db_name() +
    case type_desc
        when 'WINDOWS_LOGIN' 
            then ';trusted_connection=true'
        else
            ';user id=' + suser_name() + ';password='
    end
    as ConnectionString
from sys.server_principals
where name = suser_name()</li> 
  <li>Clone the repo somewhere.</li>
  <li>Open MovieManager.sln in the root of the repo in Visual Studio. Click the prompt to install required components.</li>
  <li>Open MovieRepository.cs in MovieManager.Server/Repositories, find and replace "System.Environment.GetEnvironmentVariable("movieDb")" with "@"your connection string here"" (outer quotations in each is not included and actually add your connection string)</li>
  <li>dotnet tool install --global dotnet-ef</li>
  <li>dotnet add package Microsoft.EntityFrameworkCore.Design</li>
  <li>Inside MovieManager.Server, delete the .bin directory and run: dotnet add package Microsoft.EntityFrameworkCore.SqlServer</li>
  <li>Then run: dotnet ef database update</li>
  <li>You can then optionally undo the find and replace you did MovieRepository.cs. If you do revert it, make sure to set an enviornment variable containing your connection string called "movieDb" in your runtime configuration in VS.</li>
  <li><p>In SQL Server Mangment Studio in the Movies database, run the following query to create an Admin account (replace username and password as desired):</p><p>INSERT INTO Users (Username, Name, Password, Email, PermissionLevel, PhoneNumber, Preference, Gender, Age) VALUES ('username3', 'Bill', 'password', 'email@gmail.com', 1, '8652341111', 0, 1, 50);</p></li>
  <li>In visual studio, right click on moviemanager.client and click "Reload with dependencies" if it says application is not installed. Then build and run the project.</li>
  <li>Click trust/yes on all the certificate warnings.</li>
</ol>

<h2>Endpoint Changes</h2>
<p>In order to accommodate for changes made during development, some of the endpoints had to have slight changes made. They mostly consist of changes involving tickets (each of our ticket objects represents a showtime), using an object id in an endpoint instead of the object, and using DTOs because they are good practice with controllers. All of these are documented below, and all required endpoints are present.</p>
<ul>
  <li>ProcessPayment: This endpoint has had additional parameters for address added to match one of the shown use cases.</li>
  <li>AddTicketsToMovie: Our ticket object already contains the movie id and quantity, as well as additional information like the showing date. This means that the endpoint takes in a ticket object instead to ensure the correct information is included.</li>
  <li>RemoveTicketsFromMovie: Because our front end may not necessarily have access to the full movie object at all times, this endpoint uses movie id instead.</li>
  <li>EditTickets: This takes in a movie id for the same reason, as well as a ticket DTO to avoid unnecessary information.</li>
  <li>AddReview: Our review object already contains the movie, so it does not need that as a parameter.  In addition, it returns an integer id so the front end can track the new object.</li>
  <li>EditReview: This takes in a DTO containing the original id and the new object to avoid attaching unneeded user data.</li>
  <li>GetReviews: This takes in a movie id instead of an object for the reasons outlined above.</li>
  <li>EditMovie: Similar to edit review, it uses a movie DTO.</li>
</ul>

<h2>Developers</h2>

<ul>
  <li>Tessa Neal - Technical Design Analyst</li>
  <li>Jacob Schatzle - Developer Analyst</li>
  <li>Carolina Turner - Developer Analyst</li>
  <li>Dylan Johnson - Developer Analyst</li>
  <li>Caleb Henry - Test Analyst</li>
  <li>Kayly Tran - Scrum Analyst</li>
</ul>

