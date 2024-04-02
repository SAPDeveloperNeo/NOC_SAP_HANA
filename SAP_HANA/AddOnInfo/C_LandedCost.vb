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
    Public Class C_LandedCost : Implements ISAP_HANA

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
                    If pVal.FormTypeEx = "992" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_RESIZE Then
                        C_LandedCost_OnResize(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.ItemUID = "btnCal" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        LandedCost_OnAfterItemPressed(FormUID, pVal, BubbleEvent)
                        'ElseIf pVal.FormTypeEx = "992" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_LOAD Then
                        '    Field_FormLoad(FormUID, pVal, BubbleEvent)
                    End If

                ElseIf pVal.BeforeAction = True Then



                    If pVal.FormTypeEx = "992" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_LOAD Then
                        C_LandedCost_OnBeforeFormLoad(FormUID, pVal, BubbleEvent)

                    End If
                End If

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Public Sub Form_Report_Display(ByRef paramarraylist As Object) Implements ISAP_HANA.Form_Report_Display
            Throw New NotImplementedException()
        End Sub




        Private Sub C_LandedCost_OnResize(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oForm.Freeze(True)

                Try
                    oItem1 = oForm.Items.Item("68")
                    oItem = oForm.Items.Item("txtqty")
                    oItem.Top = oItem1.Top + 30
                    oItem.Left = oItem1.Left
                    oItem.Width = 100
                    oItem.Enabled = False
                    'oItem.Height = 20

                    oItem1 = oForm.Items.Item("68")
                    oItem = oForm.Items.Item("btnCal")
                    oItem.Top = oItem1.Top + 80
                    oItem.Left = oItem1.Left
                    oItem.Width = 100







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

        Private Sub C_LandedCost_OnBeforeFormLoad(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oForm.Freeze(True)
                Try


                    oItem = oForm.Items.Add("btnCal", SAPbouiCOM.BoFormItemTypes.it_BUTTON)
                    oItem.FromPane = 6
                    oItem.ToPane = 6
                    oButton = oItem.Specific
                    oButton.Caption = "Calculate Total Qty"
                    oItem.Visible = True



                    oItem = oForm.Items.Add("txtqty", SAPbouiCOM.BoFormItemTypes.it_EDIT)
                    oItem.FromPane = 6
                    oItem.ToPane = 6
                    oItem.Enabled = True
                    oEditText1 = oItem.Specific
                    oEditText1.DataBind.SetBound(True, oForm.DataSources.DBDataSources.Item(0).TableName, "U_Qty")

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

        Private Sub LandedCost_OnAfterItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)



                Dim Qty As Decimal
                Try
                    Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("51").Specific
                    For iRow As Integer = 1 To m1.VisualRowCount
                        Qty = Qty + CDec(m1.Columns.Item("3").Cells.Item(iRow).Specific.Value)

                    Next

                    oForm.Items.Item("txtqty").Specific.value = Qty
                    oForm.Items.Item("txtqty").Enabled = False


                Catch ex As Exception

                End Try






            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        'Public Sub Field_FormLoad(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        '    Try
        '        BubbleEvent = True
        '        oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
        '        If pVal.Before_Action = False Then
        'oForm.Items.Item("Ref1").Enabled = True
        'oForm.Items.Item("41").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_True)
        'oForm.Items.Item("txtDCE").SetAutoManagedAttribute(SAPbouiCOM.BoAutoManagedAttr.ama_Editable, -1, SAPbouiCOM.BoModeVisualBehavior.mvb_False)
        '        End If
        '    Catch ex As Exception

        '    End Try
        'End Sub



    End Class

End Namespace
