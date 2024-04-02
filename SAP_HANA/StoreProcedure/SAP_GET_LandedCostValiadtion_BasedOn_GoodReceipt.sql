DROP PROCEDURE SAP_GET_LandedCostValiadtion_BasedOn_GoodReceipt;

Create PROCEDURE SAP_GET_LandedCostValiadtion_BasedOn_GoodReceipt 
(
 IN DocEntry NVARCHAR(100)
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
-- read stock from the database
BEGIN


Select Count(*) As "Count"  FROM 
 
 
 "PDN1" T2 INNER JOIN "OPDN" T3 ON T3."DocEntry" = T2."DocEntry"
INNER JOIN "OITM" T4 ON T4."ItemCode" = T2."ItemCode"
INNER JOIN "OCRD" T5 ON T5."CardCode" = T3."CardCode"
WHERE T3."LndCstNum" = 0 AND T5."Country"<>'NP' and T4."InvntItem" = 'Y' 
And T2."WhsCode" like '%GI%'
AND T3."DocEntry" =:DocEntry ;




END;


CALL SAP_GET_LandedCostValiadtion_BasedOn_GoodReceipt ('252') ;