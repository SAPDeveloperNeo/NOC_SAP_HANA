DROP PROCEDURE SAP_GET_Purchase_Orde_Data;

Create PROCEDURE SAP_GET_Purchase_Orde_Data 
(
 IN DocEntry NVARCHAR(100)
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
-- read stock from the database
BEGIN


SELECT Distinct T0."DocEntry", T0."DocNum",T0."U_PDate",T3."ItemCode",((IFNULL(T1."U_Qty",0)))As "U_Qty"
,T1."LineId",T2."U_CardCode",T4."WhsCode",
	   T1."U_OCRC", T1."U_OCRC2", T1."U_OCRC3", T1."U_OCRC4", T1."U_OCRC5",T5."BPLid" "Branch",
	   T0."U_ITRDocE", T0."U_ITRDocN", T0."U_ITDocE", T0."U_ITDocN",T9."U_FW", T9."U_TW",
	   IFNULL(T6."Price",0) As "Price",T1."U_Chamber",T7."AbsID",T7."Number"
FROM "@SAP_OIT"  T0 Inner Join "@SAP_IT1" T1 On T0."DocEntry"=T1."DocEntry" 
Inner Join "@SAP_IT2" T2 On T0."DocEntry"=T2."DocEntry" and T2."U_PO"='Y' --And IFNULL(T2."U_PODE",'')=''
Inner Join OITM T3 On T3."U_Route"=T1."U_OCRC3"
Inner Join OITW T4 On T3."ItemCode"=T4."ItemCode"

Left  Join ITM1 T6 On T3."ItemCode"=T6."ItemCode" And  T6."PriceList" =10
Inner Join OOAT T7 On T7."BpCode"=T0."U_TCode" And T7."Status"='A' And T7."EndDate" >=T0."U_PDate"
INNER JOIN OAT1 T8 ON T7."AbsID" = T8."AgrNo" And T3."ItemCode"=T8."ItemCode"
Left  JOIN "@SAP_OITR" T9 ON T9."DocEntry" = T0."U_ITRDocE"
Inner Join OWHS T5 On T5."WhsCode"=T4."WhsCode" and T5."BPLid"=T9."U_TB"



Where T0."DocEntry"=:DocEntry ;




END;


Call SAP_GET_Purchase_Orde_Data (120) ;