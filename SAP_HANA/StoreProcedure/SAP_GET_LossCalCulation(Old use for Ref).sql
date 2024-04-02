DROP PROCEDURE SAP_GET_LossCalCulation;

Create PROCEDURE SAP_GET_LossCalCulation 
(
 IN DocEntry NVARCHAR(100)
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
-- read stock from the database
BEGIN


Select
1000 AS "ChemberCapacity",D."U_Dip" As "DisDip",R."U_Dip" As "RecDip",D."U_Dip"-R."U_Dip"  As "Dip_Diff",

ROUND(ROUND((1000/D."U_Dip")*(D."U_Dip"-R."U_Dip")))AS "ChemberLoss",
ROUND((1000/1000)*((D."U_Temp"-R."U_Temp")*(IFNULL(T0."U_PF",0))))AS "TempLoss",

(ROUND(ROUND((1000/D."U_Dip")*(D."U_Dip"-R."U_Dip"))))
-
(ROUND((1000/1000)*((D."U_Temp"-R."U_Temp")*(IFNULL(T0."U_PF",0)))))

As "ClaimableLoss",
T0."ItemCode",R."U_Chamber" 



From (

SELECT  'R' Type,
T0."DocEntry", T0."LineId", T0."U_ItemCode",T0."U_Qty",T0."U_Temp", T0."U_Density", T0."U_Dip", T0."U_Chamber" 
FROM "@SAP_IT1"  T0 WHERE T0."DocEntry" =:DocEntry) as R inner join 



(SELECT 'D' Type,
T0."DocEntry", T0."LineId", T0."U_ItemCode",T0."U_Qty",T0."U_Temp", T0."U_Density", T0."U_Dip", T0."U_Chamber" 
FROM "@SAP_IT1"  T0 WHERE T0."DocEntry" =(SELECT T0."U_ITDocE" FROM "@SAP_OIT"  T0 WHERE T0."DocEntry" =:DocEntry)

) As D on R."U_ItemCode" = D."U_ItemCode" 
Inner Join OITM T0 On T0."ItemCode"=R."U_ItemCode" 
;
 
END;


Call SAP_GET_LossCalCulation (6) ;