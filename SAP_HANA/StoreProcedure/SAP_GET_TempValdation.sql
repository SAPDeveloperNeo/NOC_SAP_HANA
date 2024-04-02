Alter PROCEDURE SAP_GET_TempValdation 
(
 IN ItemCode NVARCHAR(100),
 IN WhsCode NVARCHAR(100),
 IN Date1 Date 
 
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
-- read stock from the database
BEGIN


IF (Select IFNULL("U_Category",'') from OWHS where "WhsCode"=:WhsCode)='GIT' 
Then 
Select 1 As "Count" from dummy ;
Else 
Select Count(*) As "Count" From "@SAP_OTMD"  T0 Inner Join "@SAP_TMD1" T1 On T0."DocEntry"=T1."DocEntry"
Where T1."U_ItemCode"=:ItemCode And T1."U_WhsCode"=:WhsCode And T0."U_PDate"=:Date1
And T0."U_Status"='O'
;
End IF ;




END;

