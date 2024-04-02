Imports System
Imports System.IO
Imports System.Net
Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates
Imports System.Text
Imports System.Xml
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Globalization
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Collections
Imports System.Diagnostics.CodeAnalysis
Namespace SAP_HANA
    Module General

        Public __Application As SAPbouiCOM.Application = Nothing
        Public __DBUserName As String = ""
        Public __DBPassword As String = ""
        Public __DBServer As String = ""
        Public __DBName As String = ""
        Public __bobCompany As SAPbobsCOM.Company = Nothing


        Public intPvalRow As Integer = Nothing
        Public oH1B As String
        Public Flag As Boolean = False
        Private oForm As SAPbouiCOM.Form
        Private DateFormat As ArrayList = New ArrayList(7)
        Public GetDateFormat As String = ""
        Public __ReportID As String = ""
        Public ReportParameterList As Object
        Public ItemBatchList As Object
        Public ParameterList As ArrayList = New ArrayList
        Public cmbValue As String = ""
        Public cmbDescr As String = ""
        Public IsBaseForm As SAPbouiCOM.Form = Nothing
        Public IsBaseUDF As String = Nothing
        Public IsBase_DN_UDF As String = Nothing

        Public IsBaseItemID As String = Nothing
        Public gHashTable As Hashtable = New Hashtable
        Public oMenuOutStandingMaster As String = ""
        Public oMenuDocumentMaster As String = ""
        Public oMenuAccountMApping As String = ""
        Public oMenuItemBasicSetup As String = ""
        Public oMenuLedgerBasicSetup As String = ""

        Public SODocEntry As String = ""
        Public SOCardCode As String = ""
        Public SODocDate As Date = Nothing


        Public GoodIssueNo As String = ""
        Public GoodIssueNoDate As Date = Nothing

        Public GINo As String = ""
        Public GIDate As Date = Nothing
        Public GISONo As String = ""
        Public GISODate As Date = Nothing
        Public GIMenuUID As String = ""

        Public UDOITDocEntry As String = ""
        Public UDOITDocNum As String = ""
        Public UDOITCardCode As String = ""
        Public CFL_DocEntry As String = ""
        Public CFL_DocNum As String = ""


        Public SPODOCNUM As String = ""
        Public SPOCARDCODE As String = ""
        Public SPODate As String = ""

        Public APIDOCNUM As String = ""
        Public APICARDCODE As String = ""
        Public APIDate As String = ""

        Public CHAMBERDOCNUM As String = ""
        Public CHAMBERDOCENTRY As String = ""
        Public CHAMBERBASEYPE As String = ""



        Public CHCARDCODE As String = ""
        Public CHTRNO As String = ""
        Public CHBASETYEP As String = ""
        Public CHDOCNUM As String = ""
        Public CHTCODE As String = ""

        Public PKLTRNO As String = ""
        Public PKLDOCNUM As String = ""


        Public PKLNO As String = ""
        Public PKLTR As String = ""



#Region "Const Enumeration"
        Public Enum ReportIDNo
            InventoryAging = 1
            OnTimeDelivery = 2
            ProjectFollowUp = 3
            ForecastDelivery = 4
            JobworkRegister = 5
            ReworkRegister = 6
            InspectionFollowUp = 7
            SupplierPerformanance = 8
        End Enum

        Public Structure SearchParams
            Dim sql As String
            Dim QueryName As String
            Dim CatCode As String
            Dim FormID As String
            Dim ItemID As String
            Dim QueryID As Long
            Dim HDRTable As String

            'RowParams
            Dim RowTable As String
            Dim ColumnName As String
            Dim TargetColumn As String
            '
        End Structure
#End Region

        Dim oUserTablesMD As SAPbobsCOM.UserTablesMD
        <DebuggerDisplay("ParameterFiled : {sParameter}, ParameterValue : {sParameterValue}")>
        Structure Parameters
            Dim sParameter As String
            Dim sParameterValue As String
            Public Sub New(ByVal Value As String, ByVal Description As String)
                sParameter = Value
                sParameterValue = Description
            End Sub

            Public Property ParameterValue() As String
                Get
                    Return sParameterValue
                End Get
                Set(ByVal value As String)
                    sParameterValue = value
                End Set
            End Property

            Public Property ParameterFiled() As String
                Get
                    Return sParameter
                End Get
                Set(ByVal value As String)
                    sParameter = value
                End Set
            End Property

        End Structure

        Public Sub SetDateFormat()

            DateFormat.Add("dd/MM/yy")
            DateFormat.Add("dd/MM/yyyy")
            DateFormat.Add("MM/dd/yy")
            DateFormat.Add("MM/dd/yyyy")
            DateFormat.Add("yyyy/MM/dd")
            DateFormat.Add("dd/Month/yyyy")
            DateFormat.Add("yy/MM/dd")

            Dim oRset As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            oRset.DoQuery("Select ""DateFormat"" From OADM")
            If oRset.RecordCount > 0 Then
                Dim DTFrmPos As String = oRset.Fields.Item("DateFormat").Value.ToString
                GetDateFormat = DateFormat(DTFrmPos)
            End If

        End Sub

        Public Property DBName() As String
            Get
                Return __DBName
            End Get
            Set(ByVal value As String)
                __DBName = value
            End Set
        End Property

        Public Property DBSerName() As String
            Get
                Return __DBServer
            End Get
            Set(ByVal value As String)
                __DBServer = value
            End Set
        End Property

        Public Property DBUserName() As String
            Get
                Return __DBUserName
            End Get
            Set(ByVal value As String)
                __DBUserName = value
            End Set
        End Property

        Public Property DBPassword() As String
            Get
                Return __DBPassword
            End Get
            Set(ByVal value As String)
                __DBPassword = value
            End Set
        End Property

        Public Property SAPApplication() As SAPbouiCOM.Application
            Get
                Return __Application
            End Get
            Set(ByVal value As SAPbouiCOM.Application)
                __Application = value
            End Set
        End Property

        Public Property bobCompany() As SAPbobsCOM.Company
            Get
                Return __bobCompany
            End Get
            Set(ByVal value As SAPbobsCOM.Company)
                __bobCompany = value
            End Set
        End Property

        Public Property ReportID() As String
            Get
                Return __ReportID
            End Get
            Set(ByVal value As String)
                __ReportID = value
            End Set
        End Property

        'Public ReadOnly Property ActiveConnection() As System.Data.Common.DbConnection
        '    Get
        '        Try
        '            Dim dbCon As System.Data.Common.DbConnection = SqlClientFactory.Instance.CreateConnection()
        '            ''Data Source=myServerAddress;Initial Catalog=myDataBase;User Id=myUsername;Password=myPassword;Integrated Security=SSPI;
        '            'Dim conStr As String = "Data Source = " + __DBServer + ";Initial Catalog=" + SAPWrapper.GUI.Credientials.DataBaseName + ";User ID=" + SAPWrapper.GUI.Credientials.UserName + "; Password=" + SAPWrapper.GUI.Credientials.Password + ";MultipleActiveResultSets=true;"
        '            '1Dim conStr As String = "Data Source = " + __DBServer + ";Initial Catalog=" + DBName + ";User ID=" + DBUserName + "; Password=agt@agt123;MultipleActiveResultSets=true;"
        '            Dim Password As String = System.Configuration.ConfigurationManager.AppSettings("PASS").ToString
        '            Dim conStr As String = "Data Source = " + __DBServer + ";Initial Catalog=" + DBName + ";User ID=" + DBUserName + "; Password=" + Password + ";MultipleActiveResultSets=true;"

        '            dbCon.ConnectionString = conStr
        '            Return dbCon
        '        Catch ex As COMException
        '            __Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        '            Return Nothing
        '        Catch ex1 As Exception
        '            __Application.StatusBar.SetText(ex1.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
        '            Return Nothing
        '        End Try
        '    End Get
        'End Property

        Public ReadOnly Property ActiveConnection() As System.Data.Common.DbConnection
            Get
                Try
                    Dim dbCon As System.Data.Common.DbConnection = SqlClientFactory.Instance.CreateConnection()
                    ''Data Source=myServerAddress;Initial Catalog=myDataBase;User Id=myUsername;Password=myPassword;Integrated Security=SSPI;
                    ' Dim conStr As String = "Data Source = " + __DBServer + ";Initial Catalog=" + SAPWrapper.GUI.Credientials.DataBaseName + ";User ID=" + SAPWrapper.GUI.Credientials.UserName + "; Password=" + SAPWrapper.GUI.Credientials.Password + ";MultipleActiveResultSets=true;"
                    '1Dim conStr As String = "Data Source = " + __DBServer + ";Initial Catalog=" + DBName + ";User ID=" + DBUserName + "; Password=agt@agt123;MultipleActiveResultSets=true;"
                    'Dim Password As String = System.Configuration.ConfigurationManager.AppSettings("PASS").ToString
                    Dim ConfigStr, Pass, U_DBServer, U_DBName, U_DBUserName As String
                    Try

                        ConfigStr = "SELECT  U_DBServer,U_DBName,U_DBUserName,U_Pass FROM [@TSSIPL_CONFIG] "
                        Dim oRsetConfig As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRsetConfig.DoQuery(ConfigStr)
                        If oRsetConfig.RecordCount > 0 Then
                            U_DBServer = oRsetConfig.Fields.Item("U_DBServer").Value
                            U_DBName = oRsetConfig.Fields.Item("U_DBName").Value
                            U_DBUserName = oRsetConfig.Fields.Item("U_DBUserName").Value
                            Pass = oRsetConfig.Fields.Item("U_Pass").Value
                        End If

                    Catch ex As Exception

                    End Try



                    Dim conStr As String = "Data Source = " + U_DBServer + ";Initial Catalog=" + U_DBName + ";User ID=" + U_DBUserName + "; Password=" + Pass + ";MultipleActiveResultSets=true;"

                    dbCon.ConnectionString = conStr
                    Return dbCon
                Catch ex As COMException
                    __Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Return Nothing
                Catch ex1 As Exception
                    __Application.StatusBar.SetText(ex1.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning)
                    Return Nothing
                End Try
            End Get
        End Property

        Public Function GetSQLDateFormat(ByVal tdate As String) As String
            Try
                Dim dt As Date = DateTime.ParseExact(tdate, GetDateFormat, DateTimeFormatInfo.InvariantInfo)
                'Dim dt As Date = Convert.ToDateTime(tdate)
                Return dt '.Month + "/" + dt.Day + "/" + dt.Year
            Catch ex As Exception
                'app.MessageBox("Date Format Of This System is Not in Proper Format. Please Specify [MM/dd/yyyy] Format From Regional Settings.", 1, "", "", "")
                Return ""
            End Try
        End Function

        Public Function GetSQLDateFormat(ByVal tdate As String, ByVal app As SAPbouiCOM.Application) As String
            Try
                Dim dt As Date = DateTime.ParseExact(tdate, "MM/dd/yyyy", DateTimeFormatInfo.InvariantInfo)
                'Dim dt As Date = Convert.ToDateTime(tdate)
                Return dt '.Month + "/" + dt.Day + "/" + dt.Year
            Catch ex As Exception
                app.MessageBox("Date Format Of This System is Not in Proper Format. Please Specify [MM/dd/yyyy] Format From Regional Settings.", 1, "", "", "")
                Return ""
            End Try
        End Function

        Public Function getApplicationPath() As String
            'Return IO.Directory.GetParent(Application.StartupPath).ToString
            Return Application.StartupPath
        End Function

        Public Sub AddRowInMatrix(ByVal sForm As SAPbouiCOM.Form, ByVal dbSrcName As String, ByVal MatrixName As String)

            Dim dbSrc As SAPbouiCOM.DBDataSource = sForm.DataSources.DBDataSources.Item(dbSrcName)
            Dim m As SAPbouiCOM.Matrix = sForm.Items.Item(MatrixName).Specific
            dbSrc.InsertRecord(dbSrc.Size)
            dbSrc.Offset = dbSrc.Size - 1
            m.AddRow(1, m.VisualRowCount)

        End Sub

        Public Sub DelRowFromMatrix(ByVal sForm As SAPbouiCOM.Form, ByVal dbSrcName As String, ByVal MatrixName As String)
            Try
                Dim dbSrc As SAPbouiCOM.DBDataSource = sForm.DataSources.DBDataSources.Item(dbSrcName)
                Dim m As SAPbouiCOM.Matrix = sForm.Items.Item(MatrixName).Specific
                Dim row As Integer = m.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_SelectionOrder)
                If row <> -1 Then
                    dbSrc.RemoveRecord(row - 1)
                    dbSrc.Offset = dbSrc.Size - 1
                    m.DeleteRow(row)
                    sForm.Update()
                    sForm.Refresh()
                    m.FlushToDataSource()
                    If sForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then sForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE

                End If
            Catch ex As Exception

            End Try
        End Sub

        Public Sub DelRowFromMatrix(ByVal sForm As SAPbouiCOM.Form, ByVal dbSrcName As String, ByVal MatrixName As String, ByVal RowID As Integer)
            Try
                Dim dbSrc As SAPbouiCOM.DBDataSource = sForm.DataSources.DBDataSources.Item(dbSrcName)
                Dim m As SAPbouiCOM.Matrix = sForm.Items.Item(MatrixName).Specific
                Dim row As Integer = RowID
                If row <> -1 Then
                    dbSrc.RemoveRecord(row - 1)
                    dbSrc.Offset = dbSrc.Size - 1
                    m.DeleteRow(row)
                    sForm.Update()
                    sForm.Refresh()
                    m.FlushToDataSource()
                    If sForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then sForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE
                End If
            Catch ex As Exception

            End Try
        End Sub

        Public Sub DelRowFromMatrix(ByVal sForm As SAPbouiCOM.Form, ByVal dbSrcName As String, ByVal MatrixName As String, ByVal DelBlankRow As Boolean)
            Try
                Dim dbSrc As SAPbouiCOM.DBDataSource = sForm.DataSources.DBDataSources.Item(dbSrcName)
                Dim m As SAPbouiCOM.Matrix = sForm.Items.Item(MatrixName).Specific
                Dim row As Integer = m.VisualRowCount 'm.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_SelectionOrder)
                If row <> -1 Then
                    dbSrc.RemoveRecord(row - 1)
                    dbSrc.Offset = dbSrc.Size - 1
                    m.DeleteRow(row)
                    sForm.Update()
                    sForm.Refresh()
                    m.FlushToDataSource()
                    If sForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE Then sForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE

                End If
            Catch ex As Exception

            End Try
        End Sub

        Public Sub ActiveForm(ByVal f As SAPbouiCOM.Form, ByVal defaultBrowser As String, ByVal defaultButton As String)
            Try
                f.Freeze(True)
                ActivateMenu(f)
                f.Mode = SAPbouiCOM.BoFormMode.fm_ADD_MODE
                f.DataBrowser.BrowseBy = defaultBrowser
                f.DefButton = defaultButton
                f.Freeze(False)
                f.Update()
            Catch ex As Exception
                SAPApplication.MessageBox("[SubMain].[MenuEvent] - " & ex.Message, 1, "Ok")
            End Try
        End Sub

        Public Sub ActivateMenu(ByVal f As SAPbouiCOM.Form)
            Try
                'f.EnableMenu(1282, True)  'Add Record
                f.EnableMenu(1281, True)  'Find Record
                f.EnableMenu(1290, True)
                f.EnableMenu(1291, True)
                f.EnableMenu(1288, True)
                f.EnableMenu(1289, True)
                f.EnableMenu(1283, True)
                f.EnableMenu(1287, True)
            Catch ex As Exception
                SAPApplication.MessageBox("[SubMain].[MenuEvent] - " & ex.Message, 1, "Ok")
            End Try
        End Sub

        Public Sub DeativateMenu(ByVal f As SAPbouiCOM.Form)
            Try
                f.EnableMenu(1282, False)  'Add Record
                f.EnableMenu(1281, False)  'Find Record
                f.EnableMenu(1290, False)
                f.EnableMenu(1291, False)
                f.EnableMenu(1288, False)
                f.EnableMenu(1289, False)
                f.EnableMenu(1283, False)
                f.EnableMenu(1287, False)
            Catch ex As Exception
                SAPApplication.MessageBox("[SubMain].[MenuEvent] - " & ex.Message, 1, "Ok")
            End Try
        End Sub

        Public Function LoadFromXML(ByRef SBO_APPLICATION As SAPbouiCOM.Application, ByVal SrfFileName As String) As SAPbouiCOM.Form
            Try
                Dim rn As Random = New Random()
                Dim formCounter As Integer = rn.Next()
                Dim oXmlDoc As New Xml.XmlDocument
                Dim sPath As String
                Try
                    oXmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory & "\Screens\" & SrfFileName)
                Catch
                    'sPath = IO.Directory.GetParent(Application.StartupPath).ToString() + "\Collaboration"
                    'oXmlDoc.Load(sPath & "\" & SrfFileName & ".xml")
                End Try
                Dim oXNode As Xml.XmlNode = oXmlDoc.GetElementsByTagName("form").Item(0)
                Dim oAttr As Xml.XmlAttribute = oXNode.Attributes.GetNamedItem("uid")
                oAttr.Value = oAttr.Value + formCounter.ToString()
                SBO_APPLICATION.LoadBatchActions(oXmlDoc.InnerXml)
                Return SBO_APPLICATION.Forms.ActiveForm
                oXmlDoc = Nothing
            Catch ex As Exception
                MessageBox.Show(ex.Message)
                Return Nothing
            End Try
        End Function

        Public Function LoadXMLFiles(ByVal app As SAPbouiCOM.Application, ByVal FileName As String) As String
            Dim FrmUID As String
            Dim FormNum As Int32 = 0
            Dim oXNode As XmlNode
            Dim oAttr As XmlAttribute
            Dim oXmlDoc As System.Xml.XmlDocument = Nothing
            Try
                Dim r As New Random()
                oXmlDoc = New XmlDocument()
                oXmlDoc.Load(FileName)
                Dim xmlString As String = oXmlDoc.InnerXml.ToString()
                oXNode = oXmlDoc.GetElementsByTagName("form").Item(0)
                oAttr = TryCast(oXNode.Attributes.GetNamedItem("uid"), XmlAttribute)
                oAttr.Value = oAttr.Value + r.Next(111, 999).ToString()
                FrmUID = oAttr.Value
                xmlString = oXmlDoc.InnerXml.ToString()
                app.LoadBatchActions(xmlString)
                Return FrmUID
            Catch ex As Exception
                app.MessageBox(ex.Message, 0, "Ok", "", "")
            Finally
                oXmlDoc = Nothing
            End Try
            Return ""
        End Function

        Public Sub Fill_Matrix_ComboBox(ByVal oCombobox As SAPbouiCOM.ComboBox, ByVal Sql As String, ByVal Value_Col As String, ByVal Desc_Col As String, ByVal DefineNew As Boolean, ByVal BlankValue As Boolean)
            Try
                If oCombobox.ValidValues.Count > 0 Then
                    For iVal As Integer = oCombobox.ValidValues.Count - 1 To 0 Step -1
                        oCombobox.ValidValues.Remove(iVal, SAPbouiCOM.BoSearchKey.psk_Index)
                    Next
                End If
                Dim oRset As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRset.DoQuery(Sql)
                If BlankValue = True Then oCombobox.ValidValues.Add("-1", "")
                If oRset.RecordCount > 0 Then
                    While oRset.EoF = False
                        oCombobox.ValidValues.Add(oRset.Fields.Item(Value_Col).Value & "", oRset.Fields.Item(Desc_Col).Value & "")
                        oRset.MoveNext()
                    End While
                End If
                If DefineNew = True Then oCombobox.ValidValues.Add("DefineNew", "Define New")

                Marshal.ReleaseComObject(oRset)

            Catch ex As Exception

            End Try
        End Sub

        Public Sub Fill_MatrixColumn_ComboBox(ByRef oMatricColumn As SAPbouiCOM.Column, ByVal Sql As String, ByVal Value_Col As String, ByVal Desc_Col As String, ByVal DefineNew As Boolean, ByVal BlankValue As Boolean)
            Try
                If oMatricColumn.ValidValues.Count > 0 Then
                    For iVal As Integer = oMatricColumn.ValidValues.Count - 1 To 0 Step -1
                        oMatricColumn.ValidValues.Remove(iVal, SAPbouiCOM.BoSearchKey.psk_Index)
                    Next
                End If
                Dim oRset As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRset.DoQuery(Sql)
                If BlankValue = True Then oMatricColumn.ValidValues.Add("", "")
                If oRset.RecordCount > 0 Then
                    While oRset.EoF = False
                        oMatricColumn.ValidValues.Add(oRset.Fields.Item(0).Value & "", oRset.Fields.Item(1).Value & "")
                        oRset.MoveNext()
                    End While
                End If
                If DefineNew = True Then oMatricColumn.ValidValues.Add("DefineNew", "Define New")

                Marshal.ReleaseComObject(oRset)

            Catch ex As Exception

            End Try
        End Sub


        Public Function GenerateCode(ByVal ObjectCode As String, ByRef bCompany As SAPbobsCOM.Company) As String
            Try
                Dim oRset As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRset.DoQuery("Select AutoKey From ONNM Where ObjectCode='" & ObjectCode & "'")
                Dim GenerateId As String = ""
                If oRset.RecordCount > 0 Then
                    GenerateId = oRset.Fields.Item("AutoKey").Value
                End If
                Return GenerateId
            Catch ex As Exception
                Return ""
            End Try
        End Function

        Public Function GenrateDocEntry(ByVal Field As String, ByVal TableName As String) As String
            Try
                Dim Code As String = ""
                '__bobCompany = __Application.Company.GetDICompany()
                Dim Sql As String = "select (case when (ifnull(max(""" + Field + """),0))=0 then 1 else (ifnull(max(""" + Field + """),0)) + 1 end) as CODE from """ + TableName + """"
                Dim oRset As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                oRset.DoQuery(Sql)
                If oRset.RecordCount > 0 Then
                    Code = oRset.Fields.Item("CODE").Value
                End If
                Marshal.ReleaseComObject(oRset)
                Return Code
            Catch ex As Exception
                Return ""
            End Try
        End Function
        Public Sub DelRowFromTable(ByVal sForm As SAPbouiCOM.Form, ByVal dbSrcName As String, ByVal MatrixName As String, ByVal MatrixColNM As String)
            Try
                Dim dbSrc As SAPbouiCOM.DBDataSource = sForm.DataSources.DBDataSources.Item(dbSrcName)
                Dim m As SAPbouiCOM.Matrix = sForm.Items.Item(MatrixName).Specific
                Dim row As Integer = m.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_SelectionOrder)

                If row <> -1 Then
                    sForm.Freeze(True)
                    Dim DocEntry As SAPbouiCOM.EditText = m.Columns.Item(MatrixColNM).Cells.Item(row).Specific

                    If DocEntry.String <> "" Then
                        Dim DocEn As String = DocEntry.String
                        Dim SqlQuery As String = ""
                        SqlQuery = "Delete from [" + dbSrcName + "] where DocEntry=" + DocEn
                        Dim oRset As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
                        oRset.DoQuery(SqlQuery)
                        If oRset.RecordCount > 0 Then

                        End If
                        m.DeleteRow(row)

                        If sForm.Mode <> SAPbouiCOM.BoFormMode.fm_UPDATE_MODE Then sForm.FormHwnd.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE

                    Else
                        sForm.SAPApplication.MessageBox("No Transaction Found for Delete", 1, "Ok", "", "")
                    End If

                    sForm.Freeze(False)
                    sForm.Update()
                    sForm.Refresh()

                Else
                    sForm.SAPApplication.MessageBox("Please select the row to Delete", 1, "Ok", "", "")

                End If
            Catch ex As Exception
                sForm.SAPApplication.MessageBox(ex.Message, 1, "Ok", "", "")
            End Try
        End Sub

        Public Function GetDataSetResult(ByVal SQLQuery As String) As DataSet
            Try
                Dim ds As DataSet = New DataSet
                Dim cmd As SqlClient.SqlCommand = New SqlClient.SqlCommand(SQLQuery, CType(ActiveConnection, SqlClient.SqlConnection))

                If cmd.Connection.State <> ConnectionState.Open Then cmd.Connection.Open()

                Dim sqlDataAdapter As SqlDataAdapter = New SqlDataAdapter(cmd)
                sqlDataAdapter.Fill(ds)

                Return ds
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        Public Function GetDate(ByVal value As String) As Date
            Try
                Dim ToDate As Date = Nothing
                ToDate = DateTime.ParseExact(value, GetDateFormat, DateTimeFormatInfo.InvariantInfo)

                Return ToDate

            Catch ex As Exception
                Return Nothing
            End Try
        End Function


        'Public Sub CrystalReportLogOnInfo(ByRef rpt As CrystalDecisions.CrystalReports.Engine.ReportDocument)
        '    Try
        '        Dim ConInfo As New CrystalDecisions.Shared.TableLogOnInfo

        '        ConInfo.ConnectionInfo.UserID = DBUserName
        '        Dim Password As String = System.Configuration.ConfigurationManager.AppSettings("PASS").ToString
        '        ConInfo.ConnectionInfo.Password = Password '"agt@agt123" ''"sql2005" 'User Password  "
        '        ConInfo.ConnectionInfo.ServerName = DBSerName '<<A class=iAs style="FONT-WEIGHT: normal; FONT-SIZE: 100%; PADDING-BOTTOM: 1px; COLOR: darkgreen; BORDER-BOTTOM: darkgreen 0.07em solid; BACKGROUND-COLOR: transparent; TEXT-DECORATION: underline" href="#" target=_blank itxtdid="2472057">Server</A> Name>
        '        ConInfo.ConnectionInfo.DatabaseName = DBName '<Database Name>"
        '        For intCounter As Integer = 0 To rpt.Database.Tables.Count - 1
        '            rpt.Database.Tables(intCounter).LogOnInfo.ConnectionInfo = ConInfo.ConnectionInfo
        '            rpt.Database.Tables(intCounter).ApplyLogOnInfo(ConInfo)
        '        Next

        '    Catch ex As Exception

        '    End Try
        'End Sub

        'Public Sub SetCrystalReportParameterValues(ByRef rpt As CrystalDecisions.CrystalReports.Engine.ReportDocument)
        '    Try
        '        'Check if there are parameters or not in report.
        '        Dim intCounter As Integer = rpt.DataDefinition.ParameterFields.Count
        '        'Dim strParValPair() As String
        '        Dim paraValue As New CrystalDecisions.Shared.ParameterDiscreteValue

        '        'Current parameter value object(collection) of crystal report parameters.
        '        Dim currValue As CrystalDecisions.Shared.ParameterValues

        '        If intCounter > 0 Then
        '            For index As Integer = 0 To intCounter - 1
        '                Dim ParmFld As String = rpt.DataDefinition.ParameterFields.Item(index).ParameterFieldName 'ParameterList.Item(index).ParameterFiled.ToString
        '                Dim ParmVal As String = ParameterList.Item(index).ParameterValue.ToString
        '                paraValue.Value = ParmVal
        '                currValue = rpt.DataDefinition.ParameterFields(ParmFld).CurrentValues
        '                currValue.Add(paraValue)
        '                rpt.DataDefinition.ParameterFields(ParmFld).ApplyCurrentValues(currValue)
        '            Next
        '        End If
        '    Catch ex As Exception

        '    End Try
        'End Sub

        Public Sub GridInitialization(ByRef Grid As SAPbouiCOM.Grid, ByVal CollapseLevel As Integer)
            Try
                For iCol As Integer = 0 To Grid.DataTable.Columns.Count - 1
                    If Grid.DataTable.Columns.Item(iCol).Type = 5 Or Grid.DataTable.Columns.Item(iCol).Type = 2 Or Grid.DataTable.Columns.Item(iCol).Type = 12 Or Grid.DataTable.Columns.Item(iCol).Type = 8 Or Grid.DataTable.Columns.Item(iCol).Type = 7 Or Grid.DataTable.Columns.Item(iCol).Type = 6 Or Grid.DataTable.Columns.Item(iCol).Type = 9 Or Grid.DataTable.Columns.Item(iCol).Type = 11 Then
                        Grid.Columns.Item(iCol).RightJustified = True
                    End If
                    Grid.Columns.Item(iCol).Editable = False
                Next

                'For iRow As Integer = 0 To Grid.Rows.Count
                '    Grid.RowHeaders.SetText(iRow, iRow + 1.ToString)
                'Next
                Grid.CollapseLevel = CollapseLevel
            Catch ex As Exception

            End Try
        End Sub

        Public Function CheckFromToDate(ByVal FromDate As Date, ByVal ToDate As Date) As Boolean
            Try

                If Not IsDate(FromDate) Then Return False : Exit Function
                If Not IsDate(ToDate) Then Return False : Exit Function

                If FromDate > ToDate Then Return False : Exit Function

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Sub AddChooseFromList(ByVal ObjectType As String, ByVal UniqueID As String, ByVal oForm As SAPbouiCOM.Form)
            Dim oCFLs As SAPbouiCOM.ChooseFromListCollection

            oCFLs = oForm.ChooseFromLists

            Dim oCFL As SAPbouiCOM.ChooseFromList
            Dim oCFLCreationParams As SAPbouiCOM.ChooseFromListCreationParams

            oCFLCreationParams = __Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams)

            ' Adding 2 CFL, one for the button and one for the edit text.
            oCFLCreationParams.MultiSelection = False
            oCFLCreationParams.ObjectType = ObjectType
            oCFLCreationParams.UniqueID = UniqueID

            oCFL = oCFLs.Add(oCFLCreationParams)
        End Sub

        Public Function GetColorCode(ByVal Color As System.Drawing.Color) As Integer
            Try
                Dim ColorCode As Integer = RGB(Color.R, Color.G, Color.B)
                Return ColorCode
            Catch ex As Exception
                Return 0
            End Try
        End Function

        Public Function GetValue(ByVal Field As String, ByVal TableName As String, ByVal Condition As String) As String
            Try
                GetValue = ""
                Dim oGetValue As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

                Dim Sql As String = ""
                Sql = "Select " + Field + " From " + TableName + IIf(Condition = "", "", " Where " + Condition)

                oGetValue.DoQuery(Sql)

                If oGetValue.RecordCount > 0 Then
                    GetValue = oGetValue.Fields.Item(0).Value & ""
                End If

                Return GetValue
            Catch ex As Exception
                Return ""
            End Try
        End Function

        Public Function GetValue(ByVal SqlQuery As String, ByVal Field As String) As String
            Try
                GetValue = ""
                Dim oGetValue As SAPbobsCOM.Recordset = __bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)

                Dim Sql As String = ""
                Sql = SqlQuery

                oGetValue.DoQuery(Sql)

                If oGetValue.RecordCount > 0 Then
                    GetValue = oGetValue.Fields.Item(Field).Value & ""
                End If

                Return GetValue
            Catch ex As Exception
                Return ""
            End Try
        End Function

        Public Sub SendData(ByRef refHashTable As Hashtable, ByRef IsBaseForm As SAPbouiCOM.Form)
            Try
                gHashTable = Nothing

                gHashTable = refHashTable

                If IsBaseForm.TypeEx = "SAP_UDO_OIT" Then
                    Dim c_M_InvTrans As Inv_Transfer = New Inv_Transfer(__Application, __bobCompany)
                    c_M_InvTrans.ReceivedData(gHashTable, IsBaseForm)

                ElseIf IsBaseForm.TypeEx = "140" Then
                    Dim C_Delivery As C_Delivery = New C_Delivery(__Application, __bobCompany)
                    C_Delivery.ReceivedData(gHashTable, IsBaseForm)

                ElseIf IsBaseForm.TypeEx = "142" Then
                    Dim C_PurchaseOrder As C_PurchaseOrder = New C_PurchaseOrder(__Application, __bobCompany)
                    C_PurchaseOrder.ReceivedData(gHashTable, IsBaseForm)

                ElseIf IsBaseForm.TypeEx = "143" Then
                    Dim C_GRN As C_GRN = New C_GRN(__Application, __bobCompany)
                    C_GRN.ReceivedData(gHashTable, IsBaseForm)


                ElseIf IsBaseForm.TypeEx = "85" Then
                    Dim C_PickList As C_PickList = New C_PickList(__Application, __bobCompany)
                    C_PickList.ReceivedData(gHashTable, IsBaseForm)

                End If
            Catch ex As Exception

            End Try
        End Sub




    End Module
End Namespace

