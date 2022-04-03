Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Namespace ganesh.Dataccess
    Public Class Cls_DataAccess
        Inherits System.ComponentModel.Component
        Private ConnOLE As OleDbConnection 'Global OLEDB Connection object
        Private ConnSQL As SqlConnection 'Global SQL Connection object
        Private mbIsOLEConn As Boolean = False
        Private mbUid As String '= "sa"
        Private mbPass As String '= ""
        Private mbDataSource As String '= "MAINSERVER"
        Private mbIniCatlog As String '= "GIDC_2009"
        Private mbDb As String
        Private mbprovider As String
        Private mbserver As String
        Private OleTrans As OleDb.OleDbTransaction
        Private bol_Ole_begintrans As Boolean
        Private SqlTrans As SqlTransaction
        Private bol_sql_begintrans As Boolean
        Private intReturnCode As Integer

        'Property for Connection type (OLE connection or not) By Default it is SQL Connection
        Public Property prop_IsOLEConn() As Boolean
            Get
                prop_IsOLEConn = mbIsOLEConn
            End Get
            Set(ByVal Value As Boolean)
                mbIsOLEConn = Value
            End Set
        End Property
        Public Property prop_user_Id() As String
            Get
                prop_user_Id = mbUid
            End Get
            Set(ByVal Value As String)
                mbUid = Value
            End Set
        End Property
        Public Property prop_Password() As String
            Get
                prop_Password = mbPass
            End Get
            Set(ByVal Value As String)
                mbPass = Value
            End Set
        End Property
        Public Property prop_provider() As String
            Get
                prop_provider = mbprovider
            End Get
            Set(ByVal Value As String)
                mbprovider = Value
            End Set
        End Property
        Public Property prop_datasource() As String
            Get
                prop_datasource = mbDataSource
            End Get
            Set(ByVal Value As String)
                mbDataSource = Value
            End Set
        End Property
        Public Property prop_database() As String
            Get
                prop_database = mbDb
            End Get
            Set(ByVal Value As String)
                mbDb = Value
            End Set
        End Property
        Public Property prop_server() As String
            Get
                prop_server = mbserver
            End Get
            Set(ByVal Value As String)
                mbserver = Value
            End Set
        End Property
        Public Property prop_iniCatlog() As String
            Get
                prop_iniCatlog = mbIniCatlog
            End Get
            Set(ByVal Value As String)
                mbIniCatlog = Value
            End Set
        End Property

        Public ReadOnly Property ReturnCode() As Integer
            Get
                ReturnCode = intReturnCode
            End Get
        End Property

        Public Function GetDataTable_on_Text(ByVal asProcName() As String, ByRef dtResult() As DataTable) As Boolean
            Try
                'Dim catCMD As OleDbCommand
                Dim stEle As Short
                Dim dtSet As DataSet
                Dim dtTable As DataTable

                'msREG_KEY = sREG_KEY
                ReDim dtResult(UBound(asProcName))
                If mbIsOLEConn = True Then
                    'Dim myReader As OleDbDataReader
                    Dim dtCmd As OleDbDataAdapter
                    OpenOLEConnection(mbprovider, mbUid, mbPass, mbDb, mbserver)
                    For stEle = 0 To UBound(asProcName)
                        ' Create a Command object with the SQL statement.
                        dtCmd = New OleDbDataAdapter(asProcName(stEle), ConnOLE)
                        ' Fill a DataSet with data returned from the database.
                        dtSet = New DataSet
                        dtCmd.Fill(dtSet)

                        ' Create a new DataTable object and assign to it
                        ' the new table in the Tables collection.
                        dtTable = New DataTable
                        dtTable = dtSet.Tables(0)
                        dtResult(stEle) = dtTable
                    Next
                    ConnOLE.Close()
                Else
                    Dim dtCmdSQL As SqlDataAdapter
                    OpenSQLConnection(mbUid, mbPass, mbIniCatlog, mbDataSource)
                    For stEle = 0 To UBound(asProcName)
                        ' Create a Command object with the SQL statement.
                        dtCmdSQL = New SqlDataAdapter(asProcName(stEle), ConnSQL)
                        dtCmdSQL.SelectCommand.CommandTimeout = 3600
                        ' Fill a DataSet with data returned from the database.
                        dtSet = New DataSet
                        dtCmdSQL.Fill(dtSet)

                        ' Create a new DataTable object and assign to it
                        ' the new table in the Tables collection.
                        dtTable = New DataTable
                        dtTable = dtSet.Tables(0)
                        dtResult(stEle) = dtTable
                    Next
                    ConnSQL.Close()
                End If
                GetDataTable_on_Text = True
            Catch ex As Exception
                'MsgBox(ex.Message)
                Err.Raise(Err.Number, Err.Source, Err.Description)
            End Try
        End Function
        'Procedure for opening SQLDB connection
        Private Function OpenSQLConnection(ByVal v_uid As String, ByVal V_Pass As String, ByVal v_ini_catlog As String, ByVal v_data_source As String) As Boolean
            Dim sConnString As String
            Try
                sConnString = "user id=" & v_uid & ";password=" & V_Pass & ";initial catalog=" & v_ini_catlog & ";data source=" & v_data_source & "; connection timeout = 600"
                ConnSQL = New SqlConnection(sConnString)
                ConnSQL.Open()
            Catch ex As Exception
                Err.Raise(Err.Number, Err.Source, Err.Description)
            End Try
        End Function
        'Procedure for opening OLEDB connection
        Private Function OpenOLEConnection(ByVal v_provider As String, ByVal v_uid As String, ByVal v_pass As String, ByVal v_db As String, ByVal v_server As String) As Boolean
            Dim sConnString As String
            Try
                If mbUid <> "" Then
                    sConnString = "provider=" & v_provider & ";UID=" & v_uid & ";PWD=" & v_pass & ";database =" & v_db & ";server=" & v_server
                Else
                    sConnString = "provider=" & v_provider & "; data source = " & v_db & ";"
                End If
                ConnOLE = New OleDbConnection(sConnString)
                ConnOLE.Open()
            Catch ex As Exception
                Err.Raise(Err.Number, Err.Source, Err.Description)
            End Try
        End Function
        'Method for getting DataSet by executing Store Procedure
        Public Function GetDataSet_on_Text(ByVal asProcName() As String, ByRef dsResult() As DataSet) As Boolean
            Try
                Dim stEle As Short
                Dim dtSet As DataSet
                If mbIsOLEConn = True Then
                    Dim dtCmd As OleDbDataAdapter
                    OpenOLEConnection(mbprovider, mbUid, mbPass, mbDataSource, mbserver)
                    ReDim dsResult(UBound(asProcName))
                    For stEle = 0 To UBound(asProcName)
                        ' Create a Command object with the SQL statement.
                        dtCmd = New OleDbDataAdapter(asProcName(stEle), ConnOLE)
                        ' Fill a DataSet with data returned from the database.
                        dtSet = New DataSet
                        dtCmd.Fill(dtSet)
                        dsResult(stEle) = dtSet
                    Next
                    ConnOLE.Close()
                Else
                    'Using SQL Connection
                    Dim dtCmdSQL As SqlDataAdapter
                    OpenSQLConnection(mbUid, mbPass, mbIniCatlog, mbDataSource)
                    ReDim dsResult(UBound(asProcName))
                    For stEle = 0 To UBound(asProcName)
                        Try
                            ' Create a Command object with the SQL statement.
                            dtCmdSQL = New SqlDataAdapter(asProcName(stEle), ConnSQL)
                            ' Fill a DataSet with data returned from the database.
                            dtSet = New DataSet
                            dtCmdSQL.Fill(dtSet)
                            dsResult(stEle) = dtSet
                            System.Diagnostics.Debug.WriteLine("getDataSet::Any Error?>>" & Err.Description & "<<" & asProcName(stEle))
                        Catch
                            System.Diagnostics.Debug.WriteLine("getDataSet::Any Exception??" & Err.Description & "??" & "<<" & asProcName(stEle))
                        End Try
                    Next
                    ConnSQL.Close()
                End If
                GetDataSet_on_Text = True
            Catch ex As Exception
                'MsgBox(ex.Message)
                Err.Raise(Err.Number, Err.Source, Err.Description)
            End Try
        End Function
#Region " Component Designer generated code "
        Public Sub New(ByVal Container As System.ComponentModel.IContainer)
            MyClass.New()
            'Required for Windows.Forms Class Composition Designer support
            Container.Add(Me)
        End Sub

        Public Sub New()
            MyBase.New()
            'This call is required by the Component Designer.
            InitializeComponent()
            'Add any initialization after the InitializeComponent() call
        End Sub

        'Component overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Component Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Component Designer
        'It can be modified using the Component Designer.
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            components = New System.ComponentModel.Container
        End Sub

#End Region
        Public Function UpdateInfo_On_Text(ByVal arrProcs() As String, Optional ByRef rsError As String = "") As Boolean
            Try
                Dim iEle As Integer
                If mbIsOLEConn = True Then
                    Dim oleComm As New OleDbCommand
                    If bol_Ole_begintrans = False Then
                        OpenOLEConnection(mbprovider, mbUid, mbPass, mbDb, mbserver)
                    Else
                        oleComm.Transaction = OleTrans
                    End If
                    oleComm.Connection = ConnOLE
                    oleComm.CommandType = CommandType.Text
                    For iEle = 0 To UBound(arrProcs)
                        oleComm.CommandTimeout = 3600
                        oleComm.CommandText = arrProcs(iEle)
                        oleComm.ExecuteNonQuery()
                    Next
                    If bol_sql_begintrans = False Then
                        ConnOLE.Close()
                        ConnOLE = Nothing
                    End If
                Else
                    'using SQL Connection
                    Dim sqlcomm As New SqlCommand
                    If bol_sql_begintrans = False Then
                        OpenSQLConnection(mbUid, mbPass, mbIniCatlog, mbDataSource)
                    Else
                        sqlcomm.Transaction = SqlTrans
                    End If
                    sqlcomm.Connection = ConnSQL
                    sqlcomm.CommandType = CommandType.Text
                    'Dim sTempVar As String
                    For iEle = 0 To UBound(arrProcs)
                        sqlcomm.CommandText = arrProcs(iEle)
                        Try
                            sqlcomm.CommandTimeout = 3600
                            sqlcomm.ExecuteNonQuery()
                            System.Diagnostics.Debug.WriteLine("updateInfo ::any error?>>" & Err.Description & "<<" & arrProcs(iEle))
                        Catch
                            'MsgBox(Err.Description)
                            System.Diagnostics.Debug.WriteLine("updateInfo ::Any Exception??" & Err.Description & "??" & "<<" & arrProcs(iEle))
                        End Try
                        'sTempVar = arrProcs(iEle)
                        'Console.WriteLine(sTempVar)
                        'Console.Out.Flush()
                    Next
                    If bol_sql_begintrans = False Then
                        ConnSQL.Close()
                        ConnSQL = Nothing
                    End If
                End If
                UpdateInfo_On_Text = True
            Catch ex As Exception
                'MsgBox(ex.Message)
                'Err.Raise()
                Err.Raise(Err.Number, Err.Source, Err.Description)
                UpdateInfo_On_Text = False
            End Try
        End Function
        Public Function getMixData_on_Text(ByVal asProcName() As String, ByRef dsResult() As DataSet) As Boolean
            Try
                Dim stEle As Short
                Dim dtSet As DataSet
                
                If mbIsOLEConn = True Then
                    Dim dtCmd As OleDbDataAdapter
                    OpenOLEConnection(mbprovider, mbUid, mbPass, mbDb, mbserver)
                    ReDim dsResult(UBound(asProcName))
                    For stEle = 0 To UBound(asProcName)
                        ' Create a Command object with the SQL statement.
                        dtCmd = New OleDbDataAdapter(asProcName(stEle), ConnOLE)
                        ' Fill a DataSet with data returned from the database.
                        dtSet = New DataSet
                        dtCmd.Fill(dtSet)
                        dsResult(stEle) = dtSet
                    Next
                    ConnOLE.Close()
                Else
                    'Using SQL Connection
                    Dim dtCmdSQL As SqlDataAdapter
                    OpenSQLConnection(mbUid, mbPass, mbIniCatlog, mbDataSource)
                    ReDim dsResult(UBound(asProcName))
                    Dim sqlcomm As New SqlCommand
                    sqlcomm.Connection = ConnSQL
                    sqlcomm.CommandType = CommandType.Text
                    sqlcomm.CommandText = asProcName(0)
                    Try
                        sqlcomm.ExecuteNonQuery()
                        System.Diagnostics.Debug.WriteLine("getmixData::update::Any Error?>>" & Err.Description & "<<" & asProcName(0))
                    Catch
                        System.Diagnostics.Debug.WriteLine("get mixData::update::Any Exception??" & Err.Description & "??")
                    End Try
                    For stEle = 1 To UBound(asProcName)
                        Try
                            ' Create a Command object with the SQL statement.
                            dtCmdSQL = New SqlDataAdapter(asProcName(stEle), ConnSQL)
                            dtSet = New DataSet
                            dtCmdSQL.Fill(dtSet)
                            dsResult(stEle) = dtSet
                            System.Diagnostics.Debug.WriteLine("get mixData::select::Any Error?>>" & Err.Description & "<<" & asProcName(stEle))
                        Catch
                            System.Diagnostics.Debug.WriteLine("get mixData::select::Any Exception??" & Err.Description & "??")
                            Err.Raise(Err.Number, Err.Source, Err.Description)
                        End Try
                    Next
                    ConnSQL.Close()
                End If
                getMixData_on_Text = True
            Catch ex As Exception
                'MsgBox(ex.Message)
                Err.Raise(Err.Number, Err.Source, Err.Description)
            End Try
        End Function
        Public Function getDataReader_on_Text(ByVal arrProcs() As String, ByRef dReaderResult() As SqlDataReader, Optional ByRef rsError As String = "") As Boolean
            Try
                Dim iEle As Integer

                Dim sqlcomm As New SqlCommand
                If bol_sql_begintrans = False Then
                    OpenSQLConnection(mbUid, mbPass, mbIniCatlog, mbDataSource)
                Else
                    sqlcomm.Transaction = SqlTrans
                End If

                sqlcomm.Connection = ConnSQL
                sqlcomm.CommandType = CommandType.Text

                ReDim dReaderResult(UBound(arrProcs))

                For iEle = 0 To UBound(arrProcs)
                    sqlcomm.CommandText = arrProcs(iEle)
                    dReaderResult(iEle) = sqlcomm.ExecuteReader()
                Next

                ConnSQL.Close()
                ConnSQL = Nothing
                getDataReader_on_Text = True
            Catch ex As Exception
                'MsgBox(ex.Message)
                Err.Raise(Err.Number, Err.Source, Err.Description)
            End Try
        End Function



        Public Function GetDataTable_on_Sp(ByVal asProcName() As String, ByRef dtResult() As DataTable, Optional ByVal SqlParam() As SqlParameter = Nothing, Optional ByVal oleparam() As OleDbParameter = Nothing) As Boolean
            Try
                'Dim catCMD As OleDbCommand
                Dim stEle As Short
                Dim sqlEle As Short
                'Dim dtSet As DataSet
                Dim dtTable As DataTable

                'msREG_KEY = sREG_KEY
                ReDim dtResult(UBound(asProcName))
                If mbIsOLEConn = True Then
                    'Dim myReader As OleDbDataReader
                    Dim dtadp As New OleDbDataAdapter
                    Dim dtcmd As New OleDb.OleDbCommand
                    If ole_begin_trans() = False Then
                        OpenOLEConnection(mbprovider, mbUid, mbPass, mbDb, mbserver)
                    Else
                        dtcmd.Transaction = OleTrans
                    End If
                    For stEle = 0 To UBound(asProcName)
                        ' Create a Command object with the SQL statement.
                        dtcmd.Connection = ConnOLE
                        dtcmd.CommandText = asProcName(stEle)
                        dtcmd.CommandTimeout = 0
                        If IsNothing(oleparam) = False Then
                            For sqlEle = 0 To UBound(oleparam)
                                dtcmd.Parameters.Add(oleparam(sqlEle))
                            Next
                        End If

                        ' Fill a DataSet with data returned from the database.
                        dtTable = New DataTable
                        dtadp.SelectCommand = dtcmd
                        dtadp.Fill(dtTable)

                        ' Create a new DataTable object and assign to it
                        ' the new table in the Tables collection.
                        'dtTable = New DataTable
                        'dtTable = dtSet.Tables(0)
                        dtResult(stEle) = dtTable
                    Next
                    If ole_begin_trans() = False Then
                        ConnOLE.Close()
                    End If
                Else
                    Dim dtCmdSQL As New SqlCommand
                    Dim dtAdpSql As New SqlDataAdapter
                    If bol_sql_begintrans = False Then
                        OpenSQLConnection(mbUid, mbPass, mbIniCatlog, mbDataSource)
                    Else
                        dtCmdSQL.Transaction = SqlTrans
                    End If
                    For stEle = 0 To UBound(asProcName)
                        ' Create a Command object with the SQL statement.
                        'dtCmdSQL = New SqlDataAdapter(asProcName(stEle), ConnSQL)
                        dtCmdSQL.Connection = ConnSQL
                        dtCmdSQL.CommandText = asProcName(stEle)
                        dtCmdSQL.CommandType = CommandType.StoredProcedure
                        dtCmdSQL.CommandTimeout = 0

                        If IsNothing(SqlParam) = False Then
                            For sqlEle = 0 To UBound(SqlParam)
                                dtCmdSQL.Parameters.Add(SqlParam(sqlEle))
                            Next
                        End If

                        ' Fill a DataSet with data returned from the database.
                        'dtSet = New DataSet
                        dtTable = New DataTable
                        dtAdpSql.SelectCommand = dtCmdSQL
                        dtAdpSql.Fill(dtTable)

                        ' Create a new DataTable object and assign to it
                        ' the new table in the Tables collection.

                        'dtTable = New DataTable
                        'dtTable = dtTable.Tables(0)

                        dtResult(stEle) = dtTable
                    Next
                    If bol_sql_begintrans = False Then
                        ConnSQL.Close()
                    End If
                End If
                GetDataTable_on_Sp = True
            Catch ex As Exception
                'MsgBox(ex.Message)
                Err.Raise(Err.Number, Err.Source, Err.Description)
            End Try
        End Function

        Public Function UpdateInfo_On_Sp1(ByVal arrProcs() As String, Optional ByVal SqlParam() As SqlParameter = Nothing, Optional ByVal oleparam() As OleDb.OleDbParameter = Nothing) As Boolean
            Try
                Dim iEle As Integer
                Dim sEle As Short
                If mbIsOLEConn = True Then
                    Dim oleComm As New OleDbCommand
                    If bol_Ole_begintrans = False Then
                        OpenOLEConnection(mbprovider, mbUid, mbPass, mbDb, mbserver)
                    Else
                        oleComm.Transaction = OleTrans
                    End If
                    oleComm.CommandTimeout = 3600
                    oleComm.Connection = ConnOLE
                    oleComm.CommandType = CommandType.StoredProcedure
                    For iEle = 0 To UBound(arrProcs)
                        oleComm.CommandText = arrProcs(iEle)
                        If IsNothing(oleparam) = False Then
                            For sEle = 0 To UBound(oleparam)
                                oleComm.Parameters.Add(oleparam(sEle))
                            Next
                        End If
                        oleComm.ExecuteNonQuery()
                    Next
                    If bol_Ole_begintrans = False Then
                        ConnOLE.Close()
                        ConnOLE = Nothing
                    End If
                Else
                    'using SQL Connection
                    Dim sqlcomm As New SqlCommand
                    If bol_sql_begintrans = False Then
                        OpenSQLConnection(mbUid, mbPass, mbIniCatlog, mbDataSource)
                    Else
                        sqlcomm.Transaction = SqlTrans
                    End If
                    sqlcomm.Connection = ConnSQL
                    sqlcomm.CommandType = CommandType.StoredProcedure
                    'Dim sTempVar As String
                    For iEle = 0 To UBound(arrProcs)
                        'Try
                        sqlcomm.CommandTimeout = 30000
                        sqlcomm.CommandText = arrProcs(iEle)
                        sqlcomm.CommandType = CommandType.StoredProcedure

                        If IsNothing(SqlParam) = False Then
                            For sEle = 0 To UBound(SqlParam)
                                'MsgBox(SqlParam(sEle).Value)
                                sqlcomm.Parameters.Add(SqlParam(sEle))
                            Next
                        End If

                        sqlcomm.ExecuteNonQuery()
                        intReturnCode = sqlcomm.Parameters("@ReturnCode").Value
                        'System.Diagnostics.Debug.WriteLine("updateInfo ::any error?>>" & Err.Description & "<<" & arrProcs(iEle))
                        UpdateInfo_On_Sp1 = True
                        'Catch ex As Exception
                        '    System.Diagnostics.Debug.WriteLine("updateInfo ::Any Exception??" & Err.Description & "??" & "<<" & arrProcs(iEle))
                        '    MsgBox(ex.Message)
                        '    End Try
                        'sTempVar = arrProcs(iEle)
                        'Console.WriteLine(sTempVar)
                        'Console.Out.Flush()
                    Next
                    If bol_sql_begintrans = False Then
                        ConnSQL.Close()
                        ConnSQL = Nothing
                    End If
                End If
                UpdateInfo_On_Sp1 = True
            Catch ex As Exception
                Err.Raise(Err.Number, Err.Source, Err.Description)
                UpdateInfo_On_Sp1 = False
            End Try
        End Function

        Public Function UpdateInfo_On_Sp(ByVal arrProcs() As String, Optional ByVal SqlParam() As SqlParameter = Nothing, Optional ByVal oleparam() As OleDb.OleDbParameter = Nothing) As Boolean
            Try
                Dim iEle As Integer
                Dim sEle As Short
                If mbIsOLEConn = True Then
                    Dim oleComm As New OleDbCommand
                    If bol_Ole_begintrans = False Then
                        OpenOLEConnection(mbprovider, mbUid, mbPass, mbDb, mbserver)
                    Else
                        oleComm.Transaction = OleTrans
                    End If
                    oleComm.CommandTimeout = 3600
                    oleComm.Connection = ConnOLE
                    oleComm.CommandType = CommandType.StoredProcedure
                    For iEle = 0 To UBound(arrProcs)
                        oleComm.CommandText = arrProcs(iEle)
                        If IsNothing(oleparam) = False Then
                            For sEle = 0 To UBound(oleparam)
                                oleComm.Parameters.Add(oleparam(sEle))
                            Next
                        End If
                        oleComm.ExecuteNonQuery()
                    Next
                    If bol_Ole_begintrans = False Then
                        ConnOLE.Close()
                        ConnOLE = Nothing
                    End If
                Else
                    'using SQL Connection
                    Dim sqlcomm As New SqlCommand
                    If bol_sql_begintrans = False Then
                        OpenSQLConnection(mbUid, mbPass, mbIniCatlog, mbDataSource)
                    Else
                        sqlcomm.Transaction = SqlTrans
                    End If
                    sqlcomm.Connection = ConnSQL
                    sqlcomm.CommandType = CommandType.StoredProcedure
                    'Dim sTempVar As String
                    For iEle = 0 To UBound(arrProcs)
                        'Try
                        sqlcomm.CommandTimeout = 30000
                        sqlcomm.CommandText = arrProcs(iEle)
                        sqlcomm.CommandType = CommandType.StoredProcedure

                        If IsNothing(SqlParam) = False Then
                            For sEle = 0 To UBound(SqlParam)
                                'MsgBox(SqlParam(sEle).Value)
                                sqlcomm.Parameters.Add(SqlParam(sEle))
                            Next
                        End If

                        sqlcomm.ExecuteNonQuery()
                        'System.Diagnostics.Debug.WriteLine("updateInfo ::any error?>>" & Err.Description & "<<" & arrProcs(iEle))
                        UpdateInfo_On_Sp = True
                        'Catch ex As Exception
                        '    System.Diagnostics.Debug.WriteLine("updateInfo ::Any Exception??" & Err.Description & "??" & "<<" & arrProcs(iEle))
                        '    MsgBox(ex.Message)
                        '    End Try
                        'sTempVar = arrProcs(iEle)
                        'Console.WriteLine(sTempVar)
                        'Console.Out.Flush()
                    Next
                    If bol_sql_begintrans = False Then
                        ConnSQL.Close()
                        ConnSQL = Nothing
                    End If
                End If
                UpdateInfo_On_Sp = True
            Catch ex As Exception
                Err.Raise(Err.Number, Err.Source, Err.Description)
                UpdateInfo_On_Sp = False
            End Try
        End Function
        Public Function ole_begin_trans() As Boolean
            Try
                OpenOLEConnection(mbprovider, mbUid, mbPass, mbDb, mbserver)
                OleTrans = ConnOLE.BeginTransaction(IsolationLevel.ReadCommitted)
                bol_Ole_begintrans = True
                Return True
            Catch ex As Exception
                Err.Raise(Err.Number, Err.Source, Err.Description)
                Return False
            End Try
        End Function
        Public Function ole_commit_trans() As Boolean
            Try
                OleTrans.Commit()
                bol_Ole_begintrans = False
                Return True
            Catch ex As Exception
                Err.Raise(Err.Number, Err.Source, Err.Description)
                Return False
            Finally
                ConnOLE.Close()
                ConnOLE = Nothing
            End Try
        End Function
        Public Function ole_rollback_trans() As Boolean
            Try
                OleTrans.Rollback()
                bol_Ole_begintrans = False
                Return True
            Catch ex As Exception
                Err.Raise(Err.Number, Err.Source, Err.Description)
                Return False
            Finally
                ConnOLE.Close()
                ConnOLE = Nothing
            End Try
        End Function

        Public Function Sql_begin_trans() As Boolean
            Try
                OpenSQLConnection(mbUid, mbPass, mbIniCatlog, mbDataSource)
                SqlTrans = ConnSQL.BeginTransaction(IsolationLevel.ReadUncommitted)
                bol_sql_begintrans = True
                Return True
            Catch ex As Exception
                Err.Raise(Err.Number, Err.Source, Err.Description)
                Return False
            End Try
        End Function
        Public Function Sql_commit_trans() As Boolean
            Try
                SqlTrans.Commit()
                bol_sql_begintrans = False
                Return True
            Catch ex As Exception
                Err.Raise(Err.Number, Err.Source, Err.Description)
                Return False
            Finally
                ConnSQL.Close()
                ConnSQL = Nothing
            End Try
        End Function
        Public Function Sql_rollback_trans() As Boolean
            Try
                SqlTrans.Rollback()
                bol_sql_begintrans = False
                Return True
            Catch ex As Exception
                Err.Raise(Err.Number, Err.Source, Err.Description)
                Return False
            Finally
                ConnSQL.Close()
                ConnSQL = Nothing
            End Try
        End Function
        Public Function ins_update_image(ByVal sqlstr As String, ByVal v_buff() As Byte) As Boolean

            Try
                If mbIsOLEConn = True Then
                    Dim oleComm As New OleDbCommand
                    If bol_Ole_begintrans = False Then
                        OpenOLEConnection(mbprovider, mbUid, mbPass, mbDb, mbserver)
                    Else
                        oleComm.Transaction = OleTrans
                    End If
                    oleComm.Connection = ConnOLE
                    oleComm.CommandType = CommandType.Text
                    oleComm.CommandText = sqlstr
                    oleComm.Parameters.AddWithValue("@Img", SqlDbType.Image).Value = v_buff
                    oleComm.ExecuteNonQuery()
                    If bol_sql_begintrans = False Then
                        ConnOLE.Close()
                        ConnOLE = Nothing
                    End If
                    Return True
                End If
            Catch ex As Exception
                Err.Raise(Err.Number, Err.Source, Err.Description)
            End Try
        End Function
        Public ReadOnly Property prop_IsSqlBeginTrans() As Boolean
            Get
                prop_IsSqlBeginTrans = bol_sql_begintrans
            End Get
        End Property
        Public ReadOnly Property prop_IsOleBeginTrans() As Boolean
            Get
                prop_IsOleBeginTrans = bol_sql_begintrans
            End Get
        End Property

        ''''added by grishma on 30 jul 2012'''''''''

        ''' <summary>
        ''' get data set as result of execution
        ''' </summary>
        ''' <param name="asProcName"></param>
        ''' <param name="dtResult"></param>
        ''' <param name="SqlParam"></param>
        ''' <param name="oleparam"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetDataSet_on_Sp(ByVal asProcName() As String, ByRef dtResult() As DataSet, Optional ByVal SqlParam() As SqlParameter = Nothing, Optional ByVal oleparam() As OleDbParameter = Nothing) As Boolean
            Try
                'Dim catCMD As OleDbCommand
                Dim stEle As Short
                Dim sqlEle As Short
                'Dim dtSet As DataSet
                Dim dtTable As DataSet

                'msREG_KEY = sREG_KEY
                ReDim dtResult(UBound(asProcName))
                If mbIsOLEConn = True Then
                    'Dim myReader As OleDbDataReader
                    Dim dtadp As New OleDbDataAdapter
                    Dim dtcmd As New OleDb.OleDbCommand
                    If ole_begin_trans() = False Then
                        OpenOLEConnection(mbprovider, mbUid, mbPass, mbDb, mbserver)
                    Else
                        dtcmd.Transaction = OleTrans
                    End If
                    For stEle = 0 To UBound(asProcName)
                        ' Create a Command object with the SQL statement.
                        dtcmd.Connection = ConnOLE
                        dtcmd.CommandText = asProcName(stEle)
                        dtcmd.CommandTimeout = 0
                        If IsNothing(oleparam) = False Then
                            For sqlEle = 0 To UBound(oleparam)
                                dtcmd.Parameters.Add(oleparam(sqlEle))
                            Next
                        End If

                        ' Fill a DataSet with data returned from the database.
                        dtTable = New DataSet
                        dtadp.SelectCommand = dtcmd
                        dtadp.Fill(dtTable)

                        ' Create a new DataTable object and assign to it
                        ' the new table in the Tables collection.
                        'dtTable = New DataTable
                        'dtTable = dtSet.Tables(0)
                        dtResult(stEle) = dtTable
                    Next
                    If ole_begin_trans() = False Then
                        ConnOLE.Close()
                    End If
                Else
                    Dim dtCmdSQL As New SqlCommand
                    Dim dtAdpSql As New SqlDataAdapter
                    If bol_sql_begintrans = False Then
                        OpenSQLConnection(mbUid, mbPass, mbIniCatlog, mbDataSource)
                    Else
                        dtCmdSQL.Transaction = SqlTrans
                    End If
                    For stEle = 0 To UBound(asProcName)
                        ' Create a Command object with the SQL statement.
                        'dtCmdSQL = New SqlDataAdapter(asProcName(stEle), ConnSQL)
                        dtCmdSQL.Connection = ConnSQL
                        dtCmdSQL.CommandText = asProcName(stEle)
                        dtCmdSQL.CommandType = CommandType.StoredProcedure
                        dtCmdSQL.CommandTimeout = 0

                        If IsNothing(SqlParam) = False Then
                            For sqlEle = 0 To UBound(SqlParam)
                                dtCmdSQL.Parameters.Add(SqlParam(sqlEle))
                            Next
                        End If

                        ' Fill a DataSet with data returned from the database.
                        'dtSet = New DataSet
                        dtTable = New DataSet
                        dtAdpSql.SelectCommand = dtCmdSQL
                        dtAdpSql.Fill(dtTable)

                        ' Create a new DataTable object and assign to it
                        ' the new table in the Tables collection.

                        'dtTable = New DataTable
                        'dtTable = dtTable.Tables(0)

                        dtResult(stEle) = dtTable
                    Next
                    If bol_sql_begintrans = False Then
                        ConnSQL.Close()
                    End If
                End If
                GetDataSet_on_Sp = True
            Catch ex As Exception
                MsgBox(ex.Message)
                Err.Raise(Err.Number, Err.Source, Err.Description)
            End Try
        End Function
        ''''''''''''''''''''''''''''''
    End Class
End Namespace