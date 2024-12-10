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
//TODO

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
