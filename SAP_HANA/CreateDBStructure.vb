Imports System.Text
Imports System.Xml
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Globalization
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Collections
Imports System.Diagnostics.CodeAnalysis

Namespace SAP_HANA
    Module CreateDBStructure


        Public oProgressBar As SAPbouiCOM.ProgressBar
        Dim Value As ArrayList = New ArrayList
        Dim defaultValue As String = ""
        Dim RetVal As Integer = Nothing

        <DebuggerDisplay("Value : {sValue}, Description : {sDescription}")>
        Structure ValidValues
            Dim sValue As String
            Dim sDescription As String
            Public Sub New(ByVal Value As String, ByVal Description As String)
                sValue = Value
                sDescription = Description
            End Sub

            Public Property Description() As String
                Get
                    Return sDescription
                End Get
                Set(ByVal value As String)
                    sDescription = value
                End Set
            End Property
            Public Property Value() As String
                Get
                    Return sValue
                End Get
                Set(ByVal value As String)
                    sValue = value
                End Set
            End Property

        End Structure

        Public Sub CreateUDOTable()
            Try
                oProgressBar = Nothing
                oProgressBar = SAPApplication.StatusBar.CreateProgressBar("Creating SAP add-on UDT, UDF, UDO. Please wait.", 5, False)
                oProgressBar.Value = 0
                oProgressBar.Maximum = 25

                ''Employee Salary Structure

                oProgressBar.Value = oProgressBar.Maximum - 1
                oProgressBar.Value = oProgressBar.Value + 2
                SAPB1_Fileds()
                Inventory_Transfer()
                VatHold()

                Account_Budget_Upload()
                Procurement_Plan_Budget_upload()
                TempMaster()

                SAPB1_Fileds()
                UserRghts_Configration()
                oProgressBar.Value = oProgressBar.Value + 2
                ChamberAllocation()
                ChamberInfo()
                Import()
                Ledger_Basic_Setup()
                Item_Basic_Setup()
                oProgressBar.Value = oProgressBar.Value + 2
                TruckRoutChange()
                TruckOwnerChange()
                TTPunishment()



                Applicable_Charges()
                oProgressBar.Value = oProgressBar.Value + 2
                Inventory_Transfer_Request()
                oProgressBar.Value = oProgressBar.Value + 2
                ' Inventory_Transfer()

                oProgressBar.Value = oProgressBar.Value + 2
                Document_Master()

                oProgressBar.Value = oProgressBar.Value + 2
                TruckMaster()
                oProgressBar.Value = oProgressBar.Value + 2
                ASTMProcess()
                'ASTMCalculation()
                oProgressBar.Value = oProgressBar.Value + 2
                Consumer()
                oProgressBar.Value = oProgressBar.Value + 2
                CalibrationProcess()
                Calibration_details()

                AccountMappping()



                oProgressBar.Value = oProgressBar.Maximum - 1
                oProgressBar.Stop()

                System.Runtime.InteropServices.Marshal.ReleaseComObject(oProgressBar)
                oProgressBar = Nothing
            Catch ex As Exception
                oProgressBar.Value = oProgressBar.Maximum - 1
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oProgressBar)
                oProgressBar.Stop()
                oProgressBar = Nothing
                SAPApplication.MessageBox("[CreateDB]-" & ex.Message, 1, "Ok", )
            End Try
        End Sub

        Private Sub SAPB1_Fileds()


            AddFields("OPCH", "TransId", "TransId", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("OBPL", "BFL", "GL Code", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("OJDT", "ITDocE", "Inv Trans  DocEntry", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("OJDT", "ITDocN", "Inv Trans  DocNum", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("OJDT", "Flag", "Flag", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("OWTR", "ITDocE", "Inv Trans  DocEntry", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("OWTR", "ITDocN", "Inv Trans  DocNum", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("OITM", "PF", "PF", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Quantity, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("OWTR", "Flag", "Flag", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("PDN1", "Temp", "Temp", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("PDN1", "Density", "Density", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("PDN1", "Dip", "Dip", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)


            AddFields("PDN1", "BC", "Bright & Clear", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, Nothing, Nothing, Nothing)
            AddFields("PDN1", "WN", "Water Nil", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, Nothing, Nothing, Nothing)
            AddFields("PDN1", "SN", "Sediments Nil", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, Nothing, Nothing, Nothing)
            AddFields("PDN1", "CWSP", "Colour (WW/S/PY)", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, Nothing, Nothing, Nothing)

            AddFields("PDN1", "Density2", "Density @ 15 °C (kg/m³)", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("PDN1", "DVari", "Density Variation", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("PDN1", "FP", "Flash Point", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)

            AddFields("OINV", "PSFJE", "PSF JE Number", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)



            Value = New ArrayList
            Value.Add(New ValidValues("1", "One"))
            Value.Add(New ValidValues("2", "Two"))
            Value.Add(New ValidValues("3", "Three"))
            Value.Add(New ValidValues("4", "Four"))
            Value.Add(New ValidValues("5", "Five"))
            AddFields("PDN1", "Chamber", "Chamber", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 2, Value, "1", Nothing, Nothing)

            Value = New ArrayList
            Value.Add(New ValidValues("Pipe", "Pipe"))
            Value.Add(New ValidValues("Road", "Road"))

            AddFields("OWHS", "TrsfMode", "Transfer Mode", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 5, Value, "Road", Nothing, Nothing)

            Value = New ArrayList
            Value.Add(New ValidValues("Y", "Yes"))
            Value.Add(New ValidValues("N", "NO"))

            AddFields("OITM", "CDR", "Calibration", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 2, Value, "Y", Nothing, Nothing)
            AddFields("OWHS", "DipReq", "DipReq", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 2, Value, "Y", Nothing, Nothing)
            AddFields("OITM", "Route", "Route Code", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("OITM", "Temp", "Temp Required", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Nothing, Nothing, Nothing, Nothing)

            AddFields("OPOR", "TCode", "Transporter Code", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("OPOR", "TName", "Transporter Name", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("OPOR", "TRNO", "Truck No", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("OPOR", "DRName", "Driver Name", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)

            AddFields("ODLN", "CMDocNum", "Chamber DocNum", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("ODLN", "CMDocEntry", "Chamber DocEntry", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)

            AddFields("OUSR", "Location", "Location", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)

            Value = New ArrayList
            Value.Add(New ValidValues("Y", "Yes"))
            Value.Add(New ValidValues("N", "No"))

            AddFields("OUSR", "A", "Temp Master Applicable", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, Nothing, Nothing, Nothing)

            Value = New ArrayList
            Value.Add(New ValidValues("DE", "Decant"))
            Value.Add(New ValidValues("DI", "Divert"))
            Value.Add(New ValidValues("TP", "Topping"))
            Value.Add(New ValidValues("S", "Sample"))


            AddFields("OPKL", "ITT", " Inventory Transfer Type", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 2, Value, "", Nothing, Nothing)
            AddFields("OPKL", "TRNO", "Truck No", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("OPKL", "CMDocNum", "Chamber DocNum", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("OPKL", "CMDocEntry", "Chamber DocEntry", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("OPKL", "CMDocEntry", "Chamber DocEntry", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("OIPF", "Qty", "TotalQty", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Quantity, 20, Nothing, Nothing, Nothing, Nothing)

        End Sub

        Private Sub Applicable_Charges()

            AddTables("SAP_OAC", "Applicable Charges", SAPbobsCOM.BoUTBTableType.bott_MasterData)

            AddFields("SAP_OAC", "LCN", "Landed Cost Name", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OAC", "CAC", "Credit Account Code", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OAC", "PO", "PO  Required", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Nothing, Nothing, Nothing, Nothing)

            AddFields("SAP_OAC", "ItemCode", "ItemCode", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OAC", "ItemName", "ItemName", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)



            '--Register Table 
            If -1 = RegisterObject("SAP_UDO_OAC", "Applicable Charges", "MD", "SAP_OAC", "", False, False, True, False, True, False, False, "", True, "Code,Name,U_LCN,U_CAC", True) Then SAPApplication.StatusBar.SetText("Object Registration failed.....", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub


        End Sub

        Private Sub Inventory_Transfer_Request()

            '--Create Master Table Inventory_Transfer_Request 
            AddTables("SAP_OITR", "Inv_Trans_Req", SAPbobsCOM.BoUTBTableType.bott_Document)

            '--Create Fields for Inventory_Transfer_Request
            AddFields("SAP_OITR", "Series", "Series", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 200, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OITR", "PDate", "Posting Date", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OITR", "DDate", "Document Date", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OITR", "FB", "From Branch", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OITR", "FW", "From Warehouse", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OITR", "TB", "To Branch", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OITR", "TW", "To Warehouse", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OITR", "Route", "Route", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 200, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OITR", "Remark", "Remark", SAPbobsCOM.BoFieldTypes.db_Memo, SAPbobsCOM.BoFldSubTypes.st_None, 200, Nothing, Nothing, Nothing, Nothing)

            AddFields("SAP_OITR", "FBN", "From Branch N", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OITR", "FWN", "From Warehouse N", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OITR", "TBN", "To Branch N", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OITR", "TWN", "To Warehouse N", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)




            Value = New ArrayList
            Value.Add(New ValidValues("DE", "Decant"))
            Value.Add(New ValidValues("DI", "Divert"))
            Value.Add(New ValidValues("S", "Select"))

            AddFields("SAP_OITR", "ITT", " Inventory Transfer Type", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 2, Value, "S", Nothing, Nothing)


            AddFields("SAP_OITR", "ITDocE", "Inv Trans DocEntry", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OITR", "ITDocN", "Inv Trans DocNum", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)

            Value = New ArrayList
            Value.Add(New ValidValues("CL", "Close"))
            Value.Add(New ValidValues("CA", "Cancle"))
            Value.Add(New ValidValues("S", "Select"))

            AddFields("SAP_OITR", "Status", "Status", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 2, Value, "S", Nothing, Nothing)


            AddTables("SAP_ITR1", "Inv_Trans_Req_Row", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)


            AddFields("SAP_ITR1", "ItemCode", "ItemCode", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ITR1", "ItemName", "ItemName", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 200, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ITR1", "FW", "FromWarehouse", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ITR1", "TW", "ToWarehouse", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ITR1", "Qty", "Quantity", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Quantity, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ITR1", "UOM", "UOM", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 200, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ITR1", "UOMG", "UOM Group", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 200, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ITR1", "Temp", "Temp", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ITR1", "Density", "Density", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ITR1", "OCRC", "Profit Center", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 30, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ITR1", "OCRC2", "Department", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 30, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ITR1", "OCRC3", "SalesLocation/Warehouse", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 30, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ITR1", "OCRC4", "Employee", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 30, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ITR1", "OCRC5", "Others", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 30, Nothing, Nothing, Nothing, Nothing)

            AddTables("SAP_ITR2", "Inv_Trans_Req_Row2", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)
            AddFields("SAP_ITR2", "TRNO", "Truck Number", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)


            If -1 = RegisterObject("SAP_UDO_OITR", "Inv_Trans_Req", "DT", "SAP_OITR", "SAP_ITR1,SAP_ITR2", False, False, False, True, True, False, True, "DocNum,DocEntry", False, "", True) Then SAPApplication.StatusBar.SetText("Object Registration failed.....", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub

        End Sub


        Private Sub Inventory_Transfer()

            '--Create Master Table Inventory_Transfer
            AddTables("SAP_OIT", "Inv_Trans", SAPbobsCOM.BoUTBTableType.bott_Document)

            '--Create Fields for Inventory_Transfer


            Value = New ArrayList
            Value.Add(New ValidValues("Y", "Yes"))
            Value.Add(New ValidValues("N", "No"))
            AddFields("SAP_OIT", "PFlag", "Post Flag", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, Nothing, Nothing, Nothing)

            AddFields("SAP_OIT", "Series", "Series", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 200, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "PDate", "Posting Date", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "DDate", "Document Date", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "FB", "From Branch", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "FW", "From Warehouse", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "TB", "To Branch", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "TW", "To Warehouse", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "FBN", "From Branch N", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "FWN", "From Warehouse N", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "TBN", "To Branch N", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "TWN", "To Warehouse N", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "Route", "Route", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 200, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "Route", "Route", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 200, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "Remark", "Remark", SAPbobsCOM.BoFieldTypes.db_Memo, SAPbobsCOM.BoFldSubTypes.st_None, 200, Nothing, Nothing, Nothing, Nothing)

            AddFields("SAP_OIT", "ITRDocE", "Inv Trans Req DocEntry", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "ITRDocN", "Inv Trans Req DocNum", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)

            AddFields("SAP_OIT", "ITDocE", "Inv Trans  DocEntry", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "ITDocN", "Inv Trans  DocNum", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "Ref", "Ref No", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 200, Nothing, Nothing, Nothing, Nothing)




            Value = New ArrayList
            Value.Add(New ValidValues("CL", "Close"))
            Value.Add(New ValidValues("CA", "Cancle"))
            Value.Add(New ValidValues("S", "Select"))

            AddFields("SAP_OIT", "Status", "Status", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 2, Value, "S", Nothing, Nothing)

            AddFields("SAP_OIT", "TCode", "Transporter Code", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "TName", "Transporter Name", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "TRNO", "Truck No", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "DRName", "Driver Name", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)

            Value = New ArrayList
            Value.Add(New ValidValues("DE", "Decant"))
            Value.Add(New ValidValues("DI", "Divert"))
            Value.Add(New ValidValues("TP", "Topping"))
            Value.Add(New ValidValues("S", "Sample"))


            AddFields("SAP_OIT", "ITT", " Inventory Transfer Type", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 2, Value, "DE", Nothing, Nothing)

            '''''''''''''''''''

            AddFields("SAP_OIT", "QCNO", "QCNO", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)

            AddFields("SAP_OIT", "ITDE", "Inv Tran DocEntry", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "ITDN", "Inv Tran DocNum", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)



            AddFields("SAP_OIT", "PODE", "PO DocEntry", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "PODN", "PO DocNum", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)

            AddFields("SAP_OIT", "GRNDE", "GRN DocEntry", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "GRNDN", "GRN DocNum", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)

            AddFields("SAP_OIT", "GIDE", "GI DocEntry", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "GIDN", "GI DocNum", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)

            AddFields("SAP_OIT", "GRDE", "GR DocEntry", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "GRDN", "GR DocNum", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)

            AddFields("SAP_OIT", "JE", "Landed Cost JE", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "AJE1", "Adjustment JE1", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "AJE2", "Adjustment JE2", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)



            'AddFields("SAP_OIT", "AmtCHL", "ChamberLoss ", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            'AddFields("SAP_OIT", "AmtTTL", "Trans Temp Loss ", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)

            AddFields("SAP_OIT", "TGIDE", " Trans Loss GI DocEntry", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "TGIDN", " Trans Loss GI DocNum", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)

            AddFields("SAP_OIT", "TRGIDE", " Tolarance Loss GI DocEntry", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "TRGIDN", " Tolarance Loss GI DocNum", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)



            ' AddFields("SAP_OIT", "AmtCL", "Claimable Loss ", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)

            AddFields("SAP_OIT", "ARDE", "PI DocEntry", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "ARDN", "PI DocNum", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)

            AddFields("SAP_OIT", "SGRNDE", "System GRN DocEntry", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "SGRNDN", "System GRN DocNum", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)


            AddFields("SAP_OIT", "TTLGIDE", " Tank Temp GI DocEntry", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "TTLGIDN", " Tank Temp GI DocNum", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)

            Value = New ArrayList
            Value.Add(New ValidValues("I", "Import"))
            Value.Add(New ValidValues("S", "Stock Transfer"))
            AddFields("SAP_OIT", "STT", "Stock Transfer Type", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 2, Value, "", Nothing, Nothing)

            Value = New ArrayList
            Value.Add(New ValidValues("N", "NA"))
            Value.Add(New ValidValues("A", "QC Approved"))
            Value.Add(New ValidValues("H", "QC Hold"))
            Value.Add(New ValidValues("R", "QC Reject"))
            AddFields("SAP_OIT", "QC", "QC Status", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 2, Value, "N", Nothing, Nothing)
            AddFields("SAP_OIT", "Temp", "Observ Temp", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "Density", "Observ Density", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "Batch", "Batch", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "FBP", "Final Boiling Point", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)

            AddFields("SAP_OIT", "SQty", "Sum Quantity", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Quantity, 20, Nothing, Nothing, Nothing, Nothing)


            AddFields("SAP_OIT", "DispTemp", "Dispatch Temp", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "DiffTemp", "Diff Temp", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "DipDiffS", "Dip Diff Sum", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIT", "TotalQty", "TotalQty", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)


            ''''''''''''''' Row
            AddTables("SAP_IT1", "Inv_Trans_Row", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            AddFields("SAP_IT1", "ItemCode", "ItemCode", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT1", "ItemName", "ItemName", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 200, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT1", "FW", "FromWarehouse", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT1", "TW", "ToWarehouse", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT1", "Qty", "Quantity", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Quantity, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT1", "UOM", "UOM", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 200, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT1", "UOMG", "UOM Group", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 200, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT1", "Temp", "Observ Temp", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT1", "Density", "Observ Density", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT1", "Dip", "Dip", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT1", "DipDiff", "Dip Difference", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT1", "ODip", " Oil Dip", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            Value = New ArrayList
            Value.Add(New ValidValues("1", "One"))
            Value.Add(New ValidValues("2", "Two"))
            Value.Add(New ValidValues("3", "Three"))
            Value.Add(New ValidValues("4", "Four"))
            Value.Add(New ValidValues("5", "Five"))
            AddFields("SAP_IT1", "Chamber", "Chamber", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 2, Value, "1", Nothing, Nothing)
            AddFields("SAP_IT1", "OCRC", "Profit Center", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 30, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT1", "OCRC2", "Department", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 30, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT1", "OCRC3", "SalesLocation/Warehouse", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 30, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT1", "OCRC4", "Employee", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 30, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT1", "OCRC5", "Others", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 30, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT1", "Batch", "Batch", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)


            Value = New ArrayList
            Value.Add(New ValidValues("Y", "Yes"))
            Value.Add(New ValidValues("N", "No"))

            AddFields("SAP_IT1", "BC", "Bright & Clear", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, Nothing, Nothing, Nothing)
            AddFields("SAP_IT1", "WN", "Water Nil", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, Nothing, Nothing, Nothing)
            AddFields("SAP_IT1", "SN", "Sediments Nil", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, Nothing, Nothing, Nothing)
            AddFields("SAP_IT1", "CWSP", "Colour (WW/S/PY)", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, Nothing, Nothing, Nothing)

            AddFields("SAP_IT1", "Density2", "Density @ 15 °C (kg/m³)", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT1", "DVari", "Density Variation", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT1", "FP", "Flash Point", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT1", "FBP", "Final Boiling Point", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)

            '''''''''''''

            ''''''''''''''' Landed Cost
            AddTables("SAP_IT2", "Inv_Trans_Landed_Cost", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            AddFields("SAP_IT2", "LCC", "Landed Cost Code", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT2", "LCN", "Landed Cost Name", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT2", "Amt", "Amount", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT2", "CAC", " Credit Account Code", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT2", "ItemCode", "ItemCode", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT2", "ItemName", "ItemName", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT2", "CardCode", "CardCode", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT2", "CardName", "CardName", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT2", "PO", "PO Required", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT2", "PODE", "PO DocEntry", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT2", "PODN", "PO DocNum", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Nothing, Nothing, Nothing, Nothing)


            '''''''''''''

            ''''''''''''''' Attachmnet 
            AddTables("SAP_IT3", "Inv_Trans_Attch", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)
            AddFields("SAP_IT3", "Attch", "Attachments", SAPbobsCOM.BoFieldTypes.db_Memo, SAPbobsCOM.BoFldSubTypes.st_Link, 250, Nothing, Nothing, Nothing, Nothing)
            '''''''''

            '''''''''''''''Loss Calculation
            AddTables("SAP_IT4", "Inv_Trans_Loss_Cal", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            AddFields("SAP_IT4", "ItemCode", "ItemCode", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            Value = New ArrayList
            Value.Add(New ValidValues("1", "One"))
            Value.Add(New ValidValues("2", "Two"))
            Value.Add(New ValidValues("3", "Three"))
            Value.Add(New ValidValues("4", "Four"))
            Value.Add(New ValidValues("5", "Five"))
            AddFields("SAP_IT4", "Chamber", "Chamber", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 2, Value, "1", Nothing, Nothing)
            AddFields("SAP_IT4", "ChemLos", "Chember Loss", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT4", "TemLoss", "Temp Loss", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT4", "ClLoss", "Claimable Loss", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_IT4", "TTL", "Tank Temp Loss", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)

            '''''''''''''





            If -1 = RegisterObject("SAP_UDO_OIT", "Inv_Trans", "DT", "SAP_OIT", "SAP_IT1,SAP_IT2,SAP_IT3,SAP_IT4", False, False, False, True, True, False, True, "DocNum,DocEntry", False, "", True) Then SAPApplication.StatusBar.SetText("Object Registration failed.....", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub

        End Sub

        Private Sub Document_Master()

            AddTables("SAP_ODM", "Document_Master", SAPbobsCOM.BoUTBTableType.bott_MasterData)

            Value = New ArrayList
            Value.Add(New ValidValues("Y", "Yes"))
            Value.Add(New ValidValues("N", "No"))

            AddFields("SAP_ODM", "VR", "Validation  Required", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, Nothing, Nothing, Nothing)
            AddFields("SAP_ODM", "LD", "Lead days", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Quantity, 20, Nothing, Nothing, Nothing, Nothing)

            '--Register Table 
            If -1 = RegisterObject("SAP_UDO_ODM", "Document_Master", "MD", "SAP_ODM", "", False, False, True, False, True, False, False, "Code,Name", True, "Code,Name", True) Then SAPApplication.StatusBar.SetText("Object Registration failed.....", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub


        End Sub

        Private Sub TruckMaster()

            '--Create Master Table TruckMaster 
            AddTables("SAP_OTM", "TruckMaster", SAPbobsCOM.BoUTBTableType.bott_Document)

            '--Create Fields for TruckMaster

            AddFields("SAP_OTM", "Date", " Date", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTM", "TRegNo", "Truck Reg No", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTM", "Trailor", "Trailor No", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTM", "Owner", "Owner", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 200, Nothing, Nothing, Nothing, Nothing)
            'AddFields("SAP_OTM", "Route", "Route", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 200, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTM", "Driver", "Driver Name", SAPbobsCOM.BoFieldTypes.db_Memo, SAPbobsCOM.BoFldSubTypes.st_None, 200, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTM", "CardCode", "CardCode", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTM", "CardName", "CardName", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)



            Value = New ArrayList
            Value.Add(New ValidValues("ATF", "ATF"))
            Value.Add(New ValidValues("NonATF", "NonATF"))
            Value.Add(New ValidValues("LPG", "LPG"))
            Value.Add(New ValidValues("Others", "Others"))
            AddFields("SAP_OTM", "Product", " Product Type", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Value, Nothing, Nothing, Nothing)
            AddFields("SAP_OTM", "Active", "Active", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Nothing, Nothing, Nothing, Nothing)

            AddFields("SAP_OTM", "ModelNum", "Model Number", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 4, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTM", "TType", "Vehicle Type", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTM", "Enum", "Engine Number", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 30, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTM", "ChNum", "Chassis Number", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 30, Nothing, Nothing, Nothing, Nothing)

            AddFields("SAP_OTM", "OTT", "Old TT Number", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 30, Nothing, Nothing, Nothing, Nothing)






            AddTables("SAP_TM1", "TruckMaster Row", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)


            AddFields("SAP_TM1", "DocCat", "Document Category", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TM1", "VDate", " Valid From", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TM1", "TDate", " Valid To", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TM1", "Attch", "Attachments", SAPbobsCOM.BoFieldTypes.db_Memo, SAPbobsCOM.BoFldSubTypes.st_Link, 250, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TM1", "Cali", "Calibration", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)

            AddTables("SAP_TM2", "TruckMaster Route", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)
            AddFields("SAP_TM2", "Route", "Route", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)

            If -1 = RegisterObject("SAP_UDO_OOTM", "TruckMaster", "DT", "SAP_OTM", "SAP_TM1,SAP_TM2", False, False, False, True, True, False, True, "DocNum,DocEntry", False, "", True) Then SAPApplication.StatusBar.SetText("Object Registration failed.....", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub

        End Sub

        Private Sub ASTMProcess()

            '--Create Master Table ASTM Process 
            AddTables("SAP_OASTM", "ASTM Process", SAPbobsCOM.BoUTBTableType.bott_Document)

            '--Create Fields for ASTM Process

            AddFields("SAP_OASTM", "Date", " Date", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OASTM", "ItemCode", "ItemCode", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OASTM", "ItemName", "Trailor No", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OASTM", "Temp", "Temp ", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)


            AddTables("SAP_ASTM1", "ASTM Process Row", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            AddFields("SAP_ASTM1", "Temp", "Temp ", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ASTM1", "ObValue", "Observe Value ", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ASTM1", "Density", "Density ", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)


            If -1 = RegisterObject("SAP_UDO_OASTM", "ASTMProcess", "DT", "SAP_OASTM", "SAP_ASTM1", False, False, False, True, True, False, True, "DocNum,DocEntry", False, "", True) Then SAPApplication.StatusBar.SetText("Object Registration failed.....", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub

        End Sub

        Private Sub Consumer()
            '--Create Master Table Consumer  
            AddTables("SAP_OCSM", "Consumer ", SAPbobsCOM.BoUTBTableType.bott_Document)
            '--Create Fields for Consumer

            AddFields("SAP_OCSM", "CardCode", "CardCode", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OCSM", "CardName", "CardName", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OCSM", "NID", "National ID", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)

            AddFields("SAP_OCSM", "FatherN", "Father Name", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OCSM", "Mob", "Contact No", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OCSM", "Email", "Email Id", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OCSM", "Address", "Address", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 250, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OCSM", "BPLID", "BPLID", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OCSM", "Branch", "Branch", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OCSM", "Province", "Province", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OCSM", "Zone", "Zone", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OCSM", "District", "District", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OCSM", "Municipality", "Municipality", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OCSM", "Ward", "Ward Number", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)

            Value = New ArrayList
            Value.Add(New ValidValues("Business", "Business"))
            Value.Add(New ValidValues("Personal", "Personal"))
            AddFields("SAP_OCSM", "UType", " Usage Type", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Value, Nothing, Nothing, Nothing)

            If -1 = RegisterObject("SAP_UDO_OCSM", "Consumer", "DT", "SAP_OCSM", "", False, False, False, True, True, False, True, "DocNum,DocEntry", False, "", True) Then SAPApplication.StatusBar.SetText("Object Registration failed.....", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub

        End Sub

        Private Sub Calibration_details()

            '--Create Master Table Calibration_details 
            AddTables("SAP_OCALD", "Calibration_Details", SAPbobsCOM.BoUTBTableType.bott_Document)

            '--Create Fields for Calibration_details

            AddFields("SAP_OCALD", "Date", " Date", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)


            AddTables("SAP_CALD1", "Calibration_details Row", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            AddFields("SAP_CALD1", "Hight", "Hight ", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_CALD1", "Qty", "Qty", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)



            If -1 = RegisterObject("SAP_UDO_OCALD", "Calibration_Details", "DT", "SAP_OCALD", "SAP_CALD1", False, False, False, True, True, False, True, "DocNum,DocEntry", False, "", True) Then SAPApplication.StatusBar.SetText("Object Registration failed.....", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub

        End Sub

        Private Sub CalibrationProcess()

            '--Create Master Table CalibrationProcess 
            AddTables("SAP_OCALP", "CalibrationProcess", SAPbobsCOM.BoUTBTableType.bott_Document)

            '--Create Fields for CalibrationProcess
            AddFields("SAP_OCALP", "FAC", "Fixed Assets", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OCALP", "Name", "Name", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OCALP", "VC", "Vehical Code", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OCALP", "PDate", "Posting Date", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OCALP", "CDate", "Calibration  Date", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OCALP", "CEDate", "Calibration Exp Date", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OCALP", "TS", "Tire Size ", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OCALP", "temp", "temp", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OCALP", "TP", "Tire Pressure", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OCALP", "TC", "Total Chamber", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)

            AddFields("SAP_OCALP", "TCAP", "Total Capacity", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)

            Value = New ArrayList
            Value.Add(New ValidValues("Active", "Active"))
            Value.Add(New ValidValues("Close", "Close"))
            Value.Add(New ValidValues("Cancle", "Cancle"))
            AddFields("SAP_OCALP", "Status", "Status", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Value, Nothing, Nothing, Nothing)

            '--Create Fields for CalibrationProcess Row

            AddTables("SAP_CALP1", "CalibrationProcess Row", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)


            AddFields("SAP_CALP1", "CHN", "Chamber No", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)

            AddFields("SAP_CALP1", "CAP", "Capacity", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_CALP1", "OVRDIP", "Over Dip ", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_CALP1", "OILDIP", "Oil Dip", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            ' AddFields("SAP_OCALP", "CDocNum", "Calibration DocNum", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            ' AddFields("SAP_OCALP", "CDocEntry", "Calibration DocEntry", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)


            AddTables("SAP_CALP2", "Calibration_details Row", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            AddFields("SAP_CALP2", "Hight", "Hight ", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_CALP2", "Qty", "Qty", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_CALP2", "CHN", "Chamber No", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)

            AddTables("SAP_CALP3", "Calibration_details Oli", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            AddFields("SAP_CALP3", "Hight", "Hight ", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_CALP3", "Qty", "Qty", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_CALP3", "CHN", "Chamber No", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)






            If -1 = RegisterObject("SAP_UDO_OCALP", "CalibrationProcess", "DT", "SAP_OCALP", "SAP_CALP1,SAP_CALP2,SAP_CALP3", False, False, False, True, True, False, True, "DocNum,DocEntry", False, "", True) Then SAPApplication.StatusBar.SetText("Object Registration failed.....", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub

        End Sub

        Private Sub TempMaster()
            '--Create Master Table TempMaster  
            AddTables("SAP_OTMD", "TempMaster ", SAPbobsCOM.BoUTBTableType.bott_Document)
            '--Create Fields for TempMaster
            AddFields("SAP_OTMD", "LocCode", "LocCode", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTMD", "LocName", "LocName", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTMD", "PDate", "Posting Date", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTMD", "Time", "Time ", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Time, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTMD", "BPLId", "BPLId", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTMD", "BPLName", "BPLName", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTMD", "Out", "OutTemp", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTMD", "In", "InTemp", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTMD", "Density", "Density", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)

            Value = New ArrayList
            Value.Add(New ValidValues("O", "Opening"))
            Value.Add(New ValidValues("C", "Closeing"))
            Value.Add(New ValidValues("OTH", "Other"))
            Value.Add(New ValidValues("S", "--Select--"))

            AddFields("SAP_OTMD", "Status", "Status", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 3, Value, "S", Nothing, Nothing)



            AddTables("SAP_TMD1", "TempMaster ", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            AddFields("SAP_TMD1", "ItemCode", "ItemCode", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TMD1", "ItemName", "ItemName", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)


            AddFields("SAP_TMD1", "WhsCode", "WhsCode", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TMD1", "WhsName", "WhsName", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TMD1", "Location", "Location", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TMD1", "Time", "Time ", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Time, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TMD1", "Out", "OutTemp", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TMD1", "In", "InTemp", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TMD1", "DIP", " Dip ", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TMD1", "WDIP", "Water Dip ", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TMD1", "ODIP", "Oil Dip ", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TMD1", "Density", "Density", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TMD1", "PDate", "Posting Date", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TMD1", "OAQty", "Over All Qty", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Quantity, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TMD1", "WQty", "Water Qty", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Quantity, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TMD1", "CalQty", "CalQty", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Quantity, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TMD1", "SAPQty", "CalQty", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Quantity, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TMD1", "Diff", "Difference", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Quantity, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TMD1", "UOM", "UOM", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)




            If -1 = RegisterObject("SAP_UDO_OTMD", "TempMaster", "DT", "SAP_OTMD", "SAP_TMD1", False, False, False, True, True, False, True, "DocNum,DocEntry", False, "", True) Then SAPApplication.StatusBar.SetText("Object Registration failed.....", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub

        End Sub


        Private Sub AccountMappping()

            AddTables("SAP_OAAM", "AccountMappping", SAPbobsCOM.BoUTBTableType.bott_MasterData)
            AddFields("SAP_OAAM", "AC", "Account Code", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)


            Value = New ArrayList
            Value.Add(New ValidValues("GR", "Good Receipt"))
            AddFields("SAP_OAAM", "TrnsType", "TrnsType", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 5, Value, Nothing, Nothing, Nothing)


            '--Register Table 
            If -1 = RegisterObject("SAP_UDO_OAAM", "AccountMappping", "MD", "SAP_OAAM", "", False, False, True, False, True, False, False, "", True, "Code,Name,U_AC,U_TrnsType", True) Then SAPApplication.StatusBar.SetText("Object Registration failed.....", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub


        End Sub


        Private Sub Account_Budget_Upload()

            '--Create Master Table Account_Budget_Upload 
            AddTables("SAP_OABU", "Account_Budget_Upload", SAPbobsCOM.BoUTBTableType.bott_Document)

            '--Create Fields for Account_Budget_Upload
            AddFields("SAP_OABU", "PDate", "Posting Date", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OABU", "Year", "Year", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OABU", "Auth", "Authorize", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Nothing, Nothing, Nothing, Nothing)




            AddTables("SAP_ABU1", "Account_Budget_Upload_Row", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            AddFields("SAP_ABU1", "Year", "Year", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ABU1", "BPLId", "Branch Code", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ABU1", "BPLName", "Branch Name", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ABU1", "CostCode", "cost center Code", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ABU1", "ActCode", "Account Code", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ABU1", "Debit", "Debit", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ABU1", "Credit", "Credit", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)

            AddFields("SAP_ABU1", "Jan", "Jan", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ABU1", "Feb", "Feb", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ABU1", "Mar", "Mar", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ABU1", "April", "April", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ABU1", "May", "May", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ABU1", "Jun", "Jun", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ABU1", "Jul", "Jul", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ABU1", "Agust", "Agust", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ABU1", "Sep", "Sep", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ABU1", "Oct", "Oct", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ABU1", "Nov", "Nov", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ABU1", "Dec", "Dec", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ABU1", "Year", "Year", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ABU1", "Upload", "Upload", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, Nothing, Nothing, Nothing)
            AddFields("SAP_ABU1", "Error", "Error Info", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 200, Nothing, Nothing, Nothing, Nothing)


            If -1 = RegisterObject("SAP_UDO_OABU", "Account_Budget_Upload", "DT", "SAP_OABU", "SAP_ABU1", False, False, False, True, True, False, True, "DocNum,DocEntry", False, "", True) Then SAPApplication.StatusBar.SetText("Object Registration failed.....", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub

        End Sub

        Private Sub Procurement_Plan_Budget_upload()

            '--Create Master Table Procurement_Plan_Budget_upload 
            AddTables("SAP_OPPBU", "Procurement_Plan_Budget_upload", SAPbobsCOM.BoUTBTableType.bott_Document)

            '--Create Fields for Procurement_Plan_Budget_upload
            AddFields("SAP_OPPBU", "PDate", "Posting Date", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OPPBU", "Year", "Year", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OPPBU", "Auth", "Authorize", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Nothing, Nothing, Nothing, Nothing)


            AddTables("SAP_PPBU", "Procurement_Plan_Budget_row", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            AddFields("SAP_PPBU", "Year", "Year", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PPBU", "BPLId", "Branch Code", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PPBU", "BPLName", "Branch Name", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PPBU", "CostCode", "cost center Code", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PPBU", "Project", "Project", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PPBU", "ItemCode", "Project", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PPBU", "ItemName", "Project", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 200, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PPBU", "Qty", "Qty", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Quantity, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PPBU", "Price", "Unit Price", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PPBU", "TAYear", "Total Year Amount", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)

            AddFields("SAP_PPBU", "Jan", "Jan", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PPBU", "Feb", "Feb", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PPBU", "Mar", "Mar", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PPBU", "April", "April", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PPBU", "May", "May", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PPBU", "Jun", "Jun", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PPBU", "Jul", "Jul", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PPBU", "Agust", "Agust", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PPBU", "Sep", "Sep", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PPBU", "Oct", "Oct", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PPBU", "Nov", "Nov", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PPBU", "Dec", "Dec", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Price, 20, Nothing, Nothing, Nothing, Nothing)

            AddFields("SAP_PPBU", "Upload", "Upload", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, Nothing, Nothing, Nothing)


            If -1 = RegisterObject("SAP_UDO_OPPBU", "Procurement_Plan_Budget_upload", "DT", "SAP_OPPBU", "SAP_PPBU", False, False, False, True, True, False, True, "DocNum,DocEntry", False, "", True) Then SAPApplication.StatusBar.SetText("Object Registration failed.....", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub

        End Sub



        Private Sub Ledger_Basic_Setup()

            AddTables("SAP_OLBS", "Ledger_Basic_Setup", SAPbobsCOM.BoUTBTableType.bott_MasterData)
            AddFields("SAP_OLBS", "BPLId", "Branch Code", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OLBS", "BPLName", "Branch Name", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)

            Value = New ArrayList
            Value.Add(New ValidValues("Y", "Yes"))
            Value.Add(New ValidValues("N", "No"))

            AddFields("SAP_OLBS", "OCRC", "Cost Center 1", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, Nothing, Nothing, Nothing)
            AddFields("SAP_OLBS", "OCRC2", "Cost Center 2", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, Nothing, Nothing, Nothing)
            AddFields("SAP_OLBS", "OCRC3", "Cost Center 3", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, Nothing, Nothing, Nothing)
            AddFields("SAP_OLBS", "OCRC4", "Cost Center 4", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, Nothing, Nothing, Nothing)
            AddFields("SAP_OLBS", "OCRC5", "Cost Center 5", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, Nothing, Nothing, Nothing)

            '--Register Table 
            If -1 = RegisterObject("SAP_UDO_OLBS", "Ledger_Basic_Setup", "MD", "SAP_OLBS", "", False, False, True, False, True, False, False, "", True, "Code,Name,U_BPLId,U_BPLName,U_OCRC,U_OCRC2,U_OCRC3,U_OCRC4,U_OCRC5", True) Then SAPApplication.StatusBar.SetText("Object Registration failed.....", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub


        End Sub

        Private Sub Item_Basic_Setup()

            AddTables("SAP_OIBS", "Item_Basic_Setup", SAPbobsCOM.BoUTBTableType.bott_MasterData)
            AddFields("SAP_OIBS", "BPLId", "Branch Code", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OIBS", "BPLName", "Branch Name", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)

            Value = New ArrayList
            Value.Add(New ValidValues("Y", "Yes"))
            Value.Add(New ValidValues("N", "No"))

            AddFields("SAP_OIBS", "OCRC", "Cost Center 1", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, Nothing, Nothing, Nothing)
            AddFields("SAP_OIBS", "OCRC2", "Cost Center 2", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, Nothing, Nothing, Nothing)
            AddFields("SAP_OIBS", "OCRC3", "Cost Center 3", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, Nothing, Nothing, Nothing)
            AddFields("SAP_OIBS", "OCRC4", "Cost Center 4", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, Nothing, Nothing, Nothing)
            AddFields("SAP_OIBS", "OCRC5", "Cost Center 5", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, Nothing, Nothing, Nothing)



            AddFields("SAP_OIBS", "Project", "Project", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Nothing, Nothing, Nothing, Nothing)


            Value = New ArrayList
            Value.Add(New ValidValues("Qty", "Quantity"))
            Value.Add(New ValidValues("Val", "Value"))
            AddFields("SAP_OIBS", "BOQV", "Quantity/Value", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 5, Value, Nothing, Nothing, Nothing)

            Value = New ArrayList
            Value.Add(New ValidValues("Y", "Yes"))
            Value.Add(New ValidValues("N", "No"))
            AddFields("SAP_OIBS", "BOS", "Budget for Sale", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, Nothing, Nothing, Nothing)

            Value = New ArrayList
            Value.Add(New ValidValues("Y", "Yes"))
            Value.Add(New ValidValues("N", "No"))
            AddFields("SAP_OIBS", "BOP", "Budget for Purchase", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, Nothing, Nothing, Nothing)


            '--Register Table 
            If -1 = RegisterObject("SAP_UDO_OIBS", "Item_Basic_Setup", "MD", "SAP_OIBS", "", False, False, True, False, True, False, False, "", True, "Code,Name,U_BPLId,U_BPLName,U_OCRC,U_OCRC2,U_OCRC3,U_OCRC4,U_OCRC5,U_Project,U_BOQV,U_BOS,U_BOP", True) Then SAPApplication.StatusBar.SetText("Object Registration failed.....", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub


        End Sub


        Private Sub ChamberInfo()



            '--Create Master Table ChamberInfo 
            AddTables("SAP_ODLN", " Delivery Chamber Info", SAPbobsCOM.BoUTBTableType.bott_Document)

            '--Create Fields for CalibrationProcess
            AddFields("SAP_ODLN", "DocNum", "Delivery DocNum", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ODLN", "CardCode", "CardCode", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ODLN", "TruckNum", "TruckNum", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ODLN", "DDate", "Delivery Date", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)

            AddFields("SAP_ODLN", "TCode", "Transporter Code", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ODLN", "TName", "Transporter Name", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ODLN", "BaseType", "BaseType", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ODLN", "Temp", "Temp", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ODLN", "Density", "Density", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_ODLN", "FBP", "Final Boiling Point", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)

            AddTables("SAP_DLN1", "Delivry Chanber info", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            AddFields("SAP_DLN1", "CHN", "Chamber No", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_DLN1", "ItemCode", "ItemCode", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_DLN1", "ItemName", "ItemName", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 200, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_DLN1", "Qty", "Quantity", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Quantity, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_DLN1", "Temp", "Temp", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_DLN1", "Dip", "Dip", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)

            AddFields("SAP_DLN1", "Density", "Density", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)

            Value = New ArrayList
            Value.Add(New ValidValues("1", "B&C"))
            Value.Add(New ValidValues("2", "Others"))
            AddFields("SAP_DLN1", "VABC", "Visual Appearance(B&C)", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, "1", Nothing, Nothing)

            Value = New ArrayList
            Value.Add(New ValidValues("N", "NIL"))
            Value.Add(New ValidValues("T", "Troces"))
            AddFields("SAP_DLN1", "Water", "Water", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, "N", Nothing, Nothing)

            Value = New ArrayList
            Value.Add(New ValidValues("N", "NIL"))
            Value.Add(New ValidValues("T", "Troces"))
            AddFields("SAP_DLN1", "Sediment", "Sediment", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Value, "N", Nothing, Nothing)

            Value = New ArrayList
            Value.Add(New ValidValues("WW", "WW"))
            Value.Add(New ValidValues("S", "S"))
            Value.Add(New ValidValues("PY", "PY"))
            AddFields("SAP_DLN1", "Colour", "Colour", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 2, Value, "WW", Nothing, Nothing)

            If -1 = RegisterObject("SAP_UDO_ODLN", "Delivery Chamber info", "DT", "SAP_ODLN", "SAP_DLN1", False, False, False, True, True, False, True, "DocNum,DocEntry", False, "", True) Then SAPApplication.StatusBar.SetText("Object Registration failed.....", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub

        End Sub

        Private Sub Import()



            '--Create Master Table ChamberInfo 
            AddTables("SAP_IMPORT", " IMPORT", SAPbobsCOM.BoUTBTableType.bott_NoObject)

            '--Create Fields for CalibrationProcess
            AddFields("SAP_IMPORT", "U_Browse", "Browse", SAPbobsCOM.BoFieldTypes.db_Memo, SAPbobsCOM.BoFldSubTypes.st_Link, 150, Nothing, Nothing, Nothing, Nothing)


        End Sub



        Private Sub TruckRoutChange()
            '--Create Master Table TruckRoutChange  
            AddTables("SAP_OTRC", "TruckRoutChange ", SAPbobsCOM.BoUTBTableType.bott_Document)
            '--Create Fields for TruckRoutChange

            AddFields("SAP_OTRC", "PDate", "Posting Date", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTRC", "BPLId", "BPLId", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTRC", "BPLName", "BPLName", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTRC", "TruckNum", "TruckNum", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)

            AddTables("SAP_TRC1", "TruckRoutChange Row ", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            AddFields("SAP_TRC1", "ERout", "Existing Route", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TRC1", "NRout", "New Route", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TRC1", "ERoutN", "Existing Route Name", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TRC1", "NRoutN", "New Route Name", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)




            If -1 = RegisterObject("SAP_UDO_OTRC", "TruckRoutChange", "DT", "SAP_OTRC", "SAP_TRC1", False, False, False, True, True, False, True, "DocNum,DocEntry", False, "", True) Then SAPApplication.StatusBar.SetText("Object Registration failed.....", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub

        End Sub

        Private Sub TruckOwnerChange()
            '--Create Master Table TruckOwnerChange  
            AddTables("SAP_OTOC", "TruckRoutChange ", SAPbobsCOM.BoUTBTableType.bott_Document)
            '--Create Fields for TruckOwnerChange

            AddFields("SAP_OTOC", "PDate", "Posting Date", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTOC", "BPLId", "BPLId", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTOC", "BPLName", "BPLName", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTOC", "TruckNum", "TruckNum", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)

            AddTables("SAP_TOC1", "TruckOwnerChange Row ", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            AddFields("SAP_TOC1", "COWName", "Current Owner", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TOC1", "CTCode", "Current Trans Code", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TOC1", "NOWName", "New Owner", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TOC1", "NTCode", "New Trans Code", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)

            If -1 = RegisterObject("SAP_UDO_OTOC", "TruckOwnerChange", "DT", "SAP_OTOC", "SAP_TOC1", False, False, False, True, True, False, True, "DocNum,DocEntry", False, "", True) Then SAPApplication.StatusBar.SetText("Object Registration failed.....", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub

        End Sub

        Private Sub TTPunishment()
            '--Create Master Table TTPunishment  
            AddTables("SAP_OTTP", "TTPunishment ", SAPbobsCOM.BoUTBTableType.bott_Document)
            '--Create Fields for TTPunishment

            AddFields("SAP_OTTP", "PDate", "Posting Date", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTTP", "BPLId", "BPLId", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTTP", "BPLName", "BPLName", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTTP", "TruckNum", "TruckNum", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTTP", "OWName", " Owner", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OTTP", "TCode", " Trans Code", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)



            AddTables("SAP_TTP1", "TTPunishment Row ", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)

            AddFields("SAP_TTP1", "Active", "Active Owner", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 1, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TTP1", "FDate", "From Date", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_TTP1", "TDate", "To Date", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)


            If -1 = RegisterObject("SAP_UDO_OTTP", "TTPunishment", "DT", "SAP_OTTP", "SAP_TTP1", False, False, False, True, True, False, True, "DocNum,DocEntry", False, "", True) Then SAPApplication.StatusBar.SetText("Object Registration failed.....", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub

        End Sub


        Private Sub UserRghts_Configration()

            AddTables("SAP_OURC", "User Rghts Configration", SAPbobsCOM.BoUTBTableType.bott_MasterData)

            Value = New ArrayList
            Value.Add(New ValidValues("QC", "QC Approval"))
            AddFields("SAP_OURC", "TrxType", "Rghts Type", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 2, Value, Nothing, Nothing, Nothing)
            AddFields("SAP_OURC", "User", "User Code", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)

            '--Register Table Default Warehouse Setting
            If -1 = RegisterObject("SAP_UDO_OURC", "User Rghts Configration ", "MD", "SAP_OURC", "", False, False, False, True, False, False, False, "", True, "Code,Name,U_TrxType,U_User", True) Then SAPApplication.StatusBar.SetText("Object Registration failed.....", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub

        End Sub

        Private Sub ChamberAllocation()



            '--Create Master Table ChamberAllocation 
            AddTables("SAP_OPKL", "Chamber Allocation", SAPbobsCOM.BoUTBTableType.bott_Document)

            '--Create Fields for ChamberAllocation
            AddFields("SAP_OPKL", "DocNum", " DocNum", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OPKL", "DocEntry", " DocEntry", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OPKL", "TruckNum", "TruckNum", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OPKL", "DDate", " Date", SAPbobsCOM.BoFieldTypes.db_Date, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)

            AddFields("SAP_OPKL", "TCode", "Transporter Code", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OPKL", "TName", "Transporter Name", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OPKL", "BaseType", "BaseType", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OPKL", "Temp", "Temp", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OPKL", "Density", "Density", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OPKL", "FBP", "Final Boiling Point", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OPKL", "WhsCode", "WhsCode", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OPKL", "WhsName", "WhsName", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)



            AddTables("SAP_PKL1", "ChamberAllocation Row", SAPbobsCOM.BoUTBTableType.bott_DocumentLines)


            AddFields("SAP_PKL1", "CHN", "Chamber No", SAPbobsCOM.BoFieldTypes.db_Numeric, SAPbobsCOM.BoFldSubTypes.st_None, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PKL1", "ItemCode", "ItemCode", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 100, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PKL1", "ItemName", "ItemName", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 200, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PKL1", "CardCode", "CardCode", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 200, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PKL1", "CardName ", "CardCode", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 200, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PKL1", "Qty", "Quantity", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Quantity, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PKL1", "Temp", "Temp", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PKL1", "Dip", "Dip", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PKL1", "CAP", "Capacity", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PKL1", "Density", "Density", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PKL1", "BDocEntry", "ObjectType", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PKL1", "BDocNum", "ObjectType", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PKL1", "ObjectType", "ObjectType", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PKL1", "FBP", "Final Boiling Point", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Measurement, 20, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PKL1", "WhsCode", "WhsCode", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_PKL1", "WhsName", "WhsName", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 150, Nothing, Nothing, Nothing, Nothing)


            If -1 = RegisterObject("SAP_UDO_OPKL", "ChamberAllocation ", "DT", "SAP_OPKL", "SAP_PKL1", False, False, False, True, True, False, True, "DocNum,DocEntry", False, "", True) Then SAPApplication.StatusBar.SetText("Object Registration failed.....", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub

        End Sub


        Private Sub VatHold()

            AddTables("SAP_OVH", "Vat Hold Configration", SAPbobsCOM.BoUTBTableType.bott_MasterData)

            AddFields("SAP_OVH", "Per", "Percentage ", SAPbobsCOM.BoFieldTypes.db_Float, SAPbobsCOM.BoFldSubTypes.st_Percentage, 10, Nothing, Nothing, Nothing, Nothing)
            AddFields("SAP_OVH", "Tax", "Tax", SAPbobsCOM.BoFieldTypes.db_Alpha, SAPbobsCOM.BoFldSubTypes.st_None, 50, Nothing, Nothing, Nothing, Nothing)

            '--Register Table Default 
            If -1 = RegisterObject("SAP_UDO_OVH", "Vat Hold Configration ", "MD", "SAP_OVH", "", False, False, False, True, False, False, False, "", True, "Code,Name,U_Per,U_Tax", True) Then SAPApplication.StatusBar.SetText("Object Registration failed.....", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error) : Exit Sub

        End Sub



    End Module
End Namespace
