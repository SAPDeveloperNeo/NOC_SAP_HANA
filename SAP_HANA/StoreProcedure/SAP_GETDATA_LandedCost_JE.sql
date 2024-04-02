DROP PROCEDURE SAP_GETDATA_LandedCost_JE;

Create PROCEDURE SAP_GETDATA_LandedCost_JE 
(
 IN DocEntry NVARCHAR(100)
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
-- read stock from the database
BEGIN

SELECT Distinct T0."U_LCC", T0."U_Amt", T0."U_CAC" as "CreditAccount" ,
(Select "U_AC" from "@SAP_OAAM" Where  "U_TrnsType"='GR') As "DebitAccount",
T1."U_TB",
T2."U_OCRC", T2."U_OCRC2", T2."U_OCRC3", T2."U_OCRC4", T2."U_OCRC5",
(Select "DocNum" from "@SAP_OIT" X Where  X."DocEntry"=:DocEntry) As "DocNum",
:DocEntry As "DocEntry",
(Select "U_PDate" from "@SAP_OIT" X Where  X."DocEntry"=:DocEntry) As "Date"
FROM  "@SAP_IT2"  T0 Inner Join "@SAP_OIT" T1 On T0."DocEntry"=T1."DocEntry" 
Inner Join "@SAP_IT1" T2 On T0."DocEntry"=T2."DocEntry"
WHERE T0."DocEntry" =(Select "U_ITDocE" from "@SAP_OIT" X Where  X."DocEntry"=:DocEntry)
And T0."U_Amt">0;
  
END;


Call SAP_GETDATA_LandedCost_JE (44) ;



