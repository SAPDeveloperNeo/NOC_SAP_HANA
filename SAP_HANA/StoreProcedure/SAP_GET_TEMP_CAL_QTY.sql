
DROP PROCEDURE SAP_GET_TEMP_CAL_QTY;

Create PROCEDURE SAP_GET_TEMP_CAL_QTY 
(
 IN WhsCode NVARCHAR(100),
 IN Height DECIMAL
 
 
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS

 Max1  DECIMAL;
 Min1  DECIMAL;
 
 MaxQty  DECIMAL;
 MinQty  DECIMAL;
 CalQty DECIMAL;
 
BEGIN






IF (:Height=(Select IFNULL(T1."U_Hight",0)   From "@SAP_OCALP"  
T0 Inner Join "@SAP_CALP2"  T1 On T0."DocEntry"=T1."DocEntry" Where T0."U_FAC"=:WhsCode
And "U_Hight"=:Height) ) THEN
Select T1."U_Qty" As "Qty"  From "@SAP_OCALP"  
T0 Inner Join "@SAP_CALP2"  T1 On T0."DocEntry"=T1."DocEntry" Where T0."U_FAC"=:WhsCode
And "U_Hight"=:Height;
Else 

Select Max(IFNULL(T1."U_Hight",0))into Min1 From "@SAP_OCALP"  
T0 Inner Join "@SAP_CALP2"  T1 On T0."DocEntry"=T1."DocEntry" Where T0."U_FAC"=:WhsCode
And "U_Hight"<:Height;


Select Min(IFNULL(T1."U_Hight",0)) into Max1 from "@SAP_OCALP"  
T0 Inner Join "@SAP_CALP2"  T1 On T0."DocEntry"=T1."DocEntry" Where T0."U_FAC"=:WhsCode
And "U_Hight">:Height;


Select ifnull(T1."U_Qty",0) into MinQty  From "@SAP_OCALP"  
T0 Inner Join "@SAP_CALP2"  T1 On T0."DocEntry"=T1."DocEntry" Where T0."U_FAC"=:WhsCode
And "U_Hight"=:Min1;


Select ifnull(T1."U_Qty",0) into MaxQty  From "@SAP_OCALP"  
T0 Inner Join "@SAP_CALP2"  T1 On T0."DocEntry"=T1."DocEntry" Where T0."U_FAC"=:WhsCode
And "U_Hight"=:Max1;

Select :MinQty+(:Height-:Min1)*(:MaxQty-:MinQty)/(:Max1-:Min1) As "Qty"
From dummy;



END IF;


END;


CALL SAP_GET_TEMP_CAL_QTY ('PO3MP001','110');