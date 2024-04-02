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


Select "ItemCode","U_Chamber",

Case When "ChemberLoss" >0 And ifnull("U_TRNO",'')= '' Then  "TempLoss"
 When "ChemberLoss" >0 And ifnull("U_TRNO",'')<>'' Then "ChemberLoss" Else 0 End As "ChemberLoss",
Case When "TempLoss" >0 Then "TempLoss" Else 0 End As "TempLoss",
Case When "ClaimableLoss" >0 And  ifnull("U_TRNO",'')<>'' Then "ClaimableLoss" Else 0 End As "ClaimableLoss",
Case When "TankTempLoss" >0 Then "TankTempLoss" Else 0 End As "TankTempLoss"

 From (
Select R."U_TRNO",
R.U_CAP AS "ChemberCapacity",D."U_Dip" As "DisDip",R."U_Dip" As "RecDip",D."U_Dip"-R."U_Dip"  As "Dip_Diff",

ROUND(ROUND((R.U_CAP/D."U_Dip")*(D."U_Dip"-R."U_Dip")))AS "ChemberLoss",

--ROUND((R.U_CAP/1000)*((D."U_Temp"-R."U_Temp")*(IFNULL(T0."U_PF",0))))AS "TempLoss",

(ROUND(ROUND((R.U_CAP/D."U_Dip")*(D."U_Dip"-R."U_Dip"))))
-
(Case When  (ROUND((R.U_CAP/1000)*((D."U_Temp"-R."U_Temp")*(IFNULL(T0."U_PF",0))))) >  (ROUND(ROUND((R.U_CAP/D."U_Dip")*(D."U_Dip"-R."U_Dip"))))   
Then (ROUND(ROUND((R.U_CAP/D."U_Dip")*(D."U_Dip"-R."U_Dip")))) Else
(ROUND((R.U_CAP/1000)*((D."U_Temp"-R."U_Temp")*(IFNULL(T0."U_PF",0)))))
End)
As "ClaimableLoss",

T0."ItemCode",R."U_Chamber" ,
(D."U_Temp"-R."U_Temp") As "TempDiff",
T0."U_PF",

Case When  (ROUND((R.U_CAP/1000)*((D."U_Temp"-R."U_Temp")*(IFNULL(T0."U_PF",0))))) >  (ROUND(ROUND((R.U_CAP/D."U_Dip")*(D."U_Dip"-R."U_Dip"))))   
Then (ROUND(ROUND((R.U_CAP/D."U_Dip")*(D."U_Dip"-R."U_Dip")))) Else
(ROUND((R.U_CAP/1000)*((D."U_Temp"-R."U_Temp")*(IFNULL(T0."U_PF",0)))))
End AS "TempLoss",
ROUND((R.U_CAP/1000)*(R."U_Temp"-R."TankTemp")*IFNULL(T0."U_PF",0)) As "TankTempLoss"

From (

SELECT  'R' Type,T0."DocEntry", T0."LineId", T0."U_ItemCode",T0."U_Qty",
T0."U_Temp",T0."U_Density", T0."U_Dip", T0."U_Chamber",T3."U_CAP",T1."U_TRNO",

(Select IFNULL(X1."U_In",0) From "@SAP_OTMD" X0 Inner Join "@SAP_TMD1" X1 On X0."DocEntry"=X1."DocEntry" And 
X1."U_ItemCode"=T0."U_ItemCode" And T1."U_TW"=X1."U_WhsCode" And X0."U_PDate"=T1."U_PDate") 
As "TankTemp"

FROM "@SAP_IT1"  T0   Inner Join "@SAP_OIT" T1 On T0."DocEntry"=T1."DocEntry"
Left Join "@SAP_OCALP" T2 On (T2."U_VC"=T1."U_TRNO" Or T2."U_FAC"=T1."U_TW") and T2."U_Status"= 'Active' and T2."U_CEDate" >=Current_date
Left Join "@SAP_CALP1" T3 On T2."DocEntry"=T3."DocEntry" And ifnull(T0."U_Chamber",'')= ifnull(cast(T3."U_CHN" as nvarchar),'') 
And T3."U_CAP"<>0
--Left Join "@SAP_TMD1" T4 On T4."U_ItemCode"=T0."U_ItemCode" And T1."U_TW"=T4."U_WhsCode" 
--Left Join "@SAP_OTMD" T5 On T5."DocEntry"=T4."DocEntry"


WHERE T0."DocEntry" =:DocEntry And IFNULL(T3."U_CAP",0)<>0) as R inner join 



(SELECT  'D' Type,T0."DocEntry", T0."LineId", T0."U_ItemCode",T0."U_Qty",
T0."U_Temp",T0."U_Density", T0."U_Dip", T0."U_Chamber",T3."U_CAP" ,T1."U_TRNO"
FROM "@SAP_IT1"  T0   Inner Join "@SAP_OIT" T1 On T0."DocEntry"=T1."DocEntry"
Left Join "@SAP_OCALP" T2 On (T2."U_VC"=T1."U_TRNO" Or T2."U_FAC"=T1."U_TW") and T2."U_Status"= 'Active' and T2."U_CEDate" >=Current_date
Left Join "@SAP_CALP1" T3 On T2."DocEntry"=T3."DocEntry" And ifnull(T0."U_Chamber",'')= ifnull(cast(T3."U_CHN" as nvarchar),'') 
And T3."U_CAP"<>0
WHERE T0."DocEntry" =(SELECT T0."U_ITDocE" FROM "@SAP_OIT"  T0 WHERE T0."DocEntry" =:DocEntry)

) As D on R."U_ItemCode" = D."U_ItemCode" 
Inner Join OITM T0 On T0."ItemCode"=R."U_ItemCode"  And R."U_Chamber"=D."U_Chamber"
) As Final
;
 
END;


Call SAP_GET_LossCalCulation (13) ;