inside `bll/`:  
`dotnet ef migrations -s ../api/ add -o Data/Migrations NAME`  
`dotnet ef database update -s ../api`