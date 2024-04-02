


DROP PROCEDURE SAP_GET_PickList_Warehouse;

Create PROCEDURE SAP_GET_PickList_Warehouse 
(
 IN DocNum NVARCHAR(100)
 
 
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
-- read stock from the database
BEGIN




SELECT   T5."WhsCode",T5."WhsName"
FROM OPKL T0  INNER JOIN PKL1 T1 ON T0."AbsEntry" = T1."AbsEntry"
INNER JOIN  ORDR T2 On T1."OrderEntry"=T2."DocEntry"
Inner Join RDR1 T3 ON T2."DocEntry" = T3."DocEntry" 
Inner Join OITW	 T4 On T3."ItemCode" =T4."ItemCode"
Inner  Join OWHS  T5 On T4."WhsCode"=T5."WhsCode"  And T5."BPLid" = T2."BPLId"
WHERE T1."AbsEntry" =:DocNum;




END;


CALL SAP_GET_PickList_Warehouse ('20');