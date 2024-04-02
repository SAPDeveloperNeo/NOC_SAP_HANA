



DROP PROCEDURE SAP_GET_Auto_PurchaseOrder;

Create PROCEDURE SAP_GET_Auto_PurchaseOrder 

LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS

-- read stock from the database
BEGIN

SELECT T0."DocEntry",T0."DocNum" FROM OPOR T0 WHERE IFNULL(T0."U_ITDocE",0)=0 and  T0."CardCode" like 'IV%';


END;


CALL SAP_GET_Auto_PurchaseOrder ;