# Appointr

## Setup Instructions

### Prerequisites

Before you begin, ensure you have the following installed:

- [**.NET Core 8 SDK**](https://dotnet.microsoft.com/download/dotnet/8.0)
- [**Visual Studio 2022 or later**](https://visualstudio.microsoft.com/downloads/)
- [**SQL Server**](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) 


### Database Setup

- Ensure that SQL Server is running on your machine.
- Run the commands in the **NuGet Package Manager Console** on **Visual Studio**:

	```bash
	Add-Migration Init
	Update-Database
	```
