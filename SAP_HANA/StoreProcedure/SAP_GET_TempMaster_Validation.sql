DROP PROCEDURE SAP_GET_TempMaster_Validation;

Create PROCEDURE SAP_GET_TempMaster_Validation 
(
 IN LocCode NVARCHAR(100)
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
-- read stock from the database
BEGIN


Select Count("DocEntry") As "Count","DocEntry" From "@SAP_OTMD" Where "U_LocCode"=:LocCode And "U_PDate" =Current_date
Group By "DocEntry";


END;


Call SAP_GET_TempMaster_Validation (20) ;