
DROP PROCEDURE SAP_GET_StockTransfer_AR_Invoice_Data;

Create PROCEDURE SAP_GET_StockTransfer_AR_Invoice_Data 
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
T0."U_TB" "Branch",'VAT@13' As "TaxCode",
IFNULL(T2."Price",0) As "Price",
 T3."U_OCRC", T3."U_OCRC2", T3."U_OCRC3", T3."U_OCRC4", T3."U_OCRC5",T3."U_Chamber",
 T3."U_Temp",T3."U_Density",T3."U_Dip"
 
 
FROM "@SAP_OIT"  T0 Inner Join "@SAP_IT4"  T1 On T0."DocEntry"=T1."DocEntry" 
Inner Join "@SAP_IT1" T3 On T0."DocEntry"=T3."DocEntry" And T3."U_Chamber" =T1."U_Chamber"
Left  Join ITM1 T2 On T1."U_ItemCode"=T2."ItemCode" And  T2."PriceList" =1
WHERE T0."DocEntry" =:DocEntry And  IFNULL(T1."U_ClLoss",0)>0;


 
END;

CALL SAP_GET_StockTransfer_AR_Invoice_Data (41)