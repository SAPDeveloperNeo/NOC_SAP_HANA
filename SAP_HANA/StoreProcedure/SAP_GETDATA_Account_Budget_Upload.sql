DROP PROCEDURE SAP_GETDATA_Account_Budget_Upload;

Create PROCEDURE SAP_GETDATA_Account_Budget_Upload 
(
 IN DocEntry NVARCHAR(100)
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
-- read stock from the database
BEGIN



SELECT 
T0."DocEntry", T0."DocNum", T0."U_PDate", T0."U_Year", T0."U_Auth", 
T1."LineId", T1."U_Year", T1."U_BPLId", T1."U_BPLName", T1."U_CostCode", T1."U_ActCode", 
T1."U_Debit", T1."U_Credit", T1."U_Jan", T1."U_Feb", T1."U_Mar", T1."U_April", 
T1."U_May", T1."U_Jun", T1."U_Jul", T1."U_Agust", T1."U_Sep", T1."U_Oct", T1."U_Nov", 
T1."U_Dec", T1."U_Upload" 
FROM "@SAP_OABU"  T0 Inner Join "@SAP_ABU1"  T1 On T0."DocEntry"=T1."DocEntry"
Where T0."DocEntry"=:DocEntry And IFNULL(T0."U_Auth",'N')='Y'
And IFNULL(T1."U_Upload",'N') ='N';
 



  
END;


Call SAP_GETDATA_Account_Budget_Upload (1) ;



