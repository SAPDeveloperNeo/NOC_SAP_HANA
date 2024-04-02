DROP PROCEDURE SAP_GET_Inventory_Transfer_Request_Data;

Create PROCEDURE SAP_GET_Inventory_Transfer_Request_Data 
(
 IN DocEntry NVARCHAR(100)
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
 FromBarch  NVARCHAR(50);
 TOBarch  NVARCHAR(50);
 FromWhs  NVARCHAR(50);
 TOWhs  NVARCHAR(50);
 FromLocation NVARCHAR(50);
 ToLocation NVARCHAR(50);

BEGIN


SELECT  Distinct  
T0."U_FB",T0."U_TB", T0."U_FW", T0."U_TW",
(Select "Location" From OWHS Where "WhsCode"=T0."U_FW") As "FromLocation", 
(Select "Location" From OWHS Where "WhsCode"=T0."U_TW") As "ToLcation"
INTO FromBarch,TOBarch,FromWhs,TOWhs,FromLocation,ToLocation
FROM "@SAP_OITR" T0  
Inner Join  OWHS T1  on (T0."U_FW"=T1."WhsCode" Or T0."U_TW"=T1."WhsCode"  )
Where T0."DocEntry"=:DocEntry;


IF :FromBarch=:TOBarch And :FromLocation=:ToLocation
Then 

 SELECT T0."DocEntry", T0."DocNum", T0."U_FB", T0."U_FW", T0."U_TB",
 T0."U_TW", T0."U_Route", T0."U_Remark", T0."U_ITDocE", T0."U_ITDocN", T0."U_Status", T0."U_ITT", 
 T1."U_ItemCode", T1."U_ItemName", T1."U_FW", T1."U_TW",
   IFNULL(T1."U_Qty",0)  - 
 (Select Sum(IFNULL("U_Qty",0)) from "@SAP_OIT" X0 Inner Join "@SAP_IT1" X1 On X0."DocEntry"=X1."DocEntry"
 And X0."U_ITRDocE"=CAST(T0."DocEntry" As NVARChaR(100)) And  X1."U_ItemCode"=T1."U_ItemCode" )As "U_Qty",
  T1."U_UOM", T1."U_UOMG", 
 T1."U_Temp", T1."U_Density",T1."U_OCRC", T1."U_OCRC2", T1."U_OCRC3", T1."U_OCRC4", T1."U_OCRC5"
 FROM "@SAP_OITR" T0 Inner Join "@SAP_ITR1"  T1 On T0."DocEntry"=T1."DocEntry"
 Where T0."DocEntry"=:DocEntry;

Else

 SELECT TOP 1 T0."DocEntry", T0."DocNum", T0."U_FB", T0."U_FW", T0."U_FB" AS "U_TB", 
 --(Select  "WhsCode" From OWHS Where "U_Category"='GIT' And "BPLid"= T0."U_FB" ) As "U_TW" ,
T3."WhsCode" As "U_TW" , 
 T0."U_Route", T0."U_Remark", T0."U_ITDocE", T0."U_ITDocN", T0."U_Status", T0."U_ITT", 
 T1."U_ItemCode", T1."U_ItemName",
 IFNULL(T1."U_Qty",0)  - 
 (Select Sum(IFNULL("U_Qty",0)) from "@SAP_OIT" X0 Inner Join "@SAP_IT1" X1 On X0."DocEntry"=X1."DocEntry"
 And X0."U_ITRDocE"=CAST(T0."DocEntry" As NVARChaR(100)) And  X1."U_ItemCode"=T1."U_ItemCode" )As "U_Qty",
 T1."U_UOM", T1."U_UOMG", 
 T1."U_Temp", T1."U_Density",T1."U_OCRC", T1."U_OCRC2", T1."U_OCRC3", T1."U_OCRC4", T1."U_OCRC5"
 FROM "@SAP_OITR" T0 Inner Join "@SAP_ITR1"  T1 On T0."DocEntry"=T1."DocEntry"
 LEFT JOIN OWHS T3 On T3."BPLid"= T0."U_FB" And "U_Category"='GIT'
 Where T0."DocEntry"=:DocEntry;
 


End IF; 


 
 



 
END;


Call SAP_GET_Inventory_Transfer_Request_Data (8) ;