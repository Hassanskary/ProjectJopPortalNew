# Description
The Job Portal is a web application designed to connect companies and job seekers. Companies can create accounts, post job listings, and manage applications. Job seekers can create profiles, upload their resumes, search for jobs, and apply to the positions that interest them. The platform ensures secure and role-based access, so companies and clients only access their relevant information.

## How to run project
- Ensure you have .NET 7.0 installed on your machine.
- Install Microsoft.EntityFrameworkCore version 7
- Install Microsoft.EntityFrameworkCore.Tools version 7
- Install Microsoft.EntityFrameworkCore.Sqlserver version 7
- Install Microsoft.EntityFrameworkCore.Design version 7
- Change ConnectionStrings in appsettings.json to your Database ConnectionStrings
- in Package Manager Console running the following commands "Add-Migration (Migration Name) " and "Update-Database"
- run project 
##  Features

 Company Functionality
1. Account Management:
   - Create an account and upload a profile picture.
   - Log in to an existing account.
   - Edit company details from the profile page.
   - Delete the company account.
  
2. Job Listings:
   - Post available job listings.
   - Edit and delete job listings.

3. Application Management:
   - View applications for job listings.
   - See applicant information including name and CV.
   - Accept or reject job applications.

 Client Functionality
1. Account Management:
   - Create an account, upload a profile picture, and upload a CV.
   - Log in to an existing account.
   - Edit personal details from the profile page.
   - Delete the client account.

2. Job Search and Application:
   - Explore all available job listings on the site.
   - Search for specific jobs using various criteria.
   - Apply for jobs.
   - View the status of job applications (accepted or rejected).

 Authorization
- Role-Based Access Control:
  - Clients cannot access company-specific pages.
  - Companies cannot access client-specific pages.



## Frontend technologies:
- HTML
- CSS
- JavaScript

## Backend technologies:
- C#
- ASP.NET
- Sql Server

## Database Design
![Capture](https://github.com/AZIZ20035/Job-Portal/assets/91346703/76599c4e-def9-47c9-b686-b58c4a1d0104)



## Demo Video
https://drive.google.com/file/d/1uEebGHB2QjpeIREI7JQLceiL_BQug-29/view
