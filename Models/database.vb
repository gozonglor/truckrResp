Imports System.IO
Imports System.Data.SqlClient
Imports System.Transactions
Imports System.Linq

Public Class database
    ' Inherits System.Web.UI.Page
    Dim email As New sendemail
    Public oDR As System.Data.SqlClient.SqlDataReader
    Private trans As System.Transactions.TransactionScope
    Public ds As DataSet = New DataSet()
    Private dtbl As DataTable = New DataTable()
    Private submitted As String

    Public Sub New()
        'submitted = System.DateTime.Now.ToString()
    End Sub

    Public Property poststatus()
        Get
            Return submitted
        End Get

        Set(ByVal Value)
            'Called when someone tries to assign a value     
            submitted = Value
        End Set
    End Property

    'Public Function setReader2(ByVal sql As String) As String
    '    Try
    '        'calling method should have a using block on oDR, when oDR is closed so is the underlying connection
    '        Dim oConn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("activedatabase"))
    '        Using oCom As New SqlCommand(sql, oConn)
    '            oConn.Open()
    '            oDR = oCom.ExecuteReader(CommandBehavior.CloseConnection)
    '            Return ""
    '        End Using
    '    Catch ex As Exception
    '        email.senderror("csorensen@pcsforpeople.com", "Website Error Has Occured", "The error: " + ex.Message + "<br /><br /> The SQL: " + sql.ToString + "<br /><br /> Date and Time: " + DateTime.Now.ToString)
    '        oDR = Nothing
    '        Return ex.Message
    '    End Try
    'End Function

    Public Function setReader(ByVal sql As String, Optional param As SqlParameter() = Nothing) As String
        Try
            'calling method should have a using block on oDR, when oDR is closed so is the underlying connection
            Dim oConn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("activedatabase"))
            Using oCom As New SqlCommand(sql, oConn) ''opening the command, sending in the sql command/our filter string into 'body' of db. using-end using disposes object after use.
                oConn.Open() ''opening sql connection

                If param IsNot Nothing Then ''if optional parameter is not empty
                    oCom.Parameters.AddRange(param) ''add the opt param to the command 
                End If

                oDR = oCom.ExecuteReader(CommandBehavior.CloseConnection) ''read the command?
                Return ""
            End Using
        Catch ex As Exception
            email.senderror("glor@macalester.edu", "Website Error Has Occured", "The error: " + ex.Message + "<br /><br /> The SQL: " + sql.ToString + "<br /><br /> Date and Time: " + DateTime.Now.ToString)
            oDR = Nothing
            Return ex.Message
        End Try
    End Function

    'Public Function insert2(ByVal sql As String, Optional param As SqlParameter() = Nothing) As Boolean
    '    Try
    '        System.Data.SqlClient.SqlConnection.ClearAllPools()
    '        Using con As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("activedatabase"))
    '            Using cmd As New SqlCommand(sql, con)
    '                con.Open()

    '                If param IsNot Nothing Then
    '                    'paramaters are set in this format {'@param','value'}
    '                    'For Each item As SqlParameter In param
    '                    'If TypeOf item.Value Is String Then
    '                    'item.Value.Trim()
    '                    cmd.Parameters.AddRange(param)
    '                    ' Else
    '                    'oCom.Parameters.Add(item)
    '                    'End If
    '                    ' Next
    '                End If

    '                'execute the insert statement
    '                cmd.ExecuteNonQuery()
    '                con.Close()
    '                Return True
    '            End Using
    '        End Using


    'Private Sub MessageBox(ByVal strMsg As String)
    '    Dim lbl As New Label
    '    lbl.Text = "<script language='javascript'>" & Environment.NewLine _
    '               & "window.alert(" & "'" & strMsg & "'" & ")</script>"
    '    Page.Controls.Add(lbl)
    'End Sub

    Public Function getReader() As System.Data.SqlClient.SqlDataReader
        Return oDR
    End Function

    Public Sub setdt()
        Try
            dtbl.Load(oDR)
        Catch ex As Exception
            email.senderror("glor@macalester.edu", "Website Error Has Occured", "Error Page: database.vb" + "<br /><br /> The error: " + ex.Message + "<br /><br /> The locatin: setdt" + "<br /><br /> Date and Time: " + DateTime.Now.ToString)
        End Try
    End Sub

    Public Overloads Function Filterdt(FilterExpression As String, ByVal dt As DataTable) As DataTable
        Using Dv As New DataView(dt)
            Dv.RowFilter = FilterExpression
            Return Dv.ToTable
        End Using
    End Function

    Public Overloads Function Filterdt(FilterExpression As String) As DataTable
        Using Dv As New DataView(dtbl)
            Dv.RowFilter = FilterExpression
            Return Dv.ToTable
        End Using
    End Function

    Public Function getdt() As DataTable
        Return dtbl
    End Function

    Public Function getnesteddt(ByVal tblName As String) As System.Data.DataTable
        Return ds.Tables(tblName)
    End Function

    Public Overloads Function setrelation(ByVal SQL1 As String, ByVal SQL2 As String, ByVal srcTbl1 As String, ByVal srcTbl2 As String, ByVal joincolumn As String, ByVal relation As String, Optional param1 As SqlParameter() = Nothing, Optional param2 As SqlParameter() = Nothing) As Boolean
        Try
            Using con As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("activedatabase"))
                Using cmd1 As New SqlDataAdapter(SQL1, con)
                    Using cmd2 As New SqlDataAdapter(SQL2, con)
                        If param1 IsNot Nothing Then
                            cmd1.SelectCommand.Parameters.AddRange(param1)
                        End If
                        If param2 IsNot Nothing Then
                            cmd2.SelectCommand.Parameters.AddRange(param2)
                        End If

                        cmd1.Fill(ds, srcTbl1)
                        cmd2.Fill(ds, srcTbl2)
                        ds.Relations.Add(relation, ds.Tables(srcTbl1).Columns(joincolumn), ds.Tables(srcTbl2).Columns(joincolumn), False)
                        con.Close()
                        Return True
                    End Using
                End Using
            End Using
        Catch ex As Exception
            email.senderror("glor@macalester.edu", "Website Error Has Occured", "Error Page: database.vb" + "<br /><br /> The error: " + ex.Message + "<br /><br /> The SQL: " + SQL1.ToString + "<br /><br /> The SQL2: " + SQL2.ToString + "<br /><br /> Date and Time: " + DateTime.Now.ToString)
            Return False
        End Try
    End Function


    Public Overloads Function setrelation(ByVal SQL1 As String, ByVal SQL2 As String, ByVal srcTbl1 As String, ByVal srcTbl2 As String, ByVal joincolumn As String, ByVal relation As String, ByRef parentfilter As String, ByVal childfilter As String, Optional param1 As SqlParameter() = Nothing, Optional param2 As SqlParameter() = Nothing) As Boolean
        Try
            Using con As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("activedatabase"))
                Using cmd1 As New SqlDataAdapter(SQL1, con)
                    Using cmd2 As New SqlDataAdapter(SQL2, con)

                        If param1 IsNot Nothing Then
                            cmd1.SelectCommand.Parameters.AddRange(param1)
                        End If
                        If param2 IsNot Nothing Then
                            cmd2.SelectCommand.Parameters.AddRange(param2)
                        End If

                        cmd1.Fill(ds, srcTbl1)
                        cmd2.Fill(ds, srcTbl2)

                        If childfilter <> "" Then
                            ds.Tables(srcTbl2).DefaultView.RowFilter = childfilter
                        End If

                        Dim filterid As String = ""
                        For Each row As DataRow In ds.Tables(srcTbl2).DefaultView.ToTable.Rows

                            If filterid = "" Then
                                filterid += "(" + row.Item(joincolumn).ToString
                            Else
                                filterid += ", " + row.Item(joincolumn).ToString
                            End If
                        Next
                        filterid += ")"

                        If filterid <> ")" Then
                            If parentfilter <> "" Then
                                ds.Tables(srcTbl1).DefaultView.RowFilter = parentfilter + " and contactID in " + filterid
                            Else
                                ds.Tables(srcTbl1).DefaultView.RowFilter = "contactID in " + filterid
                            End If
                        Else
                            'no relation is found to filter so clear all data
                            ds.Tables(srcTbl1).Rows.Clear()
                            ds.Tables(srcTbl2).Rows.Clear()
                        End If

                        Dim t As DataTable = (ds.Tables(srcTbl1).DefaultView.ToTable)
                        Dim t2 As DataTable = ds.Tables(srcTbl2)

                        ds.Tables.Remove(srcTbl1)
                        ds.Tables.Remove(srcTbl2)
                        ds.Tables.Add(t)
                        ds.Tables.Add(t2)

                        ds.Relations.Add(relation, t.Columns(joincolumn), t2.Columns(joincolumn), False)

                        con.Close()
                        Return True
                    End Using
                End Using
            End Using
        Catch ex As Exception
            email.senderror("glor@macalester.edu", "Website Error Has Occured", "Error Page: database.vb" + "<br /><br /> The error: " + ex.Message + "<br /><br /> The SQL: " + SQL1.ToString + "<br /><br /> The SQL2: " + SQL2.ToString + "<br /><br /> Date and Time: " + DateTime.Now.ToString)
            Return False
        End Try
    End Function

    Public Function insert(ByVal sql As String, Optional param As String(,) = Nothing, Optional data As Byte() = Nothing) As Boolean
        Try
            System.Data.SqlClient.SqlConnection.ClearAllPools()
            Using con As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("activedatabase"))
                Using cmd As New SqlCommand(sql, con)
                    con.Open()

                    If param IsNot Nothing Then
                        Dim x As Integer = 0
                        Dim i As Integer
                        'paramaters are set in this format {'@param','value'}
                        For i = param.GetLowerBound(0) To param.GetUpperBound(0)
                            If param(i, x + 1).Trim = "" Then
                                cmd.Parameters.AddWithValue(param(i, x), DBNull.Value)
                            Else
                                cmd.Parameters.AddWithValue(param(i, x), param(i, x + 1))
                            End If

                        Next
                    End If

                    If Not data Is Nothing Then
                        cmd.Parameters.Add("@Data", SqlDbType.VarBinary, -1).Value = data
                        'oCom.Parameters.Add(New SqlParameter("@Data", SqlDbType.VarBinary, data.Length, ParameterDirection.Input, False, 0, 0, "Data", DataRowVersion.Current, data))
                    End If

                    'execute the insert statement
                    cmd.ExecuteNonQuery()
                    con.Close()
                    Return True
                End Using
            End Using
        Catch ex As Exception
            email.senderror("glor@macalester.edu", "Website Error Has Occured", "Error Page: database.vb" + "<br /><br /> The error: " + ex.Message + "<br /><br /> The SQL: " + sql.ToString + "<br /><br /> Date and Time: " + DateTime.Now.ToString)
            Return False
        End Try
    End Function


    Public Function insert2(ByVal sql As String, Optional param As SqlParameter() = Nothing) As Boolean
        Try
            System.Data.SqlClient.SqlConnection.ClearAllPools()
            Using con As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("activedatabase"))
                Using cmd As New SqlCommand(sql, con)
                    con.Open()

                    If param IsNot Nothing Then
                        'paramaters are set in this format {'@param','value'}
                        'For Each item As SqlParameter In param
                        'If TypeOf item.Value Is String Then
                        'item.Value.Trim()
                        cmd.Parameters.AddRange(param)
                        ' Else
                        'oCom.Parameters.Add(item)
                        'End If
                        ' Next
                    End If

                    'execute the insert statement
                    cmd.ExecuteNonQuery()
                    con.Close()
                    Return True
                End Using
            End Using
        Catch ex As Exception
            email.senderror("glor@macalester.edu", "Website Error Has Occured", "Error Page: database.vb" + "<br /><br /> The error: " + ex.Message + "<br /><br /> The SQL: " + sql.ToString + "<br /><br /> Date and Time: " + DateTime.Now.ToString)
            Return False
        End Try
    End Function

    Public Function insertmax2(ByVal sql As String, Optional param As SqlParameter() = Nothing) As String
        Try
            System.Data.SqlClient.SqlConnection.ClearAllPools()
            Using con As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("activedatabase"))
                Using cmd As New SqlCommand(sql, con)
                    con.Open()

                    If param IsNot Nothing Then
                        cmd.Parameters.AddRange(param)
                    End If

                    Dim theID As String = cmd.ExecuteScalar()
                    con.Close()
                    Return theID
                End Using
            End Using
        Catch ex As Exception
            email.senderror("glor@macalester.edu", "Website Error Has Occured", "Error Page: database.vb" + "<br /><br /> The error: " + ex.Message + "<br /><br /> The SQL: " + sql.ToString + "<br /><br /> Date and Time: " + DateTime.Now.ToString)
            Return ""
        End Try

    End Function

    Public Function insertmax(ByVal sql As String, Optional param As String(,) = Nothing, Optional data As Byte() = Nothing) As String
        Try
            System.Data.SqlClient.SqlConnection.ClearAllPools()
            Using con As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("activedatabase"))
                Using cmd As New SqlCommand(sql, con)
                    con.Open()

                    If param IsNot Nothing Then
                        Dim x As Integer = 0
                        Dim i As Integer
                        'paramaters are set in this format {'@param','value'}
                        For i = param.GetLowerBound(0) To param.GetUpperBound(0)
                            If String.IsNullOrEmpty(param(i, x + 1)) Then
                                'insert null
                                cmd.Parameters.AddWithValue(param(i, x), DBNull.Value)
                            Else
                                cmd.Parameters.AddWithValue(param(i, x), param(i, x + 1))
                            End If

                        Next
                    End If

                    If Not data Is Nothing Then
                        cmd.Parameters.AddWithValue("@Data", data)
                        'oCom.Parameters.Add("@Data", SqlDbType.VarBinary, -1).Value = data
                    End If

                    'oCom.ExecuteNonQuery()
                    Dim theID As String = cmd.ExecuteScalar()

                    'oCom.CommandText = "SELECT SCOPE_IDENTITY()"
                    'Dim myReader As System.Data.SqlClient.SqlDataReader = oCom.ExecuteReader()
                    'myReader.Read()

                    'If myReader.HasRows = False Then
                    '    'If myReader.Item(0).Equals(System.DBNull.Value) Then
                    '    Return ("")
                    'Else
                    '    Return (myReader.Item(0).ToString())
                    'End If
                    con.Close()
                    Return theID
                End Using
            End Using
        Catch ex As Exception
            email.senderror("glor@macalester.edu", "Website Error Has Occured", "Error Page: database.vb" + "<br /><br /> The error: " + ex.Message + "<br /><br /> The SQL: " + sql.ToString + "<br /><br /> Date and Time: " + DateTime.Now.ToString)
            Return ""
        End Try

    End Function

    Public Function queryGetValue(ByVal sql As String, Optional param As String(,) = Nothing) As String
        Try
            System.Data.SqlClient.SqlConnection.ClearAllPools()
            Using con As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("activedatabase"))
                Using cmd As New SqlCommand(sql, con)
                    con.Open()

                    If param IsNot Nothing Then
                        Dim x As Integer = 0
                        Dim i As Integer
                        'paramaters are set in this format {'@param','value'}
                        For i = param.GetLowerBound(0) To param.GetUpperBound(0)
                            cmd.Parameters.AddWithValue(param(i, x), param(i, x + 1))
                        Next
                    End If

                    Using myReader As System.Data.SqlClient.SqlDataReader = cmd.ExecuteReader()
                        myReader.Read()

                        If myReader.HasRows = False Then
                            Return ("")
                        Else
                            Return (myReader.Item(0).ToString())
                        End If
                    End Using

                End Using
            End Using
        Catch ex As Exception
            email.senderror("glor@macalester.edu", "Website Error Has Occured", "Error Page: database.vb" + "<br /><br /> The error: " + ex.Message + "<br /><br /> The SQL: " + sql.ToString + "<br /><br /> Date and Time: " + DateTime.Now.ToString)
            Return ""
        End Try
    End Function

    Public Function queryGetValue2(ByVal sql As String, Optional param As SqlParameter() = Nothing) As String
        Try
            System.Data.SqlClient.SqlConnection.ClearAllPools()
            Using con As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("activedatabase"))
                Using cmd As New SqlCommand(sql, con)
                    con.Open()

                    If param IsNot Nothing Then
                        cmd.Parameters.AddRange(param)
                    End If

                    Using myReader As System.Data.SqlClient.SqlDataReader = cmd.ExecuteReader()
                        myReader.Read()

                        If myReader.HasRows = False Then
                            Return ("")
                        Else
                            Return (myReader.Item(0).ToString())
                        End If
                    End Using

                End Using
            End Using
        Catch ex As Exception
            email.senderror("glor@macalester.edu", "Website Error Has Occured", "Error Page: database.vb" + "<br /><br /> The error: " + ex.Message + "<br /><br /> The SQL: " + sql.ToString + "<br /><br /> Date and Time: " + DateTime.Now.ToString)
            Throw ex
        End Try
    End Function

    Public Function getData(ByVal sql As String, Optional param As String(,) = Nothing) As Byte()
        Try
            System.Data.SqlClient.SqlConnection.ClearAllPools()
            Using con As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("activedatabase"))
                Using cmd As New SqlCommand(sql, con)
                    con.Open()

                    If param IsNot Nothing Then
                        Dim x As Integer = 0
                        Dim i As Integer
                        'paramaters are set in this format {'@param','value'}
                        For i = param.GetLowerBound(0) To param.GetUpperBound(0)
                            cmd.Parameters.AddWithValue(param(i, x), param(i, x + 1))
                        Next
                    End If

                    Dim imageData As Byte() = DirectCast(cmd.ExecuteScalar(), Byte())

                    If Not imageData Is Nothing Then
                        Return imageData
                    Else
                        Return Nothing
                    End If
                End Using
            End Using
        Catch ex As Exception
            email.senderror("glor@macalester.edu", "Website Error Has Occured", "Error Page: database.vb" + "<br /><br /> The error: " + ex.Message + "<br /><br /> The SQL: " + sql.ToString + "<br /><br /> Date and Time: " + DateTime.Now.ToString)
            Return Nothing
        End Try
    End Function


    Public Function bulkinsert(ByRef dt As DataTable, ByVal destTable As String, sourcecolumn() As Integer, destcolumn() As Integer) As Boolean
        Try
            Using cn As New SqlConnection(System.Configuration.ConfigurationManager.AppSettings("activedatabase"))
                cn.Open()
                Using copy As New SqlBulkCopy(cn)
                    'copy.ColumnMappings.Add(0, 0)
                    'copy.ColumnMappings.Add(1, 1)
                    'copy.ColumnMappings.Add(2, 2)
                    'copy.ColumnMappings.Add(3, 3)
                    'copy.ColumnMappings.Add(4, 4)
                    For i As Integer = 0 To sourcecolumn.Length - 1
                        copy.ColumnMappings.Add(sourcecolumn(i), destcolumn(i))
                    Next
                    copy.DestinationTableName = destTable
                    copy.WriteToServer(dt)
                End Using
                Return True
            End Using
        Catch ex As Exception
            email.senderror("glor@macalester.edu", "Website Error Has Occured", "Error Page: database.vb" + "<br /><br /> The error: " + ex.Message + "<br /><br /> The dest table: " + destTable.ToString + "<br /><br /> Date and Time: " + DateTime.Now.ToString)
            Return False
        End Try
    End Function

    Public Overloads Function getFilteredTable(ByVal SQL As String, ByVal relationname As String, ByVal filter As String, Optional param As SqlParameter() = Nothing) As DataTable
        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.AppSettings("activedatabase")
            'Dim con As New SqlConnection(strConnection)

            Using con As New SqlConnection(strConnection)
                Using cmd As New SqlCommand(SQL, con)
                    If param IsNot Nothing Then
                        cmd.Parameters.AddRange(param)
                    End If
                    Using da As New SqlDataAdapter(cmd)
                        Dim ds As New DataSet()
                        da.Fill(ds, relationname)
                        ds.Tables(relationname).DefaultView.RowFilter() = filter
                        Return ds.Tables(relationname).DefaultView.ToTable()
                    End Using
                End Using
            End Using
        Catch ex As Exception
            email.senderror("glor@macalester.edu", "Website Error Has Occured", "Error Page: database.vb" + "<br /><br /> The error: " + ex.Message + "<br /><br /> The SQL: " + SQL.ToString + "<br /><br /> Date and Time: " + DateTime.Now.ToString)
            Return ds.Tables(relationname).DefaultView.ToTable
        End Try
    End Function

    Public Overloads Function getFilteredTable(ByVal SQL As String, ByVal relationname As String, ByVal filter As String, ByVal removeRows As String(), groupByField As String, Optional param As SqlParameter() = Nothing) As DataTable
        Try
            Dim strConnection As String = System.Configuration.ConfigurationManager.AppSettings("activedatabase")
            'Dim con As New SqlConnection(strConnection)

            Using con As New SqlConnection(strConnection)
                Using cmd As New SqlCommand(SQL, con)
                    If param IsNot Nothing Then
                        cmd.Parameters.AddRange(param)
                    End If
                    Using da As New SqlDataAdapter(cmd)
                        Dim ds As New DataSet()
                        da.Fill(ds, relationname)
                        ds.Tables(relationname).DefaultView.RowFilter() = filter

                        Dim dt As DataTable = ds.Tables(relationname).DefaultView.ToTable

                        For Each s As String In removeRows
                            dt.Columns.Remove(s)
                        Next
                        'dt.Columns.Remove("macaddy")
                        'dt.Columns.Remove("pid")
                        'ds.Tables(relationname).AsEnumerable().Select(Function(m) New From {m.CategoryId, m.CategoryName}).Distinct().ToList()
                        'ds.Tables(relationname).AsEnumerable().Select(Function(r) New from (r.clientID)
                        'distinctRows = 
                        'Dim asd As DataRowComparer
                        ' Dim DistinctData = (From dr In dt.AsEnumerable()).Distinct(New DataRowComparer(dr)).ToList()
                        'Dim qryUser = From db In dt.AsEnumerable() _
                        'Distinct()
                        'Dim names = (From row In dt.AsEnumerable() Select row Distinct).CopyToDataTable
                        'Throw New Exception("test")
                        If dt.Rows.Count = 0 Then
                            Return New DataTable

                            'Dim x = (From r In dt.AsEnumerable() Select r).Distinct(DataRowComparer.Default).ToList()
                            'Dim dt1 = dt.AsEnumerable().Distinct(DataRowComparer.Default).CopyToDataTable
                            'Dim dt2 = (From row In dt.AsEnumerable() Select row Distinct).CopyToDataTable
                            'Dim dt3 = (From row In dt.AsEnumerable() Select row).Distinct(DataRowComparer.Default).ToList
                            'Dim dt4 = (From row In dt.AsEnumerable() Select row).Distinct(DataRowComparer.Default).CopyToDataTable
                            'Dim dt5 = (From row In dt.AsEnumerable() Select row).Distinct(DataRowComparer.Default)
                        End If

                        'Dim dt123 As DataTable = dt.AsEnumerable().GroupBy(Function(r) r.Field(Of Integer)("clientID")).[Select](Function(g) g.First()).CopyToDataTable

                        'If returnfirstOccuranceOnly Then
                        Return dt.AsEnumerable().GroupBy(Function(r) r.Field(Of Object)(groupByField)).[Select](Function(g) g.First()).CopyToDataTable
                        'Else
                        '    Return dt.AsEnumerable().GroupBy(Function(r) r.Field(Of Object)(groupByField)).SelectMany(Function(g) g.OrderBy(Function(r) r.Field(Of Date)(OrderByField))).CopyToDataTable()
                        '    'Return dt.AsEnumerable().GroupBy(Function(r) r.Field(Of Object)(groupByField)).SelectMany(Function(h) h.OrderByDescending(Function(r) r.Field(Of Date)(OrderByField))).CopyToDataTable()
                        '    'Return dt.AsEnumerable().GroupBy(Function(r) r.Field(Of Object)(groupByField)).SelectMany(Function(g) g.Distinct).CopyToDataTable()
                        'End If

                        'Return dt.AsEnumerable().GroupBy(Function(r) r.Field(Of Object)("clientID")).[Select](Function(g) g.First()).CopyToDataTable
                        'Dim test = dt.AsEnumerable().GroupBy(Function(r) r.Field(Of Integer)("clientID")).Where(Function(g) g.Count() = 1).[Select](Function(g) g.First()).CopyToDataTable
                        '(From row In dt.AsEnumerable() Select row).Distinct(DataRowComparer.Default).ToList 
                        '(From row In dt.AsEnumerable() Select row Distinct).CopyToDataTable 'dt.AsEnumerable().Distinct().CopyToDataTable 'dt.DefaultView.ToTable(distinct, distinctRows) '(From dt In dt Select row Distinct) 'ds.Tables(relationname).DefaultView.ToTable(distinct, distinctRows)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            email.senderror("glor@macalester.edu", "Website Error Has Occured", "Error Page: database.vb" + "<br /><br /> The error: " + ex.Message + "<br /><br /> The SQL: " + SQL.ToString + "<br /><br /> Date and Time: " + DateTime.Now.ToString)
            Return New DataTable 'ds.Tables(relationname).DefaultView.ToTable
        End Try
    End Function


    Public Function CreateTransactionScope(ByVal runAsync As Boolean) As TransactionScope
        Dim options As New Transactions.TransactionOptions
        options.IsolationLevel = Transactions.IsolationLevel.ReadCommitted
        options.Timeout = TransactionManager.MaximumTimeout
        If runAsync Then
            trans = New System.Transactions.TransactionScope(Transactions.TransactionScopeOption.Required, options, TransactionScopeAsyncFlowOption.Enabled)
        Else
            trans = New System.Transactions.TransactionScope(Transactions.TransactionScopeOption.Required, options)
        End If


        Return trans
    End Function

    Public Function CleanSQLValue(ByVal SQL As String) As String
        SQL = Replace(SQL, "'", "''")
        SQL = Replace(SQL, ";", "")
        SQL = Replace(SQL, "*", "")
        SQL = Replace(SQL, "/", "-")
        SQL = Replace(SQL, "--", "-")
        SQL = Replace(SQL, "xp_", "xp")
        If SQL <> "" Then
            SQL = SQL.Trim()
        End If
        Return SQL
    End Function

End Class
