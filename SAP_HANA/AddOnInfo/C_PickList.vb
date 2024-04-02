
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
    Public Class C_PickList : Implements ISAP_HANA

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
        Dim oFolder As SAPbouiCOM.Folder
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
                    If pVal.FormTypeEx = "85" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_RESIZE Then
                        C_Form_OnResize(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.ItemUID = "btnChamber" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        Chamber_OnAfterItemPressed(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.ItemUID = "1" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        Add_OnAfterItemPressed(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.ItemUID = "U_CMDocEntry" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then

                        ChamberLostFocus(FormUID, pVal, BubbleEvent)
                    End If

                ElseIf pVal.BeforeAction = True Then



                    If pVal.FormTypeEx = "85" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_LOAD Then
                        C_Form_OnBeforeFormLoad(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "1" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        Add_OnBeforeItemPressed(FormUID, pVal, BubbleEvent)


                    End If
                End If

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Public Sub Form_Report_Display(ByRef paramarraylist As Object) Implements ISAP_HANA.Form_Report_Display
            Throw New NotImplementedException()
        End Sub

        Private Sub C_Form_OnResize(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oForm.Freeze(True)

                Try
                    oItem1 = oForm.Items.Item("4")
                    oItem = oForm.Items.Item("btnChamber")
                    oItem.Top = oItem1.Top + 0
                    oItem.Left = oItem1.Left + 80
                    oItem.Width = 100
                    'oItem.Height = 20

                    'oItem1 = oForm.Items.Item("4")
                    'oItem = oForm.Items.Item("txtCDN")
                    'oItem.Top = oItem1.Top + 0
                    'oItem.Left = oItem1.Left + 190
                    'oItem.Width = 40



                    'oItem1 = oForm.Items.Item("4")
                    'oItem = oForm.Items.Item("txtDCE")
                    'oItem.Top = oItem1.Top + 0
                    'oItem.Left = oItem1.Left + 240
                    'oItem.Width = 40




                    'oItem1 = oForm.Items.Item("17")
                    'oItem = oForm.Items.Item("lblTRN")
                    'oItem.Top = oItem1.Top + 50
                    'oItem.Left = oItem1.Left
                    'oItem.Width = 120

                    'oItem1 = oForm.Items.Item("18")
                    'oItem = oForm.Items.Item("txtTRN")
                    'oItem.Top = oItem1.Top + 50
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

        Private Sub C_Form_OnBeforeFormLoad(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oForm.Freeze(True)
                Try

                    oItem = oForm.Items.Add("btnChamber", SAPbouiCOM.BoFormItemTypes.it_BUTTON)
                    oItem.FromPane = 0
                    oItem.ToPane = 0
                    oButton = oItem.Specific
                    oButton.Caption = "Chamber Allocation"
                    oItem.Visible = True



                    'oItem = oForm.Items.Add("txtCDN", SAPbouiCOM.BoFormItemTypes.it_EDIT)
                    'oItem.FromPane = 0
                    'oItem.ToPane = 0
                    'oItem.Enabled = True
                    'oEditText1 = oItem.Specific
                    'oEditText1.DataBind.SetBound(True, oForm.DataSources.DBDataSources.Item(0).TableName, "U_CMDocNum")

                    'oItem = oForm.Items.Add("txtDCE", SAPbouiCOM.BoFormItemTypes.it_EDIT)
                    'oItem.FromPane = 0
                    'oItem.ToPane = 0
                    'oItem.Enabled = True
                    'oEditText1 = oItem.Specific
                    'oEditText1.DataBind.SetBound(True, oForm.DataSources.DBDataSources.Item(0).TableName, "U_CMDocEntry")




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






                If oForm.Items.Item("U_CMDocEntry").Specific.Value <> "" And oForm.Items.Item("U_CMDocNum").Specific.Value <> "" Then
                    Dim DocEntry = oForm.Items.Item("U_CMDocEntry").Specific.Value.ToString

                    __Application.ActivateMenuItem("SAP_CHALLOCATION")
                    Dim oForm1 As SAPbouiCOM.Form
                    oForm1 = __oApplication.Forms.GetForm("SAP_UDO_OPKL", __oApplication.Forms.ActiveForm.TypeCount)
                    oForm1.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE
                    oForm1.Items.Item("Item_17").Enabled = True
                    Dim txDocEntry As SAPbouiCOM.EditText = oForm1.Items.Item("Item_17").Specific
                    txDocEntry.Value = DocEntry.ToString()
                    oForm1.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                    oForm1.Items.Item("Item_17").Enabled = False
                Else


                    If oForm.Items.Item("U_TRNO").Specific.Value = "" Then
                        __oApplication.MessageBox("Please Select Truck Number", 1, "Ok", "", "")
                        BubbleEvent = False
                        Exit Sub
                    End If

                    Dim TruckNum, DocNum As String
                    Dim Qty, Capacity As Decimal


                    DocNum = oForm.Items.Item("6").Specific.Value
                    TruckNum = oForm.Items.Item("U_TRNO").Specific.Value


                    Dim CountQ As String = ""
                    CountQ = "CALL SAP_GET_Capacity ('" + TruckNum + "')"
                    Dim oRs1Count As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    oRs1Count.DoQuery(CountQ)
                    If oRs1Count.RecordCount > 0 Then
                        Capacity = oRs1Count.Fields.Item("Capacity").Value
                    End If


                    CountQ = "CALL SAP_GET_PickListQty ('" + DocNum + "')"
                    oRs1Count.DoQuery(CountQ)
                    If oRs1Count.RecordCount > 0 Then
                        Qty = oRs1Count.Fields.Item("Qty").Value
                    End If

                    'If Capacity <= Qty Then
                    '    __oApplication.MessageBox("Pick List Total Qty Not More Than Chamber Total Capacity", 1, "Ok", "", "")
                    '    BubbleEvent = False
                    '    Exit Sub
                    'End If


                    Dim Chamber As C_ChamberAllocation = New C_ChamberAllocation(__oApplication, __bobCompany)
                    IsBaseForm = oForm
                    Chamber.Form_Creation(TruckNum, DocNum, "PICK")


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
                oForm.Items.Item("U_CMDocNum").Specific.value = Code
                oForm.Items.Item("U_CMDocEntry").Specific.value = Name




            Catch ex As Exception
                __oApplication.MessageBox("SOR-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub




        Private Sub Add_OnBeforeItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or oForm.Mode = BoFormMode.fm_UPDATE_MODE Then



                    PKLNO = ""
                    PKLTR = ""
                    PKLNO = oForm.Items.Item("6").Specific.Value.ToString
                    PKLTR = oForm.Items.Item("U_TRNO").Specific.Value.ToString





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
                    If PKLNO <> "" And PKLTR <> "" Then
                        DocEntry = GetValue(" Select  T0.""AbsEntry"" FROM ""OPKL""  T0 WHERE T0.""AbsEntry"" ='" + PKLNO + "' And T0.""U_TRNO"" ='" + PKLTR + "'", "AbsEntry")
                        If DocEntry <> "" Then

                            Try


                                Dim ChamberDocEntry As String = GetValue("Select  T0.""U_CMDocEntry"" FROM ""OPKL""  T0 WHERE T0.""AbsEntry"" ='" + DocEntry + "'", "U_CMDocEntry")
                                Dim ChamberDocNum As String = GetValue("Select  T0.""U_CMDocNum"" FROM ""OPKL""  T0 WHERE T0.""AbsEntry"" ='" + DocEntry + "'", "U_CMDocNum")

                                If ChamberDocEntry <> "" And ChamberDocNum <> "" Then
                                    Dim SqlQuery As New StringBuilder
                                    SqlQuery.Append("Update ""@SAP_OPKL"" Set ""U_DocNum""= (Select ""DocNum"" From OPKL Where ""AbsEntry""='" + DocEntry + "' )")
                                    SqlQuery.Append("Where ""DocEntry""='" + ChamberDocEntry + "'")
                                    Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                    oRset1.DoQuery(SqlQuery.ToString)

                                End If

                            Catch ex As Exception

                            End Try

                        End If

                    End If

                    PKLNO = ""
                    PKLTR = ""


                End If
            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub ChamberLostFocus(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("11").Specific


                For ADDRow As Integer = 1 To m1.RowCount

                    Dim SqlQuery2 As String = "Call SAP_GET_PickListQty_CardCode ('" + oForm.Items.Item("U_CMDocEntry").Specific.Value + "','" + m1.Columns.Item("17").Cells.Item(ADDRow).Specific.Value + "','" + m1.Columns.Item("12").Cells.Item(ADDRow).Specific.Value + "')"



                    Dim oRset As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    oRset.DoQuery(SqlQuery2.ToString)

                    If oRset.RecordCount > 0 Then
                        m1.Columns.Item("19").Cells.Item(ADDRow).Specific.Value = oRset.Fields.Item("Qty").Value
                    End If

                Next


            Catch ex As Exception

            End Try
        End Sub

    End Class
End Namespace


