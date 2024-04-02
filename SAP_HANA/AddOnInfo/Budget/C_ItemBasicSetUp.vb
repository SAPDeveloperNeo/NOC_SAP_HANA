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
    Public Class C_ItemBasicSetUp : Implements ISAP_HANA

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
        Public Property ObjectCode As String Implements ISAP_HANA.ObjectCode
            Get

            End Get
            Set(value As String)

            End Set
        End Property


        Public Sub Form_Creation_MenuEvent(ByRef pVal As MenuEvent, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Creation_MenuEvent
            Throw New NotImplementedException()
        End Sub

        Public Sub Form_Load_DataEvent(ByRef BusinessObjectInfo As BusinessObjectInfo, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Load_DataEvent
            Throw New NotImplementedException()
        End Sub

        Public Sub Form_Process_ItemEvents(FormUID As String, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Process_ItemEvents
            Try
                BubbleEvent = True
                If pVal.Before_Action = False Then
                    If pVal.FormTypeEx = "SAP_UDO_OIBS" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_RESIZE Then
                        C_Form_OnResize(FormUID, pVal, BubbleEvent)


                    ElseIf pVal.ItemUID = "3" And pVal.ColUID = "U_BPLId" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_Branch_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)



                    End If

                ElseIf pVal.Before_Action = True Then
                    If pVal.FormTypeEx = "SAP_UDO_OIBS" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_FORM_LOAD Then
                        C_Form_OnBeforeFormLoad(FormUID, pVal, BubbleEvent)

                    End If

                End If

            Catch ex As Exception

            End Try
        End Sub

        Public Sub Form_Report_Display(ByRef paramarraylist As Object) Implements ISAP_HANA.Form_Report_Display
            Throw New NotImplementedException()
        End Sub

        Public Sub Form_TMenuEvent(ByRef pVal As MenuEvent, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_TMenuEvent
            Throw New NotImplementedException()
        End Sub




        Private Sub C_Form_OnResize(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oForm.Freeze(True)


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
                AddChooseFromList()

                Dim oColumn As SAPbouiCOM.Column
                Dim oLink As SAPbouiCOM.LinkedButton
                oMatrix = oForm.Items.Item("3").Specific
                oColumns = oMatrix.Columns
                oColumn = oColumns.Item("U_BPLId")
                oColumn.ChooseFromListUID = "CFL1"
                oColumn.ChooseFromListAlias = "BPLId"




                oForm.Freeze(False)
                oForm.Update()
                oForm.Refresh()
            Catch ex As Exception
                oForm.Freeze(False)
                oForm.Update()
                oForm.Refresh()
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub AddChooseFromList()
            Try
                Dim oCFLs As SAPbouiCOM.ChooseFromListCollection
                oCFLs = oForm.ChooseFromLists
                Dim oCFL As SAPbouiCOM.ChooseFromList
                Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams
                oCFLCreationParams = __Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)

                ' Adding 2 CFL, one for the button and one for the edit text.
                oCFLCreationParams.MultiSelection = False
                oCFLCreationParams.ObjectType = "247"
                oCFLCreationParams.UniqueID = "CFL1"
                oCFL = oCFLs.Add(oCFLCreationParams)



            Catch
                MsgBox(Err.Description)
            End Try
        End Sub

        Private Sub Matrix1_Branch_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                Dim dt As Date = Nothing


                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("3").Specific

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                m1.GetLineData(pVal.Row)

                                Try
                                    m1.Columns.Item("U_BPLId").Cells.Item(pVal.Row).Specific.Value = oDataTable.GetValue("BPLId", 0)
                                Catch ex As Exception

                                End Try
                                Try
                                    m1.Columns.Item("U_BPLName").Cells.Item(pVal.Row).Specific.Value = oDataTable.GetValue("BPLName", 0)
                                Catch ex As Exception

                                End Try

                                Try
                                    m1.Columns.Item("Code").Cells.Item(pVal.Row).Specific.Value = oDataTable.GetValue("BPLId", 0)
                                Catch ex As Exception

                                End Try
                                Try
                                    m1.Columns.Item("Name").Cells.Item(pVal.Row).Specific.Value = oDataTable.GetValue("BPLName", 0)
                                Catch ex As Exception

                                End Try




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

    End Class
End Namespace


