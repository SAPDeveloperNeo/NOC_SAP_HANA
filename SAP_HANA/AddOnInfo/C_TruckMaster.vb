Imports System.Drawing
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Collections
Imports System.IO
Imports System.Diagnostics.CodeAnalysis
Imports System.Runtime.InteropServices
Imports System.Globalization
Imports System.Data
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports System.Text

Namespace SAP_HANA
    Public Class C_TruckMaster : Implements ISAP_HANA

#Region "Constructors"
        Private __oApplication As SAPbouiCOM.Application
        Public __oCompany As SAPbobsCOM.Company
        Private oForm As SAPbouiCOM.Form
        Private oItem As SAPbouiCOM.Item
        Dim oLabel As SAPbouiCOM.StaticText
        Dim ocombo As SAPbouiCOM.ComboBox
        Dim oEditText1 As SAPbouiCOM.EditText
        Dim oCheckBox As SAPbouiCOM.CheckBox
        Dim oButton As SAPbouiCOM.Button
        Dim oItem1 As SAPbouiCOM.Item
        Dim oCFLEvento As SAPbouiCOM.IChooseFromListEvent
        Dim sCFL_ID As String
        Dim oCFLs As SAPbouiCOM.ChooseFromListCollection
        Dim oCons As SAPbouiCOM.Conditions
        Dim oCon As SAPbouiCOM.Condition
        Dim oCFL As SAPbouiCOM.ChooseFromList
        Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams
        Dim oDataTable1 As SAPbouiCOM.DataTable
        Dim oDataTable2 As SAPbouiCOM.DataTable
        Dim oDataTable As SAPbouiCOM.DataTable
        Dim oConditions As SAPbouiCOM.Conditions
        Dim oCondition As SAPbouiCOM.Condition
        Private oMatrix As SAPbouiCOM.Matrix
        Private oColumns As SAPbouiCOM.Columns
        Private odataset As SAPbouiCOM.DataSource
        Dim oComboBox As SAPbouiCOM.ComboBox

        Public Property ObjectCode As String Implements ISAP_HANA.ObjectCode
            Get

            End Get
            Set(value As String)

            End Set
        End Property

        Public Sub New(ByRef sApp As SAPbouiCOM.Application, ByRef oCompany As SAPbobsCOM.Company)
            __oApplication = sApp
            __oCompany = oCompany '.Company.GetDICompany()
        End Sub
#End Region

#Region "Const Enumeration"
        Public Enum menuID
            Next_Record = 1288
            Previous_Record = 1289
            First_Record = 1290
            Last_Record = 1291
            Duplicate_Row = 1287
            Duplicate = 1287
            Delete_Row = 1293
            Add_Row = 1292
            Remove = 1283
            Find = 1281
            Add = 1282
            Undo = 769
            Cut = 771
            Copy = 772
            Paste = 773
            Delete = 774
        End Enum

#End Region

        Public Sub Form_Creation_MenuEvent(ByRef pVal As MenuEvent, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Creation_MenuEvent
            Try
                Dim sFileName As String = AppDomain.CurrentDomain.BaseDirectory & "SRF\VehicalMaster.srf"
                Dim sFormName As String = "SAP_UDO_OOTM"
                Dim FormUID As String = LoadXMLFiles(__oApplication, sFileName)
                oForm = __oApplication.Forms.GetForm("SAP_UDO_OOTM", __oApplication.Forms.ActiveForm.TypeCount)
                If Not String.IsNullOrEmpty(FormUID) Then
                    ActiveForm(oForm, "Item_17", "1")
                    oForm.EnableMenu("1292", True)
                    oForm.EnableMenu("1293", True)
                    oForm.EnableMenu("520", True)
                    oForm.EnableMenu("519", True)
                    oForm.Freeze(True)
                    oForm.Mode = BoFormMode.fm_ADD_MODE

                    DefulatSetting(oForm.UniqueID, BubbleEvent)




                    oForm.Freeze(False)
                    oForm.Refresh()
                    oForm.Update()
                End If
            Catch ex As Exception
                oForm.Freeze(False)
                oForm.Refresh()
                oForm.Update()
                __oApplication.MessageBox("[MenuEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try



        End Sub

        Public Sub Form_TMenuEvent(ByRef pVal As MenuEvent, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_TMenuEvent
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(__oApplication.Forms.ActiveForm.TypeEx, __oApplication.Forms.ActiveForm.TypeCount)

                ' If oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then Exit Sub : BubbleEvent = False



                If pVal.MenuUID = CType(menuID.Add, String) And pVal.BeforeAction = False Then

                    DefulatSetting(oForm.UniqueID, BubbleEvent)
                ElseIf pVal.MenuUID = CType(menuID.Delete_Row, String) And pVal.BeforeAction = True Then


                ElseIf pVal.MenuUID = CType(menuID.Add_Row, String) And pVal.BeforeAction = False Then
                    Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                    AddRowInMatrix(oForm, "@SAP_TM1", "m1")




                End If
            Catch ex As Exception

            End Try
        End Sub

        Public Sub Form_Load_DataEvent(ByRef BusinessObjectInfo As BusinessObjectInfo, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Load_DataEvent
            Throw New NotImplementedException()
        End Sub
        Private Sub FromBranch_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OTM")

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                dbsrc.SetValue("U_BRANCH", dbsrc.Offset, oDataTable.GetValue("BPLName", 0) & "")
                                'dbsrc.SetValue("U_FBN", dbsrc.Offset, oDataTable.GetValue("BPLName", 0) & "")


                            End If

                        Catch ex As Exception

                        End Try
                        If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                    End If
                End If

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub
        Public Sub Form_Process_ItemEvents(FormUID As String, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Process_ItemEvents
            Try
                BubbleEvent = True

                If pVal.BeforeAction = False Then
                    If pVal.ItemUID = "m2" And pVal.ColUID = "Col_0" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Route_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.ItemUID = "Item_37" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        FromBranch_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_10" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        TransportarCode_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m2" And pVal.ColUID = "Col_0" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        AddRow_OnAfterLocstFocus(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_0" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        AddRow_OnAfterLocstFocus(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.ItemUID = "1" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then

                        Add_OnAfterItemPressed(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.ItemUID = "showrev" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        Add_OnAfterItemPressed(FormUID, pVal, BubbleEvent)
                    End If
                ElseIf pVal.ItemUID = "Item_37" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                    FBranch_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)
                ElseIf pVal.BeforeAction = True Then

                    If pVal.ItemUID = "1" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        Add_OnBeforeItemPressed(FormUID, pVal, BubbleEvent)

                    End If


                    If pVal.ItemUID = "m2" And pVal.ColUID = "Col_0" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Route_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_10" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        TransportCode_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)
                    End If
                End If

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Public Sub Form_Report_Display(ByRef paramarraylist As Object) Implements ISAP_HANA.Form_Report_Display
            Throw New NotImplementedException()
        End Sub
        Private Sub FBranch_OnBeforeChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)







                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                oConditions = oCFL.GetConditions()
                oConditions = Nothing
                oCFL.SetConditions(oConditions)
                oConditions = oCFL.GetConditions()


                Dim SqlQuery As String = "SELECT T1.""BPLId"" FROM OUSR T0 INNER JOIN USR6 T1 ON T0.""USER_CODE"" = T1.""UserCode"" INNER JOIN OBPL T2 ON T1.""BPLId"" = T2.""BPLId"" And  T2.""Disabled"" ='N' WHERE T0.""USERID""='" + __bobCompany.UserSignature.ToString() + "'"

                Dim oRset As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRset.DoQuery(SqlQuery)

                If oRset.RecordCount > 0 Then


                    For i As Integer = 0 To oRset.RecordCount - 1
                        If i >= 1 And i <= oRset.RecordCount - 1 Then
                            oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR
                        End If
                        oCondition = oConditions.Add
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "BPLId"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = oRset.Fields.Item("BPLId").Value & ""
                        oCondition.BracketCloseNum = 1
                        oRset.MoveNext()

                    Next
                    oCFL.SetConditions(oConditions)
                Else
                    oCondition = oConditions.Add
                    oCondition.BracketOpenNum = 1
                    oCondition.Alias = "BPLId"
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                    oCondition.CondVal = Nothing
                    oCondition.BracketCloseNum = 1
                    oCFL.SetConditions(oConditions)
                End If








            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub
        Private Sub ARInvoice_Creation(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String, ByVal Type As String)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim oRs As SAPbobsCOM.Recordset = Nothing
                Dim oRsVendor As SAPbobsCOM.Recordset = Nothing
                Dim Sql As String = ""
                Dim PartyID As String = ""



                Sql = "CALL SAP_GET_AR_Invoice_Data_FOR_TRUCKMASTER ('" + DocEntry + "')"

                ''----Fetch the Reocrd for Purchase Order Creation----

                oRs = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRs.DoQuery(Sql)
                If oRs.RecordCount > 0 Then
                    oRs.MoveFirst()
                    For i = 0 To oRs.RecordCount - 1


                        Dim oPurchaseInvoice As SAPbobsCOM.Documents = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices)

                        PartyID = oRs.Fields.Item("ConnBP").Value.ToString()

                        oPurchaseInvoice.CardCode = oRs.Fields.Item("ConnBP").Value.ToString()
                        oPurchaseInvoice.DocDate = oRs.Fields.Item("U_PDate").Value.ToString()
                        oPurchaseInvoice.DocDueDate = oRs.Fields.Item("U_PDate").Value.ToString()
                        oPurchaseInvoice.BPL_IDAssignedToInvoice = oRs.Fields.Item("BPLId").Value.ToString()
                        '  oPurchaseInvoice.UserFields.Fields.Item("U_ITDocE").Value = oRs.Fields.Item("DocEntry").Value & ""
                        '  oPurchaseInvoice.UserFields.Fields.Item("U_ITDocN").Value = oRs.Fields.Item("DocNum").Value & ""

                        ' oPurchaseInvoice.Memo = "AR Invoice Based on Truck Number  " '& oRs.Fields.Item("U_TRegNo").Value.ToString
                        '& "  and For the Document : " & oRs.Fields.Item("U_DocCat").Value & " and Revision No :" & oRs.Fields.Item("U_REVNO").Value & ""

                        oPurchaseInvoice.Comments = "AR Invoice Based on Truck Number  " & oRs.Fields.Item("U_TRegNo").Value.ToString & "  and For the Document : " & oRs.Fields.Item("U_DocCat").Value & " and Revision No :" & oRs.Fields.Item("U_REVNO").Value & ""
                        ''---This is for Purchase Order Line---
                        Dim oPurchaseInvoice_Line As SAPbobsCOM.Document_Lines = oPurchaseInvoice.Lines


                        oPurchaseInvoice_Line.ItemCode = oRs.Fields.Item("U_ITEMCODE").Value
                        oPurchaseInvoice_Line.Quantity = 1 ' oRs.Fields.Item("U_ClLoss").Value
                        oPurchaseInvoice_Line.AccountCode = oRs.Fields.Item("U_REVENUE").Value

                        'If Type = "ST" Then
                        oPurchaseInvoice_Line.WarehouseCode = oRs.Fields.Item("WhsCode").Value '"PO5HL001"

                        If oRs.Fields.Item("U_VATR").Value = "Y" Then
                            oPurchaseInvoice_Line.TaxCode = oRs.Fields.Item("TaxCode").Value
                        Else
                            oPurchaseInvoice_Line.TaxCode = "VAT@0"
                        End If

                        oPurchaseInvoice_Line.Price = oRs.Fields.Item("U_CHARGE").Value

                        'oPurchaseInvoice_Line.UserFields.Fields.Item("U_Chamber").Value = oRs.Fields.Item("U_Chamber").Value & ""
                        oPurchaseInvoice_Line.CostingCode = oRs.Fields.Item("U_BRANCH").Value & ""
                        oPurchaseInvoice_Line.CostingCode2 = "DOFINAN" '"DOSUPPD" 'oRs.Fields.Item("U_OCRC2").Value & ""
                        oPurchaseInvoice_Line.CostingCode3 = "NA1" 'oRs.Fields.Item("U_OCRC3").Value & ""
                        oPurchaseInvoice_Line.CostingCode4 = "NA" ' oRs.Fields.Item("U_OCRC4").Value & ""
                        oPurchaseInvoice_Line.CostingCode5 = "" 'oRs.Fields.Item("U_OCRC5").Value & ""



                        oPurchaseInvoice_Line.Add()



                        If oRs.Fields.Item("PenalityFlag").Value = 1 Then


                            oPurchaseInvoice_Line.ItemCode = oRs.Fields.Item("U_ITEMCODE").Value
                            oPurchaseInvoice_Line.Quantity = 1 ' oRs.Fields.Item("U_ClLoss").Value
                            oPurchaseInvoice_Line.AccountCode = oRs.Fields.Item("U_PGLCODE").Value

                            'If Type = "ST" Then
                            oPurchaseInvoice_Line.WarehouseCode = oRs.Fields.Item("WhsCode").Value '"PO5HL001"


                            oPurchaseInvoice_Line.TaxCode = "VAT@0" 'oRs.Fields.Item("TaxCode").Value
                            oPurchaseInvoice_Line.Price = oRs.Fields.Item("U_PCHARGE").Value

                            'oPurchaseInvoice_Line.UserFields.Fields.Item("U_Chamber").Value = oRs.Fields.Item("U_Chamber").Value & ""
                            oPurchaseInvoice_Line.CostingCode = oRs.Fields.Item("U_BRANCH").Value & ""
                            oPurchaseInvoice_Line.CostingCode2 = "DOFINAN" '"DOSUPPD" 'oRs.Fields.Item("U_OCRC2").Value & ""
                            oPurchaseInvoice_Line.CostingCode3 = "NA1" 'oRs.Fields.Item("U_OCRC3").Value & ""
                            oPurchaseInvoice_Line.CostingCode4 = "NA" ' oRs.Fields.Item("U_OCRC4").Value & ""
                            oPurchaseInvoice_Line.CostingCode5 = "" 'oRs.Fields.Item("U_OCRC5").Value & ""



                            oPurchaseInvoice_Line.Add()
                        End If

                        Dim Result As Integer = oPurchaseInvoice.Add()
                        If Result <> 0 Then
                            'If Result = -5002 Then

                            __oApplication.MessageBox("[ItemEvent] - AR Invoice not generated !!Please do it mannually and update document entry for specific line", 1, "Ok", "", "")

                            ' Manager_ARInvoice_Creation(FormUID, pVal, BubbleEvent, DocEntry, Type)
                            'Else
                            __oApplication.StatusBar.SetText("Error: In Generating AR Invoice - " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                            'End If

                            '__oApplication.StatusBar.SetText("Error: In Generating AR Invoice - " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                        Else

                            Dim SqlQuery As String
                            Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

                            If oRs.Fields.Item("Type").Value = "Document" Then

                                SqlQuery = "Update ""@SAP_TM1"" set U_ARNO=(SELECT MAX(""DocEntry"") from oinv),U_IND='No' WHERE ""DocEntry""='" & DocEntry & "' and ""U_DocCat""='" & oRs.Fields.Item("U_DocCat").Value & "'"
                                oRset1.DoQuery(SqlQuery.ToString)
                                SqlQuery = "Update ""@SAP_TM1REV"" set U_ARNO=(SELECT MAX(""DocEntry"") from oinv) WHERE ""DocEntry""='" & DocEntry & "' and ""U_DocCat""='" & oRs.Fields.Item("U_DocCat").Value & "' AND U_REVNO='" & oRs.Fields.Item("U_REVNO").Value & "'"
                                oRset1.DoQuery(SqlQuery.ToString)
                            Else
                                SqlQuery = "Update ""@SAP_TM2"" set U_ARNO=(SELECT MAX(""DocEntry"") from oinv),U_IND='No' WHERE ""DocEntry""='" & DocEntry & "' and ""U_Route""='" & oRs.Fields.Item("U_DocCat").Value & "'"
                                oRset1.DoQuery(SqlQuery.ToString)
                                SqlQuery = "Update ""@SAP_TM2REV"" set U_ARNO=(SELECT MAX(""DocEntry"") from oinv) WHERE ""DocEntry""='" & DocEntry & "' and ""U_Route""='" & oRs.Fields.Item("U_DocCat").Value & "' AND U_REVNO='" & oRs.Fields.Item("U_REVNO").Value & "'"
                                oRset1.DoQuery(SqlQuery.ToString)
                            End If
                            'SqlQuery.Append("Update ""@SAP_OIT"" Set ""U_ARDN""= (Select ""DocNum"" From OINV Where ""U_ITDocE""='" + DocEntry + "' AND ""CardCode""='" + PartyID + "'),")
                            'SqlQuery.Append(" ""U_ARDE""= (Select ""DocEntry"" From OINV Where ""U_ITDocE""='" + DocEntry + "' AND ""CardCode""='" + PartyID + "')")
                            'SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")


                            __oApplication.StatusBar.SetText("AR Invoice Generated", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                            End If
                            oRs.MoveNext()
                    Next
                End If

                'End If
                __oApplication.ActivateMenuItem("1288")
                __oApplication.ActivateMenuItem("1289")

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub
        Private Sub Add_OnAfterItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try

                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                '                If oForm.Mode <> BoFormMode.fm_FIND_MODE Or oForm.Mode <> BoFormMode.fm_OK_MODE Then


                Dim chkEXT As SAPbouiCOM.CheckBox = oForm.Items.Item("Item_39").Specific
                    Dim docentry As Integer
                    docentry = oForm.Items.Item("Item_17").Specific.value
                If pVal.ItemUID = "showrev" Then


                    IsBaseForm = oForm
                    IsBaseItemID = "Item_17"
                    IsBaseUDF = "DocEntry"
                    IsBase_DN_UDF = "DocNum"
                    Dim c_TruckRevHist As TruckRevHist = New TruckRevHist(__Application, __bobCompany)
                    c_TruckRevHist.Form_Creation(IsBaseForm, docentry.ToString, "List of Truck Revision History")
                ElseIf pVal.ItemUID = "1" And (oForm.Mode = BoFormMode.fm_OK_MODE) Then

                    If chkEXT.Checked Then
                            chkEXT.Checked = False
                            Exit Sub
                        End If
                        ARInvoice_Creation(FormUID, pVal, BubbleEvent, docentry, "")

                    End If

                    ' Dim chkEXT As SAPbouiCOM.CheckBox = oForm.Items.Item("Item_39").Specific
                    If chkEXT.Checked Then
                        chkEXT.Checked = False
                        ' oForm.Mode = BoFormMode.fm_OK_MODE
                    End If
            Catch ex As Exception
                __oApplication.MessageBox("SOR-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try

        End Sub
        Private Sub Add_OnBeforeItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                If oForm.Mode = BoFormMode.fm_ADD_MODE Or oForm.Mode = BoFormMode.fm_UPDATE_MODE Then

                    Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OTM")
                    BubbleEvent = True
                    Dim trsting As String = "TR"
                    Dim trnumber As String = "00000001"
                    Dim ithReturnValue As Integer
                    Dim docentry As Integer
                    Dim chkEXT As SAPbouiCOM.CheckBox = oForm.Items.Item("Item_39").Specific
                    Dim txtTruckNo As SAPbouiCOM.EditText = oForm.Items.Item("Item_9").Specific
                    Dim txtTranspCode As SAPbouiCOM.EditText = oForm.Items.Item("Item_10").Specific
                    Dim txtTranspName As SAPbouiCOM.EditText = oForm.Items.Item("Item_12").Specific
                    Dim txtModalNo As SAPbouiCOM.EditText = oForm.Items.Item("Item_24").Specific
                    Dim txtVType As SAPbouiCOM.EditText = oForm.Items.Item("Item_26").Specific
                    Dim txtbranch As SAPbouiCOM.EditText = oForm.Items.Item("Item_37").Specific
                    If txtbranch.Value = "" Then
                        __oApplication.MessageBox("[ItemEvent] - Branch can not be Blank", 1, "Ok", "", "")
                        BubbleEvent = False
                        Exit Sub
                    End If

                    If txtTruckNo.Value = "" Then
                        __oApplication.MessageBox("[ItemEvent] - Truck No. can not be Blank", 1, "Ok", "", "")
                        BubbleEvent = False
                        Exit Sub
                    End If

                    If txtTranspCode.Value = "" Then
                        __oApplication.MessageBox("[ItemEvent] - Transporter code can not be Blank", 1, "Ok", "", "")
                        BubbleEvent = False
                        Exit Sub
                    End If

                    If txtTranspName.Value = "" Then
                        __oApplication.MessageBox("[ItemEvent] - Transporter Name can not be Blank", 1, "Ok", "", "")
                        BubbleEvent = False
                        Exit Sub
                    End If

                    If txtModalNo.Value = "" Then
                        __oApplication.MessageBox("[ItemEvent] - Model No. can not be Blank", 1, "Ok", "", "")
                        BubbleEvent = False
                        Exit Sub
                    End If

                    If txtVType.Value = "" Then
                        __oApplication.MessageBox("[ItemEvent] - Vehicle Type can not be Blank", 1, "Ok", "", "")
                        BubbleEvent = False
                        Exit Sub
                    End If



                    Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                    Dim rowcount As Integer = m1.VisualRowCount
                    Dim oRset As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    If rowcount > 0 Then

                        For iRow As Integer = 1 To rowcount

                            Dim SqlQuery1 As String = "select U_CALVR  from ""@SAP_ODM"" where ""Code""='" & m1.Columns.Item("Col_0").Cells.Item(iRow).Specific.Value & "'"

                            oRset.DoQuery(SqlQuery1)
                            Dim CALVR As String = ""
                            CALVR = oRset.Fields.Item("U_CALVR").Value.ToString()

                            'If (m1.Columns.Item("Col_0").Cells.Item(iRow).Specific.Value) = "Calibration Renewal" Then
                            '    Dim SqlQuery As String = "Select ""U_CDate"",""U_CEDate""  from ""@SAP_OCALP"" where ""U_VC""='" & txtTruckNo.Value & "'"

                            '    oRset.DoQuery(SqlQuery)
                            '    Dim d1 As Date = oRset.Fields.Item("U_CDate").Value.ToString()
                            '    Dim s1 As String
                            '    s1 = d1.ToString("yyyyMMdd")
                            '    's1 = s1.ToString("dd/MM/yy")

                            '    If (m1.Columns.Item("Col_1").Cells.Item(iRow).Specific.Value) <> s1 Then
                            '        __oApplication.MessageBox("[ItemEvent] - Calibration for this truck date not match.Please modify the record before proceed", 1, "Ok", "", "")
                            '        BubbleEvent = False
                            '        Exit Sub
                            '    End If
                            '    d1 = oRset.Fields.Item("U_CEDate").Value.ToString()
                            '    s1 = d1.ToString("yyyyMMdd")
                            '    's1 = s1.ToString("dd/MM/yy")

                            '    If (m1.Columns.Item("Col_2").Cells.Item(iRow).Specific.Value) <> s1 Then
                            '        __oApplication.MessageBox("[ItemEvent] - Calibration expiration date not match.Please modify the record before proceed", 1, "Ok", "", "")
                            '        BubbleEvent = False
                            '        Exit Sub
                            '    End If
                            'Else
                            If (CALVR) = "Y" Then
                                Dim SqlQuery As String = "Select ""U_CDate"",""U_CEDate""  from ""@SAP_OCALP"" where ""U_VC""='" & txtTruckNo.Value & "'"

                                oRset.DoQuery(SqlQuery)
                                Dim d1 As Date = oRset.Fields.Item("U_CDate").Value.ToString()
                                Dim s1 As String
                                s1 = d1.ToString("yyyyMMdd")
                                's1 = s1.ToString("dd/MM/yy")

                                If (m1.Columns.Item("Col_1").Cells.Item(iRow).Specific.Value) <> s1 Then
                                    __oApplication.MessageBox("[ItemEvent] - Calibration for this truck date not match.Please modify the record before proceed", 1, "Ok", "", "")
                                    BubbleEvent = False
                                    Exit Sub
                                End If
                                d1 = oRset.Fields.Item("U_CEDate").Value.ToString()
                                s1 = d1.ToString("yyyyMMdd")
                                's1 = s1.ToString("dd/MM/yy")

                                If (m1.Columns.Item("Col_2").Cells.Item(iRow).Specific.Value) <> s1 Then
                                    __oApplication.MessageBox("[ItemEvent] - Calibration expiration date not match.Please modify the record before proceed", 1, "Ok", "", "")
                                    BubbleEvent = False
                                    Exit Sub
                                End If

                            End If
                        Next
                    End If

                    chkEXT.Item.AffectsFormMode = False
                    If chkEXT.Checked Then
                        ' oForm.Mode = BoFormMode.fm_OK_MODE
                        Exit Sub
                    End If

                    docentry = oForm.Items.Item("Item_17").Specific.value
                    ithReturnValue = __oApplication.MessageBox("You are going to modify/create ......... Do You want to  Modify this record ? ", 1, "Yes", "No")
                    Dim REVNO As Integer
                    If ithReturnValue = 1 Then
                        Dim SqlQuery As String = "Select count(*)  from ""@SAP_OTM"" where ""DocEntry""=" & docentry

                        oRset.DoQuery(SqlQuery)

                        Dim count As Integer = 0
                        count = oRset.RecordCount
                        If count > 0 Then
                            If oForm.Items.Item("Item_15").Specific.value = "" Then
                                dbsrc.SetValue("U_REVNO", dbsrc.Offset, trsting & trnumber)
                                dbsrc.SetValue("U_NEWREVNO", dbsrc.Offset, trsting & trnumber)
                            Else
                                REVNO = Right((oForm.Items.Item("Item_15").Specific.value), 8)
                                REVNO = REVNO + 1
                                dbsrc.SetValue("U_REVNO", dbsrc.Offset, trsting & REVNO.ToString("D8"))
                                dbsrc.SetValue("U_NEWREVNO", dbsrc.Offset, trsting & REVNO.ToString("D8"))
                            End If
                        Else
                            dbsrc.SetValue("U_REVNO", dbsrc.Offset, (oForm.Items.Item("Item_15").Specific.value))
                        End If


                    Else
                        BubbleEvent = False
                        Exit Sub

                    End If
                    oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                End If
            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try

        End Sub
        Private Sub DefulatSetting(ByVal FormUID As String, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(__oApplication.Forms.ActiveForm.TypeEx, __oApplication.Forms.ActiveForm.TypeCount)

                Dim ToDate As Date = Nothing
                Dim sc As String = __oApplication.Company.ServerDate
                ToDate = DateTime.ParseExact(sc, GetDateFormat, DateTimeFormatInfo.InvariantInfo)
                Dim PODate As SAPbouiCOM.EditText = oForm.Items.Item("Item_19").Specific
                PODate.String = ToDate.ToString("yyyyMMdd")


                oForm.Items.Item("Item_17").Specific.value = GenrateDocEntry("DocEntry", "@SAP_OTM")
                oForm.Items.Item("Item_18").Specific.value = GenrateDocEntry("DocEntry", "@SAP_OTM")
                '  oForm.Items.Item("Item_15").Specific.value = ""
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                AddRowInMatrix(oForm, "@SAP_TM1", "m1")

                Dim m2 As SAPbouiCOM.Matrix = oForm.Items.Item("m2").Specific
                AddRowInMatrix(oForm, "@SAP_TM2", "m2")

                oForm.PaneLevel = 1


                Dim cbDocument As SAPbouiCOM.Column = m1.Columns.Item("Col_0")

                Fill_MatrixColumn_ComboBox(cbDocument, "SELECT ""Code"",""Name"" FROM  ""@SAP_ODM"" where ""U_SHW"" ='Y' ", "Code", "Name", False, False)
                cbDocument.ValidValues.Add("", "")
                'Dim SqlQuery As String = "SELECT T2.""BPLName"" FROM OUSR T0 INNER JOIN USR6 T1 ON T0.""USER_CODE"" = T1.""UserCode"" INNER JOIN OBPL T2 ON T1.""BPLId"" = T2.""BPLId"" And  T2.""Disabled"" ='N' WHERE T0.""USERID""='" + __bobCompany.UserSignature.ToString() + "'"

                'Dim oRset As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                '   oRset.DoQuery(SqlQuery)

                '   If oRset.RecordCount > 0 Then
                ' oForm.Items.Item("Item_37").Specific.value = oRset.Fields.Item("BPLName").Value & ""
                '  End If
            Catch ex As Exception

            End Try
        End Sub



        Private Sub Route_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                Dim dt As Date = Nothing

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_TM2")
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m2").Specific

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then
                                m1.GetLineData(pVal.Row)


                                dbsrc.SetValue("U_Route", dbsrc.Offset, oDataTable.GetValue("PrcCode", 0) & "")



                                m1.SetLineData(pVal.Row)
                                m1.FlushToDataSource()

                                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                            End If
                        Catch ex As Exception

                        End Try
                    End If
                End If
            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub




        Private Sub TransportarCode_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OTM")

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                dbsrc.SetValue("U_CardCode", dbsrc.Offset, oDataTable.GetValue("CardCode", 0) & "")
                                dbsrc.SetValue("U_CardName", dbsrc.Offset, oDataTable.GetValue("CardName", 0) & "")

                            End If

                        Catch ex As Exception

                        End Try
                        If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                    End If
                End If

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub TransportCode_OnBeforeChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)



                Dim SqlQuery As String = ""


                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                oConditions = oCFL.GetConditions()
                oConditions = Nothing
                oCFL.SetConditions(oConditions)
                oConditions = oCFL.GetConditions()

                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 1
                oCondition.Alias = "CardType"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = "S"
                oCondition.BracketCloseNum = 1



                oCFL.SetConditions(oConditions)

            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub



        Private Sub Route_OnBeforeChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)



                Dim SqlQuery As String = ""


                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                oConditions = oCFL.GetConditions()
                oConditions = Nothing
                oCFL.SetConditions(oConditions)
                oConditions = oCFL.GetConditions()

                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 1
                ' oCondition.Alias = "DimCode"
                oCondition.Alias = "CCTypeCode"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                ' oCondition.CondVal = "3"
                oCondition.CondVal = "Route"
                oCondition.BracketCloseNum = 1



                oCFL.SetConditions(oConditions)

            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub AddRow_OnAfterLocstFocus(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                If pVal.ItemUID = "m2" Then


                    Dim m2 As SAPbouiCOM.Matrix = oForm.Items.Item("m2").Specific
                    If m2.Columns.Item("Col_0").Cells.Item(pVal.Row).Specific.Value <> "" Then

                        For iRow As Integer = 1 To m2.VisualRowCount
                            If m2.Columns.Item("Col_0").Cells.Item(pVal.Row).Specific.Value = m2.Columns.Item("Col_0").Cells.Item(iRow).Specific.Value Then
                                If pVal.Row <> iRow Then
                                    __oApplication.MessageBox("SUB-[ItemEvent] - Can not add same route multipe time", 1, "Ok", "", "")
                                    Exit Sub
                                End If
                            End If
                        Next
                        AddRowInMatrix(oForm, "@SAP_TM2", "m2")

                    End If
                ElseIf pVal.ItemUID = "m1" Then
                    Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                    If m1.Columns.Item("Col_0").Cells.Item(pVal.Row).Specific.Value <> "" Then

                        For iRow As Integer = 1 To m1.VisualRowCount
                            If m1.Columns.Item("Col_0").Cells.Item(pVal.Row).Specific.Value = m1.Columns.Item("Col_0").Cells.Item(iRow).Specific.Value Then
                                If pVal.Row <> iRow Then
                                    __oApplication.MessageBox("SUB-[ItemEvent] - Can not add same document entry multipe time", 1, "Ok", "", "")
                                    Exit Sub
                                End If
                            End If
                        Next
                    End If
                End If
            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

    End Class
End Namespace

