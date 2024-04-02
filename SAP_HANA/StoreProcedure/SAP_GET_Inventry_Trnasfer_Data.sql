DROP PROCEDURE SAP_GET_Inventry_Trnasfer_Data;

Create PROCEDURE SAP_GET_Inventry_Trnasfer_Data 
(
 IN DocEntry NVARCHAR(100)
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
-- read stock from the database
BEGIN

SELECT  T0."DocEntry", T0."DocNum",T0."U_PDate",
T0."U_TW" As "FromWhs",T0."U_TB" As "FromBranch",T2."U_TW" As "ToWhs",T2."U_TB" As "ToBranch",
T0."U_ITRDocE", T0."U_ITRDocN", T0."U_ITDocE", T0."U_ITDocN", 
T0."U_TCode","U_TName", T0."U_TRNO",T0."U_DRName",T0."U_ITT",T0."U_PODE",T0."U_PODN",T0."U_ITDN",T0."U_ITDE",
T1."LineId", T1."U_ItemCode",T1."U_ItemName",T1."U_Qty", T1."U_OCRC", T1."U_OCRC2", T1."U_OCRC3", T1."U_OCRC4", T1."U_OCRC5",
T0."U_Remark",T1."U_Chamber",T1."U_UOM",T1."U_UOMG",
T1."U_Temp",T1."U_Density",T4."U_CAP",T4."U_OILDIP"
FROM "@SAP_OIT"  T0 Inner Join "@SAP_IT1"  T1 On T0."DocEntry"=T1."DocEntry" 
Left Join "@SAP_OITR" T2 On T0."U_ITRDocE"=T2."DocEntry"
Inner Join "@SAP_OCALP" T3 On T0."U_TRNO"=T3."U_VC" and T3."U_CEDate">=T0."U_PDate" and T3."U_Status"='Active'
Inner Join "@SAP_CALP1" T4 On T3."DocEntry"=T4."DocEntry" And T1."U_Chamber"=T4."U_CHN"
WHERE T0."DocEntry" =:DocEntry

;
 
END;


Call SAP_GET_Inventry_Trnasfer_Data (120) ;