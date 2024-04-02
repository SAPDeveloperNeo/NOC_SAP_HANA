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
    Public Class C_TruckRouteChnge : Implements ISAP_HANA


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
#End Region

        Public Sub Form_Creation_MenuEvent(ByRef pVal As MenuEvent, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Creation_MenuEvent
            Try
                Dim sFileName As String = AppDomain.CurrentDomain.BaseDirectory & "SRF\TruckRouteChange.srf"
                Dim sFormName As String = "SAP_UDO_OTRC"
                Dim FormUID As String = LoadXMLFiles(__oApplication, sFileName)
                oForm = __oApplication.Forms.GetForm("SAP_UDO_OTRC", __oApplication.Forms.ActiveForm.TypeCount)
                If Not String.IsNullOrEmpty(FormUID) Then
                    ActiveForm(oForm, "Item_27", "1")
                    oForm.EnableMenu("1292", True)
                    oForm.EnableMenu("1293", True)
                    oForm.EnableMenu("520", True)
                    oForm.EnableMenu("519", True)

                    oForm.Mode = BoFormMode.fm_ADD_MODE

                    DefulatSetting(oForm.UniqueID, BubbleEvent)




                    'oForm.Freeze(False)
                    'oForm.Refresh()
                    'oForm.Update()
                End If
            Catch ex As Exception
                ' oForm.Freeze(False)
                'oForm.Refresh()
                'oForm.Update()
                ' __oApplication.MessageBox("[MenuEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
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

                    If pVal.ItemUID = "Item_8" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Branch_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_0" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Existing_Route_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_2" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        New_Route_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_0" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        AddRow_OnAfterLocstFocus(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_29" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        TruckNum_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)
                    End If

                ElseIf pVal.BeforeAction = True Then

                    If pVal.ItemUID = "m1" And pVal.ColUID = "Col_0" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_OCRC3_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_2" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_NEW_OCRC3_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)
                    End If

                End If

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Public Sub Form_Report_Display(ByRef paramarraylist As Object) Implements ISAP_HANA.Form_Report_Display
            Throw New NotImplementedException()
        End Sub

        Private Sub DefulatSetting(ByVal FormUID As String, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(__oApplication.Forms.ActiveForm.TypeEx, __oApplication.Forms.ActiveForm.TypeCount)
                oForm.Freeze(True)
                Dim ToDate As Date = Nothing
                Dim sc As String = __oApplication.Company.ServerDate
                ToDate = DateTime.ParseExact(sc, GetDateFormat, DateTimeFormatInfo.InvariantInfo)
                Dim PODate As SAPbouiCOM.EditText = oForm.Items.Item("Item_26").Specific
                PODate.String = ToDate.ToString("yyyyMMdd")


                oForm.Items.Item("Item_27").Specific.value = GenrateDocEntry("DocEntry", "@SAP_OTRC")
                oForm.Items.Item("Item_25").Specific.value = GenrateDocEntry("DocEntry", "@SAP_OTRC")


                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                AddRowInMatrix(oForm, "@SAP_TRC1", "m1")






                oForm.Freeze(False)

            Catch ex As Exception
                oForm.Freeze(False)
            End Try
        End Sub

        Private Sub AddRow_OnAfterLocstFocus(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific


                If m1.Columns.Item("Col_0").Cells.Item(pVal.Row).Specific.Value <> "" Then
                    AddRowInMatrix(oForm, "@SAP_TRC1", "m1")
                End If

            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Branch_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OTRC")

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                dbsrc.SetValue("U_BPLId", dbsrc.Offset, oDataTable.GetValue("BPLId", 0) & "")
                                dbsrc.SetValue("U_BPLName", dbsrc.Offset, oDataTable.GetValue("BPLName", 0) & "")


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

        Private Sub Matrix1_Route_OnBeforeChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)



                'Dim m2 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                oConditions = oCFL.GetConditions()
                oConditions = Nothing
                oCFL.SetConditions(oConditions)
                oConditions = oCFL.GetConditions()



                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific


                If m1.Columns.Item("Col_2").Cells.Item(pVal.Row).Specific.Value = "" Then
                    __oApplication.MessageBox("Select Whs Code", 1, "Ok", "", "")

                    oCondition = oConditions.Add
                    oCondition.BracketOpenNum = 1
                    oCondition.Alias = ""
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                    oCondition.CondVal = ""
                    oCondition.BracketCloseNum = 1
                    oCFL.SetConditions(oConditions)


                Else
                    Dim SqlQuery As String = "SELECT T0.""ItemCode"" FROM OITW T0 WHERE T0.""WhsCode"" ='" + m1.Columns.Item("Col_2").Cells.Item(pVal.Row).Specific.Value + "'"

                    Dim oRset As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    oRset.DoQuery(SqlQuery)

                    If oRset.RecordCount > 0 Then


                        For i As Integer = 0 To oRset.RecordCount - 1
                            If i >= 1 And i <= oRset.RecordCount - 1 Then
                                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR
                            End If
                            oCondition = oConditions.Add
                            oCondition.BracketOpenNum = 1
                            oCondition.Alias = "ItemCode"
                            oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                            oCondition.CondVal = oRset.Fields.Item("ItemCode").Value & ""
                            oCondition.BracketCloseNum = 1
                            oRset.MoveNext()

                        Next

                        'Else
                        'oCondition = oConditions.Add
                        'oCondition.BracketOpenNum = 1
                        'oCondition.Alias = "ItemCode"
                        'oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        'oCondition.CondVal = Nothing
                        'oCondition.BracketCloseNum = 1
                        'oCFL.SetConditions(oConditions)
                    End If


                End If
                oCFL.SetConditions(oConditions)

            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Existing_Route_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                Dim dt As Date = Nothing

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_TRC1")
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then
                                m1.GetLineData(pVal.Row)


                                dbsrc.SetValue("U_ERout", dbsrc.Offset, oDataTable.GetValue("OcrCode", 0) & "")
                                dbsrc.SetValue("U_ERoutN", dbsrc.Offset, oDataTable.GetValue("OcrName", 0) & "")


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

        Private Sub New_Route_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                Dim dt As Date = Nothing

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_TRC1")
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then
                                m1.GetLineData(pVal.Row)


                                dbsrc.SetValue("U_NRout", dbsrc.Offset, oDataTable.GetValue("OcrCode", 0) & "")
                                dbsrc.SetValue("U_NRoutN", dbsrc.Offset, oDataTable.GetValue("OcrName", 0) & "")


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

        Private Sub Matrix1_OCRC3_OnBeforeChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)



                Dim m2 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                oConditions = oCFL.GetConditions()
                oConditions = Nothing
                oCFL.SetConditions(oConditions)
                oConditions = oCFL.GetConditions()

                'oCondition = oConditions.Add
                'oCondition.BracketOpenNum = 1
                'oCondition.Alias = "DimCode"
                'oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                'oCondition.CondVal = "3"
                'oCondition.BracketCloseNum = 1
                'oCFL.SetConditions(oConditions)

                Dim SqlQuery2 As New StringBuilder

                SqlQuery2.Append("Select T1.""U_Route"" from ""@SAP_OTM"" T0 Inner join ""@SAP_TM2"" T1 On T0.""DocEntry""=T1.""DocEntry"" And T0.""U_TRegNo""='" + oForm.Items.Item("Item_29").Specific.Value + "' And T0.""U_Active""='Y'")

                Dim oRset As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRset.DoQuery(SqlQuery2.ToString)

                If oRset.RecordCount > 0 Then
                    For i As Integer = 0 To oRset.RecordCount - 1
                        If i >= 1 And i <= oRset.RecordCount - 1 Then
                            oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR
                        End If
                        oCondition = oConditions.Add
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "OcrCode"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = oRset.Fields.Item("U_Route").Value & ""
                        oCondition.BracketCloseNum = 1
                        oRset.MoveNext()

                    Next
                    oCFL.SetConditions(oConditions)

                Else
                    oCFL.SetConditions(oConditions)
                End If





            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Matrix1_NEW_OCRC3_OnBeforeChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)



                Dim m2 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                oConditions = oCFL.GetConditions()
                oConditions = Nothing
                oCFL.SetConditions(oConditions)
                oConditions = oCFL.GetConditions()

                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 1
                oCondition.Alias = "DimCode"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = "3"
                oCondition.BracketCloseNum = 1
                oCFL.SetConditions(oConditions)



            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub


        Private Sub TruckNum_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OTRC")

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                dbsrc.SetValue("U_TruckNum", dbsrc.Offset, oDataTable.GetValue("U_TRegNo", 0) & "")


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



    End Class
End Namespace


