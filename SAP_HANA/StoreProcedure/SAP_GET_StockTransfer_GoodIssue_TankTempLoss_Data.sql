DROP PROCEDURE SAP_GET_StockTransfer_GoodIssue_TankTempLoss_Data;

Create PROCEDURE SAP_GET_StockTransfer_GoodIssue_TankTempLoss_Data 
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
 T0."U_TW", T0."U_ITRDocE", T0."U_ITRDocN", T0."U_ITDocE", T0."U_ITDocN", T0."U_TCode",T0."U_Remark" ,
T1."LineId", T1."U_ItemCode",T1."U_TTL",
T0."U_TB" "Branch",T1."U_Chamber",
T2."U_OCRC", T2."U_OCRC2", T2."U_OCRC3", T2."U_OCRC4", T2."U_OCRC5",
T2."U_Temp",T2."U_Density",T2."U_Dip",
(Select "Series" From NNM1 Where "ObjectCode"='60' And "BPLId"=T0."U_TB") As "Series"
FROM "@SAP_OIT"  T0 Inner Join "@SAP_IT4"  T1 On T0."DocEntry"=T1."DocEntry" 
Inner Join  "@SAP_IT1" T2 On T2."DocEntry"=T0."DocEntry" and T1."U_Chamber"=T2."U_Chamber"

WHERE T0."DocEntry" =DocEntry And  IFNULL(T1."U_TTL",0)>0;
 
END;


Call SAP_GET_StockTransfer_GoodIssue_TankTempLoss_Data (41) ;