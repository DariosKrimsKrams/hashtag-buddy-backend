## using EF .Net Core and using a Mysql DB

FYI Entity Framework 6.0 läuft nur unter .Net Framework 4.6 oder nutze EF Core mit .Net Core 1.x oder 2.x. Mit .Net Standard kann mit EF nicht nutzen. using following tutorial:
  * https://www.learnentityframeworkcore.com/walkthroughs/existing-database
  * https://docs.microsoft.com/de-de/ef/core/providers/
  * https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql

Des Weiteren (Stand Juli 2018) läuft Pomelo.entityFramworkCore.Mysql (getestet mit v2.0-rc2) nicht mit EntityFrameworkCore 2.1

Go to folder of the proj:

```
dotnet add package Microsoft.EntityFrameworkCore.Tools 
dotnet add package Pomelo.EntityFrameworkCore.MySql
```

add in .csproj file:
```
<ItemGroup>
<DotNetCliToolReference
    Include="Microsoft.EntityFrameworkCore.Tools.DotNet"
    Version="2.0.2" />
</ItemGroup>
```

to restore dependencies:
```	
dotnet restore
```

following file generates the database-first models:
```
dotnet ef dbcontext scaffold "Server=89.22.110.69;User Id=instaq_prod;Password=PASSWORD;Database=InstaqProd" "Pomelo.EntityFrameworkCore.MySql" -f
```

For .Net Core 3.0 install dotnet CLI:

```
dotnet tool install -g dotnet-ef
```

Next step is running in error mesage, which can be solved by adding desc. NuGet package.
```
Your startup project 'DFL.MeinLosTvModel' doesn't reference Microsoft.EntityFrameworkCore.Design.
This package is required for the Entity Framework Core Tools to work. Ensure your startup project is correct, install the package, and try again.
```
