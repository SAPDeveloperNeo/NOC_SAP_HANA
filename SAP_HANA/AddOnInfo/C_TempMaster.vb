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
    Public Class C_TempMaster : Implements ISAP_HANA
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
                Dim sFileName As String = AppDomain.CurrentDomain.BaseDirectory & "SRF\TempMaster.srf"
                Dim sFormName As String = "SAP_UDO_OTMD"
                Dim FormUID As String = LoadXMLFiles(__oApplication, sFileName)
                oForm = __oApplication.Forms.GetForm("SAP_UDO_OTMD", __oApplication.Forms.ActiveForm.TypeCount)
                If Not String.IsNullOrEmpty(FormUID) Then
                    ActiveForm(oForm, "Item_27", "1")
                    oForm.EnableMenu("1292", True)
                    oForm.EnableMenu("1293", True)
                    oForm.EnableMenu("520", True)
                    oForm.EnableMenu("519", True)
                    oForm.EnableMenu("1281", True)
                    oForm.EnableMenu("1282", True)
                    oForm.EnableMenu("1288", True)
                    oForm.EnableMenu("1289", True)
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

        Public Sub Form_Process_ItemEvents(FormUID As String, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Process_ItemEvents
            Try
                BubbleEvent = True

                If pVal.BeforeAction = False Then

                    If pVal.ItemUID = "Item_29" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Location_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_8" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Branch_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_2" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        WhsCode_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_0" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        ItemCode_OnAfterChooseFromList(FormUID, pVal, BubbleEvent)

                        'ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_0" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        '    AddRow_OnAfterLocstFocus(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.ItemUID = "m1" And (pVal.ColUID = "Col_7") And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        OilDip1_OnAfterLocstFocus(FormUID, pVal, BubbleEvent)
                        'Qty_UneditableAfterwhs(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.ItemUID = "m1" And (pVal.ColUID = "Col_12") And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        OilDip2_OnAfterLocstFocus(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "m1" And (pVal.ColUID = "Col_8") And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        OilDIFF_OnAfterLocstFocus(FormUID, pVal, BubbleEvent)

                        'ElseIf pVal.ItemUID = "m1" And (pVal.ColUID = "Col_1") And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        '    Qty_UneditableAfterwhs(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_2" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        OutSideTempLostFocus(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_5" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        TankTempLostFocus(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "Item_13" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_LOST_FOCUS Then
                        DensityTempLostFocus(FormUID, pVal, BubbleEvent)
                    End If

                ElseIf pVal.BeforeAction = True Then

                    If pVal.ItemUID = "m1" And pVal.ColUID = "Col_0" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_ItemCode_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.ItemUID = "m1" And pVal.ColUID = "Col_2" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST Then
                        Matrix1_WhsCode_OnBeforeChooseFromList(FormUID, pVal, BubbleEvent)

                    ElseIf pVal.ItemUID = "1" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        Add_OnBeforeItemPressed(FormUID, pVal, BubbleEvent)
                    ElseIf pVal.ItemUID = "btnPost" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        PostSAP_OnAfterItemPressed(FormUID, pVal, BubbleEvent)
                    End If

                End If

            Catch ex As Exception

                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub
        Private Sub Maanger_CreateGoodsReceipt(ByVal DocEntry As String, ByVal ItemCode As String)
            Try
                ' BubbleEvent = True
                '  oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

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
                    Sql = "CALL SAP_GET_GoodIssue_DipLog('" + DocEntry + "','" + ItemCode & "','GR')"

                    oRecordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    oRecordset.DoQuery(Sql)

                    If oRecordset.RecordCount > 0 Then

                        oGoodsReceipt = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry)

                        oGoodsReceipt.DocDate = oRecordset.Fields.Item("U_PDate").Value & ""
                        oGoodsReceipt.TaxDate = oRecordset.Fields.Item("U_PDate").Value & ""
                        oGoodsReceipt.DocDueDate = oRecordset.Fields.Item("U_PDate").Value & ""

                        oGoodsReceipt.Comments = "From Dip Log:"
                        oGoodsReceipt.BPL_IDAssignedToInvoice = CInt(oRecordset.Fields.Item("BPLId").Value)
                        oGoodsReceipt.UserFields.Fields.Item("U_DLDOCENTRY").Value = oRecordset.Fields.Item("DocEntry").Value & ""
                        oGoodsReceipt.UserFields.Fields.Item("U_DLDOCNUM").Value = oRecordset.Fields.Item("DocNum").Value & ""
                        oGoodsReceipt.UserFields.Fields.Item("U_Flag").Value = "DIPLOG" & ""
                        oGoodsReceipt.Series = oRecordset.Fields.Item("Series").Value & ""





                        'Adding Row level Data
                        While oRecordset.EoF = False

                            oGoodsReceipt.Lines.ItemCode = oRecordset.Fields.Item("U_ItemCode").Value & ""
                            oGoodsReceipt.Lines.Quantity = oRecordset.Fields.Item("Qty").Value
                            oGoodsReceipt.Lines.WarehouseCode = oRecordset.Fields.Item("WhsCode").Value & ""
                            ' oGoodsReceipt.Lines.Price = 1 'oRecordset.Fields.Item("Price").Value & ""
                            ' oGoodsReceipt.Lines.LineTotal = 25 'oRecordset.Fields.Item("LineTotal").Value & ""

                            oGoodsReceipt.Lines.CostingCode = oRecordset.Fields.Item("U_Office").Value & ""
                            oGoodsReceipt.Lines.CostingCode2 = "DOSUPPD" 'oRecordset.Fields.Item("OcrCode2").Value & ""
                            oGoodsReceipt.Lines.CostingCode3 = "NA1" 'oRecordset.Fields.Item("OcrCode3").Value & ""
                            oGoodsReceipt.Lines.CostingCode4 = "NA" 'oRecordset.Fields.Item("OcrCode4").Value & ""
                            oGoodsReceipt.Lines.CostingCode5 = "" '- THK ' oRecordset.Fields.Item("OcrCode5").Value & ""
                            'oGoodsReceipt.Lines.UserFields.Fields.Item("U_Chamber").Value = oRecordset.Fields.Item("U_Chamber").Value & ""
                            'oGoodsReceipt.Lines.UserFields.Fields.Item("U_Temp").Value = oRecordset.Fields.Item("U_Temp").Value & ""
                            'oGoodsReceipt.Lines.UserFields.Fields.Item("U_Density").Value = oRecordset.Fields.Item("U_Density").Value & ""
                            'oGoodsReceipt.Lines.UserFields.Fields.Item("U_Dip").Value = oRecordset.Fields.Item("U_Dip").Value & ""

                            'oGoodsReceipt.Lines.AccountCode = oRecordset.Fields.Item("AccountCode").Value & ""


                            oGoodsReceipt.Lines.Add()
                            iCount = iCount + 1
                            oRecordset.MoveNext()

                        End While

                        Dim Result As Integer = oGoodsReceipt.Add()

                        If Result <> 0 Then

                            __oApplication.StatusBar.SetText("Error: Good Receipt Not Generated - " + oCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Else


                            Dim SqlQuery As New StringBuilder

                            SqlQuery.Append("Update ""@SAP_TMD1"" Set ""U_FLAG""= 'Y',")
                            SqlQuery.Append(" ""U_GRDOCE""= (Select top 1 a.""DocEntry"" From OIGN a INNER JOIN IGN1 B ON a.""DocEntry"" =B.""DocEntry"" And ""ItemCode""='" + ItemCode + "' Where ""U_DLDOCENTRY""='" + DocEntry + "' And ""U_Flag""='DIPLOG') ,U_TRANSTYPE='GR' ")
                            SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")


                            Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                            oRset1.DoQuery(SqlQuery.ToString)


                            SqlQuery.Append("Select top 1 ""DocNum"" From OIGN a INNER JOIN IGN1 B ON a.""DocEntry"" =B.""DocEntry"" And ""ItemCode""='" + ItemCode + "' Where ""U_DLDOCENTRY""='" + DocEntry + "' And ""U_Flag""='DIPLOG',U_TRANSTYPE='GR' ")
                            'SAP_Tranction(FormUID, pVal, BubbleEvent, DocEntry)
                            Dim PostoRs2 As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                            PostoRs2.DoQuery(SqlQuery.ToString)


                            __oApplication.StatusBar.SetText("Good Receipt Generated :" + PostoRs2.Fields.Item("DocNum").Value, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                        End If

                    End If

                    Marshal.ReleaseComObject(oRecordset)
                    Try
                        Marshal.ReleaseComObject(oGoodsReceipt)
                    Catch ex As Exception
                        '  BubbleEvent = False
                    End Try
                    oCompany.Disconnect()
                End If

            Catch ex As Exception
                '  BubbleEvent = False
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub CreateGoodsReceipt(ByVal DocEntry As String, ByVal ItemCode As String)
            Try
                'BubbleEvent = True
                ' oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                Dim errItemCodes As String = ""
                Dim errMachineNos As String = ""

                Dim LineID As String = ""
                Dim errRowFlag As Boolean = False

                Dim oGoodsReceipt As SAPbobsCOM.Documents = Nothing
                Dim oRecordset As SAPbobsCOM.Recordset = Nothing
                Dim oRecordset1 As SAPbobsCOM.Recordset = Nothing
                Dim oRs1 As SAPbobsCOM.Recordset = Nothing

                Dim ErrorCode As Integer
                Dim ErrMessage As String
                Dim iCount As Integer = 1



                Dim Sql As String = ""
                Sql = "CALL SAP_GET_GoodIssue_DipLog('" + DocEntry + "','" + ItemCode & "','GR')"

                oRecordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRecordset.DoQuery(Sql)

                If oRecordset.RecordCount > 0 Then

                    oGoodsReceipt = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenEntry)
                    Dim loccode As String = oForm.Items.Item("Item_29").Specific.Value
                    Sql = "select TOP 1 ""U_PDate"" from ""@SAP_OTMD"" where ""U_Status""='C' and ""U_LocCode""='" + loccode + "' order by ""U_PDate"" desc"
                    oRecordset1 = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    oRecordset1.DoQuery(Sql)

                    oGoodsReceipt.DocDate = oRecordset1.Fields.Item("U_PDate").Value & ""
                    oGoodsReceipt.TaxDate = oRecordset1.Fields.Item("U_PDate").Value & ""
                    oGoodsReceipt.DocDueDate = oRecordset1.Fields.Item("U_PDate").Value & ""

                    oGoodsReceipt.Comments = "From Dip Log:"
                    oGoodsReceipt.BPL_IDAssignedToInvoice = CInt(oRecordset.Fields.Item("BPLId").Value)
                    oGoodsReceipt.UserFields.Fields.Item("U_DLDOCENTRY").Value = oRecordset.Fields.Item("DocEntry").Value & ""
                    oGoodsReceipt.UserFields.Fields.Item("U_DLDOCNUM").Value = oRecordset.Fields.Item("DocNum").Value & ""
                    oGoodsReceipt.UserFields.Fields.Item("U_Flag").Value = "DIPLOGGR" & ""
                    oGoodsReceipt.Series = oRecordset.Fields.Item("Series").Value & ""





                    'Adding Row level Data
                    While oRecordset.EoF = False

                        oGoodsReceipt.Lines.ItemCode = oRecordset.Fields.Item("U_ItemCode").Value & ""
                        oGoodsReceipt.Lines.Quantity = oRecordset.Fields.Item("Qty").Value
                        oGoodsReceipt.Lines.WarehouseCode = oRecordset.Fields.Item("WhsCode").Value & ""
                        '        oGoodsReceipt.Lines.Price = 1 'oRecordset.Fields.Item("Price").Value & ""
                        '       oGoodsReceipt.Lines.LineTotal = 25 ' oRecordset.Fields.Item("LineTotal").Value & ""

                        oGoodsReceipt.Lines.CostingCode = oRecordset.Fields.Item("U_Office").Value & ""
                        oGoodsReceipt.Lines.CostingCode2 = "DOSUPPD" 'oRecordset.Fields.Item("OcrCode2").Value & ""
                        oGoodsReceipt.Lines.CostingCode3 = "NA1" 'oRecordset.Fields.Item("OcrCode3").Value & ""
                        oGoodsReceipt.Lines.CostingCode4 = "NA" 'oRecordset.Fields.Item("OcrCode4").Value & ""
                        oGoodsReceipt.Lines.CostingCode5 = "" '- THK ' oRecordset.Fields.Item("OcrCode5").Value & ""
                        'oGoodsReceipt.Lines.UserFields.Fields.Item("U_Chamber").Value = oRecordset.Fields.Item("U_Chamber").Value & ""
                        'oGoodsReceipt.Lines.UserFields.Fields.Item("U_Temp").Value = oRecordset.Fields.Item("U_Temp").Value & ""
                        'oGoodsReceipt.Lines.UserFields.Fields.Item("U_Density").Value = oRecordset.Fields.Item("U_Density").Value & ""
                        'oGoodsReceipt.Lines.UserFields.Fields.Item("U_Dip").Value = oRecordset.Fields.Item("U_Dip").Value & ""

                        'oGoodsReceipt.Lines.AccountCode = oRecordset.Fields.Item("AccountCode").Value & ""


                        oGoodsReceipt.Lines.Add()
                        iCount = iCount + 1
                        oRecordset.MoveNext()

                    End While

                    Dim Result As Integer = oGoodsReceipt.Add()

                    If Result <> 0 Then
                        'If Result = -5002 Then

                        'Maanger_CreateGoodsReceipt(DocEntry, ItemCode)
                        'Else
                        __oApplication.StatusBar.SetText("Error: Good Receipt Not Generated -  for Item: " + ItemCode + " :" + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        'End If
                        '  __oApplication.StatusBar.SetText("Error: Good Receipt Not Generated - " + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Else


                        Dim SqlQuery As New StringBuilder

                        'SqlQuery.Append("Update ""@SAP_OIT"" Set ""U_GRDN""= (Select ""DocNum"" From OIGN Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='STITGR' ),")
                        'SqlQuery.Append(" ""U_GRDE""= (Select ""DocEntry"" From OIGN Where ""U_ITDocE""='" + DocEntry + "' And ""U_Flag""='STITGR' )")
                        'SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")

                        SqlQuery.Append("Update ""@SAP_TMD1"" Set ""U_FLAG""= 'Y',")
                        SqlQuery.Append(" ""U_GRDOCE""= (Select top 1 a.""DocEntry"" From OIGN a INNER JOIN IGN1 B ON a.""DocEntry"" =B.""DocEntry"" And ""ItemCode""='" + ItemCode + "' Where ""U_DLDOCENTRY""='" + DocEntry + "' And ""U_Flag""='DIPLOGGR') ,U_TRANSTYPE='GR' ")
                        SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "' and ""U_AdjQty"">0 AND ""U_ItemCode""='" + ItemCode + "'")


                        Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRset1.DoQuery(SqlQuery.ToString)
                        SqlQuery.Clear()

                        SqlQuery.Append("Select top 1 ""DocNum"",a.""DocEntry"" From OIGN a INNER JOIN IGN1 B ON a.""DocEntry"" =B.""DocEntry"" And ""ItemCode""='" + ItemCode + "' Where ""U_DLDOCENTRY""='" + DocEntry + "' And ""U_Flag""='DIPLOGGR'")
                        'SAP_Tranction(FormUID, pVal, BubbleEvent, DocEntry)
                        Dim PostoRs2 As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        PostoRs2.DoQuery(SqlQuery.ToString)


                        __oApplication.StatusBar.SetText("Good Receipt Generated :" + CType(PostoRs2.Fields.Item("DocNum").Value, String), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                        SqlQuery.Clear()
                        Dim SQL1 As String = "Update OIGN  Set ""U_ITDocE""= U_DLDOCENTRY ,""U_ITDocN""=U_DLDOCNUM where ""DocEntry"" =" + CType(PostoRs2.Fields.Item("DocEntry").Value, String)
                        Dim oRset2 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRset2.DoQuery(SQL1)
                    End If

                End If

                Marshal.ReleaseComObject(oRecordset)
                Try
                    Marshal.ReleaseComObject(oGoodsReceipt)
                Catch ex As Exception
                    ' BubbleEvent = False
                End Try



            Catch ex As Exception
                'BubbleEvent = False
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub
        Private Sub ChildItemToPost(ByVal Docentry As String, ByVal itemCode As String)
            Dim PostSQL As String
            PostSQL = "Select distinct ""U_ItemCode"" from ""@SAP_TMD1"" Where ""DocEntry""='" + Docentry.ToString + "' and ""U_ItemCode""='" & itemCode & "' and ""U_AdjQty"">0 and IFNULL(U_FLAG,'N')<>'Y' AND IFNULL(U_TRANSTYPE,'')<>'GR'"
            'SAP_Tranction(FormUID, pVal, BubbleEvent, DocEntry)

            Dim PostoRs2 As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            PostoRs2.DoQuery(PostSQL)
            If PostoRs2.RecordCount > 0 Then
                __oApplication.StatusBar.SetText("Please wait Good Issue Transaction Processing For Item " + itemCode, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                For i = 0 To PostoRs2.RecordCount - 1
                    CreateGoodsReceipt(Docentry, PostoRs2.Fields.Item("U_ItemCode").Value & "")
                Next
                __oApplication.StatusBar.SetText("Good Issue Transaction Processed For Item " + itemCode, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            End If

            PostSQL = "Select distinct ""U_ItemCode"" from ""@SAP_TMD1"" Where ""DocEntry""='" + Docentry.ToString + "' and ""U_ItemCode""='" & itemCode & "' and ""U_AdjQty""<0 and IFNULL(U_FLAG,'N')<>'Y' AND IFNULL(U_TRANSTYPE,'')<>'GI'"
            'SAP_Tranction(FormUID, pVal, BubbleEvent, DocEntry)
            Dim PostoRs1 As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            PostoRs1.DoQuery(PostSQL)
            If PostoRs1.RecordCount > 0 Then
                __oApplication.StatusBar.SetText("Please wait Good Reciept Transaction Processing For Item " + itemCode, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                For i = 0 To PostoRs1.RecordCount - 1
                    CreateGoodsIssue(Docentry, PostoRs1.Fields.Item("U_ItemCode").Value & "")
                Next
                __oApplication.StatusBar.SetText("Good Reciept Transaction Processed For Item " + itemCode, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
            End If


        End Sub
        Private Sub CreateGoodsIssue(ByVal DocEntry As String, ByVal ItemCode As String)
            Try
                ' BubbleEvent = True
                ' oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim errItemCodes As String = ""
                Dim errMachineNos As String = ""

                Dim LineID As String = ""
                Dim errRowFlag As Boolean = False

                Dim oGoodsIssue As SAPbobsCOM.Documents = Nothing
                Dim oRecordset As SAPbobsCOM.Recordset = Nothing
                Dim oRecordset1 As SAPbobsCOM.Recordset = Nothing
                Dim oRs1 As SAPbobsCOM.Recordset = Nothing

                Dim ErrorCode As Integer
                Dim ErrMessage As String
                Dim iCount As Integer = 1



                Dim Sql As String = ""
                Sql = "CALL SAP_GET_GoodIssue_DipLog('" + DocEntry + "','" + ItemCode + "','GI')"

                oRecordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRecordset.DoQuery(Sql)

                If oRecordset.RecordCount > 0 Then

                    oGoodsIssue = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit)
                    Dim loccode As String = oForm.Items.Item("Item_29").Specific.Value
                    Sql = "select TOP 1 ""U_PDate"" from ""@SAP_OTMD"" where ""U_Status""='C' and ""U_LocCode""='" + loccode + "' order by ""U_PDate"" desc"

                    oRecordset1 = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    oRecordset1.DoQuery(Sql)

                    oGoodsIssue.DocDate = oRecordset1.Fields.Item("U_PDate").Value & ""
                    oGoodsIssue.TaxDate = oRecordset1.Fields.Item("U_PDate").Value & ""
                    oGoodsIssue.DocDueDate = oRecordset1.Fields.Item("U_PDate").Value & ""
                    'oGoodsIssue.Reference1 = oRecordset.Fields.Item("U_RefNo").Value & ""
                    oGoodsIssue.Comments = "From Dip Log Book " & oRecordset.Fields.Item("DocEntry").Value & "" ' oRecordset.Fields.Item("U_Remark").Value & ""
                    oGoodsIssue.BPL_IDAssignedToInvoice = CInt(oRecordset.Fields.Item("BPLId").Value) & ""
                    ' oGoodsIssue.BPLName = oRecordset.Fields.Item("BPLName").Value & ""
                    oGoodsIssue.UserFields.Fields.Item("U_DLDOCENTRY").Value = oRecordset.Fields.Item("DocEntry").Value & ""
                    oGoodsIssue.UserFields.Fields.Item("U_DLDOCNUM").Value = oRecordset.Fields.Item("DocNum").Value & ""
                    oGoodsIssue.UserFields.Fields.Item("U_Flag").Value = "DIPLOGGI" & ""
                    oGoodsIssue.Series = oRecordset.Fields.Item("Series").Value & ""





                    'Adding Row level Data
                    While oRecordset.EoF = False

                        oGoodsIssue.Lines.ItemCode = oRecordset.Fields.Item("U_ItemCode").Value & ""
                        oGoodsIssue.Lines.Quantity = oRecordset.Fields.Item("Qty").Value * -1
                        oGoodsIssue.Lines.WarehouseCode = oRecordset.Fields.Item("WhsCode").Value & ""

                        oGoodsIssue.Lines.CostingCode = oRecordset.Fields.Item("U_Office").Value & ""
                        oGoodsIssue.Lines.CostingCode2 = "DOSUPPD"
                        oGoodsIssue.Lines.CostingCode3 = "NA1"
                        oGoodsIssue.Lines.CostingCode4 = "NA"

                        ''oGoodsIssue.Lines.UserFields.Fields.Item("U_Chamber").Value = oRecordset.Fields.Item("U_Chamber").Value & ""


                        'oGoodsIssue.Lines.UserFields.Fields.Item("U_Temp").Value = oRecordset.Fields.Item("U_Temp").Value & ""
                        'oGoodsIssue.Lines.UserFields.Fields.Item("U_Density").Value = oRecordset.Fields.Item("U_Density").Value & ""
                        'oGoodsIssue.Lines.UserFields.Fields.Item("U_Dip").Value = oRecordset.Fields.Item("U_Dip").Value & ""
                        '    oGoodsIssue.Lines.AccountCode = oRecordset.Fields.Item("AccountCode").Value & ""


                        oGoodsIssue.Lines.Add()
                        iCount = iCount + 1
                        oRecordset.MoveNext()

                    End While

                    Dim Result As Integer = oGoodsIssue.Add()

                    If Result <> 0 Then

                        '  If Result = -5002 Then

                        'Maanger_CreateGoodsIssue(DocEntry, ItemCode)
                        ' Else
                        __oApplication.StatusBar.SetText("Error: Good Issue Not Generated - for Item " + ItemCode + " :" + bobCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        'End If


                    Else


                        Dim SqlQuery As New StringBuilder

                        SqlQuery.Append("Update ""@SAP_TMD1"" Set ""U_FLAG""= 'Y',")
                        SqlQuery.Append(" ""U_GIDOCE""= (Select TOP 1 A.""DocEntry"" From OIGE A INNER JOIN IGE1 B ON A.""DocEntry""= B.""DocEntry""  AND ""ItemCode""='" + ItemCode + "'  Where ""U_DLDOCENTRY""='" + DocEntry + "' And ""U_Flag""='DIPLOGGI'  ), U_TRANSTYPE='GI'")
                        SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "' and ""U_AdjQty""<0 AND ""U_ItemCode""='" + ItemCode + "'")


                        Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRset1.DoQuery(SqlQuery.ToString)
                        SqlQuery.Clear()
                        SqlQuery.Append("Select TOP 1  A.""DocNum"" ,A.""DocEntry"" From OIGE A INNER JOIN IGE1 B ON A.""DocEntry""= B.""DocEntry""  AND ""ItemCode""='" + ItemCode + "'  Where ""U_DLDOCENTRY""='" + DocEntry + "' And ""U_Flag""='DIPLOGGI' ")
                        oRset1.DoQuery(SqlQuery.ToString)
                        __oApplication.StatusBar.SetText("Good Issue Generated :-" + CType(oRset1.Fields.Item("DocNum").Value, String), SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)

                        SqlQuery.Clear()
                        Dim Sql1 As String = "Update OIGE  Set ""U_ITDocE""= U_DLDOCENTRY ,""U_ITDocN""=U_DLDOCNUM where ""DocEntry"" =" + CType(oRset1.Fields.Item("DocEntry").Value, String)
                        Dim oRset2 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRset2.DoQuery(Sql1)
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
        Private Sub Maanger_CreateGoodsIssue(ByVal DocEntry As String, ByVal ItemCode As String)
            Try
                ' BubbleEvent = True
                '  oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)


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
                    Sql = "CALL SAP_GET_GoodIssue_DipLog('" + DocEntry + "','" + ItemCode + "','GI')"

                        oRecordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    oRecordset.DoQuery(Sql)

                    If oRecordset.RecordCount > 0 Then

                        oGoodsIssue = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInventoryGenExit)

                        oGoodsIssue.DocDate = oRecordset.Fields.Item("U_PDate").Value & ""
                        oGoodsIssue.TaxDate = oRecordset.Fields.Item("U_PDate").Value & ""
                        oGoodsIssue.DocDueDate = oRecordset.Fields.Item("U_PDate").Value & ""
                        'oGoodsIssue.Reference1 = oRecordset.Fields.Item("U_RefNo").Value & ""
                        oGoodsIssue.Comments = "From Dip Log Book " & oRecordset.Fields.Item("DocEntry").Value & "" ' oRecordset.Fields.Item("U_Remark").Value & ""
                        oGoodsIssue.BPL_IDAssignedToInvoice = CInt(oRecordset.Fields.Item("BPLId").Value) & ""
                        ' oGoodsIssue.BPLName = oRecordset.Fields.Item("BPLName").Value & ""
                        oGoodsIssue.UserFields.Fields.Item("U_DLDOCENTRY").Value = oRecordset.Fields.Item("DocEntry").Value & ""
                        oGoodsIssue.UserFields.Fields.Item("U_DLDOCNUM").Value = oRecordset.Fields.Item("DocNum").Value & ""
                        oGoodsIssue.UserFields.Fields.Item("U_Flag").Value = "DIPLOG" & ""
                        oGoodsIssue.Series = oRecordset.Fields.Item("Series").Value & ""





                        'Adding Row level Data
                        While oRecordset.EoF = False

                            oGoodsIssue.Lines.ItemCode = oRecordset.Fields.Item("U_ItemCode").Value & ""
                            oGoodsIssue.Lines.Quantity = oRecordset.Fields.Item("Qty").Value * -1
                            oGoodsIssue.Lines.WarehouseCode = oRecordset.Fields.Item("WhsCode").Value & ""

                            oGoodsIssue.Lines.CostingCode = oRecordset.Fields.Item("U_Office").Value & ""
                            oGoodsIssue.Lines.CostingCode2 = "DOSUPPD"
                            oGoodsIssue.Lines.CostingCode3 = "NA1"
                            oGoodsIssue.Lines.CostingCode4 = "NA"

                            ''oGoodsIssue.Lines.UserFields.Fields.Item("U_Chamber").Value = oRecordset.Fields.Item("U_Chamber").Value & ""


                            'oGoodsIssue.Lines.UserFields.Fields.Item("U_Temp").Value = oRecordset.Fields.Item("U_Temp").Value & ""
                            'oGoodsIssue.Lines.UserFields.Fields.Item("U_Density").Value = oRecordset.Fields.Item("U_Density").Value & ""
                            'oGoodsIssue.Lines.UserFields.Fields.Item("U_Dip").Value = oRecordset.Fields.Item("U_Dip").Value & ""
                            '    oGoodsIssue.Lines.AccountCode = oRecordset.Fields.Item("AccountCode").Value & ""


                            oGoodsIssue.Lines.Add()
                            iCount = iCount + 1
                            oRecordset.MoveNext()

                        End While

                        Dim Result As Integer = oGoodsIssue.Add()

                        If Result <> 0 Then
                            __oApplication.StatusBar.SetText("Error: Good Issue Not Generated - " + oCompany.GetLastErrorDescription, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                        Else


                            Dim SqlQuery As New StringBuilder

                            SqlQuery.Append("Update ""@SAP_TMD1"" Set ""U_FLAG""= 'Y',")
                            SqlQuery.Append(" ""U_GIDOCE""= (Select TOP 1 ""DocEntry"" From OIGE A INNER JOIN IGE1 B ON A.""DocEntry""= B.""DocEntry""  AND ""ItemCode""='" + ItemCode + "'  Where ""U_DLDOCENTRY""='" + DocEntry + "' And ""U_Flag""='DIPLOG'  ), U_TRANSTYPE='GI'")
                            SqlQuery.Append("Where ""DocEntry""='" + DocEntry + "'")


                            Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                            oRset1.DoQuery(SqlQuery.ToString)

                            SqlQuery.Append("(Select TOP 1 ""DocNum"" From OIGE A INNER JOIN IGE1 B ON A.""DocEntry""= B.""DocEntry""  AND ""ItemCode""='" + ItemCode + "'  Where ""U_DLDOCENTRY""='" + DocEntry + "' And ""U_Flag""='DIPLOG' ")
                            oRset1.DoQuery(SqlQuery.ToString)
                            __oApplication.StatusBar.SetText("Good Issue Generated :-" + oRset1.Fields.Item("DocNum").Value, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)




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
        Private Sub PostSAP_OnAfterItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                If oForm.Mode <> BoFormMode.fm_OK_MODE Then
                    __oApplication.MessageBox("Form should be OK mode", 1, "Ok", "", "")
                    BubbleEvent = False
                    Exit Sub
                End If
                Dim DocEntry As String = oForm.Items.Item("Item_27").Specific.Value
                Dim Status As String = oForm.Items.Item("Item_16").Specific.Value
                Dim PostSQL As String

                If Status = "O" Then



                    ''PostSQL = "Update ""@SAP_OIT"" Set ""U_PFlag""='Y' Where ""DocEntry""='" + DocEntry.ToString + "'"
                    'Dim PostoRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    'PostoRs.DoQuery(PostSQL)

                    PostSQL = "Select  DISTINCT ""U_ItemCode"" from ""@SAP_TMD1"" Where ""DocEntry""='" + DocEntry.ToString + "'"
                    'SAP_Tranction(FormUID, pVal, BubbleEvent, DocEntry)
                    Dim PostoRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    PostoRs.DoQuery(PostSQL)
                    If PostoRs.RecordCount > 0 Then
                        For i = 0 To PostoRs.RecordCount - 1
                            Dim itemCode = PostoRs.Fields.Item("U_ItemCode").Value
                            ChildItemToPost(DocEntry, itemCode)
                            PostoRs.MoveNext()
                        Next
                        __oApplication.StatusBar.SetText("SAP Posting Done successfully ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success)
                    Else
                        __oApplication.MessageBox("Before Proceed ! Please save the Record", 1, "Ok", "", "")
                    End If
                Else
                    __oApplication.StatusBar.SetText("SAP Posting not required while closing  ", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success)
                End If
                'If DocEntry.ToString <> "" Then
                '    Dim TransType As String
                '    Dim TransTypeQuery As String = ""
                '    TransTypeQuery = "CALL SAP_GET_TransctionType ('" + DocEntry.ToString + "')"
                '    Dim TransTypeoRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                '    TransTypeoRs.DoQuery(TransTypeQuery)
                '    If TransTypeoRs.RecordCount > 0 Then
                '        TransType = TransTypeoRs.Fields.Item("TransType").Value
                '    End If
                '    ''''''''''''''''''''' This Transction Use For Base On Inventry Transfer Request '''''''''''''''''''''''''''''''
                '    Dim FL, TL, FW, TW, FB, TB, PODE, ITDE, GRN, GI, GR, TGI, INV, IT, TTLGIDN, INVTYPE, LCJ, AJE1, AJE2 As String
                '    Dim LS As Int64
                '    If TransType = "BOIR" Then
                '        Dim Query As String = ""
                '        Query = "CALL SAP_GET_Condition_Inventory_Transction ('" + DocEntry.ToString + "')"
                '        Dim oRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                '        oRs.DoQuery(Query)
                '        If oRs.RecordCount > 0 Then
                '            FL = oRs.Fields.Item("FromLocation").Value
                '            TL = oRs.Fields.Item("ToLcation").Value
                '            FW = oRs.Fields.Item("U_FW").Value
                '            TW = oRs.Fields.Item("U_TW").Value
                '            FB = oRs.Fields.Item("U_FB").Value
                '            TB = oRs.Fields.Item("U_TB").Value

                '        End If

                '        '   If FL = TL And FB = TB 
                '        If FL = TL Then
                '            IT = GetValue("SELECT T0.""U_ITDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITDE")
                '            If IT = "" Then
                '                Normal_InventoryTransfer(FormUID, pVal, BubbleEvent, DocEntry)
                '            End If
                '        End If

                '        'If FL <> TL And FB <> TB Then
                '        If FL <> TL Then
                '            PODE = GetValue("SELECT  T0.""U_PODE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_PODE")
                '            If PODE = "" Then
                '                Dim LCAmt As Decimal = GetValue("SELECT Sum(IFNULL(T0.""U_Amt"",0)) AS ""LC"" FROM ""@SAP_IT2""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "LC")
                '                If LCAmt > 0 Then
                '                    PurchaseOrder_Creation(FormUID, pVal, BubbleEvent, DocEntry)
                '                End If

                '            End If

                '            IT = GetValue("SELECT T0.""U_ITDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITDE")
                '            If IT = "" Then
                '                Normal_InventoryTransfer(FormUID, pVal, BubbleEvent, DocEntry)
                '            End If

                '            ITDE = GetValue("SELECT T0.""U_ITDocE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITDocE")
                '            If ITDE <> "" Then



                '                Dim ItemCode As String = GetValue("Select TOP 1 ""U_ItemCode"" From  ""@SAP_IT1""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' And IFNULL(""U_ItemCode"" ,'')<>'' ", "U_ItemCode")
                '                Dim ItemGrp As String = GetValue("Select ""ItmsGrpNam"" from OITB Where ""ItmsGrpCod"" =(SELECT T0.""ItmsGrpCod"" FROM OITM T0 WHERE T0.""ItemCode"" ='" + ItemCode + "')", "ItmsGrpNam")
                '                If ItemGrp = "Trading" Then
                '                    Dim QC As String = GetValue("SELECT T0.""U_QC"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_QC")
                '                    If QC = "A" Then



                '                        GI = GetValue("SELECT T0.""U_GIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GIDE")
                '                        If GI = "" Then
                '                            CreateGoodsIssue(FormUID, pVal, BubbleEvent, DocEntry)
                '                        End If
                '                        GI = GetValue("SELECT T0.""U_GIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GIDE")
                '                        If GI <> "" Then



                '                            GR = GetValue("SELECT T0.""U_GRDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GRDE")
                '                            If GR = "" Then
                '                                CreateGoodsReceipt(FormUID, pVal, BubbleEvent, DocEntry)
                '                            End If

                '                            GRN = GetValue("SELECT T0.""U_GRNDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GRNDE")
                '                            If GRN = "" Then
                '                                GoodReceiptPO_Creation(FormUID, pVal, BubbleEvent, DocEntry)
                '                            End If


                '                            'LS = 0
                '                            'LS = GetValue("SELECT Count(T0.""DocEntry"") AS COUNT FROM ""@SAP_IT4""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' AND IFNULL(T0.""U_ItemCode"",'')<>''", "COUNT")
                '                            'If LS <= 0 Then
                '                            '    LossCalCulation(FormUID, pVal, BubbleEvent, DocEntry, "ST")
                '                            'End If

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

                '                    Else

                '                        If __oApplication.MessageBox("Transcation not process because of QC Not Approved Please Change the Reject Warehouse .. If You Change Reject Warehouse Then Continue", 1, "Yes", "No", "") = 2 Then
                '                            BubbleEvent = False
                '                            Exit Sub
                '                        Else
                '                            Dim Branch As String = GetValue("Select  ""U_TB"" From  ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' And IFNULL(""U_TB"" ,'')<>'' ", "U_TB")
                '                            Dim Whs As String = GetValue("Select  ""U_TW"" From  ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' And IFNULL(""U_TW"" ,'')<>'' ", "U_TW")
                '                            Dim HWhs As String = GetValue("SELECT T0.""WhsCode"" FROM OWHS T0 WHERE T0.""U_Category"" ='Reject'  and  T0.""BPLid"" = '" + Branch + "'", "WhsCode")
                '                            If Whs <> HWhs Then
                '                                __oApplication.MessageBox("Transcation not process because of QC Not Approved Please Change the Reject Warehouse..", 1, "Ok", "", "")
                '                                BubbleEvent = False
                '                                Exit Sub
                '                            Else


                '                                GI = GetValue("SELECT T0.""U_GIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GIDE")
                '                                If GI = "" Then
                '                                    CreateGoodsIssue(FormUID, pVal, BubbleEvent, DocEntry)
                '                                End If
                '                                GI = GetValue("SELECT T0.""U_GIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GIDE")
                '                                If GI <> "" Then



                '                                    GR = GetValue("SELECT T0.""U_GRDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GRDE")
                '                                    If GR = "" Then
                '                                        CreateGoodsReceipt(FormUID, pVal, BubbleEvent, DocEntry)
                '                                    End If

                '                                    GRN = GetValue("SELECT T0.""U_GRNDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GRNDE")
                '                                    If GRN = "" Then
                '                                        GoodReceiptPO_Creation(FormUID, pVal, BubbleEvent, DocEntry)
                '                                    End If


                '                                    'LS = 0
                '                                    'LS = GetValue("SELECT Count(T0.""DocEntry"") AS COUNT FROM ""@SAP_IT4""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' AND IFNULL(T0.""U_ItemCode"",'')<>''", "COUNT")
                '                                    'If LS <= 0 Then
                '                                    '    LossCalCulation(FormUID, pVal, BubbleEvent, DocEntry, "ST")
                '                                    'End If

                '                                    TGI = GetValue("SELECT T0.""U_TGIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TGIDE")
                '                                    If TGI = "" Then
                '                                        Create_TransportationLoss_GoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "ST")
                '                                    End If

                '                                    INV = GetValue("SELECT T0.""U_ARDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ARDE")
                '                                    If INV = "" Then
                '                                        ARInvoice_Creation(FormUID, pVal, BubbleEvent, DocEntry, "ST")
                '                                    End If


                '                                    INVTYPE = GetValue("SELECT T0.""U_ITT"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITT")
                '                                    If INVTYPE = "DE" Then
                '                                        TTLGIDN = GetValue("SELECT T0.""U_TTLGIDN"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TTLGIDN")
                '                                        If TTLGIDN = "" Then
                '                                            TannkTempLoss_CreateGoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "ST")
                '                                        End If
                '                                    End If

                '                                    LCJ = GetValue("SELECT T0.""U_JE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_JE")
                '                                    If LCJ = "" Then
                '                                        LandedCost_JounralEntry(FormUID, pVal, BubbleEvent, DocEntry)
                '                                    End If

                '                                    AJE1 = GetValue("SELECT T0.""U_AJE1"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_AJE1")
                '                                    If AJE1 = "" Then
                '                                        AdjustmentJounralEntry_FromBranch(FormUID, pVal, BubbleEvent, DocEntry)
                '                                    End If

                '                                    AJE2 = GetValue("SELECT T0.""U_AJE2"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_AJE2")
                '                                    If AJE2 = "" Then
                '                                        AdjustmentJounralEntry_ToBranch(FormUID, pVal, BubbleEvent, DocEntry)
                '                                    End If
                '                                End If
                '                            End If
                '                        End If
                '                    End If


                '                Else



                '                    GI = GetValue("SELECT T0.""U_GIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GIDE")
                '                    If GI = "" Then
                '                        CreateGoodsIssue(FormUID, pVal, BubbleEvent, DocEntry)
                '                    End If
                '                    GI = GetValue("SELECT T0.""U_GIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GIDE")
                '                    If GI <> "" Then


                '                        GR = GetValue("SELECT T0.""U_GRDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GRDE")
                '                        If GR = "" Then
                '                            CreateGoodsReceipt(FormUID, pVal, BubbleEvent, DocEntry)
                '                        End If

                '                        GRN = GetValue("SELECT T0.""U_GRNDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GRNDE")
                '                        If GRN = "" Then
                '                            GoodReceiptPO_Creation(FormUID, pVal, BubbleEvent, DocEntry)
                '                        End If


                '                        'LS = 0
                '                        'LS = GetValue("SELECT Count(T0.""DocEntry"") AS COUNT FROM ""@SAP_IT4""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' AND IFNULL(T0.""U_ItemCode"",'')<>''", "COUNT")
                '                        'If LS <= 0 Then
                '                        '    LossCalCulation(FormUID, pVal, BubbleEvent, DocEntry, "ST")
                '                        'End If

                '                        TGI = GetValue("SELECT T0.""U_TGIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TGIDE")
                '                        If TGI = "" Then
                '                            Create_TransportationLoss_GoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "ST")
                '                        End If

                '                        INV = GetValue("SELECT T0.""U_ARDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ARDE")
                '                        If INV = "" Then
                '                            ARInvoice_Creation(FormUID, pVal, BubbleEvent, DocEntry, "ST")
                '                        End If


                '                        INVTYPE = GetValue("SELECT T0.""U_ITT"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITT")
                '                        If INVTYPE = "DE" Then
                '                            TTLGIDN = GetValue("SELECT T0.""U_TTLGIDN"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TTLGIDN")
                '                            If TTLGIDN = "" Then
                '                                TannkTempLoss_CreateGoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "ST")
                '                            End If
                '                        End If

                '                        LCJ = GetValue("SELECT T0.""U_JE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_JE")
                '                        If LCJ = "" Then
                '                            LandedCost_JounralEntry(FormUID, pVal, BubbleEvent, DocEntry)
                '                        End If

                '                        AJE1 = GetValue("SELECT T0.""U_AJE1"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_AJE1")
                '                        If AJE1 = "" Then
                '                            AdjustmentJounralEntry_FromBranch(FormUID, pVal, BubbleEvent, DocEntry)
                '                        End If

                '                        AJE2 = GetValue("SELECT T0.""U_AJE2"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_AJE2")
                '                        If AJE2 = "" Then
                '                            AdjustmentJounralEntry_ToBranch(FormUID, pVal, BubbleEvent, DocEntry)
                '                        End If
                '                    End If
                '                End If

                '                Try
                '                    Dim SqlQuery As New StringBuilder

                '                    SqlQuery.Append("Update ""@SAP_OITR"" Set ""U_ITDocN""= (Select ""DocNum"" From ""@SAP_OIT"" Where ""DocEntry""='" + DocEntry + "'  ),")
                '                    SqlQuery.Append(" ""U_ITDocE""= (Select ""DocEntry"" From ""@SAP_OIT"" Where ""DocEntry""='" + DocEntry + "' )")
                '                    SqlQuery.Append("Where ""DocEntry""='" + GetValue("SELECT T0.""U_ITRDocE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITRDocE") + "'")


                '                    Dim oRset1 As SAPbobsCOM.Recordset = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                '                    oRset1.DoQuery(SqlQuery.ToString)
                '                Catch ex As Exception

                '                End Try
                '            End If



                '        End If
                '        ''''''''''''''''''''' This Transction Use For Base On Inventry Transfer Request '''''''''''''''''''END'''''''''''''''''''''''''''''''''''''''

                '        ''''''''''''''''''''' This Transction Use For Base On GRN Start '''''''''''''''''''END'''''''''''''''''''''''''''''''''''''''
                '    ElseIf TransType = "BOGRN" Then


                '        Dim ItemCode As String = GetValue("Select TOP 1 ""U_ItemCode"" From  ""@SAP_IT1""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' And IFNULL(""U_ItemCode"" ,'')<>'' ", "U_ItemCode")

                '        Dim ItemGrp As String = GetValue("Select ""ItmsGrpNam"" from OITB Where ""ItmsGrpCod"" =(SELECT T0.""ItmsGrpCod"" FROM OITM T0 WHERE T0.""ItemCode"" ='" + ItemCode + "')", "ItmsGrpNam")
                '        If ItemGrp = "Trading" Then
                '            Dim QC As String = GetValue("SELECT T0.""U_QC"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_QC")
                '            If QC = "A" Then
                '                GRN = GetValue("SELECT T0.""U_GRNDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GRNDE")
                '                If GRN = "" Then
                '                    Purchase_GoodReceiptPO_Creation(FormUID, pVal, BubbleEvent, DocEntry)
                '                End If


                '                'LS = 0
                '                'LS = GetValue("SELECT Count(T0.""DocEntry"") AS COUNT FROM ""@SAP_IT4""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' AND IFNULL(T0.""U_ItemCode"",'')<>''", "COUNT")
                '                'If LS <= 0 Then
                '                '    LossCalCulation(FormUID, pVal, BubbleEvent, DocEntry, "PF")
                '                'End If

                '                TGI = GetValue("SELECT T0.""U_TGIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TGIDE")
                '                If TGI = "" Then
                '                    Create_TransportationLoss_GoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "PF")
                '                End If

                '                INV = GetValue("SELECT T0.""U_ARDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ARDE")
                '                If INV = "" Then
                '                    ARInvoice_Creation(FormUID, pVal, BubbleEvent, DocEntry, "PF")
                '                End If

                '                INVTYPE = GetValue("SELECT T0.""U_ITT"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITT")
                '                If INVTYPE = "DE" Then
                '                    TTLGIDN = GetValue("SELECT T0.""U_TTLGIDN"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TTLGIDN")
                '                    If TTLGIDN = "" Then
                '                        TannkTempLoss_CreateGoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "PF")
                '                    End If
                '                End If


                '                IT = GetValue("SELECT T0.""U_ITDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITDE")
                '                If IT = "" Then
                '                    Normal_InventoryTransfer(FormUID, pVal, BubbleEvent, DocEntry)
                '                End If
                '            Else

                '                If __oApplication.MessageBox("Transcation not process because of QC Not Approved Please Change the Reject Warehouse .. If You Change Reject Warehouse Then Continua", 1, "Yes", "No", "") = 2 Then
                '                    BubbleEvent = False
                '                    Exit Sub
                '                Else
                '                    Dim Branch As String = GetValue("Select  ""U_TB"" From  ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' And IFNULL(""U_TB"" ,'')<>'' ", "U_TB")
                '                    Dim Whs As String = GetValue("Select  ""U_TW"" From  ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' And IFNULL(""U_TW"" ,'')<>'' ", "U_TW")
                '                    Dim HWhs As String = GetValue("SELECT T0.""WhsCode"" FROM OWHS T0 WHERE T0.""U_Category"" ='Reject'  and  T0.""BPLid"" = '" + Branch + "'", "WhsCode")

                '                    If Whs <> HWhs Then
                '                        __oApplication.MessageBox("Transcation not process because of QC Not Approved Please Change the Reject Warehouse..", 1, "Ok", "", "")
                '                        BubbleEvent = False
                '                        Exit Sub
                '                    Else
                '                        GRN = GetValue("SELECT T0.""U_GRNDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GRNDE")
                '                        If GRN = "" Then
                '                            Purchase_GoodReceiptPO_Creation(FormUID, pVal, BubbleEvent, DocEntry)
                '                        End If


                '                        'LS = 0
                '                        'LS = GetValue("SELECT Count(T0.""DocEntry"") AS COUNT FROM ""@SAP_IT4""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' AND IFNULL(T0.""U_ItemCode"",'')<>''", "COUNT")
                '                        'If LS <= 0 Then
                '                        '    LossCalCulation(FormUID, pVal, BubbleEvent, DocEntry, "PF")
                '                        'End If

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
                '                    End If
                '                End If


                '            End If
                '        Else
                '            GRN = GetValue("SELECT T0.""U_GRNDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_GRNDE")
                '            If GRN = "" Then
                '                Purchase_GoodReceiptPO_Creation(FormUID, pVal, BubbleEvent, DocEntry)
                '            End If


                '            'LS = 0
                '            'LS = GetValue("SELECT Count(T0.""DocEntry"") AS COUNT FROM ""@SAP_IT4""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "' AND IFNULL(T0.""U_ItemCode"",'')<>''", "COUNT")
                '            'If LS <= 0 Then
                '            '    LossCalCulation(FormUID, pVal, BubbleEvent, DocEntry, "PF")
                '            'End If

                '            TGI = GetValue("SELECT T0.""U_TGIDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TGIDE")
                '            If TGI = "" Then
                '                Create_TransportationLoss_GoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "PF")
                '            End If

                '            INV = GetValue("SELECT T0.""U_ARDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ARDE")
                '            If INV = "" Then
                '                ARInvoice_Creation(FormUID, pVal, BubbleEvent, DocEntry, "PF")
                '            End If

                '            INVTYPE = GetValue("SELECT T0.""U_ITT"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITT")
                '            If INVTYPE = "DE" Then
                '                TTLGIDN = GetValue("SELECT T0.""U_TTLGIDN"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_TTLGIDN")
                '                If TTLGIDN = "" Then
                '                    TannkTempLoss_CreateGoodsIssue(FormUID, pVal, BubbleEvent, DocEntry, "PF")
                '                End If
                '            End If


                '            IT = GetValue("SELECT T0.""U_ITDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITDE")
                '            If IT = "" Then
                '                Normal_InventoryTransfer(FormUID, pVal, BubbleEvent, DocEntry)
                '            End If

                '        End If

                '    End If


                '    '''This Code For QC Sample Type 
                '    '''''''''''''''''''''
                '    Dim QCTYPE As String = GetValue("SELECT T0.""U_ITT"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITT")
                '    Dim QCNO As String = GetValue("SELECT T0.""U_QCNO"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_QCNO")

                '    If QCTYPE = "S" And QCNO <> "" Then

                '        IT = GetValue("SELECT T0.""U_ITDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITDE")
                '        If IT = "" Then
                '            Normal_InventoryTransfer(FormUID, pVal, BubbleEvent, DocEntry)
                '        End If
                '    End If


                '    '''This Code For Topping 
                '    '''''''''''''''''''''
                '    If QCTYPE = "TP" Or QCTYPE = "S" Then

                '        IT = GetValue("SELECT T0.""U_ITDE"" FROM ""@SAP_OIT""  T0 WHERE T0.""DocEntry"" ='" + DocEntry + "'", "U_ITDE")
                '        If IT = "" Then
                '            Normal_InventoryTransfer(FormUID, pVal, BubbleEvent, DocEntry)
                '        End If
                '    End If



                'End If

                __Application.ActivateMenuItem("1304")
            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
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

                ElseIf pVal.MenuUID = CType(menuID.Find, String) And pVal.BeforeAction = False Then
                    oForm.Items.Item("Item_26").Enabled = True
                End If
            Catch ex As Exception

            End Try
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


                oForm.Items.Item("Item_27").Specific.value = GenrateDocEntry("DocEntry", "@SAP_OTMD")
                oForm.Items.Item("Item_25").Specific.value = GenrateDocEntry("DocEntry", "@SAP_OTMD")


                'Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                'AddRowInMatrix(oForm, "@SAP_TMD1", "m1")

                Dim Loc As String = GetValue("SELECT T0.""U_Location"" FROM OUSR T0 WHERE T0.""USERID"" ='" + __bobCompany.UserSignature.ToString + "' And  T0.""U_A"" ='Y'", "U_Location")
                Loc = GetValue("SELECT T0.""U_Location"" FROM OUSR T0 WHERE T0.""USERID"" ='" + __bobCompany.UserSignature.ToString + "' And  T0.""U_A"" ='Y'", "U_Location")

                If Loc = "" Then
                    __oApplication.MessageBox("Please Configure  Location ", 1, "Ok", "", "")
                    oForm.Close()
                    Exit Sub
                End If


                'Dim Query As String = ""
                'Query = "CALL SAP_GET_TempMaster_Validation ('" + Loc + "')"
                'Dim oRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                'oRs.DoQuery(Query)
                'If oRs.RecordCount > 0 Then
                '    If oRs.Fields.Item("Count").Value >= 0 Then
                '        If __oApplication.MessageBox("Temperature Master Today Date Information allready exits. Do you want to open exiting record ", 1, "Yes", "No", "") = 2 Then
                '            oForm.Close()
                '            Exit Sub
                '        Else
                '            oForm.Mode = BoFormMode.fm_FIND_MODE
                '            oForm.Items.Item("Item_27").Specific.Value = oRs.Fields.Item("DocEntry").Value
                '            oForm.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular)
                '        End If

                '    End If


                'End If

                oForm.Items.Item("Item_29").Specific.Value = Loc
                oForm.Items.Item("Item_8").Specific.Value = GetValue("SELECT TOP 1 T1.""BPLId"" FROM OUSR T0  INNER JOIN USR6 T1 ON T0.""USER_CODE"" = T1.""UserCode"" WHERE T0.""USERID"" ='" + __bobCompany.UserSignature.ToString + "'  And  T0.""U_A"" ='Y'", "BPLId")
                oForm.Items.Item("Item_19").Click()


                'oForm.Items.Item("Item_26").Enabled = False



                oForm.Freeze(False)

            Catch ex As Exception
                oForm.Freeze(False)
            End Try
        End Sub


        Private Sub Whs_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OTMD")

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                dbsrc.SetValue("U_WhsCode", dbsrc.Offset, oDataTable.GetValue("WhsCode", 0) & "")
                                dbsrc.SetValue("U_WhsName", dbsrc.Offset, oDataTable.GetValue("WhsName", 0) & "")


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

        Private Sub Location_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OTMD")

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                dbsrc.SetValue("U_LocCode", dbsrc.Offset, oDataTable.GetValue("Code", 0) & "")
                                dbsrc.SetValue("U_LocName", dbsrc.Offset, oDataTable.GetValue("Location", 0) & "")
                                Fill_Matrix(FormUID, pVal, BubbleEvent, oDataTable.GetValue("Code", 0))

                                oForm.Items.Item("Item_25").Click()
                                oForm.Items.Item("Item_29").Enabled = False

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

        Private Sub Branch_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_OTMD")

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then

                                dbsrc.SetValue("U_BPLId", dbsrc.Offset, oDataTable.GetValue("BPLId", 0) & "")
                                dbsrc.SetValue("U_BPLName", dbsrc.Offset, oDataTable.GetValue("BPLName", 0) & "")

                                oForm.Items.Item("Item_25").Click()
                                oForm.Items.Item("Item_8").Enabled = False

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

        Private Sub OilDip1_OnAfterLocstFocus(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim Flag As Boolean = True
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                Dim oRowCtrl As SAPbouiCOM.CommonSetting
                oRowCtrl = m1.CommonSetting()
                oRowCtrl.SetCellEditable(pVal.Row, 8, True)
                'oRowCtrl.SetCellEditable(pVal.Row, 9, True)
                oRowCtrl.SetCellEditable(pVal.Row, 11, True)
                oRowCtrl.SetCellEditable(pVal.Row, 12, True)
                oRowCtrl.SetCellEditable(pVal.Row, 13, True)
                oRowCtrl.SetCellEditable(pVal.Row, 14, True)

                Dim TQty, WQty, OQty As Decimal
                TQty = 0
                WQty = 0
                OQty = 0

                Dim Min As Decimal = GetValue("Select IFNULL(MIN(T1.""U_Hight""),0) As ""Min"" from ""@SAP_OCALP""  T0 Inner Join ""@SAP_CALP2""  T1 On T0.""DocEntry""=T1.""DocEntry"" Where  IFNULL(T1.""U_CHN"",0)<>0 AND  T0.""U_FAC""='" + m1.Columns.Item("Col_2").Cells.Item(pVal.Row).Specific.Value + "'", "Min")
                Dim Max As Decimal = GetValue("Select IFNULL(MAX(T1.""U_Hight""),0) As ""Max"" from ""@SAP_OCALP""  T0 Inner Join ""@SAP_CALP2""  T1 On T0.""DocEntry""=T1.""DocEntry"" Where IFNULL(T1.""U_CHN"",0)<>0 AND T0.""U_FAC""='" + m1.Columns.Item("Col_2").Cells.Item(pVal.Row).Specific.Value + "'", "Max")


                If CDec(m1.Columns.Item("Col_7").Cells.Item(pVal.Row).Specific.Value) < Min Or CDec(m1.Columns.Item("Col_7").Cells.Item(pVal.Row).Specific.Value) > Max Then


                    If Flag = True Then
                        __oApplication.MessageBox("Over All Dip Required in following rang (Min: '" + Min.ToString + "' And Max: '" + Max.ToString + "')", 1, "Ok", "", "")
                        m1.Columns.Item("Col_7").Cells.Item(pVal.Row).Specific.Value = "0"
                        BubbleEvent = False
                        Flag = False
                        Exit Sub
                    Else
                        Flag = False
                        BubbleEvent = False
                        Exit Sub
                    End If

                Else

                    Try
                        '------------------------Over All Dip---------
                        Dim QueryOD As String = ""
                        QueryOD = "CALL SAP_GET_TEMP_CAL_QTY ('" + m1.Columns.Item("Col_2").Cells.Item(pVal.Row).Specific.Value + "','" + m1.Columns.Item("Col_7").Cells.Item(pVal.Row).Specific.Value + "')"


                        Dim oRsOD As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRsOD.DoQuery(QueryOD)
                        If oRsOD.RecordCount > 0 Then
                            m1.Columns.Item("Col_14").Cells.Item(pVal.Row).Specific.Value = oRsOD.Fields.Item("Qty").Value
                            TQty = oRsOD.Fields.Item("Qty").Value
                        End If

                    Catch ex As Exception

                    End Try
                End If

                'If CDec(m1.Columns.Item("Col_12").Cells.Item(pVal.Row).Specific.Value) < Min Or CDec(m1.Columns.Item("Col_12").Cells.Item(pVal.Row).Specific.Value) > Max Then

                '    If Flag = True Then
                '        __oApplication.MessageBox("Water Dip Required in following rang (Min: '" + Min.ToString + "' And Max: '" + Max.ToString + "')", 1, "Ok", "", "")
                '        ' m1.Columns.Item("Col_12").Cells.Item(pVal.Row).Specific.Value = "0"
                '        BubbleEvent = False
                '        Flag = False
                '        Exit Sub
                '    Else
                '        Flag = False
                '        BubbleEvent = False
                '        Exit Sub
                '    End If


                'Else
                '    Try
                '        '------------------------Water All Dip---------
                '        Dim QueryWD As String = ""
                '        QueryWD = "CALL SAP_GET_TEMP_CAL_QTY ('" + m1.Columns.Item("Col_2").Cells.Item(pVal.Row).Specific.Value + "','" + m1.Columns.Item("Col_12").Cells.Item(pVal.Row).Specific.Value + "')"


                '        Dim oRsWD As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                '        oRsWD.DoQuery(QueryWD)
                '        If oRsWD.RecordCount > 0 Then
                '            m1.Columns.Item("Col_15").Cells.Item(pVal.Row).Specific.Value = oRsWD.Fields.Item("Qty").Value
                '            WQty = oRsWD.Fields.Item("Qty").Value
                '        End If

                '    Catch ex As Exception

                '    End Try
                'End If


                Try
                    '------------------------Water All Dip---------
                    Dim QueryWD As String = ""
                    QueryWD = "CALL SAP_GET_TEMP_CAL_QTY_WATER ('" + m1.Columns.Item("Col_2").Cells.Item(pVal.Row).Specific.Value + "','" + m1.Columns.Item("Col_12").Cells.Item(pVal.Row).Specific.Value + "')"


                    Dim oRsWD As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    oRsWD.DoQuery(QueryWD)
                    If oRsWD.RecordCount > 0 Then
                        m1.Columns.Item("Col_15").Cells.Item(pVal.Row).Specific.Value = oRsWD.Fields.Item("Qty").Value
                        WQty = oRsWD.Fields.Item("Qty").Value
                    End If

                Catch ex As Exception

                End Try




                OQty = TQty - WQty

                m1.Columns.Item("Col_8").Cells.Item(pVal.Row).Specific.Value = OQty

                m1.Columns.Item("Col_10").Cells.Item(pVal.Row).Specific.Value = CDec(m1.Columns.Item("Col_8").Cells.Item(pVal.Row).Specific.Value) - CDec(m1.Columns.Item("Col_9").Cells.Item(pVal.Row).Specific.Value)
                m1.Columns.Item("Col_16").Cells.Item(pVal.Row).Specific.Value = CDec(m1.Columns.Item("Col_8").Cells.Item(pVal.Row).Specific.Value) - CDec(m1.Columns.Item("Col_9").Cells.Item(pVal.Row).Specific.Value)


                Dim OverDip As Decimal
                Dim WaterDip As Decimal
                If CDec(m1.Columns.Item("Col_7").Cells.Item(pVal.Row).Specific.Value) > 0 Then
                    OverDip = CDec(m1.Columns.Item("Col_7").Cells.Item(pVal.Row).Specific.Value)
                Else
                    OverDip = 0
                End If
                If CDec(m1.Columns.Item("Col_12").Cells.Item(pVal.Row).Specific.Value) > 0 Then
                    WaterDip = CDec(m1.Columns.Item("Col_12").Cells.Item(pVal.Row).Specific.Value)
                Else
                    WaterDip = 0
                End If

                Dim OilDip As Decimal = OverDip - WaterDip
                m1.Columns.Item("Col_13").Cells.Item(pVal.Row).Specific.Value = OilDip


                oRowCtrl.SetCellEditable(pVal.Row, 8, False)
                ' oRowCtrl.SetCellEditable(pVal.Row, 9, False)
                oRowCtrl.SetCellEditable(pVal.Row, 11, False)
                oRowCtrl.SetCellEditable(pVal.Row, 12, False)
                oRowCtrl.SetCellEditable(pVal.Row, 13, False)
                oRowCtrl.SetCellEditable(pVal.Row, 14, False)

                'Try
                '    Dim Query As String = ""
                '    Query = "CALL SAP_GET_TEMP_CAL_QTY ('" + m1.Columns.Item("Col_2").Cells.Item(pVal.Row).Specific.Value + "','" + m1.Columns.Item("Col_13").Cells.Item(pVal.Row).Specific.Value + "')"
                '    Dim oRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                '    oRs.DoQuery(Query)
                '    If oRs.RecordCount > 0 Then
                '        m1.Columns.Item("Col_8").Cells.Item(pVal.Row).Specific.Value = oRs.Fields.Item("Qty").Value

                '        m1.Columns.Item("Col_10").Cells.Item(pVal.Row).Specific.Value = CDec(m1.Columns.Item("Col_8").Cells.Item(pVal.Row).Specific.Value) - CDec(m1.Columns.Item("Col_9").Cells.Item(pVal.Row).Specific.Value)





                '    End If
                'Catch ex As Exception

                'End Try








                'oForm.Items.Item("Item_25").Click()




            Catch ex As Exception
                '__oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub OilDip2_OnAfterLocstFocus(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim Flag As Boolean = True
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                Dim oRowCtrl As SAPbouiCOM.CommonSetting
                oRowCtrl = m1.CommonSetting()
                oRowCtrl.SetCellEditable(pVal.Row, 8, True)
                'oRowCtrl.SetCellEditable(pVal.Row, 9, True)
                oRowCtrl.SetCellEditable(pVal.Row, 11, True)
                oRowCtrl.SetCellEditable(pVal.Row, 12, True)
                oRowCtrl.SetCellEditable(pVal.Row, 13, True)
                oRowCtrl.SetCellEditable(pVal.Row, 14, True)

                Dim TQty, WQty, OQty As Decimal
                TQty = 0
                WQty = 0
                OQty = 0

                Dim Min As Decimal = GetValue("Select IFNULL(MIN(T1.""U_Hight""),0) As ""Min"" from ""@SAP_OCALP""  T0 Inner Join ""@SAP_CALP3""  T1 On T0.""DocEntry""=T1.""DocEntry"" Where IFNULL(T1.""U_CHN"",0)<>0 AND T0.""U_FAC""='" + m1.Columns.Item("Col_2").Cells.Item(pVal.Row).Specific.Value + "'", "Min")
                Dim Max As Decimal = GetValue("Select IFNULL(MAX(T1.""U_Hight""),0) As ""Max"" from ""@SAP_OCALP""  T0 Inner Join ""@SAP_CALP3""  T1 On T0.""DocEntry""=T1.""DocEntry"" Where  IFNULL(T1.""U_CHN"",0)<>0 AND T0.""U_FAC""='" + m1.Columns.Item("Col_2").Cells.Item(pVal.Row).Specific.Value + "'", "Max")


                'If CDec(m1.Columns.Item("Col_7").Cells.Item(pVal.Row).Specific.Value) < Min Or CDec(m1.Columns.Item("Col_7").Cells.Item(pVal.Row).Specific.Value) > Max Then


                '    If Flag = True Then
                '        __oApplication.MessageBox("Over All Dip Required in following rang (Min: '" + Min.ToString + "' And Max: '" + Max.ToString + "')", 1, "Ok", "", "")
                '        ' m1.Columns.Item("Col_7").Cells.Item(pVal.Row).Specific.Value = "0"
                '        BubbleEvent = False
                '        Flag = False
                '        Exit Sub
                '    Else
                '        Flag = False
                '        BubbleEvent = False
                '        Exit Sub
                '    End If

                'Else

                '    Try
                '        '------------------------Over All Dip---------
                '        Dim QueryOD As String = ""
                '        QueryOD = "CALL SAP_GET_TEMP_CAL_QTY ('" + m1.Columns.Item("Col_2").Cells.Item(pVal.Row).Specific.Value + "','" + m1.Columns.Item("Col_7").Cells.Item(pVal.Row).Specific.Value + "')"


                '        Dim oRsOD As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                '        oRsOD.DoQuery(QueryOD)
                '        If oRsOD.RecordCount > 0 Then
                '            m1.Columns.Item("Col_14").Cells.Item(pVal.Row).Specific.Value = oRsOD.Fields.Item("Qty").Value
                '            TQty = oRsOD.Fields.Item("Qty").Value
                '        End If

                '    Catch ex As Exception

                '    End Try
                'End If

                If CDec(m1.Columns.Item("Col_12").Cells.Item(pVal.Row).Specific.Value) < Min Or CDec(m1.Columns.Item("Col_12").Cells.Item(pVal.Row).Specific.Value) > Max Then

                    If Flag = True Then
                        __oApplication.MessageBox("Water Dip Required in following rang (Min: '" + Min.ToString + "' And Max: '" + Max.ToString + "')", 1, "Ok", "", "")
                        m1.Columns.Item("Col_12").Cells.Item(pVal.Row).Specific.Value = "0"
                        BubbleEvent = False
                        Flag = False
                        Exit Sub
                    Else
                        Flag = False
                        BubbleEvent = False
                        Exit Sub
                    End If


                Else
                    Try
                        '------------------------Water All Dip---------
                        Dim QueryWD As String = ""
                        QueryWD = "CALL SAP_GET_TEMP_CAL_QTY_WATER ('" + m1.Columns.Item("Col_2").Cells.Item(pVal.Row).Specific.Value + "','" + m1.Columns.Item("Col_12").Cells.Item(pVal.Row).Specific.Value + "')"


                        Dim oRsWD As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRsWD.DoQuery(QueryWD)
                        If oRsWD.RecordCount > 0 Then
                            m1.Columns.Item("Col_15").Cells.Item(pVal.Row).Specific.Value = oRsWD.Fields.Item("Qty").Value
                            WQty = oRsWD.Fields.Item("Qty").Value
                        End If

                    Catch ex As Exception

                    End Try
                End If



                Try
                    '------------------------Over All Dip---------
                    Dim QueryOD As String = ""
                    QueryOD = "CALL SAP_GET_TEMP_CAL_QTY ('" + m1.Columns.Item("Col_2").Cells.Item(pVal.Row).Specific.Value + "','" + m1.Columns.Item("Col_7").Cells.Item(pVal.Row).Specific.Value + "')"


                    Dim oRsOD As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                    oRsOD.DoQuery(QueryOD)
                    If oRsOD.RecordCount > 0 Then
                        m1.Columns.Item("Col_14").Cells.Item(pVal.Row).Specific.Value = oRsOD.Fields.Item("Qty").Value
                        TQty = oRsOD.Fields.Item("Qty").Value
                    End If

                Catch ex As Exception

                End Try


                OQty = TQty - WQty

                m1.Columns.Item("Col_8").Cells.Item(pVal.Row).Specific.Value = OQty

                m1.Columns.Item("Col_10").Cells.Item(pVal.Row).Specific.Value = CDec(m1.Columns.Item("Col_8").Cells.Item(pVal.Row).Specific.Value) - CDec(m1.Columns.Item("Col_9").Cells.Item(pVal.Row).Specific.Value)
                m1.Columns.Item("Col_16").Cells.Item(pVal.Row).Specific.Value = CDec(m1.Columns.Item("Col_8").Cells.Item(pVal.Row).Specific.Value) - CDec(m1.Columns.Item("Col_9").Cells.Item(pVal.Row).Specific.Value)

                Dim OverDip As Decimal
                Dim WaterDip As Decimal
                If CDec(m1.Columns.Item("Col_7").Cells.Item(pVal.Row).Specific.Value) > 0 Then
                    OverDip = CDec(m1.Columns.Item("Col_7").Cells.Item(pVal.Row).Specific.Value)
                Else
                    OverDip = 0
                End If
                If CDec(m1.Columns.Item("Col_12").Cells.Item(pVal.Row).Specific.Value) > 0 Then
                    WaterDip = CDec(m1.Columns.Item("Col_12").Cells.Item(pVal.Row).Specific.Value)
                Else
                    WaterDip = 0
                End If

                Dim OilDip As Decimal = OverDip - WaterDip
                m1.Columns.Item("Col_13").Cells.Item(pVal.Row).Specific.Value = OilDip

                oRowCtrl.SetCellEditable(pVal.Row, 8, False)
                ' oRowCtrl.SetCellEditable(pVal.Row, 9, False)
                oRowCtrl.SetCellEditable(pVal.Row, 11, False)
                oRowCtrl.SetCellEditable(pVal.Row, 12, False)
                oRowCtrl.SetCellEditable(pVal.Row, 13, False)
                oRowCtrl.SetCellEditable(pVal.Row, 14, False)

                'Try
                '    Dim Query As String = ""
                '    Query = "CALL SAP_GET_TEMP_CAL_QTY ('" + m1.Columns.Item("Col_2").Cells.Item(pVal.Row).Specific.Value + "','" + m1.Columns.Item("Col_13").Cells.Item(pVal.Row).Specific.Value + "')"
                '    Dim oRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                '    oRs.DoQuery(Query)
                '    If oRs.RecordCount > 0 Then
                '        m1.Columns.Item("Col_8").Cells.Item(pVal.Row).Specific.Value = oRs.Fields.Item("Qty").Value

                '        m1.Columns.Item("Col_10").Cells.Item(pVal.Row).Specific.Value = CDec(m1.Columns.Item("Col_8").Cells.Item(pVal.Row).Specific.Value) - CDec(m1.Columns.Item("Col_9").Cells.Item(pVal.Row).Specific.Value)





                '    End If
                'Catch ex As Exception

                'End Try








                ' oForm.Items.Item("Item_25").Click()




            Catch ex As Exception
                '__oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub


        Private Sub OilDIFF_OnAfterLocstFocus(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                Dim oRowCtrl As SAPbouiCOM.CommonSetting
                oRowCtrl = m1.CommonSetting()

                oRowCtrl.SetCellEditable(pVal.Row, 14, True)

                m1.Columns.Item("Col_10").Cells.Item(pVal.Row).Specific.Value = CDec(m1.Columns.Item("Col_8").Cells.Item(pVal.Row).Specific.Value) - CDec(m1.Columns.Item("Col_9").Cells.Item(pVal.Row).Specific.Value)
                m1.Columns.Item("Col_16").Cells.Item(pVal.Row).Specific.Value = CDec(m1.Columns.Item("Col_8").Cells.Item(pVal.Row).Specific.Value) - CDec(m1.Columns.Item("Col_9").Cells.Item(pVal.Row).Specific.Value)



                oRowCtrl.SetCellEditable(pVal.Row, 14, False)
            Catch ex As Exception
                '__oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub AddRow_OnAfterLocstFocus(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific


                If m1.Columns.Item("Col_0").Cells.Item(pVal.Row).Specific.Value <> "" Then
                    AddRowInMatrix(oForm, "@SAP_TMD1", "m1")
                End If

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

        Private Sub ItemCode_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                Dim dt As Date = Nothing

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_TMD1")
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then
                                m1.GetLineData(pVal.Row)


                                dbsrc.SetValue("U_ItemCode", dbsrc.Offset, oDataTable.GetValue("ItemCode", 0) & "")
                                dbsrc.SetValue("U_ItemName", dbsrc.Offset, oDataTable.GetValue("ItemName", 0) & "")


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


        Private Sub Matrix1_WhsCode_OnBeforeChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
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
                oCondition.Alias = "Location"
                oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
                oCondition.CondVal = oForm.Items.Item("Item_29").Specific.Value
                oCondition.BracketCloseNum = 1
                oCFL.SetConditions(oConditions)



            Catch ex As Exception
                __oApplication.MessageBox("SUB-[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Private Sub WhsCode_OnAfterChooseFromList(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)

                oCFLEvento = pVal
                sCFL_ID = oCFLEvento.ChooseFromListUID
                oForm = __oApplication.Forms.Item(FormUID)
                oCFL = oForm.ChooseFromLists.Item(sCFL_ID)
                Dim dt As Date = Nothing

                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_TMD1")
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

                If oCFLEvento.BeforeAction = False Then
                    If Not oCFLEvento.SelectedObjects Is Nothing Then
                        Try
                            Dim oDataTable As SAPbouiCOM.DataTable = oCFLEvento.SelectedObjects

                            If oDataTable.Rows.Count > 0 Then
                                m1.GetLineData(pVal.Row)


                                dbsrc.SetValue("U_WhsCode", dbsrc.Offset, oDataTable.GetValue("WhsCode", 0) & "")
                                dbsrc.SetValue("U_WhsName", dbsrc.Offset, oDataTable.GetValue("WhsName", 0) & "")


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


        Private Sub Fill_Matrix(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean, ByVal DocEntry As String)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific




                Dim dbsrc As SAPbouiCOM.DBDataSource = oForm.DataSources.DBDataSources.Item("@SAP_TMD1")

                Dim Query As String = ""
                Query = "CALL SAP_GET_TempMaster ('" + oForm.Items.Item("Item_29").Specific.Value.ToString + "')"
                m1.Clear()
                dbsrc.Clear()

                Dim oRs As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRs.DoQuery(Query)
                If oRs.RecordCount > 0 Then


                    For iRow As Integer = 1 To oRs.RecordCount

                        dbsrc.Offset = dbsrc.Size - 1
                        m1.AddRow(1, m1.VisualRowCount)
                        m1.GetLineData(m1.VisualRowCount)


                        dbsrc.SetValue("U_ItemCode", dbsrc.Offset, oRs.Fields.Item("ItemCode").Value & "")
                        dbsrc.SetValue("U_ItemName", dbsrc.Offset, oRs.Fields.Item("ItemName").Value & "")
                        dbsrc.SetValue("U_WhsCode", dbsrc.Offset, oRs.Fields.Item("WhsCode").Value & "")
                        dbsrc.SetValue("U_WhsName", dbsrc.Offset, oRs.Fields.Item("WhsName").Value & "")
                        dbsrc.SetValue("U_CalQty", dbsrc.Offset, oRs.Fields.Item("CalQty").Value & "")
                        dbsrc.SetValue("U_SAPQty", dbsrc.Offset, oRs.Fields.Item("SAPQTY").Value & "")
                        dbsrc.SetValue("U_Diff", dbsrc.Offset, oRs.Fields.Item("Diff").Value & "")
                        dbsrc.SetValue("U_UOM", dbsrc.Offset, oRs.Fields.Item("UomCode").Value & "")
                        dbsrc.SetValue("U_AdjQty", dbsrc.Offset, oRs.Fields.Item("Diff").Value & "")
                        dbsrc.SetValue("U_AdjRate", dbsrc.Offset, oRs.Fields.Item("Rate").Value & "")
                        ' dbsrc.SetValue("U_GIDOCE", dbsrc.Offset, oRs.Fields.Item("Rate").Value & "")
                        ' dbsrc.SetValue("U_GRDOCE", dbsrc.Offset, oRs.Fields.Item("Rate").Value & "")
                        m1.SetLineData(m1.VisualRowCount)
                        m1.FlushToDataSource()

                        Try
                            Dim OilDipReq As String = GetValue("Select IFNULL(""U_DipReq"",'Y') AS ""DipReq"" From OWHS Where ""WhsCode""='" + oRs.Fields.Item("WhsCode").Value + "'", "DipReq")
                            Dim oRowCtrl As SAPbouiCOM.CommonSetting
                            If OilDipReq = "Y" Then

                                oRowCtrl = m1.CommonSetting()

                                oRowCtrl.SetCellEditable(iRow, 12, False)
                            Else
                                oRowCtrl = m1.CommonSetting()
                                oRowCtrl.SetCellEditable(iRow, 12, True)
                            End If
                        Catch ex As Exception

                        End Try


                        oRs.MoveNext()
                    Next
                End If
                Marshal.ReleaseComObject(oRs)
            Catch ex As Exception

            End Try
        End Sub



        Private Sub OutSideTempLostFocus(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific


                For ADDRow As Integer = 1 To m1.RowCount
                    Try
                        m1.Columns.Item("Col_5").Cells.Item(ADDRow).Specific.Value = oForm.Items.Item("Item_2").Specific.Value
                    Catch ex As Exception

                    End Try

                Next


            Catch ex As Exception

            End Try
        End Sub

        Private Sub TankTempLostFocus(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific


                For ADDRow As Integer = 1 To m1.RowCount
                    Try
                        m1.Columns.Item("Col_4").Cells.Item(ADDRow).Specific.Value = oForm.Items.Item("Item_5").Specific.Value
                    Catch ex As Exception

                    End Try

                Next


            Catch ex As Exception

            End Try
        End Sub

        Private Sub DensityTempLostFocus(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific


                For ADDRow As Integer = 1 To m1.RowCount
                    Try
                        m1.Columns.Item("Col_6").Cells.Item(ADDRow).Specific.Value = oForm.Items.Item("Item_13").Specific.Value
                    Catch ex As Exception

                    End Try

                Next


            Catch ex As Exception

            End Try
        End Sub


        Private Sub Add_OnBeforeItemPressed(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                If oForm.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE Or oForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then
                    Dim Status As String



                    ocombo = oForm.Items.Item("Item_16").Specific

                    If ocombo.Selected.Value = "S" Then
                        __oApplication.MessageBox("Please Select the status of document ", 1, "Ok", "", "")
                        BubbleEvent = False
                        Exit Sub
                    End If


                    Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
                    If m1.RowCount <= 0 Then
                        __oApplication.MessageBox("Matrix Can Not Blank ", 1, "Ok", "", "")
                        BubbleEvent = False
                        Exit Sub
                    End If




                    If ocombo.Selected.Value = "O" Then
                        Status = GetValue("SELECT Top 1 T0.""U_Status"" FROM ""@SAP_OTMD"" T0 WHERE  T0.""U_PDate""='" + oForm.Items.Item("Item_26").Specific.Value + "' And T0.""U_Status""='O' And T0.""DocEntry""<>'" + oForm.Items.Item("Item_27").Specific.Value + "' and ""U_LocCode""='" + oForm.Items.Item("Item_29").Specific.Value + "'", "U_Status")
                        If Status <> "" Then
                            __oApplication.MessageBox("Can not add this document because Opening dip already available for today  ", 1, "Ok", "", "")
                            BubbleEvent = False
                            Exit Sub
                        End If
                    End If


                End If
            Catch ex As Exception
                __oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
            End Try
        End Sub




        Private Sub FormDatLoadEvent(ByVal FormUID As String, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(__oApplication.Forms.ActiveForm.TypeEx, __oApplication.Forms.ActiveForm.TypeCount)
                oForm.Freeze(True)
                oForm.Items.Item("Item_29").Enabled = False
                oForm.Items.Item("Item_8").Enabled = False
                oForm.Items.Item("Item_26").Enabled = False

                oForm.Freeze(False)
            Catch ex As Exception
                '__oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
                oForm.Freeze(False)
            End Try
        End Sub

        'Private Sub Qty_UneditableAfterwhs(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
        '    Try
        '        BubbleEvent = True
        '        oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
        '        Dim m2 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific
        '        'Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

        '        Dim SqlQuery As String = "SELECT T0.""WhsCode"" FROM OWHS T0 WHERE T0.""WhsCode"" ='" + m2.Columns.Item("Col_2").Cells.Item(pVal.Row).Specific.Value + "' AND T0.""U_DipReq"" ='N'"
        '        Dim oRset As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
        '        oRset.DoQuery(SqlQuery)

        '        If (oRset.RecordCount > 0) Then

        '            Try

        '                BubbleEvent = True
        '                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
        '                Dim Flag As Boolean = True
        '                Dim m1 As SAPbouiCOM.Matrix = oForm.Items.Item("m1").Specific

        '                If m1.Columns.Item("Col_2").Cells.Item(pVal.Row).Specific.Value = oRset.Fields.Item("WhsCode").Value Then
        '                    'm1.Columns.Item("Col_6").Cells.Item(ADDRow).Specific.Value
        '                    'm1.Columns.Item()
        '                    For ADDRow As Integer = 1 To m1.RowCount
        '                        Try
        '                            m1.Columns.Item("")
        '                            m1.CommonSetting.SetCellEditable(pVal.Row, ADDRow, True)
        '                        Catch ex As Exception

        '                        End Try

        '                    Next
        '                End If
        '                'oRowCtrl.SetCellEditable(pVal.Row, 9, True)

        '            Catch ex As Exception
        '                '__oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
        '                oForm.Freeze(False)
        '            End Try
        '            'For i As Integer = 0 To oRset.RecordCount - 1
        '            '    If i >= 1 And i <= oRset.RecordCount - 1 Then
        '            '        oCondition.Relationship = SAPbouiCOM.BoConditionRelationship.cr_OR
        '            '    End If
        '            '    oCondition = oConditions.Add
        '            '    oCondition.BracketOpenNum = 1
        '            '    oCondition.Alias = "ItemCode"
        '            '    oCondition.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL
        '            '    oCondition.CondVal = oRset.Fields.Item("ItemCode").Value & ""
        '            '    oCondition.BracketCloseNum = 1
        '            '    oRset.MoveNext()

        '            'Next

        '        End If
        '    Catch ex As Exception
        '        '__oApplication.MessageBox("[ItemEvent] - " & ex.Message, 1, "Ok", "", "")
        '        oForm.Freeze(False)
        '    End Try
        'End Sub

    End Class
End Namespace

