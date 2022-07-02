This is a WPF reboot of [Zinger](https://github.com/adamfoneil/Postulate.Zinger). There are capable SQL IDEs out there such as [SSMS](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16), [DataGrip](https://www.jetbrains.com/datagrip/), and [MySql Workbench](https://www.mysql.com/products/workbench/). The one thing these clients don't do (as far as I know, maybe there are extensions out there) is [generate C# wrapper classes](https://github.com/adamfoneil/Zinger2/blob/master/Zinger2.Service/Abstract/QueryProvider.cs#L75) around SQL results. This is really handy when writing queries for [Dapper](https://github.com/DapperLib/Dapper). To get the benefit of Dapper, you need C# result classes that map to your query columns. That's what Zinger does.

Zinger also has a [schema browser](https://github.com/adamfoneil/Postulate.Zinger/blob/master/Zinger/Controls/SchemaBrowser.cs) feature that I like a bit better than SSMS. Although SSMS is more comprehensive, mine is more searchable and discoverable in the way it shows foreign key info. This is handy when learning an unfamiliar schema. IMO, SSMS is hard to search, explore and learn from. I'm not familiar with DataGrip, maybe it does this well. And I don't use MySql Workbench that much. The main reason I mention it is because Zinger is cross-platform (at least for [these platforms](https://github.com/adamfoneil/Zinger2/blob/master/Zinger2.Service/Models/Connection.cs#L3-L9)).

This is the WinForms UI in action:

![image](https://user-images.githubusercontent.com/4549398/177013846-915431de-8c40-4831-9e37-ef74f22a14ad.png)

The gist of it is you write ordinary SQL queries, run them to preview the results, and then get the C# output to use when writing Dapper code in C#.

This is what I'd like to implement in WPF, but with some improvements. Accordingly, the UI above won't be a perfect match with the WPF version:
- Handle multiple results from a batch or stored procedure. The legacy project supports only one result per query.
- Support table UDT parameters


