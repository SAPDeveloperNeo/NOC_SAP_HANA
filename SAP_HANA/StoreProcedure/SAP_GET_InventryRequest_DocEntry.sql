



DROP PROCEDURE SAP_GET_InventryRequest_DocEntry;

Create PROCEDURE SAP_GET_InventryRequest_DocEntry 

LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
-- read stock from the database
BEGIN

Select  T3."DocEntry" from
"@SAP_OIT" T0  Inner Join "@SAP_IT1" T1 On T0."DocEntry"=T1."DocEntry"
Inner Join "@SAP_OITR" T3 On T0."U_ITRDocE"=CAST(T3."DocEntry" As NVARChaR(100))
Left Join "@SAP_ITR1" T4 On T3."DocEntry"=T4."DocEntry"
And T4."U_ItemCode"=T1."U_ItemCode" And T4."LineId"=T1."LineId"
Group By T3."DocEntry"
having  Sum(IFNULL(T4."U_Qty",0))-Sum(T1."U_Qty")<0;


END;



CALL SAP_GET_InventryRequest_DocEntry ;