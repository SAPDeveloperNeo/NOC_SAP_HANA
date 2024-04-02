



DROP PROCEDURE SAP_GET_PickListQty;

Create PROCEDURE SAP_GET_PickListQty 
(
 IN DocEntry NVARCHAR(100),
 IN CardCode NVARCHAR(100),
 IN ItemCode NVARCHAR(100)
 
 
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS

Status NVARCHAR(10);
MaxDocNo NVARCHAR(50);
-- read stock from the database
BEGIN

Select Sum(IFNULL(T1."U_Qty",0)) As "Qty",T1."U_ItemCode",
T1."U_CardCode" from "@SAP_OPKL" T0 Inner  Join "@SAP_PKL1" T1 
On T0."DocEntry"=T1."DocEntry" Where T0."DocEntry"=:DocEntry And T1."U_ItemCode"=:ItemCode 
And T1."U_CardCode"=:CardCode
Group By T1."U_ItemCode",T1."U_CardCode";



END;


--CALL SAP_GET_PickListQty ('12','','');