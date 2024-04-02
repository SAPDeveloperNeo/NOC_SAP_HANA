DROP PROCEDURE SAP_GET_GRN_Data;

Create PROCEDURE SAP_GET_GRN_Data 
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
T2."DocEntry",T2."DocNum",T2."U_PDate",T0."BPLId" As "Branch",
T1."U_Chamber" ,T0."DocEntry" As "BaseEntry",T1."LineNum",T2."U_Ref"
FROM OPOR T0  INNER JOIN POR1 T1 ON T0."DocEntry" = T1."DocEntry"
INNER JOIN "@SAP_OIT" T2 On T2."U_PODE"=T0."DocEntry"

Where T2."DocEntry"=:DocEntry;




END;


Call SAP_GET_GRN_Data (96) ;