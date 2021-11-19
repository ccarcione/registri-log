# Registri Log 

Small Angular/.Net Core library that offers functions of:

- EF Core Audit
- API Request/Response logging
- _Serilog_ additional functions
- Angular GUI of all functions

**If the documentation or the project should present problems you can open an issue indicating the problem:)**

Link:

- [Development Repository GitLab - registri-log (recommended)](https://gitlab.com/projects-experimenta/registri-log)
- [Open Issue on GitLab (recommended)](https://gitlab.com/projects-experimenta/registri-log/-/issues/new)
- [Mirror Repository GitHub](https://github.com/ccarcione/registri-log)
- [Open Issue on GitHub](https://github.com/ccarcione/registri-log/issues/new)

## How To: install and configure

### MS .Net Core

- Install the **net-logs-log** package from nuget.org --> [Link](https://www.nuget.org/packages/net-registri-log/)

  ```c#
  dotnet add package net-registri-log
  ```
  
  
  
- Configure the library options in the **appsettings. {Env} .json** file:

  ```json
    "net-registri-log": {
      "ApiLog.Options": {
        "Enable": true,
        "TrackRequestBody": true,
        "TrackResponseBody": true,
        "IgnorePath": [
          "/ApiLog/"
        ]
      },
      "Logs.Options": {
        "Enable": true,
        "SerilogDebuggingSelfLogEnable": true,
        "SerilogSelfFileName": "SerilogSelf.log"
      }
    },
  
  ```

  

- Add to the **ConfigureServices** method:

  ```c#
  public void ConfigureServices(IServiceCollection services)
  {
      // ...
      // ...
      
      services.AddNetRegistriLog(Configuration);
  
      // ricordati di configurare il tuo DbContext!!!
      services.AddDbContext<ApplicationDbContext>(options =>
                                                  {                                              options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                                                  });
      // ...
      // ...
  }
  ```

- Add to the **Configure** method:

  ```c#
  public void ConfigureServices(IServiceCollection services)
  {
      app.UseNetRegistriLog();
      
      // ...
      // ...
  }
  ```

- Update the **Program** class:
  
  ```c#
      public class Program
      {
          public static void Main(string[] args)
          {
              // required to configure the Serilog
              net_registri_log.Logs.Logger.Initialize();
  
              try
              {
                  Log.Information("Starting host...");
                  CreateHostBuilder(args).Build().Run();
              }
              catch (Exception ex)
              {
                  Log.Fatal(ex, "Host terminated unexpectedly.");
              }
              finally
              {
                  Log.CloseAndFlush();
              }
          }
  
          public static IHostBuilder CreateHostBuilder(string[] args) =>
              Host.CreateDefaultBuilder(args)
                  .UseSerilog()	// needed for Serilog
                  .ConfigureWebHostDefaults(webBuilder =>
                  {
                      webBuilder.UseStartup<Startup>();
                  });
      }
  
  ```
  
- Add the _Serilog_ configuration to the **appsettings.{Env}.json** file. The library uses a precise configuration.
  Change or extend the example:
  
  ```json
    "Serilog": {
      "Using": [ "Serilog.Sinks.Console", "Serilog.Sinks.File", "Serilog.Sinks.MSSqlServer", "Serilog.Expressions" ],
      "MinimumLevel": {
        "Default": "Debug",
        // the namespaces of the Override section ignore the Default MinimumLevel.
        "Override": {
          "Microsoft": "Warning",
          "Microsoft.Hosting.Lifetime": "Information",
          "System": "Warning",
          "Microsoft.EntityFrameworkCore.Database.Command": "Warning",
          "Microsoft.AspNetCore.Authentication": "Debug",
          "net-registri-log": "Debug"
        }
      },
      "WriteTo": [
        {
          "Name": "Logger",
          "Args": {
            "configureLogger": {
              // all logging service providers (sinks) are defined here.
              "WriteTo": [
                // log settings to console.
                // https://github.com/serilog/serilog-sinks-console
                {
                  "Name": "Console",
                  "Args": {
                    "outputTemplate": "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{Data:l}{NewLine}{Exception}{NewLine}",
                    "theme": "Serilog.Sinks.SystemConsole.Themes.AnsiConsoleTheme::Code, Serilog.Sinks.Console"
                  }
                },
                // log settings to file.
                // https://github.com/serilog/serilog-sinks-file
                {
                  // name of the sink type.
                  "Name": "File",
                  "Args": {
                    // directory/path where the logs will be written.
                    "path": "../LogFiles/net-registri-log.webapi/net-registri-log.log",
                    // maximum size of single log file.
                    "fileSizeLimitBytes": 1000000,
                    // (setting 3, for more info see Serilog documentation)
                    "rollingInterval": 3,
                    "rollOnFileSizeLimit": true,
                    "shared": true,
                    "outputTemplate": "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{Data:l}{NewLine}{Exception}{NewLine}",
                    // disk write interval
                    "flushToDiskInterval": 1
                  }
                },
                // log settings on sql server.
                // https://github.com/serilog/serilog-sinks-mssqlserver
                {
                  // nome del tipo di sink.
                  "Name": "MSSqlServer",
                  "Args": {
                    "connectionString": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=net-registri-log;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False",
                    // table on which the logger will write.
                    // IT CAN BE CHANGED AS NEEDED
                    "sinkOptionsSection": {
                      "tableName": "Logs",
                      "schemaName": "dbo",
                      // if true Serilog will create the necessary tables and fields.
                      "autoCreateSqlTable": true
                    },
                    "columnOptionsSection": {
                      // some unnecessary standard columns of the logging service have been removed.
                      // IT CAN BE CHANGED AS NEEDED
                      "removeStandardColumns": [ "MessageTemplate", "Properties" ],
                      // HERE ARE DEFINED THE CUSTOMIZED FIELDS OF THE LIBRARY.
                      // !!! YOU CAN ADD THE OTHER FIELDS BUT DO NOT REMOVE ANY CONFIGURATION !!!
                      "customColumns": [
                        {
                          "ColumnName": "UserName",
                          "DataType": "nvarchar",
                          "DataLength": 50,
                          "AllowNull": true
                        },
                        {
                          "ColumnName": "Operation",
                          "DataType": "nvarchar",
                          "DataLength": 50,
                          "AllowNull": true
                        },
                        {
                          "ColumnName": "JsonObject",
                          "DataType": "nvarchar",
                          "AllowNull": true
                        }
                      ],
                      "timeStamp": {
                        "columnName": "Timestamp",
                        "convertToUtc": false
                      }
                    }
                  }
                }
              ]
            }
          }
        }
      ],
      "Enrich": [ "FromLogContext" ]
    }
  
  ```
  
  
  
- The necessary library migrations are automatically applied when the project is first started.
  For more information check out the static `UpdateDatabaseMigrate` method in the ` net_registri_log.Providers.ApplicationBuilderExtensions` class.



**NB**: from now on, to update the context of your project it will be necessary to specify:

```c#
Add-Migration <nome> -Context <context>
```



#### API-Log

The function will be active from the first start. The information is contained in the _dbo.ApiLogs_ table of the configured database.



#### Audit-Log

To use this feature you need to use a workaround:

- Use the DI to get the **RegistriLogDbContext** context.

- Instead of using the **SaveChanges  SaveChangesAsync** of your context use the context of the library by passing it the context you were working on:

  ```c#
  await _registriLogDbContext.SaveAndAuditChangesAsync(_context);
  ```

-  The information is contained in the _dbo.AuditLogs_ table of the configured database.



#### Logs

The library extends some functionality of the _Serilog_ library:

- API for consulting logs
- extension methods that handle two new data: _jsonObject, operation_

**NB**: Serilog must be configured (see above).



### Angular Project

- Install the package via npm --> [Link](https://www.npmjs.com/package/ngx-registri-log)

  ```powershell
  npm i ngx-registri-log
  ```

- Add the module to the project in **app.module.ts**.

  ```typescript
  import { NgxRegistriLogModule } from 'ngx-registri-log';
  
  @NgModule({
    // ...
    // ...
    
    imports: [
      NgxRegistriLogModule
    ],
    
    // ...
    // ...
  })
  export class AppModule { }
  ```

- The library exposes the components:

  ```html
  <lib-audit-log></lib-audit-log>
  <lib-event-api></lib-event-api>
  <lib-logs></lib-logs>
  ```



## Sample and Development Projects

The following projects have been developed in the repository:

- node\ --> workspace Angular
   - node\projects\ngx-logs-logs --> library project
   - node\projects\client --> library test client
- net\ --> .Net Core solution
   - net\net-registri-logs --> library project
   - net\net-registri-log.webapi --> test API project (for tests I recommend using swagger UI)
- sample\ --> example .Net + Angular stack with libraries configured by _NPM_ and _NuGet_.

