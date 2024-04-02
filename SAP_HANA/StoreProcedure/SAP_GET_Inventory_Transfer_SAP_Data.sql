DROP PROCEDURE SAP_GET_Inventory_Transfer_SAP_Data;

Create PROCEDURE SAP_GET_Inventory_Transfer_SAP_Data 
(
 IN DocEntry NVARCHAR(100)
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
-- read stock from the database
BEGIN

SELECT T1."U_FW", T0."DocEntry", T0."DocNum",T0."U_PDate", 
T1."U_FW", T1."U_TW", T0."U_ITRDocE", T0."U_ITRDocN", T0."U_ITDocE", T0."U_ITDocN", T0."U_TCode", 
T1."LineId", T1."U_ItemCode", T1."U_OCRC", T1."U_OCRC2", T1."U_OCRC3", T1."U_OCRC4", T1."U_OCRC5",
T1."U_Chamber",T1."U_Temp",T1."U_Density",T1."U_Dip",

 (IFNULL(T1."U_Qty",0)-(IFNULL(T2."U_TemLoss",0)+IFNULL(T2."U_ClLoss",0)+IFNULL(T2."U_TTL",0))) As "U_Qty",
 
T4."UomEntry"
FROM "@SAP_OIT"  T0 Inner Join "@SAP_IT1"  T1 On T0."DocEntry"=T1."DocEntry" 
Left  Join "@SAP_IT4"  T2  on T2."DocEntry"=T0."DocEntry" 
and T2."U_Chamber"=T1."U_Chamber" and IFNULL(T2."U_ItemCode",'')<>''
Left Join OUOM T4 On T4."UomCode"=T1."U_UOM"
WHERE T0."DocEntry" =:DocEntry;
 
END;


Call SAP_GET_Inventory_Transfer_SAP_Data (25) ;