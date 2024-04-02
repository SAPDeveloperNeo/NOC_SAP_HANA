DROP PROCEDURE SAP_GET_Service_Purchase_Orde_Data;

Create PROCEDURE SAP_GET_Service_Purchase_Orde_Data 
(
 IN DocEntry NVARCHAR(100)
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
-- read stock from the database
BEGIN


SELECT  T0."DocEntry", T0."DocNum",T0."DocDate",T3."ItemCode",T10."U_Qty" As "Quantity",
T1."LineNum",T0."U_TCode" As "CardCode",T4."WhsCode",
T1."OcrCode", T1."OcrCode2", T1."OcrCode3", T1."OcrCode4", T1."OcrCode5",T5."BPLid" "Branch",	  
IFNULL(T6."Price",0) As "Price",T1."U_Chamber",T7."AbsID",T7."Number",
T0."U_TCode", T0."U_TName", T0."U_TRNO"

FROM OPOR  T0 Inner Join POR1 T1 On T0."DocEntry"=T1."DocEntry" and IFNULL(T0."U_TCode",'')<>'' 
and IFNULL(T0."U_ITDocE",'')='' and IFNULL(T0."U_Flag",'')=''
Inner Join OITM T3 On T3."U_Route"=T1."OcrCode3" And T3."InvntItem"='N'
Inner Join OITW T4 On T3."ItemCode"=T4."ItemCode"
Inner Join OWHS T5 On T5."WhsCode"=T4."WhsCode" and T5."BPLid"=T0."BPLId"
Left  Join ITM1 T6 On T3."ItemCode"=T6."ItemCode" And  T6."PriceList" =10
Inner Join OOAT T7 On T7."BpCode"=T0."U_TCode" And T7."Status"='A' And T7."EndDate" >=T0."DocDate"
INNER JOIN OAT1 T8 ON T7."AbsID" = T8."AgrNo" And T3."ItemCode"=T8."ItemCode"
INNER JOIN "@SAP_ODLN" T9 ON T9."DocEntry"=T0."U_CMDocEntry" And T9."U_TCode"=T0."U_TCode" 
And T0."DocNum" = T9."U_DocNum" And T0."U_TRNO"=T9."U_TruckNum"
INNER JOIN "@SAP_DLN1" T10 On T9."DocEntry"=T10."DocEntry"
Where T0."DocEntry"=:DocEntry ;




END;


CALL SAP_GET_Service_Purchase_Orde_Data ('82') ;