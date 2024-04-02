Imports System.Drawing
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Collections
Imports System.IO
Imports System.Diagnostics.CodeAnalysis
Imports System.Runtime.InteropServices
Imports System.Globalization
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports Microsoft.Office.Interop
Imports SAPbouiCOM
Namespace SAP_HANA
    Public Class C_ImportBudget : Implements ISAP_HANA



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

        Public Sub New(ByRef sApp As SAPbouiCOM.Application, ByRef oCompany As SAPbobsCOM.Company)
            __oApplication = sApp
            __oCompany = oCompany '.Company.GetDICompany()
        End Sub

        Public Property ObjectCode As String Implements ISAP_HANA.ObjectCode
            Get

            End Get
            Set(value As String)

            End Set
        End Property
#End Region








        Public Sub Form_Report_Display(ByRef paramarraylist As Object) Implements ISAP_HANA.Form_Report_Display
            Throw New NotImplementedException()
        End Sub



        Public Sub Form_Creation_MenuEvent(ByRef pVal As MenuEvent, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Creation_MenuEvent
            Try
                Dim sFileName As String = AppDomain.CurrentDomain.BaseDirectory & "SRF\IBudget.srf"
                Dim sFormName As String = "IBudget"
                Dim FormUID As String = LoadXMLFiles(__oApplication, sFileName)
                oForm = __oApplication.Forms.GetForm("IBudget", __oApplication.Forms.ActiveForm.TypeCount)
                If Not String.IsNullOrEmpty(FormUID) Then

                    oForm.Freeze(True)
                    oForm.Mode = BoFormMode.fm_ADD_MODE

                    oForm.DataSources.UserDataSources.Add("db0", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 1)
                    oForm.DataSources.UserDataSources.Add("db1", SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 1)
                    Dim opAB, opPB As SAPbouiCOM.OptionBtn

                    opAB = oForm.Items.Item("opAB").Specific
                    opAB.DataBind.SetBound(True, "", "db0")

                    opPB = oForm.Items.Item("opPB").Specific
                    opPB.DataBind.SetBound(True, "", "db1")

                    opAB.GroupWith("opPB")
                    opPB.GroupWith("opAB")

                    opAB.Selected = True
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
            Throw New NotImplementedException()
        End Sub

        Public Sub Form_Load_DataEvent(ByRef BusinessObjectInfo As BusinessObjectInfo, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Load_DataEvent
            Throw New NotImplementedException()
        End Sub

        Public Sub Form_Process_ItemEvents(FormUID As String, ByRef pVal As ItemEvent, ByRef BubbleEvent As Boolean) Implements ISAP_HANA.Form_Process_ItemEvents
            Try
                BubbleEvent = True


                If pVal.BeforeAction = False Then
                    If pVal.ItemUID = "btnUpload" And pVal.EventType = SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED Then
                        UploadData_Click(FormUID, pVal, BubbleEvent)

                    End If
                ElseIf pVal.BeforeAction = True Then

                End If
            Catch ex As Exception


            End Try
        End Sub

        Private Sub UploadData_Click(ByVal FormUID As String, ByRef pVal As SAPbouiCOM.ItemEvent, ByRef BubbleEvent As Boolean)
            Try
                BubbleEvent = True
                oForm = __oApplication.Forms.GetForm(pVal.FormTypeEx, pVal.FormTypeCount)
                Dim objConn As OleDbConnection = Nothing
                Dim dt As System.Data.DataTable = Nothing
                Dim NoOFBL, NoOfCN As Integer

                Dim filePath As String = oForm.Items.Item("4").Specific.Value
                ' Dim filePath As String = "D:\\Attachments"

                Dim fileExt As String = String.Empty
                fileExt = Path.GetExtension(filePath)

                If fileExt.CompareTo(".xls") = 0 OrElse fileExt.CompareTo(".xlsx") = 0 Then

                    Try

                        Dim conn As String = String.Empty

                        If fileExt.CompareTo(".xls") = 0 Then
                            conn = "provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & filePath & ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=1';"
                        Else
                            conn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & filePath & ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1';"
                        End If

                        objConn = New OleDbConnection(conn)
                        objConn.Open()
                        dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)



                        Dim excelSheets As String() = New String(dt.Rows.Count - 1) {}
                        Dim i As Integer = 0
                        Dim dtAll As System.Data.DataTable = New System.Data.DataTable()
                        Dim dsAll As System.Data.DataSet = New System.Data.DataSet()

                        For Each row As DataRow In dt.Rows


                            If Not row("TABLE_NAME").ToString().Contains("FilterDatabase") Then

                                Dim sheet_name As String = row("TABLE_NAME").ToString()

                                Try

                                    Dim dtexcel As System.Data.DataTable = New System.Data.DataTable()
                                    Dim oleAdpt As OleDbDataAdapter = New OleDbDataAdapter("select * from [" & sheet_name & "]", conn)

                                    oleAdpt.Fill(dtexcel)
                                    dtexcel.TableName = sheet_name
                                    dsAll.Tables.Add(dtexcel)

                                Catch ex As Exception
                                    MessageBox.Show(ex.Message.ToString())
                                End Try
                            End If
                        Next

                        Dim oGeneralService As SAPbobsCOM.GeneralService
                        Dim oGeneralData As SAPbobsCOM.GeneralData
                        Dim oSons As SAPbobsCOM.GeneralDataCollection
                        Dim oSon As SAPbobsCOM.GeneralData
                        Dim oGeneralParams As SAPbobsCOM.GeneralDataParams
                        Dim sCmp As SAPbobsCOM.CompanyService
                        sCmp = __bobCompany.GetCompanyService
                        Dim Count As Integer = GetValue("SELECT Count(T0.""U_Year"") As ""Count""  FROM ""@SAP_OABU""  T0 WHERE T0.""U_Auth"" ='Y' And  T0.""U_Year"" ='" + dsAll.Tables(0).Rows(0)("Financial Year").ToString + "'", "Count")

                        If Count > 0 Then
                            __oApplication.MessageBox("Can not update Account Budget Data because of finacial year " + dsAll.Tables(0).Rows(0)("Financial Year").ToString + " authorized budget allready  uploade.   ", 1, "Ok", "", "")
                            BubbleEvent = False
                            Exit Sub
                        End If
                        Dim Year As String = GetValue("SELECT (T0.""U_Year"") As ""Year""  FROM ""@SAP_OABU""  T0 WHERE   T0.""U_Year"" ='" + dsAll.Tables(0).Rows(0)("Financial Year").ToString + "'", "Year")

                        If Year = dsAll.Tables(0).Rows(0)("Financial Year").ToString Then
                            If __oApplication.MessageBox("Account Budget Data  finacial year " + dsAll.Tables(0).Rows(0)("Financial Year").ToString + "  is allready uploaded .Do you want to add extra data.", 1, "Yes", "No", "") = 2 Then
                                BubbleEvent = False
                                Exit Sub
                            Else


                                Dim DocEntry As String = GetValue("SELECT (T0.""DocEntry"") As ""DocEntry""  FROM ""@SAP_OABU""  T0 WHERE   T0.""U_Year"" ='" + dsAll.Tables(0).Rows(0)("Financial Year").ToString + "'", "DocEntry")
                                oGeneralService = sCmp.GetGeneralService("SAP_UDO_OABU")
                                ' Specify data for main UDO
                                oGeneralParams = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralDataParams)
                                oGeneralParams.SetProperty("DocEntry", DocEntry)
                                oGeneralData = oGeneralService.GetByParams(oGeneralParams)
                                For M As Integer = 0 To dsAll.Tables(0).Rows.Count - 1
                                    'Specify data for child UDO
                                    oSons = oGeneralData.Child("SAP_ABU1")
                                    oSon = oSons.Add
                                    oSon.SetProperty("U_Year", dsAll.Tables(0).Rows(0)("Financial Year").ToString)
                                    oSon.SetProperty("U_BPLId", dsAll.Tables(0).Rows(M)("Branch Code").ToString)
                                    oSon.SetProperty("U_BPLName", dsAll.Tables(0).Rows(M)("Branch Name").ToString)
                                    oSon.SetProperty("U_CostCode", dsAll.Tables(0).Rows(M)("Cost Center Code").ToString)
                                    oSon.SetProperty("U_ActCode", dsAll.Tables(0).Rows(M)("Account Code").ToString)
                                    oSon.SetProperty("U_Debit", dsAll.Tables(0).Rows(M)("Debit").ToString)
                                    oSon.SetProperty("U_Credit", dsAll.Tables(0).Rows(M)("Credit").ToString)
                                    oSon.SetProperty("U_Jan", dsAll.Tables(0).Rows(M)("Jan").ToString)
                                    oSon.SetProperty("U_Feb", dsAll.Tables(0).Rows(M)("Feb").ToString)
                                    oSon.SetProperty("U_Mar", dsAll.Tables(0).Rows(M)("Mar").ToString)
                                    oSon.SetProperty("U_April", dsAll.Tables(0).Rows(M)("April").ToString)
                                    oSon.SetProperty("U_May", dsAll.Tables(0).Rows(M)("May").ToString)
                                    oSon.SetProperty("U_Jun", dsAll.Tables(0).Rows(M)("Jun").ToString)
                                    oSon.SetProperty("U_Jul", dsAll.Tables(0).Rows(M)("Jul").ToString)
                                    oSon.SetProperty("U_Agust", dsAll.Tables(0).Rows(M)("Agust").ToString)
                                    oSon.SetProperty("U_Sep", dsAll.Tables(0).Rows(M)("Sep").ToString)
                                    oSon.SetProperty("U_Oct", dsAll.Tables(0).Rows(M)("Oct").ToString)
                                    oSon.SetProperty("U_Nov", dsAll.Tables(0).Rows(M)("Nov").ToString)
                                    oSon.SetProperty("U_Dec", dsAll.Tables(0).Rows(M)("Dec").ToString)
                                Next
                                oGeneralService.Update(oGeneralData)
                                __oApplication.MessageBox(" Account Budget Data  of finacial year " + dsAll.Tables(0).Rows(0)("Financial Year").ToString + " Added Successfully.   ", 1, "Ok", "", "")




                            End If


                        Else


                            If dsAll.Tables(0).TableName = "Account$" Then
                                oGeneralService = sCmp.GetGeneralService("SAP_UDO_OABU")
                                oGeneralData = oGeneralService.GetDataInterface(SAPbobsCOM.GeneralServiceDataInterfaces.gsGeneralData)
                                oGeneralData.SetProperty("U_PDate", Date.Now())
                                oGeneralData.SetProperty("U_Year", dsAll.Tables(0).Rows(0)("Financial Year").ToString)
                                For M As Integer = 0 To dsAll.Tables(0).Rows.Count - 1
                                    'Specify data for child UDO
                                    oSons = oGeneralData.Child("SAP_ABU1")
                                    oSon = oSons.Add
                                    oSon.SetProperty("U_Year", dsAll.Tables(0).Rows(M)("Financial Year").ToString)
                                    oSon.SetProperty("U_BPLId", dsAll.Tables(0).Rows(M)("Branch Code").ToString)
                                    oSon.SetProperty("U_BPLName", dsAll.Tables(0).Rows(M)("Branch Name").ToString)
                                    oSon.SetProperty("U_CostCode", dsAll.Tables(0).Rows(M)("Cost Center Code").ToString)
                                    oSon.SetProperty("U_ActCode", dsAll.Tables(0).Rows(M)("Account Code").ToString)
                                    oSon.SetProperty("U_Debit", dsAll.Tables(0).Rows(M)("Debit").ToString)
                                    oSon.SetProperty("U_Credit", dsAll.Tables(0).Rows(M)("Credit").ToString)
                                    oSon.SetProperty("U_Jan", dsAll.Tables(0).Rows(M)("Jan").ToString)
                                    oSon.SetProperty("U_Feb", dsAll.Tables(0).Rows(M)("Feb").ToString)
                                    oSon.SetProperty("U_Mar", dsAll.Tables(0).Rows(M)("Mar").ToString)
                                    oSon.SetProperty("U_April", dsAll.Tables(0).Rows(M)("April").ToString)
                                    oSon.SetProperty("U_May", dsAll.Tables(0).Rows(M)("May").ToString)
                                    oSon.SetProperty("U_Jun", dsAll.Tables(0).Rows(M)("Jun").ToString)
                                    oSon.SetProperty("U_Jul", dsAll.Tables(0).Rows(M)("Jul").ToString)
                                    oSon.SetProperty("U_Agust", dsAll.Tables(0).Rows(M)("Agust").ToString)
                                    oSon.SetProperty("U_Sep", dsAll.Tables(0).Rows(M)("Sep").ToString)
                                    oSon.SetProperty("U_Oct", dsAll.Tables(0).Rows(M)("Oct").ToString)
                                    oSon.SetProperty("U_Nov", dsAll.Tables(0).Rows(M)("Nov").ToString)
                                    oSon.SetProperty("U_Dec", dsAll.Tables(0).Rows(M)("Dec").ToString)


                                Next
                            End If
                            oGeneralService.Add(oGeneralData)
                            __oApplication.MessageBox(" Account Budget Data  of finacial year " + dsAll.Tables(0).Rows(0)("Financial Year").ToString + " Added Successfully.   ", 1, "Ok", "", "")
                        End If
                    Catch ex As Exception
                        __oApplication.MessageBox("[ItemEvent - Upload] - " & ex.Message, 1, "Ok", "", "")

                    End Try
                Else
                    __oApplication.MessageBox("[Please choose .xls or .xlsx file only.] - ", 1, "Ok", "", "")

                End If

            Catch ex As Exception

            End Try

        End Sub
    End Class
End Namespace

