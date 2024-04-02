DROP PROCEDURE SAP_GETDATA_AP_VatHold_JE;
Create PROCEDURE SAP_GETDATA_AP_VatHold_JE
(
 IN DocEntry NVARCHAR(100)
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
-- read stock from the database
BEGIN

Select (Sum(T1."VatSum")*"U_Per")/100 As "Amt",T0."BPLId"  As "Branch",
T0."DocDate" As "Date" ,T0."DocEntry",T0."DocNum",T2."Name" As "CreditAccount",
T0."CardCode" As "DebitAccount", T3."DebPayAcct" As "ShortName"
From OPCH T0 Inner Join PCH1 T1 On T0."DocEntry"=T1."DocEntry"
Inner Join "@SAP_OVH" T2 On T1."TaxCode"=T2."U_Tax"
Inner Join OCRD T3 On T3."CardCode"=T0."CardCode" 
Where T0."DocEntry"=:DocEntry And IFNULL(T0."U_TransId",'')=''
Group By T0."BPLId",T0."DocDate",T0."DocEntry",T0."DocNum",T2."Name",T0."CardCode",T3."DebPayAcct" ,"U_Per"
;
  
END;


Call SAP_GETDATA_AP_VatHold_JE ('206')