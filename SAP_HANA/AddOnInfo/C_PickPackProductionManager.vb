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


Namespace SAP_HANA
    Public Class C_PickPackProductionManager : Implements ISAP_HANA

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
                    If pVal.FormTypeEx = "81" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_RESIZE Then
                        C_OnResize(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.ItemUID = "txtTC" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        Search_OnAfterItemClick(FormUID, pVal, BubbleEvent)
                    End If

                ElseIf pVal.BeforeAction = True Then



                    If pVal.FormTypeEx = "81" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_LOAD Then
                        C_OnBeforeFormLoad(FormUID, pVal, BubbleEvent)

                    End If
                End If

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try

        End Sub

        Public Sub Form_Report_Display(ByRef paramarraylist As Object) Implements ISAP_HANA.Form_Report_Display
            Throw New NotImplementedException()
        End Sub



        Private Sub C_OnResize(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oForm.Freeze(True)

                Try





                    oItem1 = oForm.Items.Item("10000024")
                    oItem = oForm.Items.Item("lblTC")
                    oItem.LinkTo = "txtTC"
                    oItem.Top = oItem1.Top
                    oItem.Left = oItem1.Left + 300
                    oItem.Width = 80


                    oItem1 = oForm.Items.Item("10000024")
                    oItem = oForm.Items.Item("txtTC")
                    oItem.Top = oItem1.Top
                    oItem.Left = oItem1.Left + 400
                    oItem.Width = 80

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
                __oApplication.MessageBox("Customization -" & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub C_OnBeforeFormLoad(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oForm.Freeze(True)
                Try




                    oItem = oForm.Items.Add("lblTC", SAPbouiCOM.BoFormItemTypes.it_STATIC)
                    oItem.FromPane = 0
                    oItem.ToPane = 0
                    oLabel = oItem.Specific
                    oLabel.Caption = "BP Code"


                    oItem = oForm.Items.Add("txtTC", SAPbouiCOM.BoFormItemTypes.it_EDIT)
                    oItem.FromPane = 0
                    oItem.ToPane = 0
                    oItem.Enabled = True
                    oItem.Visible = True
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


        Private Sub Search_OnAfterItemClick(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("10").Specific
                Dim ValueSearch As String = oForm.Items.Item("txtTC").Specific.value

                If ValueSearch = "" Then
                    ' __oApplication.MessageBox("Enter Value for search Column", 1, "Ok", "", "")
                    BubbleEvent = False
                    Exit Sub
                End If

                If m1.VisualRowCount > 0 Then


                    For iRow As Integer = 1 To m1.VisualRowCount
                        Dim ColValue As SAPbouiCOM.EditText = DirectCast(m1.Columns.Item("9").Cells.Item(iRow).Specific, SAPbouiCOM.EditText)
                        Dim Value As String = ""
                        Dim ValueSer As String = ""

                        Value = ColValue.[String].ToString()
                        ValueSer = ValueSearch.ToString()

                        If Value.ToUpper().StartsWith(ValueSer.ToUpper()) = True Then
                            Dim Docnum As SAPbouiCOM.EditText = DirectCast(m1.Columns.Item("11").Cells.Item(iRow).Specific, SAPbouiCOM.EditText)
                            Dim DocValue As String = Docnum.[String].ToString()
                            oForm.Items.Item("10000024").Specific.Value = DocValue
                            'm1.SelectRow(iRow, True, False)
                            'BubbleEvent = False
                            Return
                        End If

                    Next

                Else
                    '__oApplication.MessageBox("No Rows found for serching.....", 1, "Ok", "", "")
                    BubbleEvent = False
                    Return
                End If

            Catch ex As Exception
                __oApplication.MessageBox("[Fill Matrix] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

    End Class
End Namespace
