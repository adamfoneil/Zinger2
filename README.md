This is a [WPF reboot](https://github.com/adamfoneil/Zinger2/tree/master/Zinger.Wpf) of [Zinger](https://github.com/adamfoneil/Postulate.Zinger), with some other project types considered: a [command-line tool](https://github.com/adamfoneil/Zinger2/tree/master/Zinger.Tool), a [containerized Blazor app](https://github.com/adamfoneil/Zinger2/tree/master/Zinger.Blazor), and yet other stuff. The purpose is to be a lightweight [cross-platform](https://github.com/adamfoneil/Zinger2/blob/master/Zinger.Service/Models/Connection.cs#L3-L10) SQL IDE for generating C# result classes from queries. So for example if you write a query like this:

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
There are capable SQL IDEs out there such as [SSMS](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16), [DataGrip](https://www.jetbrains.com/datagrip/), and [MySql Workbench](https://www.mysql.com/products/workbench/). The one thing these clients don't do (as far as I know, maybe there are extensions out there) is [generate C# wrapper classes](https://github.com/adamfoneil/Zinger2/blob/master/Zinger.Service/Abstract/QueryProvider.cs#L75) around SQL results. This is really handy when writing queries for [Dapper](https://github.com/DapperLib/Dapper). To get the benefit of Dapper, you need C# result classes that map to your query columns. That's what Zinger does.

Zinger (the winforms version, anyway) also has a [schema browser](https://github.com/adamfoneil/Postulate.Zinger/blob/master/Zinger/Controls/SchemaBrowser.cs) feature that I like a bit better than SSMS. Although SSMS is more comprehensive, mine is more searchable and discoverable in the way it shows foreign key info. This is handy when learning an unfamiliar schema. IMO, SSMS is hard to search, explore and learn from. I'm not familiar with DataGrip, maybe it does this well. And I don't use MySql Workbench that much. The main reason I mention it is because Zinger is cross-platform (at least for [these platforms](https://github.com/adamfoneil/Zinger2/blob/master/Zinger.Service/Models/Connection.cs#L5-L12), though as of this writing these are all WIP).
