Imports System.Drawing
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Collections
Imports System.IO
Imports System.Diagnostics.CodeAnalysis
Imports System.Runtime.Inteervices
Imports System.Globalization
Imports System.Data
Imports SAPbobsCOM
Imports SAPbouiCOM
Imports System.Text
Imports System.Configuration
Imports System.Runtime.InteropServices

Namespace SAP_HANA
    Public Class Inv_Transfer : Implements ISAP_HANA

        Private Const V As String = "')"



#Region "Constructors"
        Private __oApplication As SAPbouiCOM.Application
        Public __oCompany As SAPbobsCOM.Company
        Private oForm As SAPbouiCOM.Form
        Private oItem As SAPbouiCOM.Item
        Dim oLabel As SAPbouiCOM.StaticText
        Dim ocombo As SAPbouiCOM.ComboBox
        Dim ocombo1 As SAPbouiCOM.ComboBox
        Dim ocboStType As SAPbouiCOM.ComboBox
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

        Public Sub New(ByRef sApp As SAPbouiCOM.Application, ByRef oCompany As SAPbobsCOM.Company)
            __oApplication = sApp
            __oCompany = oCompany '.Company.GetDICompany()
        End Sub

        Public Sub Form_Creation_MenuEvent(ByRef pVal As MenuEvent, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Creation_MenuEvent
            Try

                Dim sFileName As String = AppDomain.CurrentDomain.BaseDirectory & "SRF\InventoryTransfer.srf"
                Dim sFormName As String = "SAP_UDO_OIT"
                Dim FormUID As String = LoadXMLFiles(__oApplication, sFileName)
                oForm = __oApplication.Forms.GetForm("SAP_UDO_OIT", __oApplication.Forms.ActiveForm.TypeCount)
                If Not String.IsNullOrEmpty(FormUID) Then
                    ActiveForm(oForm, "Item_15", "1")
                    oForm.EnableMenu("1292", True)
                    oForm.EnableMenu("1293", True)
                    oForm.EnableMenu("520", True)
                    oForm.EnableMenu("519", True)
                    oForm.EnableMenu("5890", True)
                    oForm.ReportType = GetValue("Select Code From RTYP Where Name='InvTransfer'", "Code")



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
                    Dim m2 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                    Dim rowcount As Integer = m2.VisualRowCount

                    If rowcount > 0 Then

                        For iRow As Integer = 1 To rowcount

                            If m2.IsRowSelected(iRow) = True Then

                                DelRowFromMatrix(oForm, "@SAP_IT1", "m1", iRow)
                                Exit For
                            End If
                        Next
                        BubbleEvent = False
                    End If

                ElseIf pVal.MenuUID = CType(menuID.Add_Row, String) And pVal.BeforeAction = False Then



                    Dim m2 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                    If m2.RowCount > 0 Then
                        If m2.Columns.Item("Col_0").Cells.Item(m2.VisualRowCount).Specific.String <> "" Then
                            AddRowInMatrix(oForm, "@SAP_IT1", "m1")

                        End If
                    Else
                        AddRowInMatrix(oForm, "@SAP_IT1", "m1")
                        '
                    End If






                End If
            Catch ex As Exception

            End Try
        End Sub

        Public Sub Form_Load_DataEvent(ByRef BusinessObjectInfo As BusinessObjectInfo, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Load_DataEvent
            Try

                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(BusinessObjectInfo.FormTypeEx, __oApplication.Forms.ActiveForm.TypeCount)

                If BusinessObjectInfo.BeforeAction = False Then
                    FormDatLoadEvent(oForm.UniqueID, BubbleEvent)
                End If




            Catch ex As Exception

                __oApplication.MessageBox("[DataEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="FormUID"></param>
        ''' <param name="pVal"></param>
        ''' <param name="BubbleEvent"></param>
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

                    ElseIf pVal.ItemUID = "Item_31" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        TransportarCode_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

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

                        'BASED ON INVENTORY TRANSFER REQUEST
                    ElseIf pVal.ItemUID = "Item_3" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then

                        Dim DocNum As String = ""
                        Dim CheckID As String = ""

                        oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                        oForm = __oApplication.Forms.Item(FormUID)

                        DocNum = oForm.Items.Item("Item_3").Specific.Value.ToString
                        CheckID = oForm.Items.Item("Item_10").Specific.Value.ToString

                        If (String.IsNullOrEmpty(CheckID)) Then
                            If DocNum.Length > 0 Then

                                Try
                                    BubbleEvent = True

                                    Dim SQL As String
                                    Dim DocEntryNo As Integer

                                    Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OIT")
                                    ''krao changes
                                    'SQL = "SELECT  * FROM ""@SAP_OITR""  WHERE ""DocEntry"" =" + CheckID
                                    SQL = "SELECT  * FROM ""@SAP_OITR""  WHERE ""DocNum"" ='" + DocNum + "'"
                                    Dim oDataTable As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                    oDataTable.DoQuery(SQL)
                                    If oDataTable.RecordCount > 0 Then
                                        DocEntryNo = oDataTable.Fields.Item("DocEntry").Value

                                        dbsrc.SetValue("U_ITRDocN", dbsrc.Offset, oDataTable.Fields.Item("DocNum").Value & "")
                                        dbsrc.SetValue("U_ITRDocE", dbsrc.Offset, oDataTable.Fields.Item("DocEntry").Value & "")


                                        Try
                                            Fill_Matrix(FormUID, pVal, BubbleEvent, DocEntryNo.ToString)
                                        Catch ex As Exception

                                        End Try
                                        Try
                                            oForm.Items.Item("Item_76").Enabled = False
                                            oForm.Items.Item("Item_38").Enabled = False
                                        Catch ex As Exception

                                        End Try

                                    End If

                                Catch ex As Exception
                                    __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
                                End Try

                            End If
                        End If


                        'ElseIf pVal.ItemUID = "Item_3" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        'Inv_Trans_Req_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_47" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        Pane_1_OnAfterITEM_PRESSED(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_44" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        Pane_2_OnAfterITEM_PRESSED(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_48" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        Pane_3_OnAfterITEM_PRESSED(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_48" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        Pane_4_OnAfterITEM_PRESSED(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_11" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_OCRC_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_12" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_OCRC2_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_13" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_OCRC3_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_14" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_OCRC4_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_15" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_OCRC5_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_38" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        Inv_Trans_OnAfterLostFocus(FormUID, pVal, BubbleEvent)

                    ElseIf (pVal.ItemUID = "1" Or pVal.ItemUID = "btnLoss") And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        Add_OnAfterItemPressed(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "btnPost" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        PostSAP_OnAfterItemPressed(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_41" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        cfl_INV_Trans_OnAfterItemPressed(FormUID, pVal, BubbleEvent)


                        'removed by mahesh - CFL Removed
                        'ElseIf pVal.ItemUID = "Item_76" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        'GoodReceipt_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                        'BASED ON GOODS RECEIVED
                    ElseIf pVal.ItemUID = "Item_76" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then

                        'added by mahesh



                        Try


                            BubbleEvent = True
                            oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                            oForm = __oApplication.Forms.Item(FormUID)
                            Dim CheckGRN As String = ""


                            CheckGRN = oForm.Items.Item("Item_79").Specific.Value.ToString

                            If (String.IsNullOrEmpty(CheckGRN)) Then
                                Try
                                    Fill_Matrix_GoodReceipt(FormUID, pVal, BubbleEvent, oForm.Items.Item("Item_76").Specific.Value.ToString)
                                    oForm.Items.Item("Item_3").Enabled = False

                                Catch ex As Exception

                                End Try



                            End If

                        Catch ex As Exception
                            __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
                        End Try



                    ElseIf pVal.ItemUID = "Item_45" And pVal.ColUID = "Col_6" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix2_CardCode_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_10" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        QtyCalculation(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_35" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        TruckRegNo_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_103" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        TankTempLostFocus(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_105" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        DensityLostFocus(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_106" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        FBPLostFocus(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_108" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        BatchLostFocus(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_99" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_COMBO_SELECT Then
                        QCComboSelect(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_6" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        TempLostFocus(FormUID, pVal, BubbleEvent)

                    End If

                ElseIf pVal.BeforeAction = True Then

                    If pVal.ItemUID = "m1" And pVal.ColUID = "Col_11" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_OCRC_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_12" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_OCRC2_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_13" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_OCRC3_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_14" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_OCRC4_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_15" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_OCRC5_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_69" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Inv_Trans_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_3" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        InventoryTransferRequest_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "1" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        Add_OnBeforeItemPressed(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_22" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        FBranch_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_24" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        TBranch_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_25" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Twarhouse_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_3" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Twarhouse_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_23" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Fwarhouse_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_2" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Fwarhouse_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_31" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        TransportCode_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_45" And pVal.ColUID = "Col_6" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix2_CardCode_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_0" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_ItemCode_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_76" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        GoodReceipt_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_35" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
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


        Private Sub DefulatSetting(ByVal FormUID As String, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(__oApplication.Forms.ActiveForm.TypeEx, __oApplication.Forms.ActiveForm.TypeCount)

                oForm.Items.Item("Item_47").Click()


                Dim ToDate As Date = Nothing
                Dim sc As String = __oApplication.Company.ServerDate
                ToDate = DateTime.ParseExact(sc, GetDateFormat, DateTimeFormatInfo.InvariantInfo)
                Dim PODate As SAPbouiCOM.EditText = oForm.Items.Item("Item_26").Specific
                PODate.String = ToDate.ToString("yyyyMMdd")

                Dim StartDate As SAPbouiCOM.EditText = oForm.Items.Item("Item_27").Specific
                StartDate.String = ToDate.ToString("yyyyMMdd")
                ' comment krao 18/03/2023
                'oForm.Items.Item("Item_15").Specific.value = GenrateDocEntry("DocEntry", "@SAP_OIT")

                Dim cbSeries As SAPbouiCOM.ComboBox = oForm.Items.Item("Item_21").Specific
                cbSeries.ValidValues.LoadSeries("SAP_UDO_OIT", SAPbouiCOM.BoSeriesMode.sf_Add)
                cbSeries.SelectExclusive(0, SAPbouiCOM.BoSearchKey.psk_Index)

                ' comment krao 18/03/2023
                'oForm.Items.Item("Item_14").Specific.Value = GetValue("Select T0.""NextNumber"" FROM NNM1 T0 WHERE T0.""ObjectCode"" ='SAP_UDO_OIT' And  T0.""Series"" ='" + cbSeries.Selected.Value + "'", "NextNumber")


                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                AddRowInMatrix(oForm, "@SAP_IT1", "m1")


                Landed_Cost_Fill_Matrix(oForm.UniqueID, BubbleEvent)


                AddRowInMatrix(oForm, "@SAP_IT3", "Item_50")

                oForm.Items.Item("Item_3").Enabled = True
                oForm.Items.Item("Item_38").Enabled = True
                oForm.Items.Item("Item_41").Enabled = True
                oForm.Items.Item("Item_76").Enabled = True

                oForm.Items.Item("Item_31").Enabled = True
                oForm.Items.Item("Item_33").Enabled = True
                oForm.Items.Item("Item_35").Enabled = True
                oForm.Items.Item("Item_37").Enabled = True
                m1.Columns.Item("Col_11").Editable = True
                m1.Columns.Item("Col_12").Editable = True
                m1.Columns.Item("Col_13").Editable = True
                m1.Columns.Item("Col_14").Editable = True
                m1.Columns.Item("Col_15").Editable = True

                m1.Columns.Item("Col_0").Editable = True
                m1.Columns.Item("Col_1").Editable = True
                m1.Columns.Item("Col_9").Editable = True
                m1.Columns.Item("Col_3").Editable = True
                m1.Columns.Item("Col_2").Editable = True



                Dim QCApproval As Integer = GetValue("Select Count(""Code"") As ""Count"" From ""@SAP_OURC"" Where ""U_TrxType""='QC' And ""U_User""='" + __bobCompany.UserSignature.ToString + "' ", "Count")
                If QCApproval > 0 Then
                    oForm.Items.Item("Item_99").Enabled = True
                Else
                    oForm.Items.Item("Item_99").Enabled = False

                End If

                ' oForm.Items.Item("Item_99").Enabled = False
                oForm.Items.Item("Item_22").Enabled = True
                'oForm.Items.Item("btnPost").Enabled = True
                oForm.Items.Item("btnPost").Enabled = False


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

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OIT")
                oMatrix = oForm.Items.Item("m1").Specific



                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                dbsrc.SetValue("U_FW", dbsrc.Offset, oDataTable.GetValue("WhsCode", 0) & "")
                                dbsrc.SetValue("U_FWN", dbsrc.Offset, oDataTable.GetValue("WhsName", 0) & "")

                                For ADDRow As Integer = 1 To oMatrix.RowCount

                                    Try
                                        oMatrix.Columns.Item("Col_2").Cells.Item(ADDRow).Specific.Value = oDataTable.GetValue("WhsCode", 0)
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

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OIT")

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                dbsrc.SetValue("U_TW", dbsrc.Offset, oDataTable.GetValue("WhsCode", 0) & "")
                                dbsrc.SetValue("U_TWN", dbsrc.Offset, oDataTable.GetValue("WhsName", 0) & "")

                                oMatrix = oForm.Items.Item("m1").Specific


                                Dim Query As String = ""
                                Query = "Call SAP_GET_TempInfo_Whs_ItemWise('" + oMatrix.Columns.Item("Col_0").Cells.Item(1).Specific.Value + "','" + oDataTable.GetValue("WhsCode", 0).ToString + "')"


                                Dim oRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                oRs.DoQuery(Query)
                                If oRs.RecordCount > 0 Then
                                    For ADDRow As Integer = 1 To oMatrix.RowCount

                                        oMatrix.Columns.Item("Col_6").Cells.Item(ADDRow).Specific.Value = oRs.Fields.Item("U_Out").Value
                                        oMatrix.Columns.Item("Col_7").Cells.Item(ADDRow).Specific.Value = oRs.Fields.Item("U_Density").Value


                                    Next

                                End If
                                For ADDRow As Integer = 1 To oMatrix.RowCount
                                    Try
                                        oMatrix.Columns.Item("Col_3").Cells.Item(ADDRow).Specific.Value = oDataTable.GetValue("WhsCode", 0)
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

        Private Sub TransportarCode_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OIT")
                Try
                    oForm.Items.Item("Item_35").Specific.Value = ""
                    oForm.Items.Item("Item_37").Specific.Value = ""
                Catch ex As Exception

                End Try

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                dbsrc.SetValue("U_TCode", dbsrc.Offset, oDataTable.GetValue("CardCode", 0) & "")
                                dbsrc.SetValue("U_TName", dbsrc.Offset, oDataTable.GetValue("CardName", 0) & "")

                                Dim SQL As String
                                SQL = "Select Top 1 T0.""DocEntry"", T0.""DocNum"", T0.""U_TRegNo"", T0.""U_Trailor"", T0.""U_Owner"", T0.""U_Driver"", T0.""U_CardCode"", T0.""U_CardName"" FROM ""@SAP_OTM""  T0 WHERE T0.""U_CardCode"" ='" + oDataTable.GetValue("CardCode", 0) + "'"

                                Dim oRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                oRs.DoQuery(SQL)
                                If oRs.RecordCount > 0 Then

                                    dbsrc.SetValue("U_TRNO", dbsrc.Offset, oRs.Fields.Item("U_TRegNo").Value & "")
                                    dbsrc.SetValue("U_DRName", dbsrc.Offset, oRs.Fields.Item("U_Driver").Value & "")

                                    If oForm.Items.Item("Item_10").Specific.Value.ToString <> "" Then




                                        Dim Count As Int16

                                        'Dim CountQ As String = ""
                                        'CountQ = "CALL SAP_GET_CalibrationCount_InventryTransfer ('" + oRs.Fields.Item("U_TRegNo").Value + "')"


                                        'Dim oRs1Count As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                        'oRs1Count.DoQuery(CountQ)
                                        'If oRs1Count.RecordCount > 0 Then
                                        '    Count = oRs1Count.Fields.Item("Count").Value
                                        'End If

                                        Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific


                                        If m1.RowCount > 0 Then

                                            If __oApplication.MessageBox("Are you sure you want To Clear Item Matrix.After this data will be Row level data complete loss  ", 1, "Yes", "No", "") = 2 Then
                                                BubbleEvent = False
                                                Exit Sub


                                            End If
                                        End If

                                        Dim dbsrcRow As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_IT1")
                                        Dim Query As String = ""
                                        Query = "CALL SAP_GET_Inventory_Transfer_Request_Data ('" + oForm.Items.Item("Item_10").Specific.Value + "','" + oRs.Fields.Item("U_TRegNo").Value + "')"
                                        m1.Clear()
                                        dbsrcRow.Clear()

                                        Dim oRs1 As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                        oRs1.DoQuery(Query)
                                        If oRs1.RecordCount > 0 Then


                                            For iRow As Integer = 0 To oRs1.RecordCount - 1
                                                Dim CountQ As String = ""
                                                CountQ = "CALL SAP_GET_CalibrationCount_InventryTransfer ('" + oRs.Fields.Item("U_TRegNo").Value + "')"


                                                Dim oRs1Count As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                                oRs1Count.DoQuery(CountQ)
                                                If oRs1Count.RecordCount > 0 Then
                                                    For IC As Integer = 0 To oRs1Count.RecordCount - 1



                                                        dbsrcRow.Offset = dbsrc.Size - 1
                                                        m1.AddRow(1, m1.VisualRowCount)
                                                        m1.GetLineData(m1.VisualRowCount)

                                                        dbsrcRow.SetValue("U_Chamber", dbsrc.Offset, oRs1Count.Fields.Item("U_CHN").Value & "" & "")
                                                        dbsrcRow.SetValue("U_Qty", dbsrc.Offset, oRs1Count.Fields.Item("U_CAP").Value & "" & "")
                                                        dbsrcRow.SetValue("U_Dip", dbsrc.Offset, oRs1Count.Fields.Item("U_OILDIP").Value & "" & "")
                                                        dbsrcRow.SetValue("U_ODIP", dbsrc.Offset, oRs1Count.Fields.Item("U_OILDIP").Value & "" & "")

                                                        dbsrcRow.SetValue("U_ItemCode", dbsrc.Offset, oRs1.Fields.Item("U_ItemCode").Value & "")
                                                        dbsrcRow.SetValue("U_ItemName", dbsrc.Offset, oRs1.Fields.Item("U_ItemName").Value & "")
                                                        dbsrcRow.SetValue("U_FW", dbsrc.Offset, oRs1.Fields.Item("U_FW").Value & "")
                                                        dbsrcRow.SetValue("U_TW", dbsrc.Offset, oRs1.Fields.Item("U_TW").Value & "")
                                                        dbsrcRow.SetValue("U_UOM", dbsrc.Offset, oRs1.Fields.Item("U_UOM").Value & "")
                                                        dbsrcRow.SetValue("U_UOMG", dbsrc.Offset, oRs1.Fields.Item("U_UOMG").Value & "")
                                                        dbsrcRow.SetValue("U_Temp", dbsrc.Offset, oRs1.Fields.Item("U_Temp").Value & "")
                                                        dbsrcRow.SetValue("U_Density", dbsrc.Offset, oRs1.Fields.Item("U_Density").Value & "")

                                                        dbsrcRow.SetValue("U_OCRC", dbsrc.Offset, oRs1.Fields.Item("U_OCRC").Value & "")
                                                        dbsrcRow.SetValue("U_OCRC2", dbsrc.Offset, oRs1.Fields.Item("U_OCRC2").Value & "")
                                                        dbsrcRow.SetValue("U_OCRC3", dbsrc.Offset, oRs1.Fields.Item("U_OCRC3").Value & "")
                                                        dbsrcRow.SetValue("U_OCRC4", dbsrc.Offset, oRs1.Fields.Item("U_OCRC4").Value & "")
                                                        dbsrcRow.SetValue("U_OCRC5", dbsrc.Offset, oRs1.Fields.Item("U_OCRC5").Value & "")


                                                        m1.SetLineData(m1.VisualRowCount)
                                                        m1.FlushToDataSource()


                                                        oRs1Count.MoveNext()
                                                    Next
                                                End If

                                                oRs1.MoveNext()
                                            Next
                                        End If
                                        Marshal.ReleaseComObject(oRs1)

                                    End If

                                End If




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


                Dim SqlQuery2 As New StringBuilder

                SqlQuery2.Append("Select ""U_CardCode"" From ""@SAP_OTM"" Where ""U_TRegNo""='" + oForm.Items.Item("Item_35").Specific.Value + "' ")

                Dim oRset As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRset.DoQuery(SqlQuery2.ToString)

                If oRset.RecordCount > 0 Then
                    For i As Integer = 0 To oRset.RecordCount - 1
                        If i >= 1 And i <= oRset.RecordCount - 1 Then
                            oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND
                        End If
                        oCondition = oConditions.Add
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "U_CardCode"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                        oCondition.CondVal = oRset.Fields.Item("DocEntry").Value & ""
                        oCondition.BracketCloseNum = 1
                        oRset.MoveNext()

                    Next
                    oCFL.SetConditions(oConditions)

                Else
                    oCFL.SetConditions(oConditions)
                End If



            Catch ex As Exception
                __oApplication.MessageBox("Sub-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
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

                'oCondition = oConditions.Add
                'oCondition.BracketOpenNum = 1
                'oCondition.Alias = "U_CardCode"
                'oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                'oCondition.CondVal = oForm.Items.Item("Item_31").Specific.Value
                'oCondition.BracketCloseNum = 1


                Dim SqlQuery2 As New StringBuilder
                Dim pssql = "Select DISTINCT ifnull(""U_TRNO"",'') as ""U_TRegNo"" From ""@SAP_ITR2"" Where ""DocEntry""='" + oForm.Items.Item("Item_10").Specific.Value + "' and U_TRSTATUS='Ok' and ifnull(""U_TRNO"",'') <>''"

                ocombo = oForm.Items.Item("cbIIT").Specific
                ocboStType = oForm.Items.Item("Item_97").Specific

                SqlQuery2.Append("CALL SAP_GET_TruckNo ('" + ocombo.Selected.Value + "','" + oForm.Items.Item("Item_22").Specific.Value + "','" + oForm.Items.Item("Item_23").Specific.Value + "','" + oForm.Items.Item("Item_10").Specific.Value + "')")

                Dim oRset As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                'If ocboStType.Selected.Value = "S" Then
                '    oRset.DoQuery(pssql)
                'Else
                '    oRset.DoQuery(SqlQuery2.ToString)
                'End If
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

        Private Sub TruckRegNo_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OIT")
                Try
                    oForm.Items.Item("Item_35").Specific.Value = ""
                    oForm.Items.Item("Item_37").Specific.Value = ""
                Catch ex As Exception

                End Try

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then



                                Dim SQL As String
                                SQL = "Select  T0.""DocEntry"", T0.""DocNum"", T0.""U_TRegNo"", T0.""U_Trailor"", T0.""U_Owner"", T0.""U_Driver"", T0.""U_CardCode"", T0.""U_CardName"" FROM ""@SAP_OTM""  T0 WHERE T0.""U_CardCode"" ='" + oDataTable.GetValue("U_CardCode", 0) + "'  And T0.""U_TRegNo"" ='" + oDataTable.GetValue("U_TRegNo", 0) & "" + "'"

                                Dim oRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                oRs.DoQuery(SQL)
                                If oRs.RecordCount > 0 Then

                                    dbsrc.SetValue("U_TRNO", dbsrc.Offset, oRs.Fields.Item("U_TRegNo").Value & "")
                                    dbsrc.SetValue("U_DRName", dbsrc.Offset, oRs.Fields.Item("U_Driver").Value & "")
                                    dbsrc.SetValue("U_TCode", dbsrc.Offset, oRs.Fields.Item("U_CardCode").Value & "")
                                    dbsrc.SetValue("U_TName", dbsrc.Offset, oRs.Fields.Item("U_CardName").Value & "")

                                    If oForm.Items.Item("Item_10").Specific.Value.ToString <> "" Then


                                        Dim Count As Int16
                                        Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                                        If m1.RowCount > 0 Then
                                            If __oApplication.MessageBox("Are you sure you want To Clear Item Matrix.After this data will be Row level data complete loss  ", 1, "Yes", "No", "") = 2 Then
                                                BubbleEvent = False
                                                Exit Sub
                                            End If
                                        End If
                                        Dim dbsrcRow As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_IT1")
                                        Dim Query As String = ""
                                        Query = "CALL SAP_GET_Inventory_Transfer_Request_Data ('" + oForm.Items.Item("Item_10").Specific.Value + "','" + oRs.Fields.Item("U_TRegNo").Value + "')"
                                        m1.Clear()
                                        dbsrcRow.Clear()

                                        Dim oRs1 As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                        oRs1.DoQuery(Query)
                                        If oRs1.RecordCount > 0 Then
                                            For iRow As Integer = 0 To oRs1.RecordCount - 1
                                                Dim CountQ As String = ""
                                                CountQ = "CALL SAP_GET_CalibrationCount_InventryTransfer ('" + oRs.Fields.Item("U_TRegNo").Value + "')"
                                                Dim oRs1Count As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                                oRs1Count.DoQuery(CountQ)
                                                If oRs1Count.RecordCount > 0 Then
                                                    For IC As Integer = 0 To oRs1Count.RecordCount - 1
                                                        dbsrcRow.Offset = dbsrc.Size - 1
                                                        m1.AddRow(1, m1.VisualRowCount)
                                                        m1.GetLineData(m1.VisualRowCount)

                                                        dbsrcRow.SetValue("U_Chamber", dbsrc.Offset, oRs1Count.Fields.Item("U_CHN").Value & "" & "")
                                                        dbsrcRow.SetValue("U_Qty", dbsrc.Offset, oRs1Count.Fields.Item("U_CAP").Value & "" & "")
                                                        dbsrcRow.SetValue("U_Dip", dbsrc.Offset, oRs1Count.Fields.Item("U_OILDIP").Value & "" & "")
                                                        dbsrcRow.SetValue("U_ODIP", dbsrc.Offset, oRs1Count.Fields.Item("U_OILDIP").Value & "" & "")

                                                        dbsrcRow.SetValue("U_ItemCode", dbsrc.Offset, oRs1.Fields.Item("U_ItemCode").Value & "")
                                                        dbsrcRow.SetValue("U_ItemName", dbsrc.Offset, oRs1.Fields.Item("U_ItemName").Value & "")
                                                        If oForm.Items.Item("Item_23").Specific.Value = "" Then
                                                            dbsrcRow.SetValue("U_FW", dbsrc.Offset, oRs1.Fields.Item("U_FW").Value & "")
                                                        Else
                                                            dbsrcRow.SetValue("U_FW", dbsrc.Offset, oForm.Items.Item("Item_23").Specific.Value & "")
                                                        End If


                                                        dbsrcRow.SetValue("U_TW", dbsrc.Offset, oRs1.Fields.Item("U_TW").Value & "")
                                                        dbsrcRow.SetValue("U_UOM", dbsrc.Offset, oRs1.Fields.Item("U_UOM").Value & "")
                                                        dbsrcRow.SetValue("U_UOMG", dbsrc.Offset, oRs1.Fields.Item("U_UOMG").Value & "")
                                                        dbsrcRow.SetValue("U_Temp", dbsrc.Offset, oRs1.Fields.Item("U_Temp").Value & "")
                                                        dbsrcRow.SetValue("U_Density", dbsrc.Offset, oRs1.Fields.Item("U_Density").Value & "")

                                                        dbsrcRow.SetValue("U_OCRC", dbsrc.Offset, oRs1.Fields.Item("U_OCRC").Value & "")
                                                        dbsrcRow.SetValue("U_OCRC2", dbsrc.Offset, oRs1.Fields.Item("U_OCRC2").Value & "")
                                                        dbsrcRow.SetValue("U_OCRC3", dbsrc.Offset, oRs1.Fields.Item("U_OCRC3").Value & "")
                                                        dbsrcRow.SetValue("U_OCRC4", dbsrc.Offset, oRs1.Fields.Item("U_OCRC4").Value & "")
                                                        dbsrcRow.SetValue("U_OCRC5", dbsrc.Offset, oRs1.Fields.Item("U_OCRC5").Value & "")


                                                        m1.SetLineData(m1.VisualRowCount)
                                                        m1.FlushToDataSource()


                                                        oRs1Count.MoveNext()
                                                    Next
                                                End If
                                            Next
                                            oRs1.MoveNext()

                                        End If
                                        Marshal.ReleaseComObject(oRs1)

                                    End If

                                    ocombo = oForm.Items.Item("cbIIT").Specific
                                    If ocombo.Selected.Value = "TP" Then
                                        Fill_Matrix_Topping(FormUID, pVal, BubbleEvent, oDataTable.GetValue("U_TRegNo", 0))
                                    End If

                                End If




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

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OIT")

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

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OIT")

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

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OIT")

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

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_IT1")
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
                __oApplication.MessageBox("Sub-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
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

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_IT1")
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
                __oApplication.MessageBox("Sub-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
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

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_IT1")
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
                __oApplication.MessageBox("Sub-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub


        Private Sub Inv_Trans_Req_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OIT")

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                dbsrc.SetValue("U_ITRDocN", dbsrc.Offset, oDataTable.GetValue("DocNum", 0) & "")
                                dbsrc.SetValue("U_ITRDocE", dbsrc.Offset, oDataTable.GetValue("DocEntry", 0) & "")
                                Try
                                    Fill_Matrix(FormUID, pVal, BubbleEvent, oDataTable.GetValue("DocEntry", 0).ToString)
                                Catch ex As Exception

                                End Try
                                Try
                                    oForm.Items.Item("Item_76").Enabled = False
                                    oForm.Items.Item("Item_38").Enabled = False
                                Catch ex As Exception

                                End Try


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


        Private Sub GoodReceipt_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)


            Try

                BubbleEvent = True

                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OIT")


                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                dbsrc.SetValue("U_SGRNDN", dbsrc.Offset, oDataTable.GetValue("DocNum", 0) & "")
                                dbsrc.SetValue("U_SGRNDE", dbsrc.Offset, oDataTable.GetValue("DocEntry", 0) & "")
                                Try
                                    Fill_Matrix_GoodReceipt(FormUID, pVal, BubbleEvent, oDataTable.GetValue("DocEntry", 0).ToString)
                                Catch ex As Exception

                                End Try

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

        Private Sub GoodReceipt_OnBeforeChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
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

                'oCondition = oConditions.Add
                'oCondition.BracketOpenNum = 1
                'oCondition.Alias = "U_TCode"
                'oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_EQUAL
                'oCondition.CondVal = ""
                'oCondition.BracketCloseNum = 1
                'oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

                'oCondition = oConditions.Add
                'oCondition.BracketOpenNum = 1
                'oCondition.Alias = "U_TRNO"
                'oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_EQUAL
                'oCondition.CondVal = ""
                'oCondition.BracketCloseNum = 1
                'oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 1
                oCondition.Alias = "BPLId"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = oForm.Items.Item("Item_22").Specific.Value
                oCondition.BracketCloseNum = 1
                oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND

                oCondition = oConditions.Add
                oCondition.BracketOpenNum = 1
                oCondition.Alias = "U_Flag"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_IS_NULL
                oCondition.BracketCloseNum = 1
                'oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND


                Dim SqlQuery2 As New StringBuilder

                SqlQuery2.Append("CALL SAP_GET_GoodReceiptNot_DocEntry")

                Dim oRset As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRset.DoQuery(SqlQuery2.ToString)

                If oRset.RecordCount > 0 Then

                    oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND
                    For i As Integer = 0 To oRset.RecordCount - 1
                        If i >= 1 And i <= oRset.RecordCount - 1 Then
                            oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND
                        End If
                        oCondition = oConditions.Add
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "DocEntry"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_EQUAL
                        oCondition.CondVal = oRset.Fields.Item("DocEntry").Value & ""
                        oCondition.BracketCloseNum = 1
                        oRset.MoveNext()

                    Next
                    oCFL.SetConditions(oConditions)

                Else
                    oCFL.SetConditions(oConditions)
                End If

                'oCFL.SetConditions(oConditions)

            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub


        Private Sub InventoryTransferRequest_OnBeforeChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
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
                oCondition.Alias = "U_FB"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = oForm.Items.Item("Item_22").Specific.Value
                oCondition.BracketCloseNum = 1


                Dim SqlQuery2 As New StringBuilder

                SqlQuery2.Append("CALL SAP_GET_InventryRequest_DocEntry")

                Dim oRset As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRset.DoQuery(SqlQuery2.ToString)

                If oRset.RecordCount > 0 Then
                    oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND
                    For i As Integer = 0 To oRset.RecordCount - 1
                        If i >= 1 And i <= oRset.RecordCount - 1 Then
                            oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND
                        End If
                        oCondition = oConditions.Add
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "DocEntry"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_EQUAL
                        oCondition.CondVal = oRset.Fields.Item("DocEntry").Value & ""
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

        Private Sub Fill_Matrix(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String)
            Try


                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                If m1.RowCount > 0 Then

                    If __oApplication.MessageBox("Are you sure you want To Clear Item Matrix.After this data will be Row level data complete loss  ", 1, "Yes", "No", "") = 2 Then
                        BubbleEvent = False
                        Exit Sub


                    End If
                End If

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_IT1")
                Dim dbsrcHead As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OIT")
                Dim Query As String = ""
                Query = "CALL SAP_GET_Inventory_Transfer_Request_Data ('" + DocEntry.ToString + "','')"
                m1.Clear()
                dbsrc.Clear()

                Dim oRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRs.DoQuery(Query)
                If oRs.RecordCount > 0 Then



                    dbsrcHead.SetValue("U_FB", dbsrcHead.Offset, oRs.Fields.Item("U_FB").Value & "")
                    dbsrcHead.SetValue("U_FW", dbsrcHead.Offset, oRs.Fields.Item("U_FW").Value & "")
                    dbsrcHead.SetValue("U_TB", dbsrcHead.Offset, oRs.Fields.Item("U_TB").Value & "")
                    dbsrcHead.SetValue("U_TW", dbsrcHead.Offset, oRs.Fields.Item("U_TW").Value & "")
                    ' dbsrcHead.SetValue("U_Route", dbsrcHead.Offset, oRs.Fields.Item("U_Route").Value & "")
                    dbsrcHead.SetValue("U_Remark", dbsrcHead.Offset, oRs.Fields.Item("U_Remark").Value & "")
                    dbsrcHead.SetValue("U_ITT", dbsrcHead.Offset, oRs.Fields.Item("U_ITT").Value & "")


                    For iRow As Integer = 0 To oRs.RecordCount - 1

                        dbsrc.Offset = dbsrc.Size - 1
                        m1.AddRow(1, m1.VisualRowCount)
                        m1.GetLineData(m1.VisualRowCount)



                        dbsrc.SetValue("U_ItemCode", dbsrc.Offset, oRs.Fields.Item("U_ItemCode").Value & "")
                        dbsrc.SetValue("U_ItemName", dbsrc.Offset, oRs.Fields.Item("U_ItemName").Value & "")
                        dbsrc.SetValue("U_FW", dbsrc.Offset, oRs.Fields.Item("U_FW").Value & "")
                        dbsrc.SetValue("U_TW", dbsrc.Offset, oRs.Fields.Item("U_TW").Value & "")
                        dbsrc.SetValue("U_UOM", dbsrc.Offset, oRs.Fields.Item("U_UOM").Value & "")
                        dbsrc.SetValue("U_UOMG", dbsrc.Offset, oRs.Fields.Item("U_UOMG").Value & "")
                        dbsrc.SetValue("U_Temp", dbsrc.Offset, oRs.Fields.Item("U_Temp").Value & "")
                        dbsrc.SetValue("U_Density", dbsrc.Offset, oRs.Fields.Item("U_Density").Value & "")
                        dbsrc.SetValue("U_Qty", dbsrc.Offset, oRs.Fields.Item("U_Qty").Value & "")
                        dbsrc.SetValue("U_OCRC", dbsrc.Offset, oRs.Fields.Item("U_OCRC").Value & "")
                        dbsrc.SetValue("U_OCRC2", dbsrc.Offset, oRs.Fields.Item("U_OCRC2").Value & "")
                        dbsrc.SetValue("U_OCRC3", dbsrc.Offset, oRs.Fields.Item("U_OCRC3").Value & "")
                        dbsrc.SetValue("U_OCRC4", dbsrc.Offset, oRs.Fields.Item("U_OCRC4").Value & "")
                        dbsrc.SetValue("U_OCRC5", dbsrc.Offset, oRs.Fields.Item("U_OCRC5").Value & "")


                        m1.SetLineData(m1.VisualRowCount)
                        m1.FlushToDataSource()
                        oRs.MoveNext()
                    Next
                End If
                Marshal.ReleaseComObject(oRs)
            Catch ex As Exception

            End Try
        End Sub

        Private Sub Fill_Matrix_GoodReceipt(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String)
            Try


                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                Dim QueryLandCost As String = ""
                QueryLandCost = "CALL SAP_GET_LandedCostValiadtion_BasedOn_GoodReceipt ('" + DocEntry.ToString + "')"
                Dim oRsLandCost As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRsLandCost.DoQuery(QueryLandCost)
                If oRsLandCost.RecordCount > 0 Then
                    If CInt(oRsLandCost.Fields.Item("Count").Value) > 0 Then
                        __oApplication.MessageBox("First Add Landed Cost for the GRPO")
                        BubbleEvent = False
                        Exit Sub
                    End If
                End If


                If m1.RowCount > 0 Then

                    If __oApplication.MessageBox("Are you sure you want to Clear Item Matrix.After this data will be Row level data complete loss  ", 1, "Yes", "No", "") = 2 Then
                        BubbleEvent = False
                        Exit Sub


                    End If
                End If

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_IT1")
                Dim dbsrcHead As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OIT")

                Dim SQL As String
                Dim DocEntryNo As Integer

                SQL = "SELECT  Top  1 ""DocEntry"" FROM OPDN  WHERE ""DocNum"" ='" + oForm.Items.Item("Item_76").Specific.Value.ToString + "' order by ""DocDate"" desc"
                Dim oDataTable As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oDataTable.DoQuery(SQL)
                If oDataTable.RecordCount > 0 Then
                    DocEntryNo = oDataTable.Fields.Item("DocEntry").Value
                End If






                Dim Query As String = ""
                Query = "CALL SAP_GET_GoodReceiptNote_Data ('" + DocEntryNo.ToString + "')"
                m1.Clear()
                dbsrc.Clear()

                Dim oRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRs.DoQuery(Query)
                If oRs.RecordCount > 0 Then

                    dbsrcHead.SetValue("U_FB", dbsrcHead.Offset, oRs.Fields.Item("U_FB").Value & "")
                    dbsrcHead.SetValue("U_FW", dbsrcHead.Offset, oRs.Fields.Item("U_FW").Value & "")
                    dbsrcHead.SetValue("U_TB", dbsrcHead.Offset, oRs.Fields.Item("U_FB").Value & "")
                    dbsrcHead.SetValue("U_TCode", dbsrcHead.Offset, oRs.Fields.Item("U_TCode").Value & "")
                    dbsrcHead.SetValue("U_TRNO", dbsrcHead.Offset, oRs.Fields.Item("U_TRNO").Value & "")
                    dbsrcHead.SetValue("U_DRName", dbsrcHead.Offset, oRs.Fields.Item("U_DRName").Value & "")
                    dbsrcHead.SetValue("U_TName", dbsrcHead.Offset, oRs.Fields.Item("U_TName").Value & "")
                    dbsrcHead.SetValue("U_Remark", dbsrcHead.Offset, "Based On GRN")

                    'added by mahesh
                    dbsrcHead.SetValue("U_SGRNDE", dbsrcHead.Offset, DocEntryNo & "")
                    dbsrcHead.SetValue("U_SGRNDN", dbsrcHead.Offset, DocEntry & "")



                    For iRow As Integer = 0 To oRs.RecordCount - 1

                        dbsrc.Offset = dbsrc.Size - 1
                        m1.AddRow(1, m1.VisualRowCount)
                        m1.GetLineData(m1.VisualRowCount)




                        'dbsrc.SetValue("U_UOM", dbsrc.Offset, oRs.Fields.Item("U_UOM").Value & "")
                        'dbsrc.SetValue("U_UOMG", dbsrc.Offset, oRs.Fields.Item("U_UOMG").Value & "")

                        dbsrc.SetValue("U_ItemCode", dbsrc.Offset, oRs.Fields.Item("U_ItemCode").Value & "")
                        dbsrc.SetValue("U_ItemName", dbsrc.Offset, oRs.Fields.Item("U_ItemName").Value & "")
                        dbsrc.SetValue("U_FW", dbsrc.Offset, oRs.Fields.Item("U_FW").Value & "")
                        dbsrc.SetValue("U_Temp", dbsrc.Offset, oRs.Fields.Item("U_Temp").Value & "")
                        dbsrc.SetValue("U_Density", dbsrc.Offset, oRs.Fields.Item("U_Density").Value & "")
                        dbsrc.SetValue("U_Qty", dbsrc.Offset, oRs.Fields.Item("U_Qty").Value & "")
                        dbsrc.SetValue("U_Chamber", dbsrc.Offset, oRs.Fields.Item("U_Chamber").Value & "")
                        dbsrc.SetValue("U_OCRC", dbsrc.Offset, oRs.Fields.Item("U_OCRC").Value & "")
                        dbsrc.SetValue("U_OCRC2", dbsrc.Offset, oRs.Fields.Item("U_OCRC2").Value & "")
                        dbsrc.SetValue("U_OCRC3", dbsrc.Offset, oRs.Fields.Item("U_OCRC3").Value & "")
                        dbsrc.SetValue("U_OCRC4", dbsrc.Offset, oRs.Fields.Item("U_OCRC4").Value & "")
                        dbsrc.SetValue("U_OCRC5", dbsrc.Offset, oRs.Fields.Item("U_OCRC5").Value & "")
                        dbsrc.SetValue("U_UOM", dbsrc.Offset, oRs.Fields.Item("InvntryUom").Value & "")
                        dbsrc.SetValue("U_Dip", dbsrc.Offset, oRs.Fields.Item("U_Dip").Value & "")
                        dbsrc.SetValue("U_ODip", dbsrc.Offset, oRs.Fields.Item("U_Dip").Value & "")
                        oForm.Items.Item("Item_113").Specific.Value = oRs.Fields.Item("U_Temp").Value



                        m1.SetLineData(m1.VisualRowCount)
                        m1.FlushToDataSource()
                        oRs.MoveNext()
                    Next


                End If
                '  oForm.Items.Item("Item_3").Enabled = False
                oForm.Items.Item("Item_38").Enabled = False
                oForm.Items.Item("Item_41").Enabled = False



                oForm.Items.Item("Item_31").Enabled = False
                oForm.Items.Item("Item_33").Enabled = False
                oForm.Items.Item("Item_35").Enabled = False
                oForm.Items.Item("Item_37").Enabled = False
                m1.Columns.Item("Col_11").Editable = False
                m1.Columns.Item("Col_12").Editable = False
                m1.Columns.Item("Col_13").Editable = False
                m1.Columns.Item("Col_14").Editable = False
                m1.Columns.Item("Col_15").Editable = False

                m1.Columns.Item("Col_0").Editable = False
                m1.Columns.Item("Col_1").Editable = False
                m1.Columns.Item("Col_9").Editable = False
                'm1.Columns.Item("Col_3").Editable = False
                m1.Columns.Item("Col_2").Editable = False


                Marshal.ReleaseComObject(oRs)
            Catch ex As Exception

            End Try
        End Sub

        Private Sub Pane_1_OnAfterITEM_PRESSED(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oForm.PaneLevel = "1"
            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub


        Private Sub Pane_2_OnAfterITEM_PRESSED(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oForm.PaneLevel = "2"
                Dim Qty, Dip As Decimal
                Try
                    Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                    For iRow As Integer = 1 To m1.VisualRowCount
                        Qty = Qty + CDec(m1.Columns.Item("Col_8").Cells.Item(iRow).Specific.Value)
                        Dip = Dip + CDec(m1.Columns.Item("Col_25").Cells.Item(iRow).Specific.Value)
                    Next

                    oForm.Items.Item("Item_110").Specific.value = Qty
                    oForm.Items.Item("Item_117").Specific.value = Dip

                Catch ex As Exception

                End Try

            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub
        Private Sub Pane_3_OnAfterITEM_PRESSED(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oForm.PaneLevel = "3"
            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Pane_4_OnAfterITEM_PRESSED(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oForm.PaneLevel = "4"
            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub


        Private Sub Landed_Cost_Fill_Matrix(ByVal FormUID As String, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(__oApplication.Forms.ActiveForm.TypeEx, __oApplication.Forms.ActiveForm.TypeCount)

                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("Item_45").Specific



                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_IT2")

                Dim Query As String = ""
                Query = "SELECT T0.""Code"", T0.""Name"", T0.""U_LCN"", T0.""U_CAC"", T0.""U_PO"", T0.""U_ItemCode"", T0.""U_ItemName"" FROM ""@SAP_OAC""  T0"
                m1.Clear()
                dbsrc.Clear()

                Dim oRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRs.DoQuery(Query)
                If oRs.RecordCount > 0 Then


                    For iRow As Integer = 0 To oRs.RecordCount - 1

                        dbsrc.Offset = dbsrc.Size - 1
                        m1.AddRow(1, m1.VisualRowCount)
                        m1.GetLineData(m1.VisualRowCount)



                        dbsrc.SetValue("U_LCC", dbsrc.Offset, oRs.Fields.Item("Code").Value & "")
                        dbsrc.SetValue("U_LCN", dbsrc.Offset, oRs.Fields.Item("U_LCN").Value & "")
                        dbsrc.SetValue("U_CAC", dbsrc.Offset, oRs.Fields.Item("U_CAC").Value & "")
                        dbsrc.SetValue("U_ItemCode", dbsrc.Offset, oRs.Fields.Item("U_ItemCode").Value & "")
                        dbsrc.SetValue("U_ItemName", dbsrc.Offset, oRs.Fields.Item("U_ItemName").Value & "")
                        dbsrc.SetValue("U_PO", dbsrc.Offset, oRs.Fields.Item("U_PO").Value & "")



                        m1.SetLineData(m1.VisualRowCount)
                        m1.FlushToDataSource()
                        oRs.MoveNext()
                    Next
                End If
                Marshal.ReleaseComObject(oRs)


            Catch ex As Exception

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

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_IT1")
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

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_IT1")
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

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_IT1")
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

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_IT1")
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

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_IT1")
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

        Private Sub Inv_Trans_OnBeforeChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
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
                oCondition.Alias = "U_PODE"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_NULL
                ' oCondition.CondVal = oForm.Items.Item("Item_33").Specific.Value
                oCondition.BracketCloseNum = 1




                Dim SqlQuery2 As New StringBuilder

                SqlQuery2.Append("SELECT T0.""DocEntry"" FROM ""@SAP_OIT""  T0 WHERE T0.""U_ITDocE"" <>''")


                Dim oRset As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRset.DoQuery(SqlQuery2.ToString)

                If oRset.RecordCount > 0 Then
                    For i As Integer = 0 To oRset.RecordCount - 1
                        If i >= 1 And i <= oRset.RecordCount - 1 Then
                            oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR
                        End If
                        oCondition = oConditions.Add
                        oCondition.BracketOpenNum = 1
                        oCondition.Alias = "DocEntry"
                        oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_EQUAL
                        oCondition.CondVal = oRset.Fields.Item("DocEntry").Value & ""
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

        Private Sub Inv_Trans_OnAfterLostFocus(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Fill_Matrix_Inventry_Trnasfer(FormUID, pVal, BubbleEvent, oForm.Items.Item("Item_40").Specific.Value)
                oForm.Items.Item("Item_22").Enabled = False
            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Fill_Matrix_Inventry_Trnasfer(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String)
            Try


                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                If m1.RowCount > 0 Then

                    If __oApplication.MessageBox("Are you sure you want to Clear Item Matrix.After this data will be Row level data complete loss  ", 1, "Yes", "No", "") = 2 Then
                        BubbleEvent = False
                        Exit Sub


                    End If
                End If

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_IT1")
                Dim dbsrcHead As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OIT")
                Dim Query As String = ""
                Query = "CALL SAP_GET_Inventry_Trnasfer_Data ('" + DocEntry.ToString + "')"
                m1.Clear()
                dbsrc.Clear()


                Dim oRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRs.DoQuery(Query)
                If oRs.RecordCount > 0 Then



                    dbsrcHead.SetValue("U_FB", dbsrcHead.Offset, oRs.Fields.Item("FromBranch").Value & "")
                    dbsrcHead.SetValue("U_FW", dbsrcHead.Offset, oRs.Fields.Item("FromWhs").Value & "")
                    dbsrcHead.SetValue("U_TB", dbsrcHead.Offset, oRs.Fields.Item("ToBranch").Value & "")
                    dbsrcHead.SetValue("U_TW", dbsrcHead.Offset, oRs.Fields.Item("ToWhs").Value & "")

                    dbsrcHead.SetValue("U_Remark", dbsrcHead.Offset, oRs.Fields.Item("U_Remark").Value & "")
                    dbsrcHead.SetValue("U_ITRDocE", dbsrcHead.Offset, oRs.Fields.Item("U_ITRDocE").Value & "")
                    dbsrcHead.SetValue("U_ITRDocN", dbsrcHead.Offset, oRs.Fields.Item("U_ITRDocN").Value & "")

                    dbsrcHead.SetValue("U_TCode", dbsrcHead.Offset, oRs.Fields.Item("U_TCode").Value & "")
                    dbsrcHead.SetValue("U_TName", dbsrcHead.Offset, oRs.Fields.Item("U_TName").Value & "")
                    dbsrcHead.SetValue("U_TRNO", dbsrcHead.Offset, oRs.Fields.Item("U_TRNO").Value & "")
                    dbsrcHead.SetValue("U_DRName", dbsrcHead.Offset, oRs.Fields.Item("U_DRName").Value & "")
                    dbsrcHead.SetValue("U_ITT", dbsrcHead.Offset, oRs.Fields.Item("U_ITT").Value & "")
                    dbsrcHead.SetValue("U_PODE", dbsrcHead.Offset, oRs.Fields.Item("U_PODE").Value & "")
                    dbsrcHead.SetValue("U_PODN", dbsrcHead.Offset, oRs.Fields.Item("U_PODN").Value & "")
                    dbsrcHead.SetValue("U_ITDN", dbsrcHead.Offset, oRs.Fields.Item("U_ITDN").Value & "")
                    dbsrcHead.SetValue("U_ITDE", dbsrcHead.Offset, oRs.Fields.Item("U_ITDE").Value & "")




                    For iRow As Integer = 0 To oRs.RecordCount - 1

                        dbsrc.Offset = dbsrc.Size - 1
                        m1.AddRow(1, m1.VisualRowCount)
                        m1.GetLineData(m1.VisualRowCount)



                        dbsrc.SetValue("U_ItemCode", dbsrc.Offset, oRs.Fields.Item("U_ItemCode").Value & "")
                        dbsrc.SetValue("U_ItemName", dbsrc.Offset, oRs.Fields.Item("U_ItemName").Value & "")
                        dbsrc.SetValue("U_FW", dbsrcHead.Offset, oRs.Fields.Item("FromWhs").Value & "")
                        dbsrc.SetValue("U_TW", dbsrcHead.Offset, oRs.Fields.Item("ToWhs").Value & "")
                        dbsrc.SetValue("U_UOM", dbsrc.Offset, oRs.Fields.Item("U_UOM").Value & "")
                        dbsrc.SetValue("U_UOMG", dbsrc.Offset, oRs.Fields.Item("U_UOMG").Value & "")
                        dbsrc.SetValue("U_OCRC", dbsrc.Offset, oRs.Fields.Item("U_OCRC").Value & "")
                        dbsrc.SetValue("U_OCRC", dbsrc.Offset, oRs.Fields.Item("U_OCRC").Value & "")
                        dbsrc.SetValue("U_OCRC2", dbsrc.Offset, oRs.Fields.Item("U_OCRC2").Value & "")
                        dbsrc.SetValue("U_OCRC3", dbsrc.Offset, oRs.Fields.Item("U_OCRC3").Value & "")
                        dbsrc.SetValue("U_OCRC4", dbsrc.Offset, oRs.Fields.Item("U_OCRC4").Value & "")
                        dbsrc.SetValue("U_Chamber", dbsrc.Offset, oRs.Fields.Item("U_Chamber").Value & "")
                        dbsrc.SetValue("U_Temp", dbsrc.Offset, oRs.Fields.Item("U_Temp").Value & "")
                        dbsrc.SetValue("U_Density", dbsrc.Offset, oRs.Fields.Item("U_Density").Value & "")
                        dbsrc.SetValue("U_Qty", dbsrc.Offset, oRs.Fields.Item("U_CAP").Value & "")
                        dbsrc.SetValue("U_Dip", dbsrc.Offset, oRs.Fields.Item("U_OILDIP").Value & "")
                        dbsrc.SetValue("U_ODip", dbsrc.Offset, oRs.Fields.Item("U_OILDIP").Value & "")

                        oForm.Items.Item("Item_113").Specific.Value = oRs.Fields.Item("U_Temp").Value

                        m1.SetLineData(m1.VisualRowCount)
                        m1.FlushToDataSource()
                        oRs.MoveNext()
                    Next
                End If


                Marshal.ReleaseComObject(oRs)
                oForm.Items.Item("Item_16").Click()
            Catch ex As Exception

            End Try
        End Sub


        Private Sub QtyCalculation(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                ocombo = oForm.Items.Item("cbIIT").Specific
                If (ocombo.Selected.Value <> "TP") And (ocombo.Selected.Value <> "S") Then
                    Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                    If "TR100005" <> m1.Columns.Item("Col_0").Cells.Item(pVal.Row).Specific.Value Then

                        Dim Query As String = ""
                        Query = "Select (IFNULL(T1.""U_CAP"",0)/IFNULL(T1.""U_OILDIP"",0)) As ""QtyValu"" ,""U_OILDIP"",IFNULL(T1.""U_CAP"",0)  AS ""Qty"" From ""@SAP_OCALP""  T0 Inner Join ""@SAP_CALP1""  T1 On T0.""DocEntry""=T1.""DocEntry"" Where ((T0.""U_VC"" ='" + oForm.Items.Item("Item_35").Specific.Value + "'  Or ""U_FAC""='" + oForm.Items.Item("Item_25").Specific.Value + "' )And  T1.""U_CHN"" ='" + m1.Columns.Item("Col_9").Cells.Item(pVal.Row).Specific.Value + "' and IFNULL(T1.""U_CAP"",0)>0)"


                        Dim oRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRs.DoQuery(Query)
                        If oRs.RecordCount > 0 Then
                            'krao comment 05/09/2023 
                            '     Dim OilDip As Decimal = oRs.Fields.Item("U_OILDIP").Value
                            Dim OilDip As Decimal = CDec(m1.Columns.Item("Col_24").Cells.Item(pVal.Row).Specific.Value) 'oRs.Fields.Item("U_OILDIP").Value

                            oComboBox = oForm.Items.Item("Item_97").Specific
                            If oComboBox.Value = "S" Or oComboBox.Value = "O" Then
                                If CDec(m1.Columns.Item("Col_10").Cells.Item(pVal.Row).Specific.Value) > OilDip Then
                                    __oApplication.MessageBox("Can Not Enter Dip More Than Oil Dip (" + OilDip.ToString + ")")
                                    m1.Columns.Item("Col_10").Cells.Item(pVal.Row).Specific.Value = 0
                                    BubbleEvent = False
                                    Exit Sub
                                End If
                            End If

                            Dim Lineqty As Decimal = oRs.Fields.Item("Qty").Value
                            'krao commnet 05/08/23
                            ' Dim Value As Decimal = oRs.Fields.Item("QtyValu").Value
                            'Dim Value As Decimal = CDec(CDec(m1.Columns.Item("Col_8").Cells.Item(pVal.Row).Specific.Value) / OilDip) 'oRs.Fields.Item("QtyValu").Value
                            Dim Value As Decimal = CDec(CDec(Lineqty / OilDip)) 'oRs.Fields.Item("QtyValu").Value

                            m1.Columns.Item("Col_8").Cells.Item(pVal.Row).Specific.Value = Math.Round(Value * CDec(m1.Columns.Item("Col_10").Cells.Item(pVal.Row).Specific.Value))

                            'Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_IT1")
                            'dbsrc.Offset = pVal.Row
                            'dbsrc.SetValue("U_DipDiff", dbsrc.Offset, CDec(m1.Columns.Item("Col_24").Cells.Item(pVal.Row).Specific.Value) - CDec(m1.Columns.Item("Col_10").Cells.Item(pVal.Row).Specific.Value) & "")
                            If CDec(m1.Columns.Item("Col_24").Cells.Item(pVal.Row).Specific.Value) > 0 Then


                                Dim oRowCtrl As SAPbouiCOM.CommonSetting
                                oRowCtrl = m1.CommonSetting()
                                oRowCtrl.SetCellEditable(pVal.Row, 11, True)
                                m1.Columns.Item("Col_25").Cells.Item(pVal.Row).Specific.Value = CDec(m1.Columns.Item("Col_24").Cells.Item(pVal.Row).Specific.Value) - CDec(m1.Columns.Item("Col_10").Cells.Item(pVal.Row).Specific.Value)
                                oRowCtrl.SetCellEditable(pVal.Row, 11, False)

                            End If

                        End If
                        Marshal.ReleaseComObject(oRs)
                    End If
                End If

                Dim Qty, Dip As Decimal
                Try
                    Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                    For iRow As Integer = 1 To m1.VisualRowCount
                        Qty = Qty + CDec(m1.Columns.Item("Col_8").Cells.Item(iRow).Specific.Value)
                        Dip = Dip + CDec(m1.Columns.Item("Col_25").Cells.Item(iRow).Specific.Value)
                    Next

                    oForm.Items.Item("Item_110").Specific.value = Qty
                    oForm.Items.Item("Item_117").Specific.value = Dip

                Catch ex As Exception
                End Try




            Catch ex As Exception

            End Try
        End Sub


        Private Sub cfl_INV_Trans_OnAfterItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                IsBaseForm = oForm
                IsBaseItemID = "Item_38"
                IsBaseUDF = "U_ITDocE"
                IsBase_DN_UDF = "U_ITDocN"
                Dim FB As String = oForm.Items.Item("Item_22").Specific.Value
                Dim TB As String = oForm.Items.Item("Item_24").Specific.Value
                Dim FW As String = oForm.Items.Item("Item_23").Specific.Value
                Dim TW As String = oForm.Items.Item("Item_25").Specific.Value
                Dim c_G_ChooseFromList As c_G_ChooseFromList = New c_G_ChooseFromList(__Application, __bobCompany)
                c_G_ChooseFromList.Form_Creation(IsBaseForm, "CALL SAP_CFL_ListOfInventoryTransfer ", "List of Inventory Transfer", FB, TB, FW, TW)


            Catch ex As Exception
                __oApplication.MessageBox("SOR-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Public Sub ReceivedData(ByVal hTable As Hashtable, ByRef IsBaseForm As SAPbouiCOM.Form)
            Try
                oForm = IsBaseForm ' __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim refHashTable As Hashtable = New Hashtable
                refHashTable = gHashTable
                Dim enumerLookup As IDictionaryEnumerator = refHashTable.GetEnumerator
                oForm = IsBaseForm
                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OIT")

                Dim Code As String = refHashTable("Code").ToString()
                Dim Name As String = refHashTable("Name").ToString()



                If IsBaseItemID = "Item_38" Then
                    If CFL_DocNum <> "All" And CFL_DocNum <> "" Then
                        Code = CFL_DocEntry
                        Name = CFL_DocNum
                    End If
                    If oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE Then
                        oForm.Items.Item("Item_38").Specific.String = Code
                    Else
                        dbsrc.SetValue(IsBaseUDF, dbsrc.Offset, Code)
                        dbsrc.SetValue(IsBase_DN_UDF, dbsrc.Offset, Name)
                        oForm.Items.Item("Item_38").Specific.String = Code
                        oForm.Items.Item("Item_16").Click()
                    End If

                End If

            Catch ex As Exception
                __oApplication.MessageBox("SOR-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
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


        Private Sub Matrix2_CardCode_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                Dim dt As Date = Nothing

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_IT2")
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("Item_45").Specific

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                m1.GetLineData(pVal.Row)

                                dbsrc.SetValue("U_CardCode", dbsrc.Offset, oDataTable.GetValue("CardCode", 0) & "")
                                dbsrc.SetValue("U_CardName", dbsrc.Offset, oDataTable.GetValue("CardName", 0) & "")

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

        Private Sub Matrix2_CardCode_OnBeforeChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)



                Dim m2 As SAPbouiCOM.Matrix = oForm.Items.Item("Item_45").Specific

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

        Private Sub TruckReg_OnBeforeChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
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

                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                Dim SqlQuery As String = "SELECT T0.""ItemCode"" FROM OITW T0 WHERE T0.""WhsCode"" ='" + m1.Columns.Item("Col_3").Cells.Item(pVal.Row).Specific.Value + "'"

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
                    oCFL.SetConditions(oConditions)
                Else
                    oCondition = oConditions.Add
                    oCondition.BracketOpenNum = 1
                    oCondition.Alias = "ItemCode"
                    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                    oCondition.CondVal = Nothing
                    oCondition.BracketCloseNum = 1
                    oCFL.SetConditions(oConditions)
                End If




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

        Private Sub TankTempLostFocus(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific


                For ADDRow As Integer = 1 To m1.RowCount
                    Try
                        m1.Columns.Item("Col_6").Cells.Item(ADDRow).Specific.Value = oForm.Items.Item("Item_103").Specific.Value
                    Catch ex As Exception

                    End Try

                Next
                TempLostFocus(FormUID, pVal, BubbleEvent)

            Catch ex As Exception

            End Try
        End Sub

        Private Sub TempLostFocus(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                If CDec(oForm.Items.Item("Item_113").Specific.Value) > 0 Then
                    oForm.Items.Item("Item_115").Specific.Value = CDec(oForm.Items.Item("Item_113").Specific.Value) - CDec(m1.Columns.Item("Col_6").Cells.Item(1).Specific.Value)
                End If



            Catch ex As Exception

            End Try
        End Sub

        Private Sub DensityLostFocus(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific


                For ADDRow As Integer = 1 To m1.RowCount
                    Try
                        m1.Columns.Item("Col_7").Cells.Item(ADDRow).Specific.Value = oForm.Items.Item("Item_105").Specific.Value
                    Catch ex As Exception

                    End Try

                Next


            Catch ex As Exception

            End Try
        End Sub

        Private Sub BatchLostFocus(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific


                For ADDRow As Integer = 1 To m1.RowCount
                    Try
                        m1.Columns.Item("Col_5").Cells.Item(ADDRow).Specific.Value = oForm.Items.Item("Item_108").Specific.Value
                    Catch ex As Exception

                    End Try

                Next


            Catch ex As Exception

            End Try
        End Sub

        Private Sub FBPLostFocus(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific


                For ADDRow As Integer = 1 To m1.RowCount
                    Try
                        m1.Columns.Item("Col_23").Cells.Item(ADDRow).Specific.Value = oForm.Items.Item("Item_106").Specific.Value
                    Catch ex As Exception

                    End Try

                Next


            Catch ex As Exception

            End Try
        End Sub

        Private Sub Fill_Matrix_Topping(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String)
            Try

                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                If m1.RowCount > 0 Then

                    If __oApplication.MessageBox("Are you sure you want to Clear Item Matrix.After this data will be Row level data complete loss  ", 1, "Yes", "No", "") = 2 Then
                        BubbleEvent = False
                        Exit Sub


                    End If
                End If

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_IT1")
                Dim dbsrcHead As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OIT")
                Dim Query As String = ""
                Query = "CALL SAP_GET_Topping_Data ('" + DocEntry.ToString + "')"
                m1.Clear()
                dbsrc.Clear()


                Dim oRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRs.DoQuery(Query)
                If oRs.RecordCount > 0 Then



                    'dbsrcHead.SetValue("U_FB", dbsrcHead.Offset, oRs.Fields.Item("U_FB").Value & "")
                    'dbsrcHead.SetValue("U_FW", dbsrcHead.Offset, oRs.Fields.Item("U_FW").Value & "")
                    dbsrcHead.SetValue("U_TB", dbsrcHead.Offset, oRs.Fields.Item("U_TB").Value & "")
                    dbsrcHead.SetValue("U_TW", dbsrcHead.Offset, oRs.Fields.Item("U_TW").Value & "")

                    'dbsrcHead.SetValue("U_Remark", dbsrcHead.Offset, oRs.Fields.Item("U_Remark").Value & "")
                    'dbsrcHead.SetValue("U_ITRDocE", dbsrcHead.Offset, oRs.Fields.Item("U_ITRDocE").Value & "")
                    'dbsrcHead.SetValue("U_ITRDocN", dbsrcHead.Offset, oRs.Fields.Item("U_ITRDocN").Value & "")

                    dbsrcHead.SetValue("U_TCode", dbsrcHead.Offset, oRs.Fields.Item("U_TCode").Value & "")
                    dbsrcHead.SetValue("U_TName", dbsrcHead.Offset, oRs.Fields.Item("U_TName").Value & "")
                    dbsrcHead.SetValue("U_TRNO", dbsrcHead.Offset, oRs.Fields.Item("U_TRNO").Value & "")
                    dbsrcHead.SetValue("U_DRName", dbsrcHead.Offset, oRs.Fields.Item("U_DRName").Value & "")

                    For iRow As Integer = 0 To oRs.RecordCount - 1

                        dbsrc.Offset = dbsrc.Size - 1
                        m1.AddRow(1, m1.VisualRowCount)
                        m1.GetLineData(m1.VisualRowCount)



                        dbsrc.SetValue("U_ItemCode", dbsrc.Offset, oRs.Fields.Item("U_ItemCode").Value & "")
                        dbsrc.SetValue("U_ItemName", dbsrc.Offset, oRs.Fields.Item("U_ItemName").Value & "")
                        dbsrc.SetValue("U_FW", dbsrcHead.Offset, oRs.Fields.Item("U_FW").Value & "")
                        dbsrc.SetValue("U_TW", dbsrcHead.Offset, oRs.Fields.Item("U_TW").Value & "")
                        dbsrc.SetValue("U_UOM", dbsrc.Offset, oRs.Fields.Item("U_UOM").Value & "")
                        dbsrc.SetValue("U_UOMG", dbsrc.Offset, oRs.Fields.Item("U_UOMG").Value & "")
                        dbsrc.SetValue("U_OCRC", dbsrc.Offset, oRs.Fields.Item("U_OCRC").Value & "")
                        dbsrc.SetValue("U_OCRC", dbsrc.Offset, oRs.Fields.Item("U_OCRC").Value & "")
                        dbsrc.SetValue("U_OCRC2", dbsrc.Offset, oRs.Fields.Item("U_OCRC2").Value & "")
                        dbsrc.SetValue("U_OCRC3", dbsrc.Offset, oRs.Fields.Item("U_OCRC3").Value & "")
                        dbsrc.SetValue("U_OCRC4", dbsrc.Offset, oRs.Fields.Item("U_OCRC4").Value & "")
                        dbsrc.SetValue("U_Chamber", dbsrc.Offset, oRs.Fields.Item("U_Chamber").Value & "")
                        dbsrc.SetValue("U_Temp", dbsrc.Offset, oRs.Fields.Item("U_Temp").Value & "")
                        dbsrc.SetValue("U_Density", dbsrc.Offset, oRs.Fields.Item("U_Density").Value & "")
                        dbsrc.SetValue("U_Qty", dbsrc.Offset, oRs.Fields.Item("QTy").Value & "")
                        dbsrc.SetValue("U_DIP", dbsrc.Offset, oRs.Fields.Item("U_DIP").Value & "")
                        dbsrc.SetValue("U_ODIP", dbsrc.Offset, oRs.Fields.Item("U_ODIP").Value & "")


                        m1.SetLineData(m1.VisualRowCount)
                        m1.FlushToDataSource()
                        oRs.MoveNext()
                    Next


                End If
                Marshal.ReleaseComObject(oRs)

            Catch ex As Exception

            End Try
        End Sub



        Private Sub QCComboSelect(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                ocombo = oForm.Items.Item("Item_99").Specific
                If ocombo.Selected.Value = "H" Then
                    oForm.Items.Item("btnPost").Enabled = False
                Else
                    oForm.Items.Item("btnPost").Enabled = True
                End If


            Catch ex As Exception

            End Try
        End Sub


        ' ------------------------------------SAP TRANC------------------

        Private Sub Add_OnBeforeItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Then
                    ' comment krao 18/03/2023
                    oForm.Items.Item("Item_15").Specific.value = GenrateDocEntry("DocEntry", "@SAP_OIT")

                    Dim cbSeries As SAPbouiCOM.ComboBox = oForm.Items.Item("Item_21").Specific
                    cbSeries.ValidValues.LoadSeries("SAP_UDO_OIT", SAPbouiCOM.BoSeriesMode.sf_Add)
                    cbSeries.SelectExclusive(0, SAPbouiCOM.BoSearchKey.psk_Index)

                    ' comment krao 18/03/2023
                    oForm.Items.Item("Item_14").Specific.Value = GetValue("Select T0.""NextNumber"" FROM NNM1 T0 WHERE T0.""ObjectCode"" ='SAP_UDO_OIT' And  T0.""Series"" ='" + cbSeries.Selected.Value + "'", "NextNumber")

                End If

                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                    Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                    Dim sql As String
                    ocombo1 = oForm.Items.Item("cbIIT").Specific
                    If (ocombo1.Selected.Value = "MX") Then
                        sql = "Select * from ""@SAP_ITR1"" where ""DocEntry""='" & oForm.Items.Item("Item_10").Specific.Value & "'"
                        Dim reqQty As Double = 0.0
                        Dim ReqItemCode As String
                        Dim trfitemcode As String
                        Dim trfQty As Double = 0.0
                        Dim oRs2 As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRs2.DoQuery(sql)
                        If oRs2.RecordCount > 1 Then


                            For iRow1 As Integer = 0 To oRs2.RecordCount - 1
                                reqQty = CDbl(oRs2.Fields.Item("U_Qty").Value)
                                ReqItemCode = oRs2.Fields.Item("U_ItemCode").Value
                                For iRow As Integer = 1 To m1.VisualRowCount
                                    If ReqItemCode = m1.Columns.Item("Col_0").Cells.Item(iRow).Specific.Value Then
                                        trfQty = trfQty + CDbl(m1.Columns.Item("Col_8").Cells.Item(iRow).Specific.Value)
                                    End If

                                Next

                                If reqQty <> trfQty Then
                                    __oApplication.MessageBox("Request Quantity and Transfered Quantity does not match", 1, "Ok", "", "")
                                    BubbleEvent = False
                                    Exit Sub
                                End If
                                reqQty = 0
                                trfQty = 0
                                oRs2.MoveNext()
                            Next

                        End If

                    End If

                    ''Qty Verification 
                    For iRow As Integer = 1 To m1.VisualRowCount
                        ocombo = oForm.Items.Item("cbIIT").Specific






                        If (ocombo.Selected.Value <> "TP") And (ocombo.Selected.Value <> "S") Then

                            If "TR100005" <> m1.Columns.Item("Col_0").Cells.Item(iRow).Specific.Value Then




                                Dim Query As String = ""
                                Query = "Select (IFNULL(T1.""U_CAP"",0)/IFNULL(T1.""U_OILDIP"",0)) As ""QtyValu"" ,""U_OILDIP"",IFNULL(T1.""U_CAP"",0) as ""Qty""  From ""@SAP_OCALP""  T0 Inner Join ""@SAP_CALP1""  T1 On T0.""DocEntry""=T1.""DocEntry"" Where ((T0.""U_VC"" ='" + oForm.Items.Item("Item_35").Specific.Value + "'  Or ""U_FAC""='" + oForm.Items.Item("Item_25").Specific.Value + "' )And  T1.""U_CHN"" ='" + m1.Columns.Item("Col_9").Cells.Item(iRow).Specific.Value + "' and IFNULL(T1.""U_CAP"",0)>0)"


                                Dim oRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                oRs.DoQuery(Query)
                                If oRs.RecordCount > 0 Then

                                    'Dim OilDip As Decimal = oRs.Fields.Item("U_OILDIP").Value
                                    '''krao
                                   ' Dim OilDip As Decimal = CDec(m1.Columns.Item("Col_24").Cells.Item(pVal.Row).Specific.Value) 'oRs.Fields.Item("U_OILDIP").Value


                                    'Dim Lineqty As Decimal = oRs.Fields.Item("Qty").Value
                                    ''krao commnet 05/08/23
                                    '' Dim Value As Decimal = oRs.Fields.Item("QtyValu").Value
                                    ''Dim Value As Decimal = CDec(CDec(m1.Columns.Item("Col_8").Cells.Item(pVal.Row).Specific.Value) / OilDip) 'oRs.Fields.Item("QtyValu").Value
                                    'Dim Value As Decimal = CDec(CDec(Lineqty / OilDip) 'oRs.Fields.Item("QtyValu").Value

                                    'm1.Columns.Item("Col_8").Cells.Item(pVal.Row).Specific.Value = Math.Round(Value * CDec(m1.Columns.Item("Col_10").Cells.Item(pVal.Row).Specific.Value))


                                    '''
                                    '''


                                    'If CDec(m1.Columns.Item("Col_10").Cells.Item(iRow).Specific.Value) > OilDip Then
                                    '    __oApplication.MessageBox("Can Not Enter Dip More Than Oil Dip (" + OilDip.ToString + ")")
                                    '    m1.Columns.Item("Col_10").Cells.Item(iRow).Specific.Value = 0
                                    '    BubbleEvent = False
                                    '    Exit Sub
                                    'End If

                                    'Dim Lineqty As Decimal = oRs.Fields.Item("Qty").Value
                                    'krao commnet 05/08/23
                                    ' Dim Value As Decimal = oRs.Fields.Item("QtyValu").Value
                                    'Dim Value As Decimal = CDec(CDec(m1.Columns.Item("Col_8").Cells.Item(pVal.Row).Specific.Value) / OilDip) 'oRs.Fields.Item("QtyValu").Value
                                    'Dim Value As Decimal = CDec(CDec(Lineqty / OilDip)) 'oRs.Fields.Item("QtyValu").Value

                                    'm1.Columns.Item("Col_8").Cells.Item(pVal.Row).Specific.Value = Math.Round(Value * CDec(m1.Columns.Item("Col_10").Cells.Item(pVal.Row).Specific.Value))

                                    m1.Columns.Item("Col_25").Cells.Item(iRow).Specific.Value = CDec(m1.Columns.Item("Col_24").Cells.Item(iRow).Specific.Value) - CDec(m1.Columns.Item("Col_10").Cells.Item(iRow).Specific.Value)
                                End If
                                Marshal.ReleaseComObject(oRs)

                            End If
                        End If
                    Next

                    ''Qty Verification end 





                    Dim FL, TL, WTM As String
                    If oForm.Items.Item("Item_23").Specific.Value = "" Then
                        __oApplication.MessageBox("From Warehouse Can Not Blank", 1, "Ok", "", "")
                        BubbleEvent = False
                        Exit Sub

                    ElseIf oForm.Items.Item("Item_25").Specific.Value = "" Then
                        '__oApplication.MessageBox("TO Warehouse Can Not Blank", 1, "Ok", "", "")
                        'BubbleEvent = False
                        'Exit Sub
                    ElseIf oForm.Items.Item("Item_31").Specific.Value = "" Then
                        If oForm.Items.Item("Item_79").Specific.Value = "" Then

                            FL = GetValue("SELECT T0.""Location"" FROM OWHS T0 WHERE T0.""WhsCode""='" + oForm.Items.Item("Item_23").Specific.Value + "'", "Location")
                            TL = GetValue("SELECT T0.""Location"" FROM OWHS T0 WHERE T0.""WhsCode""='" + oForm.Items.Item("Item_25").Specific.Value + "'", "Location")
                            WTM = GetValue("SELECT T0.""U_TrsfMode"" FROM OWHS T0 WHERE T0.""WhsCode""='" + oForm.Items.Item("Item_25").Specific.Value + "'", "U_TrsfMode")

                            If (FL = TL) Or WTM = "Pipe" Then
                            Else
                                __oApplication.MessageBox("Transporter Code  Can Not Blank", 1, "Ok", "", "")
                                BubbleEvent = False
                            End If



                        End If



                    End If



                    If m1.RowCount <= 0 Then
                        __oApplication.MessageBox("Matrix Can Not Blank ", 1, "Ok", "", "")
                        BubbleEvent = False
                        Exit Sub
                    End If
                    Dim Flag As Boolean
                    For iRow As Integer = 1 To m1.VisualRowCount
                        If CDec(m1.Columns.Item("Col_8").Cells.Item(iRow).Specific.Value) > 0 Then
                            Flag = True
                            ' Exit For
                        Else
                            Flag = False
                            Exit For
                        End If

                        Dim ItemGrp As String = GetValue("Select ""ItmsGrpNam"" from OITB Where ""ItmsGrpCod"" =(SELECT T0.""ItmsGrpCod"" FROM OITM T0 WHERE T0.""ItemCode"" ='" + m1.Columns.Item("Col_0").Cells.Item(iRow).Specific.Value + "')", "ItmsGrpNam")
                        If ItemGrp = "Trading" And "TR100005" <> m1.Columns.Item("Col_0").Cells.Item(iRow).Specific.Value Then
                            If CDec(m1.Columns.Item("Col_6").Cells.Item(iRow).Specific.Value) > 0 Then
                                Flag = True

                            Else
                                Flag = False
                                Exit For
                            End If

                            If CDec(m1.Columns.Item("Col_7").Cells.Item(iRow).Specific.Value) > 0 Then
                                Flag = True

                            Else
                                Flag = False
                                Exit For
                            End If

                            If CDec(m1.Columns.Item("Col_10").Cells.Item(iRow).Specific.Value) > 0 Then
                                Flag = True

                            Else
                                Flag = False
                                Exit For
                            End If
                        End If
                        If (m1.Columns.Item("Col_11").Cells.Item(iRow).Specific.Value) <> "" Then
                            Flag = True

                        Else
                            Flag = False
                            Exit For
                        End If
                        If (m1.Columns.Item("Col_12").Cells.Item(iRow).Specific.Value) <> "" Then
                            Flag = True

                        Else
                            Flag = False
                            Exit For
                        End If
                        If (m1.Columns.Item("Col_13").Cells.Item(iRow).Specific.Value) <> "" Then
                            Flag = True

                        Else
                            Flag = False
                            Exit For
                        End If
                        If (m1.Columns.Item("Col_14").Cells.Item(iRow).Specific.Value) <> "" Then
                            Flag = True

                        Else
                            Flag = False
                            Exit For
                        End If


                        ocombo = oForm.Items.Item("cbIIT").Specific
                        If ocombo.Selected.Value = "DE" Then
                            Dim Itemcode As String = m1.Columns.Item("Col_0").Cells.Item(iRow).Specific.Value
                            Dim TempM As String = GetValue("Select IFNULL(""U_Temp"",'N') AS  ""TEMP"" From OITM Where ""ItemCode""='" + Itemcode + "'", "TEMP")
                            If TempM = "Y" Then
                                Dim sc As String = __oApplication.Company.ServerDate
                                Dim Date1 As Date = Nothing
                                Date1 = DateTime.ParseExact(sc, GetDateFormat, DateTimeFormatInfo.InvariantInfo)
                                Dim SQLQUERY = "select Count(*) AS COUNT from oitw where ""ItemCode""='" & m1.Columns.Item("Col_0").Cells.Item(iRow).Specific.Value & "' and ""WhsCode""='" & oForm.Items.Item("Item_25").Specific.Value & "'"
                                Dim cnt As String = GetValue(SQLQUERY, "COUNT")

                                If cnt = "0" Then
                                    Dim SQLQUERY1 = "select Count(*) AS COUNT1 from owhs where ""WhsCode""='" & oForm.Items.Item("Item_25").Specific.Value & "' and ""U_Category""='Hold' "
                                    Dim cnt1 As String = GetValue(SQLQUERY1, "COUNT1")
                                    If cnt1 > 0 Then
                                        __oApplication.MessageBox("Please Select the correct To Warehouse!!To  warehouse catogory Hold not allowed")
                                        BubbleEvent = False
                                        Exit Sub
                                    Else
                                        __oApplication.MessageBox("Please select correct To Warehouse for proceed!! ToWarehouse not maintained")
                                        BubbleEvent = False
                                        Exit Sub
                                    End If
                                End If
                                Dim SQLQUERY2 = "select Count(*) AS COUNT1 from owhs where ""WhsCode""='" & oForm.Items.Item("Item_25").Specific.Value & "' and ""U_Category""='Hold' "
                                Dim cnt2 As String = GetValue(SQLQUERY2, "COUNT1")
                                If cnt2 > 0 Then
                                    __oApplication.MessageBox("Please Select the correct To Warehouse!!To  warehouse catogory Hold not allowed")
                                    BubbleEvent = False
                                    Exit Sub
                                End If
                                Dim Query = "CALL SAP_GET_TempValdation('" & m1.Columns.Item("Col_0").Cells.Item(iRow).Specific.Value & "','" & oForm.Items.Item("Item_25").Specific.Value & "','" & oForm.Items.Item("Item_26").Specific.Value & "')"
                                '                                Dim Query = "CALL SAP_GET_TempValdation('" &
                                '                                    m1.Columns.Item("Col_0").Cells.Item(iRow).Specific.Value & 

                                '                                "','" & 
                                'oForm.Items.Item("Item_25").Specific.Value &
                                '                                "','" &
                                '                                oForm.Items.Item("Item_26").Specific.Value &
                                '                                "')"
                                Dim oRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                oRs.DoQuery(Query)
                                If oRs.RecordCount > 0 Then
                                    If CInt(oRs.Fields.Item("Count").Value) <= 0 Then
                                        __oApplication.MessageBox("First Update Temp Master Information")
                                        BubbleEvent = False
                                        Exit Sub
                                    End If
                                End If
                            End If
                        End If


                    Next
                    Dim ItemWaitActual, ItemWait As Decimal
                    ocombo = oForm.Items.Item("cbIIT").Specific
                    If ocombo.Selected.Value <> "S" Then


                        If Flag = False Then
                            __oApplication.MessageBox("Please Select Matrix Row and Enter Qty/Temp/Dip/Density/Office/Route/Equipment_Tank_Vehicles", 1, "Ok", "", "")
                            BubbleEvent = False
                            Exit Sub

                        End If
                    End If



                    If oForm.Items.Item("Item_79").Specific.Value.ToString = "" And oForm.Items.Item("Item_40").Specific.Value.ToString = "" And oForm.Items.Item("Item_10").Specific.Value.ToString <> "" Then

                        Dim QLV As String = "CALL SAP_GET_Inventory_Transfer_Request_BasedValidation('" + oForm.Items.Item("Item_10").Specific.Value.ToString + "' )"

                        Dim oRsLV As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRsLV.DoQuery(QLV)
                        If oRsLV.RecordCount > 0 Then


                            If WTM = "Pipe" Then
                            Else



                                If CInt(oRsLV.Fields.Item("Validation").Value) = 1 Then

                                    Dim m2 As SAPbouiCOM.Matrix = oForm.Items.Item("Item_45").Specific
                                    Dim FlagLV As Boolean
                                    Flag = False
                                    For iRowLV As Integer = 1 To m2.VisualRowCount


                                        If (m2.Columns.Item("Col_2").Cells.Item(iRowLV).Specific.Value) <> "" And (m2.Columns.Item("Col_6").Cells.Item(iRowLV).Specific.Value) <> "" Then
                                            Flag = True
                                            Exit For
                                        Else
                                            Flag = False

                                        End If
                                    Next

                                    If Flag = False Then
                                        __oApplication.MessageBox("Please Enter Landed cost Information", 1, "Ok", "", "")
                                        BubbleEvent = False
                                        Exit Sub
                                    End If
                                End If
                            End If
                        End If

                    End If



                    UDOITDocEntry = ""
                    UDOITDocNum = ""
                    UDOITCardCode = ""

                    UDOITDocEntry = oForm.Items.Item("Item_15").Specific.Value.ToString
                    UDOITDocNum = oForm.Items.Item("Item_14").Specific.Value.ToString
                    UDOITCardCode = oForm.Items.Item("Item_31").Specific.Value.ToString





                End If
            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Add_OnAfterItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                'If oForm.Mode = BoFormMode.fm_ADD_MODE Then
                '    DefulatSetting(oForm.UniqueID, BubbleEvent)
                'End If

                If oForm.Mode <> SAPbouiCOM.BoFormMode.fm_FIND_MODE Then

                    Dim DocEntry As String
                    '   If oForm.Items.Item("Item_42").Specific.Value = "" Then
                    If UDOITDocNum = "" And UDOITDocEntry = "" Then
                        DocEntry = GetValue(" Select  T0.""DocEntry"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocNum"" ='" + oForm.Items.Item("Item_14").Specific.Value.ToString + "' And T0.""DocEntry"" ='" + oForm.Items.Item("Item_15").Specific.Value.ToString + "'", "DocEntry")
                    Else
                        DocEntry = GetValue(" Select  T0.""DocEntry"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocNum"" ='" + UDOITDocNum + "' And T0.""DocEntry"" ='" + UDOITDocEntry + "'", "DocEntry")
                    End If


                    SAP_Tranction(FormUID, pVal, BubbleEvent, DocEntry)
                    'End If

                    UDOITDocEntry = ""
                    UDOITDocNum = ""
                    UDOITCardCode = ""


                    Try
                        __Application.ActivateMenuItem("1304")
                    Catch ex As Exception

                    End Try

                End If

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub


        Private Sub SAP_Tranction(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                If DocEntry.ToString <> "" Then
                    Dim TransType As String
                    Dim TransTypeQuery As String = ""
                    TransTypeQuery = "CALL SAP_GET_TransctionType ('" + DocEntry.ToString + "')"
                    Dim TransTypeoRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    TransTypeoRs.DoQuery(TransTypeQuery)
                    If TransTypeoRs.RecordCount > 0 Then
                        TransType = TransTypeoRs.Fields.Item("TransType").Value
                    End If
                    ''''''''''''''''''''' This Transction Use For Base On Inventry Transfer Request '''''''''''''''''''''''''''''''
                    Dim FL, TL, FW, TW, FB, TB, PODE, ITDE, GRN, GI, GR, TGI, INV, IT, TTLGIDN, INVTYPE, LCJ, AJE1, AJE2 As String
                    Dim LS As Int64
                    If TransType = "BOIR" Then
                        Dim Query As String = ""
                        Query = "CALL SAP_GET_Condition_Inventory_Transction ('" + DocEntry.ToString + "')"
                        Dim oRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRs.DoQuery(Query)
                        If oRs.RecordCount > 0 Then
                            FL = oRs.Fields.Item("FromLocation").Value
                            TL = oRs.Fields.Item("ToLcation").Value
                            FW = oRs.Fields.Item("U_FW").Value
                            TW = oRs.Fields.Item("U_TW").Value
                            FB = oRs.Fields.Item("U_FB").Value
                            TB = oRs.Fields.Item("U_TB").Value

                        End If
                        If FL <> TL Then
                            'LS = 0
                            'LS = GetValue("SELECT Count(T0.""DocEntry"") AS COUNT FROM ""@SAP_IT4""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' AND IFNULL(T0.""U_ItemCode"",'')<>''", "COUNT")
                            'If LS <= 0 Then
                            LossCalCulation(FormUID, pVal, BubbleEvent, DocEntry, "ST")
                            'End If
                        End If
                        ''''''''''''''''''''' This Transction Use For Base On Inventry Transfer Request '''''''''''''''''''END'''''''''''''''''''''''''''''''''''''''

                        ''''''''''''''''''''' This Transction Use For Base On GRN Start '''''''''''''''''''END'''''''''''''''''''''''''''''''''''''''
                    ElseIf TransType = "BOGRN" Then
                        Dim ItemCode As String = GetValue("Select TOP 1 ""U_ItemCode"" From  ""@SAP_IT1""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' And IFNULL(""U_ItemCode"" ,'')<>'' ", "U_ItemCode")

                        Dim ItemGrp As String = GetValue("Select ""ItmsGrpNam"" from OITB Where ""ItmsGrpCod"" =(SELECT T0.""ItmsGrpCod"" FROM OITM T0 WHERE T0.""ItemCode"" ='" + ItemCode + "')", "ItmsGrpNam")
                        If ItemGrp = "Trading" Then

                            'LS = 0
                            'LS = GetValue("SELECT Count(T0.""DocEntry"") AS COUNT FROM ""@SAP_IT4""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' AND IFNULL(T0.""U_ItemCode"",'')<>''", "COUNT")
                            'If LS <= 0 Then
                            LossCalCulation(FormUID, pVal, BubbleEvent, DocEntry, "PF")
                            'End If
                        End If

                    End If





                    'If oForm.Mode = BoFormMode.fm_ADD_MODE Then
                    '    oForm.Mode = SAPbouiCOM.BoFormMode.fm_FIND_MODE
                    '    oForm.Items.Item("Item_15").Enabled = True
                    '    Dim txDocEntry As SAPbouiCOM.EditText = oForm.Items.Item("Item_15").Specific
                    '    txDocEntry.Value = DocEntry.ToString()
                    '    oForm.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                    '    oForm.Items.Item("Item_15").Enabled = False
                    'End If
                End If

            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub PostSAP_OnAfterItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                If oForm.Mode <> BoFormMode.fm_OK_MODE Then
                    __oApplication.MessageBox("Form should be OK mode", 1, "Ok", "", "")
                    BubbleEvent = False
                    Exit Sub
                End If
                Dim DocEntry As String = oForm.Items.Item("Item_15").Specific.Value
                Dim PostSQL As String
                Dim refNo As String = oForm.Items.Item("Item_95").Specific.Value

                If refNo = "" Then
                    __oApplication.MessageBox("Please enter RefNo before proceed ", 1, "Ok", "", "")
                    BubbleEvent = False
                    Exit Sub
                End If

                'PostSQL = "Update ""@SAP_OTMD"" Set ""U_TRANFlag""='Y' Where ""DocEntry""='" + DocEntry.ToString + "'"
                'Dim PostoRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                'PostoRs.DoQuery(PostSQL)


                ' SAP_Tranction(FormUID, pVal, BubbleEvent, DocEntry)


                If DocEntry.ToString <> "" Then
                    Dim TransType As String
                    Dim TransTypeQuery As String = ""
                    TransTypeQuery = "CALL SAP_GET_TransctionType ('" + DocEntry.ToString + "')"
                    Dim TransTypeoRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    TransTypeoRs.DoQuery(TransTypeQuery)
                    If TransTypeoRs.RecordCount > 0 Then
                        TransType = TransTypeoRs.Fields.Item("TransType").Value
                    End If
                    ''''''''''''''''''''' This Transction Use For Base On Inventry Transfer Request '''''''''''''''''''''''''''''''
                    Dim FL, TL, FW, TW, FB, TB, PODE, ITDE, GRN, GI, GR, TGI, INV, IT, TTLGIDN, INVTYPE, LCJ, AJE1, AJE2 As String
                    Dim LS As Int64
                    If TransType = "BOIR" Then
                        Dim Query As String = ""
                        Query = "CALL SAP_GET_Condition_Inventory_Transction ('" + DocEntry.ToString + "')"
                        Dim oRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRs.DoQuery(Query)
                        If oRs.RecordCount > 0 Then
                            FL = oRs.Fields.Item("FromLocation").Value
                            TL = oRs.Fields.Item("ToLcation").Value
                            FW = oRs.Fields.Item("U_FW").Value
                            TW = oRs.Fields.Item("U_TW").Value
                            FB = oRs.Fields.Item("U_FB").Value
                            TB = oRs.Fields.Item("U_TB").Value

                        End If

                        '   If FL = TL And FB = TB 
                        If FL = TL Then
                            IT = GetValue("SELECT T0.""U_ITDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITDE")
                            If IT = "" Then
                                Normal_InventoryTransfer(FormUID, pVal, BubbleEvent, DocEntry)
                            End If
                        End If

                        'If FL <> TL And FB <> TB Then
                        If FL <> TL Then
                            PODE = GetValue("SELECT  T0.""U_PODE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_PODE")
                            If PODE = "" Then
                                Dim LCAmt As Decimal = GetValue("SELECT Sum(IFNULL(T0.""U_Amt"",0)) AS ""LC"" FROM ""@SAP_IT2""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "LC")
                                If LCAmt > 0 Then
                                    PurchaseOrder_Creation(FormUID, pVal, BubbleEvent, DocEntry)
                                End If

                            End If

                            IT = GetValue("SELECT T0.""U_ITDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITDE")
                            If IT = "" Then
                                Normal_InventoryTransfer(FormUID, pVal, BubbleEvent, DocEntry)
                            End If

                            ITDE = GetValue("SELECT T0.""U_ITDocE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITDocE")
                            If ITDE <> "" Then



                                Dim ItemCode As String = GetValue("Select TOP 1 ""U_ItemCode"" From  ""@SAP_IT1""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' And IFNULL(""U_ItemCode"" ,'')<>'' ", "U_ItemCode")
                                Dim ItemGrp As String = GetValue("Select ""ItmsGrpNam"" from OITB Where ""ItmsGrpCod"" =(SELECT T0.""ItmsGrpCod"" FROM OITM T0 WHERE T0.""ItemCode"" ='" + ItemCode + "')", "ItmsGrpNam")
                                If ItemGrp = "Trading" Then
                                    Dim QC As String = GetValue("SELECT T0.""U_QC"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_QC")
                                    If QC = "A" Then



                                        GI = GetValue("SELECT T0.""U_GIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GIDE")
                                        If GI = "" Then
                                            CreateGoodsIssue(FormUID, pVal, BubbleEvent, DocEntry)
                                        End If
                                        GI = GetValue("SELECT T0.""U_GIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GIDE")
                                        If GI <> "" Then



                                            GR = GetValue("SELECT T0.""U_GRDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GRDE")
                                            If GR = "" Then
                                                CreateGoodsReceipt(FormUID, pVal, BubbleEvent, DocEntry)
                                            End If

                                            GRN = GetValue("SELECT T0.""U_GRNDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GRNDE")
                                            If GRN = "" Then
                                                GoodReceiptPO_Creation(FormUID, pVal, BubbleEvent, DocEntry)
                                            End If


                                            'LS = 0
                                            'LS = GetValue("SELECT Count(T0.""DocEntry"") AS COUNT FROM ""@SAP_IT4""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' AND IFNULL(T0.""U_ItemCode"",'')<>''", "COUNT")
                                            'If LS <= 0 Then
                                            '    LossCalCulation(FormUID, pVal, BubbleEvent, DocEntry, "ST")
                                            'End If

                                            TGI = GetValue("SELECT T0.""U_TGIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TGIDE")
                                            If TGI = "" Then
                                                Create_TransportationLoss_GoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "ST")
                                            End If

                                            INV = GetValue("SELECT T0.""U_ARDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ARDE")
                                            If INV = "" Then
                                                ARInvoice_Creation(FormUID, pVal, BubbleEvent, DocEntry, "ST")
                                            End If


                                            INVTYPE = GetValue("SELECT T0.""U_ITT"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITT")
                                            If INVTYPE = "DE" Then
                                                TTLGIDN = GetValue("SELECT T0.""U_TTLGIDN"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TTLGIDN")
                                                If TTLGIDN = "" Then
                                                    TannkTempLoss_CreateGoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "ST")
                                                End If
                                            End If

                                            LCJ = GetValue("SELECT T0.""U_JE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_JE")
                                            If LCJ = "" Then
                                                LandedCost_JounralEntry(FormUID, pVal, BubbleEvent, DocEntry)
                                            End If

                                            AJE1 = GetValue("SELECT T0.""U_AJE1"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_AJE1")
                                            If AJE1 = "" Then
                                                AdjustmentJounralEntry_FromBranch(FormUID, pVal, BubbleEvent, DocEntry)
                                            End If

                                            AJE2 = GetValue("SELECT T0.""U_AJE2"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_AJE2")
                                            If AJE2 = "" Then
                                                AdjustmentJounralEntry_ToBranch(FormUID, pVal, BubbleEvent, DocEntry)
                                            End If

                                        End If

                                    Else

                                        If __oApplication.MessageBox("Transcation not process because of QC Not Approved Please Change the Reject Warehouse .. If You Change Reject Warehouse Then Continue", 1, "Yes", "No", "") = 2 Then
                                            BubbleEvent = False
                                            Exit Sub
                                        Else
                                            Dim Branch As String = GetValue("Select  ""U_TB"" From  ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' And IFNULL(""U_TB"" ,'')<>'' ", "U_TB")
                                            Dim Whs As String = GetValue("Select  ""U_TW"" From  ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' And IFNULL(""U_TW"" ,'')<>'' ", "U_TW")
                                            Dim HWhs As String = GetValue("SELECT T0.""WhsCode"" FROM OWHS T0 WHERE T0.""U_Category"" ='Reject'  and  T0.""BPLid"" = '" + Branch + "'", "WhsCode")
                                            If Whs <> HWhs Then
                                                __oApplication.MessageBox("Transcation not process because of QC Not Approved Please Change the Reject Warehouse..", 1, "Ok", "", "")
                                                BubbleEvent = False
                                                Exit Sub
                                            Else


                                                GI = GetValue("SELECT T0.""U_GIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GIDE")
                                                If GI = "" Then
                                                    CreateGoodsIssue(FormUID, pVal, BubbleEvent, DocEntry)
                                                End If
                                                GI = GetValue("SELECT T0.""U_GIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GIDE")
                                                If GI <> "" Then



                                                    GR = GetValue("SELECT T0.""U_GRDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GRDE")
                                                    If GR = "" Then
                                                        CreateGoodsReceipt(FormUID, pVal, BubbleEvent, DocEntry)
                                                    End If

                                                    GRN = GetValue("SELECT T0.""U_GRNDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GRNDE")
                                                    If GRN = "" Then
                                                        GoodReceiptPO_Creation(FormUID, pVal, BubbleEvent, DocEntry)
                                                    End If


                                                    'LS = 0
                                                    'LS = GetValue("SELECT Count(T0.""DocEntry"") AS COUNT FROM ""@SAP_IT4""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' AND IFNULL(T0.""U_ItemCode"",'')<>''", "COUNT")
                                                    'If LS <= 0 Then
                                                    '    LossCalCulation(FormUID, pVal, BubbleEvent, DocEntry, "ST")
                                                    'End If

                                                    TGI = GetValue("SELECT T0.""U_TGIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TGIDE")
                                                    If TGI = "" Then
                                                        Create_TransportationLoss_GoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "ST")
                                                    End If

                                                    INV = GetValue("SELECT T0.""U_ARDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ARDE")
                                                    If INV = "" Then
                                                        ARInvoice_Creation(FormUID, pVal, BubbleEvent, DocEntry, "ST")
                                                    End If


                                                    INVTYPE = GetValue("SELECT T0.""U_ITT"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITT")
                                                    If INVTYPE = "DE" Then
                                                        TTLGIDN = GetValue("SELECT T0.""U_TTLGIDN"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TTLGIDN")
                                                        If TTLGIDN = "" Then
                                                            TannkTempLoss_CreateGoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "ST")
                                                        End If
                                                    End If

                                                    LCJ = GetValue("SELECT T0.""U_JE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_JE")
                                                    If LCJ = "" Then
                                                        LandedCost_JounralEntry(FormUID, pVal, BubbleEvent, DocEntry)
                                                    End If

                                                    AJE1 = GetValue("SELECT T0.""U_AJE1"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_AJE1")
                                                    If AJE1 = "" Then
                                                        AdjustmentJounralEntry_FromBranch(FormUID, pVal, BubbleEvent, DocEntry)
                                                    End If

                                                    AJE2 = GetValue("SELECT T0.""U_AJE2"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_AJE2")
                                                    If AJE2 = "" Then
                                                        AdjustmentJounralEntry_ToBranch(FormUID, pVal, BubbleEvent, DocEntry)
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If


                                Else



                                    GI = GetValue("SELECT T0.""U_GIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GIDE")
                                    If GI = "" Then
                                        CreateGoodsIssue(FormUID, pVal, BubbleEvent, DocEntry)
                                    End If
                                    GI = GetValue("SELECT T0.""U_GIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GIDE")
                                    If GI <> "" Then


                                        GR = GetValue("SELECT T0.""U_GRDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GRDE")
                                        If GR = "" Then
                                            CreateGoodsReceipt(FormUID, pVal, BubbleEvent, DocEntry)
                                        End If

                                        GRN = GetValue("SELECT T0.""U_GRNDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GRNDE")
                                        If GRN = "" Then
                                            GoodReceiptPO_Creation(FormUID, pVal, BubbleEvent, DocEntry)
                                        End If


                                        'LS = 0
                                        'LS = GetValue("SELECT Count(T0.""DocEntry"") AS COUNT FROM ""@SAP_IT4""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' AND IFNULL(T0.""U_ItemCode"",'')<>''", "COUNT")
                                        'If LS <= 0 Then
                                        '    LossCalCulation(FormUID, pVal, BubbleEvent, DocEntry, "ST")
                                        'End If

                                        TGI = GetValue("SELECT T0.""U_TGIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TGIDE")
                                        If TGI = "" Then
                                            Create_TransportationLoss_GoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "ST")
                                        End If

                                        INV = GetValue("SELECT T0.""U_ARDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ARDE")
                                        If INV = "" Then
                                            ARInvoice_Creation(FormUID, pVal, BubbleEvent, DocEntry, "ST")
                                        End If


                                        INVTYPE = GetValue("SELECT T0.""U_ITT"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITT")
                                        If INVTYPE = "DE" Then
                                            TTLGIDN = GetValue("SELECT T0.""U_TTLGIDN"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TTLGIDN")
                                            If TTLGIDN = "" Then
                                                TannkTempLoss_CreateGoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "ST")
                                            End If
                                        End If

                                        LCJ = GetValue("SELECT T0.""U_JE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_JE")
                                        If LCJ = "" Then
                                            LandedCost_JounralEntry(FormUID, pVal, BubbleEvent, DocEntry)
                                        End If

                                        AJE1 = GetValue("SELECT T0.""U_AJE1"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_AJE1")
                                        If AJE1 = "" Then
                                            AdjustmentJounralEntry_FromBranch(FormUID, pVal, BubbleEvent, DocEntry)
                                        End If

                                        AJE2 = GetValue("SELECT T0.""U_AJE2"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_AJE2")
                                        If AJE2 = "" Then
                                            AdjustmentJounralEntry_ToBranch(FormUID, pVal, BubbleEvent, DocEntry)
                                        End If
                                    End If
                                End If

                                Try
                                    Dim SqlQuery As New StringBuilder

                                    SqlQuery.Append("Update ""@SAP_OITR"" Set ""U_ITDocN""= (Select ""DocNum"" From ""@SAP_OIT"" Where ""DocEntry""='" + DocEntry + "'  ),")
                                    SqlQuery.Append(" ""U_ITDocE""= (Select ""DocEntry"" From ""@SAP_OIT"" Where ""DocEntry""='" + DocEntry + "' )")
                                    SqlQuery.Append("Where ""DocEntry""='" + GetValue("SELECT T0.""U_ITRDocE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITRDocE") + "'")


                                    Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                    oRset1.DoQuery(SqlQuery.ToString)
                                Catch ex As Exception

                                End Try
                            End If



                        End If
                        ''''''''''''''''''''' This Transction Use For Base On Inventry Transfer Request '''''''''''''''''''END'''''''''''''''''''''''''''''''''''''''

                        ''''''''''''''''''''' This Transction Use For Base On GRN Start '''''''''''''''''''END'''''''''''''''''''''''''''''''''''''''
                    ElseIf TransType = "BOGRN" Then


                        Dim ItemCode As String = GetValue("Select TOP 1 ""U_ItemCode"" From  ""@SAP_IT1""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' And IFNULL(""U_ItemCode"" ,'')<>'' ", "U_ItemCode")

                        Dim ItemGrp As String = GetValue("Select ""ItmsGrpNam"" from OITB Where ""ItmsGrpCod"" =(SELECT T0.""ItmsGrpCod"" FROM OITM T0 WHERE T0.""ItemCode"" ='" + ItemCode + "')", "ItmsGrpNam")
                        If ItemGrp = "Trading" Then
                            Dim QC As String = GetValue("SELECT T0.""U_QC"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_QC")
                            If QC = "A" Then
                                GRN = GetValue("SELECT T0.""U_GRNDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GRNDE")
                                If GRN = "" Then
                                    Purchase_GoodReceiptPO_Creation(FormUID, pVal, BubbleEvent, DocEntry)
                                End If


                                'LS = 0
                                'LS = GetValue("SELECT Count(T0.""DocEntry"") AS COUNT FROM ""@SAP_IT4""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' AND IFNULL(T0.""U_ItemCode"",'')<>''", "COUNT")
                                'If LS <= 0 Then
                                '    LossCalCulation(FormUID, pVal, BubbleEvent, DocEntry, "PF")
                                'End If

                                TGI = GetValue("SELECT T0.""U_TGIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TGIDE")
                                If TGI = "" Then
                                    Create_TransportationLoss_GoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "PF")
                                End If

                                INV = GetValue("SELECT T0.""U_ARDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ARDE")
                                If INV = "" Then
                                    ARInvoice_Creation(FormUID, pVal, BubbleEvent, DocEntry, "PF")
                                End If

                                INVTYPE = GetValue("SELECT T0.""U_ITT"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITT")
                                If INVTYPE = "DE" Then
                                    TTLGIDN = GetValue("SELECT T0.""U_TTLGIDN"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TTLGIDN")
                                    If TTLGIDN = "" Then
                                        TannkTempLoss_CreateGoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "PF")
                                    End If
                                End If


                                IT = GetValue("SELECT T0.""U_ITDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITDE")
                                If IT = "" Then
                                    Normal_InventoryTransfer(FormUID, pVal, BubbleEvent, DocEntry)
                                End If
                            Else

                                If __oApplication.MessageBox("Transcation not process because of QC Not Approved Please Change the Reject Warehouse .. If You Change Reject Warehouse Then Continua", 1, "Yes", "No", "") = 2 Then
                                    BubbleEvent = False
                                    Exit Sub
                                Else
                                    Dim Branch As String = GetValue("Select  ""U_TB"" From  ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' And IFNULL(""U_TB"" ,'')<>'' ", "U_TB")
                                    Dim Whs As String = GetValue("Select  ""U_TW"" From  ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' And IFNULL(""U_TW"" ,'')<>'' ", "U_TW")
                                    Dim HWhs As String = GetValue("SELECT T0.""WhsCode"" FROM OWHS T0 WHERE T0.""U_Category"" ='Reject'  and  T0.""BPLid"" = '" + Branch + "'", "WhsCode")

                                    If Whs <> HWhs Then
                                        __oApplication.MessageBox("Transcation not process because of QC Not Approved Please Change the Reject Warehouse..", 1, "Ok", "", "")
                                        BubbleEvent = False
                                        Exit Sub
                                    Else
                                        GRN = GetValue("SELECT T0.""U_GRNDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GRNDE")
                                        If GRN = "" Then
                                            Purchase_GoodReceiptPO_Creation(FormUID, pVal, BubbleEvent, DocEntry)
                                        End If


                                        'LS = 0
                                        'LS = GetValue("SELECT Count(T0.""DocEntry"") AS COUNT FROM ""@SAP_IT4""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' AND IFNULL(T0.""U_ItemCode"",'')<>''", "COUNT")
                                        'If LS <= 0 Then
                                        '    LossCalCulation(FormUID, pVal, BubbleEvent, DocEntry, "PF")
                                        'End If

                                        TGI = GetValue("SELECT T0.""U_TGIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TGIDE")
                                        If TGI = "" Then
                                            Create_TransportationLoss_GoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "PF")
                                        End If

                                        INV = GetValue("SELECT T0.""U_ARDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ARDE")
                                        If INV = "" Then
                                            ARInvoice_Creation(FormUID, pVal, BubbleEvent, DocEntry, "PF")
                                        End If

                                        INVTYPE = GetValue("SELECT T0.""U_ITT"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITT")
                                        If INVTYPE = "DE" Then
                                            TTLGIDN = GetValue("SELECT T0.""U_TTLGIDN"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TTLGIDN")
                                            If TTLGIDN = "" Then
                                                TannkTempLoss_CreateGoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "PF")
                                            End If
                                        End If


                                        IT = GetValue("SELECT T0.""U_ITDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITDE")
                                        If IT = "" Then
                                            Normal_InventoryTransfer(FormUID, pVal, BubbleEvent, DocEntry)
                                        End If
                                    End If
                                End If


                            End If
                        Else
                            GRN = GetValue("SELECT T0.""U_GRNDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GRNDE")
                            If GRN = "" Then
                                Purchase_GoodReceiptPO_Creation(FormUID, pVal, BubbleEvent, DocEntry)
                            End If


                            'LS = 0
                            'LS = GetValue("SELECT Count(T0.""DocEntry"") AS COUNT FROM ""@SAP_IT4""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' AND IFNULL(T0.""U_ItemCode"",'')<>''", "COUNT")
                            'If LS <= 0 Then
                            '    LossCalCulation(FormUID, pVal, BubbleEvent, DocEntry, "PF")
                            'End If

                            TGI = GetValue("SELECT T0.""U_TGIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TGIDE")
                            If TGI = "" Then
                                Create_TransportationLoss_GoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "PF")
                            End If

                            INV = GetValue("SELECT T0.""U_ARDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ARDE")
                            If INV = "" Then
                                ARInvoice_Creation(FormUID, pVal, BubbleEvent, DocEntry, "PF")
                            End If

                            INVTYPE = GetValue("SELECT T0.""U_ITT"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITT")
                            If INVTYPE = "DE" Then
                                TTLGIDN = GetValue("SELECT T0.""U_TTLGIDN"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TTLGIDN")
                                If TTLGIDN = "" Then
                                    TannkTempLoss_CreateGoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "PF")
                                End If
                            End If


                            IT = GetValue("SELECT T0.""U_ITDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITDE")
                            If IT = "" Then
                                Normal_InventoryTransfer(FormUID, pVal, BubbleEvent, DocEntry)
                            End If

                        End If

                    End If


                    '''This Code For QC Sample Type 
                    '''''''''''''''''''''
                    Dim QCTYPE As String = GetValue("SELECT T0.""U_ITT"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITT")
                    Dim QCNO As String = GetValue("SELECT T0.""U_QCNO"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_QCNO")

                    If QCTYPE = "S" And QCNO <> "" Then

                        IT = GetValue("SELECT T0.""U_ITDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITDE")
                        If IT = "" Then
                            Normal_InventoryTransfer(FormUID, pVal, BubbleEvent, DocEntry)
                        End If
                    End If


                    '''This Code For Topping 
                    '''''''''''''''''''''
                    If QCTYPE = "TP" Or QCTYPE = "S" Then

                        IT = GetValue("SELECT T0.""U_ITDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITDE")
                        If IT = "" Then
                            Normal_InventoryTransfer(FormUID, pVal, BubbleEvent, DocEntry)
                        End If
                    End If



                End If

                __Application.ActivateMenuItem("1304")
            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub



        Private Sub Normal_InventoryTransfer(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                Dim oST As SAPbobsCOM.StockTransfer = Nothing
                oST = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oStockTransfer)


                Dim Sql As String = "CALL SAP_GET_Inventory_Transfer_SAP_Data ('" + DocEntry + "')"
                Dim oRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRs.DoQuery(Sql)
                If oRs.RecordCount > 0 Then

                    oST.CardCode = oRs.Fields.Item("U_TCode").Value & ""
                    oST.DocDate = oRs.Fields.Item("U_PDate").Value


                    oST.Comments = "Inventory Transfer DocNum -" + oRs.Fields.Item("DocNum").Value.ToString
                    oST.UserFields.Fields.Item("U_ITDocE").Value = oRs.Fields.Item("DocEntry").Value & ""
                    oST.UserFields.Fields.Item("U_ITDocN").Value = oRs.Fields.Item("DocNum").Value & ""

                    oST.FromWarehouse = oRs.Fields.Item("U_FW").Value
                    oST.ToWarehouse = oRs.Fields.Item("U_TW").Value

                    While oRs.EoF = False

                        oST.Lines.ItemCode = oRs.Fields.Item("U_ItemCode").Value & ""
                        oST.Lines.Quantity = oRs.Fields.Item("U_Qty").Value & ""

                        oST.Lines.FromWarehouseCode = oRs.Fields.Item("U_FW").Value
                        Try
                            oST.Lines.WarehouseCode = oRs.Fields.Item("U_TW").Value
                        Catch ex As Exception

                        End Try

                        If oRs.Fields.Item("U_Chamber").Value <> "" Then
                            oST.Lines.UserFields.Fields.Item("U_Chamber").Value = oRs.Fields.Item("U_Chamber").Value & ""
                        End If

                        oST.Lines.UserFields.Fields.Item("U_Temp").Value = oRs.Fields.Item("U_Temp").Value & ""
                        oST.Lines.UserFields.Fields.Item("U_Density").Value = oRs.Fields.Item("U_Density").Value & ""
                        oST.Lines.UserFields.Fields.Item("U_Dip").Value = oRs.Fields.Item("U_Dip").Value & ""
                        oST.Lines.UoMEntry = oRs.Fields.Item("UomEntry").Value & ""

                        oST.Lines.Add()

                        oRs.MoveNext()
                    End While
                    Dim result As Integer = oST.Add()
                    If result = 0 Then




                        Try

                            '''This Code For QC Sample Type 
                            '''''''''''''''''''''
                            Dim QCTYPE As String = GetValue("SELECT T0.""U_ITT"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITT")
                            Dim QCNO As String = GetValue("SELECT T0.""U_QCNO"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_QCNO")


                            If QCTYPE = "S" And QCNO <> "" Then
                                Dim SqlQuery_QC As New StringBuilder
                                SqlQuery_QC.Append("Update ""@ITN_OQCD"" Set ""U_SMPLSN""='" + DocEntry + "'")
                                SqlQuery_QC.Append("Where ""DocEntry""='" + QCNO + "'")

                                Dim oRset_QC As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                oRset_QC.DoQuery(SqlQuery_QC.ToString)
                            Else

                                Dim SqlQuery As New StringBuilder

                                SqlQuery.Append("Update ""@SAP_OIT"" Set ""U_ITDN""= (Select ""DocNum"" From OWTR Where ""U_ITDocE""='" + DocEntry + "'  ),")
                                SqlQuery.Append(" ""U_ITDE""= (Select ""DocEntry"" From OWTR Where ""U_ITDocE""='" + DocEntry + "' )")
                                SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")

                                Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                                oRset1.DoQuery(SqlQuery.ToString)
                            End If


                        Catch ex As Exception

                        End Try


                        __oApplication.StatusBar.SetText("Inventory Trnasfer Generated", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                    Else
                        __oApplication.StatusBar.SetText("Error: Inventory Trnasfer Generated - " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)


                    End If
                End If

                '''KRAO ADDED CODE
                '''
                ' Dim SqlQuery As New StringBuilder
                Dim str As String


                str = "UPDATE B  SET U_ITDOCN_S=A.""DocNum"",U_ITDOCE_S=A.""DocEntry""  FROM ""@SAP_OIT"" A INNER JOIN ""@SAP_ITR2"" B ON A.""U_ITRDocE""=B.""DocEntry"" AND IFNULL(A.U_TRNO,'')=IFNULL(B.U_TRNO,'') WHERE A.""DocEntry"" ='" + DocEntry + "' and U_STT='S' AND U_QC='A' and B.U_TRNO='" + oForm.Items.Item("Item_35").Specific.Value + "'"
                'SqlQuery.Append("Update ""@SAP_OIT"" Set ""U_ITDN""= (Select ""DocNum"" From OWTR Where ""U_ITDocE""='" + DocEntry + "'  ),")
                'SqlQuery.Append(" ""U_ITDE""= (Select ""DocEntry"" From OWTR Where ""U_ITDocE""='" + DocEntry + "' )")
                'SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")

                Dim oRset2 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRset2.DoQuery(str)

                str = "UPDATE B  SET U_ITDOCN_R=A.""DocNum"",U_ITDOCE_R=A.""DocEntry""   FROM ""@SAP_OIT"" A INNER JOIN ""@SAP_ITR2"" B ON A.""U_ITRDocE""=B.""DocEntry"" AND IFNULL(A.U_TRNO,'')=IFNULL(B.U_TRNO,'') WHERE A.""DocEntry"" ='" + DocEntry + "' and U_STT='R' AND U_QC='A'and B.U_TRNO='" + oForm.Items.Item("Item_35").Specific.Value + "'"
                oRset2.DoQuery(str)


            Catch ex As Exception
                __oApplication.MessageBox("[SUB-InventoryTransfer] - " & ex.Message, 1, "Ok", "", "")

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
                Sql = "CALL SAP_GET_Purchase_Orde_Data ('" + DocEntry + "')"
                oRs = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRs.DoQuery(Sql)
                If oRs.RecordCount > 0 Then


                    Dim oPurchaseOrder As SAPbobsCOM.Documents = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders)

                    '---This is for Header Details of Purchase Order----

                    oPurchaseOrder.CardCode = oRs.Fields.Item("U_CardCode").Value.ToString()
                    oPurchaseOrder.DocDate = oRs.Fields.Item("U_PDate").Value.ToString()
                    oPurchaseOrder.DocDueDate = oRs.Fields.Item("U_PDate").Value.ToString()

                    oPurchaseOrder.BPL_IDAssignedToInvoice = oRs.Fields.Item("Branch").Value.ToString()



                    oPurchaseOrder.UserFields.Fields.Item("U_ITDocE").Value = oRs.Fields.Item("DocEntry").Value & ""
                    oPurchaseOrder.UserFields.Fields.Item("U_ITDocN").Value = oRs.Fields.Item("DocNum").Value & ""
                    oPurchaseOrder.UserFields.Fields.Item("U_Flag").Value = "NSSPO" & ""

                    'added by mahesh
                    oPurchaseOrder.UserFields.Fields.Item("U_Comments").Value = oRs.Fields.Item("U_TRNO").Value & ""

                    ''---This is for Purchase Order Line---


                    Dim oPurchaseOrder_Line As SAPbobsCOM.Document_Lines = oPurchaseOrder.Lines
                    While oRs.EoF = False

                        oPurchaseOrder_Line.ItemCode = oRs.Fields.Item("ItemCode").Value
                        oPurchaseOrder_Line.Quantity = oRs.Fields.Item("U_Qty").Value

                        oPurchaseOrder_Line.WarehouseCode = oRs.Fields.Item("WhsCode").Value
                        oPurchaseOrder_Line.Price = oRs.Fields.Item("Price").Value

                        oPurchaseOrder_Line.CostingCode = oRs.Fields.Item("U_OCRC").Value & ""
                        oPurchaseOrder_Line.CostingCode2 = oRs.Fields.Item("U_OCRC2").Value & ""
                        oPurchaseOrder_Line.CostingCode3 = oRs.Fields.Item("U_OCRC3").Value & ""
                        oPurchaseOrder_Line.CostingCode4 = oRs.Fields.Item("U_OCRC4").Value & ""
                        oPurchaseOrder_Line.CostingCode5 = oRs.Fields.Item("U_OCRC5").Value & ""
                        oPurchaseOrder_Line.UserFields.Fields.Item("U_Chamber").Value = oRs.Fields.Item("U_Chamber").Value & ""
                        oPurchaseOrder_Line.AgreementNo = oRs.Fields.Item("AbsID").Value & ""

                        oPurchaseOrder_Line.Add()
                        oRs.MoveNext()
                    End While

                    Dim Result As Integer = oPurchaseOrder.Add()



                    If Result <> 0 Then

                        ' If Result = -5002 Then
                        Manager_PurchaseOrder_Creation(FormUID, pVal, BubbleEvent, DocEntry)
                        'Else
                        ' __oApplication.StatusBar.SetText("Error: In Generating Purchase Order - " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        'End If


                    Else

                        Dim SqlQuery As New StringBuilder

                        SqlQuery.Append("Update ""@SAP_OIT"" Set ""U_PODN""= (Select ""DocNum"" From OPOR Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='NSSPO'  ),")
                        SqlQuery.Append(" ""U_PODE""= (Select ""DocEntry"" From OPOR Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='NSSPO' )")
                        SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")





                        Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRset1.DoQuery(SqlQuery.ToString)
                        __oApplication.StatusBar.SetText("Purchase Order Generated", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    End If
                End If

                'End If

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
                BubbleEvent = False
            End Try
        End Sub

        Private Sub GoodReceiptPO_Creation(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim oRs As SAPbobsCOM.Recordset = Nothing
                Dim oRsVendor As SAPbobsCOM.Recordset = Nothing
                Dim Sql As String = ""





                ''----Fetch the Reocrd for Purchase Order Creation----
                Sql = "CALL SAP_GET_GRN_Data ('" + DocEntry + "')"
                oRs = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRs.DoQuery(Sql)
                If oRs.RecordCount > 0 Then
                    Dim oGoodReceiptPO As SAPbobsCOM.Documents = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes)

                    oGoodReceiptPO.CardCode = oRs.Fields.Item("CardCode").Value.ToString()
                    oGoodReceiptPO.DocDate = oRs.Fields.Item("U_PDate").Value.ToString()
                    oGoodReceiptPO.DocDueDate = oRs.Fields.Item("U_PDate").Value.ToString()
                    oGoodReceiptPO.BPL_IDAssignedToInvoice = oRs.Fields.Item("Branch").Value.ToString()
                    oGoodReceiptPO.UserFields.Fields.Item("U_ITDocE").Value = oRs.Fields.Item("DocEntry").Value & ""
                    oGoodReceiptPO.UserFields.Fields.Item("U_ITDocN").Value = oRs.Fields.Item("DocNum").Value & ""
                    oGoodReceiptPO.NumAtCard = oRs.Fields.Item("U_Ref").Value & ""

                    'removed : cause -> multiple values updating on single column
                    oGoodReceiptPO.UserFields.Fields.Item("U_TRNO").Value = oRs.Fields.Item("U_TRNO").Value & ""


                    ''---This is for Purchase Order Line---
                    Dim oPurchaseOrder_Line As SAPbobsCOM.Document_Lines = oGoodReceiptPO.Lines
                    While oRs.EoF = False

                        oPurchaseOrder_Line.ItemCode = oRs.Fields.Item("ItemCode").Value
                        oPurchaseOrder_Line.Quantity = oRs.Fields.Item("Quantity").Value
                        oPurchaseOrder_Line.UnitPrice = oRs.Fields.Item("Price").Value

                        oPurchaseOrder_Line.CostingCode = oRs.Fields.Item("OcrCode").Value & ""
                        oPurchaseOrder_Line.CostingCode2 = oRs.Fields.Item("OcrCode2").Value & ""
                        oPurchaseOrder_Line.CostingCode3 = oRs.Fields.Item("OcrCode3").Value & ""
                        oPurchaseOrder_Line.CostingCode4 = oRs.Fields.Item("OcrCode4").Value & ""
                        oPurchaseOrder_Line.CostingCode5 = oRs.Fields.Item("OcrCode5").Value & ""
                        oPurchaseOrder_Line.UserFields.Fields.Item("U_Chamber").Value = oRs.Fields.Item("U_Chamber").Value & ""

                        oPurchaseOrder_Line.BaseLine = oRs.Fields.Item("LineNum").Value & ""
                        oPurchaseOrder_Line.BaseType = "22"
                        oPurchaseOrder_Line.BaseEntry = oRs.Fields.Item("BaseEntry").Value & ""


                        oPurchaseOrder_Line.Add()
                        oRs.MoveNext()
                    End While

                    Dim Result As Integer = oGoodReceiptPO.Add()
                    If Result <> 0 Then
                        __oApplication.StatusBar.SetText("Error: In Generating Good Receipt PO- " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Else

                        Dim SqlQuery As New StringBuilder

                        SqlQuery.Append("Update ""@SAP_OIT"" Set ""U_GRNDN""= (Select ""DocNum"" From OPDN Where ""U_ITDocE""='" + DocEntry + "'  ),")
                        SqlQuery.Append(" ""U_GRNDE""= (Select ""DocEntry"" From OPDN Where ""U_ITDocE""='" + DocEntry + "' )")
                        SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")


                        Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRset1.DoQuery(SqlQuery.ToString)
                        __oApplication.StatusBar.SetText("Good Receipt PO Generated", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    End If
                End If

                'End If

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub CreateGoodsIssue(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim errItemCodes As String = ""
                Dim errMachineNos As String = ""

                Dim LineID As String = ""
                Dim errRowFlag As Boolean = False

                Dim oGoodsIssue As SAPbobsCOM.Documents = Nothing
                Dim oRecordset As SAPbobsCOM.Recordset = Nothing
                Dim oRs1 As SAPbobsCOM.Recordset = Nothing

                Dim ErrorCode As Integer
                Dim ErrMessage As String
                Dim iCount As Integer = 1



                Dim Sql As String = ""
                Sql = "CALL SAP_GET_GoodIssue_Data('" + DocEntry + "')"

                oRecordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRecordset.DoQuery(Sql)

                If oRecordset.RecordCount > 0 Then

                    oGoodsIssue = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit)

                    oGoodsIssue.DocDate = oRecordset.Fields.Item("U_PDate").Value & ""
                    oGoodsIssue.TaxDate = oRecordset.Fields.Item("U_PDate").Value & ""
                    oGoodsIssue.DocDueDate = oRecordset.Fields.Item("U_PDate").Value & ""
                    'oGoodsIssue.Reference1 = oRecordset.Fields.Item("U_RefNo").Value & ""
                    oGoodsIssue.Comments = oRecordset.Fields.Item("U_Remark").Value & ""
                    oGoodsIssue.BPL_IDAssignedToInvoice = CInt(oRecordset.Fields.Item("Branch").Value)
                    oGoodsIssue.UserFields.Fields.Item("U_ITDocE").Value = oRecordset.Fields.Item("DocEntry").Value & ""
                    oGoodsIssue.UserFields.Fields.Item("U_ITDocN").Value = oRecordset.Fields.Item("DocNum").Value & ""
                    oGoodsIssue.UserFields.Fields.Item("U_Flag").Value = "STITGI" & ""
                    oGoodsIssue.Series = oRecordset.Fields.Item("Series").Value & ""





                    'Adding Row level Data
                    While oRecordset.EoF = False

                        oGoodsIssue.Lines.ItemCode = oRecordset.Fields.Item("U_ItemCode").Value & ""
                        oGoodsIssue.Lines.Quantity = oRecordset.Fields.Item("U_Qty").Value
                        oGoodsIssue.Lines.WarehouseCode = oRecordset.Fields.Item("U_FW").Value & ""

                        oGoodsIssue.Lines.CostingCode = oRecordset.Fields.Item("U_OCRC").Value & ""
                        oGoodsIssue.Lines.CostingCode2 = oRecordset.Fields.Item("U_OCRC2").Value & ""
                        oGoodsIssue.Lines.CostingCode3 = oRecordset.Fields.Item("U_OCRC3").Value & ""
                        oGoodsIssue.Lines.CostingCode4 = oRecordset.Fields.Item("U_OCRC4").Value & ""
                        oGoodsIssue.Lines.CostingCode5 = oRecordset.Fields.Item("U_OCRC5").Value & ""
                        oGoodsIssue.Lines.UserFields.Fields.Item("U_Chamber").Value = oRecordset.Fields.Item("U_Chamber").Value & ""


                        oGoodsIssue.Lines.UserFields.Fields.Item("U_Temp").Value = oRecordset.Fields.Item("U_Temp").Value & ""
                        oGoodsIssue.Lines.UserFields.Fields.Item("U_Density").Value = oRecordset.Fields.Item("U_Density").Value & ""
                        oGoodsIssue.Lines.UserFields.Fields.Item("U_Dip").Value = oRecordset.Fields.Item("U_Dip").Value & ""
                        oGoodsIssue.Lines.AccountCode = oRecordset.Fields.Item("AccountCode").Value & ""


                        oGoodsIssue.Lines.Add()
                        iCount = iCount + 1
                        oRecordset.MoveNext()

                    End While

                    Dim Result As Integer = oGoodsIssue.Add()

                    If Result <> 0 Then
                        __oApplication.StatusBar.SetText("Error: Good Issue Not Generated - " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        '  If Result = -5002 Then

                        Maanger_CreateGoodsIssue(FormUID, pVal, BubbleEvent, DocEntry)
                        ' Else
                        '__oApplication.StatusBar.SetText("Error: Good Issue Not Generated - " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        'End If


                    Else


                        Dim SqlQuery As New StringBuilder

                        SqlQuery.Append("Update ""@SAP_OIT"" Set ""U_GIDN""= (Select ""DocNum"" From OIGE Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='STITGI'  ),")
                        SqlQuery.Append(" ""U_GIDE""= (Select ""DocEntry"" From OIGE Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='STITGI'  )")
                        SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")


                        Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRset1.DoQuery(SqlQuery.ToString)
                        __oApplication.StatusBar.SetText("Good Issue Generated", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)


                    End If

                End If

                Marshal.ReleaseComObject(oRecordset)
                Try
                    Marshal.ReleaseComObject(oGoodsIssue)
                Catch ex As Exception

                End Try



            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub CreateGoodsReceipt(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                Dim errItemCodes As String = ""
                Dim errMachineNos As String = ""

                Dim LineID As String = ""
                Dim errRowFlag As Boolean = False

                Dim oGoodsReceipt As SAPbobsCOM.Documents = Nothing
                Dim oRecordset As SAPbobsCOM.Recordset = Nothing
                Dim oRs1 As SAPbobsCOM.Recordset = Nothing

                Dim ErrorCode As Integer
                Dim ErrMessage As String
                Dim iCount As Integer = 1



                Dim Sql As String = ""
                Sql = "CALL SAP_GET_GoodReceipt_Data('" + DocEntry + "')"

                oRecordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRecordset.DoQuery(Sql)

                If oRecordset.RecordCount > 0 Then

                    oGoodsReceipt = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry)

                    oGoodsReceipt.DocDate = oRecordset.Fields.Item("U_PDate").Value & ""
                    oGoodsReceipt.TaxDate = oRecordset.Fields.Item("U_PDate").Value & ""
                    oGoodsReceipt.DocDueDate = oRecordset.Fields.Item("U_PDate").Value & ""

                    oGoodsReceipt.Comments = oRecordset.Fields.Item("U_Remark").Value & ""
                    oGoodsReceipt.BPL_IDAssignedToInvoice = CInt(oRecordset.Fields.Item("Branch").Value)
                    oGoodsReceipt.UserFields.Fields.Item("U_ITDocE").Value = oRecordset.Fields.Item("DocEntry").Value & ""
                    oGoodsReceipt.UserFields.Fields.Item("U_ITDocN").Value = oRecordset.Fields.Item("DocNum").Value & ""
                    oGoodsReceipt.UserFields.Fields.Item("U_Flag").Value = "STITGR" & ""
                    oGoodsReceipt.Series = oRecordset.Fields.Item("Series").Value & ""





                    'Adding Row level Data
                    While oRecordset.EoF = False

                        oGoodsReceipt.Lines.ItemCode = oRecordset.Fields.Item("ItemCode").Value & ""
                        oGoodsReceipt.Lines.Quantity = oRecordset.Fields.Item("Quantity").Value
                        oGoodsReceipt.Lines.WarehouseCode = oRecordset.Fields.Item("U_TW").Value & ""
                        'oGoodsReceipt.Lines.Price = oRecordset.Fields.Item("Price").Value & ""
                        oGoodsReceipt.Lines.LineTotal = oRecordset.Fields.Item("LineTotal").Value & ""

                        oGoodsReceipt.Lines.CostingCode = oRecordset.Fields.Item("OcrCode").Value & ""
                        oGoodsReceipt.Lines.CostingCode2 = oRecordset.Fields.Item("OcrCode2").Value & ""
                        oGoodsReceipt.Lines.CostingCode3 = oRecordset.Fields.Item("OcrCode3").Value & ""
                        oGoodsReceipt.Lines.CostingCode4 = oRecordset.Fields.Item("OcrCode4").Value & ""
                        oGoodsReceipt.Lines.CostingCode5 = oRecordset.Fields.Item("OcrCode5").Value & ""
                        oGoodsReceipt.Lines.UserFields.Fields.Item("U_Chamber").Value = oRecordset.Fields.Item("U_Chamber").Value & ""
                        oGoodsReceipt.Lines.UserFields.Fields.Item("U_Temp").Value = oRecordset.Fields.Item("U_Temp").Value & ""
                        oGoodsReceipt.Lines.UserFields.Fields.Item("U_Density").Value = oRecordset.Fields.Item("U_Density").Value & ""
                        oGoodsReceipt.Lines.UserFields.Fields.Item("U_Dip").Value = oRecordset.Fields.Item("U_Dip").Value & ""

                        oGoodsReceipt.Lines.AccountCode = oRecordset.Fields.Item("AccountCode").Value & ""


                        oGoodsReceipt.Lines.Add()
                        iCount = iCount + 1
                        oRecordset.MoveNext()

                    End While

                    Dim Result As Integer = oGoodsReceipt.Add()

                    If Result <> 0 Then
                        'If Result = -5002 Then

                        Maanger_CreateGoodsReceipt(FormUID, pVal, BubbleEvent, DocEntry)
                        'Else
                        '    __oApplication.StatusBar.SetText("Error: Good Receipt Not Generated - " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        'End If
                        '  __oApplication.StatusBar.SetText("Error: Good Receipt Not Generated - " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Else


                        Dim SqlQuery As New StringBuilder

                        SqlQuery.Append("Update ""@SAP_OIT"" Set ""U_GRDN""= (Select ""DocNum"" From OIGN Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='STITGR' ),")
                        SqlQuery.Append(" ""U_GRDE""= (Select ""DocEntry"" From OIGN Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='STITGR' )")
                        SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")


                        Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRset1.DoQuery(SqlQuery.ToString)
                        __oApplication.StatusBar.SetText("Good Receipt Generated", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)


                    End If

                End If

                Marshal.ReleaseComObject(oRecordset)
                Try
                    Marshal.ReleaseComObject(oGoodsReceipt)
                Catch ex As Exception
                    BubbleEvent = False
                End Try



            Catch ex As Exception
                BubbleEvent = False
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub LossCalCulation(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String, ByVal Type As String)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                Dim oGeneralService As SAPbobsCOM.GeneralService
                Dim oGeneralData As SAPbobsCOM.GeneralData
                Dim oSons As SAPbobsCOM.GeneralDataCollection
                Dim oSon As SAPbobsCOM.GeneralData
                Dim sCmp As SAPbobsCOM.CompanyService
                Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
                Try

                    Dim LS As Int64 = 0
                    LS = GetValue("SELECT Count(T0.""DocEntry"") AS COUNT FROM ""@SAP_IT4""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' AND IFNULL(T0.""U_ItemCode"",'')<>''", "COUNT")
                    If LS >= 0 Then


                        Dim Delete As String = "Delete From   ""@SAP_IT4""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'"


                        Dim H1oRsetDelete As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        H1oRsetDelete.DoQuery(Delete)
                    End If



                Catch ex As Exception

                End Try










                'Dim oGeneralService As SAPbobsCOM.GeneralService
                'Dim oGeneralData As SAPbobsCOM.GeneralData
                'Dim oSons As SAPbobsCOM.GeneralDataCollection
                'Dim oSon As SAPbobsCOM.GeneralData
                'Dim sCmp As SAPbobsCOM.CompanyService
                sCmp = __bobCompany.GetCompanyService

                'Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
                'Get a handle to the SM_MOR UDO
                oGeneralService = sCmp.GetGeneralService("SAP_UDO_OIT")
                ' Specify data for main UDO
                oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                oGeneralParams.SetProperty("DocEntry", DocEntry)
                oGeneralData = oGeneralService.GetByParams(oGeneralParams)





                Dim H1 As String = ""
                If Type = "ST" Then
                    H1 = "CALL SAP_GET_LossCalCulation('" + DocEntry + "')  "
                ElseIf Type = "PF" Then
                    H1 = "CALL SAP_GET_Purchase_LossCalCulation('" + DocEntry + "')  "
                End If

                Dim H1oRset As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                H1oRset.DoQuery(H1)
                If H1oRset.RecordCount > 0 Then
                    Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                    Dim rowcount As Integer = m1.VisualRowCount
                    If rowcount <> H1oRset.RecordCount Then
                        __oApplication.MessageBox("Loss Calculation rows does not match the chamber rows!! can not proceed", 1, "Ok", "", "")
                        BubbleEvent = False
                        Exit Sub
                    End If
                    For i As Integer = 0 To H1oRset.RecordCount - 1

                        oSons = oGeneralData.Child("SAP_IT4")
                        oSon = oSons.Add

                        oSon.SetProperty("U_ItemCode", H1oRset.Fields.Item("ItemCode").Value & "")
                        oSon.SetProperty("U_Chamber", H1oRset.Fields.Item("U_Chamber").Value & "")
                        oSon.SetProperty("U_ChemLos", H1oRset.Fields.Item("ChemberLoss").Value & "")
                        oSon.SetProperty("U_TemLoss", H1oRset.Fields.Item("TempLoss").Value & "")
                        oSon.SetProperty("U_ClLoss", H1oRset.Fields.Item("ClaimableLoss").Value & "")
                        oSon.SetProperty("U_TTL", H1oRset.Fields.Item("TankTempLoss").Value & "")

                        H1oRset.MoveNext()
                    Next
                End If
                oGeneralService.Update(oGeneralData)
                __oApplication.StatusBar.SetText("Loss Calculation Completed", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)






            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent - Loss] - " & ex.Message, 1, "Ok", "", "")

            End Try

        End Sub

        Private Sub Create_TransportationLoss_GoodsIssue(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String, ByVal Type As String)
            Try
                Dim errItemCodes As String = ""
                Dim errMachineNos As String = ""

                Dim LineID As String = ""
                Dim errRowFlag As Boolean = False

                Dim oGoodsIssue As SAPbobsCOM.Documents = Nothing
                Dim oRecordset As SAPbobsCOM.Recordset = Nothing
                Dim oRs1 As SAPbobsCOM.Recordset = Nothing

                Dim ErrorCode As Integer
                Dim ErrMessage As String
                Dim iCount As Integer = 1

                Dim Sql As String = ""
                If Type = "ST" Then
                    Sql = "CALL SAP_GET_StockTransfer_GoodIssue_TempLoss_Data('" + DocEntry + "')"
                ElseIf Type = "PF" Then
                    Sql = "CALL SAP_GET_GoodIssue_TempLoss_Data('" + DocEntry + "')"
                End If





                oRecordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRecordset.DoQuery(Sql)

                If oRecordset.RecordCount > 0 Then

                    oGoodsIssue = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit)

                    oGoodsIssue.DocDate = oRecordset.Fields.Item("U_PDate").Value & ""
                    oGoodsIssue.TaxDate = oRecordset.Fields.Item("U_PDate").Value & ""
                    oGoodsIssue.DocDueDate = oRecordset.Fields.Item("U_PDate").Value & ""
                    'oGoodsIssue.Reference1 = oRecordset.Fields.Item("U_RefNo").Value & ""
                    oGoodsIssue.Comments = "Based on Inventory Transfer " + oRecordset.Fields.Item("DocNum").Value.ToString + " ,Tolerance Loss" & ""
                    oGoodsIssue.BPL_IDAssignedToInvoice = CInt(oRecordset.Fields.Item("Branch").Value)
                    oGoodsIssue.UserFields.Fields.Item("U_ITDocE").Value = oRecordset.Fields.Item("DocEntry").Value & ""
                    oGoodsIssue.UserFields.Fields.Item("U_ITDocN").Value = oRecordset.Fields.Item("DocNum").Value & ""
                    oGoodsIssue.UserFields.Fields.Item("U_Flag").Value = "TL" & ""
                    oGoodsIssue.Series = oRecordset.Fields.Item("Series").Value & ""





                    'Adding Row level Data
                    While oRecordset.EoF = False

                        oGoodsIssue.Lines.ItemCode = oRecordset.Fields.Item("U_ItemCode").Value & ""
                        oGoodsIssue.Lines.Quantity = oRecordset.Fields.Item("U_TemLoss").Value


                        If Type = "ST" Then
                            oGoodsIssue.Lines.WarehouseCode = oRecordset.Fields.Item("U_TW").Value & ""
                        ElseIf Type = "PF" Then
                            oGoodsIssue.Lines.WarehouseCode = oRecordset.Fields.Item("U_FW").Value & ""
                        End If


                        oGoodsIssue.Lines.UserFields.Fields.Item("U_Chamber").Value = oRecordset.Fields.Item("U_Chamber").Value & ""
                        oGoodsIssue.Lines.CostingCode = oRecordset.Fields.Item("U_OCRC").Value & ""
                        oGoodsIssue.Lines.CostingCode2 = oRecordset.Fields.Item("U_OCRC2").Value & ""
                        oGoodsIssue.Lines.CostingCode3 = oRecordset.Fields.Item("U_OCRC3").Value & ""
                        oGoodsIssue.Lines.CostingCode4 = oRecordset.Fields.Item("U_OCRC4").Value & ""
                        oGoodsIssue.Lines.CostingCode5 = oRecordset.Fields.Item("U_OCRC5").Value & ""

                        oGoodsIssue.Lines.UserFields.Fields.Item("U_Temp").Value = oRecordset.Fields.Item("U_Temp").Value & ""
                        oGoodsIssue.Lines.UserFields.Fields.Item("U_Density").Value = oRecordset.Fields.Item("U_Density").Value & ""
                        oGoodsIssue.Lines.UserFields.Fields.Item("U_Dip").Value = oRecordset.Fields.Item("U_Dip").Value & ""


                        oGoodsIssue.Lines.Add()
                        iCount = iCount + 1
                        oRecordset.MoveNext()

                    End While

                    Dim Result As Integer = oGoodsIssue.Add()

                    If Result <> 0 Then

                        'If Result = -5002 Then

                        Manager_Create_TransportationLoss_GoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, Type)
                        'Else
                        '    __oApplication.StatusBar.SetText("Error: Tolerance Loss Good Issue Not Generated - " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        'End If
                        '  __oApplication.StatusBar.SetText("Error: Tolerance Loss Good Issue Not Generated - " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                    Else


                        Dim SqlQuery As New StringBuilder

                        SqlQuery.Append("Update ""@SAP_OIT"" Set ""U_TGIDN""= (Select ""DocNum"" From OIGE Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='TL'  ),")
                        SqlQuery.Append(" ""U_TGIDE""= (Select ""DocEntry"" From OIGE Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='TL')")
                        SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")


                        Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRset1.DoQuery(SqlQuery.ToString)
                        __oApplication.StatusBar.SetText(" Tolerance Loss Good Issue  Generated", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)


                    End If

                End If

                Marshal.ReleaseComObject(oRecordset)
                Try
                    'Marshal.ReleaseComObject(oGoodsIssue)
                Catch ex As Exception

                End Try



            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
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


                If Type = "ST" Then
                    Sql = "CALL SAP_GET_StockTransfer_AR_Invoice_Data ('" + DocEntry + "')"
                ElseIf Type = "PF" Then
                    Sql = "CALL SAP_GET_AR_Invoice_Data ('" + DocEntry + "')"
                End If

                ''----Fetch the Reocrd for Purchase Order Creation----

                oRs = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRs.DoQuery(Sql)
                If oRs.RecordCount > 0 Then
                    Dim oPurchaseInvoice As SAPbobsCOM.Documents = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices)

                    PartyID = oRs.Fields.Item("U_TCode").Value.ToString()

                    oPurchaseInvoice.CardCode = oRs.Fields.Item("U_TCode").Value.ToString()
                    oPurchaseInvoice.DocDate = oRs.Fields.Item("U_PDate").Value.ToString()
                    oPurchaseInvoice.DocDueDate = oRs.Fields.Item("U_PDate").Value.ToString()
                    oPurchaseInvoice.BPL_IDAssignedToInvoice = oRs.Fields.Item("Branch").Value.ToString()
                    oPurchaseInvoice.UserFields.Fields.Item("U_ITDocE").Value = oRs.Fields.Item("DocEntry").Value & ""
                    oPurchaseInvoice.UserFields.Fields.Item("U_ITDocN").Value = oRs.Fields.Item("DocNum").Value & ""



                    ''---This is for Purchase Order Line---
                    Dim oPurchaseInvoice_Line As SAPbobsCOM.Document_Lines = oPurchaseInvoice.Lines
                    While oRs.EoF = False

                        oPurchaseInvoice_Line.ItemCode = oRs.Fields.Item("U_ItemCode").Value
                        oPurchaseInvoice_Line.Quantity = oRs.Fields.Item("U_ClLoss").Value


                        If Type = "ST" Then
                            oPurchaseInvoice_Line.WarehouseCode = oRs.Fields.Item("U_TW").Value
                        ElseIf Type = "PF" Then
                            oPurchaseInvoice_Line.WarehouseCode = oRs.Fields.Item("U_FW").Value
                        End If

                        oPurchaseInvoice_Line.TaxCode = oRs.Fields.Item("TaxCode").Value
                        oPurchaseInvoice_Line.Price = oRs.Fields.Item("Price").Value

                        oPurchaseInvoice_Line.UserFields.Fields.Item("U_Chamber").Value = oRs.Fields.Item("U_Chamber").Value & ""
                        oPurchaseInvoice_Line.CostingCode = oRs.Fields.Item("U_OCRC").Value & ""
                        oPurchaseInvoice_Line.CostingCode2 = oRs.Fields.Item("U_OCRC2").Value & ""
                        oPurchaseInvoice_Line.CostingCode3 = oRs.Fields.Item("U_OCRC3").Value & ""
                        oPurchaseInvoice_Line.CostingCode4 = oRs.Fields.Item("U_OCRC4").Value & ""
                        oPurchaseInvoice_Line.CostingCode5 = oRs.Fields.Item("U_OCRC5").Value & ""
                        oPurchaseInvoice_Line.UserFields.Fields.Item("U_Temp").Value = oRs.Fields.Item("U_Temp").Value & ""
                        oPurchaseInvoice_Line.UserFields.Fields.Item("U_Density").Value = oRs.Fields.Item("U_Density").Value & ""
                        oPurchaseInvoice_Line.UserFields.Fields.Item("U_Dip").Value = oRs.Fields.Item("U_Dip").Value & ""


                        oPurchaseInvoice_Line.Add()
                        oRs.MoveNext()
                    End While

                    Dim Result As Integer = oPurchaseInvoice.Add()
                    If Result <> 0 Then
                        'If Result = -5002 Then

                        Manager_ARInvoice_Creation(FormUID, pVal, BubbleEvent, DocEntry, Type)
                        'Else
                        '    __oApplication.StatusBar.SetText("Error: In Generating AR Invoice - " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        'End If

                        '__oApplication.StatusBar.SetText("Error: In Generating AR Invoice - " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                    Else

                        Dim SqlQuery As New StringBuilder

                        SqlQuery.Append("Update ""@SAP_OIT"" Set ""U_ARDN""= (Select ""DocNum"" From OINV Where ""U_ITDocE""='" + DocEntry + "' AND ""CardCode""='" + PartyID + "'),")
                        SqlQuery.Append(" ""U_ARDE""= (Select ""DocEntry"" From OINV Where ""U_ITDocE""='" + DocEntry + "' AND ""CardCode""='" + PartyID + "')")
                        SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")

                        Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRset1.DoQuery(SqlQuery.ToString)
                        __oApplication.StatusBar.SetText("AR Invoice Generated", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    End If
                End If

                'End If

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub


        Private Sub Purchase_GoodReceiptPO_Creation(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim oRs As SAPbobsCOM.Recordset = Nothing
                Dim oRsVendor As SAPbobsCOM.Recordset = Nothing
                Dim Sql As String = ""





                ''----Fetch the Reocrd for Purchase Order Creation----
                Sql = "CALL SAP_GET_Purchase_GRN_Data ('" + DocEntry + "')"
                oRs = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRs.DoQuery(Sql)
                If oRs.RecordCount > 0 Then
                    Dim oGoodReceiptPO As SAPbobsCOM.Documents = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes)

                    oGoodReceiptPO.CardCode = oRs.Fields.Item("CardCode").Value.ToString()
                    oGoodReceiptPO.DocDate = oRs.Fields.Item("DocDate").Value.ToString()
                    oGoodReceiptPO.DocDueDate = oRs.Fields.Item("DocDate").Value.ToString()
                    oGoodReceiptPO.BPL_IDAssignedToInvoice = oRs.Fields.Item("Branch").Value.ToString()
                    oGoodReceiptPO.UserFields.Fields.Item("U_ITDocE").Value = oRs.Fields.Item("DocEntry").Value & ""
                    oGoodReceiptPO.UserFields.Fields.Item("U_Flag").Value = "ITSGRN" & ""
                    oGoodReceiptPO.NumAtCard = oRs.Fields.Item("VenderRef").Value & ""





                    ''---This is for Purchase Order Line---
                    Dim oPurchaseOrder_Line As SAPbobsCOM.Document_Lines = oGoodReceiptPO.Lines
                    While oRs.EoF = False

                        oPurchaseOrder_Line.ItemCode = oRs.Fields.Item("ItemCode").Value
                        oPurchaseOrder_Line.Quantity = oRs.Fields.Item("Quantity").Value
                        oPurchaseOrder_Line.UnitPrice = oRs.Fields.Item("Price").Value

                        oPurchaseOrder_Line.CostingCode = oRs.Fields.Item("OcrCode").Value & ""
                        oPurchaseOrder_Line.CostingCode2 = oRs.Fields.Item("OcrCode2").Value & ""
                        oPurchaseOrder_Line.CostingCode3 = oRs.Fields.Item("OcrCode3").Value & ""
                        oPurchaseOrder_Line.CostingCode4 = oRs.Fields.Item("OcrCode4").Value & ""
                        oPurchaseOrder_Line.CostingCode5 = oRs.Fields.Item("OcrCode5").Value & ""
                        oPurchaseOrder_Line.UserFields.Fields.Item("U_Chamber").Value = oRs.Fields.Item("U_Chamber").Value & ""

                        oPurchaseOrder_Line.BaseEntry = oRs.Fields.Item("BaseEntry").Value
                        oPurchaseOrder_Line.BaseType = 22
                        oPurchaseOrder_Line.BaseLine = oRs.Fields.Item("LineNum").Value


                        oPurchaseOrder_Line.Add()
                        oRs.MoveNext()
                    End While

                    Dim Result As Integer = oGoodReceiptPO.Add()
                    If Result <> 0 Then
                        'If Result = -5002 Then

                        Manager_Purchase_GoodReceiptPO_Creation(FormUID, pVal, BubbleEvent, DocEntry)
                        'Else
                        '    __oApplication.StatusBar.SetText("Error: In Generating Good Receipt PO- " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        'End If

                        '__oApplication.StatusBar.SetText("Error: In Generating Good Receipt PO- " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                    Else

                        Dim SqlQuery As New StringBuilder

                        SqlQuery.Append("Update ""@SAP_OIT"" Set ""U_GRNDN""= (Select ""DocNum"" From OPDN Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='ITSGRN'  ),")
                        SqlQuery.Append(" ""U_GRNDE""= (Select ""DocEntry"" From OPDN Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='ITSGRN' )")
                        SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")


                        Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRset1.DoQuery(SqlQuery.ToString)
                        __oApplication.StatusBar.SetText("Good Receipt PO Generated", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    End If
                End If

                'End If

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub TannkTempLoss_CreateGoodsIssue(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String, ByVal Type As String)
            Try
                Dim errItemCodes As String = ""
                Dim errMachineNos As String = ""

                Dim LineID As String = ""
                Dim errRowFlag As Boolean = False

                Dim oGoodsIssue As SAPbobsCOM.Documents = Nothing
                Dim oRecordset As SAPbobsCOM.Recordset = Nothing
                Dim oRs1 As SAPbobsCOM.Recordset = Nothing

                Dim ErrorCode As Integer
                Dim ErrMessage As String
                Dim iCount As Integer = 1



                Dim Sql As String = ""


                If Type = "ST" Then
                    Sql = "CALL SAP_GET_StockTransfer_GoodIssue_TankTempLoss_Data('" + DocEntry + "')"
                ElseIf Type = "PF" Then
                    Sql = "CALL SAP_GET_GoodIssue_TankTempLoss_Data('" + DocEntry + "')"
                End If


                oRecordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRecordset.DoQuery(Sql)

                If oRecordset.RecordCount > 0 Then

                    oGoodsIssue = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit)

                    oGoodsIssue.DocDate = oRecordset.Fields.Item("U_PDate").Value & ""
                    oGoodsIssue.TaxDate = oRecordset.Fields.Item("U_PDate").Value & ""
                    oGoodsIssue.DocDueDate = oRecordset.Fields.Item("U_PDate").Value & ""
                    'oGoodsIssue.Reference1 = oRecordset.Fields.Item("U_RefNo").Value & ""
                    oGoodsIssue.Comments = "Based on Inventory Transfer " + oRecordset.Fields.Item("DocNum").Value.ToString + " ,Tank Temp Loss" & ""
                    oGoodsIssue.BPL_IDAssignedToInvoice = CInt(oRecordset.Fields.Item("Branch").Value)
                    oGoodsIssue.UserFields.Fields.Item("U_ITDocE").Value = oRecordset.Fields.Item("DocEntry").Value & ""
                    oGoodsIssue.UserFields.Fields.Item("U_ITDocN").Value = oRecordset.Fields.Item("DocNum").Value & ""
                    oGoodsIssue.UserFields.Fields.Item("U_Flag").Value = "TTL" & ""
                    oGoodsIssue.Series = oRecordset.Fields.Item("Series").Value & ""





                    'Adding Row level Data
                    While oRecordset.EoF = False

                        oGoodsIssue.Lines.ItemCode = oRecordset.Fields.Item("U_ItemCode").Value & ""
                        oGoodsIssue.Lines.Quantity = oRecordset.Fields.Item("U_TTL").Value

                        If Type = "ST" Then
                            oGoodsIssue.Lines.WarehouseCode = oRecordset.Fields.Item("U_TW").Value & ""
                        ElseIf Type = "PF" Then
                            oGoodsIssue.Lines.WarehouseCode = oRecordset.Fields.Item("U_FW").Value & ""
                        End If



                        oGoodsIssue.Lines.UserFields.Fields.Item("U_Chamber").Value = oRecordset.Fields.Item("U_Chamber").Value & ""
                        oGoodsIssue.Lines.CostingCode = oRecordset.Fields.Item("U_OCRC").Value & ""
                        oGoodsIssue.Lines.CostingCode2 = oRecordset.Fields.Item("U_OCRC2").Value & ""
                        oGoodsIssue.Lines.CostingCode3 = oRecordset.Fields.Item("U_OCRC3").Value & ""
                        oGoodsIssue.Lines.CostingCode4 = oRecordset.Fields.Item("U_OCRC4").Value & ""
                        oGoodsIssue.Lines.CostingCode5 = oRecordset.Fields.Item("U_OCRC5").Value & ""


                        oGoodsIssue.Lines.UserFields.Fields.Item("U_Temp").Value = oRecordset.Fields.Item("U_Temp").Value & ""
                        oGoodsIssue.Lines.UserFields.Fields.Item("U_Density").Value = oRecordset.Fields.Item("U_Density").Value & ""
                        oGoodsIssue.Lines.UserFields.Fields.Item("U_Dip").Value = oRecordset.Fields.Item("U_Dip").Value & ""

                        oGoodsIssue.Lines.Add()
                        iCount = iCount + 1
                        oRecordset.MoveNext()

                    End While

                    Dim Result As Integer = oGoodsIssue.Add()

                    If Result <> 0 Then



                        'If Result = -5002 Then

                        Manager_TannkTempLoss_CreateGoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, Type)
                        'Else
                        '    __oApplication.StatusBar.SetText("Error:Tank Temp Loss Good Issue Not Generated - " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        'End If

                        '__oApplication.StatusBar.SetText("Error:Tank Temp Loss Good Issue Not Generated - " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                    Else


                        Dim SqlQuery As New StringBuilder

                        SqlQuery.Append("Update ""@SAP_OIT"" Set ""U_TTLGIDN""= (Select ""DocNum"" From OIGE Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='TTL'  ),")
                        SqlQuery.Append(" ""U_TTLGIDE""= (Select ""DocEntry"" From OIGE Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='TTL')")
                        SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")


                        Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRset1.DoQuery(SqlQuery.ToString)
                        __oApplication.StatusBar.SetText(" Tank Temp Loss Good Issue  Generated", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)


                    End If

                End If

                Marshal.ReleaseComObject(oRecordset)
                'Try
                '    Marshal.ReleaseComObject(oGoodsIssue)
                'Catch ex As Exception

                'End Try



            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub


        Private Sub LandedCost_JounralEntry(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)


                Dim oRecordset As SAPbobsCOM.Recordset = Nothing
                Dim Sql As String = ""
                Sql = "CALL SAP_GETDATA_LandedCost_JE ('" + DocEntry + "')"
                oRecordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRecordset.DoQuery(Sql)
                If oRecordset.RecordCount > 0 Then


                    Dim oJounalEntry As SAPbobsCOM.JournalEntries = Nothing
                    oJounalEntry = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
                    oJounalEntry.ReferenceDate = oRecordset.Fields.Item("Date").Value
                    oJounalEntry.TaxDate = oRecordset.Fields.Item("Date").Value
                    oJounalEntry.DueDate = oRecordset.Fields.Item("Date").Value
                    oJounalEntry.UserFields.Fields.Item("U_ITDocE").Value = oRecordset.Fields.Item("DocEntry").Value & ""
                    oJounalEntry.UserFields.Fields.Item("U_ITDocN").Value = oRecordset.Fields.Item("DocNum").Value & ""
                    oJounalEntry.UserFields.Fields.Item("U_Flag").Value = "LCJ"



                    oJounalEntry.Memo = "LC Based On Inventory Trasnsfer  '" + oRecordset.Fields.Item("DocNum").Value.ToString & "" + "'  "



                    oJounalEntry.Lines.BPLID = oRecordset.Fields.Item("U_TB").Value
                    oJounalEntry.Lines.AccountCode = oRecordset.Fields.Item("DebitAccount").Value
                    oJounalEntry.Lines.Debit = oRecordset.Fields.Item("U_Amt").Value & ""
                    oJounalEntry.Lines.Credit = 0
                    oJounalEntry.Lines.DueDate = oRecordset.Fields.Item("Date").Value
                    oJounalEntry.Lines.TaxDate = oRecordset.Fields.Item("Date").Value
                    oJounalEntry.Lines.ReferenceDate1 = oRecordset.Fields.Item("Date").Value

                    oJounalEntry.Lines.CostingCode = oRecordset.Fields.Item("U_OCRC").Value & ""
                    oJounalEntry.Lines.CostingCode2 = oRecordset.Fields.Item("U_OCRC2").Value & ""
                    oJounalEntry.Lines.CostingCode3 = oRecordset.Fields.Item("U_OCRC3").Value & ""
                    oJounalEntry.Lines.CostingCode4 = oRecordset.Fields.Item("U_OCRC4").Value & ""
                    oJounalEntry.Lines.CostingCode5 = oRecordset.Fields.Item("U_OCRC5").Value & ""

                    oJounalEntry.Lines.Add()



                    oJounalEntry.Lines.BPLID = oRecordset.Fields.Item("U_TB").Value
                    oJounalEntry.Lines.AccountCode = oRecordset.Fields.Item("CreditAccount").Value
                    oJounalEntry.Lines.Debit = 0
                    oJounalEntry.Lines.Credit = oRecordset.Fields.Item("U_Amt").Value & ""
                    oJounalEntry.Lines.DueDate = oRecordset.Fields.Item("Date").Value
                    oJounalEntry.Lines.TaxDate = oRecordset.Fields.Item("Date").Value
                    oJounalEntry.Lines.ReferenceDate1 = oRecordset.Fields.Item("Date").Value

                    oJounalEntry.Lines.CostingCode = oRecordset.Fields.Item("U_OCRC").Value & ""
                    oJounalEntry.Lines.CostingCode2 = oRecordset.Fields.Item("U_OCRC2").Value & ""
                    oJounalEntry.Lines.CostingCode3 = oRecordset.Fields.Item("U_OCRC3").Value & ""
                    oJounalEntry.Lines.CostingCode4 = oRecordset.Fields.Item("U_OCRC4").Value & ""
                    oJounalEntry.Lines.CostingCode5 = oRecordset.Fields.Item("U_OCRC5").Value & ""
                    oJounalEntry.Lines.Add()





                    Dim Result As Integer = oJounalEntry.Add()
                    If Result <> 0 Then
                        Manager_LandedCost_JounralEntry(FormUID, pVal, BubbleEvent, DocEntry)
                        'If Result = -5002 Then
                        '    Manager_LandedCost_JounralEntry(FormUID, pVal, BubbleEvent, DocEntry)
                        'Else
                        '    __oApplication.StatusBar.SetText("Error:Landde Cost JE Not Generated - " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        'End If
                    Else

                        Dim SqlQuery As New StringBuilder

                        SqlQuery.Append("Update ""@SAP_OIT"" Set ""U_JE""= (Select ""TransId"" From OJDT Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='LCJ'  )")
                        SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")


                        Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRset1.DoQuery(SqlQuery.ToString)

                        __oApplication.StatusBar.SetText("Landde Cost JE  Generated", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                    End If




                End If





            Catch ex As Exception
                '__oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")

                If ex.Message = "The logged-on user does not have permission to use this object" Then
                    Manager_LandedCost_JounralEntry(FormUID, pVal, BubbleEvent, DocEntry)
                Else
                    __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
                End If

            End Try
        End Sub


        Private Sub AdjustmentJounralEntry_FromBranch(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)


                Dim oRecordset As SAPbobsCOM.Recordset = Nothing
                Dim Sql As String = ""
                Sql = "CALL SAP_GETDATA_Adjustment_JE_FromBranch ('" + DocEntry + "')"
                oRecordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRecordset.DoQuery(Sql)
                If oRecordset.RecordCount > 0 Then


                    Dim oJounalEntry As SAPbobsCOM.JournalEntries = Nothing
                    oJounalEntry = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
                    oJounalEntry.ReferenceDate = oRecordset.Fields.Item("Date").Value
                    oJounalEntry.TaxDate = oRecordset.Fields.Item("Date").Value
                    oJounalEntry.DueDate = oRecordset.Fields.Item("Date").Value
                    oJounalEntry.UserFields.Fields.Item("U_ITDocE").Value = oRecordset.Fields.Item("DocEntry").Value & ""
                    oJounalEntry.UserFields.Fields.Item("U_ITDocN").Value = oRecordset.Fields.Item("DocNum").Value & ""
                    oJounalEntry.UserFields.Fields.Item("U_Flag").Value = "AJEFB"



                    oJounalEntry.Memo = "ADJ FB Based On Inventory Trasnsfer  '" + oRecordset.Fields.Item("DocNum").Value.ToString & "" + "'  "



                    oJounalEntry.Lines.BPLID = oRecordset.Fields.Item("Branch").Value
                    oJounalEntry.Lines.AccountCode = oRecordset.Fields.Item("DebitAccount").Value
                    oJounalEntry.Lines.Debit = oRecordset.Fields.Item("Amt").Value & ""
                    oJounalEntry.Lines.Credit = 0
                    oJounalEntry.Lines.DueDate = oRecordset.Fields.Item("Date").Value
                    oJounalEntry.Lines.TaxDate = oRecordset.Fields.Item("Date").Value
                    oJounalEntry.Lines.ReferenceDate1 = oRecordset.Fields.Item("Date").Value

                    oJounalEntry.Lines.CostingCode = oRecordset.Fields.Item("U_OCRC").Value & ""
                    oJounalEntry.Lines.CostingCode2 = oRecordset.Fields.Item("U_OCRC2").Value & ""
                    oJounalEntry.Lines.CostingCode3 = oRecordset.Fields.Item("U_OCRC3").Value & ""
                    oJounalEntry.Lines.CostingCode4 = oRecordset.Fields.Item("U_OCRC4").Value & ""
                    oJounalEntry.Lines.CostingCode5 = oRecordset.Fields.Item("U_OCRC5").Value & ""

                    oJounalEntry.Lines.Add()



                    oJounalEntry.Lines.BPLID = oRecordset.Fields.Item("Branch").Value
                    oJounalEntry.Lines.AccountCode = oRecordset.Fields.Item("CreditAccount").Value
                    oJounalEntry.Lines.Debit = 0
                    oJounalEntry.Lines.Credit = oRecordset.Fields.Item("Amt").Value & ""
                    oJounalEntry.Lines.DueDate = oRecordset.Fields.Item("Date").Value
                    oJounalEntry.Lines.TaxDate = oRecordset.Fields.Item("Date").Value
                    oJounalEntry.Lines.ReferenceDate1 = oRecordset.Fields.Item("Date").Value

                    oJounalEntry.Lines.CostingCode = oRecordset.Fields.Item("U_OCRC").Value & ""
                    oJounalEntry.Lines.CostingCode2 = oRecordset.Fields.Item("U_OCRC2").Value & ""
                    oJounalEntry.Lines.CostingCode3 = oRecordset.Fields.Item("U_OCRC3").Value & ""
                    oJounalEntry.Lines.CostingCode4 = oRecordset.Fields.Item("U_OCRC4").Value & ""
                    oJounalEntry.Lines.CostingCode5 = oRecordset.Fields.Item("U_OCRC5").Value & ""
                    oJounalEntry.Lines.Add()





                    Dim Result As Integer = oJounalEntry.Add()
                    If Result <> 0 Then
                        Maanger_AdjustmentJounralEntry_FromBranch(FormUID, pVal, BubbleEvent, DocEntry)

                        'If Result = -5002 Then

                        '    Maanger_AdjustmentJounralEntry_FromBranch(FormUID, pVal, BubbleEvent, DocEntry)
                        'Else
                        '    __oApplication.StatusBar.SetText("Error:Adjustment JE From Branch Not Generated - " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        'End If
                        ' __oApplication.StatusBar.SetText("Error:Adjustment JE From Branch Not Generated - " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                    Else

                        Dim SqlQuery As New StringBuilder

                        SqlQuery.Append("Update ""@SAP_OIT"" Set ""U_AJE1""= (Select ""TransId"" From OJDT Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='AJEFB'  )")
                        SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")


                        Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRset1.DoQuery(SqlQuery.ToString)

                        __oApplication.StatusBar.SetText("Adjustment JE From Branch  Generated ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                    End If




                End If





            Catch ex As Exception
                If ex.Message = "The logged-on user does not have permission to use this object" Then

                    Maanger_AdjustmentJounralEntry_FromBranch(FormUID, pVal, BubbleEvent, DocEntry)
                Else
                    __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
                End If


            End Try
        End Sub

        Private Sub AdjustmentJounralEntry_ToBranch(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)


                Dim oRecordset As SAPbobsCOM.Recordset = Nothing
                Dim Sql As String = ""
                Sql = "CALL SAP_GETDATA_Adjustment_JE2_ToBranch ('" + DocEntry + "')"
                oRecordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRecordset.DoQuery(Sql)
                If oRecordset.RecordCount > 0 Then


                    Dim oJounalEntry As SAPbobsCOM.JournalEntries = Nothing
                    oJounalEntry = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
                    oJounalEntry.ReferenceDate = oRecordset.Fields.Item("Date").Value
                    oJounalEntry.TaxDate = oRecordset.Fields.Item("Date").Value
                    oJounalEntry.DueDate = oRecordset.Fields.Item("Date").Value
                    oJounalEntry.UserFields.Fields.Item("U_ITDocE").Value = oRecordset.Fields.Item("DocEntry").Value & ""
                    oJounalEntry.UserFields.Fields.Item("U_ITDocN").Value = oRecordset.Fields.Item("DocNum").Value & ""
                    oJounalEntry.UserFields.Fields.Item("U_Flag").Value = "AJETB"



                    oJounalEntry.Memo = "ADJ TB Based On Inventory Trasnsfer  '" + oRecordset.Fields.Item("DocNum").Value.ToString & "" + "'  "



                    oJounalEntry.Lines.BPLID = oRecordset.Fields.Item("Branch").Value
                    oJounalEntry.Lines.AccountCode = oRecordset.Fields.Item("DebitAccount").Value
                    oJounalEntry.Lines.Debit = oRecordset.Fields.Item("Amt").Value & ""
                    oJounalEntry.Lines.Credit = 0
                    oJounalEntry.Lines.DueDate = oRecordset.Fields.Item("Date").Value
                    oJounalEntry.Lines.TaxDate = oRecordset.Fields.Item("Date").Value
                    oJounalEntry.Lines.ReferenceDate1 = oRecordset.Fields.Item("Date").Value

                    oJounalEntry.Lines.CostingCode = oRecordset.Fields.Item("U_OCRC").Value & ""
                    oJounalEntry.Lines.CostingCode2 = oRecordset.Fields.Item("U_OCRC2").Value & ""
                    oJounalEntry.Lines.CostingCode3 = oRecordset.Fields.Item("U_OCRC3").Value & ""
                    oJounalEntry.Lines.CostingCode4 = oRecordset.Fields.Item("U_OCRC4").Value & ""
                    oJounalEntry.Lines.CostingCode5 = oRecordset.Fields.Item("U_OCRC5").Value & ""

                    oJounalEntry.Lines.Add()



                    oJounalEntry.Lines.BPLID = oRecordset.Fields.Item("Branch").Value
                    oJounalEntry.Lines.AccountCode = oRecordset.Fields.Item("CreditAccount").Value
                    oJounalEntry.Lines.Debit = 0
                    oJounalEntry.Lines.Credit = oRecordset.Fields.Item("Amt").Value & ""
                    oJounalEntry.Lines.DueDate = oRecordset.Fields.Item("Date").Value
                    oJounalEntry.Lines.TaxDate = oRecordset.Fields.Item("Date").Value
                    oJounalEntry.Lines.ReferenceDate1 = oRecordset.Fields.Item("Date").Value

                    oJounalEntry.Lines.CostingCode = oRecordset.Fields.Item("U_OCRC").Value & ""
                    oJounalEntry.Lines.CostingCode2 = oRecordset.Fields.Item("U_OCRC2").Value & ""
                    oJounalEntry.Lines.CostingCode3 = oRecordset.Fields.Item("U_OCRC3").Value & ""
                    oJounalEntry.Lines.CostingCode4 = oRecordset.Fields.Item("U_OCRC4").Value & ""
                    oJounalEntry.Lines.CostingCode5 = oRecordset.Fields.Item("U_OCRC5").Value & ""
                    oJounalEntry.Lines.Add()





                    Dim Result As Integer = oJounalEntry.Add()
                    If Result <> 0 Then
                        Manager_AdjustmentJounralEntry_ToBranch(FormUID, pVal, BubbleEvent, DocEntry)
                        'If Result = -5002 Then

                        '    Manager_AdjustmentJounralEntry_ToBranch(FormUID, pVal, BubbleEvent, DocEntry)
                        'Else
                        '    __oApplication.StatusBar.SetText("Error:Adjustment JE To Branch Not Generated - " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        'End If
                        '__oApplication.StatusBar.SetText("Error:Adjustment JE To Branch Not Generated - " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                    Else

                        Dim SqlQuery As New StringBuilder

                        SqlQuery.Append("Update ""@SAP_OIT"" Set ""U_AJE2""= (Select ""TransId"" From OJDT Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='AJETB'  )")
                        SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")


                        Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRset1.DoQuery(SqlQuery.ToString)

                        __oApplication.StatusBar.SetText("Adjustment JE To Branch  Generated ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                    End If




                End If





            Catch ex As Exception
                '__oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
                If ex.Message = "The logged-on user does not have permission to use this object" Then

                    Manager_AdjustmentJounralEntry_ToBranch(FormUID, pVal, BubbleEvent, DocEntry)
                Else
                    __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
                End If
            End Try
        End Sub

        Private Sub FormDatLoadEvent(ByVal FormUID As String, ByRef BubbleEvent As Boolean)
            Try


                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(__oApplication.Forms.ActiveForm.TypeEx, __oApplication.Forms.ActiveForm.TypeCount)
                oForm.Freeze(True)



                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("Item_9").Specific

                Try

                    Try
                        ocombo = oForm.Items.Item("cbIIT").Specific
                        If ocombo.Selected.Value = "DI" Then
                            Dim SqlUpdate As String = "Update ""@SAP_IT4""  Set ""U_TTL""=0  where ""DocEntry""= '" + oForm.Items.Item("Item_15").Specific.String + "' "
                            Dim oRsUpdate As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                            oRsUpdate.DoQuery(SqlUpdate)
                        End If
                    Catch ex As Exception

                    End Try



                    Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_IT4")
                    Dim SqlQuery As String = "select * from ""@SAP_IT4"" where ""DocEntry""= '" + oForm.Items.Item("Item_15").Specific.String + "' and IFNULL(""U_ItemCode"",'')<>''"

                    m1.Clear()
                    dbsrc.Clear()
                    Dim oRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    oRs.DoQuery(SqlQuery)
                    If oRs.RecordCount > 0 Then
                        For iRow As Integer = 0 To oRs.RecordCount - 1

                            dbsrc.Offset = dbsrc.Size - 1
                            m1.AddRow(1, m1.VisualRowCount)
                            m1.GetLineData(m1.VisualRowCount)




                            dbsrc.SetValue("DocEntry", dbsrc.Offset, oRs.Fields.Item("DocEntry").Value & "")
                            dbsrc.SetValue("LineId", dbsrc.Offset, oRs.Fields.Item("LineId").Value & "")
                            dbsrc.SetValue("VisOrder", dbsrc.Offset, oRs.Fields.Item("VisOrder").Value & "")
                            dbsrc.SetValue("Object", dbsrc.Offset, oRs.Fields.Item("Object").Value & "")
                            dbsrc.SetValue("LogInst", dbsrc.Offset, oRs.Fields.Item("LogInst").Value & "")
                            dbsrc.SetValue("U_ItemCode", dbsrc.Offset, oRs.Fields.Item("U_ItemCode").Value & "")
                            dbsrc.SetValue("U_Chamber", dbsrc.Offset, oRs.Fields.Item("U_Chamber").Value & "")
                            dbsrc.SetValue("U_ChemLos", dbsrc.Offset, oRs.Fields.Item("U_ChemLos").Value & "")
                            dbsrc.SetValue("U_TemLoss", dbsrc.Offset, oRs.Fields.Item("U_TemLoss").Value & "")
                            dbsrc.SetValue("U_ClLoss", dbsrc.Offset, oRs.Fields.Item("U_ClLoss").Value & "")
                            dbsrc.SetValue("U_TTL", dbsrc.Offset, oRs.Fields.Item("U_TTL").Value & "")




                            m1.SetLineData(m1.VisualRowCount)
                            m1.FlushToDataSource()
                            oRs.MoveNext()
                        Next
                    End If
                    Marshal.ReleaseComObject(oRs)




                Catch ex As Exception

                End Try

                Try
                    If oForm.Items.Item("Item_79").Specific.value.ToString <> "" Then
                        oForm.Items.Item("Item_38").Enabled = False
                        oForm.Items.Item("Item_3").Enabled = False
                        oForm.Items.Item("Item_76").Enabled = True
                    ElseIf oForm.Items.Item("Item_10").Specific.value.ToString <> "" Then
                        oForm.Items.Item("Item_76").Enabled = False
                        oForm.Items.Item("Item_38").Enabled = True
                        oForm.Items.Item("Item_3").Enabled = True
                    End If


                    Dim QCApproval As Integer = GetValue("Select Count(""Code"") As ""Count"" From ""@SAP_OURC"" Where ""U_TrxType""='QC' And ""U_User""='" + __bobCompany.UserSignature.ToString + "' ", "Count")
                    If QCApproval > 0 Then
                        oForm.Items.Item("Item_99").Enabled = True
                    Else
                        oForm.Items.Item("Item_99").Enabled = False

                    End If

                Catch ex As Exception

                End Try






                Try
                    ocombo = oForm.Items.Item("Item_99").Specific
                    If ocombo.Selected.Value = "H" Then
                        oForm.Items.Item("btnPost").Enabled = False
                    Else
                        oForm.Items.Item("btnPost").Enabled = True
                    End If

                Catch ex As Exception

                End Try
                Try
                    If oForm.Items.Item("Item_38").Specific.value = "" Then
                        oForm.Items.Item("Item_22").Enabled = True
                    Else
                        oForm.Items.Item("Item_22").Enabled = False
                    End If
                Catch ex As Exception

                End Try

                Try
                    ocombo = oForm.Items.Item("Item_99").Specific
                    If ocombo.Selected.Value = "N" Then
                        oForm.Items.Item("btnPost").Enabled = False

                    Else
                        oForm.Items.Item("btnPost").Enabled = True
                    End If
                Catch ex As Exception

                End Try


                oForm.Update()

                oForm.Freeze(False)
            Catch ex As Exception
                '__oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
                oForm.Freeze(False)
            End Try
        End Sub



        '''Manager Id through transcation creation 

        Private Sub Manager_PurchaseOrder_Creation(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)


                Dim Server As String
                Dim CompanyDB As String
                Dim UserName As String
                Dim Password As String
                Dim DbUserName As String
                Dim DbPassword As String
                Dim LicenseServer As String

                Dim USSQL = "SELECT ""SERVER"",""CompanyDB"",""US"",""PD"", ""DbUserName"", ""DbPassword"" , ""LicenseServer"" FROM ""USINV"""
                Dim oRsUS As SAPbobsCOM.Recordset = Nothing
                oRsUS = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRsUS.DoQuery(USSQL)
                If oRsUS.RecordCount > 0 Then
                    Server = oRsUS.Fields.Item("SERVER").Value.ToString()
                    CompanyDB = oRsUS.Fields.Item("CompanyDB").Value.ToString()
                    UserName = oRsUS.Fields.Item("US").Value.ToString()
                    Password = oRsUS.Fields.Item("PD").Value.ToString()
                    DbUserName = oRsUS.Fields.Item("DbUserName").Value.ToString()
                    DbPassword = oRsUS.Fields.Item("DbPassword").Value.ToString()
                    LicenseServer = oRsUS.Fields.Item("LicenseServer").Value.ToString()
                End If




                Dim oCompany As SAPbobsCOM.Company = New SAPbobsCOM.Company()

                oCompany.Server = Server
                oCompany.UseTrusted = False
                oCompany.CompanyDB = CompanyDB
                oCompany.UserName = UserName
                oCompany.Password = Password
                oCompany.DbUserName = DbUserName
                oCompany.DbPassword = DbPassword
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB
                oCompany.LicenseServer = LicenseServer

                Dim lRetCode As Integer = oCompany.Connect()
                If lRetCode = 0 Then
                    Dim oRs As SAPbobsCOM.Recordset = Nothing
                    Dim oRsVendor As SAPbobsCOM.Recordset = Nothing
                    Dim Sql As String = ""





                    ''----Fetch the Reocrd for Purchase Order Creation----
                    Sql = "CALL SAP_GET_Purchase_Orde_Data ('" + DocEntry + "')"
                    oRs = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    oRs.DoQuery(Sql)
                    If oRs.RecordCount > 0 Then


                        Dim oPurchaseOrder As SAPbobsCOM.Documents = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders)

                        '---This is for Header Details of Purchase Order----

                        oPurchaseOrder.CardCode = oRs.Fields.Item("U_CardCode").Value.ToString()
                        oPurchaseOrder.DocDate = oRs.Fields.Item("U_PDate").Value.ToString()
                        oPurchaseOrder.DocDueDate = oRs.Fields.Item("U_PDate").Value.ToString()
                        oPurchaseOrder.BPL_IDAssignedToInvoice = oRs.Fields.Item("Branch").Value.ToString()
                        oPurchaseOrder.UserFields.Fields.Item("U_ITDocE").Value = oRs.Fields.Item("DocEntry").Value & ""
                        oPurchaseOrder.UserFields.Fields.Item("U_ITDocN").Value = oRs.Fields.Item("DocNum").Value & ""
                        oPurchaseOrder.UserFields.Fields.Item("U_Flag").Value = "NSSPO" & ""




                        ''---This is for Purchase Order Line---
                        Dim oPurchaseOrder_Line As SAPbobsCOM.Document_Lines = oPurchaseOrder.Lines
                        While oRs.EoF = False

                            oPurchaseOrder_Line.ItemCode = oRs.Fields.Item("ItemCode").Value
                            oPurchaseOrder_Line.Quantity = oRs.Fields.Item("U_Qty").Value

                            oPurchaseOrder_Line.WarehouseCode = oRs.Fields.Item("WhsCode").Value
                            oPurchaseOrder_Line.Price = oRs.Fields.Item("Price").Value

                            oPurchaseOrder_Line.CostingCode = oRs.Fields.Item("U_OCRC").Value & ""
                            oPurchaseOrder_Line.CostingCode2 = oRs.Fields.Item("U_OCRC2").Value & ""
                            oPurchaseOrder_Line.CostingCode3 = oRs.Fields.Item("U_OCRC3").Value & ""
                            oPurchaseOrder_Line.CostingCode4 = oRs.Fields.Item("U_OCRC4").Value & ""
                            oPurchaseOrder_Line.CostingCode5 = oRs.Fields.Item("U_OCRC5").Value & ""
                            oPurchaseOrder_Line.UserFields.Fields.Item("U_Chamber").Value = oRs.Fields.Item("U_Chamber").Value & ""
                            oPurchaseOrder_Line.AgreementNo = oRs.Fields.Item("AbsID").Value & ""

                            oPurchaseOrder_Line.Add()
                            oRs.MoveNext()
                        End While

                        Dim Result As Integer = oPurchaseOrder.Add()
                        If Result <> 0 Then
                            __oApplication.StatusBar.SetText("Error: In Generating Purchase Order - " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Else

                            Dim SqlQuery As New StringBuilder

                            SqlQuery.Append("Update ""@SAP_OIT"" Set ""U_PODN""= (Select ""DocNum"" From OPOR Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='NSSPO'  ),")
                            SqlQuery.Append(" ""U_PODE""= (Select ""DocEntry"" From OPOR Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='NSSPO' )")
                            SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")


                            Dim oRset1 As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                            oRset1.DoQuery(SqlQuery.ToString)
                            __oApplication.StatusBar.SetText("Purchase Order Generated", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        End If
                    End If

                End If
                oCompany.Disconnect()

            Catch ex As Exception

            End Try
        End Sub

        Private Sub Maanger_CreateGoodsIssue(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)


                Dim Server As String
                Dim CompanyDB As String
                Dim UserName As String
                Dim Password As String
                Dim DbUserName As String
                Dim DbPassword As String
                Dim LicenseServer As String

                Dim USSQL = "SELECT ""SERVER"",""CompanyDB"",""US"",""PD"", ""DbUserName"", ""DbPassword"" , ""LicenseServer"" FROM ""USINV"""
                Dim oRsUS As SAPbobsCOM.Recordset = Nothing
                oRsUS = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRsUS.DoQuery(USSQL)
                If oRsUS.RecordCount > 0 Then
                    Server = oRsUS.Fields.Item("SERVER").Value.ToString()
                    CompanyDB = oRsUS.Fields.Item("CompanyDB").Value.ToString()
                    UserName = oRsUS.Fields.Item("US").Value.ToString()
                    Password = oRsUS.Fields.Item("PD").Value.ToString()
                    DbUserName = oRsUS.Fields.Item("DbUserName").Value.ToString()
                    DbPassword = oRsUS.Fields.Item("DbPassword").Value.ToString()
                    LicenseServer = oRsUS.Fields.Item("LicenseServer").Value.ToString()
                End If

                Dim oCompany As SAPbobsCOM.Company = New SAPbobsCOM.Company()

                oCompany.Server = Server
                oCompany.UseTrusted = False
                oCompany.CompanyDB = CompanyDB
                oCompany.UserName = UserName
                oCompany.Password = Password
                oCompany.DbUserName = DbUserName
                oCompany.DbPassword = DbPassword
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB
                oCompany.LicenseServer = LicenseServer

                Dim lRetCode As Integer = oCompany.Connect()
                If lRetCode = 0 Then





                    Dim errItemCodes As String = ""
                    Dim errMachineNos As String = ""

                    Dim LineID As String = ""
                    Dim errRowFlag As Boolean = False

                    Dim oGoodsIssue As SAPbobsCOM.Documents = Nothing
                    Dim oRecordset As SAPbobsCOM.Recordset = Nothing
                    Dim oRs1 As SAPbobsCOM.Recordset = Nothing

                    Dim ErrorCode As Integer
                    Dim ErrMessage As String
                    Dim iCount As Integer = 1



                    Dim Sql As String = ""
                    Sql = "CALL SAP_GET_GoodIssue_Data('" + DocEntry + "')"

                    oRecordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    oRecordset.DoQuery(Sql)

                    If oRecordset.RecordCount > 0 Then

                        oGoodsIssue = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit)

                        oGoodsIssue.DocDate = oRecordset.Fields.Item("U_PDate").Value & ""
                        oGoodsIssue.TaxDate = oRecordset.Fields.Item("U_PDate").Value & ""
                        oGoodsIssue.DocDueDate = oRecordset.Fields.Item("U_PDate").Value & ""
                        'oGoodsIssue.Reference1 = oRecordset.Fields.Item("U_RefNo").Value & ""
                        oGoodsIssue.Comments = oRecordset.Fields.Item("U_Remark").Value & ""
                        oGoodsIssue.BPL_IDAssignedToInvoice = CInt(oRecordset.Fields.Item("Branch").Value)
                        oGoodsIssue.UserFields.Fields.Item("U_ITDocE").Value = oRecordset.Fields.Item("DocEntry").Value & ""
                        oGoodsIssue.UserFields.Fields.Item("U_ITDocN").Value = oRecordset.Fields.Item("DocNum").Value & ""
                        oGoodsIssue.UserFields.Fields.Item("U_Flag").Value = "STITGI" & ""
                        oGoodsIssue.Series = oRecordset.Fields.Item("Series").Value & ""





                        'Adding Row level Data
                        While oRecordset.EoF = False

                            oGoodsIssue.Lines.ItemCode = oRecordset.Fields.Item("U_ItemCode").Value & ""
                            oGoodsIssue.Lines.Quantity = oRecordset.Fields.Item("U_Qty").Value
                            oGoodsIssue.Lines.WarehouseCode = oRecordset.Fields.Item("U_FW").Value & ""

                            oGoodsIssue.Lines.CostingCode = oRecordset.Fields.Item("U_OCRC").Value & ""
                            oGoodsIssue.Lines.CostingCode2 = oRecordset.Fields.Item("U_OCRC2").Value & ""
                            oGoodsIssue.Lines.CostingCode3 = oRecordset.Fields.Item("U_OCRC3").Value & ""
                            oGoodsIssue.Lines.CostingCode4 = oRecordset.Fields.Item("U_OCRC4").Value & ""
                            oGoodsIssue.Lines.CostingCode5 = oRecordset.Fields.Item("U_OCRC5").Value & ""
                            oGoodsIssue.Lines.UserFields.Fields.Item("U_Chamber").Value = oRecordset.Fields.Item("U_Chamber").Value & ""


                            oGoodsIssue.Lines.UserFields.Fields.Item("U_Temp").Value = oRecordset.Fields.Item("U_Temp").Value & ""
                            oGoodsIssue.Lines.UserFields.Fields.Item("U_Density").Value = oRecordset.Fields.Item("U_Density").Value & ""
                            oGoodsIssue.Lines.UserFields.Fields.Item("U_Dip").Value = oRecordset.Fields.Item("U_Dip").Value & ""
                            oGoodsIssue.Lines.AccountCode = oRecordset.Fields.Item("AccountCode").Value & ""


                            oGoodsIssue.Lines.Add()
                            iCount = iCount + 1
                            oRecordset.MoveNext()

                        End While

                        Dim Result As Integer = oGoodsIssue.Add()

                        If Result <> 0 Then
                            __oApplication.StatusBar.SetText("Error: Good Issue Not Generated - " + oCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Else


                            Dim SqlQuery As New StringBuilder

                            SqlQuery.Append("Update ""@SAP_OIT"" Set ""U_GIDN""= (Select ""DocNum"" From OIGE Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='STITGI'  ),")
                            SqlQuery.Append(" ""U_GIDE""= (Select ""DocEntry"" From OIGE Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='STITGI'  )")
                            SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")


                            Dim oRset1 As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                            oRset1.DoQuery(SqlQuery.ToString)
                            __oApplication.StatusBar.SetText("Good Issue Generated", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)


                        End If

                    End If

                    Marshal.ReleaseComObject(oRecordset)
                    Try
                        Marshal.ReleaseComObject(oGoodsIssue)
                    Catch ex As Exception

                    End Try
                    oCompany.Disconnect()
                End If

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Maanger_CreateGoodsReceipt(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                Dim Server As String
                Dim CompanyDB As String
                Dim UserName As String
                Dim Password As String
                Dim DbUserName As String
                Dim DbPassword As String
                Dim LicenseServer As String

                Dim USSQL = "SELECT ""SERVER"",""CompanyDB"",""US"",""PD"", ""DbUserName"", ""DbPassword"" , ""LicenseServer"" FROM ""USINV"""
                Dim oRsUS As SAPbobsCOM.Recordset = Nothing
                oRsUS = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRsUS.DoQuery(USSQL)
                If oRsUS.RecordCount > 0 Then
                    Server = oRsUS.Fields.Item("SERVER").Value.ToString()
                    CompanyDB = oRsUS.Fields.Item("CompanyDB").Value.ToString()
                    UserName = oRsUS.Fields.Item("US").Value.ToString()
                    Password = oRsUS.Fields.Item("PD").Value.ToString()
                    DbUserName = oRsUS.Fields.Item("DbUserName").Value.ToString()
                    DbPassword = oRsUS.Fields.Item("DbPassword").Value.ToString()
                    LicenseServer = oRsUS.Fields.Item("LicenseServer").Value.ToString()
                End If

                Dim oCompany As SAPbobsCOM.Company = New SAPbobsCOM.Company()

                oCompany.Server = Server
                oCompany.UseTrusted = False
                oCompany.CompanyDB = CompanyDB
                oCompany.UserName = UserName
                oCompany.Password = Password
                oCompany.DbUserName = DbUserName
                oCompany.DbPassword = DbPassword
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB
                oCompany.LicenseServer = LicenseServer

                Dim lRetCode As Integer = oCompany.Connect()
                If lRetCode = 0 Then



                    Dim errItemCodes As String = ""
                    Dim errMachineNos As String = ""

                    Dim LineID As String = ""
                    Dim errRowFlag As Boolean = False

                    Dim oGoodsReceipt As SAPbobsCOM.Documents = Nothing
                    Dim oRecordset As SAPbobsCOM.Recordset = Nothing
                    Dim oRs1 As SAPbobsCOM.Recordset = Nothing

                    Dim ErrorCode As Integer
                    Dim ErrMessage As String
                    Dim iCount As Integer = 1



                    Dim Sql As String = ""
                    Sql = "CALL SAP_GET_GoodReceipt_Data('" + DocEntry + "')"

                    oRecordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    oRecordset.DoQuery(Sql)

                    If oRecordset.RecordCount > 0 Then

                        oGoodsReceipt = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry)

                        oGoodsReceipt.DocDate = oRecordset.Fields.Item("U_PDate").Value & ""
                        oGoodsReceipt.TaxDate = oRecordset.Fields.Item("U_PDate").Value & ""
                        oGoodsReceipt.DocDueDate = oRecordset.Fields.Item("U_PDate").Value & ""

                        oGoodsReceipt.Comments = oRecordset.Fields.Item("U_Remark").Value & ""
                        oGoodsReceipt.BPL_IDAssignedToInvoice = CInt(oRecordset.Fields.Item("Branch").Value)
                        oGoodsReceipt.UserFields.Fields.Item("U_ITDocE").Value = oRecordset.Fields.Item("DocEntry").Value & ""
                        oGoodsReceipt.UserFields.Fields.Item("U_ITDocN").Value = oRecordset.Fields.Item("DocNum").Value & ""
                        oGoodsReceipt.UserFields.Fields.Item("U_Flag").Value = "STITGR" & ""
                        oGoodsReceipt.Series = oRecordset.Fields.Item("Series").Value & ""





                        'Adding Row level Data
                        While oRecordset.EoF = False

                            oGoodsReceipt.Lines.ItemCode = oRecordset.Fields.Item("ItemCode").Value & ""
                            oGoodsReceipt.Lines.Quantity = oRecordset.Fields.Item("Quantity").Value
                            oGoodsReceipt.Lines.WarehouseCode = oRecordset.Fields.Item("U_TW").Value & ""
                            'oGoodsReceipt.Lines.Price = oRecordset.Fields.Item("Price").Value & ""
                            oGoodsReceipt.Lines.LineTotal = oRecordset.Fields.Item("LineTotal").Value & ""

                            oGoodsReceipt.Lines.CostingCode = oRecordset.Fields.Item("OcrCode").Value & ""
                            oGoodsReceipt.Lines.CostingCode2 = oRecordset.Fields.Item("OcrCode2").Value & ""
                            oGoodsReceipt.Lines.CostingCode3 = oRecordset.Fields.Item("OcrCode3").Value & ""
                            oGoodsReceipt.Lines.CostingCode4 = oRecordset.Fields.Item("OcrCode4").Value & ""
                            oGoodsReceipt.Lines.CostingCode5 = oRecordset.Fields.Item("OcrCode5").Value & ""
                            oGoodsReceipt.Lines.UserFields.Fields.Item("U_Chamber").Value = oRecordset.Fields.Item("U_Chamber").Value & ""
                            oGoodsReceipt.Lines.UserFields.Fields.Item("U_Temp").Value = oRecordset.Fields.Item("U_Temp").Value & ""
                            oGoodsReceipt.Lines.UserFields.Fields.Item("U_Density").Value = oRecordset.Fields.Item("U_Density").Value & ""
                            oGoodsReceipt.Lines.UserFields.Fields.Item("U_Dip").Value = oRecordset.Fields.Item("U_Dip").Value & ""

                            oGoodsReceipt.Lines.AccountCode = oRecordset.Fields.Item("AccountCode").Value & ""


                            oGoodsReceipt.Lines.Add()
                            iCount = iCount + 1
                            oRecordset.MoveNext()

                        End While

                        Dim Result As Integer = oGoodsReceipt.Add()

                        If Result <> 0 Then

                            __oApplication.StatusBar.SetText("Error: Good Receipt Not Generated - " + oCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Else


                            Dim SqlQuery As New StringBuilder

                            SqlQuery.Append("Update ""@SAP_OIT"" Set ""U_GRDN""= (Select ""DocNum"" From OIGN Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='STITGR' ),")
                            SqlQuery.Append(" ""U_GRDE""= (Select ""DocEntry"" From OIGN Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='STITGR' )")
                            SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")


                            Dim oRset1 As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                            oRset1.DoQuery(SqlQuery.ToString)
                            __oApplication.StatusBar.SetText("Good Receipt Generated", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)


                        End If

                    End If

                    Marshal.ReleaseComObject(oRecordset)
                    Try
                        Marshal.ReleaseComObject(oGoodsReceipt)
                    Catch ex As Exception
                        BubbleEvent = False
                    End Try
                    oCompany.Disconnect()
                End If

            Catch ex As Exception
                BubbleEvent = False
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Manager_Create_TransportationLoss_GoodsIssue(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String, ByVal Type As String)
            Try
                Dim errItemCodes As String = ""
                Dim errMachineNos As String = ""


                Dim Server As String
                Dim CompanyDB As String
                Dim UserName As String
                Dim Password As String
                Dim DbUserName As String
                Dim DbPassword As String
                Dim LicenseServer As String

                Dim USSQL = "SELECT ""SERVER"",""CompanyDB"",""US"",""PD"", ""DbUserName"", ""DbPassword"" , ""LicenseServer"" FROM ""USINV"""
                Dim oRsUS As SAPbobsCOM.Recordset = Nothing
                oRsUS = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRsUS.DoQuery(USSQL)
                If oRsUS.RecordCount > 0 Then
                    Server = oRsUS.Fields.Item("SERVER").Value.ToString()
                    CompanyDB = oRsUS.Fields.Item("CompanyDB").Value.ToString()
                    UserName = oRsUS.Fields.Item("US").Value.ToString()
                    Password = oRsUS.Fields.Item("PD").Value.ToString()
                    DbUserName = oRsUS.Fields.Item("DbUserName").Value.ToString()
                    DbPassword = oRsUS.Fields.Item("DbPassword").Value.ToString()
                    LicenseServer = oRsUS.Fields.Item("LicenseServer").Value.ToString()
                End If

                Dim oCompany As SAPbobsCOM.Company = New SAPbobsCOM.Company()

                oCompany.Server = Server
                oCompany.UseTrusted = False
                oCompany.CompanyDB = CompanyDB
                oCompany.UserName = UserName
                oCompany.Password = Password
                oCompany.DbUserName = DbUserName
                oCompany.DbPassword = DbPassword
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB
                oCompany.LicenseServer = LicenseServer

                Dim lRetCode As Integer = oCompany.Connect()
                If lRetCode = 0 Then



                    Dim LineID As String = ""
                    Dim errRowFlag As Boolean = False

                    Dim oGoodsIssue As SAPbobsCOM.Documents = Nothing
                    Dim oRecordset As SAPbobsCOM.Recordset = Nothing
                    Dim oRs1 As SAPbobsCOM.Recordset = Nothing

                    Dim ErrorCode As Integer
                    Dim ErrMessage As String
                    Dim iCount As Integer = 1

                    Dim Sql As String = ""
                    If Type = "ST" Then
                        Sql = "CALL SAP_GET_StockTransfer_GoodIssue_TempLoss_Data('" + DocEntry + "')"
                    ElseIf Type = "PF" Then
                        Sql = "CALL SAP_GET_GoodIssue_TempLoss_Data('" + DocEntry + "')"
                    End If





                    oRecordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    oRecordset.DoQuery(Sql)

                    If oRecordset.RecordCount > 0 Then

                        oGoodsIssue = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit)

                        oGoodsIssue.DocDate = oRecordset.Fields.Item("U_PDate").Value & ""
                        oGoodsIssue.TaxDate = oRecordset.Fields.Item("U_PDate").Value & ""
                        oGoodsIssue.DocDueDate = oRecordset.Fields.Item("U_PDate").Value & ""
                        'oGoodsIssue.Reference1 = oRecordset.Fields.Item("U_RefNo").Value & ""
                        oGoodsIssue.Comments = "Based on Inventory Transfer " + oRecordset.Fields.Item("DocNum").Value.ToString + " ,Tolerance Loss" & ""
                        oGoodsIssue.BPL_IDAssignedToInvoice = CInt(oRecordset.Fields.Item("Branch").Value)
                        oGoodsIssue.UserFields.Fields.Item("U_ITDocE").Value = oRecordset.Fields.Item("DocEntry").Value & ""
                        oGoodsIssue.UserFields.Fields.Item("U_ITDocN").Value = oRecordset.Fields.Item("DocNum").Value & ""
                        oGoodsIssue.UserFields.Fields.Item("U_Flag").Value = "TL" & ""
                        oGoodsIssue.Series = oRecordset.Fields.Item("Series").Value & ""





                        'Adding Row level Data
                        While oRecordset.EoF = False

                            oGoodsIssue.Lines.ItemCode = oRecordset.Fields.Item("U_ItemCode").Value & ""
                            oGoodsIssue.Lines.Quantity = oRecordset.Fields.Item("U_TemLoss").Value


                            If Type = "ST" Then
                                oGoodsIssue.Lines.WarehouseCode = oRecordset.Fields.Item("U_TW").Value & ""
                            ElseIf Type = "PF" Then
                                oGoodsIssue.Lines.WarehouseCode = oRecordset.Fields.Item("U_FW").Value & ""
                            End If


                            oGoodsIssue.Lines.UserFields.Fields.Item("U_Chamber").Value = oRecordset.Fields.Item("U_Chamber").Value & ""
                            oGoodsIssue.Lines.CostingCode = oRecordset.Fields.Item("U_OCRC").Value & ""
                            oGoodsIssue.Lines.CostingCode2 = oRecordset.Fields.Item("U_OCRC2").Value & ""
                            oGoodsIssue.Lines.CostingCode3 = oRecordset.Fields.Item("U_OCRC3").Value & ""
                            oGoodsIssue.Lines.CostingCode4 = oRecordset.Fields.Item("U_OCRC4").Value & ""
                            oGoodsIssue.Lines.CostingCode5 = oRecordset.Fields.Item("U_OCRC5").Value & ""

                            oGoodsIssue.Lines.UserFields.Fields.Item("U_Temp").Value = oRecordset.Fields.Item("U_Temp").Value & ""
                            oGoodsIssue.Lines.UserFields.Fields.Item("U_Density").Value = oRecordset.Fields.Item("U_Density").Value & ""
                            oGoodsIssue.Lines.UserFields.Fields.Item("U_Dip").Value = oRecordset.Fields.Item("U_Dip").Value & ""


                            oGoodsIssue.Lines.Add()
                            iCount = iCount + 1
                            oRecordset.MoveNext()

                        End While

                        Dim Result As Integer = oGoodsIssue.Add()

                        If Result <> 0 Then

                            __oApplication.StatusBar.SetText("Error: Tolerance Loss Good Issue Not Generated - " + oCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Else


                            Dim SqlQuery As New StringBuilder

                            SqlQuery.Append("Update ""@SAP_OIT"" Set ""U_TGIDN""= (Select ""DocNum"" From OIGE Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='TL'  ),")
                            SqlQuery.Append(" ""U_TGIDE""= (Select ""DocEntry"" From OIGE Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='TL')")
                            SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")


                            Dim oRset1 As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                            oRset1.DoQuery(SqlQuery.ToString)
                            __oApplication.StatusBar.SetText(" Tolerance Loss Good Issue  Generated", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)


                        End If

                    End If

                    Marshal.ReleaseComObject(oRecordset)
                    Try
                        Marshal.ReleaseComObject(oGoodsIssue)
                    Catch ex As Exception

                    End Try
                    oCompany.Disconnect()
                End If


            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Manager_ARInvoice_Creation(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String, ByVal Type As String)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)


                Dim Server As String
                Dim CompanyDB As String
                Dim UserName As String
                Dim Password As String
                Dim DbUserName As String
                Dim DbPassword As String
                Dim LicenseServer As String

                Dim USSQL = "SELECT ""SERVER"",""CompanyDB"",""US"",""PD"", ""DbUserName"", ""DbPassword"" , ""LicenseServer"" FROM ""USINV"""
                Dim oRsUS As SAPbobsCOM.Recordset = Nothing
                oRsUS = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRsUS.DoQuery(USSQL)
                If oRsUS.RecordCount > 0 Then
                    Server = oRsUS.Fields.Item("SERVER").Value.ToString()
                    CompanyDB = oRsUS.Fields.Item("CompanyDB").Value.ToString()
                    UserName = oRsUS.Fields.Item("US").Value.ToString()
                    Password = oRsUS.Fields.Item("PD").Value.ToString()
                    DbUserName = oRsUS.Fields.Item("DbUserName").Value.ToString()
                    DbPassword = oRsUS.Fields.Item("DbPassword").Value.ToString()
                    LicenseServer = oRsUS.Fields.Item("LicenseServer").Value.ToString()
                End If

                Dim oCompany As SAPbobsCOM.Company = New SAPbobsCOM.Company()

                oCompany.Server = Server
                oCompany.UseTrusted = False
                oCompany.CompanyDB = CompanyDB
                oCompany.UserName = UserName
                oCompany.Password = Password
                oCompany.DbUserName = DbUserName
                oCompany.DbPassword = DbPassword
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB
                oCompany.LicenseServer = LicenseServer

                Dim lRetCode As Integer = oCompany.Connect()
                If lRetCode = 0 Then



                    Dim oRs As SAPbobsCOM.Recordset = Nothing
                    Dim oRsVendor As SAPbobsCOM.Recordset = Nothing
                    Dim Sql As String = ""


                    If Type = "ST" Then
                        Sql = "CALL SAP_GET_StockTransfer_AR_Invoice_Data ('" + DocEntry + "')"
                    ElseIf Type = "PF" Then
                        Sql = "CALL SAP_GET_AR_Invoice_Data ('" + DocEntry + "')"
                    End If

                    ''----Fetch the Reocrd for Purchase Order Creation----

                    oRs = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    oRs.DoQuery(Sql)
                    If oRs.RecordCount > 0 Then
                        Dim oPurchaseInvoice As SAPbobsCOM.Documents = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices)

                        oPurchaseInvoice.CardCode = oRs.Fields.Item("U_TCode").Value.ToString()
                        oPurchaseInvoice.DocDate = oRs.Fields.Item("U_PDate").Value.ToString()
                        oPurchaseInvoice.DocDueDate = oRs.Fields.Item("U_PDate").Value.ToString()
                        oPurchaseInvoice.BPL_IDAssignedToInvoice = oRs.Fields.Item("Branch").Value.ToString()
                        oPurchaseInvoice.UserFields.Fields.Item("U_ITDocE").Value = oRs.Fields.Item("DocEntry").Value & ""
                        oPurchaseInvoice.UserFields.Fields.Item("U_ITDocN").Value = oRs.Fields.Item("DocNum").Value & ""



                        ''---This is for Purchase Order Line---
                        Dim oPurchaseInvoice_Line As SAPbobsCOM.Document_Lines = oPurchaseInvoice.Lines
                        While oRs.EoF = False

                            oPurchaseInvoice_Line.ItemCode = oRs.Fields.Item("U_ItemCode").Value
                            oPurchaseInvoice_Line.Quantity = oRs.Fields.Item("U_ClLoss").Value


                            If Type = "ST" Then
                                oPurchaseInvoice_Line.WarehouseCode = oRs.Fields.Item("U_TW").Value
                            ElseIf Type = "PF" Then
                                oPurchaseInvoice_Line.WarehouseCode = oRs.Fields.Item("U_FW").Value
                            End If

                            oPurchaseInvoice_Line.TaxCode = oRs.Fields.Item("TaxCode").Value
                            oPurchaseInvoice_Line.Price = oRs.Fields.Item("Price").Value

                            oPurchaseInvoice_Line.UserFields.Fields.Item("U_Chamber").Value = oRs.Fields.Item("U_Chamber").Value & ""
                            oPurchaseInvoice_Line.CostingCode = oRs.Fields.Item("U_OCRC").Value & ""
                            oPurchaseInvoice_Line.CostingCode2 = oRs.Fields.Item("U_OCRC2").Value & ""
                            oPurchaseInvoice_Line.CostingCode3 = oRs.Fields.Item("U_OCRC3").Value & ""
                            oPurchaseInvoice_Line.CostingCode4 = oRs.Fields.Item("U_OCRC4").Value & ""
                            oPurchaseInvoice_Line.CostingCode5 = oRs.Fields.Item("U_OCRC5").Value & ""
                            oPurchaseInvoice_Line.UserFields.Fields.Item("U_Temp").Value = oRs.Fields.Item("U_Temp").Value & ""
                            oPurchaseInvoice_Line.UserFields.Fields.Item("U_Density").Value = oRs.Fields.Item("U_Density").Value & ""
                            oPurchaseInvoice_Line.UserFields.Fields.Item("U_Dip").Value = oRs.Fields.Item("U_Dip").Value & ""


                            oPurchaseInvoice_Line.Add()
                            oRs.MoveNext()
                        End While

                        Dim Result As Integer = oPurchaseInvoice.Add()
                        If Result <> 0 Then
                            __oApplication.StatusBar.SetText("Error: In Generating AR Invoice - " + oCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Else

                            Dim SqlQuery As New StringBuilder

                            SqlQuery.Append("Update ""@SAP_OIT"" Set ""U_ARDN""= (Select TOP 1 ""DocNum"" From OINV Where ""U_ITDocE""='" + DocEntry + "'  ),")
                            SqlQuery.Append(" ""U_ARDE""= (Select TOP 1 ""DocEntry"" From OINV Where ""U_ITDocE""='" + DocEntry + "' )")
                            SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")


                            Dim oRset1 As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                            oRset1.DoQuery(SqlQuery.ToString)
                            __oApplication.StatusBar.SetText("AR Invoice Generated", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        End If
                    End If
                    oCompany.Disconnect()
                End If


            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub


        Private Sub Manager_Purchase_GoodReceiptPO_Creation(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim oRs As SAPbobsCOM.Recordset = Nothing
                Dim oRsVendor As SAPbobsCOM.Recordset = Nothing
                Dim Sql As String = ""


                Dim Server As String
                Dim CompanyDB As String
                Dim UserName As String
                Dim Password As String
                Dim DbUserName As String
                Dim DbPassword As String
                Dim LicenseServer As String

                Dim USSQL = "SELECT ""SERVER"",""CompanyDB"",""US"",""PD"", ""DbUserName"", ""DbPassword"" , ""LicenseServer"" FROM ""USINV"""
                Dim oRsUS As SAPbobsCOM.Recordset = Nothing
                oRsUS = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRsUS.DoQuery(USSQL)
                If oRsUS.RecordCount > 0 Then
                    Server = oRsUS.Fields.Item("SERVER").Value.ToString()
                    CompanyDB = oRsUS.Fields.Item("CompanyDB").Value.ToString()
                    UserName = oRsUS.Fields.Item("US").Value.ToString()
                    Password = oRsUS.Fields.Item("PD").Value.ToString()
                    DbUserName = oRsUS.Fields.Item("DbUserName").Value.ToString()
                    DbPassword = oRsUS.Fields.Item("DbPassword").Value.ToString()
                    LicenseServer = oRsUS.Fields.Item("LicenseServer").Value.ToString()
                End If
                Dim oCompany As SAPbobsCOM.Company = New SAPbobsCOM.Company()

                oCompany.Server = Server
                oCompany.UseTrusted = False
                oCompany.CompanyDB = CompanyDB
                oCompany.UserName = UserName
                oCompany.Password = Password
                oCompany.DbUserName = DbUserName
                oCompany.DbPassword = DbPassword
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB
                oCompany.LicenseServer = LicenseServer

                Dim lRetCode As Integer = oCompany.Connect()
                If lRetCode = 0 Then





                    ''----Fetch the Reocrd for Purchase Order Creation----
                    Sql = "CALL SAP_GET_Purchase_GRN_Data ('" + DocEntry + "')"
                    oRs = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    oRs.DoQuery(Sql)
                    If oRs.RecordCount > 0 Then
                        Dim oGoodReceiptPO As SAPbobsCOM.Documents = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes)

                        oGoodReceiptPO.CardCode = oRs.Fields.Item("CardCode").Value.ToString()
                        oGoodReceiptPO.DocDate = oRs.Fields.Item("DocDate").Value.ToString()
                        oGoodReceiptPO.DocDueDate = oRs.Fields.Item("DocDate").Value.ToString()
                        oGoodReceiptPO.BPL_IDAssignedToInvoice = oRs.Fields.Item("Branch").Value.ToString()
                        oGoodReceiptPO.UserFields.Fields.Item("U_ITDocE").Value = oRs.Fields.Item("DocEntry").Value & ""
                        oGoodReceiptPO.UserFields.Fields.Item("U_Flag").Value = "ITSGRN" & ""
                        oGoodReceiptPO.NumAtCard = oRs.Fields.Item("VenderRef").Value & ""





                        ''---This is for Purchase Order Line---
                        Dim oPurchaseOrder_Line As SAPbobsCOM.Document_Lines = oGoodReceiptPO.Lines
                        While oRs.EoF = False

                            oPurchaseOrder_Line.ItemCode = oRs.Fields.Item("ItemCode").Value
                            oPurchaseOrder_Line.Quantity = oRs.Fields.Item("Quantity").Value
                            oPurchaseOrder_Line.UnitPrice = oRs.Fields.Item("Price").Value

                            oPurchaseOrder_Line.CostingCode = oRs.Fields.Item("OcrCode").Value & ""
                            oPurchaseOrder_Line.CostingCode2 = oRs.Fields.Item("OcrCode2").Value & ""
                            oPurchaseOrder_Line.CostingCode3 = oRs.Fields.Item("OcrCode3").Value & ""
                            oPurchaseOrder_Line.CostingCode4 = oRs.Fields.Item("OcrCode4").Value & ""
                            oPurchaseOrder_Line.CostingCode5 = oRs.Fields.Item("OcrCode5").Value & ""
                            oPurchaseOrder_Line.UserFields.Fields.Item("U_Chamber").Value = oRs.Fields.Item("U_Chamber").Value & ""

                            oPurchaseOrder_Line.BaseEntry = oRs.Fields.Item("BaseEntry").Value
                            oPurchaseOrder_Line.BaseType = 22
                            oPurchaseOrder_Line.BaseLine = oRs.Fields.Item("LineNum").Value


                            oPurchaseOrder_Line.Add()
                            oRs.MoveNext()
                        End While

                        Dim Result As Integer = oGoodReceiptPO.Add()
                        If Result <> 0 Then
                            __oApplication.StatusBar.SetText("Error: In Generating Good Receipt PO- " + oCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Else

                            Dim SqlQuery As New StringBuilder

                            SqlQuery.Append("Update ""@SAP_OIT"" Set ""U_GRNDN""= (Select ""DocNum"" From OPDN Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='ITSGRN'  ),")
                            SqlQuery.Append(" ""U_GRNDE""= (Select ""DocEntry"" From OPDN Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='ITSGRN' )")
                            SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")


                            Dim oRset1 As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                            oRset1.DoQuery(SqlQuery.ToString)
                            __oApplication.StatusBar.SetText("Good Receipt PO Generated", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        End If
                    End If
                    oCompany.Disconnect()
                End If

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Manager_TannkTempLoss_CreateGoodsIssue(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String, ByVal Type As String)
            Try


                Dim Server As String
                Dim CompanyDB As String
                Dim UserName As String
                Dim Password As String
                Dim DbUserName As String
                Dim DbPassword As String
                Dim LicenseServer As String

                Dim USSQL = "SELECT ""SERVER"",""CompanyDB"",""US"",""PD"", ""DbUserName"", ""DbPassword"" , ""LicenseServer"" FROM ""USINV"""
                Dim oRsUS As SAPbobsCOM.Recordset = Nothing
                oRsUS = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRsUS.DoQuery(USSQL)
                If oRsUS.RecordCount > 0 Then
                    Server = oRsUS.Fields.Item("SERVER").Value.ToString()
                    CompanyDB = oRsUS.Fields.Item("CompanyDB").Value.ToString()
                    UserName = oRsUS.Fields.Item("US").Value.ToString()
                    Password = oRsUS.Fields.Item("PD").Value.ToString()
                    DbUserName = oRsUS.Fields.Item("DbUserName").Value.ToString()
                    DbPassword = oRsUS.Fields.Item("DbPassword").Value.ToString()
                    LicenseServer = oRsUS.Fields.Item("LicenseServer").Value.ToString()
                End If

                Dim oCompany As SAPbobsCOM.Company = New SAPbobsCOM.Company()

                oCompany.Server = Server
                oCompany.UseTrusted = False
                oCompany.CompanyDB = CompanyDB
                oCompany.UserName = UserName
                oCompany.Password = Password
                oCompany.DbUserName = DbUserName
                oCompany.DbPassword = DbPassword
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB
                oCompany.LicenseServer = LicenseServer

                Dim lRetCode As Integer = oCompany.Connect()
                If lRetCode = 0 Then




                    Dim errItemCodes As String = ""
                    Dim errMachineNos As String = ""

                    Dim LineID As String = ""
                    Dim errRowFlag As Boolean = False

                    Dim oGoodsIssue As SAPbobsCOM.Documents = Nothing
                    Dim oRecordset As SAPbobsCOM.Recordset = Nothing
                    Dim oRs1 As SAPbobsCOM.Recordset = Nothing

                    Dim ErrorCode As Integer
                    Dim ErrMessage As String
                    Dim iCount As Integer = 1



                    Dim Sql As String = ""


                    If Type = "ST" Then
                        Sql = "CALL SAP_GET_StockTransfer_GoodIssue_TankTempLoss_Data('" + DocEntry + "')"
                    ElseIf Type = "PF" Then
                        Sql = "CALL SAP_GET_GoodIssue_TankTempLoss_Data('" + DocEntry + "')"
                    End If


                    oRecordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    oRecordset.DoQuery(Sql)

                    If oRecordset.RecordCount > 0 Then

                        oGoodsIssue = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit)

                        oGoodsIssue.DocDate = oRecordset.Fields.Item("U_PDate").Value & ""
                        oGoodsIssue.TaxDate = oRecordset.Fields.Item("U_PDate").Value & ""
                        oGoodsIssue.DocDueDate = oRecordset.Fields.Item("U_PDate").Value & ""
                        'oGoodsIssue.Reference1 = oRecordset.Fields.Item("U_RefNo").Value & ""
                        oGoodsIssue.Comments = "Based on Inventory Transfer " + oRecordset.Fields.Item("DocNum").Value.ToString + " ,Tank Temp Loss" & ""
                        oGoodsIssue.BPL_IDAssignedToInvoice = CInt(oRecordset.Fields.Item("Branch").Value)
                        oGoodsIssue.UserFields.Fields.Item("U_ITDocE").Value = oRecordset.Fields.Item("DocEntry").Value & ""
                        oGoodsIssue.UserFields.Fields.Item("U_ITDocN").Value = oRecordset.Fields.Item("DocNum").Value & ""
                        oGoodsIssue.UserFields.Fields.Item("U_Flag").Value = "TTL" & ""
                        oGoodsIssue.Series = oRecordset.Fields.Item("Series").Value & ""





                        'Adding Row level Data
                        While oRecordset.EoF = False

                            oGoodsIssue.Lines.ItemCode = oRecordset.Fields.Item("U_ItemCode").Value & ""
                            oGoodsIssue.Lines.Quantity = oRecordset.Fields.Item("U_TTL").Value

                            If Type = "ST" Then
                                oGoodsIssue.Lines.WarehouseCode = oRecordset.Fields.Item("U_TW").Value & ""
                            ElseIf Type = "PF" Then
                                oGoodsIssue.Lines.WarehouseCode = oRecordset.Fields.Item("U_FW").Value & ""
                            End If



                            oGoodsIssue.Lines.UserFields.Fields.Item("U_Chamber").Value = oRecordset.Fields.Item("U_Chamber").Value & ""
                            oGoodsIssue.Lines.CostingCode = oRecordset.Fields.Item("U_OCRC").Value & ""
                            oGoodsIssue.Lines.CostingCode2 = oRecordset.Fields.Item("U_OCRC2").Value & ""
                            oGoodsIssue.Lines.CostingCode3 = oRecordset.Fields.Item("U_OCRC3").Value & ""
                            oGoodsIssue.Lines.CostingCode4 = oRecordset.Fields.Item("U_OCRC4").Value & ""
                            oGoodsIssue.Lines.CostingCode5 = oRecordset.Fields.Item("U_OCRC5").Value & ""


                            oGoodsIssue.Lines.UserFields.Fields.Item("U_Temp").Value = oRecordset.Fields.Item("U_Temp").Value & ""
                            oGoodsIssue.Lines.UserFields.Fields.Item("U_Density").Value = oRecordset.Fields.Item("U_Density").Value & ""
                            oGoodsIssue.Lines.UserFields.Fields.Item("U_Dip").Value = oRecordset.Fields.Item("U_Dip").Value & ""

                            oGoodsIssue.Lines.Add()
                            iCount = iCount + 1
                            oRecordset.MoveNext()

                        End While

                        Dim Result As Integer = oGoodsIssue.Add()

                        If Result <> 0 Then

                            __oApplication.StatusBar.SetText("Error:Tank Temp Loss Good Issue Not Generated - " + oCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Else


                            Dim SqlQuery As New StringBuilder

                            SqlQuery.Append("Update ""@SAP_OIT"" Set ""U_TTLGIDN""= (Select ""DocNum"" From OIGE Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='TTL'  ),")
                            SqlQuery.Append(" ""U_TTLGIDE""= (Select ""DocEntry"" From OIGE Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='TTL')")
                            SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")


                            Dim oRset1 As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                            oRset1.DoQuery(SqlQuery.ToString)
                            __oApplication.StatusBar.SetText(" Tank Temp Loss Good Issue  Generated", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)


                        End If

                    End If

                    Marshal.ReleaseComObject(oRecordset)
                    'Try
                    '    Marshal.ReleaseComObject(oGoodsIssue)
                    'Catch ex As Exception

                    'End Try
                    oCompany.Disconnect()

                End If

            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub


        Private Sub Manager_LandedCost_JounralEntry(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)


                Dim Server As String
                Dim CompanyDB As String
                Dim UserName As String
                Dim Password As String
                Dim DbUserName As String
                Dim DbPassword As String
                Dim LicenseServer As String

                Dim USSQL = "SELECT ""SERVER"",""CompanyDB"",""US"",""PD"", ""DbUserName"", ""DbPassword"" , ""LicenseServer"" FROM ""USINV"""
                Dim oRsUS As SAPbobsCOM.Recordset = Nothing
                oRsUS = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRsUS.DoQuery(USSQL)
                If oRsUS.RecordCount > 0 Then
                    Server = oRsUS.Fields.Item("SERVER").Value.ToString()
                    CompanyDB = oRsUS.Fields.Item("CompanyDB").Value.ToString()
                    UserName = oRsUS.Fields.Item("US").Value.ToString()
                    Password = oRsUS.Fields.Item("PD").Value.ToString()
                    DbUserName = oRsUS.Fields.Item("DbUserName").Value.ToString()
                    DbPassword = oRsUS.Fields.Item("DbPassword").Value.ToString()
                    LicenseServer = oRsUS.Fields.Item("LicenseServer").Value.ToString()
                End If

                Dim oCompany As SAPbobsCOM.Company = New SAPbobsCOM.Company()

                oCompany.Server = Server
                oCompany.UseTrusted = False
                oCompany.CompanyDB = CompanyDB
                oCompany.UserName = UserName
                oCompany.Password = Password
                oCompany.DbUserName = DbUserName
                oCompany.DbPassword = DbPassword
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB
                oCompany.LicenseServer = LicenseServer

                Dim lRetCode As Integer = oCompany.Connect()
                If lRetCode = 0 Then

                    Dim oRecordset As SAPbobsCOM.Recordset = Nothing
                    Dim Sql As String = ""
                    Sql = "CALL SAP_GETDATA_LandedCost_JE ('" + DocEntry + "')"
                    oRecordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    oRecordset.DoQuery(Sql)
                    If oRecordset.RecordCount > 0 Then


                        Dim oJounalEntry As SAPbobsCOM.JournalEntries = Nothing
                        oJounalEntry = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
                        oJounalEntry.ReferenceDate = oRecordset.Fields.Item("Date").Value
                        oJounalEntry.TaxDate = oRecordset.Fields.Item("Date").Value
                        oJounalEntry.DueDate = oRecordset.Fields.Item("Date").Value
                        oJounalEntry.UserFields.Fields.Item("U_ITDocE").Value = oRecordset.Fields.Item("DocEntry").Value & ""
                        oJounalEntry.UserFields.Fields.Item("U_ITDocN").Value = oRecordset.Fields.Item("DocNum").Value & ""
                        oJounalEntry.UserFields.Fields.Item("U_Flag").Value = "LCJ"



                        oJounalEntry.Memo = "LC Based On Inventory Trasnsfer  '" + oRecordset.Fields.Item("DocNum").Value.ToString & "" + "'  "



                        oJounalEntry.Lines.BPLID = oRecordset.Fields.Item("U_TB").Value
                        oJounalEntry.Lines.AccountCode = oRecordset.Fields.Item("DebitAccount").Value
                        oJounalEntry.Lines.Debit = oRecordset.Fields.Item("U_Amt").Value & ""
                        oJounalEntry.Lines.Credit = 0
                        oJounalEntry.Lines.DueDate = oRecordset.Fields.Item("Date").Value
                        oJounalEntry.Lines.TaxDate = oRecordset.Fields.Item("Date").Value
                        oJounalEntry.Lines.ReferenceDate1 = oRecordset.Fields.Item("Date").Value

                        oJounalEntry.Lines.CostingCode = oRecordset.Fields.Item("U_OCRC").Value & ""
                        oJounalEntry.Lines.CostingCode2 = oRecordset.Fields.Item("U_OCRC2").Value & ""
                        oJounalEntry.Lines.CostingCode3 = oRecordset.Fields.Item("U_OCRC3").Value & ""
                        oJounalEntry.Lines.CostingCode4 = oRecordset.Fields.Item("U_OCRC4").Value & ""
                        oJounalEntry.Lines.CostingCode5 = oRecordset.Fields.Item("U_OCRC5").Value & ""

                        oJounalEntry.Lines.Add()



                        oJounalEntry.Lines.BPLID = oRecordset.Fields.Item("U_TB").Value
                        oJounalEntry.Lines.AccountCode = oRecordset.Fields.Item("CreditAccount").Value
                        oJounalEntry.Lines.Debit = 0
                        oJounalEntry.Lines.Credit = oRecordset.Fields.Item("U_Amt").Value & ""
                        oJounalEntry.Lines.DueDate = oRecordset.Fields.Item("Date").Value
                        oJounalEntry.Lines.TaxDate = oRecordset.Fields.Item("Date").Value
                        oJounalEntry.Lines.ReferenceDate1 = oRecordset.Fields.Item("Date").Value

                        oJounalEntry.Lines.CostingCode = oRecordset.Fields.Item("U_OCRC").Value & ""
                        oJounalEntry.Lines.CostingCode2 = oRecordset.Fields.Item("U_OCRC2").Value & ""
                        oJounalEntry.Lines.CostingCode3 = oRecordset.Fields.Item("U_OCRC3").Value & ""
                        oJounalEntry.Lines.CostingCode4 = oRecordset.Fields.Item("U_OCRC4").Value & ""
                        oJounalEntry.Lines.CostingCode5 = oRecordset.Fields.Item("U_OCRC5").Value & ""
                        oJounalEntry.Lines.Add()





                        Dim Result As Integer = oJounalEntry.Add()
                        If Result <> 0 Then
                            __oApplication.StatusBar.SetText("Error:Landde Cost JE Not Generated - " + oCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                        Else

                            Dim SqlQuery As New StringBuilder

                            SqlQuery.Append("Update ""@SAP_OIT"" Set ""U_JE""= (Select ""TransId"" From OJDT Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='LCJ'  )")
                            SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")


                            Dim oRset1 As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                            oRset1.DoQuery(SqlQuery.ToString)

                            __oApplication.StatusBar.SetText("Landde Cost JE  Generated", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                        End If




                    End If
                    oCompany.Disconnect()
                End If



            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub


        Private Sub Maanger_AdjustmentJounralEntry_FromBranch(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                Dim Server As String
                Dim CompanyDB As String
                Dim UserName As String
                Dim Password As String
                Dim DbUserName As String
                Dim DbPassword As String
                Dim LicenseServer As String

                Dim USSQL = "SELECT ""SERVER"",""CompanyDB"",""US"",""PD"", ""DbUserName"", ""DbPassword"" , ""LicenseServer"" FROM ""USINV"""
                Dim oRsUS As SAPbobsCOM.Recordset = Nothing
                oRsUS = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRsUS.DoQuery(USSQL)
                If oRsUS.RecordCount > 0 Then
                    Server = oRsUS.Fields.Item("SERVER").Value.ToString()
                    CompanyDB = oRsUS.Fields.Item("CompanyDB").Value.ToString()
                    UserName = oRsUS.Fields.Item("US").Value.ToString()
                    Password = oRsUS.Fields.Item("PD").Value.ToString()
                    DbUserName = oRsUS.Fields.Item("DbUserName").Value.ToString()
                    DbPassword = oRsUS.Fields.Item("DbPassword").Value.ToString()
                    LicenseServer = oRsUS.Fields.Item("LicenseServer").Value.ToString()
                End If

                Dim oCompany As SAPbobsCOM.Company = New SAPbobsCOM.Company()

                oCompany.Server = Server
                oCompany.UseTrusted = False
                oCompany.CompanyDB = CompanyDB
                oCompany.UserName = UserName
                oCompany.Password = Password
                oCompany.DbUserName = DbUserName
                oCompany.DbPassword = DbPassword
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB
                oCompany.LicenseServer = LicenseServer

                Dim lRetCode As Integer = oCompany.Connect()
                If lRetCode = 0 Then

                    Dim oRecordset As SAPbobsCOM.Recordset = Nothing
                    Dim Sql As String = ""
                    Sql = "CALL SAP_GETDATA_Adjustment_JE_FromBranch ('" + DocEntry + "')"
                    oRecordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    oRecordset.DoQuery(Sql)
                    If oRecordset.RecordCount > 0 Then


                        Dim oJounalEntry As SAPbobsCOM.JournalEntries = Nothing
                        oJounalEntry = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
                        oJounalEntry.ReferenceDate = oRecordset.Fields.Item("Date").Value
                        oJounalEntry.TaxDate = oRecordset.Fields.Item("Date").Value
                        oJounalEntry.DueDate = oRecordset.Fields.Item("Date").Value
                        oJounalEntry.UserFields.Fields.Item("U_ITDocE").Value = oRecordset.Fields.Item("DocEntry").Value & ""
                        oJounalEntry.UserFields.Fields.Item("U_ITDocN").Value = oRecordset.Fields.Item("DocNum").Value & ""
                        oJounalEntry.UserFields.Fields.Item("U_Flag").Value = "AJEFB"



                        oJounalEntry.Memo = "ADJ FB Based On Inventory Trasnsfer  '" + oRecordset.Fields.Item("DocNum").Value.ToString & "" + "'  "



                        oJounalEntry.Lines.BPLID = oRecordset.Fields.Item("Branch").Value
                        oJounalEntry.Lines.AccountCode = oRecordset.Fields.Item("DebitAccount").Value
                        oJounalEntry.Lines.Debit = oRecordset.Fields.Item("Amt").Value & ""
                        oJounalEntry.Lines.Credit = 0
                        oJounalEntry.Lines.DueDate = oRecordset.Fields.Item("Date").Value
                        oJounalEntry.Lines.TaxDate = oRecordset.Fields.Item("Date").Value
                        oJounalEntry.Lines.ReferenceDate1 = oRecordset.Fields.Item("Date").Value

                        oJounalEntry.Lines.CostingCode = oRecordset.Fields.Item("U_OCRC").Value & ""
                        oJounalEntry.Lines.CostingCode2 = oRecordset.Fields.Item("U_OCRC2").Value & ""
                        oJounalEntry.Lines.CostingCode3 = oRecordset.Fields.Item("U_OCRC3").Value & ""
                        oJounalEntry.Lines.CostingCode4 = oRecordset.Fields.Item("U_OCRC4").Value & ""
                        oJounalEntry.Lines.CostingCode5 = oRecordset.Fields.Item("U_OCRC5").Value & ""

                        oJounalEntry.Lines.Add()



                        oJounalEntry.Lines.BPLID = oRecordset.Fields.Item("Branch").Value
                        oJounalEntry.Lines.AccountCode = oRecordset.Fields.Item("CreditAccount").Value
                        oJounalEntry.Lines.Debit = 0
                        oJounalEntry.Lines.Credit = oRecordset.Fields.Item("Amt").Value & ""
                        oJounalEntry.Lines.DueDate = oRecordset.Fields.Item("Date").Value
                        oJounalEntry.Lines.TaxDate = oRecordset.Fields.Item("Date").Value
                        oJounalEntry.Lines.ReferenceDate1 = oRecordset.Fields.Item("Date").Value

                        oJounalEntry.Lines.CostingCode = oRecordset.Fields.Item("U_OCRC").Value & ""
                        oJounalEntry.Lines.CostingCode2 = oRecordset.Fields.Item("U_OCRC2").Value & ""
                        oJounalEntry.Lines.CostingCode3 = oRecordset.Fields.Item("U_OCRC3").Value & ""
                        oJounalEntry.Lines.CostingCode4 = oRecordset.Fields.Item("U_OCRC4").Value & ""
                        oJounalEntry.Lines.CostingCode5 = oRecordset.Fields.Item("U_OCRC5").Value & ""
                        oJounalEntry.Lines.Add()





                        Dim Result As Integer = oJounalEntry.Add()
                        If Result <> 0 Then
                            __oApplication.StatusBar.SetText("Error:Adjustment JE From Branch Not Generated - " + oCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                        Else

                            Dim SqlQuery As New StringBuilder

                            SqlQuery.Append("Update ""@SAP_OIT"" Set ""U_AJE1""= (Select ""TransId"" From OJDT Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='AJEFB'  )")
                            SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")


                            Dim oRset1 As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                            oRset1.DoQuery(SqlQuery.ToString)

                            __oApplication.StatusBar.SetText("Adjustment JE From Branch  Generated ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                        End If




                    End If

                    oCompany.Disconnect()
                End If




            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub Manager_AdjustmentJounralEntry_ToBranch(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim Server As String
                Dim CompanyDB As String
                Dim UserName As String
                Dim Password As String
                Dim DbUserName As String
                Dim DbPassword As String
                Dim LicenseServer As String

                Dim USSQL = "SELECT ""SERVER"",""CompanyDB"",""US"",""PD"", ""DbUserName"", ""DbPassword"" , ""LicenseServer"" FROM ""USINV"""
                Dim oRsUS As SAPbobsCOM.Recordset = Nothing
                oRsUS = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRsUS.DoQuery(USSQL)
                If oRsUS.RecordCount > 0 Then
                    Server = oRsUS.Fields.Item("SERVER").Value.ToString()
                    CompanyDB = oRsUS.Fields.Item("CompanyDB").Value.ToString()
                    UserName = oRsUS.Fields.Item("US").Value.ToString()
                    Password = oRsUS.Fields.Item("PD").Value.ToString()
                    DbUserName = oRsUS.Fields.Item("DbUserName").Value.ToString()
                    DbPassword = oRsUS.Fields.Item("DbPassword").Value.ToString()
                    LicenseServer = oRsUS.Fields.Item("LicenseServer").Value.ToString()
                End If

                Dim oCompany As SAPbobsCOM.Company = New SAPbobsCOM.Company()

                oCompany.Server = Server
                oCompany.UseTrusted = False
                oCompany.CompanyDB = CompanyDB
                oCompany.UserName = UserName
                oCompany.Password = Password
                oCompany.DbUserName = DbUserName
                oCompany.DbPassword = DbPassword
                oCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB
                oCompany.LicenseServer = LicenseServer

                Dim lRetCode As Integer = oCompany.Connect()
                If lRetCode = 0 Then



                    Dim oRecordset As SAPbobsCOM.Recordset = Nothing
                    Dim Sql As String = ""
                    Sql = "CALL SAP_GETDATA_Adjustment_JE2_ToBranch ('" + DocEntry + "')"
                    oRecordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    oRecordset.DoQuery(Sql)
                    If oRecordset.RecordCount > 0 Then


                        Dim oJounalEntry As SAPbobsCOM.JournalEntries = Nothing
                        oJounalEntry = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries)
                        oJounalEntry.ReferenceDate = oRecordset.Fields.Item("Date").Value
                        oJounalEntry.TaxDate = oRecordset.Fields.Item("Date").Value
                        oJounalEntry.DueDate = oRecordset.Fields.Item("Date").Value
                        oJounalEntry.UserFields.Fields.Item("U_ITDocE").Value = oRecordset.Fields.Item("DocEntry").Value & ""
                        oJounalEntry.UserFields.Fields.Item("U_ITDocN").Value = oRecordset.Fields.Item("DocNum").Value & ""
                        oJounalEntry.UserFields.Fields.Item("U_Flag").Value = "AJETB"



                        oJounalEntry.Memo = "ADJ TB Based On Inventory Trasnsfer  '" + oRecordset.Fields.Item("DocNum").Value.ToString & "" + "'  "



                        oJounalEntry.Lines.BPLID = oRecordset.Fields.Item("Branch").Value
                        oJounalEntry.Lines.AccountCode = oRecordset.Fields.Item("DebitAccount").Value
                        oJounalEntry.Lines.Debit = oRecordset.Fields.Item("Amt").Value & ""
                        oJounalEntry.Lines.Credit = 0
                        oJounalEntry.Lines.DueDate = oRecordset.Fields.Item("Date").Value
                        oJounalEntry.Lines.TaxDate = oRecordset.Fields.Item("Date").Value
                        oJounalEntry.Lines.ReferenceDate1 = oRecordset.Fields.Item("Date").Value

                        oJounalEntry.Lines.CostingCode = oRecordset.Fields.Item("U_OCRC").Value & ""
                        oJounalEntry.Lines.CostingCode2 = oRecordset.Fields.Item("U_OCRC2").Value & ""
                        oJounalEntry.Lines.CostingCode3 = oRecordset.Fields.Item("U_OCRC3").Value & ""
                        oJounalEntry.Lines.CostingCode4 = oRecordset.Fields.Item("U_OCRC4").Value & ""
                        oJounalEntry.Lines.CostingCode5 = oRecordset.Fields.Item("U_OCRC5").Value & ""

                        oJounalEntry.Lines.Add()



                        oJounalEntry.Lines.BPLID = oRecordset.Fields.Item("Branch").Value
                        oJounalEntry.Lines.AccountCode = oRecordset.Fields.Item("CreditAccount").Value
                        oJounalEntry.Lines.Debit = 0
                        oJounalEntry.Lines.Credit = oRecordset.Fields.Item("Amt").Value & ""
                        oJounalEntry.Lines.DueDate = oRecordset.Fields.Item("Date").Value
                        oJounalEntry.Lines.TaxDate = oRecordset.Fields.Item("Date").Value
                        oJounalEntry.Lines.ReferenceDate1 = oRecordset.Fields.Item("Date").Value

                        oJounalEntry.Lines.CostingCode = oRecordset.Fields.Item("U_OCRC").Value & ""
                        oJounalEntry.Lines.CostingCode2 = oRecordset.Fields.Item("U_OCRC2").Value & ""
                        oJounalEntry.Lines.CostingCode3 = oRecordset.Fields.Item("U_OCRC3").Value & ""
                        oJounalEntry.Lines.CostingCode4 = oRecordset.Fields.Item("U_OCRC4").Value & ""
                        oJounalEntry.Lines.CostingCode5 = oRecordset.Fields.Item("U_OCRC5").Value & ""
                        oJounalEntry.Lines.Add()





                        Dim Result As Integer = oJounalEntry.Add()
                        If Result <> 0 Then
                            __oApplication.StatusBar.SetText("Error:Adjustment JE To Branch Not Generated - " + oCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                        Else

                            Dim SqlQuery As New StringBuilder

                            SqlQuery.Append("Update ""@SAP_OIT"" Set ""U_AJE2""= (Select ""TransId"" From OJDT Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='AJETB'  )")
                            SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")


                            Dim oRset1 As SAPbobsCOM.Recordset = oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                            oRset1.DoQuery(SqlQuery.ToString)

                            __oApplication.StatusBar.SetText("Adjustment JE To Branch  Generated ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                        End If




                    End If
                    oCompany.Disconnect()

                End If


            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub







        'Private Sub SAP_Tranction(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String)
        '    Try
        '        BubbleEvent = True
        '        oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
        '        If DocEntry.ToString <> "" Then


        '            Dim TransType As String

        '            Dim TransTypeQuery As String = ""
        '            TransTypeQuery = "CALL SAP_GET_TransctionType ('" + DocEntry.ToString + "')"
        '            Dim TransTypeoRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        '            TransTypeoRs.DoQuery(TransTypeQuery)
        '            If TransTypeoRs.RecordCount > 0 Then
        '                TransType = TransTypeoRs.Fields.Item("TransType").Value
        '            End If


        '            ''''''''''''''''''''' This Transction Use For Base On Inventry Transfer Request '''''''''''''''''''''''''''''''
        '            Dim FL, TL, FW, TW, FB, TB, PODE, ITDE, GRN, GI, GR, TGI, INV, IT, TTLGIDN, INVTYPE, LCJ, AJE1, AJE2 As String
        '            Dim LS As Int64
        '            If TransType = "BOIR" Then



        '                Dim Query As String = ""
        '                Query = "CALL SAP_GET_Condition_Inventory_Transction ('" + DocEntry.ToString + "')"
        '                Dim oRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        '                oRs.DoQuery(Query)
        '                If oRs.RecordCount > 0 Then
        '                    FL = oRs.Fields.Item("FromLocation").Value
        '                    TL = oRs.Fields.Item("ToLcation").Value
        '                    FW = oRs.Fields.Item("U_FW").Value
        '                    TW = oRs.Fields.Item("U_TW").Value
        '                    FB = oRs.Fields.Item("U_FB").Value
        '                    TB = oRs.Fields.Item("U_TB").Value

        '                End If

        '                If FL = TL And FB = TB Then
        '                    IT = GetValue("SELECT T0.""U_ITDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITDE")
        '                    If IT = "" Then
        '                        Normal_InventoryTransfer(FormUID, pVal, BubbleEvent, DocEntry)
        '                    End If
        '                End If

        '                If FL <> TL And FB <> TB Then

        '                    PODE = GetValue("SELECT  T0.""U_PODE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_PODE")
        '                    If PODE = "" Then
        '                        Dim LCAmt As Decimal = GetValue("SELECT Sum(IFNULL(T0.""U_Amt"",0)) AS ""LC"" FROM ""@SAP_IT2""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "LC")
        '                        If LCAmt > 0 Then
        '                            PurchaseOrder_Creation(FormUID, pVal, BubbleEvent, DocEntry)
        '                        End If

        '                    End If

        '                    IT = GetValue("SELECT T0.""U_ITDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITDE")
        '                    If IT = "" Then
        '                        Normal_InventoryTransfer(FormUID, pVal, BubbleEvent, DocEntry)
        '                    End If

        '                    ITDE = GetValue("SELECT T0.""U_ITDocE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITDocE")
        '                    If ITDE <> "" Then



        '                        Dim ItemCode As String = GetValue("Select TOP 1 ""U_ItemCode"" From  ""@SAP_IT1""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' And IFNULL(""U_ItemCode"" ,'')<>'' ", "U_ItemCode")
        '                        Dim ItemGrp As String = GetValue("Select ""ItmsGrpNam"" from OITB Where ""ItmsGrpCod"" =(SELECT T0.""ItmsGrpCod"" FROM OITM T0 WHERE T0.""ItemCode"" ='" + ItemCode + "')", "ItmsGrpNam")
        '                        If ItemGrp = "Trading" Then
        '                            Dim QC As String = GetValue("SELECT T0.""U_QC"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_QC")
        '                            If QC = "A" Then



        '                                GI = GetValue("SELECT T0.""U_GIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GIDE")
        '                                If GI = "" Then
        '                                    CreateGoodsIssue(FormUID, pVal, BubbleEvent, DocEntry)
        '                                End If

        '                                GR = GetValue("SELECT T0.""U_GRDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GRDE")
        '                                If GR = "" Then
        '                                    CreateGoodsReceipt(FormUID, pVal, BubbleEvent, DocEntry)
        '                                End If

        '                                GRN = GetValue("SELECT T0.""U_GRNDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GRNDE")
        '                                If GRN = "" Then
        '                                    GoodReceiptPO_Creation(FormUID, pVal, BubbleEvent, DocEntry)
        '                                End If


        '                                LS = 0
        '                                LS = GetValue("SELECT Count(T0.""DocEntry"") AS COUNT FROM ""@SAP_IT4""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' AND IFNULL(T0.""U_ItemCode"",'')<>''", "COUNT")
        '                                If LS <= 0 Then
        '                                    LossCalCulation(FormUID, pVal, BubbleEvent, DocEntry, "ST")
        '                                End If

        '                                TGI = GetValue("SELECT T0.""U_TGIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TGIDE")
        '                                If TGI = "" Then
        '                                    Create_TransportationLoss_GoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "ST")
        '                                End If

        '                                INV = GetValue("SELECT T0.""U_ARDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ARDE")
        '                                If INV = "" Then
        '                                    ARInvoice_Creation(FormUID, pVal, BubbleEvent, DocEntry, "ST")
        '                                End If


        '                                INVTYPE = GetValue("SELECT T0.""U_ITT"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITT")
        '                                If INVTYPE = "DE" Then
        '                                    TTLGIDN = GetValue("SELECT T0.""U_TTLGIDN"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TTLGIDN")
        '                                    If TTLGIDN = "" Then
        '                                        TannkTempLoss_CreateGoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "ST")
        '                                    End If
        '                                End If

        '                                LCJ = GetValue("SELECT T0.""U_JE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_JE")
        '                                If LCJ = "" Then
        '                                    LandedCost_JounralEntry(FormUID, pVal, BubbleEvent, DocEntry)
        '                                End If

        '                                AJE1 = GetValue("SELECT T0.""U_AJE1"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_AJE1")
        '                                If AJE1 = "" Then
        '                                    AdjustmentJounralEntry_FromBranch(FormUID, pVal, BubbleEvent, DocEntry)
        '                                End If

        '                                AJE2 = GetValue("SELECT T0.""U_AJE2"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_AJE2")
        '                                If AJE2 = "" Then
        '                                    AdjustmentJounralEntry_ToBranch(FormUID, pVal, BubbleEvent, DocEntry)
        '                                End If


        '                            Else

        '                                If __oApplication.MessageBox("Transcation not process because of QC Not Approved Please Change the Hold Warehouse .. If You Change Hold Warehouse Then Continua", 1, "Yes", "No", "") = 2 Then
        '                                    BubbleEvent = False
        '                                    Exit Sub
        '                                Else
        '                                    Dim Branch As String = GetValue("Select  ""U_TB"" From  ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' And IFNULL(""U_TB"" ,'')<>'' ", "U_TB")
        '                                    Dim Whs As String = GetValue("Select  ""U_TW"" From  ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' And IFNULL(""U_TW"" ,'')<>'' ", "U_TW")
        '                                    Dim HWhs As String = GetValue("SELECT T0.""WhsCode"" FROM OWHS T0 WHERE T0.""U_Category"" ='Hold'  and  T0.""BPLid"" = '" + Branch + "'", "WhsCode")
        '                                    If Whs <> HWhs Then
        '                                        __oApplication.MessageBox("Transcation not process because of QC Not Approved Please Change the Hold Warehouse..", 1, "Ok", "", "")
        '                                        BubbleEvent = False
        '                                        Exit Sub
        '                                    Else


        '                                        GI = GetValue("SELECT T0.""U_GIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GIDE")
        '                                        If GI = "" Then
        '                                            CreateGoodsIssue(FormUID, pVal, BubbleEvent, DocEntry)
        '                                        End If

        '                                        GR = GetValue("SELECT T0.""U_GRDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GRDE")
        '                                        If GR = "" Then
        '                                            CreateGoodsReceipt(FormUID, pVal, BubbleEvent, DocEntry)
        '                                        End If

        '                                        GRN = GetValue("SELECT T0.""U_GRNDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GRNDE")
        '                                        If GRN = "" Then
        '                                            GoodReceiptPO_Creation(FormUID, pVal, BubbleEvent, DocEntry)
        '                                        End If


        '                                        LS = 0
        '                                        LS = GetValue("SELECT Count(T0.""DocEntry"") AS COUNT FROM ""@SAP_IT4""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' AND IFNULL(T0.""U_ItemCode"",'')<>''", "COUNT")
        '                                        If LS <= 0 Then
        '                                            LossCalCulation(FormUID, pVal, BubbleEvent, DocEntry, "ST")
        '                                        End If

        '                                        TGI = GetValue("SELECT T0.""U_TGIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TGIDE")
        '                                        If TGI = "" Then
        '                                            Create_TransportationLoss_GoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "ST")
        '                                        End If

        '                                        INV = GetValue("SELECT T0.""U_ARDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ARDE")
        '                                        If INV = "" Then
        '                                            ARInvoice_Creation(FormUID, pVal, BubbleEvent, DocEntry, "ST")
        '                                        End If


        '                                        INVTYPE = GetValue("SELECT T0.""U_ITT"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITT")
        '                                        If INVTYPE = "DE" Then
        '                                            TTLGIDN = GetValue("SELECT T0.""U_TTLGIDN"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TTLGIDN")
        '                                            If TTLGIDN = "" Then
        '                                                TannkTempLoss_CreateGoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "ST")
        '                                            End If
        '                                        End If

        '                                        LCJ = GetValue("SELECT T0.""U_JE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_JE")
        '                                        If LCJ = "" Then
        '                                            LandedCost_JounralEntry(FormUID, pVal, BubbleEvent, DocEntry)
        '                                        End If

        '                                        AJE1 = GetValue("SELECT T0.""U_AJE1"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_AJE1")
        '                                        If AJE1 = "" Then
        '                                            AdjustmentJounralEntry_FromBranch(FormUID, pVal, BubbleEvent, DocEntry)
        '                                        End If

        '                                        AJE2 = GetValue("SELECT T0.""U_AJE2"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_AJE2")
        '                                        If AJE2 = "" Then
        '                                            AdjustmentJounralEntry_ToBranch(FormUID, pVal, BubbleEvent, DocEntry)
        '                                        End If
        '                                    End If
        '                                End If
        '                            End If


        '                        Else



        '                            GI = GetValue("SELECT T0.""U_GIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GIDE")
        '                            If GI = "" Then
        '                                CreateGoodsIssue(FormUID, pVal, BubbleEvent, DocEntry)
        '                            End If

        '                            GR = GetValue("SELECT T0.""U_GRDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GRDE")
        '                            If GR = "" Then
        '                                CreateGoodsReceipt(FormUID, pVal, BubbleEvent, DocEntry)
        '                            End If

        '                            GRN = GetValue("SELECT T0.""U_GRNDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GRNDE")
        '                            If GRN = "" Then
        '                                GoodReceiptPO_Creation(FormUID, pVal, BubbleEvent, DocEntry)
        '                            End If


        '                            LS = 0
        '                            LS = GetValue("SELECT Count(T0.""DocEntry"") AS COUNT FROM ""@SAP_IT4""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' AND IFNULL(T0.""U_ItemCode"",'')<>''", "COUNT")
        '                            If LS <= 0 Then
        '                                LossCalCulation(FormUID, pVal, BubbleEvent, DocEntry, "ST")
        '                            End If

        '                            TGI = GetValue("SELECT T0.""U_TGIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TGIDE")
        '                            If TGI = "" Then
        '                                Create_TransportationLoss_GoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "ST")
        '                            End If

        '                            INV = GetValue("SELECT T0.""U_ARDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ARDE")
        '                            If INV = "" Then
        '                                ARInvoice_Creation(FormUID, pVal, BubbleEvent, DocEntry, "ST")
        '                            End If


        '                            INVTYPE = GetValue("SELECT T0.""U_ITT"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITT")
        '                            If INVTYPE = "DE" Then
        '                                TTLGIDN = GetValue("SELECT T0.""U_TTLGIDN"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TTLGIDN")
        '                                If TTLGIDN = "" Then
        '                                    TannkTempLoss_CreateGoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "ST")
        '                                End If
        '                            End If

        '                            LCJ = GetValue("SELECT T0.""U_JE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_JE")
        '                            If LCJ = "" Then
        '                                LandedCost_JounralEntry(FormUID, pVal, BubbleEvent, DocEntry)
        '                            End If

        '                            AJE1 = GetValue("SELECT T0.""U_AJE1"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_AJE1")
        '                            If AJE1 = "" Then
        '                                AdjustmentJounralEntry_FromBranch(FormUID, pVal, BubbleEvent, DocEntry)
        '                            End If

        '                            AJE2 = GetValue("SELECT T0.""U_AJE2"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_AJE2")
        '                            If AJE2 = "" Then
        '                                AdjustmentJounralEntry_ToBranch(FormUID, pVal, BubbleEvent, DocEntry)
        '                            End If
        '                        End If

        '                        Try
        '                            Dim SqlQuery As New StringBuilder

        '                            SqlQuery.Append("Update ""@SAP_OITR"" Set ""U_ITDocN""= (Select ""DocNum"" From ""@SAP_OIT"" Where ""DocEntry""='" + DocEntry + "'  ),")
        '                            SqlQuery.Append(" ""U_ITDocE""= (Select ""DocEntry"" From ""@SAP_OIT"" Where ""DocEntry""='" + DocEntry + "' )")
        '                            SqlQuery.Append("Where ""DocEntry""='" + GetValue("SELECT T0.""U_ITRDocE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITRDocE") + "'")


        '                            Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        '                            oRset1.DoQuery(SqlQuery.ToString)
        '                        Catch ex As Exception

        '                        End Try
        '                    End If



        '                End If
        '                ''''''''''''''''''''' This Transction Use For Base On Inventry Transfer Request '''''''''''''''''''END'''''''''''''''''''''''''''''''''''''''

        '                ''''''''''''''''''''' This Transction Use For Base On GRN Start '''''''''''''''''''END'''''''''''''''''''''''''''''''''''''''
        '            ElseIf TransType = "BOGRN" Then


        '                Dim ItemCode As String = GetValue("Select TOP 1 ""U_ItemCode"" From  ""@SAP_IT1""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' And IFNULL(""U_ItemCode"" ,'')<>'' ", "U_ItemCode")

        '                Dim ItemGrp As String = GetValue("Select ""ItmsGrpNam"" from OITB Where ""ItmsGrpCod"" =(SELECT T0.""ItmsGrpCod"" FROM OITM T0 WHERE T0.""ItemCode"" ='" + ItemCode + "')", "ItmsGrpNam")
        '                If ItemGrp = "Trading" Then
        '                    Dim QC As String = GetValue("SELECT T0.""U_QC"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_QC")
        '                    If QC = "A" Then
        '                        GRN = GetValue("SELECT T0.""U_GRNDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GRNDE")
        '                        If GRN = "" Then
        '                            Purchase_GoodReceiptPO_Creation(FormUID, pVal, BubbleEvent, DocEntry)
        '                        End If


        '                        LS = 0
        '                        LS = GetValue("SELECT Count(T0.""DocEntry"") AS COUNT FROM ""@SAP_IT4""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' AND IFNULL(T0.""U_ItemCode"",'')<>''", "COUNT")
        '                        If LS <= 0 Then
        '                            LossCalCulation(FormUID, pVal, BubbleEvent, DocEntry, "PF")
        '                        End If

        '                        TGI = GetValue("SELECT T0.""U_TGIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TGIDE")
        '                        If TGI = "" Then
        '                            Create_TransportationLoss_GoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "PF")
        '                        End If

        '                        INV = GetValue("SELECT T0.""U_ARDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ARDE")
        '                        If INV = "" Then
        '                            ARInvoice_Creation(FormUID, pVal, BubbleEvent, DocEntry, "PF")
        '                        End If

        '                        INVTYPE = GetValue("SELECT T0.""U_ITT"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITT")
        '                        If INVTYPE = "DE" Then
        '                            TTLGIDN = GetValue("SELECT T0.""U_TTLGIDN"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TTLGIDN")
        '                            If TTLGIDN = "" Then
        '                                TannkTempLoss_CreateGoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "PF")
        '                            End If
        '                        End If


        '                        IT = GetValue("SELECT T0.""U_ITDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITDE")
        '                        If IT = "" Then
        '                            Normal_InventoryTransfer(FormUID, pVal, BubbleEvent, DocEntry)
        '                        End If
        '                    Else

        '                        If __oApplication.MessageBox("Transcation not process because of QC Not Approved Please Change the Hold Warehouse .. If You Change Hold Warehouse Then Continua", 1, "Yes", "No", "") = 2 Then
        '                            BubbleEvent = False
        '                            Exit Sub
        '                        Else
        '                            Dim Branch As String = GetValue("Select  ""U_TB"" From  ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' And IFNULL(""U_TB"" ,'')<>'' ", "U_TB")
        '                            Dim Whs As String = GetValue("Select  ""U_TW"" From  ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' And IFNULL(""U_TW"" ,'')<>'' ", "U_TW")
        '                            Dim HWhs As String = GetValue("SELECT T0.""WhsCode"" FROM OWHS T0 WHERE T0.""U_Category"" ='Hold'  and  T0.""BPLid"" = '" + Branch + "'", "WhsCode")

        '                            If Whs <> HWhs Then
        '                                __oApplication.MessageBox("Transcation not process because of QC Not Approved Please Change the Hold Warehouse..", 1, "Ok", "", "")
        '                                BubbleEvent = False
        '                                Exit Sub
        '                            Else
        '                                GRN = GetValue("SELECT T0.""U_GRNDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GRNDE")
        '                                If GRN = "" Then
        '                                    Purchase_GoodReceiptPO_Creation(FormUID, pVal, BubbleEvent, DocEntry)
        '                                End If


        '                                LS = 0
        '                                LS = GetValue("SELECT Count(T0.""DocEntry"") AS COUNT FROM ""@SAP_IT4""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' AND IFNULL(T0.""U_ItemCode"",'')<>''", "COUNT")
        '                                If LS <= 0 Then
        '                                    LossCalCulation(FormUID, pVal, BubbleEvent, DocEntry, "PF")
        '                                End If

        '                                TGI = GetValue("SELECT T0.""U_TGIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TGIDE")
        '                                If TGI = "" Then
        '                                    Create_TransportationLoss_GoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "PF")
        '                                End If

        '                                INV = GetValue("SELECT T0.""U_ARDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ARDE")
        '                                If INV = "" Then
        '                                    ARInvoice_Creation(FormUID, pVal, BubbleEvent, DocEntry, "PF")
        '                                End If

        '                                INVTYPE = GetValue("SELECT T0.""U_ITT"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITT")
        '                                If INVTYPE = "DE" Then
        '                                    TTLGIDN = GetValue("SELECT T0.""U_TTLGIDN"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TTLGIDN")
        '                                    If TTLGIDN = "" Then
        '                                        TannkTempLoss_CreateGoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "PF")
        '                                    End If
        '                                End If


        '                                IT = GetValue("SELECT T0.""U_ITDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITDE")
        '                                If IT = "" Then
        '                                    Normal_InventoryTransfer(FormUID, pVal, BubbleEvent, DocEntry)
        '                                End If
        '                            End If
        '                        End If


        '                    End If
        '                Else
        '                    GRN = GetValue("SELECT T0.""U_GRNDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GRNDE")
        '                    If GRN = "" Then
        '                        Purchase_GoodReceiptPO_Creation(FormUID, pVal, BubbleEvent, DocEntry)
        '                    End If


        '                    LS = 0
        '                    LS = GetValue("SELECT Count(T0.""DocEntry"") AS COUNT FROM ""@SAP_IT4""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' AND IFNULL(T0.""U_ItemCode"",'')<>''", "COUNT")
        '                    If LS <= 0 Then
        '                        LossCalCulation(FormUID, pVal, BubbleEvent, DocEntry, "PF")
        '                    End If

        '                    TGI = GetValue("SELECT T0.""U_TGIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TGIDE")
        '                    If TGI = "" Then
        '                        Create_TransportationLoss_GoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "PF")
        '                    End If

        '                    INV = GetValue("SELECT T0.""U_ARDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ARDE")
        '                    If INV = "" Then
        '                        ARInvoice_Creation(FormUID, pVal, BubbleEvent, DocEntry, "PF")
        '                    End If

        '                    INVTYPE = GetValue("SELECT T0.""U_ITT"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITT")
        '                    If INVTYPE = "DE" Then
        '                        TTLGIDN = GetValue("SELECT T0.""U_TTLGIDN"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TTLGIDN")
        '                        If TTLGIDN = "" Then
        '                            TannkTempLoss_CreateGoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "PF")
        '                        End If
        '                    End If


        '                    IT = GetValue("SELECT T0.""U_ITDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITDE")
        '                    If IT = "" Then
        '                        Normal_InventoryTransfer(FormUID, pVal, BubbleEvent, DocEntry)
        '                    End If

        '                End If

        '            End If


        '            '''This Code For QC Sample Type 
        '            '''''''''''''''''''''
        '            Dim QCTYPE As String = GetValue("SELECT T0.""U_ITT"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITT")
        '            Dim QCNO As String = GetValue("SELECT T0.""U_QCNO"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_QCNO")

        '            If QCTYPE = "S" And QCNO <> "" Then

        '                IT = GetValue("SELECT T0.""U_ITDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITDE")
        '                If IT = "" Then
        '                    Normal_InventoryTransfer(FormUID, pVal, BubbleEvent, DocEntry)
        '                End If
        '            End If




        '        End If
        '        '__Application.ActivateMenuItem("1304")
        '    Catch ex As Exception
        '        __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
        '    End Try
        'End Sub

    End Class
End Namespace

