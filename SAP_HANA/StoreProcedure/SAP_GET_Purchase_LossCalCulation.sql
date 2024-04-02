DROP PROCEDURE SAP_GET_Purchase_LossCalCulation;

Create PROCEDURE SAP_GET_Purchase_LossCalCulation 
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

Case when IFNULL("U_VC",'')<>'' Then 
Case When "ChemberLoss" >0 Then "ChemberLoss" Else 0 End 
else "TempLoss" end As  "ChemberLoss",

Case When "TempLoss" >0 Then "TempLoss" Else 0 End As "TempLoss",


Case when IFNULL("U_VC",'')<>'' Then 

Case When "ClaimableLoss" >0 Then "ClaimableLoss" Else 0 End Else 0 End  As "ClaimableLoss",

Case When "TankTempLoss" >0 Then "TankTempLoss" Else 0 End As "TankTempLoss"


 From (

Select
R."U_VC" ,R.U_CAP AS "ChemberCapacity",D."U_Dip" As "DisDip",R."U_Dip" As "RecDip",D."U_Dip"-R."U_Dip"  As "Dip_Diff",

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
--,R.U_CAP,R."U_Temp",R."TankTemp"

From (

SELECT Distinct 'R' Type,T0."DocEntry", T0."LineId", T0."U_ItemCode",T0."U_Qty",
T0."U_Temp",T0."U_Density", T0."U_Dip", T0."U_Chamber",T2."U_VC",
Case When IFNULL(T2."U_VC",'')='' Then T0."U_Qty" Else  IFNULL(T3."U_CAP",0) End  As "U_CAP",

(Select IFNULL(X1."U_In",0) From "@SAP_OTMD" X0 Inner Join "@SAP_TMD1" X1 On X0."DocEntry"=X1."DocEntry" And 
X1."U_ItemCode"=T0."U_ItemCode" And T1."U_TW"=X1."U_WhsCode" And X0."U_PDate"=T1."U_PDate" And X0."U_Status"='O') 
As "TankTemp"
FROM "@SAP_IT1"  T0   Inner Join "@SAP_OIT" T1 On T0."DocEntry"=T1."DocEntry"
Left Join "@SAP_OCALP" T2 On T2."U_VC"=T1."U_TRNO" and T2."U_Status"= 'Active' and T2."U_CEDate" >=Current_date
Left Join "@SAP_CALP1" T3 On T2."DocEntry"=T3."DocEntry" And ifnull(T0."U_Chamber",'')= ifnull(cast(T3."U_CHN" as nvarchar),'')

--Left Join "@SAP_TMD1" T4 On T4."U_ItemCode"=T0."U_ItemCode" And T1."U_TW"=T4."U_WhsCode" 
--Left Join "@SAP_OTMD" T5 On T5."DocEntry"=T4."DocEntry"
--And T5."U_PDate"=T1."U_PDate"
WHERE T0."DocEntry" =:DocEntry) as R inner join 



(SELECT Distinct 'D' Type,T0."DocEntry", T1."LineNum", T1."ItemCode",T10."U_Qty" As "Quantity",
T10."U_Temp",T10."U_Density", T10."U_Dip", T10."U_CHN" As "U_Chamber",
Case When IFNULL(T2."U_VC",'')='' Then T10."U_Qty"*1000 Else  IFNULL(T3."U_CAP",0) End  As "U_CAP",
--T3."U_CAP" 
T2."U_VC"
FROM OPDN  T0   Inner Join PDN1 T1 On T0."DocEntry"=T1."DocEntry"
INNER JOIN "@SAP_ODLN" T9 ON T9."DocEntry"=T0."U_CMDocEntry" --And T9."U_TCode"=T0."U_TCode" 
---And T0."U_TRNO"=T9."U_TruckNum" 
INNER JOIN "@SAP_DLN1" T10 On T9."DocEntry"=T10."DocEntry"
Left Join "@SAP_OCALP" T2 On T2."U_VC"=T0."U_TRNO" and T2."U_Status"= 'Active' and T2."U_CEDate" >=Current_date
Left Join "@SAP_CALP1" T3 On T2."DocEntry"=T3."DocEntry" And ifnull(cast(T10."U_CHN" as nvarchar),'') = ifnull(cast(T3."U_CHN" as nvarchar),'') 

WHERE T0."DocEntry" =(SELECT T0."U_SGRNDE" FROM "@SAP_OIT"  T0 WHERE T0."DocEntry" =:DocEntry)

) As D on R."U_ItemCode" = D."ItemCode" 
Inner Join OITM T0 On T0."ItemCode"=R."U_ItemCode"  And R."U_Chamber"=D."U_Chamber"
) As Final
;
 
END;


Call SAP_GET_Purchase_LossCalCulation (109) ;