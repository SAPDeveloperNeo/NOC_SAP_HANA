



DROP PROCEDURE SAP_GET_ChamberAllocation_ItemList;

Create PROCEDURE SAP_GET_ChamberAllocation_ItemList 
(
 IN DocNum NVARCHAR(100)
 
 
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
-- read stock from the database
BEGIN




SELECT   T3."ItemCode",T3."Dscription",T2."CardCode" ,T2."CardName",T3."WhsCode",T4."WhsName"
FROM OPKL T0  INNER JOIN PKL1 T1 ON T0."AbsEntry" = T1."AbsEntry"
INNER JOIN  ORDR T2 On T1."OrderEntry"=T2."DocEntry"
Inner Join RDR1 T3 ON T2."DocEntry" = T3."DocEntry" 
Left Join OWHS  T4 On T4."WhsCode"=T3."WhsCode"
WHERE T1."AbsEntry" =:DocNum;
--Group By T3."ItemCode",T3."Dscription",T2."CardCode",T2."CardName"   ;



END;


CALL SAP_GET_ChamberAllocation_ItemList ('39');