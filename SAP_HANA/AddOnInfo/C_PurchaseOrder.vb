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
    Public Class C_PurchaseOrder : Implements ISAP_HANA

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
            'Udf = 6913 ' created mg
        End Enum

#End Region

        Public Sub Form_Creation_MenuEvent(ByRef pVal As MenuEvent, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Creation_MenuEvent

        End Sub

        Public Sub Form_Load_DataEvent(ByRef BusinessObjectInfo As BusinessObjectInfo, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Load_DataEvent

        End Sub

        Public Sub Form_Process_ItemEvents(FormUID As String, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Process_ItemEvents
            Try
                BubbleEvent = True

                If pVal.BeforeAction = False Then
                    If pVal.ItemUID = "1" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        Add_OnAfterItemPressed(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.FormTypeEx = "142" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_RESIZE Then
                        C_Delivery_OnResize(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.ItemUID = "btnChamber" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        Chamber_OnAfterItemPressed(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.FormTypeEx = "142" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_LOAD Then
                        Chamber_FormLoad(FormUID, pVal, BubbleEvent)
                        'RefPO_FormLoad(FormUID, pVal, BubbleEvent)
                        'ElseIf pVal.FormTypeEx = "-142" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_LOAD Then
                        '    RefPO_FormLoad(FormUID, pVal, BubbleEvent)
                    End If

                ElseIf pVal.BeforeAction = True Then

                    If pVal.ItemUID = "1" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        Add_OnBeforeItemPressed(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.FormTypeEx = "142" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_LOAD Then
                        C_Delivery_OnBeforeFormLoad(FormUID, pVal, BubbleEvent)
                    End If
                End If

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Public Sub Form_Report_Display(ByRef paramarraylist As Object) Implements ISAP_HANA.Form_Report_Display

        End Sub

        Public Sub Form_TMenuEvent(ByRef pVal As MenuEvent, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_TMenuEvent

        End Sub


        Private Sub Add_OnBeforeItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or oForm.Mode = BoFormMode.fm_UPDATE_MODE Then



                    SPODOCNUM = ""
                    SPOCARDCODE = ""
                    SPODate = ""

                    SPODate = oForm.Items.Item("10").Specific.Value.ToString
                    SPODOCNUM = oForm.Items.Item("8").Specific.Value.ToString
                    SPOCARDCODE = oForm.Items.Item("4").Specific.Value.ToString





                End If
            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Add_OnAfterItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                If oForm.Mode = BoFormMode.fm_ADD_MODE Or oForm.Mode = BoFormMode.fm_OK_MODE Then


                    Dim DocEntry As String
                    If SPODOCNUM <> "" And SPOCARDCODE <> "" Then
                        DocEntry = GetValue(" Select  T0.""DocEntry"" FROM ""OPOR""  T0 WHERE T0.""DocNum"" ='" + SPODOCNUM + "' And T0.""CardCode"" ='" + SPOCARDCODE + "' AND T0.""DocDate""='" + SPODate + "'", "DocEntry")
                        If DocEntry <> "" Then

                            Try


                                Dim ChamberDocEntry As String = GetValue("Select  T0.""U_CMDocEntry"" FROM ""OPOR""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_CMDocEntry")
                                Dim ChamberDocNum As String = GetValue("Select  T0.""U_CMDocNum"" FROM ""OPOR""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_CMDocNum")

                                If ChamberDocEntry <> "" And ChamberDocNum <> "" Then
                                    Dim SqlQuery As New StringBuilder
                                    SqlQuery.Append("Update ""@SAP_ODLN"" Set ""U_DocNum""= (Select ""DocNum"" From OPOR Where ""DocEntry""='" + DocEntry + "' )")
                                    SqlQuery.Append("Where ""DocEntry""='" + ChamberDocEntry + "'")
                                    Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                    oRset1.DoQuery(SqlQuery.ToString)

                                End If

                            Catch ex As Exception

                            End Try
                            PurchaseOrder_Creation(FormUID, pVal, BubbleEvent, DocEntry)
                        End If

                    End If




                    SPODOCNUM = ""
                    SPOCARDCODE = ""
                    SPODate = ""

                End If
            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub PurchaseOrder_Creation(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim oRs As SAPbobsCOM.Recordset = Nothing
                Dim oRsVendor As SAPbobsCOM.Recordset = Nothing
                Dim Sql As String = ""





                ''----Fetch the Reocrd for Purchase Order Creation----
                Sql = "CALL SAP_GET_Service_Purchase_Orde_Data ('" + DocEntry + "')"
                oRs = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRs.DoQuery(Sql)
                If oRs.RecordCount > 0 Then
                    Dim oPurchaseOrder As SAPbobsCOM.Documents = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders)

                    '---This is for Header Details of Purchase Order----

                    oPurchaseOrder.CardCode = oRs.Fields.Item("CardCode").Value.ToString()
                    oPurchaseOrder.DocDate = oRs.Fields.Item("DocDate").Value.ToString()
                    oPurchaseOrder.DocDueDate = oRs.Fields.Item("DocDate").Value.ToString()
                    oPurchaseOrder.BPL_IDAssignedToInvoice = oRs.Fields.Item("Branch").Value.ToString()
                    oPurchaseOrder.UserFields.Fields.Item("U_ITDocE").Value = oRs.Fields.Item("DocEntry").Value & ""
                    oPurchaseOrder.UserFields.Fields.Item("U_ITDocN").Value = oRs.Fields.Item("DocNum").Value & ""
                    oPurchaseOrder.UserFields.Fields.Item("U_Flag").Value = "SPO" & ""
                    oPurchaseOrder.UserFields.Fields.Item("U_TRNO").Value = oRs.Fields.Item("U_TRNO").Value & ""


                    ''---This is for Purchase Order Line---
                    Dim oPurchaseOrder_Line As SAPbobsCOM.Document_Lines = oPurchaseOrder.Lines
                    While oRs.EoF = False

                        oPurchaseOrder_Line.ItemCode = oRs.Fields.Item("ItemCode").Value
                        oPurchaseOrder_Line.Quantity = oRs.Fields.Item("Quantity").Value

                        oPurchaseOrder_Line.WarehouseCode = oRs.Fields.Item("WhsCode").Value
                        oPurchaseOrder_Line.Price = oRs.Fields.Item("Price").Value

                        oPurchaseOrder_Line.CostingCode = oRs.Fields.Item("OcrCode").Value & ""
                        oPurchaseOrder_Line.CostingCode2 = oRs.Fields.Item("OcrCode2").Value & ""
                        oPurchaseOrder_Line.CostingCode3 = oRs.Fields.Item("OcrCode3").Value & ""
                        oPurchaseOrder_Line.CostingCode4 = oRs.Fields.Item("OcrCode4").Value & ""
                        oPurchaseOrder_Line.CostingCode5 = oRs.Fields.Item("OcrCode5").Value & ""
                        oPurchaseOrder_Line.UserFields.Fields.Item("U_Chamber").Value = oRs.Fields.Item("U_Chamber").Value & ""
                        oPurchaseOrder_Line.AgreementNo = CInt(oRs.Fields.Item("AbsID").Value)

                        oPurchaseOrder_Line.Add()
                        oRs.MoveNext()
                    End While

                    Dim Result As Integer = oPurchaseOrder.Add()
                    If Result <> 0 Then
                        __oApplication.StatusBar.SetText("Error: In Generating Purchase Order - " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Else

                        Dim SqlQuery As New StringBuilder

                        SqlQuery.Append("Update ""OPOR"" Set ""U_ITDocN""= (Select ""DocNum"" From OPOR Where ""U_ITDocE""='" + DocEntry + "' and ""U_Flag""='SPO'  ),")
                        SqlQuery.Append(" ""U_ITDocE""= (Select ""DocEntry"" From OPOR Where ""U_ITDocE""='" + DocEntry + "' and ""U_Flag""='SPO' )")
                        SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")


                        Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRset1.DoQuery(SqlQuery.ToString)
                        __oApplication.StatusBar.SetText("Purchase Order Generated", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    End If
                End If

                'End If

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub C_Delivery_OnResize(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oForm.Freeze(True)

                Try
                    oItem1 = oForm.Items.Item("230")
                    oItem = oForm.Items.Item("btnChamber")
                    oItem.Top = oItem1.Top + 25
                    oItem.Left = oItem1.Left
                    oItem.Width = 100
                    'oItem.Height = 20

                    oItem1 = oForm.Items.Item("230")
                    oItem = oForm.Items.Item("txtCDN")
                    oItem.Top = oItem1.Top + 25
                    oItem.Left = oItem1.Left + 110
                    oItem.Width = 40



                    oItem1 = oForm.Items.Item("230")
                    oItem = oForm.Items.Item("txtDCE")
                    oItem.Top = oItem1.Top + 25
                    oItem.Left = oItem1.Left + 150
                    oItem.Width = 40




                    'oItem1 = oForm.Items.Item("86")
                    'oItem = oForm.Items.Item("lblTC")
                    'oItem.LinkTo = "txtTC"
                    'oItem.Top = oItem1.Top + 15
                    'oItem.Left = oItem1.Left
                    'oItem.Width = 80

                    'oItem1 = oForm.Items.Item("lblTC")
                    'oItem = oForm.Items.Item("lnkT")
                    'oItem.Top = oItem1.Top
                    'oItem.Left = oItem1.Left + 90




                    'oItem1 = oForm.Items.Item("46")
                    'oItem = oForm.Items.Item("txtTC")
                    'oItem.Top = oItem1.Top + 15
                    'oItem.Left = oItem1.Left
                    'oItem.Width = 80

                    'oItem1 = oForm.Items.Item("46")
                    'oItem = oForm.Items.Item("txtTN")
                    'oItem.Top = oItem1.Top + 15
                    'oItem.Left = oItem1.Left + 85
                    'oItem.Width = 80


                    'oItem1 = oForm.Items.Item("86")
                    'oItem = oForm.Items.Item("lblTRN")
                    'oItem.Top = oItem1.Top + 33
                    'oItem.Left = oItem1.Left
                    'oItem.Width = 120

                    'oItem1 = oForm.Items.Item("46")
                    'oItem = oForm.Items.Item("txtTRN")
                    'oItem.Top = oItem1.Top + 33
                    'oItem.Left = oItem1.Left
                    'oItem.Width = 120






                Catch ex As Exception

                End Try





                oForm.Freeze(False)
                oForm.Update()
                oForm.Refresh()
            Catch ex As Exception
                oForm.Freeze(False)
                oForm.Update()
                oForm.Refresh()
                __oApplication.MessageBox("Auric_Customization -" & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub C_Delivery_OnBeforeFormLoad(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oForm.Freeze(True)
                Try
                    'oItem = oForm.Items.Add("FDCIF", SAPbouiCOM.BoFormItemTypes.it_FOLDER)
                    'oItem.FromPane = 0
                    'oItem.ToPane = 0
                    'oFolder = oItem.Specific
                    'oFolder.Caption = "Chamber Info"
                    'oItem.DataBind.SetBound(True, "", "Fo")

                    'Dim oFolder As SAPbouiCOM.Folder = Nothing
                    'Dim oItem As SAPbouiCOM.Item
                    'Dim oItemRef As SAPbouiCOM.Item = Nothing
                    'Dim iMaxPane As Integer = 0
                    'oItemRef = oForm.Items.Item("1320002137")
                    'oItem = oForm.Items.Add("MyFld", BoFormItemTypes.it_FOLDER)
                    'oItem.Top = oItemRef.Top
                    'oItem.Height = oItemRef.Height
                    'oItem.Left = oItemRef.Left + oItemRef.Width
                    'oItem.Width = oItemRef.Width
                    'oItem.Visible = True
                    'oFolder = oItem.Specific
                    'oFolder.Caption = "Chember Info"
                    'oFolder.GroupWith(oItemRef.UniqueID)
                    'oFolder.Pane = 25
                    ''Create a matrix on the folder
                    'oItem = oForm.Items.Add("MyMtx", BoFormItemTypes.it_MATRIX)
                    'oItem.FromPane = 25
                    'oItem.ToPane = 25
                    'oItemRef = oForm.Items.Item("1320002138")
                    'oItem.Top = oItemRef.Top
                    'oItem.Left = 10
                    'oItem.Width = 300
                    'oItem.Height = 150

                    oItem = oForm.Items.Add("btnChamber", SAPbouiCOM.BoFormItemTypes.it_BUTTON)
                    oItem.FromPane = 0
                    oItem.ToPane = 0
                    oButton = oItem.Specific
                    oButton.Caption = "Chamber Info"
                    oItem.Visible = True



                    oItem = oForm.Items.Add("txtCDN", SAPbouiCOM.BoFormItemTypes.it_EDIT)
                    oItem.FromPane = 0
                    oItem.ToPane = 0
                    oItem.Enabled = True
                    oEditText1 = oItem.Specific
                    oEditText1.DataBind.SetBound(True, oForm.DataSources.DBDataSources.Item(0).TableName, "U_CMDocNum")

                    oItem = oForm.Items.Add("txtDCE", SAPbouiCOM.BoFormItemTypes.it_EDIT)
                    oItem.FromPane = 0
                    oItem.ToPane = 0
                    oItem.Enabled = True
                    oEditText1 = oItem.Specific
                    oEditText1.DataBind.SetBound(True, oForm.DataSources.DBDataSources.Item(0).TableName, "U_CMDocEntry")



                    'oItem = oForm.Items.Add("lblTC", SAPbouiCOM.BoFormItemTypes.it_STATIC)
                    'oItem.FromPane = 0
                    'oItem.ToPane = 0
                    'oLabel = oItem.Specific
                    'oLabel.Caption = "Transportar Code"


                    'oItem = oForm.Items.Add("txtTC", SAPbouiCOM.BoFormItemTypes.it_EDIT)
                    'oItem.FromPane = 0
                    'oItem.ToPane = 0
                    'oItem.Enabled = True
                    'oItem.Visible = True
                    'oEditText1 = oItem.Specific
                    'oEditText1.DataBind.SetBound(True, oForm.DataSources.DBDataSources.Item(0).TableName, "U_TCode")

                    'oItem = oForm.Items.Add("txtTN", SAPbouiCOM.BoFormItemTypes.it_EDIT)
                    'oItem.FromPane = 0
                    'oItem.ToPane = 0
                    'oItem.Visible = True
                    'oItem.Enabled = True
                    'oEditText1 = oItem.Specific
                    'oEditText1.DataBind.SetBound(True, oForm.DataSources.DBDataSources.Item(0).TableName, "U_TName")


                    'oItem = oForm.Items.Add("lnkT", SAPbouiCOM.BoFormItemTypes.it_LINKED_BUTTON)
                    'oItem.FromPane = 0
                    'oItem.ToPane = 0
                    'oItem.Visible = True
                    'oItem.LinkTo = "txtTC"
                    'Dim lnkbtn As SAPbouiCOM.LinkedButton = oItem.Specific
                    'lnkbtn.LinkedObject = SAPbouiCOM.BoLinkedObject.lf_BusinessPartner




                    'oItem = oForm.Items.Add("lblTRN", SAPbouiCOM.BoFormItemTypes.it_STATIC)
                    'oItem.FromPane = 0
                    'oItem.ToPane = 0
                    'oLabel = oItem.Specific
                    'oLabel.Caption = "Truck Number"


                    'oItem = oForm.Items.Add("txtTRN", SAPbouiCOM.BoFormItemTypes.it_EDIT)
                    'oItem.FromPane = 0
                    'oItem.ToPane = 0
                    'oItem.Enabled = True
                    'oEditText1 = oItem.Specific
                    'oEditText1.DataBind.SetBound(True, oForm.DataSources.DBDataSources.Item(0).TableName, "U_TRNO")




                Catch ex As Exception

                End Try

                oForm.Freeze(False)
                oForm.Update()
                oForm.Refresh()
            Catch ex As Exception
                oForm.Freeze(False)
                oForm.Update()
                oForm.Refresh()
                __oApplication.MessageBox("Customization-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Chamber_OnAfterItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)






                If oForm.Items.Item("txtCDN").Specific.Value <> "" And oForm.Items.Item("txtDCE").Specific.Value <> "" Then
                    Dim DocEntry = oForm.Items.Item("txtDCE").Specific.Value.ToString

                    __Application.ActivateMenuItem("SAP_CHINFO")
                    Dim oForm1 As SAPbouiCOM.Form
                    oForm1 = __oApplication.Forms.GetForm("SAP_UDO_ODLN", __oApplication.Forms.ActiveForm.TypeCount)
                    oForm1.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE
                    oForm1.Items.Item("Item_17").Enabled = True
                    Dim txDocEntry As SAPbouiCOM.EditText = oForm1.Items.Item("Item_17").Specific
                    txDocEntry.Value = DocEntry.ToString()
                    oForm1.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                    oForm1.Items.Item("Item_17").Enabled = False
                Else
                    oMatrix = oForm.Items.Item("38").Specific
                    If oForm.Items.Item("4").Specific.Value = "" Then
                        __oApplication.MessageBox("Please Select CardCode", 1, "Ok", "", "")
                        BubbleEvent = False
                        Exit Sub
                    ElseIf oForm.Items.Item("U_TCode").Specific.Value = "" Then
                        __oApplication.MessageBox("Please Select Transportar Code", 1, "Ok", "", "")
                        BubbleEvent = False
                        Exit Sub
                    ElseIf oForm.Items.Item("U_TRNO").Specific.Value = "" Then
                        __oApplication.MessageBox("Please Select Truck Number", 1, "Ok", "", "")
                        BubbleEvent = False
                        Exit Sub
                    ElseIf oMatrix.Columns.Item("1").Cells.Item(1).Specific.Value = "" Then
                        __oApplication.MessageBox("Please Select ItemCode", 1, "Ok", "", "")
                        BubbleEvent = False
                        Exit Sub
                    ElseIf CDec(oMatrix.Columns.Item("11").Cells.Item(1).Specific.Value) <= 0 Then
                        __oApplication.MessageBox("Please Enter the Qty", 1, "Ok", "", "")
                        BubbleEvent = False
                        Exit Sub
                    End If

                    Dim Chamber As C_ChamberInfo = New C_ChamberInfo(__oApplication, __bobCompany)
                    Dim CardCode, TCode, TName, TruckNum, DocNum, ItemCode, ItemName, DDate, BaseType As String
                    Dim Qty As Decimal



                    BaseType = "PO"
                    CardCode = oForm.Items.Item("4").Specific.Value
                    TCode = oForm.Items.Item("U_TCode").Specific.Value
                    TName = oForm.Items.Item("U_TName").Specific.Value
                    TruckNum = oForm.Items.Item("U_TRNO").Specific.Value
                    DocNum = oForm.Items.Item("8").Specific.Value
                    DDate = oForm.Items.Item("10").Specific.Value


                    ItemCode = oMatrix.Columns.Item("1").Cells.Item(1).Specific.Value
                    ItemName = oMatrix.Columns.Item("3").Cells.Item(1).Specific.Value
                    Qty = oMatrix.Columns.Item("11").Cells.Item(1).Specific.Value
                    IsBaseForm = oForm
                    Chamber.Form_Creation(CardCode, TCode, TName, TruckNum, DDate, DocNum, ItemCode, ItemName, Qty, BaseType)


                End If




            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Public Sub ReceivedData(ByVal hTable As Hashtable, ByRef IsBaseForm As SAPbouiCOM.Form)
            Try
                oForm = IsBaseForm ' __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim refHashTable As Hashtable = New Hashtable
                refHashTable = gHashTable
                Dim enumerLookup As IDictionaryEnumerator = refHashTable.GetEnumerator
                oForm = IsBaseForm


                Dim Code As String = refHashTable("Code").ToString()
                Dim Name As String = refHashTable("Name").ToString()
                oForm.Items.Item("txtCDN").Specific.value = Code
                oForm.Items.Item("txtDCE").Specific.value = Name




            Catch ex As Exception
                __oApplication.MessageBox("SOR-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub
        Public Sub Chamber_FormLoad(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                If pVal.Before_Action = False Then
                    oForm.Items.Item("txtCDN").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                    oForm.Items.Item("txtDCE").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
                End If
            Catch ex As Exception

            End Try
        End Sub
        'Public Sub RefPO_FormLoad(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        '    Try
        '        BubbleEvent = True
        '        oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
        '        If pVal.Before_Action = False Then
        '            oForm.Items.Item("U_ITDocE").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
        '            oForm.Items.Item("U_ITDocN").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
        '        End If
        '    Catch ex As Exception

        '    End Try
        'End Sub

    End Class

End Namespace
