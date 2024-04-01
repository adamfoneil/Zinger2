This is a [WPF reboot](https://github.com/adamfoneil/Zinger2/tree/master/Zinger.Wpf) of [Zinger](https://github.com/adamfoneil/Postulate.Zinger), with some other project types considered: a [command-line tool](https://github.com/adamfoneil/Zinger2/tree/master/Zinger.Tool), a [containerized Blazor app](https://github.com/adamfoneil/Zinger2/tree/master/Zinger.Blazor), and yet other stuff. The purpose is to be a lightweight [cross-platform](https://github.com/adamfoneil/Zinger2/blob/master/Zinger.Service/Models/Connection.cs#L3-L10) SQL IDE for generating C# result classes from queries. So for example if you write a query like this -- assuming it's a valid query in a valid connection:

```sql
SELECT * FROM [Whatever];
```
You'll get a C# class that maps to the columns of `[Whatever]` -- along with the results. For example:

![image](https://github.com/adamfoneil/Zinger2/assets/4549398/e3f64fa0-5b7e-4554-8dfd-80c7c4f29cd5)

This lets you write Dapper code like this:

```csharp
var results = await cn.QueryAsync<Whatever>("SELECT * FROM [Whatever]");
```

You now have type-safe access to `Whatever` using Dapper, without creating the `Whatever` class by hand.

# Why
There are capable SQL IDEs out there such as [SSMS](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16), [DataGrip](https://www.jetbrains.com/datagrip/), and [MySql Workbench](https://www.mysql.com/products/workbench/). The one thing these clients don't do (as far as I know, maybe there are extensions out there) is [generate C# wrapper classes](https://github.com/adamfoneil/Zinger2/blob/master/Zinger.Service/Abstract/QueryProvider.cs#L75) around SQL results. This is really handy when writing queries for [Dapper](https://github.com/DapperLib/Dapper). To get the benefit of Dapper, you need C# result classes that map to your query columns -- assuming you don't want to use `dynamic`. That's what Zinger does -- generate those result classes from queries.
