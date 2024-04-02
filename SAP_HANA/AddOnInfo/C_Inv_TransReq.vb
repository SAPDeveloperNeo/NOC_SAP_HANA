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
    Public Class C_Inv_TransReq : Implements ISAP_HANA



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
            Try
                Dim sFileName As String = AppDomain.CurrentDomain.BaseDirectory & "SRF\InventoryTransferRequest.srf"
                Dim sFormName As String = "SAP_UDO_OITR"
                Dim FormUID As String = LoadXMLFiles(__oApplication, sFileName)
                oForm = __oApplication.Forms.GetForm("SAP_UDO_OITR", __oApplication.Forms.ActiveForm.TypeCount)
                If Not String.IsNullOrEmpty(FormUID) Then
                    ActiveForm(oForm, "Item_15", "1")
                    oForm.EnableMenu("1292", True)
                    oForm.EnableMenu("1293", True)
                    oForm.EnableMenu("520", True)
                    oForm.EnableMenu("519", True)
                    oForm.EnableMenu("5890", True)
                    oForm.ReportType = GetValue("Select Code From RTYP Where Name='InvRequest'", "Code")
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

        Public Sub Form_Load_DataEvent(ByRef BusinessObjectInfo As BusinessObjectInfo, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Load_DataEvent
            Throw New NotImplementedException()
        End Sub

        Public Sub Form_Process_ItemEvents(FormUID As String, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Process_ItemEvents
            Try
                BubbleEvent = True

                If pVal.BeforeAction = False Then
                    If pVal.ItemUID = "Item_23" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        FromWhs_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_25" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        ToWhs_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_22" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        FromBranch_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_24" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        ToBranch_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_13" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Route_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_0" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        ItemCode_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_2" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        FWarehouse_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_3" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        TWarehouse_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "1" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        ADD_OnAfterItemPreess(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_13" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_OCRC_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_9" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_OCRC2_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_10" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_OCRC3_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_11" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_OCRC4_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_12" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_OCRC5_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m2" And pVal.ColUID = "Col_0" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        TruckRegNo_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.ItemUID = "m2" And pVal.ColUID = "Col_0" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        TruckRegNo_OnAfterLostFocus(FormUID, pVal, BubbleEvent)



                    End If

                ElseIf pVal.BeforeAction = True Then

                    If pVal.ItemUID = "m1" And pVal.ColUID = "Col_13" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_OCRC_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_9" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_OCRC2_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_10" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_OCRC3_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_11" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_OCRC4_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_12" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_OCRC5_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_25" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Twarhouse_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_23" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Fwarhouse_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_0" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_ItemCode_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.ItemUID = "1" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        Add_OnBeforeItemPressed(FormUID, pVal, BubbleEvent)


                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_2" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_FWhs_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_3" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_TWhs_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_24" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        TBranch_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m2" And pVal.ColUID = "Col_0" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        TruckRegNo_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    End If
                End If

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Public Sub Form_Report_Display(ByRef paramarraylist As Object) Implements ISAP_HANA.Form_Report_Display
            Throw New NotImplementedException()
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
                    AddRowInMatrix(oForm, "@SAP_ITR1", "m1")




                End If
            Catch ex As Exception

            End Try
        End Sub


        Private Sub DefulatSetting(ByVal FormUID As String, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(__oApplication.Forms.ActiveForm.TypeEx, __oApplication.Forms.ActiveForm.TypeCount)
                If oForm.Items.Item("Item_15").Specific.Value = "" Then


                    Dim ToDate As Date = Nothing
                    Dim sc As String = __oApplication.Company.ServerDate
                    ToDate = DateTime.ParseExact(sc, GetDateFormat, DateTimeFormatInfo.InvariantInfo)
                    Dim PODate As SAPbouiCOM.EditText = oForm.Items.Item("Item_26").Specific
                    PODate.String = ToDate.ToString("yyyyMMdd")

                    Dim StartDate As SAPbouiCOM.EditText = oForm.Items.Item("Item_27").Specific
                    StartDate.String = ToDate.ToString("yyyyMMdd")

                    oForm.Items.Item("Item_15").Specific.value = GenrateDocEntry("DocEntry", "@SAP_OITR")

                    Dim cbSeries As SAPbouiCOM.ComboBox = oForm.Items.Item("Item_21").Specific
                    cbSeries.ValidValues.LoadSeries("SAP_UDO_OITR", SAPbouiCOM.BoSeriesMode.sf_Add)
                    cbSeries.SelectExclusive(0, SAPbouiCOM.BoSearchKey.psk_Index)

                    oForm.Items.Item("Item_14").Specific.Value = GetValue("SELECT T0.""NextNumber"" FROM NNM1 T0 WHERE T0.""ObjectCode"" ='SAP_UDO_OITR' And  T0.""Series"" ='" + cbSeries.Selected.Value + "'", "NextNumber")




                    Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                    AddRowInMatrix(oForm, "@SAP_ITR1", "m1")

                    Dim m2 As SAPbouiCOM.Matrix = oForm.Items.Item("m2").Specific
                    AddRowInMatrix(oForm, "@SAP_ITR2", "m2")
                    oForm.PaneLevel = 1
                End If
            Catch ex As Exception

            End Try
        End Sub

        Private Sub FromWhs_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OITR")

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                dbsrc.SetValue("U_FW", dbsrc.Offset, oDataTable.GetValue("WhsCode", 0) & "")
                                dbsrc.SetValue("U_FWN", dbsrc.Offset, oDataTable.GetValue("WhsName", 0) & "")

                                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific


                                For ADDRow As Integer = 1 To m1.RowCount
                                    Try
                                        m1.Columns.Item("Col_2").Cells.Item(ADDRow).Specific.Value = oDataTable.GetValue("WhsCode", 0)
                                    Catch ex As Exception

                                    End Try

                                Next
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

        Private Sub ToWhs_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OITR")

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                dbsrc.SetValue("U_TW", dbsrc.Offset, oDataTable.GetValue("WhsCode", 0) & "")
                                dbsrc.SetValue("U_TWN", dbsrc.Offset, oDataTable.GetValue("WhsName", 0) & "")




                                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific


                                For ADDRow As Integer = 1 To m1.RowCount
                                    Try
                                        m1.Columns.Item("Col_3").Cells.Item(ADDRow).Specific.Value = oDataTable.GetValue("WhsCode", 0)
                                    Catch ex As Exception

                                    End Try

                                Next


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


        Private Sub FromBranch_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OITR")

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                dbsrc.SetValue("U_FB", dbsrc.Offset, oDataTable.GetValue("BPLId", 0) & "")
                                dbsrc.SetValue("U_FBN", dbsrc.Offset, oDataTable.GetValue("BPLName", 0) & "")


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

        Private Sub ToBranch_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OITR")

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                dbsrc.SetValue("U_TB", dbsrc.Offset, oDataTable.GetValue("BPLId", 0) & "")
                                dbsrc.SetValue("U_TBN", dbsrc.Offset, oDataTable.GetValue("BPLName", 0) & "")

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


        Private Sub Route_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OITR")

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                dbsrc.SetValue("U_Route", dbsrc.Offset, oDataTable.GetValue("PrcCode", 0) & "")

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



        Private Sub ItemCode_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                Dim dt As Date = Nothing

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_ITR1")
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then
                                m1.GetLineData(pVal.Row)


                                dbsrc.SetValue("U_ItemCode", dbsrc.Offset, oDataTable.GetValue("ItemCode", 0) & "")
                                dbsrc.SetValue("U_ItemName", dbsrc.Offset, oDataTable.GetValue("ItemName", 0) & "")

                                dbsrc.SetValue("U_UOM", dbsrc.Offset, oDataTable.GetValue("InvntryUom", 0) & "")
                                dbsrc.SetValue("U_UOMG", dbsrc.Offset, oDataTable.GetValue("UgpEntry", 0) & "")
                                dbsrc.SetValue("U_FW", dbsrc.Offset, oForm.Items.Item("Item_23").Specific.value.ToString & "")
                                dbsrc.SetValue("U_TW", dbsrc.Offset, oForm.Items.Item("Item_25").Specific.value.ToString & "")

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

        Private Sub FWarehouse_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                Dim dt As Date = Nothing

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_ITR1")
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then
                                m1.GetLineData(pVal.Row)


                                dbsrc.SetValue("U_FW", dbsrc.Offset, oDataTable.GetValue("WhsCode", 0) & "")


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
        Private Sub TWarehouse_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                Dim dt As Date = Nothing

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_ITR1")
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then
                                m1.GetLineData(pVal.Row)


                                dbsrc.SetValue("U_TW", dbsrc.Offset, oDataTable.GetValue("WhsCode", 0) & "")


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

        Private Sub ADD_OnAfterItemPreess(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                If oForm.Mode = BoFormMode.fm_ADD_MODE Then
                    DefulatSetting(oForm.UniqueID, BubbleEvent)
                End If

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub


        Private Sub Matrix1_OCRC_OnBeforeChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
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
                oCondition.CondVal = "1"
                oCondition.BracketCloseNum = 1
                oCFL.SetConditions(oConditions)



            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Matrix1_OCRC_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                Dim dt As Date = Nothing

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_ITR1")
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                m1.GetLineData(pVal.Row)

                                dbsrc.SetValue("U_OCRC", dbsrc.Offset, oDataTable.GetValue("OcrCode", 0) & "")

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

        Private Sub Matrix1_OCRC2_OnBeforeChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
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
                oCondition.CondVal = "2"
                oCondition.BracketCloseNum = 1
                oCFL.SetConditions(oConditions)



            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Matrix1_OCRC2_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                Dim dt As Date = Nothing

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_ITR1")
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                m1.GetLineData(pVal.Row)

                                dbsrc.SetValue("U_OCRC2", dbsrc.Offset, oDataTable.GetValue("OcrCode", 0) & "")

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

        Private Sub Matrix1_OCRC3_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                Dim dt As Date = Nothing

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_ITR1")
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                m1.GetLineData(pVal.Row)

                                dbsrc.SetValue("U_OCRC3", dbsrc.Offset, oDataTable.GetValue("OcrCode", 0) & "")

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


        Private Sub Matrix1_OCRC4_OnBeforeChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
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
                oCondition.CondVal = "4"
                oCondition.BracketCloseNum = 1


                'oCondition = oConditions.Add
                'oCondition.BracketOpenNum = 1
                'oCondition.Alias = "U_Branch"
                'oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                'oCondition.CondVal = oForm.Items.Item("Item_24").Specific.Value.ToString
                'oCondition.BracketCloseNum = 1

                'oCondition = oConditions.Add
                'oCondition.BracketOpenNum = 1
                'oCondition.Alias = "U_Branch"
                'oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_CONTAIN
                'oCondition.CondVal = "NA"
                'oCondition.BracketCloseNum = 1
                oCFL.SetConditions(oConditions)



            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Matrix1_OCRC4_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                Dim dt As Date = Nothing

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_ITR1")
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                m1.GetLineData(pVal.Row)

                                dbsrc.SetValue("U_OCRC4", dbsrc.Offset, oDataTable.GetValue("OcrCode", 0) & "")

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

        Private Sub Matrix1_OCRC5_OnBeforeChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
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
                oCondition.CondVal = "5"
                oCondition.BracketCloseNum = 1


                'oCondition = oConditions.Add
                'oCondition.BracketOpenNum = 1
                'oCondition.Alias = "U_Branch"
                'oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                'oCondition.CondVal = oForm.Items.Item("Item_24").Specific.Value.ToString
                'oCondition.BracketCloseNum = 1

                'oCondition = oConditions.Add
                'oCondition.BracketOpenNum = 1
                'oCondition.Alias = "U_Branch"
                'oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_CONTAIN
                'oCondition.CondVal = "NA"
                'oCondition.BracketCloseNum = 1



                oCFL.SetConditions(oConditions)



            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Matrix1_OCRC5_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                Dim dt As Date = Nothing

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_ITR1")
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                m1.GetLineData(pVal.Row)

                                dbsrc.SetValue("U_OCRC5", dbsrc.Offset, oDataTable.GetValue("OcrCode", 0) & "")

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



        Private Sub Twarhouse_OnBeforeChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
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
                oCondition.Alias = "BPLId"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = oForm.Items.Item("Item_24").Specific.Value.ToString
                oCondition.BracketCloseNum = 1



                oCFL.SetConditions(oConditions)

            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Fwarhouse_OnBeforeChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
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
                oCondition.Alias = "BPLId"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = oForm.Items.Item("Item_22").Specific.Value.ToString
                oCondition.BracketCloseNum = 1



                oCFL.SetConditions(oConditions)

            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Matrix1_ItemCode_OnBeforeChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
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

                'oCondition = oConditions.Add
                'oCondition.BracketOpenNum = 1
                'oCondition.Alias = "WhsCode"
                'oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                'oCondition.CondVal = oForm.Items.Item("Item_23").Specific.Value
                'oCondition.BracketCloseNum = 1
                'oCFL.SetConditions(oConditions)



                Dim SqlQuery As String = "SELECT T0.""ItemCode"" FROM OITW T0 WHERE T0.""WhsCode"" ='" + oForm.Items.Item("Item_25").Specific.Value + "'"

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

                oCFL.SetConditions(oConditions)


            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub


        Private Sub Add_OnBeforeItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                    Dim Status As String
                    If oForm.Items.Item("Item_15").Specific.Value <> "" And oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then

                        Dim cnt As Integer = 0
                        'cnt = GetValue("Select count(*) AS ""Count"" from ""@SAP_OITR"" where ""DocEntry""=" + oForm.Items.Item("Item_15").Specific.Value.ToString + " and ifnull(""U_ITDocE"",'')<>''", "Count")
                        cnt = GetValue("Select count(*) AS ""Count"" from ""@SAP_OIT"" where ""U_ITRDocE""=" + oForm.Items.Item("Item_15").Specific.Value.ToString + "", "Count")
                        'If cnt > 0 Then
                        '    __oApplication.MessageBox("Inventory Transfer already created ,Please reverse the document", 1, "Ok", "", "")
                        '    BubbleEvent = False
                        '    Exit Sub
                        'End If
                    End If

                    If oForm.Items.Item("Item_23").Specific.Value = "" Then
                        __oApplication.MessageBox("From Warehouse Can Not Blank", 1, "Ok", "", "")
                        BubbleEvent = False
                        Exit Sub

                    ElseIf oForm.Items.Item("Item_25").Specific.Value = "" Then
                        __oApplication.MessageBox("To Warehouse Can Not Blank", 1, "Ok", "", "")
                        BubbleEvent = False
                        Exit Sub
                    End If


                    Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                    If m1.RowCount <= 0 Then
                        __oApplication.MessageBox("Matrix Can Not Blank ", 1, "Ok", "", "")
                        BubbleEvent = False
                        Exit Sub
                    End If


                    Dim Flag As Boolean
                    Dim mainqty As Double = 0.0
                    For iRow As Integer = 1 To m1.VisualRowCount
                        If CDec(m1.Columns.Item("Col_8").Cells.Item(iRow).Specific.Value) > 0 Then


                            mainqty = mainqty + CDbl(m1.Columns.Item("Col_8").Cells.Item(iRow).Specific.Value)
                        End If
                    Next

                    For iRow As Integer = 1 To m1.VisualRowCount


                        If (m1.Columns.Item("Col_0").Cells.Item(iRow).Specific.Value) <> "" Then
                            Flag = True
                            ' Exit For
                        Else
                            Flag = False
                            Exit For
                        End If

                        If CDec(m1.Columns.Item("Col_8").Cells.Item(iRow).Specific.Value) > 0 Then
                            Flag = True
                            ' Exit For
                        Else
                            Flag = False
                            Exit For
                        End If
                    Next
                    Dim m2 As SAPbouiCOM.Matrix = oForm.Items.Item("m2").Specific
                    Dim Flag1 As Boolean = False
                    Dim qty As Decimal = 0
                    Dim CNT1 As Integer = 0
                    Dim STATUS1 As String

                    If oForm.Items.Item("Item_15").Specific.Value <> "" And oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then

                        'If m2.RowCount > 0 Then


                        For iRow As Integer = 1 To m2.VisualRowCount

                            If (m2.Columns.Item("Col_5").Cells.Item(iRow).Specific.Value) <> "" And (m2.Columns.Item("Col_4").Cells.Item(iRow).Specific.Value) <> "" And oForm.Items.Item("Item_15").Specific.Value <> "" And (m2.Columns.Item("Col_0").Cells.Item(iRow).Specific.Value) <> "" Then

                                CNT1 = GetValue("Select COUNT(*) AS ""Count"" From OWTR Where ""DocEntry"" In(  Select  U_ITDE from ""@SAP_OIT"" WHERE ""DocEntry""='" + m2.Columns.Item("Col_5").Cells.Item(iRow).Specific.Value + "') And ""CANCELED""='Y'", "Count")
                                If CNT1 = 0 Then


                                    STATUS1 = GetValue("Select U_TRSTATUS As ""Count"" from ""@SAP_ITR2"" where ""DocEntry""=" + oForm.Items.Item("Item_15").Specific.Value.ToString + " And U_TRNO='" + m2.Columns.Item("Col_4").Cells.Item(iRow).Specific.Value + "'", "Count")
                                    If m2.Columns.Item("Col_4").Cells.Item(iRow).Specific.Value <> STATUS1 Then
                                        Flag1 = True
                                        __oApplication.MessageBox("Inventory Transfer already created , Please reverse the document", 1, "Ok", "", "")
                                        BubbleEvent = False
                                        Exit For

                                    End If
                                End If
                            End If
                        Next
                        'End If
                    End If
                    'For iRow As Integer = 1 To m2.VisualRowCount

                    '    If (m2.Columns.Item("Col_5").Cells.Item(iRow).Specific.Value) = "" And (m2.Columns.Item("Col_4").Cells.Item(iRow).Specific.Value) = "" Then

                    '        STATUS1 = GetValue("Select U_TRSTATUS AS ""Count"" from ""@SAP_ITR2"" where ""DocEntry""=" + oForm.Items.Item("Item_15").Specific.Value.ToString + " and U_TRNO='" + m2.Columns.Item("Col_4").Cells.Item(iRow).Specific.Value + "'", "Count")
                    '        If m2.Columns.Item("Col_4").Cells.Item(iRow).Specific.Value <> STATUS1 Then
                    '            Flag1 = True
                    '            __oApplication.MessageBox("Inventory Transfer already created , Please reverse the document", 1, "Ok", "", "")
                    '            BubbleEvent = False
                    '            Exit For

                    '        End If
                    '    End If
                    'Next

                    'If Flag1 = True Then
                    '    __oApplication.MessageBox("Inventory Transfer already created , Please reverse the document", 1, "Ok", "", "")
                    '    BubbleEvent = False
                    '    Exit Sub
                    'End If
                    ocombo = oForm.Items.Item("cbIIT").Specific
                    If ocombo.Selected.Value <> "MX" Then




                        For iRow As Integer = 1 To m2.VisualRowCount
                            If (m2.Columns.Item("Col_0").Cells.Item(iRow).Specific.Value) <> "" And (m2.Columns.Item("Col_1").Cells.Item(iRow).Specific.Value) = "Ok" Then

                                qty = qty + CDec(m2.Columns.Item("Col_8").Cells.Item(iRow).Specific.Value)
                                Flag1 = True

                            End If
                        Next
                        If m1.Columns.Item("Col_8").Cells.Item(1).Specific.Value <> qty And Flag1 = True Then
                            If __oApplication.MessageBox("Quantity entered in document And Truck capacity quantity Is different", vbYesNo) = vbYes Then
                                m1.Columns.Item("Col_8").Cells.Item(1).Specific.Value = qty
                            Else
                                BubbleEvent = False
                                Exit Sub

                            End If
                        End If
                        ' End If

                        If Flag = False Then
                            __oApplication.MessageBox("Please Enter ItemCode/Qty", 1, "Ok", "", "")
                            BubbleEvent = False
                            Exit Sub

                        End If



                        If m2.RowCount <= 0 Then
                            __oApplication.MessageBox("Matrix Can Not Blank ", 1, "Ok", "", "")
                            BubbleEvent = False
                            Exit Sub
                        End If

                    Else
                        'For iRow As Integer = 1 To m2.VisualRowCount

                        'Next

                        If (m2.VisualRowCount - 1) > 1 Then

                            __oApplication.MessageBox("Mix can have one TT ", 1, "Ok", "", "")
                            BubbleEvent = False
                        Else
                            If mainqty <> CDec(m2.Columns.Item("Col_8").Cells.Item(1).Specific.Value) Then
                                __oApplication.MessageBox("Truck Capacity and Request Quantity not matched ", 1, "Ok", "", "")
                                BubbleEvent = False

                            End If

                        End If


                    End If




                    End If
            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub


        Private Sub Matrix1_FWhs_OnBeforeChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
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
                oCondition.Alias = "BPLId"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = oForm.Items.Item("Item_22").Specific.Value.ToString
                oCondition.BracketCloseNum = 1




                oCFL.SetConditions(oConditions)



            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Matrix1_TWhs_OnBeforeChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
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
                oCondition.Alias = "BPLId"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = oForm.Items.Item("Item_24").Specific.Value.ToString
                oCondition.BracketCloseNum = 1




                oCFL.SetConditions(oConditions)



            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub


        Private Sub TBranch_OnBeforeChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
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


        Private Sub TruckRegNo_OnBeforeChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
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


                Dim SqlQuery2 As New StringBuilder
                ocombo = oForm.Items.Item("cbIIT").Specific
                SqlQuery2.Append("CALL SAP_GET_TruckNo_InventryRequest('" + oForm.Items.Item("Item_24").Specific.Value.ToString() + "','" + ocombo.Selected.Value.ToString + "')")
                Dim oRset As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRset.DoQuery(SqlQuery2.ToString)

                If oRset.RecordCount > 0 Then
                    For i As Integer = 0 To oRset.RecordCount - 1
                        If i >= 1 And i <= oRset.RecordCount - 1 Then
                            oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR
                        End If
                        oCondition = oConditions.Add
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "U_TRegNo"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = oRset.Fields.Item("U_TRegNo").Value & ""
                        oCondition.BracketCloseNum = 1
                        oRset.MoveNext()

                    Next
                    oCFL.SetConditions(oConditions)

                Else

                    oCondition = oConditions.Add
                    oCondition.BracketOpenNum = 1
                    oCondition.Alias = "U_TRegNo"
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                    oCondition.CondVal = Nothing
                    oCondition.BracketCloseNum = 1
                    oCFL.SetConditions(oConditions)
                End If



            Catch ex As Exception
                __oApplication.MessageBox("Sub-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub TruckRegNo_OnAfterLostFocus(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            BubbleEvent = True
            oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

            Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_ITR2")
            Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m2").Specific
            If m1.Columns.Item("Col_0").Cells.Item(pVal.Row).Specific.Value <> "" Then
                dbsrc.SetValue("U_TDOCNUM", dbsrc.Offset, oForm.Items.Item("Item_14").Specific.Value + "_" + pVal.Row.ToString & "")
                m1.Columns.Item("Col_2").Cells.Item(pVal.Row).Specific.Value = oForm.Items.Item("Item_14").Specific.Value + "_" + pVal.Row.ToString
                '   AddRowInMatrix(oForm, "@SAP_ITR2", "m2")
            End If


        End Sub

        Private Sub TruckRegNo_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                Dim dt As Date = Nothing

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_ITR2")
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m2").Specific

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                m1.GetLineData(pVal.Row)

                                dbsrc.SetValue("U_TRNO", dbsrc.Offset, oDataTable.GetValue("U_TRegNo", 0) & "")
                                dbsrc.SetValue("U_TDOCNUM", dbsrc.Offset, oForm.Items.Item("Item_14").Specific.Value + "_" + pVal.Row.ToString & "")
                                If m1.Columns.Item("Col_1").Cells.Item(pVal.Row).Specific.Value = "" Then
                                    dbsrc.SetValue("U_TRSTATUS", dbsrc.Offset, "Ok" & "")
                                End If

                                Dim qty As Decimal
                                'cnt = GetValue("Select count(*) AS ""Count"" from ""@SAP_OIT"" where ""ITRDocE""=" + oForm.Items.Item("Item_15").Specific.Value.ToString + "", "Count")
                                qty = GetValue("Select sum(U_CAP) AS ""Count"" FROM ""@SAP_OCALP"" A INNER JOIN ""@SAP_CALP1"" B On A.""DocEntry""=B.""DocEntry"" where ""U_VC""='" + oDataTable.GetValue("U_TRegNo", 0) + "'", "Count")
                                dbsrc.SetValue("U_TRCAP", dbsrc.Offset, qty)
                                m1.SetLineData(pVal.Row)
                                    m1.FlushToDataSource()

                                    If oForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE




                                End If
                        Catch ex As Exception

                        End Try
                    End If
                End If
                If m1.Columns.Item("Col_0").Cells.Item(pVal.Row).Specific.Value <> "" Then
                    AddRowInMatrix(oForm, "@SAP_ITR2", "m2")
                End If

                'For ADDRow As Integer = 1 To m1.RowCount
                '    Try
                '        m1.Columns.Item("Col_2").Cells.Item(ADDRow).Specific.Value = oForm.Items.Item("Item_14").Specific.Value + "_" + ADDRow.ToString
                '    Catch ex As Exception

                '    End Try

                'Next

            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub
    End Class
End Namespace

