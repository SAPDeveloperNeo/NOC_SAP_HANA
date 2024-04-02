DROP PROCEDURE SAP_GET_TempMaster;

Create PROCEDURE SAP_GET_TempMaster 
(
 IN LocCode NVARCHAR(100)
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
-- read stock from the database
BEGIN


SELECT T0."ItemCode", T0."ItemName", T3."WhsCode", T3."WhsName",
Sum(IFNULL(T4."InQty",0))-Sum(IFNULL(T4."OutQty",0)) As "SAPQTY",
0 As "CalQty" ,0-(Sum(IFNULL(T4."InQty",0))-Sum(IFNULL(T4."OutQty",0))) As "Diff",
T0."InvntryUom" As "UomCode"
FROM OITM T0  INNER JOIN OITB T1 ON T0."ItmsGrpCod" = T1."ItmsGrpCod" 
INNER JOIN OITW T2 ON T0."ItemCode" = T2."ItemCode" 
INNER JOIN OWHS T3 ON T2."WhsCode" = T3."WhsCode" 
Left Join OINM T4 On T3."WhsCode"=T4."Warehouse" And T0."ItemCode"=T4."ItemCode" And T4."DocDate" <=Current_date
Left Join OUOM T5 On T5."UomEntry"=T0."PriceUnit"
WHERE T1."ItmsGrpNam" ='Trading' and  T3."U_Category" Not In  ('GIT') 
And T3."Location"=:LocCode
Group By T0."ItemCode", T0."ItemName", T3."WhsCode", T3."WhsName",T0."InvntryUom"
ORDER BY T0."ItemCode";





 
END;


Call SAP_GET_TempMaster (3) ;