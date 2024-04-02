DROP PROCEDURE SAP_GET_GoodReceipt_Data;

Create PROCEDURE SAP_GET_GoodReceipt_Data 
(
 IN DocEntry NVARCHAR(100)
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS

Qty  Decimal ;
Amount  Decimal;
Price  Decimal;

BEGIN


Select (Select  IFNULL(Sum("U_Amt"),0) From "@SAP_IT2"  Where "DocEntry"=T0."U_ITDocE")
Into Amount
From "@SAP_OIT" T0  Where T0."DocEntry"=:DocEntry;

Select (Select  IFNULL(Sum("U_Qty"),0) From "@SAP_IT1"  Where "DocEntry"=T0."U_ITDocE")
Into Qty
From "@SAP_OIT" T0  Where T0."DocEntry"=:DocEntry;




SELECT (T2."Debit"+:Amount)/:Qty  INTO Price FROM OIGE T0  
INNER JOIN OJDT T1 ON T0."TransId" = T1."TransId" 
INNER JOIN JDT1 T2 ON T1."TransId" = T2."TransId"
Inner Join "@SAP_OIT" T3 On  T0."DocEntry"= T3."U_GIDE"  
 WHERE T2."Debit" >0 and  T3."DocEntry" =:DocEntry;

IF :Amount>0 Then 





 Select 
T0."DocEntry", T0."DocNum",T0."U_PDate", T0."U_ITRDocE", T0."U_ITRDocN", T0."U_ITDocE", 
T0."U_ITRDocE", T0."U_ITRDocN", T0."U_ITDocE",T0."U_ITDocN", T0."U_TCode",T0."U_Remark" ,
T0."U_TW",T0."U_TB" "Branch", 
T1."ItemCode",T1."Quantity",:Price As "Price",--(T1."StockPrice"+ (:Amount/:Qty)) As "Price" ,
(T1."Quantity"*:Price) As "LineTotal",
T1."OcrCode", T1."OcrCode2", T1."OcrCode3", T1."OcrCode4", T1."OcrCode5",T1."U_Chamber",
T1."U_Temp",T1."U_Density",T1."U_Dip",
(Select Top 1"U_AC" from "@SAP_OAAM" Where "U_TrnsType"='GR') As "AccountCode",
(Select "Series" From NNM1 Where "ObjectCode"='59' And "BPLId"=T0."U_TB") As "Series"
From "@SAP_OIT" T0 Inner Join IGE1 T1  On T0."U_GIDE"=T1."DocEntry" 
 WHERE T0."DocEntry"=:DocEntry;
Else
 Select 
T0."DocEntry", T0."DocNum",T0."U_PDate", T0."U_ITRDocE", T0."U_ITRDocN", T0."U_ITDocE", 
T0."U_ITRDocE", T0."U_ITRDocN", T0."U_ITDocE",T0."U_ITDocN", T0."U_TCode",T0."U_Remark" ,
T0."U_TW",T0."U_TB" "Branch", 
T1."ItemCode",T1."Quantity",Price As "Price",--(T1."StockPrice") As "Price" ,
(T1."Quantity"*:Price) As "LineTotal",
T1."OcrCode", T1."OcrCode2", T1."OcrCode3", T1."OcrCode4", T1."OcrCode5",T1."U_Chamber",
T1."U_Temp",T1."U_Density",T1."U_Dip",
(Select Top 1"U_AC" from "@SAP_OAAM" Where "U_TrnsType"='GR') As "AccountCode",
(Select "Series" From NNM1 Where "ObjectCode"='59' And "BPLId"=T0."U_TB") As "Series"
From "@SAP_OIT" T0 Inner Join IGE1 T1  On T0."U_GIDE"=T1."DocEntry" 
 WHERE T0."DocEntry"=:DocEntry;
End IF;


END;


Call SAP_GET_GoodReceipt_Data (124) ;