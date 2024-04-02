DROP PROCEDURE SAP_GET_Purchase_Invoice_Data;

Create PROCEDURE SAP_GET_Purchase_Invoice_Data 
(
 IN DocEntry NVARCHAR(100)
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
-- read stock from the database
BEGIN

SELECT  T0."DocEntry", T0."DocNum",T0."U_PDate", 
 T0."U_TW", T0."U_ITRDocE", T0."U_ITRDocN", T0."U_ITDocE", T0."U_ITDocN", 
(Select "ConnBP" From OCRD Where "CardCode"=T0."U_TCode") As "U_TCode",
 ---T0."U_TCode",
 T0."U_Remark" ,
T1."LineId", T1."U_ItemCode",T1."U_ClLoss",
T0."U_TB" "Branch"
FROM "@SAP_OIT"  T0 Inner Join "@SAP_IT4"  T1 On T0."DocEntry"=T1."DocEntry" 

WHERE T0."DocEntry" =DocEntry And  IFNULL(T1."U_ClLoss",0)>0;
 
 
END;


Call SAP_GET_Purchase_Invoice_Data (6) ;


