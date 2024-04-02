alter PROCEDURE SAP_GETDATA_Adjustment_JE_FromBranch 
(
 IN DocEntry NVARCHAR(100)
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
-- read stock from the database
BEGIN


Select Distinct  T1."U_BFL" As "DebitAccount",T0."U_FB" As "Branch",
(Select "DocTotal" from OIGE Where "DocEntry"=T0."U_GIDE" And "U_Flag"='STITGI') As "Amt",
(Select "U_AC" from "@SAP_OAAM" Where  "U_TrnsType"='GR') As "CreditAccount",
T2."U_OCRC", T2."U_OCRC2", T2."U_OCRC3", T2."U_OCRC4", T2."U_OCRC5",T0."DocEntry",T0."DocNum",
T0."U_PDate" As "Date"
 From "@SAP_OIT" T0 
 Inner Join OBPL T1 On (T0."U_TB"=T1."BPLId" )
 Inner Join "@SAP_IT1" T2 On T0."DocEntry"=T2."DocEntry"
Where T0."DocEntry"=:DocEntry;



  
END;