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

    Public Class C_GRN : Implements ISAP_HANA

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
            Throw New NotImplementedException()
        End Sub

        Public Sub Form_TMenuEvent(ByRef pVal As MenuEvent, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_TMenuEvent
            Throw New NotImplementedException()
        End Sub

        Public Sub Form_Load_DataEvent(ByRef BusinessObjectInfo As BusinessObjectInfo, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Load_DataEvent
            Throw New NotImplementedException()
        End Sub

        Public Sub Form_Process_ItemEvents(FormUID As String, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Process_ItemEvents
            Try
                BubbleEvent = True

                If pVal.BeforeAction = False Then
                    If pVal.FormTypeEx = "143" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_RESIZE Then
                        C_Delivery_OnResize(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.ItemUID = "btnChamber" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        Chamber_OnAfterItemPressed(FormUID, pVal, BubbleEvent)
                    End If

                ElseIf pVal.BeforeAction = True Then



                    If pVal.FormTypeEx = "143" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_LOAD Then
                        C_Delivery_OnBeforeFormLoad(FormUID, pVal, BubbleEvent)

                    End If
                End If

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Public Sub Form_Report_Display(ByRef paramarraylist As Object) Implements ISAP_HANA.Form_Report_Display
            Throw New NotImplementedException()
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

                    oItem1 = oForm.Items.Item("230")
                    oItem = oForm.Items.Item("btnAcdnt")
                    oItem.Top = oItem1.Top + 25
                    oItem.Left = oItem1.Left + 195
                    oItem.Width = 100


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


                    oItem = oForm.Items.Add("btnChamber", SAPbouiCOM.BoFormItemTypes.it_BUTTON)
                    oItem.FromPane = 0
                    oItem.ToPane = 0
                    oButton = oItem.Specific
                    oButton.Caption = "Chamber Info"
                    oItem.Visible = True

                    oItem = oForm.Items.Add("btnAcdnt", SAPbouiCOM.BoFormItemTypes.it_BUTTON)
                    oItem.FromPane = 0
                    oItem.ToPane = 0
                    oButton = oItem.Specific
                    oButton.Caption = "Acciedent Info"
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
                    'If oForm.Items.Item("4").Specific.Value = "" Then
                    '    __oApplication.MessageBox("Please Select CardCode", 1, "Ok", "", "")
                    '    BubbleEvent = False
                    '    Exit Sub
                    'ElseIf oForm.Items.Item("txtTC").Specific.Value = "" Then
                    '    __oApplication.MessageBox("Please Select Transportar Code", 1, "Ok", "", "")
                    '    BubbleEvent = False
                    '    Exit Sub
                    'ElseIf oForm.Items.Item("txtTRN").Specific.Value = "" Then
                    '    __oApplication.MessageBox("Please Select Truck Number", 1, "Ok", "", "")
                    '    BubbleEvent = False
                    '    Exit Sub
                    If oMatrix.Columns.Item("1").Cells.Item(1).Specific.Value = "" Then
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



                    BaseType = "GRN"
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
    End Class
End Namespace

