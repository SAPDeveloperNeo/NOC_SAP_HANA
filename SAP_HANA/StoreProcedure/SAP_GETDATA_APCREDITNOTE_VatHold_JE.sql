DROP PROCEDURE SAP_GETDATA_APCREDITNOTE_VatHold_JE;
Create PROCEDURE SAP_GETDATA_APCREDITNOTE_VatHold_JE
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
T0."DocDate" As "Date" ,T0."DocEntry",T0."DocNum",T2."Name" As "DebitAccount",
T0."CardCode" As "CreditAccount"
From ORPC T0 Inner Join RPC1 T1 On T0."DocEntry"=T1."DocEntry"
Inner Join "@SAP_OVH" T2 On T1."TaxCode"=T2."U_Tax"
Inner  Join  PCH1 T3 On  T1."BaseEntry"=T3."DocEntry"  And  T1."BaseLine" =T3."LineNum"
Inner Join OPCH T4 On T4."DocEntry"=T3."DocEntry" And IFNULL(T4."U_TransId",'')<>''
Where T0."DocEntry"=:DocEntry 
And T0."U_TransId"=T4."U_TransId" and IFNULL(T0
."U_TransId",'')<>''
Group By T0."BPLId",T0."DocDate",T0."DocEntry",T0."DocNum",T2."Name",T0."CardCode","U_Per" ; 



  
END;


Call SAP_GETDATA_APCREDITNOTE_VatHold_JE ('151')