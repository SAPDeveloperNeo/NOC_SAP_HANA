



DROP PROCEDURE SAP_GET_CalibrationCount;

Create PROCEDURE SAP_GET_CalibrationCount 
(
 IN TruckNum NVARCHAR(100)
 
 
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
-- read stock from the database
BEGIN

Select T1."U_CHN",T1."U_OILDIP",T1."U_CAP",
IFNULL(T1."U_CAP",1) As "QtyValu"

 From "@SAP_OCALP"  
T0 Inner Join "@SAP_CALP1"  T1 On T0."DocEntry"=T1."DocEntry" Where (T0."U_VC" =:TruckNum )
and T0."U_Status"= 'Active' and T0."U_CEDate" >=Current_date and T1."U_OILDIP">0;


END;


--Call SAP_GET_CalibrationCount ('0853NA5KHA') ;

CALL SAP_GET_CalibrationCount ('2707LU2KHA');