Alter PROCEDURE SAP_GET_TruckNo_InventryRequest 
(

IN TBranch NVARCHAR(50)
 
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
-- read stock from the database
BEGIN


Select Distinct T0."U_TRegNo",case when T5."U_Status" ='F' then 'Main Tank' else 'Hold' end
 From "@SAP_OTM"  T0 Inner Join "@SAP_TM2" T1 On  T0."DocEntry"=T1."DocEntry"
Inner Join OPRC T3 On "PrcCode"=T1."U_Route"
Inner Join OBPL T4 On T4."BPLName"=T3."U_Branch" 
inner join "@TRUCK_STATUS"  T5 on T5."Code" = T0."U_TRegNo" 
--inner join "OWHS" T6 on T6."WhsCode" = :FWHSCODE and
And T5."U_Status" in ('F')
Where T4."BPLId"=:TBranch ;



 

 
END;


CALL SAP_GET_TruckNo_InventryRequest('13');



