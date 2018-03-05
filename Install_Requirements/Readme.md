# Installation Requirements:

In this folder are two installers, one for 32bit os and one for 64bit os. They require Admin access to install and won't be required
unless the sql server is pointed to a MySQL database.

## To install the ODBC Drivers:

simply double click on the driver install for the version of windows that this system is installed on, and follow the prompts.
AGAIN Admin privlages will be required to complete the install. Please plan ahead for IT delays.

When the install is completed you wont have to redo the install until a new computer is set up OR if something happens to one computer.
If using SQL Server by Microsoft you will not need to install the MYSQL ODBC driver.


### Last point note::
If you are unsure about wich driver to use, install both drivers and let windows decide which is best.
All computers using the mysql database will have to have either one or both of the drivers to work. 
Considering that both MSSQL and MySQL are very prominent sql engines across the internet for data base stores, only MSSQL and MySQL will function with the application. Note that use of some version sof MySQL and date/time functions require that you pass a value for dates, times, and timestamps; some of these include MariaDB 5.5.6, 10.1.0 and greater.
Do not attempt to use other MySQL drivers than 5.2 unless you update the code as well. the connection string had to be specific with the driver fversion to use.
