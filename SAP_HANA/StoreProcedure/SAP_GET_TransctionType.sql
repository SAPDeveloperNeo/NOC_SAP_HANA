DROP PROCEDURE SAP_GET_TransctionType;

Create PROCEDURE SAP_GET_TransctionType 
(
 IN DocEntry NVARCHAR(100)
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
 

BEGIN


SELECT ( Case When IFNULL("U_ITRDocE",'')<>'' Then 'BOIR' 
		  When IFNULL("U_SGRNDE",'')<>'' Then 'BOGRN'  End ) As "TransType"
FROM "@SAP_OIT"  Where "DocEntry"=:DocEntry;

 
END;


Call SAP_GET_TransctionType (1) ;