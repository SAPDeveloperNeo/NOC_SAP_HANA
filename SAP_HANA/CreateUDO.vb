
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
Imports SAPbobsCOM
Imports SAPbouiCOM


Namespace SAP_HANA
    Module CreateUDO

        Dim oUserTablesMD As SAPbobsCOM.UserTablesMD
        <DebuggerDisplay("Value : {sValue}, Description : {sDescription}")>
        Structure ValidValues
            Dim sValue As String
            Dim sDescription As String
            Public Sub New(ByVal Value As String, ByVal Description As String)
                sValue = Value
                sDescription = Description
            End Sub

            Public Property Description() As String
                Get
                    Return sDescription
                End Get
                Set(ByVal value As String)
                    sDescription = value
                End Set
            End Property

            Public Property Value() As String
                Get
                    Return sValue
                End Get
                Set(ByVal value As String)
                    sValue = value
                End Set
            End Property

        End Structure

#Region "Create Table"

        Public Sub AddTables(ByVal strTab As String, ByVal strDesc As String, ByVal nType As SAPbobsCOM.BoUTBTableType)
            GC.Collect()

            Try

                oProgressBar.Text = "Creating Table.... " & strTab
                ' oUserTablesMD = New SAPbobsCOM.UserTablesMD
                'bobCompany = SAPApplication.Company.GetDICompany
                oUserTablesMD = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserTables)
                'Adding Table
                If Not oUserTablesMD.GetByKey(strTab) Then
                    oUserTablesMD.TableName = strTab
                    oUserTablesMD.TableDescription = strDesc
                    oUserTablesMD.TableType = nType
                    If oUserTablesMD.Add <> 0 Then
                        Throw New Exception(bobCompany.GetLastErrorDescription)
                    End If
                End If
            Catch ex As Exception
                Throw ex
            Finally
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oUserTablesMD)
                oUserTablesMD = Nothing
                GC.WaitForPendingFinalizers()
                GC.Collect()
            End Try
        End Sub

#End Region

#Region "Add Field"

        Public Sub AddFields(ByVal tablename As String, ByVal fieldName As String, ByVal Description As String, ByVal datatype As SAPbobsCOM.BoFieldTypes, ByVal subdatatype As SAPbobsCOM.BoFldSubTypes, ByVal size As Integer, ByVal validvalue As ArrayList, ByVal defaultValue As String, ByVal SetLinkTable As Boolean, ByVal LinkTable As String)
            Dim oUserFieldsMD As SAPbobsCOM.UserFieldsMD = Nothing
            Dim errDesc As String = ""

            Try
                oProgressBar.Text = "Creating Table.... " & tablename & " : Field Name : " & fieldName
                'Do the coding for adding fields.
                oUserFieldsMD = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserFields)
                oUserFieldsMD.TableName = tablename

                oUserFieldsMD.Name = fieldName
                oUserFieldsMD.Description = Description
                oUserFieldsMD.Type = datatype
                oUserFieldsMD.SubType = subdatatype
                If subdatatype <> 83 Then
                    oUserFieldsMD.Size = size
                    oUserFieldsMD.EditSize = size
                End If

                If SetLinkTable = True Then
                    If LinkTable <> "" Then
                        oUserFieldsMD.LinkedTable = LinkTable
                    End If
                Else
                    'For checking valid values
                    If Not validvalue Is Nothing Then
                        For iVal As Integer = 0 To validvalue.Count - 1
                            oUserFieldsMD.ValidValues.Value = validvalue.Item(iVal).Value
                            oUserFieldsMD.ValidValues.Description = validvalue.Item(iVal).Description
                            oUserFieldsMD.ValidValues.Add()
                        Next
                        'For Each vall As ValidValues In validvalue
                        '    oUserFieldsMD.ValidValues.Value = vall.Value
                        '    oUserFieldsMD.ValidValues.Description = vall.Description
                        '    oUserFieldsMD.ValidValues.Add()
                        'Next

                        If defaultValue <> "" Then
                            oUserFieldsMD.DefaultValue = defaultValue
                        End If
                    End If
                End If



                Dim UDFCode As Integer = oUserFieldsMD.Add()
                If UDFCode = 0 Then
                    errDesc = "The Field " & fieldName & " is Added in table " & tablename & "."
                End If
                If UDFCode <> 0 Then
                    errDesc = bobCompany.GetLastErrorDescription()
                End If
                'oProgressBar.Value = oProgressBar.Value + 1
            Catch ex As Exception
                errDesc = ex.Message
            Finally
                Marshal.ReleaseComObject(oUserFieldsMD)
                GC.Collect()
            End Try
        End Sub

#End Region

#Region "Register Object"

        Public Function RegisterObject(ByVal Code As String, ByVal Name As String, ByVal ObjectType As String, ByVal TableName As String, ByVal ChildTableName As String, ByVal CanCancel As Boolean, ByVal CanClose As Boolean, ByVal CanCreateDefaultForm As Boolean, ByVal CanDelete As Boolean, ByVal CanFind As Boolean, ByVal CanYearTransfer As Boolean, ByVal ManageSeries As Boolean, ByVal FindColumns As String, ByVal defaultform As Boolean, ByVal defaultformFields As String, ByVal CanLog As Boolean) As Integer
            Try
                Dim errCode As Integer
                Dim errMsg As String = ""
                oProgressBar.Text = "Registering Table.... " & TableName
                Dim UDO As SAPbobsCOM.UserObjectsMD = bobCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserObjectsMD) 'oApplication.Company.GetDICompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oUserObjectsMD) '
                If UDO.GetByKey(Code) = False Then
                    UDO.CanCancel = GetYesNo(CanCancel)
                    UDO.CanClose = GetYesNo(CanClose)
                    UDO.CanDelete = GetYesNo(CanDelete)
                    UDO.CanFind = GetYesNo(CanFind)
                    UDO.CanCreateDefaultForm = GetYesNo(defaultform)
                    UDO.ManageSeries = GetYesNo(ManageSeries)
                    UDO.CanLog = GetYesNo(CanLog)


                    If UDO.CanCreateDefaultForm = SAPbobsCOM.BoYesNoEnum.tYES Then
                        Dim s As String() = defaultformFields.Split(",")
                        For Each o As String In s
                            Dim s1 As String() = o.Split("|")
                            If s1.Length > 0 Then UDO.FormColumns.FormColumnAlias = s1(0).ToString()
                            If s1.Length > 1 Then UDO.FormColumns.FormColumnDescription = s1(1).ToString()
                            UDO.FormColumns.Add()
                        Next
                    End If

                    If UDO.CanFind = SAPbobsCOM.BoYesNoEnum.tYES Then
                        If FindColumns <> "" Then
                            Dim s As String() = FindColumns.Split(",")
                            'If s.Length > 1 Then
                            If s.Length > 0 Then
                                For Each o As String In s
                                    Dim s1 As String = ""
                                    If o.ToString() = "Code" Or o.ToString() = "Name" Or o.ToString() = "DocNum" Or o.ToString() = "DocEntry" Then
                                        s1 = o.ToString()
                                    Else
                                        s1 = "U_" & o.ToString()
                                    End If
                                    UDO.FindColumns.ColumnAlias = s1
                                    UDO.FindColumns.Add()
                                Next
                            End If
                        End If
                    End If

                    If UDO.CanLog = SAPbobsCOM.BoYesNoEnum.tYES Then
                        UDO.LogTableName = "A" & TableName
                    End If
                    UDO.Code = Code
                    UDO.Name = Name
                    UDO.ObjectType = GetUDOType(ObjectType)
                    UDO.TableName = TableName

                    If ChildTableName <> "" Then 'UDO.ChildTables.TableName = ChildTableName
                        Dim childTables As String() = ChildTableName.Split(",")
                        If childTables.Length > 0 Then
                            For Each o As String In childTables
                                UDO.ChildTables.TableName = o.ToString
                                UDO.ChildTables.Add()
                            Next
                        End If
                    End If
                    Dim i As Integer = UDO.Add()

                    If i <> 0 Then
                        bobCompany.GetLastError(errCode, errMsg)
                        System.Windows.Forms.MessageBox.Show(errMsg)
                        SAPApplication.StatusBar.SetText(errMsg, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error)
                        UDO.Update()
                        If Marshal.IsComObject(UDO) Then
                            Marshal.ReleaseComObject(UDO)
                            UDO = Nothing
                            GC.Collect()
                            Return errCode
                        End If
                    End If
                    UDO.Update()
                    __Application.MetadataAutoRefresh = True
                    If Marshal.IsComObject(UDO) Then Marshal.ReleaseComObject(UDO)
                    UDO = Nothing
                    'GC.WaitForPendingFinalizers();
                    GC.Collect()
                    'oProgressBar.Value = oProgressBar.Value + 1
                    Return 0
                Else
                    'oProgressBar.Value = oProgressBar.Value + 1
                    SAPApplication.StatusBar.SetText("Table Already Exists......", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_None)
                End If
            Catch ex As Exception
                GC.Collect()
                Return -1
            End Try
        End Function

        Private Function GetYesNo(ByVal s As Boolean) As SAPbobsCOM.BoYesNoEnum
            If s = True Then
                Return SAPbobsCOM.BoYesNoEnum.tYES
            Else
                Return SAPbobsCOM.BoYesNoEnum.tNO
            End If
        End Function

        Private Function GetUDOType(ByVal Type As String) As SAPbobsCOM.BoUDOObjType
            If Type = "MD" Then
                Return SAPbobsCOM.BoUDOObjType.boud_MasterData
            Else
                Return SAPbobsCOM.BoUDOObjType.boud_Document
            End If
        End Function

#End Region

    End Module
End Namespace

