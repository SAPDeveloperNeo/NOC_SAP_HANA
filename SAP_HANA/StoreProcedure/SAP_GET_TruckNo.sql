Alter PROCEDURE SAP_GET_TruckNo 
(
 IN IType NVARCHAR(5),
 IN FBranch NVARCHAR(50),
 IN FWHSCODE NVARCHAR(50),
 IN IRDE NVARCHAR(50)
 )
LANGUAGE SQLSCRIPT 
--SQL SECURITY INVOKER

--READS SQL DATA
AS
-- read stock from the database
BEGIN

If :IType<>'TP'
Then 


IF :IRDE<>''
THEN


		Select Distinct T0."U_TRegNo",case when T5."U_Status" ='F' then 'Main Tank' else 'Hold' end
		 From "@SAP_OTM"  T0 Inner Join "@SAP_TM2" T1 On  T0."DocEntry"=T1."DocEntry"
		Inner Join OPRC T3 On "PrcCode"=T1."U_Route"
		Inner Join OBPL T4 On T4."BPLName"=T3."U_Branch" 
		inner join "@TRUCK_STATUS"  T5 on T5."Code" = T0."U_TRegNo" and T5."U_Status" not in ('E' ,'T')
		inner join "OWHS" T6 on T6."WhsCode" = :FWHSCODE and
		T5."U_Status" in ('F' ,'D')
		Where T4."BPLId"=:FBranch And T0."U_TRegNo" In 	
									  (Select "U_TRNO" From "@SAP_ITR2" Where "DocEntry"=:IRDE);
Else 
		Select Distinct T0."U_TRegNo",case when T5."U_Status" ='F' then 'Main Tank' else 'Hold' end
		 From "@SAP_OTM"  T0 Inner Join "@SAP_TM2" T1 On  T0."DocEntry"=T1."DocEntry"
		Inner Join OPRC T3 On "PrcCode"=T1."U_Route"
		Inner Join OBPL T4 On T4."BPLName"=T3."U_Branch" 
		inner join "@TRUCK_STATUS"  T5 on T5."Code" = T0."U_TRegNo" and T5."U_Status" not in ('E' ,'T')
		inner join "OWHS" T6 on T6."WhsCode" = :FWHSCODE and
		T5."U_Status" in ('F' ,'D')
		Where T4."BPLId"=:FBranch ;
End If;


Else

	IF :IRDE<>''
	THEN
			Select Distinct T0."U_TRegNo" From "@SAP_OTM"  T0 Inner Join "@SAP_TM2" T1 On  T0."DocEntry"=T1."DocEntry"
			Inner Join OPRC T3 On "PrcCode"=T1."U_Route"
			Inner Join OBPL T4 On T4."BPLName"=T3."U_Branch" 
			Where T4."BPLId"=:FBranch And 
			T0."U_TRegNo" In (
			SELECT DISTINCT T0."Code" FROM "@TRUCK_STATUS"  T0 WHERE T0."U_Status" ='D' 
			And IFNULL("Code",'')<>'')And T0."U_TRegNo" In 	
									  (Select "U_TRNO" From "@SAP_ITR2" Where "DocEntry"=:IRDE);
	Else
	
			Select Distinct T0."U_TRegNo" From "@SAP_OTM"  T0 Inner Join "@SAP_TM2" T1 On  T0."DocEntry"=T1."DocEntry"
			Inner Join OPRC T3 On "PrcCode"=T1."U_Route"
			Inner Join OBPL T4 On T4."BPLName"=T3."U_Branch" 
			Where T4."BPLId"=:FBranch And 
			T0."U_TRegNo" In (
			SELECT DISTINCT T0."Code" FROM "@TRUCK_STATUS"  T0 WHERE T0."U_Status" ='D' 
			And IFNULL("Code",'')<>'');
	End If;





End If; 

 
END;


Call SAP_GET_TruckNo ('DE','13','AFSACC01','1');



