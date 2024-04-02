DROP PROCEDURE SAP_GET_Inventory_Transfer_Request_BasedValidation;

Create PROCEDURE SAP_GET_Inventory_Transfer_Request_BasedValidation 
(
 IN DocEntry NVARCHAR(100)
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
 FromBarch  NVARCHAR(50);
 TOBarch  NVARCHAR(50);
 FromWhs  NVARCHAR(50);
 TOWhs  NVARCHAR(50);
 FromLocation NVARCHAR(50);
 ToLocation NVARCHAR(50);

BEGIN


SELECT  Distinct  
T0."U_FB",T0."U_TB", T0."U_FW", T0."U_TW",
(Select "Location" From OWHS Where "WhsCode"=T0."U_FW") As "FromLocation", 
(Select "Location" From OWHS Where "WhsCode"=T0."U_TW") As "ToLcation"
INTO FromBarch,TOBarch,FromWhs,TOWhs,FromLocation,ToLocation
FROM "@SAP_OITR" T0  
Inner Join  OWHS T1  on (T0."U_FW"=T1."WhsCode" Or T0."U_TW"=T1."WhsCode"  )
Where T0."DocEntry"=:DocEntry;


IF  :FromLocation<>:ToLocation
Then 
 Select 1 As "Validation" From Dummy;
 Else
  Select 0 As "Validation" From Dummy;
End IF; 




 
END;


Call SAP_GET_Inventory_Transfer_Request_BasedValidation (40) ;