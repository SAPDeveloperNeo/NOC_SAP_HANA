



DROP PROCEDURE SAP_GET_GoodReceiptNot_DocEntry;

Create PROCEDURE SAP_GET_GoodReceiptNot_DocEntry 

LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
-- read stock from the database
BEGIN
SELECT Distinct T1."DocEntry"
  FROM OPDN T0  INNER JOIN PDN1 T1 ON T0."DocEntry" = T1."DocEntry" 
  Left Join "@SAP_TMD1" T2 On T1."ItemCode"=T2."U_ItemCode" and T1."WhsCode"=T2."U_WhsCode"
  Left Join "@SAP_OTMD" T11 On   T11."DocEntry"=T2."DocEntry" And T11."U_PDate"=Current_date And T11."U_Status"='O'
  Inner Join OITM T3 On T3."ItemCode"=T1."ItemCode"
  Left JOIN "@SAP_ODLN" T9 ON T9."DocEntry"=T0."U_CMDocEntry"	 And T0."U_TRNO"=T9."U_TruckNum" 
Left JOIN "@SAP_DLN1" T10 On T9."DocEntry"=T10."DocEntry"
Where   IFNULL((CASE When IFNULL(T0."U_CMDocEntry",'')<>'' Then T10."U_Qty" Else T1."Quantity"*1000 End),0)   - 
 IFNULL((Select Sum(IFNULL("U_Qty",0)) from "@SAP_OIT" X0 Inner Join "@SAP_IT1" X1 On X0."DocEntry"=X1."DocEntry"
 And X0."U_SGRNDE"=CAST(T0."DocEntry" As NVARChaR(100)) And  X1."U_ItemCode"=
 (CASE When IFNULL(T0."U_CMDocEntry",'')<>'' Then  T10."U_ItemCode"  Else T1."ItemCode" End)
 And X1."U_Chamber"= IFNULL(T10."U_CHN",1)),0)<=0;


END;



CALL SAP_GET_GoodReceiptNot_DocEntry ;