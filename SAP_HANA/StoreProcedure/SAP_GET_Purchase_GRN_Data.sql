




DROP PROCEDURE SAP_GET_Purchase_GRN_Data;

Create PROCEDURE SAP_GET_Purchase_GRN_Data 
(
 IN DocEntry NVARCHAR(100)
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
-- read stock from the database
BEGIN





SELECT T0."CardCode", T1."ItemCode", T1."Dscription", T1."Quantity",T1."Price", 
T1."OcrCode", T1."OcrCode2", T1."OcrCode3", T1."OcrCode4", T1."OcrCode5", 
T0."BPLId" As "Branch",T0."DocDate",:DocEntry As "DocEntry",T1."U_Chamber",
T0."DocEntry" As "BaseEntry",T1."LineNum",
(SELECT T0."NumAtCard" FROM OPDN T0 WHERE T0."DocEntry"= (
Select "U_SGRNDE" From "@SAP_OIT"  Where  "DocEntry"=:DocEntry)) As "VenderRef"

FROM OPOR T0  INNER JOIN POR1 T1 ON T0."DocEntry" = T1."DocEntry"
Where T0."DocEntry"=(Select "DocEntry" From OPOR Where "U_ITDocE" = (
Select Distinct T0."BaseEntry" From PDN1 T0 Where "DocEntry"=(
Select "U_SGRNDE" From "@SAP_OIT"  Where  "DocEntry"=:DocEntry)
And T0."BaseType"='22') And "U_Flag"='SPO');




END;


Call SAP_GET_Purchase_GRN_Data (90) ;