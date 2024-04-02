
DROP PROCEDURE SAP_GET_GoodReceiptNote_Data;
Create PROCEDURE SAP_GET_GoodReceiptNote_Data 
(
 IN DocEntry NVARCHAR(100)
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
-- read stock from the database
BEGIN

 
 SELECT Distinct T1."DocEntry", T0."DocNum", T1."ItemCode" As "U_ItemCode", T1."Dscription" As "U_ItemName",
 
 IFNULL((CASE When IFNULL(T0."U_CMDocEntry",'')<>'' Then T10."U_Qty" Else T1."Quantity"*1000 End),0)   - 
 IFNULL((Select Sum(IFNULL("U_Qty",0)) from "@SAP_OIT" X0 Inner Join "@SAP_IT1" X1 On X0."DocEntry"=X1."DocEntry"
 And X0."U_SGRNDE"=CAST(T0."DocEntry" As NVARChaR(100)) And  X1."U_ItemCode"=
 (CASE When IFNULL(T0."U_CMDocEntry",'')<>'' Then  T10."U_ItemCode"  Else T1."ItemCode" End)
 And X1."U_Chamber"= IFNULL(T10."U_CHN",1)),0) 
 As "U_Qty",
 
 T0."BPLId" As "U_FB",
  T1."WhsCode" As  "U_FW",T1."OcrCode" As "U_OCRC", T1."OcrCode2" As "U_OCRC2",
  T1."OcrCode3" "U_OCRC3", T1."OcrCode4" As "U_OCRC4",
  T1."OcrCode5" As  "U_OCRC5", T2."U_Out" As "U_Temp", T2."U_Density", T10."U_Dip", 
  IFNULL(T10."U_CHN",1) As "U_Chamber", 
  T0."U_TCode",T0."U_TRNO",T0."U_DRName",T0."U_TName",
  T3."InvntryUom"
  FROM OPDN T0  INNER JOIN PDN1 T1 ON T0."DocEntry" = T1."DocEntry" 
  Left Join "@SAP_TMD1" T2 On T1."ItemCode"=T2."U_ItemCode" and T1."WhsCode"=T2."U_WhsCode"
  Left Join "@SAP_OTMD" T11 On   T11."DocEntry"=T2."DocEntry" And T11."U_PDate"=Current_date And T11."U_Status"='O'
  Inner Join OITM T3 On T3."ItemCode"=T1."ItemCode"
  Left JOIN "@SAP_ODLN" T9 ON T9."DocEntry"=T0."U_CMDocEntry"
	 And T0."U_TRNO"=T9."U_TruckNum" 
Left JOIN "@SAP_DLN1" T10 On T9."DocEntry"=T10."DocEntry"
Where  T0."DocEntry"=:DocEntry
And  IFNULL((CASE When IFNULL(T0."U_CMDocEntry",'')<>'' Then T10."U_Qty" Else T1."Quantity"*1000 End),0)   - 
 IFNULL((Select Sum(IFNULL("U_Qty",0)) from "@SAP_OIT" X0 Inner Join "@SAP_IT1" X1 On X0."DocEntry"=X1."DocEntry"
 And X0."U_SGRNDE"=CAST(T0."DocEntry" As NVARChaR(100)) And  X1."U_ItemCode"=
 (CASE When IFNULL(T0."U_CMDocEntry",'')<>'' Then  T10."U_ItemCode"  Else T1."ItemCode" End)
 And X1."U_Chamber"= IFNULL(T10."U_CHN",1)),0)>0 ;
 
END;
CALL SAP_GET_GoodReceiptNote_Data (491)