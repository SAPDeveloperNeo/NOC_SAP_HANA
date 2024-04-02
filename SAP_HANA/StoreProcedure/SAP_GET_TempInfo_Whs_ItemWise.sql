



DROP PROCEDURE SAP_GET_TempInfo_Whs_ItemWise;

Create PROCEDURE SAP_GET_TempInfo_Whs_ItemWise 
(
 IN ItemCode NVARCHAR(100),
 IN WhsCode NVARCHAR(100)
 
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
-- read stock from the database
BEGIN

Select T1."U_Out", T1."U_Density" From "@SAP_OTMD" T0 inner Join "@SAP_TMD1" T1 On T0."DocEntry"=T1."DocEntry"
Where T1."U_ItemCode"=:ItemCode And T1."U_WhsCode"=:WhsCode And T0."U_PDate"=Current_date;

END;


Call SAP_GET_TempInfo_Whs_ItemWise ('TR100001','PO5MP001') ;