DROP PROCEDURE SAP_GET_GoodIssue_Data;

Create PROCEDURE SAP_GET_GoodIssue_Data 
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
T0."U_FW", T0."U_TW", T0."U_ITRDocE", T0."U_ITRDocN", T0."U_ITDocE", T0."U_ITDocN", T0."U_TCode",T0."U_Remark" ,
T1."LineId", T1."U_ItemCode", T1."U_OCRC", T1."U_OCRC2", T1."U_OCRC3", T1."U_OCRC4", T1."U_OCRC5",
T0."U_FB" "Branch",T1."U_Chamber",
T1."U_Temp",T1."U_Density",T1."U_Dip",
(Select Top 1"U_AC" from "@SAP_OAAM" Where "U_TrnsType"='GR') As "AccountCode",
(Select "Series" From NNM1 Where "ObjectCode"='60' And "BPLId"=T0."U_FB") As "Series",
(Select "U_Qty" From "@SAP_IT1" Where "DocEntry"=T0."U_ITDocE" and "U_Chamber"=T1."U_Chamber")
As "U_Qty"

FROM "@SAP_OIT"  T0 Inner Join "@SAP_IT1"  T1 On T0."DocEntry"=T1."DocEntry" 

WHERE T0."DocEntry" =:DocEntry;
 
END;


Call SAP_GET_GoodIssue_Data (121) ;