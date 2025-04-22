# Boilerplate
This project Boilerplate .Net

# How to use
1. Migrate docker-compose database postgres with `docker compose up` or `docker compose up -d`
2. Generate Model Table With command `dotnet ef migrations add Init`
3. Migrate All table Using `dotnet ef database update`
4. Run .Net Project Using Command `dotnet run`
5. Create Request Using Swagger with host `http://localhost:5242/swagger/`