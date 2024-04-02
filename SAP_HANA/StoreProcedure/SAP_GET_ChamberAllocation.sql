



DROP PROCEDURE SAP_GET_ChamberAllocation;

Create PROCEDURE SAP_GET_ChamberAllocation 
(
 IN TruckNum NVARCHAR(100)
 
 
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS

Status NVARCHAR(10);
MaxDocNo NVARCHAR(50);
-- read stock from the database
BEGIN


SELECT "U_Status","U_MaxDocNo" INTO Status,MaxDocNo FROM "@TRUCK_STATUS"  T0 Where "Code"=:TruckNum ;

IF :Status='DT'  OR (:Status='F') Then 

Select T1."U_CAP" As "Qty",T1."U_OILDIP" As "Dip",T1."U_CHN"  As "Chamber" 
 From "@SAP_OCALP"  
T0 Inner Join "@SAP_CALP1"  T1 On T0."DocEntry"=T1."DocEntry" Where (T0."U_VC" =:TruckNum )
and T0."U_Status"= 'Active' and T0."U_CEDate" >=Current_date and T1."U_OILDIP">0;

End IF;

IF :Status='D' Then 
Select "U_Qty" As "Qty","U_Dip" As "Dip","U_Chamber" As "Chamber" From "@SAP_IT1" where "DocEntry"=:MaxDocNo;

End IF;



END;


CALL SAP_GET_ChamberAllocation ('0662NA5KHA');