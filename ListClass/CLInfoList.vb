
Imports Csla
Imports Csla.Data
Imports System.Xml
Imports pbs.Helper
Imports System.Data.SqlClient

'Namespace EXT.VUS

<Serializable()> _
Public Class CLInfoList
    Inherits Csla.ReadOnlyListBase(Of CLInfoList, CLInfo)

#Region " Transfer and Report Function "

    Public Shared Function TransferOut(ByVal Code_From As String, ByVal Code_To As String, ByVal FileName As String) As Integer
        Dim _dt As New DataTable(GetType(CL).ToString)
        Dim oa As New ObjectAdapter()

        For Each info As CLInfo In CLInfoList.GetCLInfoList
            If info.Code >= Code_From AndAlso info.Code <= Code_To Then
                oa.Fill(_dt, CL.GetBO(info.ToString))
            End If
        Next
        Try
            _dt.Columns.Remove("IsNew")
            _dt.Columns.Remove("IsValid")
            _dt.Columns.Remove("IsSavable")
            _dt.Columns.Remove("IsDeleted")
            _dt.Columns.Remove("IsDirty")
            _dt.Columns.Remove("BrokenRulesCollection")
        Catch ex As Exception
        End Try

        For Each col As DataColumn In _dt.Columns
            col.ColumnMapping = MappingType.Attribute
        Next

        _dt.WriteXml(FileName)

        Return _dt.Rows.Count

    End Function

    Public Shared Function GetMyReportDataset() As List(Of DataTable)
        CLInfoList.InvalidateCache()

        Dim thelist = CLInfoList.GetCLInfoList.ToList()

        Dim shr = New pbs.BO.ObjectShredder(Of CLInfo)
        Dim dts As New List(Of DataTable)
        dts.Add(shr.Shred(thelist, Nothing, LoadOption.OverwriteChanges))

        Return dts
    End Function

#End Region

#Region " Business Properties and Methods "
    Private Shared _DTB As String = String.Empty
    Const _SUNTB As String = ""
    Private Shared _list As CLInfoList
#End Region 'Business Properties and Methods

#Region " Factory Methods "

    Private Sub New()
        _DTB = Context.CurrentBECode
    End Sub

    Public Shared Function GetCLInfo(ByVal pClassid As String) As CLInfo
        Dim Info As CLInfo = CLInfo.EmptyCLInfo(pClassid)
        ContainsCode(pClassid, Info)
        Return Info
    End Function

    Public Shared Function GetDescription(ByVal pClassid As String) As String
        Return GetCLInfo(pClassid).Description
    End Function

    Public Shared Function GetCLInfoList() As CLInfoList
        If _list Is Nothing Or _DTB <> Context.CurrentBECode Then

            _DTB = Context.CurrentBECode
            _list = DataPortal.Fetch(Of CLInfoList)(New FilterCriteria())

        End If
        Return _list
    End Function

    Public Shared Sub InvalidateCache()
        _list = Nothing
        _clDic = Nothing
    End Sub

    Public Shared Sub ResetCache()
        _list = Nothing
        _clDic = Nothing
    End Sub

    'Private Shared invalidateLock As New Object
    'Public Shared Sub InvalidateCache()
    '    SyncLock invalidateLock
    '        If Not SettingsProvider.SoftInvalidateCache Then
    '            ResetCache()
    '        Else
    '            Dim thelist = GetCLInfoList_Full()
    '            If thelist.Count > GetServerRecordCount() Then
    '                'someone delete some record on server. need to reload everything
    '                ResetCache()
    '            Else
    '                If thelist IsNot Nothing Then thelist.UpdatedInfoList()
    '            End If
    '        End If
    '    End SyncLock
    'End Sub

    Public Shared Function ContainsCode(ByVal pClassid As String, Optional ByRef RetInfo As CLInfo = Nothing) As Boolean

        RetInfo = CLInfo.EmptyCLInfo(pClassid)
        If GetCLDic.ContainsKey(pClassid) Then
            RetInfo = GetCLDic(pClassid)
            Return True
        End If

    End Function

    'Public Shared Function ContainsCode(ByVal Target As Object, ByVal e As Validation.RuleArgs) As Boolean
    '    Dim value As String = CType(CallByName(Target, e.PropertyName, CallType.Get), String)
    '    'no thing to check
    '    If String.IsNullOrEmpty(value) Then Return True

    '    If ContainsCode(value) Then
    '        Return True
    '    Else
    '        e.Description = String.Format(ResStr(Msg.NOSUCHITEM), ResStr("CL"), value)
    '        Return False
    '    End If
    'End Function

#End Region ' Factory Methods

#Region " Data Access "

#Region " Filter Criteria "

    <Serializable()> _
    Private Class FilterCriteria
        Friend _timeStamp() As Byte
        Public Sub New()
        End Sub
    End Class

#End Region
    Private Shared _lockObj As New Object

    Private Overloads Sub DataPortal_Fetch(ByVal criteria As FilterCriteria)
        SyncLock _lockObj
            RaiseListChangedEvents = False
            IsReadOnly = False
            Using cn = New SqlConnection(pbs.BO.EXT.VUS.Settings.GetSettings.GetConnectionString)
                cn.Open()

                Using cm = cn.CreateCommand()
                    cm.CommandType = CommandType.Text
                    'cm.CommandText = <SqlText>SELECT * FROM ListClass WHERE DTB='<%= _DTB %>'</SqlText>.Value.Trim
                    cm.CommandText = <SqlText>SELECT * FROM ListClass</SqlText>.Value.Trim

                    ' If criteria._timeStamp IsNot Nothing AndAlso criteria._timeStamp.Length > 0 Then
                    'cm.CommandText = <SqlText>SELECT * FROM ListClass WHERE DTB='<%= _DTB %>'</SqlText>.Value.Trim AND TIME_STAMP > @CurrentTimeStamp.Value.Trim
                    '     cm.Parameters.AddWithValue("@CurrentTimeStamp", criteria._timeStamp)
                    ' Else
                    '     cm.CommandText = <SqlText>SELECT * FROM ListClass WHERE DTB='<%= _DTB %>'</SqlText>.Value.Trim
                    ' End If

                    Using dr As New SafeDataReader(cm.ExecuteReader)
                        While dr.Read
                            Dim info = CLInfo.GetCLInfo(dr)
                            Me.Add(info)
                        End While
                    End Using

                End Using

                ' 'read the current version of the list
                ' Using cm As SqlCommand = cn.CreateSQLCommand()
                'cm.CommandText = SELECT max(TIME_STAMP) FROM ListClass WHERE DTB.Value.Tri
                '     Dim ret = cm.ExecuteScalar
                '     If ret IsNot Nothing Then _listTimeStamp = ret
                ' End Using

            End Using
            IsReadOnly = True
            RaiseListChangedEvents = True
        End SyncLock
    End Sub

#End Region ' Data Access                   
#Region "CL Dictionary"

    Private Shared _clDic As Dictionary(Of String, CLInfo)

    Private Shared Function GetCLDic() As Dictionary(Of String, CLInfo)
        If _clDic Is Nothing OrElse _DTB <> Context.CurrentBECode Then
            _clDic = New Dictionary(Of String, CLInfo)

            For Each itm In CLInfoList.GetCLInfoList
                _clDic.Add(itm.Code, itm)
            Next
        End If

        Return _clDic

    End Function

#End Region

#Region "TimeStamp"
    'Private _listTimeStamp() As Byte

    'Private Sub UpdatedInfoList()
    '    'get new updated notes by row stamp
    '    Dim newInfos = DataPortal.Fetch(Of PODInfoList)(New FilterCriteria() With {._timeStamp = _listTimeStamp})

    '    If newInfos.Count = 0 Then Exit Sub

    '    'merge new notes with the old one

    '    Dim oldPODs = GetPODInfoList_Full()
    '    Dim oldDic = New Dictionary(Of String, PODInfo)
    '    For Each itm In oldPODs
    '        oldDic.Add(itm.Code, itm)
    '    Next

    '    oldPODs.IsReadOnly = False
    '    oldPODs.RaiseListChangedEvents = False

    '    For Each info In newInfos
    '        If oldDic.ContainsKey(info.Code) Then

    '            Dim oldNote = oldDic(info.Code)
    '            oldPODs.Remove(oldNote)
    '            oldPODs.Add(info)

    '            oldDic(info.Code) = info
    '        Else
    '            oldPODs.Add(info)

    '            oldDic.Add(info.Code, info)
    '        End If
    '    Next


    '    oldPODs.IsReadOnly = False
    '    oldPODs.RaiseListChangedEvents = False

    '    _podDic = oldDic
    '    _list = oldPODs
    '    _list._listTimeStamp = newInfos._listTimeStamp

    'End Sub

    'Private Shared Function GetServerRecordCount() As Integer
    '    Dim script = SELECT count(*) FROM ListClass  _DTB .Value.Tri
    '    Return SQLCommander.GetScalarInteger(script)
    'End Function
#End Region

End Class

'End Namespace