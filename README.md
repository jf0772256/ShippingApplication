# ShippingApplication
Capstone Project Redux

## Install Requrements AKA Dependencies:
We have written the application in such a way so as to allow the connection to connect to a MySQL database. In order to complete the connection an ODBC driver must be present on the compuer where the application is being installed to. This driver allows for the connection between the application and the MySQL database engine. We have included the required installers that will allow you to communicate with a MySQL database. The installers are for either 32bit or 64bit Windows os. these installer require administratior permission or password to install. You may not ever need to install these drivers if you only are using MSSQL database engines. If you are unsure which version of the MySQL ODBC driver to install we recommend that you have both the 32bit and 64bit drivers installed.
Each computer running the software if connecting to MySQL will require that the ODBC driver(s) be installed on them.

### .Net Framework used for this development is targeted at 4.6.1, if this framework is missing from the package you will be prompted to install it from Microsofts web site. Note though that you may need administrator privileges to install the software.

## Ensure that MSSQL ODBC Drivers are installed:
As a just in case we have included the installers for the latest MSSQL ODBC Driver installers for both 32bot and 64bit windows. Please make sure that you install both if they are not installed already.

## Ensure that your ODBC driver for MSSQL is installed on your computer:
It may be the case that you have not had the proper driver for MSSQL, this is unlikly, but in the event that it is the case please use the SQL Server (32/64bit) driver to allow connections to MSSQL based servers.

## Checking for drivers:
To check if you have the correct ODBC drivers installed on your system. 
* Open File Exlorer.
* Navigate to Control Panel (the start up is quick access, click the cheveron to select control panel)
* Either search for 'Administrative Tools' 
... or... 
* Select from the drop down in the upper right hand corner of the content window (defaults: Categories), select either large or small icons, select Administrative Tools.
* #### *****We will want you to check both 32bit and 64 bit ODBC Drivers listsings*****
* Double click on either the 32bit or 64bit ODBC Data Sources.
* Click on the 'Drivers' tab.
* 32bit will have more drivers than 64bit and may take a second to load all of them, scroll thtrough the list for the drivers wanted (bottom for MSSQL 'Sql Server' , about mid way or top for MySQL(if installed)).
