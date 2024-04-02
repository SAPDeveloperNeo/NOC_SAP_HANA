DROP PROCEDURE SAP_GET_Condition_Inventory_Transction;

Create PROCEDURE SAP_GET_Condition_Inventory_Transction 
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
FROM "@SAP_OITR" T0  
Inner Join  OWHS T1  on (T0."U_FW"=T1."WhsCode" Or T0."U_TW"=T1."WhsCode")
Where T0."DocEntry"=(Select "U_ITRDocE" From "@SAP_OIT" Where "DocEntry"=:DocEntry);

 
END;


Call SAP_GET_Condition_Inventory_Transction (1) ;