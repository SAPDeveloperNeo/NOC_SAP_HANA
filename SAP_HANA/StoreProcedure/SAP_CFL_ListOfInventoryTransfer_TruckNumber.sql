DROP PROCEDURE SAP_CFL_ListOfInventoryTransfer_TruckNumber;
CREATE PROCEDURE SAP_CFL_ListOfInventoryTransfer_TruckNumber 
(
 IN UserId INT,
 IN TRNO NVARCHAR(100)
)
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
-- read stock from the database
BEGIN


SELECT '' "Select",T0."DocEntry", T0."DocNum", T0."U_PDate", T0."U_FB",
T0."U_FW", T0."U_TB", T0."U_TW", T0."U_ITRDocE", T0."U_ITRDocN",
T0."U_TCode", T0."U_ITT" ,T0."U_TRNO",
Sum(T2."U_Qty")
FROM "@SAP_OIT"  T0 
Inner Join OWHS T1 On T0."U_TW"=T1."WhsCode" And T1."U_Category"='GIT' And  IFNULL(T0."U_TRNO",'')=:TRNO
Inner Join "@SAP_IT1" T2 On T0."DocEntry"=T2."DocEntry"
Inner Join "@SAP_OITR" T3 On Cast(T3."DocEntry" As NVarchar(100))=IFNULL(T0."U_ITRDocE",'') 
And  T3."U_TB" In (SELECT T1."BPLId" FROM OUSR T0 
INNER JOIN USR6 T1 ON T0."USER_CODE" = T1."UserCode" 
INNER JOIN OBPL T2 ON T1."BPLId" = T2."BPLId" And  T2."Disabled" ='N' WHERE T0."USERID"=:UserId)

where    ifnull((SELECT sum( ifnull(T11."U_Qty",0)+ifnull(T12."U_ChemLos",0)) as "TotalQty" 
FROM "@SAP_OIT"  T10 
inner join "@SAP_IT1"  T11 on T10."DocEntry" = T11."DocEntry" 
left join "@SAP_IT4"  T12  on T10."DocEntry" = T12."DocEntry" and T11."U_Chamber" = T12."U_Chamber" and T11."U_ItemCode" = T12."U_ItemCode"
where T10."U_ITDocE" = T0."DocEntry"-- and T10."DocEntry" <> T0."DocEntry"
and T11."U_ItemCode" =T2."U_ItemCode"
),0)

<ifnull(T2."U_Qty",0) 



Group By T0."DocEntry", T0."DocNum", T0."U_PDate", T0."U_FB",T0."U_TRNO",
T0."U_FW", T0."U_TB", T0."U_TW", T0."U_ITRDocE", T0."U_ITRDocN",
T0."U_TCode", T0."U_ITT" 
Having Sum(T2."U_Qty")>0 Order By T0."DocEntry";

END;



