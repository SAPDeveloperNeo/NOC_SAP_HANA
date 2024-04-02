
DROP PROCEDURE SAP_GET_Topping_Data;
Create PROCEDURE SAP_GET_Topping_Data 
(
 IN TRNO NVARCHAR(100)
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
-- read stock from the database
BEGIN

SELECT T0."DocEntry", T0."DocNum", T0."U_FB", T0."U_FW", T0."U_TB", 
T0."U_TW", T0."U_Route", T0."U_Remark", T0."U_ITRDocE", T0."U_ITRDocN", 
T0."U_ITDocE", T0."U_ITDocN", T0."U_Status", T0."U_TCode", T0."U_TName", 
T0."U_TRNO", T0."U_DRName",
T1."LineId", T1."U_ItemCode", T1."U_ItemName", T1."U_FW", T1."U_TW",
(T4."U_CAP"-T1."U_Qty") As "QTY", T1."U_UOM", T1."U_UOMG", T1."U_Temp", T1."U_Density", T1."U_Dip", 
T1."U_Chamber", T1."U_OCRC", T1."U_OCRC", T1."U_OCRC", T1."U_OCRC", T1."U_OCRC", 
T1."U_OCRC", T1."U_OCRC2", T1."U_OCRC3", T1."U_OCRC4", T1."U_OCRC5",
T1."U_BC", T1."U_WN", T1."U_SN", T1."U_CWSP", T1."U_Density2",
T1."U_DVari", T1."U_FP", T1."U_Batch", T1."U_FBP", T1."U_ODip"
FROM "@SAP_OIT"  T0 Inner Join  "@SAP_IT1"  T1 On T0."DocEntry"=T1."DocEntry"
Inner Join "@SAP_OCALP" T3 On T3."U_VC"=T0."U_TRNO"
Inner join "@SAP_CALP1" T4 On T4."DocEntry"=T3."DocEntry" And T4."U_CHN"=T1."U_Chamber"
Where T0."DocEntry"=(SELECT Top 1 "U_MaxDocNo" FROM "@TRUCK_STATUS" WHERE "U_Status" ='D' And "Code"=:TRNO);


 
END;
CALL SAP_GET_Topping_Data ('0662NA5KHA')